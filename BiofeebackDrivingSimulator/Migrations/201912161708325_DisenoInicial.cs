namespace BiofeebackDrivingSimulator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisenoInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Eegs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaHora = c.DateTime(nullable: false),
                        Senal = c.Int(nullable: false),
                        Attention = c.Int(nullable: false),
                        Meditation = c.Int(nullable: false),
                        Delta = c.Int(nullable: false),
                        Theta = c.Int(nullable: false),
                        LowAlpha = c.Int(nullable: false),
                        HighAlpha = c.Int(nullable: false),
                        LowBeta = c.Int(nullable: false),
                        HighBeta = c.Int(nullable: false),
                        LowGamma = c.Int(nullable: false),
                        HighGamma = c.Int(nullable: false),
                        Sesion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sesions", t => t.Sesion_Id)
                .Index(t => t.Sesion_Id);
            
            CreateTable(
                "dbo.FrecuenciaCardiacas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaHora = c.DateTime(nullable: false),
                        Valor = c.Single(nullable: false),
                        Sesion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sesions", t => t.Sesion_Id)
                .Index(t => t.Sesion_Id);
            
            CreateTable(
                "dbo.Sesions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Comentarios = c.String(),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.Temperaturas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaHora = c.DateTime(nullable: false),
                        Valor = c.Single(nullable: false),
                        Sesion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sesions", t => t.Sesion_Id)
                .Index(t => t.Sesion_Id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombres = c.String(nullable: false, maxLength: 500),
                        Apellidos = c.String(),
                        Sexo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sesions", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Temperaturas", "Sesion_Id", "dbo.Sesions");
            DropForeignKey("dbo.FrecuenciaCardiacas", "Sesion_Id", "dbo.Sesions");
            DropForeignKey("dbo.Eegs", "Sesion_Id", "dbo.Sesions");
            DropIndex("dbo.Temperaturas", new[] { "Sesion_Id" });
            DropIndex("dbo.Sesions", new[] { "Usuario_Id" });
            DropIndex("dbo.FrecuenciaCardiacas", new[] { "Sesion_Id" });
            DropIndex("dbo.Eegs", new[] { "Sesion_Id" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.Temperaturas");
            DropTable("dbo.Sesions");
            DropTable("dbo.FrecuenciaCardiacas");
            DropTable("dbo.Eegs");
        }
    }
}
