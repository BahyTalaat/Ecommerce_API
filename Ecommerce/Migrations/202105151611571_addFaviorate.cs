namespace Ecommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFaviorate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FaviorateProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.String(maxLength: 128),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FaviorateProducts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FaviorateProducts", "Product_Id", "dbo.Products");
            DropIndex("dbo.FaviorateProducts", new[] { "Product_Id" });
            DropIndex("dbo.FaviorateProducts", new[] { "User_Id" });
            DropTable("dbo.FaviorateProducts");
        }
    }
}
