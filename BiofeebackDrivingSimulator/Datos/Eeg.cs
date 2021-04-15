using System;

namespace BiofeebackDrivingSimulator.Datos
{
    /// <summary>
    /// Clase modelo de Electroencefalograma
    /// </summary>
    public class Eeg
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int Senal { get; set; }
        public int Attention { get; set; }
        public int Meditation { get; set; }
        public int Delta { get; set; }
        public int Theta { get; set; }
        public int LowAlpha { get; set; }
        public int HighAlpha { get; set; }
        public int LowBeta { get; set; }
        public int HighBeta { get; set; }
        public int LowGamma { get; set; }
        public int HighGamma { get; set; }
    }
}
