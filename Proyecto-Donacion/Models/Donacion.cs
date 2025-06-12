using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Proyecto_Donacion.Models
{
    public class Donacion
    {
        // Propiedades que reflejan las columnas de la tabla Donacion
        public int ID_donaciones { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int ID_beneficiario { get; set; }

        // Constructor vacío
        public Donacion() { }

        // ===================== MÉTODOS CRUD =====================

        /// <summary>Crea una nueva donación en la base de datos.</summary>
        public static int CrearDonacion(decimal Monto, DateTime Fecha, int ID_Beneficiario)
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarDonacion", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MONTO", Monto);
                cmd.Parameters.AddWithValue("@FECHA", Fecha);
                cmd.Parameters.AddWithValue("@ID_BENEFICIARIO", ID_Beneficiario);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        /// <summary>Obtiene una donación por su ID.</summary>
        public static Donacion ObtenerPorID(int ID_Donaciones)
        {
            Donacion dona = null;
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerDonacionPorID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_DONACIONES", ID_Donaciones);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dona = new Donacion
                    {
                        ID_donaciones = (int)reader["ID_DONACIONES"],
                        Monto = (decimal)reader["MONTO"],
                        Fecha = (DateTime)reader["FECHA"],
                        ID_beneficiario = (int)reader["ID_BENEFICIARIO"]
                    };
                }
                reader.Close();
            }
            return dona;
        }

        /// <summary>Obtiene todas las donaciones.</summary>
        public static List<Donacion> ObtenerTodos()
        {
            List<Donacion> lista = new List<Donacion>();
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerTodasLasDonaciones", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Donacion dona = new Donacion
                    {
                        ID_donaciones = (int)reader["ID_DONACIONES"],
                        Monto = (decimal)reader["MONTO"],
                        Fecha = (DateTime)reader["FECHA"],
                        ID_beneficiario = (int)reader["ID_BENEFICIARIO"]
                    };
                    lista.Add(dona);
                }
                reader.Close();
            }
            return lista;
        }

        /// <summary>Actualiza una donación existente.</summary>
        public static bool ActualizarDonacion(int ID_donacion, decimal monto, DateTime fecha, int ID_beneficiario)
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarDonacion", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_DONACIONES", ID_donacion);
                cmd.Parameters.AddWithValue("@MONTO", monto);
                cmd.Parameters.AddWithValue("@FECHA", fecha);
                cmd.Parameters.AddWithValue("@ID_BENEFICIARIO", ID_beneficiario);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return (rows > 0);
            }
        }

        /// <summary>Elimina una donación por su ID.</summary>
        public static bool EliminarDonacion(int ID_Donaciones)
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarDonacion", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_DONACIONES", ID_Donaciones);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return (rows > 0);
            }
        }
    }
}
