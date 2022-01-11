using Allup.Models.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Admin.ViewModel
{
    public class CategoryCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile File { get; set; }
        public bool IsMain { get; set; }
        public int ParentId { get; set; }
    }
}
