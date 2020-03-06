using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace bug_tracker.Helper
{
    public class AttachmentHelper
    {
        public static string GetDefaultIcon(string path)
        {
            var defaultPath = "/Images/file.png";
            switch (Path.GetExtension(path))
            {
                case ".pdf":
                    defaultPath = "/Images/pdf.png";
                    break;
                case ".doc":
                case ".docx":
                    defaultPath = "/Images/doc.png";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".bmp":
                case ".gif":
                case ".tif":
                    defaultPath = "/Images/jpg.png";
                    break;
            }
            return defaultPath;
        }
    }
}