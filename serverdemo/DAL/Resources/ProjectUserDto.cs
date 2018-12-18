using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Resources
{
    public class ProjectUserDto
    {
        public int? Id { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
    }
}
