namespace UnitOfWork.NET.EntityFramework.NUnit.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Startup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Role",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Description = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.UserRole",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    IdUser = c.Int(nullable: false),
                    IdRole = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.IdRole, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser)
                .Index(t => t.IdRole);

            CreateTable(
                "dbo.User",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Login = c.String(nullable: false, maxLength: 50),
                    Password = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "IdUser", "dbo.User");
            DropForeignKey("dbo.UserRole", "IdRole", "dbo.Role");
            DropIndex("dbo.UserRole", new[] { "IdRole" });
            DropIndex("dbo.UserRole", new[] { "IdUser" });
            DropTable("dbo.User");
            DropTable("dbo.UserRole");
            DropTable("dbo.Role");
        }
    }
}
