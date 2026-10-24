using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Services;
using SchoolManagement.Application.Authentication;
using SchoolManagement.Application.AutoMapper;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IAverageCalculator, AverageCalculator>();
        services.AddScoped<IUserRegistrationService, UserRegistrationService>();

        // ---- Fase 1 ----
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IAdministratorService, AdministratorService>();

        // ---- Fase 2 ----
        services.AddScoped<IAuxiliaryMeansService, AuxiliaryMeansService>();
        services.AddScoped<ITechnologicalMeansService, TechnologicalMeansService>();
        services.AddScoped<IMaintenanceService, MaintenanceService>();
        services.AddScoped<IRestrictionService, RestrictionService>();
        services.AddScoped<IClassRoomService, ClassRoomService>();
        services.AddScoped<IClassRoomRestrictionService, ClassRoomRestrictionService>();
        services.AddScoped<IClassRoomTechMeanService, ClassRoomTechMeanService>();
        services.AddScoped<ISubjectAuxMeanService, SubjectAuxMeanService>();
        services.AddScoped<IProfessorSubjectService, ProfessorSubjectService>();
        services.AddScoped<IStudentSubjectService, StudentSubjectService>();

        // ---- Fase 3 ----
        services.AddScoped<IProfessorService, ProfessorService>();
        services.AddScoped<ISecretaryService, SecretaryService>();
        services.AddScoped<IStudentService, StudentService>();

        // ---- Fase 4 ----
        services.AddScoped<IProfessorStudentSubjectService, ProfessorStudentSubjectService>();
        services.AddScoped<IProfStudSubCourseService, ProfStudSubCourseService>();
        services.AddScoped<ISecretaryProfessorStudentSubjectService, SecretaryProfessorStudentSubjectService>();

        // ---- Fase 5 ----
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IProfessorClassRoomService, ProfessorClassRoomService>();
        services.AddScoped<IClassRoomMeanRequestService, ClassRoomMeanRequestService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}