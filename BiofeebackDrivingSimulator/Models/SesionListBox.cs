using System;

namespace BiofeebackDrivingSimulator.Models
{
    /// <summary>
    /// Clase para tener datos planos de las sesiones registradas
    /// </summary>
    public class SesionListBox
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Cometario { get; set; }
    }
}
