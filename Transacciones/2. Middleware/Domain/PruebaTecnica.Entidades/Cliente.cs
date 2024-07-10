using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Test.Entidades
{
    [Table("clientes", Schema = "dbo")]
    public class Cliente
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("idcliente")]
        public int idcliente { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("nombre")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "nombre es un campo requerido")]
        [StringLength(250)]
        public string nombre { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("apikey")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "apikey es un campo requerido")]
        [StringLength(50)]
        public string apikey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("codigo_convenio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "codigo convenio es un campo requerido")]
        [StringLength(10)]
        public string codigo_convenio { get; set; }
    }
}
