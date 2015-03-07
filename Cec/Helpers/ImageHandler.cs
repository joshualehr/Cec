using Cec.Models;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Cec.Helpers
{
    /// <summary>
    /// Class to help handle images
    /// </summary>
    public class ImageHandler
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();
        private string relativeDirectoryPath = "/Content/Images/";

        /// <summary>
        /// Method to save several sizes of the image and return a list of their physical path
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="directoryPath">Physical directory path</param>      
        public List<string> Save(HttpPostedFile image, string directoryPath)
        {
            Dictionary<string, string> versions = new Dictionary<string, string>();
            //Define the versions to generate
            versions.Add("_thumb", "width=100&height=100&crop=auto&format=jpg"); //Crop to square thumbnail
            versions.Add("_medium", "maxwidth=400&maxheight=400&format=jpg"); //Fit inside 400x400 area, jpeg
            versions.Add("_large", "maxwidth=1900&maxheight=1900&format=jpg"); //Fit inside 1900x1200 area

            //Loop through each uploaded file
            var physicalPaths = new List<string>();
            foreach (string fileKey in HttpContext.Current.Request.Files.Keys)
            {
                HttpPostedFile file = HttpContext.Current.Request.Files[fileKey];
                if (file.ContentLength <= 0) continue; //Skip unused file controls.

                //Get the physical path for the uploads folder and make sure it exists
                string uploadFolder = directoryPath;
                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                //Generate each version
                var name = Guid.NewGuid().ToString();
                foreach (string suffix in versions.Keys)
                {
                    //Generate a filename (GUIDs are best).
                    string fileName = Path.Combine(uploadFolder, name + suffix);

                    //Let the image builder add the correct extension based on the output file type
                    fileName = ImageBuilder.Current.Build(new ImageJob(image, fileName, new Instructions(versions[suffix]), false, true)).FinalPath;
                    physicalPaths.Add(fileName);
                }
            }
            return physicalPaths;
        }

        /// <summary>
        /// Method to save a new image and return it's relative path
        /// </summary>
        /// <param name="image">Image to upload</param>
        /// <param name="directoryPath">Path to the save to directory</param>
        public string SaveNewImage(HttpPostedFileBase image, string directoryPath)
        {
            if (image != null)
            {
                var ext = Path.GetExtension(image.FileName);
                var name = Guid.NewGuid().ToString();
                var fileName = name + ext;
                var relativePath = relativeDirectoryPath + fileName;
                image.SaveAs(directoryPath + fileName);
                return relativePath;
            }
            return null;
        }

        /// <summary>
        /// Method to delete an image
        /// </summary>
        /// <param name="imagePath">Path to the image</param>
        public void DeleteImage(string imagePath)
        {
            File.Delete(imagePath);
        }

        /// <summary>
        /// Method to check whether image is used by only one material
        /// </summary>
        /// <param name="imagePath">Path to the image</param>
        public bool OtherMaterialUsesImage(string imagePath)
        {
            var materials = db.Materials.Where(m => m.ImagePath == imagePath);
            if (materials.Count() > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method to check whether image is used by any material
        /// </summary>
        /// <param name="imagePath">Path to the image</param>
        public bool AnyMaterialUsesImage(string imagePath)
        {
            return db.Materials.Any(m => m.ImagePath == imagePath);
        }

        /// <summary>
        /// Method to return a material's image path if saved
        /// </summary>
        /// <param name="imagePath">Path to the image</param>
        public string GetPossibleImagePath(HttpPostedFileBase image)
        {
            var fileName = Path.GetFileName(image.FileName);
            var relativePath = relativeDirectoryPath + fileName;
            return relativePath;
        }
    }
}