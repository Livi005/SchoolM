using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Notifications;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Infrastructure;

public class Context : IdentityDbContext<User>
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    // ---------- Entities ----------
    public DbSet<Administrator> Administrators => Set<Administrator>();
    public DbSet<AuxiliaryMeans> AuxiliaryMeans => Set<AuxiliaryMeans>();
    public DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Maintenance> Maintenances => Set<Maintenance>();
    public DbSet<Professor> Professors => Set<Professor>();
    public DbSet<ProfessorPunishment> ProfessorsPunishments => Set<ProfessorPunishment>();
    public DbSet<Restriction> Restrictions => Set<Restriction>();
    public DbSet<Secretary> Secretaries => Set<Secretary>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<TechnologicalMeans> TechnologicalMeans => Set<TechnologicalMeans>();

    // ---------- Relations ----------
    public DbSet<ClassRoomRestriction> ClassRoomRestrictions => Set<ClassRoomRestriction>();
    public DbSet<ClassRoomTechMean> ClassRoomTechMeans => Set<ClassRoomTechMean>();
    public DbSet<ProfessorClassRoom> ProfessorClassRooms => Set<ProfessorClassRoom>();
    public DbSet<ProfessorStudentSubject> ProfessorStudentSubjects => Set<ProfessorStudentSubject>();
    public DbSet<ProfessorSubject> ProfessorSubjects => Set<ProfessorSubject>();
    public DbSet<ProfStudSubCourse> ProfStudSubCourses => Set<ProfStudSubCourse>();
    public DbSet<SecretaryProfessorStudentSubject> SecretaryProfessorStudentSubjects => Set<SecretaryProfessorStudentSubject>();
    public DbSet<StudentSubject> StudentSubjects => Set<StudentSubject>();
    public DbSet<SubjectAuxMean> SubjectAuxMeans => Set<SubjectAuxMean>();

    // ---------- Notifications ----------
    public DbSet<MeanNotification> MeanNotifications => Set<MeanNotification>();
    public DbSet<ProfessorNotification> ProfessorNotifications => Set<ProfessorNotification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mean: herencia TPT (tabla base + tablas derivadas)
        modelBuilder.Entity<Mean>().UseTptMappingStrategy();

        // Filtro global de soft delete
        ApplySoftDeleteFilters(modelBuilder);

        // -------------------------------------------------------------
        // Entidades
        // -------------------------------------------------------------

        modelBuilder.Entity<Administrator>(e =>
        {
            e.HasKey(x => x.AdminId);
            e.Property(x => x.AdminName).IsRequired().HasMaxLength(64);
            e.Property(x => x.UserId).IsRequired().HasMaxLength(64);
            e.Property(x => x.AdminSalary).IsRequired();
        });

        modelBuilder.Entity<Mean>(e =>
        {
            e.Property(x => x.NameMean).IsRequired().HasMaxLength(32);
            e.Property(x => x.State).IsRequired().HasMaxLength(32);
        });

        modelBuilder.Entity<AuxiliaryMeans>(e =>
            e.Property(x => x.Type).IsRequired().HasMaxLength(16));

        modelBuilder.Entity<ClassRoom>(e =>
        {
            e.Property(x => x.Location).IsRequired();
        });

        modelBuilder.Entity<Course>(e =>
            e.Property(x => x.CourseName).IsRequired());

        modelBuilder.Entity<Maintenance>(e =>
        {
            e.HasKey(x => x.IdM);
            e.Property(x => x.MaintenanceDate).IsRequired();
            e.Property(x => x.Cost).IsRequired();
            e.Property(x => x.TypeOfMean).HasConversion<int>().IsRequired();

            e.HasOne(x => x.AuxMean).WithMany(a => a.Maintenances)
                .HasForeignKey(x => x.IdAuxMean).IsRequired(false);
            e.HasOne(x => x.TechnologicalMean).WithMany(t => t.Maintenances)
                .HasForeignKey(x => x.IdTechMean).IsRequired(false);
        });

        modelBuilder.Entity<Professor>(e =>
        {
            e.Property(x => x.UserId).IsRequired().HasMaxLength(64);
            e.Property(x => x.NameProf).IsRequired().HasMaxLength(64);
            e.Property(x => x.Contract).IsRequired().HasMaxLength(20);
            e.Property(x => x.Spec).IsRequired().HasMaxLength(20);

            // N:N con ClassRoom
            e.HasMany(p => p.ClassRooms).WithMany(c => c.Professors)
                .UsingEntity<ProfessorClassRoom>(
                    j => j.HasOne(x => x.ClassRoom).WithMany().HasForeignKey(x => x.IdClassR),
                    j => j.HasOne(x => x.Professor).WithMany().HasForeignKey(x => x.IdProf),
                    j => j.HasKey(x => x.IdProfClass));

            // N:N con Subject
            e.HasMany(p => p.Subjects).WithMany(s => s.Professors)
                .UsingEntity<ProfessorSubject>(
                    j => j.HasOne(x => x.Subject).WithMany().HasForeignKey(x => x.IdSub),
                    j => j.HasOne(x => x.Professor).WithMany().HasForeignKey(x => x.IdProf),
                    j => j.HasKey(x => x.IdProfSub));

            // N:N con StudentSubject (con nota)
            e.HasMany(p => p.StudentSubjects).WithMany(ss => ss.Professors)
                .UsingEntity<ProfessorStudentSubject>(
                    j => j.HasOne(x => x.StudentSubject).WithMany().HasForeignKey(x => x.IdStudSub),
                    j => j.HasOne(x => x.Professor).WithMany().HasForeignKey(x => x.IdProf),
                    j =>
                    {
                        j.HasKey(x => x.IdProfStudSub);
                        j.Property(x => x.StudentGrades).HasDefaultValue(0);
                    });
        });

        modelBuilder.Entity<ProfessorPunishment>(e =>
        {
            e.HasKey(x => x.IdPun);
            e.HasOne(x => x.Professor).WithMany().HasForeignKey(x => x.IdProf);
            e.Property(x => x.PunishmentDate).IsRequired();
        });

        modelBuilder.Entity<Restriction>(e =>
        {
            e.Property(x => x.NameRes).IsRequired().HasMaxLength(16);
        });

        modelBuilder.Entity<Secretary>(e =>
        {
            e.Property(x => x.NameS).IsRequired().HasMaxLength(64);
            e.Property(x => x.UserId).IsRequired();

            e.HasMany(s => s.Evaluations).WithMany(pss => pss.Secretaries)
                .UsingEntity<SecretaryProfessorStudentSubject>(
                    j => j.HasOne(x => x.Evaluation).WithMany().HasForeignKey(x => x.IdProfStudSub),
                    j => j.HasOne(x => x.Secretary).WithMany().HasForeignKey(x => x.IdSec),
                    j => j.HasKey(x => x.IdSecProfStudSub));
        });

        modelBuilder.Entity<Student>(e =>
        {
            e.Property(x => x.NameStud).IsRequired().HasMaxLength(64);
            e.Property(x => x.UserId).IsRequired();

            e.HasOne(s => s.Course).WithMany(c => c.Students)
                .HasForeignKey(s => s.IdC).OnDelete(DeleteBehavior.Cascade);

            e.HasMany(s => s.Subjects).WithMany(sub => sub.Students)
                .UsingEntity<StudentSubject>(
                    j => j.HasOne(x => x.Subject).WithMany().HasForeignKey(x => x.IdSub),
                    j => j.HasOne(x => x.Student).WithMany().HasForeignKey(x => x.IdStud),
                    j =>
                    {
                        j.HasKey(x => x.IdStudSub);
                        j.Property(x => x.NJAbsents).HasDefaultValue(0);
                    });
        });

        modelBuilder.Entity<Subject>(e =>
        {
            e.Property(x => x.NameSub).IsRequired().HasMaxLength(16);
            e.Property(x => x.StudyProgram).IsRequired().HasMaxLength(20);
            e.Property(x => x.CourseLoad).IsRequired();

            e.HasOne(s => s.ClassRoom).WithMany(c => c.Subjects)
                .HasForeignKey(s => s.IdClassRoom).OnDelete(DeleteBehavior.Cascade);

            e.HasMany(s => s.AuxiliaryMeans).WithMany(a => a.Subjects)
                .UsingEntity<SubjectAuxMean>(
                    j => j.HasOne(x => x.AuxMean).WithMany().HasForeignKey(x => x.IdAuxMean),
                    j => j.HasOne(x => x.Subject).WithMany().HasForeignKey(x => x.IdSub),
                    j => j.HasKey(x => x.IdSubAuxMean));
        });

        modelBuilder.Entity<ClassRoom>(e =>
        {
            e.HasMany(c => c.Restrictions).WithMany(r => r.ClassRooms)
                .UsingEntity<ClassRoomRestriction>(
                    j => j.HasOne(x => x.Restriction).WithMany().HasForeignKey(x => x.IdRest),
                    j => j.HasOne(x => x.ClassRoom).WithMany().HasForeignKey(x => x.IdClassRoom),
                    j => j.HasKey(x => x.IdClassRoomRest));

            e.HasMany(c => c.TechnologicalMeans).WithMany(t => t.ClassRooms)
                .UsingEntity<ClassRoomTechMean>(
                    j => j.HasOne(x => x.TechnologicalMeans).WithMany().HasForeignKey(x => x.IdTechMean),
                    j => j.HasOne(x => x.ClassRoom).WithMany().HasForeignKey(x => x.IdClassRoom),
                    j => j.HasKey(x => x.IdClassRoomTech));
        });

        // -------------------------------------------------------------
        // Relaciones independientes
        // -------------------------------------------------------------

        modelBuilder.Entity<ProfStudSubCourse>(e =>
        {
            e.HasKey(x => x.IdProfStudSubCourse);
            e.HasOne(x => x.Professor).WithMany().HasForeignKey(x => x.IdProf).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.Subject).WithMany().HasForeignKey(x => x.IdSub).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.Student).WithMany().HasForeignKey(x => x.IdStud).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.Course).WithMany().HasForeignKey(x => x.IdCourse).OnDelete(DeleteBehavior.NoAction);
        });

        // -------------------------------------------------------------
        // Notificaciones
        // -------------------------------------------------------------

        modelBuilder.Entity<MeanNotification>(e =>
        {
            e.HasKey(x => x.MeanNotId);
            e.Property(x => x.MeanName).IsRequired();
            e.Property(x => x.MeanType).IsRequired();
        });

        modelBuilder.Entity<ProfessorNotification>(e =>
        {
            e.HasKey(x => x.ProfNotId);
            e.Property(x => x.ProfName).IsRequired();
            e.HasOne(x => x.Professor).WithMany().HasForeignKey(x => x.IdProf);
        });
    }

    /// <summary>
    /// Aplica filtro global para que EF ignore entidades con IsDeleted = true.
    /// </summary>
    private static void ApplySoftDeleteFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType)) continue;
            if (entityType.ClrType.IsAbstract) continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var body = Expression.Equal(
                Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted)),
                Expression.Constant(false));
            var lambda = Expression.Lambda(body, parameter);
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}