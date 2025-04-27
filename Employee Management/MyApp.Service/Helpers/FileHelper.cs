using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyApp.Service.Helpers
{
    public static class FileHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string UploadFolder = "wwwroot/uploads";

        /// <summary>
        /// Save the uploaded image file and return the saved filename.
        /// </summary>
        public static async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Logger.Warn("Attempted to save an empty or null file.");
                return null;
            }

            try
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid():N}{extension}";
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), UploadFolder, fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(savePath)); // Ensure folder exists

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                Logger.Info($"Image saved successfully: {fileName}");
                return fileName;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error saving image.");
                throw;
            }
        }
    }
}
