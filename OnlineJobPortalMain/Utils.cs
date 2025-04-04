using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineJobPortalMain
{
    public class Utils
    {
        public static bool IsValidExtension(string fileName)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            foreach (string ext in allowedExtensions)
            {
                if (fileName.ToLower().EndsWith(ext))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsValidExtension4Resume(string fileName)
        {
            bool isValid = false;
            string[] allowedExtensions = { ".doc", ".docx", ".pdf"};
            for(int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileName.Contains(allowedExtensions[i])) 
                {
                    isValid = true;
                    break;
                }
            }
          return isValid;
        }
    }
}