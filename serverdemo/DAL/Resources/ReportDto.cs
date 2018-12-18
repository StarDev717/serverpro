using System.Collections.Generic;

namespace DAL.Resources
{
    public class ReportDto
    {
        public int? Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AverageDistance { get; set; }
        public float AverageDuration { get; set; }
        public bool IsActive { get; set; }
        public ICollection<BuildingDistanceDto> Buildings { get; set; }
        public ICollection<EmployeeDistanceDto> Employees { get; set; }
        public ICollection<ProjectionDataDto> Projections { get; set; }

    }
}
