using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Proyecto_Donacion.Models
{
    public class Usuario
    {
        private int UsuarioID;
        private string Nombre;
        private string Correo;
        private string Telefono;
        private string PasswordHash;
        private bool Rol;

        public int UsuarioID1 { get => UsuarioID; set => UsuarioID = value; }
        public string Nombre1 { get => Nombre; set => Nombre = value; }
        public string Telefono1 { get => Telefono; set => Telefono = value; }
        public string Correo1 { get => Correo; set => Correo = value; }
        public string PasswordHash1 { get => PasswordHash; set => PasswordHash = value; }
        public bool Rol1 { get => Rol; set => Rol = value; }
        // Constructor con parámetros


        // Constructor vacío (opcionalmente podríamos tener sobrecargas)
        public Usuario() { }

        public int CrearUsuario()
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("INSERTAR_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", this.Nombre1);
                cmd.Parameters.AddWithValue("@Correo", this.Correo1);
                cmd.Parameters.AddWithValue("@Telefono", this.Telefono1);
                cmd.Parameters.AddWithValue("@Password", SeguridadUtil.HashSHA256(this.PasswordHash1));
                cmd.Parameters.AddWithValue("@Rol", this.Rol1);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                return (result != null) ? Convert.ToInt32(result) : -1;
            }
        }

        public static List<Usuario> ObtenerTodos()
        {
            List<Usuario> lista = new List<Usuario>();
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECCIONAR_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Usuario us = new Usuario();
                    us.Nombre = reader["Nombre"].ToString();
                    us.Correo = reader["Correo"].ToString();
                    us.Telefono = reader["Telefono"].ToString();
                    us.PasswordHash = reader["PasswordHash"].ToString();
                    us.Rol = (bool)reader["Rol"];
                    us.UsuarioID = (int)reader["UsuarioID"];
                    lista.Add(us);
                }
            } 
            return lista;
        }

        public bool EliminarUsuario(int id)
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("ELIMINAR_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioID", id);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public bool ActualizarUsuario()
        {
            string connString = ConfigurationManager.ConnectionStrings["DonacionBD"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("ActualizarUsuario", conn);
                cmd.Parameters.AddWithValue("@Nombre", this.Nombre);
                cmd.Parameters.AddWithValue("@Correo", this.Correo);
                cmd.Parameters.AddWithValue("@Telefono", this.Telefono);
                cmd.Parameters.AddWithValue("@PasswordHash", this.PasswordHash);
                cmd.Parameters.AddWithValue("@Rol", this.Rol);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return (rows > 0);
            }
        }
    }
}