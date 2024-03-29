﻿using Microsoft.AspNetCore.Http;
using StoreSupplier.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StoreSupplier.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(10, ErrorMessage = "Minimum length is 10")]
        public string Description { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "You must chose a category!")]
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category{ get; set; }

        [NotMapped]

        [FileExtension]
        public IFormFile ImageUpload { get; set; }

        public static implicit operator Product(int v)
        {
            throw new NotImplementedException();
        }
    }
}
