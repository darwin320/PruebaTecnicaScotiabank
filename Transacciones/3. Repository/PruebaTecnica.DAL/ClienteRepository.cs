using System.Data;
using System.Data.SqlClient;

namespace PruebaTecnica.Test.DAL
{
    public partial class ClienteRepository
    {
        #region Propiedades
        private string ConnectionString { get; set; }
        #endregion Propiedades

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public ClienteRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Permite obtener la información de un cliente especifico a partir de su identificador.
        /// </summary>
        /// <param name="idcliente"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable GetCliente(int idcliente)
        {
            DataTable dtCliente = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            using (SqlCommand cmd = new SqlCommand())
            {
                using (SqlConnection cn = new SqlConnection(this.ConnectionString))
                {
                    try
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "procClienteSelectById";
                        cmd.Parameters.Add(new SqlParameter("@@idcliente", idcliente));
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dtCliente);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudo obtener la información del cliente, Intentelo nuevamente", ex);
                    }
                }
            }
            return dtCliente;
        }

        /// <summary>
        /// Permite obtener la información de un cliente especifico a partir de su apikey.
        /// </summary>
        /// <param name="apikey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable GetCliente(string apikey)
        {
            DataTable dtCliente = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            using (SqlCommand cmd = new SqlCommand())
            {
                using (SqlConnection cn = new SqlConnection(this.ConnectionString))
                {
                    try
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "procClienteSelectByapikey";
                        cmd.Parameters.Add(new SqlParameter("@@apikey", apikey));
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dtCliente);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudo obtener la información del cliente, Intentelo nuevamente", ex);
                    }
                }
            }
            return dtCliente;
        }

        /// <summary>
        /// Permite obtener la información de los clientes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable GetClientes()
        {
            DataTable dtCliente = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            using (SqlCommand cmd = new SqlCommand())
            {
                using (SqlConnection cn = new SqlConnection(this.ConnectionString))
                {
                    try
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "procClienteSelect";
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dtCliente);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudo obtener la información de los clientes, Intentelo nuevamente", ex);
                    }
                }
            }
            return dtCliente;
        }
    }
}