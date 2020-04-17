using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTest.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Size { get; set; }
        public string LastModified { get; set; }
    }
}