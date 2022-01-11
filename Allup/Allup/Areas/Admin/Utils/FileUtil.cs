using Allup.Areas.Admin.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Admin.Utils
{
    public static class FileUtil
    {
        public static string FileCreate(IFormFile file)
        {

            string path = Path.Combine(FileConstants.ImagePath,Guid.NewGuid() + file.FileName);

            FileStream fs = new FileStream(path, FileMode.Create);

            file.CopyToAsync(fs);

            fs.Close();
            fs.Dispose();

            return file.FileName;
        }

        public static void FileDelete(string path)
        {

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
