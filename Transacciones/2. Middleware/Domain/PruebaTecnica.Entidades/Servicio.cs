using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Test.Entidades
{
    [Table("servicios", Schema = "dbo")]
    public class Servicio
    {
        /// <summary>
        /// Identificador del servicio.
        /// </summary>
        [Key]
        [Column("id_servicio")]
        public int id_servicio { get; set; }

        /// <summary>
        /// Identificador del comercio que ofrece el servicio.
        /// </summary>
        [Column("id_comercio")]
        [Required(ErrorMessage = "El ID del comercio es un campo requerido.")]
        public int id_comercio { get; set; }

        /// <summary>
        /// Nombre del servicio.
        /// </summary>
        [Column("nom_servicio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del servicio es un campo requerido.")]
        [StringLength(100)]
        public string nom_servicio { get; set; }

        /// <summary>
        /// Hora de apertura del servicio.
        /// </summary>
        [Column("hora_apertura")]
        [Required(ErrorMessage = "La hora de apertura es un campo requerido.")]
        public TimeSpan hora_apertura { get; set; }

        /// <summary>
        /// Hora de cierre del servicio.
        /// </summary>
        [Column("hora_cierre")]
        [Required(ErrorMessage = "La hora de cierre es un campo requerido.")]
        public TimeSpan hora_cierre { get; set; }

        /// <summary>
        /// Duración del servicio en minutos.
        /// </summary>
        [Column("duracion")]
        [Required(ErrorMessage = "La duración es un campo requerido.")]
        public int duracion { get; set; }

        /// <summary>
        /// Comercio que ofrece el servicio.
        /// </summary>
        [ForeignKey("id_comercio")]
        public Comercio comercio { get; set; }

        /// <summary>
        /// Colección de turnos disponibles para el servicio.
        /// </summary>
        public ICollection<Turno> Turnos { get; set; }
    }
}
