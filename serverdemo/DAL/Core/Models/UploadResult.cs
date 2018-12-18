using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Core.Models
{
    public class UploadResult
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public string FileName { get; set; }
    }
}
