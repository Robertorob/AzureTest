using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AzureTest.Models;
using AzureTest.Services;
using System.Linq;
using System.IO;

namespace AzureTest.Controllers
{
    public class StorageAccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext context;
        private readonly StorageAccountService storageAccountService;

        public StorageAccountController(ILogger<HomeController> logger, DatabaseContext context, StorageAccountService storageAccountService)
        {
            this.context = context;
            _logger = logger;
            this.storageAccountService = storageAccountService;
        }

        public IActionResult Index()
        {
            var files = this.storageAccountService
                .ListFiles()
                .Select(f => new FileModel 
                { 
                    FileName = Path.GetFileNameWithoutExtension(f.FileName), 
                    Extension = Path.GetExtension(f.FileName),
                    Size = f.Size,
                    LastModified = f.LastModified
                });

            var containers = this.storageAccountService.ListContainers();

            return View(new StorageAccountModel
            {
                Files = files,
                Containers = containers
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
