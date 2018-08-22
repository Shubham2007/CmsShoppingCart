using CmsShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.ViewModels.Pages
{
    public class PagesVM
    {
        public PagesVM()
        {
        }

        public PagesVM(PagesDTO row)
        {
            Id = row.Id;
            Slug = row.Slug;
            Body = row.Body;
            Title = row.Title;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;
        }

        public int Id { get; set; }
        public string Slug { get; set; }

        [Required(ErrorMessage = "Body Required")]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Body { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        public int Sorting { get; set; }
        public bool HasSidebar { get; set; }
    }
}