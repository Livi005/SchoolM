using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.Application.ApplicationServices.MapsDto.Profesor
{
    public class ProfesorDto
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Is_Dean { get; set; }
        public float Salary { get; set; }
        public int Contract { get; set; }
        public int Experience { get; set; }
    }
}
