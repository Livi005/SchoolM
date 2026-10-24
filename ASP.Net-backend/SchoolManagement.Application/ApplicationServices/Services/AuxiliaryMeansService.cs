using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.AuxiliaryMeans;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class AuxiliaryMeansService
    : BaseService<AuxiliaryMeans, AuxiliaryMeansDto, AuxiliaryMeansResponseDto>, IAuxiliaryMeansService
{
    public AuxiliaryMeansService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuxiliaryMeansService> logger)
        : base(unitOfWork, mapper, logger) { }
}