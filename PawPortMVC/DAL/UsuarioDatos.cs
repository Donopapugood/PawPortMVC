using Microsoft.Data.SqlClient;
using PawPortMVC.Models;
using System.Data;
using System.Security.Cryptography;
using System.Text;

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

            using SqlConnection conexion =
                new SqlConnection(_cadenaConexion);

            using SqlCommand comando =
                new SqlCommand(consulta, conexion);

            comando.Parameters.Add(
                "@Correo",
                SqlDbType.VarChar,
                100
            ).Value = correo.Trim();

            comando.Parameters.Add(
                "@Hash",
                SqlDbType.VarChar,
                256
            ).Value = hash;

            conexion.Open();

            using SqlDataReader lector = comando.ExecuteReader();

            if (!lector.Read())
            {
                return null;
            }

            Usuario usuario = new Usuario
            {
                IdUsuario =
                    Convert.ToInt32(lector["id_usuario"]),

                IdSucursal =
                    lector["id_sucursal"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(lector["id_sucursal"]),

                Nombre =
                    lector["nombre"].ToString() ?? "",

                Apellido =
                    lector["apellido"].ToString() ?? "",

                CorreoElectronico =
                    lector["correo_electronico"].ToString() ?? "",

                Rol =
                    lector["rol"].ToString() ?? "",

                Estado =
                    Convert.ToBoolean(lector["estado"])
            };

            return usuario;
        }

        private string ConvertirSHA256(string texto)
        {
            byte[] bytes =
                Encoding.UTF8.GetBytes(texto);

            byte[] hash =
                SHA256.HashData(bytes);

            return Convert.ToHexString(hash);
        }
    }
}