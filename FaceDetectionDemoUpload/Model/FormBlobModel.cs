using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceDetectionDemoUpload.Model
{
    public class FormBlobModel
    {
        public string Name { get; set; }
        public IFormFile FileImage { get; set; }
    }
}
