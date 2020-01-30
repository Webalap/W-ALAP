using Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static ExigoImageApi Images()
        {
            return new ExigoImageApi();
        }
    }

    public class AvatarResponse
    {
        public byte[] Bytes { get; set; }
        public string FileType { get; set; }
    }

    public sealed class ExigoImageApi
    {
        private string LoginName = GlobalSettings.Exigo.Api.LoginName;
        private string Password = GlobalSettings.Exigo.Api.Password;

        public AvatarResponse GetCustomerAvatarResponse(int customerID, string filename)
        {
            var response = new AvatarResponse();

            var path = $"/customers/{customerID.ToString()}/avatars";

            byte[] bytes;

            using (var conn = Exigo.Sql())
            {
                conn.Open();

                var cmd = new SqlCommand(@"
			    SELECT
				TOP 1 ImageData 
			    FROM 
				ImageFiles 
			    WHERE 
				Path=@FilePath 
				AND Name=@FileName
			    ", conn);
                cmd.Parameters.Add("@FilePath", System.Data.SqlDbType.NVarChar, 500).Value = path;
                cmd.Parameters.Add("@FileName", System.Data.SqlDbType.NVarChar, 500).Value = filename;
                bytes = (byte[])cmd.ExecuteScalar();
            }
            response.Bytes = bytes;

            var extension = Path.GetExtension(filename).ToLower();
            string contentType = "image/jpeg";
            switch (extension)
            {
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".jpeg":
                    contentType = "image/png";
                    break;
                case ".bmp":
                    contentType = "image/bmp";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".jpg":
                default:
                    contentType = "image/jpeg";
                    break;
            }
            response.FileType = contentType;
            return response;
        }

        public AvatarResponse GetCustomerAvatarResponse(int customerID, AvatarType type, bool cache = true, byte[] bytes = null)
        {
            var response = new AvatarResponse();

            var path = "/customers/" + customerID.ToString();
            var filename = "avatar";
            switch (type)
            {
                case AvatarType.Tiny: filename += "-xs"; break;
                case AvatarType.Small: filename += "-sm"; break;
                case AvatarType.Large: filename += "-lg"; break;
            }

            // All images set to png, so we have to have this work around for now
            filename = filename + ".png";
            if (bytes == null)
            {
                using (var conn = Exigo.Sql())
                {
                    conn.Open();

                    var cmd = new SqlCommand(@"
						SELECT 
							TOP 1 ImageData 
						FROM
							ImageFiles 
						WHERE
							Path=@FilePath 
							AND Name=@FileName
						", conn);
                    cmd.Parameters.Add("@FilePath", System.Data.SqlDbType.NVarChar, 500).Value = path;
                    cmd.Parameters.Add("@FileName", System.Data.SqlDbType.NVarChar, 500).Value = filename;
                    bytes = (byte[])cmd.ExecuteScalar();
                }
            }

            response.Bytes = bytes;

            // If we didn't find anything there, convert the default image (which is Base64) to a byte array.
            // We'll use that instead
            if (bytes == null)
            {
                bytes = Convert.FromBase64String(GlobalSettings.Avatars.DefaultAvatarAsBase64);


                return GetCustomerAvatarResponse(customerID, type, cache, GlobalUtilities.ResizeImage(bytes, type));
            }
            else
            {

                var extension = Path.GetExtension(filename).ToLower();
                string contentType = "image/jpeg";
                switch (extension)
                {
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".jpeg":
                        contentType = "image/png";
                        break;
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".jpg":
                    default:
                        contentType = "image/jpeg";
                        break;
                }

                response.FileType = contentType;
            }


            return response;
        }

        //public byte[] GetCustomerAvatar(int customerID, AvatarType type, bool cache = true)
        //{
        //    var bytes = new byte[0];


        //    // Try to return the image found at the avatar path
        //    bytes = GlobalUtilities.GetExternalImageBytes(GlobalUtilities.GetCustomerAvatarUrl(customerID, type, cache));


        //    // If we didn't find anything there, convert the default image (which is Base64) to a byte array.
        //    // We'll use that instead
        //    if (bytes == null || bytes.Length == 0)
        //    {
        //        Exigo.Images().SetCustomerAvatar(customerID, Convert.FromBase64String(GlobalSettings.Avatars.DefaultAvatarAsBase64));
        //        return GetCustomerAvatar(customerID, type, cache);
        //    }

        //    return bytes;
        //}

        //public bool SaveUncroppedCustomerAvatar(int customerID, byte[] bytes)
        //{
        //    // Define the customer profile settings
        //    var path = "customers/{0}".FormatWith(customerID);
        //    var filename = "avatar-raw.png";
        //    var maxWidth = 500;
        //    var maxHeight = 500;

        //    // Resize the image
        //    var resizedBytes = GlobalUtilities.ResizeImage(bytes, maxWidth, maxHeight);

        //    // Save the image
        //    return SaveImage(path, filename, resizedBytes);
        //}
        public bool SetCustomerAvatar(int customerID, byte[] bytes, bool saveToHistory = false)
        {
            //// Define the customer profile settings
            //var path      = "customers/{0}".FormatWith(customerID);

            //// Resize and save the images for each of the sizes
            //var result = false;
            //result = SaveImage(path, "avatar-lg.png", GlobalUtilities.ResizeImage(bytes, 300, 300));
            //result = SaveImage(path, "avatar.png", GlobalUtilities.ResizeImage(bytes, 100, 100));
            //result = SaveImage(path, "avatar-sm.png", GlobalUtilities.ResizeImage(bytes, 50, 50));
            //result = SaveImage(path, "avatar-xs.png", GlobalUtilities.ResizeImage(bytes, 16, 16));

            //// Save the image to the avatar history if applicable
            //if (result && saveToHistory)
            //{
            //    SaveCustomerAvatarToHistory(customerID, GlobalUtilities.ResizeImage(bytes, 300, 300));
            //}

            //return result;
            bool result = ((GlobalUtilities.InsertOrUpdateAvatarToAPI(customerID, bytes) ? GlobalUtilities.InsertOrUpdateAvatarToReportingDatabase(customerID, bytes) : false));
            if (result && saveToHistory)
            {
                SaveCustomerAvatarToHistory(customerID, bytes);
            }
            return result;
        }
        public bool SaveCustomerAvatarToHistory(int customerID, byte[] bytes)
        {
            //// Define the customer profile settings
            //var path = "customers/{0}/avatars".FormatWith(customerID);
            //var filename = "{0}.png".FormatWith(Path.GetRandomFileName());
            //var maxWidth = 300;
            //var maxHeight = 300;

            //// Resize the image
            //var resizedBytes = GlobalUtilities.ResizeImage(bytes, maxWidth, maxHeight);

            //// Determine if this avatar has already been uploaded before by looking at it's size
            //List<ImageFile> images;
            //using (var context = Exigo.Sql())
            //{
            //    string sqlProcedure = string.Format("GetImagesByPath '{0}'", path);
            //    images = context.Query<ImageFile>(sqlProcedure).ToList();
            //}
            //bool isUnique = images.Any(s => s.Size == resizedBytes.Length);
            //// Save the image
            //return (isUnique) ? SaveImage(path, filename, resizedBytes) : false;

            // Define the customer profile settings
            var path = $"/customers/{customerID}/avatars";
            var filename = $"{Path.GetRandomFileName()}.png";
            const int maxWidth = 300;
            const int maxHeight = 300;

            // Resize the image
            var resizedBytes = GlobalUtilities.ResizeImage(bytes, maxWidth, maxHeight);
            int size = resizedBytes.Length;

            bool result = false;

            using (var conn = Exigo.Sql())
            {
                conn.Open();
                //Check if this historical avatar exists by checking the customerID and filesize
                if (!GlobalUtilities.AvatarExists(customerID))
                {   //save the image
                    var cmd = new SqlCommand(@"
				INSERT INTO ImageFiles 
				(
				    [Path]
				    ,[Name]
				    ,[ModifiedDate]
				    ,[Size]
				    ,[ImageData]
				) 
				VALUES 
				(
				    @path
				    ,@filename
				    ,@modifiedDate
				    ,@size
				    ,@file
				)
				SELECT @@ROWCOUNT;
				", conn);

                    cmd.Parameters.Add("@path", System.Data.SqlDbType.NVarChar, 500).Value = path;
                    cmd.Parameters.Add("@filename", System.Data.SqlDbType.NVarChar, 500).Value = filename;
                    cmd.Parameters.Add("@modifiedDate", System.Data.SqlDbType.DateTime, 500).Value = DateTime.Now;
                    cmd.Parameters.Add("@size", System.Data.SqlDbType.NVarChar, 500).Value = size;
                    cmd.Parameters.Add("@file", System.Data.SqlDbType.NVarChar, 500).Value = resizedBytes;


                    try
                    {
                        bool savedToApi = SaveImage(path, filename, resizedBytes);
                        bool savedToReportingDB = (bool)cmd.ExecuteScalar();

                        result = savedToApi && savedToReportingDB;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return result;
        }

        public List<string> GetCustomerAvatarHistory(int customerID)
        {
            //var path = "/customers/{0}/avatars".FormatWith(customerID);
            //List<ImageFile> images;
            //using (var context = Exigo.Sql())
            //{
            //    string sqlProcedure = string.Format("GetImagesByPath '{0}'", path);
            //    images = context.Query<ImageFile>(sqlProcedure).ToList();
            //    images.ForEach(s => s.Url = string.Format("http://indiahicks-api.exigo.com/4.0/IndiaHicks/images{0}/{1}", s.Path, s.FileName));
            //}
            //return images.Select(c => c.Url).ToList();

            var path = $"/customers/{customerID}/avatars";

            List<string> imagesNames;
            using (var ctx = Exigo.Sql())
            {
                imagesNames = ctx.Query<string>(@"
			    SELECT
				Name
			    FROM
				ImageFiles
			    WHERE
				Path = @path
			    ORDER BY 
				ModifiedDate DESC
			", new
                {
                    path
                }).ToList();
            }

            var urls = imagesNames.Select(imageName =>
            $"/profiles/historicalavatar/{customerID}/{imageName}").ToList();

            return urls;
        }

        public bool SaveImage(string path, string filename, byte[] bytes)
        {
            //if (path.StartsWith("/")) path = path.Remove(0, 1);
            //if (path.EndsWith("/")) path = path.Remove(path.Length - 1, 1);

            //var url = "http://{0}.exigo.com/4.0/{1}/images/{2}/{3}".FormatWith(
            //    GlobalSettings.Exigo.Api.GetSubdomain(),
            //    GlobalSettings.Exigo.Api.CompanyKey,
            //    path,
            //    filename);

            //var request = (HttpWebRequest)WebRequest.Create(url);
            //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(LoginName + ":" + Password)));
            //request.Method = "POST";
            //request.ContentLength = bytes.Length;
            //var writer = request.GetRequestStream();
            //writer.Write(bytes, 0, bytes.Length);
            //writer.Close();

            //var response = (HttpWebResponse)request.GetResponse();

            //return response.StatusCode == HttpStatusCode.OK;

            try
            {

                Exigo.WebService().SetImageFile(new Common.Api.ExigoWebService.SetImageFileRequest
                {
                    Name = filename,
                    Path = path,
                    ImageData = bytes
                });
            }
            catch
            {
                return false;
            }

            // If we got here, everything should be good
            return true;
        }
    }
}