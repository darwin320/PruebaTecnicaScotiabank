using PruebaTecnica.Test.DAL;
using PruebaTecnica.Test.Entidades;
using PruebaTecnica.Test.Bll.Utils;

namespace PruebaTecnica.Test.Bll
{
    public partial class ClienteBll
    {
        #region Propiedades
        private string ConnectionString { get; set; }

        private readonly ClienteRepository _ClienteRepository;

        #endregion Propiedades

        public ClienteBll(string connectionString)
        {
            this.ConnectionString = connectionString;
            this._ClienteRepository = new ClienteRepository(this.ConnectionString);
        }

        /// <summary>
        /// Permite obtener la información de un cliente especifico a partir de su identificador.
        /// </summary>
        /// <param name="idcliente"></param>
        /// <returns></returns>
        public Cliente GetCliente(int idcliente)
        {
            Cliente cliente = new();
            var dtCliente = this._ClienteRepository.GetCliente(idcliente);
            if (dtCliente!.HasData())
            {
                cliente = dtCliente.ToEntityClass<Cliente>().FirstOrDefault();
            }
            return cliente;
        }

        /// <summary>
        /// Permite obtener la información de un cliente especifico a partir de su apikey.
        /// </summary>
        /// <param name="apikey"></param>
        /// <returns></returns>
        public Cliente GetCliente(string apikey)
        {
            Cliente cliente = new();
            var dtCliente = this._ClienteRepository.GetCliente(apikey);
            if (dtCliente!.HasData())
            {
                cliente = dtCliente.ToEntityClass<Cliente>().FirstOrDefault();
            }
            return cliente;
        }

        /// <summary>
        /// Permite obtener la información de los clientes.
        /// </summary>
        /// <returns></returns>
        public List<Cliente> GetClientes()
        {
            List<Cliente> clientes = new();
            var dtCliente = this._ClienteRepository.GetClientes();
            if (dtCliente!.HasData())
            {
                clientes = dtCliente.ToEntityClass<Cliente>();
            }
            return clientes;
        }
    }
}