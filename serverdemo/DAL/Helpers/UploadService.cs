using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DAL.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public class UploadService
    {
        private const string bucketName = "test-commute-time-study/uploads";
        private string keyName = "";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

        private IAmazonS3 s3Client;
        public UploadService(string _awsAccessKey, string _awsSecretKey, string _keyName)
        {
            keyName = _keyName;
            s3Client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, bucketRegion);
        }

        public async Task<UploadResult> UploadFileAsync(string filePath)
        {
            bool success = false;
            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                // Option 3. Upload data from a type of System.IO.Stream.
                using (var fileToUpload =
                    new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload,
                                               bucketName, keyName);
                }
                success = true;
           }
            catch (AmazonS3Exception e)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            var r = new UploadResult() { Success = success };
            return r;
        }

        public string GetObjectUrl(string keyName)
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
            request.BucketName = bucketName;
            request.Key = keyName;
            request.Expires = DateTime.Now.AddHours(1);
            request.Protocol = Protocol.HTTP;
            string url = s3Client.GetPreSignedURL(request);
            return url;
        }

        public async Task<string> GetObjectData(string keyName)
        {
            string responseBody = "";
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };
                using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                    string contentType = response.Headers["Content-Type"];
                    Console.WriteLine("Object metadata, Title: {0}", title);
                    Console.WriteLine("Content type: {0}", contentType);

                    responseBody = reader.ReadToEnd(); // Now you process the response body.
                    return responseBody;
                }
            }
            catch (AmazonS3Exception e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
