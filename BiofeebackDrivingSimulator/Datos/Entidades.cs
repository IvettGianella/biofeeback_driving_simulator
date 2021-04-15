namespace BiofeebackDrivingSimulator.Datos
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Entidades : DbContext
    {
        // El contexto se ha configurado para usar una cadena de conexión 'Entidades' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'BiofeebackDrivingSimulator.Entidades' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'Entidades'  en el archivo de configuración de la aplicación.
        public Entidades()
            : base("name=Entidades")
        {
        }

        // Agregue un DbSet para cada tipo de entidad que desee incluir en el modelo. Para obtener más información 
        // sobre cómo configurar y usar un modelo Code First, vea http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Sesion> Sesiones { get; set; }
        public virtual DbSet<FrecuenciaCardiaca> FrecuenciaCardiacas { get; set; }
        public virtual DbSet<Temperatura> Temperaturas { get; set; }
        public virtual DbSet<Eeg> Eegs { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}