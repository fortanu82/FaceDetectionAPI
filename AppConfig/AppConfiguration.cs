using Microsoft.Extensions.Configuration;  
using System;  
using System.Collections.Generic;  
using System.IO;  
using System.Text;  
  
namespace FaceDetectionDemoUpload.AppConfig
{
    public static class AppConfiguration
    {
        private static IConfiguration currentConfig;

        public static void SetConfig(IConfiguration configuration)
        {
            currentConfig = configuration;
        }


        public static string GetConfiguration(string configKey)
        {
            try
            {
                string connectionString = currentConfig.GetConnectionString(configKey);
                return connectionString;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return "";
        }
        public static string GetConfigurationKey(string configKey)
        {
            try
            {
                string value = currentConfig.GetValue<string>(configKey);
                return value;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return "";
        }

    }
}