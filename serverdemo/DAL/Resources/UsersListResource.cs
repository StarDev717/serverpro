using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Resources
{
    public class UsersListResource
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public bool IsAdmin { get; set; }
        public string HomePhone { get; set; }
        public string Gender { get; set; }
        public string Designation { get; set; }
        public string BirthDay { get; set; }
        public string BirthMonth { get; set; }
        public string BirthYear { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
