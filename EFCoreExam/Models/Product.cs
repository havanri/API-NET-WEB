using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreExam.Models
{
    public class Product : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Precision(10, 0)]
        public decimal Price { get; set; }
        public string? FeatureImageName { get; set; }
        public string? FeatureImagePath { get; set; }
        public string? ShortDescription { get; set; }
        public string? MainDescription { get; set; }
        public string? Slug { get; set; }
        //m-o
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        //o-m
        public List<AlbumImage>? AlbumImages { get; set; }
        //m-m
        public List<ProductTag>? ProductTags { get; set; }
        public List<ProductTaxonomy>? ProductTaxonomies { get; set; }
        public SalePrice? SalePrice { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
