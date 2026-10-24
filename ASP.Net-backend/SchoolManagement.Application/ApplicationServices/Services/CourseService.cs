using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Course;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class CourseService : BaseService<Course, CourseDto, CourseResponseDto>, ICourseService
{
    public CourseService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CourseService> logger)
        : base(unitOfWork, mapper, logger)
    {
    }
}