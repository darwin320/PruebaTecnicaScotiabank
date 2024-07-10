using PruebaTecnica.Test.DAL.Interfaces;
using PruebaTecnica.Test.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PruebaTecnica.Test.DAL
{
    public class TurnosRepository : ITurnosRepository
    {
        #region Propiedades
        private string ConnectionString { get; set; }
        #endregion Propiedades

        public TurnosRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Genera turnos en la base de datos según los parámetros proporcionados.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio para la generación de turnos</param>
        /// <param name="fechaFin">Fecha de fin para la generación de turnos</param>
        /// <param name="idServicio">ID del servicio para el cual se generarán los turnos</param>
        /// <returns>Lista de turnos generados</returns>
        /// <exception cref="Exception"></exception>
        public List<Turno> GenerarTurnos(DateTime fechaInicio, DateTime fechaFin, int idServicio)
        {
            DataTable dtTurnos = this.GenerarTurnosDataTable(fechaInicio, fechaFin, idServicio);
            List<Turno> turnos = new List<Turno>();

            // Verificación de llenado del DataTable
            Console.WriteLine($"Número de filas en dtTurnos: {dtTurnos.Rows.Count}");

            if (dtTurnos.Rows.Count == 0)
            {
                Console.WriteLine("No se generaron turnos.");
                return turnos;
            }

            foreach (DataRow row in dtTurnos.Rows)
            {
                Console.WriteLine($"Procesando fila: {row["id_turno"]}");
                Turno turno = new Turno
                {
                    id_turno = Convert.ToInt32(row["id_turno"]),
                    id_servicio = Convert.ToInt32(row["id_servicio"]),
                    fecha_turno = Convert.ToDateTime(row["fecha_turno"]),
                    hora_inicio = TimeSpan.Parse(row["hora_inicio"].ToString()),
                    hora_fin = TimeSpan.Parse(row["hora_fin"].ToString()),
                    estado = row["estado"].ToString()
                };

                turno.servicio = this.ObtenerServicio(turno.id_servicio);

                turnos.Add(turno);
            }

            Console.WriteLine($"Número de turnos generados: {turnos.Count}");
            return turnos;
        }

        private DataTable GenerarTurnosDataTable(DateTime fechaInicio, DateTime fechaFin, int idServicio)
        {
            DataTable dtTurnos = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            using (SqlCommand cmd = new SqlCommand())
            {
                using (SqlConnection cn = new SqlConnection(this.ConnectionString))
                {
                    try
                    {
                        cn.Open(); // Abre la conexión antes de configurar el comando
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "GenerarTurnos";
                        cmd.Parameters.AddWithValue("@fecha_inicio", fechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"));
                        cmd.Parameters.AddWithValue("@fecha_fin", fechaFin.ToString("yyyy-MM-ddTHH:mm:ss"));
                        cmd.Parameters.AddWithValue("@id_servicio", idServicio);
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dtTurnos);

                        // Verificación adicional
                        if (dtTurnos.Rows.Count == 0)
                        {
                            Console.WriteLine("No se generaron turnos.");
                        }
                        else
                        {
                            Console.WriteLine($"{dtTurnos.Rows.Count} turnos generados.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudo generar los turnos, inténtelo nuevamente", ex);
                    }
                }
            }
            return dtTurnos;
        }

        private Servicio ObtenerServicio(int idServicio)
        {
            Servicio servicio = null;
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM servicios WHERE id_servicio = @id_servicio", cn))
                {
                    cmd.Parameters.AddWithValue("@id_servicio", idServicio);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                servicio = new Servicio
                                {
                                    id_servicio = Convert.ToInt32(reader["id_servicio"]),
                                    id_comercio = Convert.ToInt32(reader["id_comercio"]),
                                    nom_servicio = reader["nom_servicio"].ToString(),
                                    hora_apertura = TimeSpan.Parse(reader["hora_apertura"].ToString()),
                                    hora_cierre = TimeSpan.Parse(reader["hora_cierre"].ToString()),
                                    duracion = Convert.ToInt32(reader["duracion"]),
                                    comercio = ObtenerComercio(Convert.ToInt32(reader["id_comercio"]))
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudo cargar el servicio relacionado, inténtelo nuevamente", ex);
                    }
                }
            }
            return servicio;
        }

        private Comercio ObtenerComercio(int idComercio)
        {
            Comercio comercio = null;
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM comercios WHERE id_comercio = @id_comercio", cn))
                {
                    cmd.Parameters.AddWithValue("@id_comercio", idComercio);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                comercio = new Comercio
                                {
                                    id_comercio = Convert.ToInt32(reader["id_comercio"]),
                                    nom_comercio = reader["nom_comercio"].ToString(),
                                    aforo_maximo = Convert.ToInt32(reader["aforo_maximo"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudo cargar el comercio relacionado, inténtelo nuevamente", ex);
                    }
                }
            }
            return comercio;
        }
    }
}
