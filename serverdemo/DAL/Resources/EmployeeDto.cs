using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Resources
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
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
        public bool IsActive { get; set; }
    }
}
