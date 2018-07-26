using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    /// <summary>
    /// 相当于一个属性
    /// 可以直接返回
    /// 
    /// https://forums.asp.net/t/1981336.aspx?Storing+Image+Url+in+Database
    /// https://stackoverflow.com/questions/4896640/net-load-file-to-httppostedfilebase
    /// http://www.prideparrot.com/blog/archive/2012/8/uploading_and_returning_files
    /// </summary>
    [NotMapped]
    public class ImageFileManager
    {
        /// <summary>
        /// ID
        /// </summary>
        private const string BASE_PATH = "~/Uploads/Images/";

        private static string GetPath(string fileName)
        {
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(BASE_PATH), fileName);
            return path;
        }
        
        public static string Upload(HttpPostedFile file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileName = null;
                try
                {
                    var ext = Path.GetExtension(file.FileName);
                    var onlyName = Path.GetFileNameWithoutExtension(file.FileName);
                    fileName = Path.GetFileName(Base64Encode(onlyName) + ext);
                    
                    var path = GetPath(fileName);
                    System.Diagnostics.Debug.WriteLine(path);
                    file.SaveAs(path);
                    
                } catch
                {

                }
                return fileName;
            } else
            {
                // 图片上传错误
                return null;
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static FileStream Load(string fileName)
        {
            var path = GetPath(fileName);
            try
            {
                var fs = new FileStream(path, FileMode.Open);
                return fs;
            } catch (FileNotFoundException)
            {
                return null;
            }
            
            
        }
    }

    public class Image
    {
        /// <summary>
        /// Id of Imgs
        /// </summary>
        [Key]
        public int ImageID { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public bool Local { get; set; }

        public Image()
        {
            Local = false;
        }

        [NotMapped]
        public string ImageShownURL
        {
            get
            {
                if (!Local)
                {
                    return ImageURL;
                } else
                {
                    return "/Uploads/Images/" + ImageURL;
                }
            }
        }
    }
}