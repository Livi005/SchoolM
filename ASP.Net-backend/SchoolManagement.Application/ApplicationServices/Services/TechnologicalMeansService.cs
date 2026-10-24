using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.TechnologicalMeans;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class TechnologicalMeansService
    : BaseService<TechnologicalMeans, TechnologicalMeansDto, TechnologicalMeansResponseDto>, ITechnologicalMeansService
{
    public TechnologicalMeansService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TechnologicalMeansService> logger)
        : base(unitOfWork, mapper, logger) { }
}