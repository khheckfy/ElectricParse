using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ElectricParse.Domain.Entities;

namespace ElectricParse.Data.EntityFramework
{
    public partial class ModelDataContext : DbContext
    {
        public ModelDataContext()
            : base("name=DB")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderCategory> OrderCategories { get; set; }
        public virtual DbSet<OrderCategoryProduct> OrderCategoryProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
