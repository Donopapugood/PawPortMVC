using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PawPortMVC.Models;

namespace PawPortMVC.DAL
{
    public class UsuarioDatos
    {
        private readonly string _cadenaConexion;

        public UsuarioDatos(IConfiguration configuration)
        {
            _cadenaConexion =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "No se encontró la cadena de conexión DefaultConnection.");
        }

        public Usuario? ValidarLogin(string correo, string clave)
        {
            string hash = ConvertirSHA256(clave);

            string consulta = @"
                SELECT
                    id_usuario,
                    id_sucursal,
                    nombre,
                    apellido,
                    correo_electronico,
                    rol,
                    estado
                FROM Usuarios
                WHERE correo_electronico = @Correo
                  AND contrasena_hash = @Hash
                  AND estado = 1;";

            using SqlConnection conexion = new SqlConnection(_cadenaConexion);
            using SqlCommand comando = new SqlCommand(consulta, conexion);

            comando.Parameters.Add("@Correo", SqlDbType.VarChar, 100).Value = correo.Trim();
            comando.Parameters.Add("@Hash", SqlDbType.VarChar, 256).Value = hash;
            conexion.Open();

            using SqlDataReader lector = comando.ExecuteReader();

            if (!lector.Read())
            {
                return null;
            }

            return new Usuario
            {
                IdUsuario = Convert.ToInt32(lector["id_usuario"]),
                IdSucursal = lector["id_sucursal"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(lector["id_sucursal"]),
                Nombre = lector["nombre"].ToString() ?? "",
                Apellido = lector["apellido"].ToString() ?? "",
                CorreoElectronico = lector["correo_electronico"].ToString() ?? "",
                Rol = lector["rol"].ToString() ?? "",
                Estado = Convert.ToBoolean(lector["estado"])
            };
        }

        public bool RegistrarUsuario(string nombre, string apellido, string correo, string telefono, string rol, string clave, out string mensajeError)
        {
            mensajeError = "";
            try
            {
                using SqlConnection conexion = new SqlConnection(_cadenaConexion);
                conexion.Open();

                string verificar = "SELECT COUNT(1) FROM Usuarios WHERE correo_electronico = @Correo;";
                using (SqlCommand cmdVerificar = new SqlCommand(verificar, conexion))
                {
                    cmdVerificar.Parameters.AddWithValue("@Correo", correo.Trim());
                    int existe = Convert.ToInt32(cmdVerificar.ExecuteScalar());
                    if (existe > 0)
                    {
                        mensajeError = "Ya existe una cuenta registrada con ese correo en PawPortDB.";
                        return false;
                    }
                }

                string duiGenerado = Random.Shared.Next(10000000, 99999999).ToString() + "-" + Random.Shared.Next(0, 9).ToString();
                string hash = ConvertirSHA256(clave);

                string insertarUsuario = @"
                    INSERT INTO Usuarios (id_sucursal, nombre, apellido, dui, correo_electronico, contrasena_hash, telefono, rol, estado)
                    OUTPUT INSERTED.id_usuario
                    VALUES (
                        (SELECT TOP 1 id_sucursal FROM Sucursales ORDER BY id_sucursal),
                        @Nombre, @Apellido, @Dui, @Correo, @Hash, @Telefono, @Rol, 1
                    );";

                int nuevoIdUsuario;
                using (SqlCommand cmdInsertar = new SqlCommand(insertarUsuario, conexion))
                {
                    cmdInsertar.Parameters.AddWithValue("@Nombre", nombre.Trim());
                    cmdInsertar.Parameters.AddWithValue("@Apellido", string.IsNullOrWhiteSpace(apellido) ? "PawPort" : apellido.Trim());
                    cmdInsertar.Parameters.AddWithValue("@Dui", duiGenerado);
                    cmdInsertar.Parameters.AddWithValue("@Correo", correo.Trim());
                    cmdInsertar.Parameters.AddWithValue("@Hash", hash);
                    cmdInsertar.Parameters.AddWithValue("@Telefono", string.IsNullOrWhiteSpace(telefono) ? "7000-0000" : telefono.Trim());
                    cmdInsertar.Parameters.AddWithValue("@Rol", string.IsNullOrWhiteSpace(rol) ? "Propietario" : rol.Trim());
                    nuevoIdUsuario = Convert.ToInt32(cmdInsertar.ExecuteScalar());
                }

                if (rol == "Propietario" || string.IsNullOrWhiteSpace(rol))
                {
                    string insertarProp = @"
                        INSERT INTO Propietarios (id_usuario, direccion_residencia, contacto_emergencia)
                        VALUES (@IdUsuario, 'San Salvador, El Salvador', @Telefono);";
                    using SqlCommand cmdProp = new SqlCommand(insertarProp, conexion);
                    cmdProp.Parameters.AddWithValue("@IdUsuario", nuevoIdUsuario);
                    cmdProp.Parameters.AddWithValue("@Telefono", string.IsNullOrWhiteSpace(telefono) ? "7000-0000" : telefono.Trim());
                    cmdProp.ExecuteNonQuery();
                }
                else if (rol == "Veterinario")
                {
                    string jv = "JV-" + Random.Shared.Next(1000, 9999).ToString();
                    string insertarVet = @"
                        INSERT INTO Veterinarios (id_usuario, numero_junta_vigilancia, especialidad)
                        VALUES (@IdUsuario, @JV, 'Medicina veterinaria general');";
                    using SqlCommand cmdVet = new SqlCommand(insertarVet, conexion);
                    cmdVet.Parameters.AddWithValue("@IdUsuario", nuevoIdUsuario);
                    cmdVet.Parameters.AddWithValue("@JV", jv);
                    cmdVet.ExecuteNonQuery();
                }

                Datos.CargarDesdeSqlServer();
                return true;
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                return false;
            }
        }

        private string ConvertirSHA256(string texto)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(texto);
            byte[] hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
