using System;

namespace BiofeebackDrivingSimulator.Datos
{
    /// <summary>
    /// Clase modelo de Temperatura
    /// </summary>
    public class Temperatura
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public float Valor { get; set; }
    }
}
