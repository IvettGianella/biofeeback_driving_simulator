namespace BiofeebackDrivingSimulator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilaMuestraenUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "EsMuestra", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "EsMuestra");
        }
    }
}
