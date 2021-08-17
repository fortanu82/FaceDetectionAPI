using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceDetectionDemoUpload.Model
{
    public class FormBlobModel
    {
        public string EmployeeName { get; set; }
        public string FileName { get; set; }
        public IFormFile FileImage { get; set; }
    }
}
