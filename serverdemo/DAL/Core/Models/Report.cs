using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Core.Models
{
    public class Report : Trackable
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AverageDistance { get; set; }
        public float AverageDuration { get; set; }
        public bool IsActive { get; set; }

        public Project Project { get; set; }
        public ICollection<ProjectionData> Projections { get; set; }
        public Report()
        {
            Projections = new HashSet<ProjectionData>();
        }
    }
}
