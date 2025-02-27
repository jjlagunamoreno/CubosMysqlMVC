using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.IO;
using System.Linq;

namespace PracticaCubosMVC.Helpers
{
    public enum Folders { Images }

    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnvironment;

        public HelperPathProvider(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = folder == Folders.Images ? "images" : "uploads";
            string rootPath = this.hostEnvironment.WebRootPath;
            return Path.Combine(rootPath, carpeta, fileName);
        }
    }
}
