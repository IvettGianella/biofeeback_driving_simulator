using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiofeebackDrivingSimulator.Datos
{
    /// <summary>
    /// Clase modelo para Usuario usando DataAnnotations
    /// </summary>
    public class Usuario
    {
        public int Id { get; set; }

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(500, ErrorMessage = "El campo {0} tiene un tamaño maximo de {1} caracteres.")]
        public string Nombres { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(500, ErrorMessage = "El campo {0} tiene un tamaño maximo de {1} caracteres.")]
        public string Apellidos { get; set; }

        [Display(Name = "Sexo")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public bool Sexo { get; set; }

        [Display(Name = "Edad")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int Edad { get; set; }

        [Display(Name = "Muestra")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public bool EsMuestra { get; set; }

        public virtual ICollection<Sesion> Sesiones { get; set; }
    }
}
