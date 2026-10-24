using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Infrastructure;

public static class InitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ContextInitializer>();
        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }
}

public class ContextInitializer
{
    private readonly Context _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<ContextInitializer> _logger;

    public ContextInitializer(
        Context context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<ContextInitializer> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try { await TrySeedAsync(); }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        await SeedRolesAsync();
        var superAdmin = await SeedSuperAdminAsync();
        await SeedAdminAsync();
        await SeedSecretaryAsync();
        var professors = await SeedProfessorsAsync(superAdmin);
        var techMeans = await SeedTechnologicalMeansAsync();
        var auxMeans = await SeedAuxiliaryMeansAsync();
        var classrooms = await SeedClassRoomsAsync();
        var subjects = await SeedSubjectsAsync(classrooms);
        var courses = await SeedCoursesAsync();
        var students = await SeedStudentsAsync(courses);

        await SeedClassRoomTechMeansAsync(classrooms, techMeans);
        await SeedProfessorSubjectsAsync(professors, subjects);
        await SeedStudentSubjectsAsync(students, subjects);
        await SeedSubjectAuxMeansAsync(subjects, auxMeans);
        await SeedProfessorStudentSubjectsAsync(professors, students, subjects);

        await _context.SaveChangesAsync();
    }

    // ---------- Seed helpers ----------

    private async Task SeedRolesAsync()
    {
        foreach (var role in new[] { Roles.SuperAdmin, Roles.Admin, Roles.Secretary, Roles.Professor, Roles.Student })
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private async Task<User> SeedSuperAdminAsync()
    {
        const string userName = "SuperAdmin";
        var existing = await _userManager.FindByNameAsync(userName);
        if (existing is not null) return existing;

        var user = new User { UserName = userName, Email = "superadmin@localhost" };
        await _userManager.CreateAsync(user, "Superadminpassword1*");
        await _userManager.AddToRoleAsync(user, Roles.SuperAdmin);

        var professor = new Professor
        {
            UserId = user.Id,
            NameProf = "Pepe Gonzalez",
            Contract = "Tiempo Completo",
            Spec = "spec0",
            Salary = 10000,
            LaboralExperience = 15,
            IsDean = true
        };
        _context.Professors.Add(professor);
        await _context.SaveChangesAsync();
        return user;
    }

    private async Task SeedAdminAsync()
    {
        if (await _context.Administrators.AnyAsync()) return;

        var user = new User { UserName = "Admin", Email = "admin@localhost" };
        await _userManager.CreateAsync(user, "Adminpassword1*");
        await _userManager.AddToRoleAsync(user, Roles.Admin);

        _context.Administrators.Add(new Administrator
        {
            UserId = user.Id,
            AdminName = "PedroAdmin",
            AdminSalary = 1500
        });
        await _context.SaveChangesAsync();
    }

    private async Task SeedSecretaryAsync()
    {
        if (await _context.Secretaries.AnyAsync()) return;

        var user = new User { UserName = "Secretary", Email = "secretary@localhost" };
        await _userManager.CreateAsync(user, "Secretarypassword1*");
        await _userManager.AddToRoleAsync(user, Roles.Secretary);

        _context.Secretaries.Add(new Secretary
        {
            UserId = user.Id,
            NameS = "Armando",
            SalaryS = 1500
        });
        await _context.SaveChangesAsync();
    }

    private async Task<List<Professor>> SeedProfessorsAsync(User superAdmin)
    {
        var list = await _context.Professors.ToListAsync();
        if (list.Count >= 10) return list;

        for (int i = 1; i < 10; i++)
        {
            var user = new User { UserName = "pepe" + i };
            await _userManager.CreateAsync(user, "Pepe0password1*");
            await _userManager.AddToRoleAsync(user, Roles.Professor);

            list.Add(new Professor
            {
                UserId = user.Id,
                NameProf = "Pepe" + i + " Gonzalez",
                Contract = "Tiempo Completo",
                Spec = "spec" + i,
                Salary = 10000,
                LaboralExperience = 5
            });
        }
        _context.Professors.AddRange(list.Where(p => p.IdProf == 0));
        await _context.SaveChangesAsync();
        return await _context.Professors.ToListAsync();
    }

    private async Task<List<TechnologicalMeans>> SeedTechnologicalMeansAsync()
    {
        if (await _context.TechnologicalMeans.AnyAsync())
            return await _context.TechnologicalMeans.ToListAsync();

        var list = new List<TechnologicalMeans>();
        for (int i = 0; i < 5; i++)
        {
            var mean = new TechnologicalMeans { NameMean = "medio" + i, State = "ok" };
            for (int j = 0; j < 10; j++)
                list.Add(new TechnologicalMeans { NameMean = mean.NameMean, State = mean.State });
        }
        _context.TechnologicalMeans.AddRange(list);
        await _context.SaveChangesAsync();
        return list;
    }

    private async Task<List<AuxiliaryMeans>> SeedAuxiliaryMeansAsync()
    {
        if (await _context.AuxiliaryMeans.AnyAsync())
            return await _context.AuxiliaryMeans.ToListAsync();

        var list = new List<AuxiliaryMeans>();
        for (int i = 0; i < 5; i++)
        {
            var mean = new AuxiliaryMeans { NameMean = "auxmedio" + i, State = "ok", Type = "Pizarra" };
            for (int j = 0; j < 10; j++)
                list.Add(new AuxiliaryMeans { NameMean = mean.NameMean, State = mean.State, Type = mean.Type });
        }
        _context.AuxiliaryMeans.AddRange(list);
        await _context.SaveChangesAsync();
        return list;
    }

    private async Task<List<ClassRoom>> SeedClassRoomsAsync()
    {
        if (await _context.ClassRooms.AnyAsync())
            return await _context.ClassRooms.ToListAsync();

        var list = Enumerable.Range(0, 5)
            .Select(i => new ClassRoom { Location = i.ToString() })
            .ToList();
        _context.ClassRooms.AddRange(list);
        await _context.SaveChangesAsync();
        return list;
    }

    private async Task<List<Subject>> SeedSubjectsAsync(List<ClassRoom> classrooms)
    {
        if (await _context.Subjects.AnyAsync())
            return await _context.Subjects.ToListAsync();

        var list = Enumerable.Range(0, 5).Select(i => new Subject
        {
            NameSub = "subject" + i,
            StudyProgram = "studyprogram1",
            CourseLoad = 10,
            IdClassRoom = classrooms[i].IdClassR,
            ClassRoom = classrooms[i]
        }).ToList();
        _context.Subjects.AddRange(list);
        await _context.SaveChangesAsync();
        return list;
    }

    private async Task<List<Course>> SeedCoursesAsync()
    {
        if (await _context.Courses.AnyAsync())
            return await _context.Courses.ToListAsync();

        var list = new List<Course>
        {
            new() { CourseName = "2023-2024" },
            new() { CourseName = "2024-2025" }
        };
        _context.Courses.AddRange(list);
        await _context.SaveChangesAsync();
        return list;
    }

    private async Task<List<Student>> SeedStudentsAsync(List<Course> courses)
    {
        if (await _context.Students.AnyAsync())
            return await _context.Students.ToListAsync();

        const int total = 50;
        var list = new List<Student>();
        for (int i = 0; i < total; i++)
        {
            var user = new User { UserName = "pedro" + i };
            if (await _userManager.FindByNameAsync(user.UserName) is not null) continue;

            await _userManager.CreateAsync(user, "Pedropassword1*");
            await _userManager.AddToRoleAsync(user, Roles.Student);

            list.Add(new Student
            {
                UserId = user.Id,
                NameStud = "Pedro" + i + " Gonzalez",
                Age = 15,
                EActivity = true,
                Course = courses[i % courses.Count]
            });
        }
        _context.Students.AddRange(list);
        await _context.SaveChangesAsync();
        return list;
    }

    private async Task SeedClassRoomTechMeansAsync(List<ClassRoom> classrooms, List<TechnologicalMeans> means)
    {
        if (await _context.ClassRoomTechMeans.AnyAsync()) return;

        var count = Math.Min(classrooms.Count, means.Count);
        for (int i = 0; i < count; i++)
        {
            _context.ClassRoomTechMeans.Add(new ClassRoomTechMean
            {
                IdClassRoom = classrooms[i].IdClassR,
                IdTechMean = means[i].IdMean
            });
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedProfessorSubjectsAsync(List<Professor> professors, List<Subject> subjects)
    {
        if (await _context.ProfessorSubjects.AnyAsync()) return;

        for (int i = 0; i < subjects.Count && 2 * i + 1 < professors.Count; i++)
        {
            _context.ProfessorSubjects.Add(new ProfessorSubject
            {
                IdProf = professors[2 * i].IdProf,
                IdSub = subjects[i].IdSub
            });
            _context.ProfessorSubjects.Add(new ProfessorSubject
            {
                IdProf = professors[2 * i + 1].IdProf,
                IdSub = subjects[i].IdSub
            });
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedStudentSubjectsAsync(List<Student> students, List<Subject> subjects)
    {
        if (await _context.StudentSubjects.AnyAsync()) return;

        for (int i = 0; i < students.Count; i++)
        {
            var subRange = i % 2 == 0
                ? subjects.Take(2).ToList()
                : subjects.Skip(2).Take(2).ToList();

            foreach (var sub in subRange)
            {
                _context.StudentSubjects.Add(new StudentSubject
                {
                    IdStud = students[i].IdStud,
                    IdSub = sub.IdSub
                });
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedSubjectAuxMeansAsync(List<Subject> subjects, List<AuxiliaryMeans> means)
    {
        if (await _context.SubjectAuxMeans.AnyAsync()) return;

        var count = Math.Min(subjects.Count, means.Count);
        for (int i = 0; i < count; i++)
        {
            _context.SubjectAuxMeans.Add(new SubjectAuxMean
            {
                IdSub = subjects[i].IdSub,
                IdAuxMean = means[i].IdMean
            });
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedProfessorStudentSubjectsAsync(
        List<Professor> professors,
        List<Student> students,
        List<Subject> subjects)
    {
        if (await _context.ProfessorStudentSubjects.AnyAsync()) return;

        var professorSubjects = await _context.ProfessorSubjects.ToListAsync();
        var studentSubjects = await _context.StudentSubjects.ToListAsync();

        foreach (var ps in professorSubjects)
        {
            var matching = studentSubjects.Where(ss => ss.IdSub == ps.IdSub).ToList();
            foreach (var ss in matching)
            {
                _context.ProfessorStudentSubjects.Add(new ProfessorStudentSubject
                {
                    IdProf = ps.IdProf,
                    IdStudSub = ss.IdStudSub,
                    StudentGrades = 8
                });
            }
        }
        await _context.SaveChangesAsync();
    }
}