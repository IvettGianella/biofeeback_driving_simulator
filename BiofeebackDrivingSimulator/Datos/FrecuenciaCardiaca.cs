using System;

namespace BiofeebackDrivingSimulator.Datos
{
    /// <summary>
    /// Clase modelo de Ritmo Cardiaco
    /// </summary>
    public class FrecuenciaCardiaca
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public float Valor { get; set; }
    }
}
