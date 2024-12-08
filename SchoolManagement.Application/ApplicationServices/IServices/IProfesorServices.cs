using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolManagement.Application.ApplicationServices.MapsDto.Profesor;
using SchoolManagement.Domain.Entities;
//using SchoolManagement.Infrastructure.DataAccess.Filters;

namespace SchoolManagement.Application.ApplicationServices.IServices
{
    public interface IProfesorService
    {
        Task<ProfesorDto> CreateProfesorAsync(ProfesorDto profesorDto);
        Task<ProfesorDto> UpdateProfesorAsync(ProfesorDto profesorDto);
        Task<IEnumerable<ProfesorDto>> ListProfesorAsync();
        Task DeleteProfesorByIdAsync(int profesorDto);
    }
}
