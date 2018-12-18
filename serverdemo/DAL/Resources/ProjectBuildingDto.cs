using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Resources
{
    public class ProjectBuildingDto
    {
        public int? Id { get; set; }
        public int ProjectId { get; set; }
        public int BuildingId { get; set; }
    }
}
