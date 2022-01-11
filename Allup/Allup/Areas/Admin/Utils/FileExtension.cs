using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Admin.Utils
{
    public static class FileExtension
    {
        public static bool IsContains(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }

        public static bool IsRightSize(this IFormFile file, int kb)
        {
            return file.Length > 1024 * kb;
        }
    }
}
