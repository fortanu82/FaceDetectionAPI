using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceDetectionDemoUpload.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class IdentifyPersonController: ControllerBase
    {
        private readonly ILogger<IdentifyPersonController> _logger;
        public IdentifyPersonController(ILogger<IdentifyPersonController> logger)
        {
            _logger = logger;

        }

        //[HttpGet("IdentifyPersonFace")]

        //public async Task<string> UploadBlobFile([FromForm] FormBlobModel formModel)
    }
}
