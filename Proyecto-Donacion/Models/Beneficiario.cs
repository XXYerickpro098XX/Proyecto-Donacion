using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Proyecto_Donacion.Models
{
    public class Beneficiario
    {
        // Propiedades
        public int ID_Beneficiario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }

        public Beneficiario() { }

        // === CREAR ===
        public int CrearBeneficiario()
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarBeneficiario", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", this.Nombre);
                cmd.Parameters.AddWithValue("@Email", this.Email);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        // === OBTENER TODOS ===
        public static List<Beneficiario> ObtenerTodosBeneficiario()
        {
            List<Beneficiario> lista = new List<Beneficiario>();
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerTodosLosBeneficiarios", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Beneficiario b = new Beneficiario
                    {
                        ID_Beneficiario = Convert.ToInt32(reader["ID_Beneficiario"]),
                        Nombre = reader["Nombre"].ToString(),
                        Email = reader["Email"].ToString()
                    };
                    lista.Add(b);
                }
                reader.Close();
            }
            return lista;
        }

        // === OBTENER POR ID ===
        public static Beneficiario ObtenerPorIdBeneficiario(int id)
        {
            Beneficiario bene = null;
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerBeneficiarioPorID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_Beneficiario", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    bene = new Beneficiario
                    {
                        ID_Beneficiario = Convert.ToInt32(reader["ID_Beneficiario"]),
                        Nombre = reader["Nombre"].ToString(),
                        Email = reader["Email"].ToString()
                    };
                }
                reader.Close();
            }
            return bene;
        }

        // === ACTUALIZAR ===
        public bool ActualizarBeneficiario()
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarBeneficiario", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_Beneficiario", this.ID_Beneficiario);
                cmd.Parameters.AddWithValue("@Nombre", this.Nombre);
                cmd.Parameters.AddWithValue("@Email", this.Email);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        // === ELIMINAR ===
        public bool EliminarBeneficiario()
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarBeneficiario", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_Beneficiario", this.ID_Beneficiario);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
    }
}
