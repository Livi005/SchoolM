using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Restriction;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class RestrictionService
    : BaseService<Restriction, RestrictionDto, RestrictionResponseDto>, IRestrictionService
{
    public RestrictionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RestrictionService> logger)
        : base(unitOfWork, mapper, logger) { }
}