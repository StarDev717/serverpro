using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Resources
{
    public class BuildingDto
    {
        public int? Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Class { get; set; }
        public string Submarket { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public bool IsActive { get; set; }
    }
}
