using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Resources
{
    public class ProjectionDataDto : UserDto
    {

        public int? Id { get; set; }
        // Building
        public string BuildingTitle { get; set; }
        public string Class { get; set; }
        public string Submarket { get; set; }
        public string BuildingAddress { get; set; }
        public string BuildingCity { get; set; }
        public string BuildingState { get; set; }
        public string BuildingCountry { get; set; }
        public string BuildingZip { get; set; }
        public double BuildingLat { get; set; }
        public double BuildingLong { get; set; }
        
        public ICollection<EmployeeDistanceDto> Employees { get; set; }
    }
}
