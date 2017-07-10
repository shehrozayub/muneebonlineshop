using OnlineShopping.DAL;
using OnlineShopping.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace OnlineShopping.Utility
{
    public class UploadContent
    {
        /// <summary>
        /// Uploading and resizing an image, Currently it is used to upload member profile pic, provider service banner image and category image
        /// </summary> 
        /// <param name="originalImage"></param>
        /// <param name="imagePrefix"></param>
        /// <param name="rootPath"></param>
        /// <param name="server"></param>
        /// <param name="_unitOfWork"></param>
        /// <param name="memberId"></param>
        /// <param name="serviceId"></param>
        public void UploadImage(HttpPostedFileBase originalImage, string imagePrefix, string rootPath, HttpServerUtilityBase server, GenericUnitOfWork _unitOfWork, int memberId, int productId = 0, int categoryId = 0)
        {            
            bool existsOriginal = System.IO.Directory.Exists(server.MapPath("~/" + rootPath + "Original/"));
            if (!existsOriginal)
                System.IO.Directory.CreateDirectory(server.MapPath("~/" + rootPath + "Original/"));
            originalImage.SaveAs(server.MapPath("~/" + rootPath + "Original/" + imagePrefix + originalImage.FileName));

            // Large size for service banner image
            if (productId != 0)
            {
                WebImage img1 = new WebImage(server.MapPath("~/" + rootPath + "Original/" + imagePrefix + originalImage.FileName));
                img1.Resize(849, 320);
                bool exists = System.IO.Directory.Exists(server.MapPath("~/" + rootPath + "Large/"));
                if (!exists)
                    System.IO.Directory.CreateDirectory(server.MapPath("~/" + rootPath + "Large/"));
                img1.Save(Path.Combine(server.MapPath("~/" + rootPath + "Large/" + imagePrefix + originalImage.FileName)));
            }

            WebImage img2 = new WebImage(server.MapPath("~/" + rootPath + "Original/" + imagePrefix + originalImage.FileName));
            img2.Resize(274, 175);
            bool exists2 = System.IO.Directory.Exists(server.MapPath("~/" + rootPath + "Medium/"));
            if (!exists2)
                System.IO.Directory.CreateDirectory(server.MapPath("~/" + rootPath + "Medium/"));
            img2.Save(Path.Combine(server.MapPath("~/" + rootPath + "Medium/" + imagePrefix + originalImage.FileName)));

            WebImage img3 = new WebImage(server.MapPath("~/" + rootPath + "Original/" + imagePrefix + originalImage.FileName));
            img3.Resize(68, 68);
            bool exists3 = System.IO.Directory.Exists(server.MapPath("~/" + rootPath + "Small/"));
            if (!exists3)
                System.IO.Directory.CreateDirectory(server.MapPath("~/" + rootPath + "Small/"));
            img3.Save(Path.Combine(server.MapPath("~/" + rootPath + "Small/" + imagePrefix + originalImage.FileName)));

            System.IO.File.Delete(server.MapPath("~/" + rootPath + "Original/" + imagePrefix + originalImage.FileName));
        }

        public void UploadStaticPageImage(HttpPostedFileBase originalImage, string rootPath, string imagePrefix, HttpServerUtilityBase server)
        {
            originalImage.SaveAs(server.MapPath("~/" + rootPath + imagePrefix + originalImage.FileName));
        }

        /// <summary>
        /// Save an image at specified path 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageSubPath"></param>
        /// <param name="memberId"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string UploadImage1(HttpPostedFileBase image, string imageSubPath, HttpServerUtilityBase server, GenericUnitOfWork _unitOfWork, int memberId = 0)
        {
            string ImgPath = imageSubPath + memberId + "_" + image.FileName;
            var imgnew = server.MapPath(ImgPath);
            bool exists = System.IO.Directory.Exists(server.MapPath(imageSubPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(server.MapPath(imageSubPath));

            ResizeAndUploadImageByHttpPostedFileBase(image, (imageSubPath + "/Large/"), new Size(750, 300), memberId + "_", image.FileName, server);
            ResizeAndUploadImageByHttpPostedFileBase(image, (imageSubPath + "/Medium/"), new Size(234, 156), memberId + "_", image.FileName, server);
            return ImgPath;
        }

        #region Resize Image

        public static bool ResizeAndUploadImageByHttpPostedFileBase(HttpPostedFileBase originalImage, string targetPath, Size size, string imgPrefix, string outputImageName, HttpServerUtilityBase server)
        {
            bool success = false;
            try
            {
                bool exists = System.IO.Directory.Exists(server.MapPath(targetPath));
                if (!exists)
                    System.IO.Directory.CreateDirectory(server.MapPath(targetPath));

                WebImage img = new WebImage(originalImage.InputStream);
                if (img.Width > size.Width)
                    img.Resize(size.Width, size.Height);
                img.Save(Path.Combine(targetPath, imgPrefix + outputImageName));

                //using (var m = new MemoryStream())
                //{
                //    System.Drawing.Image img_Original = System.Drawing.Image.FromStream(originalImage.InputStream);
                //    Image image = ResizeImage(img_Original, size);
                //    image.Save(Path.Combine(targetPath, outputImageName));                    
                //}
                // image.Dispose();
                success = true;
            }
            catch (Exception ex)
            {
                // LogFileWrite(ex);
            }
            return success;
        }

        public static bool ResizeAndUploadImage(string originalImage, string targetPath, Size size, string outputImageName)
        {
            bool success = false;
            try
            {
                System.Drawing.Image img_Original = System.Drawing.Image.FromFile(originalImage);
                Image image = ResizeImage(img_Original, size);
                image.Save(Path.Combine(targetPath, outputImageName));
                image.Dispose();
                success = true;
            }
            catch (Exception ex)
            {
                // LogFileWrite(ex);
            }
            return success;
        }
        private static Image ResizeImage(Image imgPhoto, Size size)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            int Height = size.Height;
            int Width = size.Width;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);
            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }
        #endregion

        public void UploadMemberProfilePic()
        {

        }
    }
}