using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SchoolManagement.Application.ApplicationServices.MapsDto.Profesor;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Application.ApplicationServices.MapsDto;
public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<ProfesorDto, Domain.Entities.Profesor>();
        CreateMap<Domain.Entities.Profesor, ProfesorDto>();
    }
}