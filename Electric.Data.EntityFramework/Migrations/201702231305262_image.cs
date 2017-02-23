namespace ElectricParse.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class image : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        ProductImageId = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(nullable: false, maxLength: 512),
                        Path = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.ProductImageId);
            
            AddColumn("dbo.OrderCategoryProducts", "ProductImageId", c => c.Int());
            CreateIndex("dbo.OrderCategoryProducts", "ProductImageId");
            AddForeignKey("dbo.OrderCategoryProducts", "ProductImageId", "dbo.ProductImages", "ProductImageId");
            DropColumn("dbo.OrderCategoryProducts", "ImageUrl");
            DropColumn("dbo.Products", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ImageUrl", c => c.String(maxLength: 1024));
            AddColumn("dbo.OrderCategoryProducts", "ImageUrl", c => c.String(maxLength: 1024));
            DropForeignKey("dbo.OrderCategoryProducts", "ProductImageId", "dbo.ProductImages");
            DropIndex("dbo.OrderCategoryProducts", new[] { "ProductImageId" });
            DropColumn("dbo.OrderCategoryProducts", "ProductImageId");
            DropTable("dbo.ProductImages");
        }
    }
}
