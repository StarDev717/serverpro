using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Resources
{
    public class CreateReportDto
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AverageDistance { get; set; }
        public float AverageDuration { get; set; }
        public bool IsActive { get; set; }
        public ICollection<CreateProjectionDto> Projections { get; set; }
    }
}
