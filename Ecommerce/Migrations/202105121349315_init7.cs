namespace Ecommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: true));
            AlterColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime(nullable: true));
            DropColumn("dbo.AspNetUsers", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.Int());
        }
    }
}
