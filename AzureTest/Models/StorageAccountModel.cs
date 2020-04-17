using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTest.Models
{
    public class StorageAccountModel
    {
        public IEnumerable<FileModel> Files { get; set; }
        public IEnumerable<ContainerModel> Containers { get; set; }
    }
}