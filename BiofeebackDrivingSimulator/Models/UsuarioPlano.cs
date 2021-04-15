namespace BiofeebackDrivingSimulator.Models
{
    /// <summary>
    /// Clase de datos planos del usario para presentarlo en pantalla
    /// </summary>
    public class UsuarioPlano
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Sexo { get; set; }
        public int Edad { get; set; }
        public int Sesiones { get; set; }
    }
}
