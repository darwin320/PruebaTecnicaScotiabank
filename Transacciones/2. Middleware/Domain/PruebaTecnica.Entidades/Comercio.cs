using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Test.Entidades
{
    [Table("comercios", Schema = "dbo")]
    public class Comercio
    {
        /// <summary>
        /// Identificador del comercio.
        /// </summary>
        [Key]
        [Column("id_comercio")]
        public int id_comercio { get; set; }

        /// <summary>
        /// Nombre del comercio.
        /// </summary>
        [Column("nom_comercio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del comercio es un campo requerido.")]
        [StringLength(100)]
        public string nom_comercio { get; set; }

        /// <summary>
        /// Aforo máximo del comercio.
        /// </summary>
        [Column("aforo_maximo")]
        [Required(ErrorMessage = "El aforo máximo es un campo requerido.")]
        public int aforo_maximo { get; set; }

        /// <summary>
        /// Colección de servicios que ofrece el comercio.
        /// </summary>
        public ICollection<Servicio> Servicios { get; set; }
    }
}
