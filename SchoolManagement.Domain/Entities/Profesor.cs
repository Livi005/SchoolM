using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolManagement.Domain.Common;
namespace SchoolManagement.Domain.Entities;

public class Profesor : BaseEntity
{
    public int ID { get; set; }
    public string Name { get; set; } = null;
    public string LastName { get; set; } = null;
    public bool Is_Dean { get; set; } = false;
    public float Salary { get; set; } = 0;
    public int Contract { get; set; } = 0;
    public int Experience { get; set; } = 0;
}
