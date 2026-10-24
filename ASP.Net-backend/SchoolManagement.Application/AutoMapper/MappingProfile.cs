using AutoMapper;
using SchoolManagement.Application.ApplicationServices.Maps_Dto;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Administrator;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.AuxiliaryMeans;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoom;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomRestriction;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomTechMean;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Course;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Maintenance;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfStudSubCourse;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Professor;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorStudentSubject;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorSubject;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Restriction;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Secretary;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.SecretaryProfessorStudentSubject;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Student;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.StudentSubject;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Subject;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.SubjectAuxMean;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.TechnologicalMeans;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ---------- Identity ----------
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<RegisterDto, User>();
        CreateMap<LoginDto, User>();

        // ---------- Administrator ----------
        CreateMap<AdministratorDto, Administrator>();
        CreateMap<Administrator, AdministratorDto>();
        CreateMap<Administrator, AdministratorResponseDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.AdminId))
            .ForMember(d => d.Administrator, o => o.MapFrom(s => s));

        // ---------- Course ----------
        CreateMap<CourseDto, Course>().ForMember(d => d.Students, o => o.Ignore());
        CreateMap<Course, CourseDto>();
        CreateMap<Course, CourseResponseDto>();

        // ---------- AuxiliaryMeans ----------
        CreateMap<AuxiliaryMeansDto, AuxiliaryMeans>()
            .ForMember(d => d.Subjects, o => o.Ignore())
            .ForMember(d => d.Professors, o => o.Ignore())
            .ForMember(d => d.Maintenances, o => o.Ignore());
        CreateMap<AuxiliaryMeans, AuxiliaryMeansResponseDto>();

        // ---------- TechnologicalMeans ----------
        CreateMap<TechnologicalMeansDto, TechnologicalMeans>()
            .ForMember(d => d.ClassRooms, o => o.Ignore())
            .ForMember(d => d.Maintenances, o => o.Ignore());
        CreateMap<TechnologicalMeans, TechnologicalMeansResponseDto>();

        // ---------- Maintenance ----------
        CreateMap<MaintenanceDto, Maintenance>()
            .ForMember(d => d.AuxMean, o => o.Ignore())
            .ForMember(d => d.TechnologicalMean, o => o.Ignore());
        CreateMap<Maintenance, MaintenanceResponseDto>()
            .ForMember(d => d.MeanName, o => o.Ignore());

        // ---------- Restriction ----------
        CreateMap<RestrictionDto, Restriction>().ForMember(d => d.ClassRooms, o => o.Ignore());
        CreateMap<Restriction, RestrictionResponseDto>();

        // ---------- ClassRoom ----------
        CreateMap<ClassRoomDto, ClassRoom>()
            .ForMember(d => d.Subjects, o => o.Ignore())
            .ForMember(d => d.Professors, o => o.Ignore())
            .ForMember(d => d.Restrictions, o => o.Ignore())
            .ForMember(d => d.TechnologicalMeans, o => o.Ignore());
        CreateMap<ClassRoom, ClassRoomResponseDto>();

        // ---------- ClassRoomRestriction ----------
        CreateMap<ClassRoomRestrictionDto, ClassRoomRestriction>()
            .ForMember(d => d.ClassRoom, o => o.Ignore())
            .ForMember(d => d.Restriction, o => o.Ignore());
        CreateMap<ClassRoomRestriction, ClassRoomRestrictionResponseDto>();

        // ---------- ClassRoomTechMean ----------
        CreateMap<ClassRoomTechMeanDto, ClassRoomTechMean>()
            .ForMember(d => d.ClassRoom, o => o.Ignore())
            .ForMember(d => d.TechnologicalMeans, o => o.Ignore());
        CreateMap<ClassRoomTechMean, ClassRoomTechMeanResponseDto>()
            .ForMember(d => d.TechName, o => o.Ignore());

        // ---------- SubjectAuxMean ----------
        CreateMap<SubjectAuxMeanDto, SubjectAuxMean>()
            .ForMember(d => d.Subject, o => o.Ignore())
            .ForMember(d => d.AuxMean, o => o.Ignore());
        CreateMap<SubjectAuxMean, SubjectAuxMeanResponseDto>();

        // ---------- ProfessorSubject ----------
        CreateMap<ProfessorSubjectDto, ProfessorSubject>()
            .ForMember(d => d.Professor, o => o.Ignore())
            .ForMember(d => d.Subject, o => o.Ignore());
        CreateMap<ProfessorSubject, ProfessorSubjectResponseDto>();

        // ---------- StudentSubject ----------
        CreateMap<StudentSubjectDto, StudentSubject>()
            .ForMember(d => d.Student, o => o.Ignore())
            .ForMember(d => d.Subject, o => o.Ignore())
            .ForMember(d => d.Professors, o => o.Ignore());
        CreateMap<StudentSubject, StudentSubjectResponseDto>();

        // ---------- Subject ----------
        CreateMap<SubjectDto, Subject>()
            .ForMember(d => d.ClassRoom, o => o.Ignore())
            .ForMember(d => d.Professors, o => o.Ignore())
            .ForMember(d => d.Students, o => o.Ignore())
            .ForMember(d => d.AuxiliaryMeans, o => o.Ignore());
        CreateMap<Subject, SubjectResponseDto>()
            .ForMember(d => d.ClassRoomLocation, o => o.Ignore());

        // ---------- Professor ----------
        CreateMap<ProfessorDto, Professor>()
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Subjects, o => o.Ignore())
            .ForMember(d => d.AuxiliaryMeans, o => o.Ignore())
            .ForMember(d => d.ClassRooms, o => o.Ignore())
            .ForMember(d => d.StudentSubjects, o => o.Ignore());
        CreateMap<Professor, ProfessorResponseDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.IdProf))
            .ForMember(d => d.Professor, o => o.MapFrom(s => s));

        // ---------- Secretary ----------
        CreateMap<SecretaryDto, Secretary>()
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Evaluations, o => o.Ignore());
        CreateMap<Secretary, SecretaryResponseDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.IdS))
            .ForMember(d => d.Secretary, o => o.MapFrom(s => s));

        // ---------- Student ----------
        CreateMap<StudentDto, Student>()
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Course, o => o.Ignore())
            .ForMember(d => d.Subjects, o => o.Ignore())
            .ForMember(d => d.Professors, o => o.Ignore());
        CreateMap<Student, StudentResponseDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.IdStud))
            .ForMember(d => d.IdC, o => o.MapFrom(s => s.IdC))
            .ForMember(d => d.Student, o => o.MapFrom(s => s));

        // ---------- ProfessorStudentSubject ----------
        CreateMap<ProfessorStudentSubjectDto, ProfessorStudentSubject>()
            .ForMember(d => d.Professor, o => o.Ignore())
            .ForMember(d => d.StudentSubject, o => o.Ignore())
            .ForMember(d => d.Secretaries, o => o.Ignore())
            .ForMember(d => d.IdProf, o => o.Ignore())
            .ForMember(d => d.IdStudSub, o => o.Ignore());
        CreateMap<ProfessorStudentSubject, ProfessorStudentSubjectResponseDto>()
            .ForMember(d => d.IdStud, o => o.Ignore())
            .ForMember(d => d.IdSub, o => o.Ignore())
            .ForMember(d => d.StudentName, o => o.Ignore())
            .ForMember(d => d.SubjectName, o => o.Ignore())
            .ForMember(d => d.ProfessorName, o => o.Ignore());

        // ---------- ProfStudSubCourse ----------
        CreateMap<ProfStudSubCourseDto, ProfStudSubCourse>()
            .ForMember(d => d.Professor, o => o.Ignore())
            .ForMember(d => d.Student, o => o.Ignore())
            .ForMember(d => d.Subject, o => o.Ignore())
            .ForMember(d => d.Course, o => o.Ignore());
        CreateMap<ProfStudSubCourse, ProfStudSubCourseResponseDto>()
            .ForMember(d => d.StudentName, o => o.Ignore())
            .ForMember(d => d.ProfessorName, o => o.Ignore())
            .ForMember(d => d.CourseName, o => o.Ignore())
            .ForMember(d => d.SubjectName, o => o.Ignore());

        // ---------- SecretaryProfessorStudentSubject ----------
        CreateMap<SecretaryProfessorStudentSubjectDto, SecretaryProfessorStudentSubject>()
            .ForMember(d => d.Secretary, o => o.Ignore())
            .ForMember(d => d.Evaluation, o => o.Ignore());
        CreateMap<SecretaryProfessorStudentSubject, SecretaryProfessorStudentSubjectResponseDto>();
    }
}