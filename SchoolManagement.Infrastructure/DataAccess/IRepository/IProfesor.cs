using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolManagement.Domain;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Common.Interfaces;

namespace SchoolManagement.Infrastructure.DataAccess.IProfesorRepository
{
    public interface IProfesorRepository : IRepository<Profesor>
    {
      
    }
}