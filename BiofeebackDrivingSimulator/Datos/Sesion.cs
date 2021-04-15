using System;
using System.Collections.Generic;

namespace BiofeebackDrivingSimulator.Datos
{
    /// <summary>
    /// Clase modelo de una sesión de registro de
    /// caracteristicas fisiologicas
    /// </summary>
    public class Sesion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentarios { get; set; }

        public virtual ICollection<FrecuenciaCardiaca> FrecuenciaCardiacas { get; set; }
        public virtual ICollection<Temperatura> Temperaturas { get; set; }
        public virtual ICollection<Eeg> Eegs { get; set; }
    }
}
