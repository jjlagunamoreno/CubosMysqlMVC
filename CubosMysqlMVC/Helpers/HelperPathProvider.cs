using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace PracticaCubosMVC.Helpers
{
    public enum Folders { Images, Facturas, Uploads, Temporal }

    public class HelperPathProvider
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public HelperPathProvider(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = folder switch
            {
                Folders.Images => "images",
                Folders.Facturas => "facturas",
                Folders.Uploads => "uploads",
                Folders.Temporal => "temp",
                _ => "uploads"
            };

            string rootPath = _hostEnvironment.WebRootPath;
            return Path.Combine(rootPath, carpeta, fileName);
        }
    }
}
