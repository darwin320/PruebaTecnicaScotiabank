using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Test.Entidades
{
    [Table("turnos", Schema = "dbo")]
    public class Turno
    {
        /// <summary>
        /// Identificador del turno.
        /// </summary>
        [Key]
        [Column("id_turno")]
        public int id_turno { get; set; }

        /// <summary>
        /// Identificador del servicio al que pertenece el turno.
        /// </summary>
        [Column("id_servicio")]
        [Required(ErrorMessage = "El ID del servicio es un campo requerido.")]
        public int id_servicio { get; set; }

        /// <summary>
        /// Fecha del turno.
        /// </summary>
        [Column("fecha_turno")]
        [Required(ErrorMessage = "La fecha del turno es un campo requerido.")]
        public DateTime fecha_turno { get; set; }

        /// <summary>
        /// Hora de inicio del turno.
        /// </summary>
        [Column("hora_inicio")]
        [Required(ErrorMessage = "La hora de inicio es un campo requerido.")]
        public TimeSpan hora_inicio { get; set; }

        /// <summary>
        /// Hora de fin del turno.
        /// </summary>
        [Column("hora_fin")]
        [Required(ErrorMessage = "La hora de fin es un campo requerido.")]
        public TimeSpan hora_fin { get; set; }

        /// <summary>
        /// Estado del turno (e.g., Disponible, Reservado).
        /// </summary>
        [Column("estado")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El estado del turno es un campo requerido.")]
        [StringLength(20)]
        public string estado { get; set; }

        /// <summary>
        /// Servicio al que pertenece el turno.
        /// </summary>
        [ForeignKey("id_servicio")]
        public Servicio servicio { get; set; }
    }
}
