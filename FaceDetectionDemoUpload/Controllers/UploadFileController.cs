using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;
using FaceDetectionDemoUpload.Services;
using Microsoft.AspNetCore.Http;
using FaceDetectionDemoUpload.Model;

namespace FaceDetectionDemoUpload.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadFileController : ControllerBase
    {

        private readonly ILogger<UploadFileController> _logger;
        public UploadFileController(ILogger<UploadFileController> logger)
        {
            _logger = logger;

        }
        //
        [HttpGet("UploadFile")]
        public void UploadFile()
        {
            BlobStorageService blobStorageService = new BlobStorageService();
            blobStorageService.UploadFileToBlob("tarun1.jpg", Program.UploadFileAsync(), "image/jpg");
        }
        [HttpPost("UploadBlobFile")]

        //IFormCollection data, IFormFile imageFile
        public string UploadBlobFile([FromForm] FormBlobModel formModel)
        {
            string fileName = formModel.Name;
            var imageFile = formModel.FileImage;
            //            foreach (var file in files)
            //           {
            if (imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    //string s = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
                    BlobStorageService blobStorageService = new BlobStorageService();
                    string blobURL = blobStorageService.UploadFileToBlob(fileName, fileBytes, "image/jpg");
                    return blobURL;
                }
            }
            else 
            {
                throw new ArgumentNullException("Image File vaule is Null");
            }

   //         }
        }
    }
}
