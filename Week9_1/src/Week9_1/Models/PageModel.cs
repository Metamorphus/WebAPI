using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Week9_1.Models
{
    public class CmsContext : DbContext
    {
        public DbSet<Page> Pages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./cms.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Page>()
                .Property(p => p.AddedDate)
                .HasDefaultValueSql("DATE()")
                .ValueGeneratedOnAdd();
        }
    }

    public class Page
    {
        public int PageId { get; set; }

        [Display(Name = "URL")]
        [Required]
        public string UrlName { get; set; }
        [Required]
        public string Content { get; set; }

        public string Description { get; set; }
        public string Title { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Added at")]
        [DataType(DataType.Date)]
        public DateTime AddedDate { get; set; }
    }
}
