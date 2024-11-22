namespace InfinityNetServer.Services.File.Infrastructure.Minio
{
    public class MinioOptions
    {

        public string Endpoint { get; set; }

        public int Port { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public string MainBucketName { get; set; }

        public string TempBucketName { get; set; }

    }
}
