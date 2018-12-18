using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Resources
{
    public class CreateProjectDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string Buildings { get; set; }
        public string Employees { get; set; }
    }
}
