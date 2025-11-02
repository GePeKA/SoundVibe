using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Abstractions.Services;

namespace Infrastructure.Services
{
	public class S3StorageService: IStorageService
	{
		private readonly AmazonS3Client _s3Client;
		private const string BUCKET_NAME = "music-object-storage";
		private const double DURATION = 24; //Длительность подписанной ссылки в часах
		private const int MAX_SIZE = 1024 * 1024 * 50;

		public S3StorageService()
		{
			AmazonS3Config configsS3 = new AmazonS3Config
			{
				ServiceURL = "https://s3.yandexcloud.net"
			};
			_s3Client = new AmazonS3Client(configsS3);
		}

		public async Task<bool> PutAsync(string name, Stream fileStream, string contentType)
		{
			if (fileStream.Length > MAX_SIZE)
			{
				throw new Exception(); //TODO exception
			}

			var putObjectRequest = new PutObjectRequest() 
			{ 
				BucketName = BUCKET_NAME,
				Key = name,
				InputStream = fileStream,
				ContentType = contentType
			};

			var response = await _s3Client.PutObjectAsync(putObjectRequest);

			return (int)response.HttpStatusCode == 200;
		}

		public async Task<string> GetUrlAsync(string name)
		{
			var request = new GetPreSignedUrlRequest()
			{
				BucketName = BUCKET_NAME,
				Key = name,
				Verb = HttpVerb.GET,
				Expires = DateTime.UtcNow.AddHours(DURATION)
			};

			var url = await _s3Client.GetPreSignedURLAsync(request);

			return url;
		}

        private async Task EnsureBucketExists()
        {
            try
            {
                // Проверяем наличие бакета
                var response = await _s3Client.ListBucketsAsync();
                var bucketExists = response.Buckets.Any(b => b.BucketName == _bucketName);

                if (!bucketExists)
                {
                    // Создаем бакет, если он не существует
                    var createBucketRequest = new PutBucketRequest { BucketName = _bucketName };
                    await _s3Client.PutBucketAsync(createBucketRequest);
                    Console.WriteLine($"Bucket {_bucketName} created successfully.");
                }
                else
                {
                    Console.WriteLine($"Bucket {_bucketName} exists.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error creating bucket {_bucketName}: {e.Message}");
                throw;
            }
        }
    }
}
