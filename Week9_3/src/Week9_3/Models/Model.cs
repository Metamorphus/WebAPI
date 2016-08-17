using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Week9_3.Models
{
    public class CmsContext : DbContext
    {
        public DbSet<Page> Pages { get; set; }
        public DbSet<NavLink> Links { get; set; }
        public DbSet<RelatedPage> RelatedPages { get; set; }

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

            modelBuilder.Entity<NavLink>()
                .HasOne(link => link.ParentPage)
                .WithMany(page => page.OutcomingLinks);

            modelBuilder.Entity<NavLink>()
                .HasOne(link => link.Page)
                .WithMany(page => page.IncomingLinks);

            modelBuilder.Entity<RelatedPage>()
                .HasOne(rel => rel.FirstPage)
                .WithMany(page => page.OutcomingRelations);

            modelBuilder.Entity<RelatedPage>()
                .HasOne(rel => rel.SecondPage)
                .WithMany(page => page.IncomingRelations);
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

        public List<NavLink> OutcomingLinks { get; set; } = new List<NavLink>();
        public List<NavLink> IncomingLinks { get; set; } = new List<NavLink>();

        public List<RelatedPage> OutcomingRelations { get; set; } = new List<RelatedPage>();
        public List<RelatedPage> IncomingRelations { get; set; } = new List<RelatedPage>();
    }

    public class NavLink
    {
        public int NavLinkId { get; set; }

        [Required]
        public int ParentPageId { get; set; }
        [ForeignKey("ParentPageId")]
        public Page ParentPage { get; set; }

        [Required]
        public int PageId { get; set; }
        [ForeignKey("PageId")]
        public Page Page { get; set; }

        public string Title { get; set; }
        public int Position { get; set; }
    }

    public class RelatedPage
    {
        [Key]
        public int RelationId { get; set; }

        [Required]
        public int FirstPageId { get; set; }
        [ForeignKey("FirstPageId")]
        public Page FirstPage { get; set; }

        [Required]
        public int SecondPageId { get; set; }
        [ForeignKey("SecondPageId")]
        public Page SecondPage { get; set; }
    }
}
