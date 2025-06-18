using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Proyecto_Donacion.Models
{
    public class Donacion
    {
        public int ID_donaciones { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int ID_BENEFICIARIOS { get; set; }
        public int UsuarioID { get; set; }

        public Donacion() { }

        /// <summary>Obtiene el ID del donante desde el UsuarioID.</summary>
        private static int ObtenerIdDonante(int usuarioId)
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID_DONANTES FROM DONANTES WHERE UsuarioID = @UsuarioID", conn);
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        /// <summary>Crea una nueva donación en la base de datos.</summary>
        public static int CrearDonacion(decimal Monto, DateTime Fecha, int UsuarioID)
        {
            int idDonante = ObtenerIdDonante(UsuarioID);
            if (idDonante == -1)
                throw new Exception("No se encontró un donante vinculado a este usuario.");

            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarDonacion", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MONTO", Monto);
                cmd.Parameters.AddWithValue("@FECHA", Fecha);
                cmd.Parameters.AddWithValue("@ID_DONANTES", idDonante);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
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
                        UsuarioID = ObtenerUsuarioIDDesdeDonanteID((int)reader["ID_DONANTES"])
                    };
                    lista.Add(dona);
                }
                reader.Close();
            }
            return lista;
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
                        ID_BENEFICIARIOS = (int)reader["ID_BENEFICIARIOS"],
                        UsuarioID = ObtenerUsuarioIDDesdeDonanteID((int)reader["ID_DONANTES"]) // ← Opcional
                    };
                }
                reader.Close();
            }
            return dona;
        }

        /// <summary>Este método es opcional si necesitás mostrar UsuarioID al recuperar donación.</summary>
        private static int ObtenerUsuarioIDDesdeDonanteID(int idDonante)
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT UsuarioID FROM DONANTES WHERE ID_DONANTES = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", idDonante);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        /// <summary>Actualiza una donación existente.</summary>
        public static bool ActualizarDonacion(int ID_donacion, decimal monto, DateTime fecha, int ID_beneficiarios)
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarDonacion", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_DONACIONES", ID_donacion);
                cmd.Parameters.AddWithValue("@MONTO", monto);
                cmd.Parameters.AddWithValue("@FECHA", fecha);
                cmd.Parameters.AddWithValue("@ID_BENEFICIARIOS", ID_beneficiarios);

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
