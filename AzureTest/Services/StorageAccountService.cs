using AzureTest.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTest.Services
{
    public class StorageAccountService
    {
        private readonly IConfiguration configuration;
        private readonly AzureKeyVaultService keyVaultServie;
        private readonly string storageAccountConnectionString;

        public StorageAccountService(IConfiguration configuration, AzureKeyVaultService keyVaultServie)
        {
            this.configuration = configuration;
            this.keyVaultServie = keyVaultServie;
            this.storageAccountConnectionString = this.keyVaultServie.GetConnectionString("StorageAccountConnectionString");
        }

        //public void UploadToBlob(string fileToUpload, string azureContainerName)
        //{
        //    Console.WriteLine("Inside upload method");

        //    string file_extension,
        //    filename_withExtension,
        //    storageAccount_connectionString;
        //    Stream file;

        //    //Copy the storage account connection string from Azure portal     
        //    storageAccount_connectionString = "your Azure storage account connection string here";

        //    // << reading the file as filestream from local machine >>    
        //    file = new FileStream(fileToUpload, FileMode.Open);

        //    CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(storageAccount_connectionString);
        //    CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();
        //    var containers = blobClient.ListContainersSegmentedAsync(null).Result;
        //    CloudBlobContainer container = blobClient.GetContainerReference(azureContainerName);

        //    //checking the container exists or not  
        //    if (container.CreateIfNotExistsAsync().Result)
        //    {

        //        container.SetPermissionsAsync(new BlobContainerPermissions
        //        {
        //            PublicAccess =
        //          BlobContainerPublicAccessType.Blob
        //        });

        //    }

        //    //reading file name & file extention    
        //    file_extension = Path.GetExtension(fileToUpload);
        //    filename_withExtension = Path.GetFileName(fileToUpload);

        //    CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filename_withExtension);
        //    cloudBlockBlob.Properties.ContentType = file_extension;

        //    cloudBlockBlob.UploadFromStreamAsync(file); // << Uploading the file to the blob >>  

        //    Console.WriteLine("Upload Completed!");

        //}

        //public async void DownloadFromBlob(string filetoDownload, string azureContainerName)
        //{
        //    Console.WriteLine("Inside downloadfromBlob()");

        //    string conectionString = this.configuration.GetSection("StorageAccount")["connectionString"];

        //    CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(conectionString);
        //    CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

        //    CloudBlobContainer container = blobClient.GetContainerReference(azureContainerName);
        //    CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filetoDownload);

        //    // provide the file download location below            
        //    Stream file = File.OpenWrite(@"E:\Tools\BlobFile\" + filetoDownload);    
          

        //    await cloudBlockBlob.DownloadToStreamAsync(file);

        //    Console.WriteLine("Download completed!");

        //}


        public IEnumerable<FileModel> ListFiles(string containerName)
        {
            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(this.storageAccountConnectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            return container.ListBlobsSegmentedAsync(null)
                .Result
                .Results
                .Select(f => new FileModel 
                { 
                    FileName = Path.GetFileNameWithoutExtension(f.Uri.Segments.Last()),
                    Size = "123",
                    LastModified = "04/16/2020",
                    Extension = Path.GetExtension(f.Uri.Segments.Last())
                });
        }

        public IEnumerable<ContainerModel> ListContainers()
        {
            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(this.storageAccountConnectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

            var containers = blobClient.ListContainersSegmentedAsync(null)
                .Result
                .Results
                .Select(f => new ContainerModel
                {
                    ContainerName = f.Name,
                    Files = ListFiles(f.Name)
                });

            return containers;
        }
    }
}
