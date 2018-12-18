using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Resources
{
    public class EmployeeDistanceDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public float Duration { get; set; }
        public float Distance { get; set; }
        public double EmployeeLat { get; set; }
        public double EmployeeLong { get; set; }

    }
}
