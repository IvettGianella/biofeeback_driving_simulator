namespace BiofeebackDrivingSimulator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrecionUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "Edad", c => c.Int(nullable: false));
            AlterColumn("dbo.Usuarios", "Apellidos", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuarios", "Apellidos", c => c.String());
            DropColumn("dbo.Usuarios", "Edad");
        }
    }
}
