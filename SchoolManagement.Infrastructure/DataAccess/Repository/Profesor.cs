using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Infrastructure;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Common.Implementation;
using SchoolManagement.Infrastructure.DataAccess.IProfesorRepository;

namespace TravelAgency.Infrastructure.DataAccess.ProfesorRepository;
public class ProfesorRepository : GenericRepository<Profesor>, IProfesorRepository
{
    public ProfesorRepository(SchoolManagement_Context context) : base(context)
    {
    }
}
