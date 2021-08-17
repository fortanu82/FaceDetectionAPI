using FaceDetectionDemoUpload.AppConfig;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaceDetectionDemoUpload.Services
{
    public class FaceDetectionService
    {
        static string personGroupId = "475a2258-b2c7-47f6-b8ea-fc610067f9d0"; //; Guid.NewGuid().ToString();
        const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;
        const string IMAGE_BASE_URL = "https://imageupload0408.blob.core.windows.net/imageupload/";
        // </snippet_image_url>

        // <snippet_creds>
        // From your Face subscription in the Azure portal, get your subscription key and endpoint.
        const string SUBSCRIPTION_KEY = "eb4dfc2ecc4549859bd345425d425433";
        const string ENDPOINT = "https://faceapidemo0508.cognitiveservices.azure.com/";
        IFaceClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
        public async Task IdentifyInPersonGroup(string employeeName, string imageName, string employeeImageURL)
        {
            Console.WriteLine("========IDENTIFY FACES========");
            Console.WriteLine();

            // Create a dictionary for all your images, grouping similar ones under the same key.
            // A group photo that includes some of the persons you seek to identify from your dictionary.
            //string sourceImageFileName = "tarun3.JPG";
            // </snippet_persongroup_files>

            // <snippet_persongroup_create>
            // Create a person group. 
            Console.WriteLine($"Create a person group ({personGroupId}).");
            bool statusCodeNotFound = false;
             try
             {
                 PersonGroup personGroup = await client.PersonGroup.GetAsync(personGroupId);
                 statusCodeNotFound = true;
             }
             catch (Exception ex)
             {
                 string message = ex.Message;
                 statusCodeNotFound = message.Contains("status code 'NotFound'");
                 statusCodeNotFound = false;
            }
             if (!statusCodeNotFound)
             {
                 await client.PersonGroup.CreateAsync(personGroupId, personGroupId, recognitionModel: RECOGNITION_MODEL4);
             }
            // The similar faces will be grouped into a single person group person.
            // Limit TPS
            await Task.Delay(250);
            Person person = await client.PersonGroupPerson.CreateAsync(personGroupId: personGroupId, name: employeeName);
            Console.WriteLine($"Create a person group person '{employeeName}'.");

            // Add face to the person group person.
            Console.WriteLine($"Add face to the person group person({employeeName}) from image `{imageName}`");
            PersistedFace face = await client.PersonGroupPerson.AddFaceFromUrlAsync(personGroupId, person.PersonId,
                $"{employeeImageURL}", imageName);
            // </snippet_persongroup_create>

            // <snippet_persongroup_train>
            // Start to train the person group.
            Console.WriteLine();
            Console.WriteLine($"Train person group {personGroupId}.");
            await client.PersonGroup.TrainAsync(personGroupId);

            // Wait until the training is completed.
            while (true)
            {
                await Task.Delay(1000);
                var trainingStatus = await client.PersonGroup.GetTrainingStatusAsync(personGroupId);
                Console.WriteLine($"Training status: {trainingStatus.Status}.");
                if (trainingStatus.Status == TrainingStatusType.Succeeded) { break; }
            }
            Console.WriteLine();

        }

        public async Task<IEnumerable> DetectEmployeeFace(string sourceImageFileURL)
        {
            // </snippet_persongroup_train>
            // <snippet_identify_sources>
            List<string> employeeFaceDetectionResults = new List<string>();

            List<Guid> sourceFaceIds = new List<Guid>();
            // Detect faces from source image url.
            List<DetectedFace> detectedFaces = await DetectFaceRecognize(client, $"{sourceImageFileURL}", RECOGNITION_MODEL4);

            // Add detected faceId to sourceFaceIds.
            foreach (var detectedFace in detectedFaces) { sourceFaceIds.Add(detectedFace.FaceId.Value); }
            // </snippet_identify_sources>

            // <snippet_identify>
            // Identify the faces in a person group. 
            var identifyResults = await client.Face.IdentifyAsync(sourceFaceIds, personGroupId);

            foreach (var identifyResult in identifyResults)
            {
                if (identifyResult.Candidates.Count > 0)
                { 
                Person person = await client.PersonGroupPerson.GetAsync(personGroupId, identifyResult.Candidates[0].PersonId);
                Console.WriteLine($"Person '{person.Name}' is identified for face in: {sourceImageFileURL} - {identifyResult.FaceId}," +
                    $" confidence: {identifyResult.Candidates[0].Confidence}.");
                employeeFaceDetectionResults.Add($"Person '{person.Name}' is identified for face in: {sourceImageFileURL} - {identifyResult.FaceId}," +
                  $" confidence: {identifyResult.Candidates[0].Confidence}.");
                }
            }
            Console.WriteLine();
            return employeeFaceDetectionResults;
        }

        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string url, string recognition_model)
        {
            // Detect faces from image URL. Since only recognizing, use the recognition model 1.
            // We use detection model 3 because we are not retrieving attributes.
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(url, recognitionModel: recognition_model, detectionModel: DetectionModel.Detection03);
            Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{Path.GetFileName(url)}`");
            return detectedFaces.ToList();
        }
        // <snippet_deletepersongroup>
        /*
		 * DELETE PERSON GROUP
		 * After this entire example is executed, delete the person group in your Azure account,
		 * otherwise you cannot recreate one with the same name (if running example repeatedly).
		 */
        public async Task<string> DeletePersonGroup()
        {
            string result = "";
            bool statusCodeNotFound = false;
            try
            {
                PersonGroup personGroup = await client.PersonGroup.GetAsync(personGroupId);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                statusCodeNotFound = message.Contains("status code 'NotFound'");                
                return result = "Does not exist";
            }
            if (!statusCodeNotFound)
            {
                await client.PersonGroup.DeleteAsync(personGroupId);
                result = "Deleted";
            }
                return result;
        }
        public static IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

    }
}
