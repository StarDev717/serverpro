using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Resources
{
    public class ProjectDto
    {
        public ProjectDto()
        {
            Buildings = new HashSet<BuildingDto>();
        }

        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<BuildingDto> Buildings { get; set; }
        public ICollection<ReportDto> Reports { get; set; }
        public ICollection<EmployeeDto> Employees { get; set; }
    }
}
