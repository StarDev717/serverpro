using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Core.Models
{
    public class S3ServiceSettings
    {
        public string Profile { get; set; }
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
