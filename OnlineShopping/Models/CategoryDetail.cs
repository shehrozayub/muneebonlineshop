using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Models
{
    public class CategoryDetail
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Minimum 3 and Minimum 5 and Maximum 100 characters are allowed", MinimumLength = 3)]
        [System.Web.Mvc.Remote("CheckCategoryExist", "Admin", ErrorMessage = "Category already exist")]
        public string CategoryName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }

    public class ProductDetail
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Minimum 3 and Minimum 5 and Maximum 100 characters are allowed", MinimumLength = 3)]
        [System.Web.Mvc.Remote("CheckProductExist", "Admin", ErrorMessage = "Product already exist")]
        public string ProductName { get; set; }
        [Required]
        [Range(1, 50)]
        public Nullable<int> CategoryId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [Required]
        public string Description { get; set; }
        public string ProductImage { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(typeof(decimal), "1", "200000", ErrorMessage = "Invalid price")]
        public decimal Price { get; set; }

        public bool IsFeatured { get; set; }

        public SelectList Categories { get; set; }
    }
}