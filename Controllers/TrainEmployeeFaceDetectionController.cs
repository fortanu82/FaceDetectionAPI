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
using System.Threading.Tasks;
using System.Collections;

namespace FaceDetectionDemoUpload.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrainEmployeeFaceDetectionController : ControllerBase
    {

        private readonly ILogger<TrainEmployeeFaceDetectionController> _logger;
        public TrainEmployeeFaceDetectionController(ILogger<TrainEmployeeFaceDetectionController> logger)
        {
            _logger = logger;

        }

        [HttpGet("UploadFile")]
        public void UploadFile()
        {
            BlobStorageService blobStorageService = new BlobStorageService();
            blobStorageService.UploadFileToBlob("tarun1.jpg", Program.UploadFileAsync(), "image/jpg");
        }
        [HttpPost("TrainEmployeeEngine")]

        //IFormCollection data, IFormFile imageFile
        public async Task<string> TrainEmployeeEngine([FromForm] FormBlobModel formModel)
        {
            var employeeName = formModel.EmployeeName;
            string fileName = formModel.FileName;
            string blobURL = uploadFileToBlobStorage(formModel);
            FaceDetectionService faceDetectionService = new FaceDetectionService();
            await faceDetectionService.IdentifyInPersonGroup(employeeName, fileName, blobURL);
            return blobURL;
        }
        [HttpPost("IdentifyEmployeeImage")]
        public async Task<IEnumerable> IdentifyEmployeeImage([FromForm] FormBlobModel formModel)
        {
            string blobURL = uploadFileToBlobStorage(formModel);
            FaceDetectionService faceDetectionService = new FaceDetectionService();
            return await faceDetectionService.DetectEmployeeFace(blobURL);
            
        }

        [HttpDelete("DeletePersonGroup")]
        public async Task<string> DeletePersonGroup()
        {
            FaceDetectionService faceDetectionService = new FaceDetectionService();
            return await faceDetectionService.DeletePersonGroup();

        }


        private static string uploadFileToBlobStorage(FormBlobModel formModel)
        {
            
            string fileName = formModel.FileName;
            var imageFile = formModel.FileImage;
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
        }
    }
}
