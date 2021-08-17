using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FaceDetectionDemoUpload
{
    public class Program
    {
        public static byte[] fileToTest;
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static byte[] UploadFileAsync()
        {
            string fileName = "C:\\Users\\48013\\Desktop\\Backup\\Repos\\Vipul Razdan\\AI\\Face-api\\images\\tarun1.jpg";
            byte[] bytes = System.IO.File.ReadAllBytes(fileName);
            return bytes;
        }
    }


}
