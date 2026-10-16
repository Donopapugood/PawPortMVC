using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using PawPortMVC.Models;

namespace PawPortMVC.DAL
{
    public static class Datos
    {
        private static string _cadenaConexion = "Server=DESKTOP-VB0V2BA;Database=PawPortDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

        public static void Inicializar(string? cadenaConexion)
        {
            if (!string.IsNullOrWhiteSpace(cadenaConexion))
            {
                _cadenaConexion = cadenaConexion;
            }
            CargarDesdeSqlServer();
        }

        public static void CargarDesdeSqlServer()
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                conn.Open();

                List<Clinica> sucursalesDb = new List<Clinica>();
                string sqlSuc = @"
                    SELECT s.id_sucursal, s.nombre_clinica, s.direccion, s.telefono, s.correo_contacto,
                           ISNULL((SELECT TOP 1 v.especialidad FROM Veterinarios v INNER JOIN Usuarios u ON u.id_usuario = v.id_usuario WHERE u.id_sucursal = s.id_sucursal), 'Medicina veterinaria general') AS especialidad
                    FROM Sucursales s
                    WHERE s.estado = 1
                    ORDER BY s.id_sucursal;";
                string[] imgs = new[] { "/recursos/Img/Ciudad_Mexico.png", "/recursos/Img/Madrid.png", "/recursos/Img/Buenos_Aires.png" };
                using (SqlCommand cmd = new SqlCommand(sqlSuc, conn))
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    int idx = 0;
                    while (r.Read())
                    {
                        string dir = r["direccion"].ToString() ?? "";
                        string ciudad = dir.Contains(",") ? dir.Split(',').Last().Trim() : "San Salvador";
                        sucursalesDb.Add(new Clinica
                        {
                            Id = Convert.ToInt32(r["id_sucursal"]),
                            Nombre = r["nombre_clinica"].ToString() ?? "",
                            Pais = "El Salvador",
                            Ciudad = ciudad,
                            Direccion = dir,
                            Telefono = r["telefono"].ToString() ?? "",
                            Correo = r["correo_contacto"].ToString() ?? "",
                            Imagen = imgs[idx % imgs.Length],
                            Especialidad = r["especialidad"].ToString() ?? "Medicina veterinaria general",
                            Descripcion = "Sede oficial de la red PawPort conectada a la base de datos central PawPortDB.",
                            Calificacion = 4.9,
                            TotalReseñas = 35 + (idx * 7)
                        });
                        idx++;
                    }
                }
                if (sucursalesDb.Count > 0) Clinicas = sucursalesDb;

                List<Veterinario> vetsDb = new List<Veterinario>();
                string sqlVets = @"
                    SELECT v.id_veterinario, u.nombre, u.apellido, v.especialidad, v.numero_junta_vigilancia,
                           ISNULL(s.nombre_clinica, 'PawPort Central San Salvador') AS clinica,
                           u.correo_electronico, u.telefono, u.estado
                    FROM Veterinarios v
                    INNER JOIN Usuarios u ON u.id_usuario = v.id_usuario
                    LEFT JOIN Sucursales s ON s.id_sucursal = u.id_sucursal
                    ORDER BY v.id_veterinario;";
                using (SqlCommand cmd = new SqlCommand(sqlVets, conn))
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        vetsDb.Add(new Veterinario
                        {
                            Id = Convert.ToInt32(r["id_veterinario"]),
                            Nombre = "Dr(a). " + r["nombre"] + " " + r["apellido"],
                            Especialidad = r["especialidad"].ToString() ?? "",
                            Clinica = r["clinica"].ToString() ?? "",
                            Correo = r["correo_electronico"].ToString() ?? "",
                            Telefono = r["telefono"].ToString() ?? "",
                            Horario = "Lunes a Sábado 08:00 - 17:00 (" + r["numero_junta_vigilancia"] + ")",
                            Estado = Convert.ToBoolean(r["estado"]) ? "Activo" : "Inactivo"
                        });
                    }
                }
                if (vetsDb.Count > 0) Veterinarios = vetsDb;

                List<Mascota> mascotasDb = new List<Mascota>();
                string sqlMasc = @"
                    SELECT m.id_mascota, m.nombre_mascota, m.especie, m.raza, m.sexo, m.fecha_nacimiento, m.color, m.peso_kg,
                           (u.nombre + ' ' + u.apellido) AS propietario,
                           (SELECT COUNT(1) FROM VacunasAplicadas va INNER JOIN ExpedientesClinicos ec ON ec.id_expediente = va.id_expediente WHERE ec.id_mascota = m.id_mascota) AS total_vacunas
                    FROM Mascotas m
                    INNER JOIN Propietarios p ON p.id_propietario = m.id_propietario
                    INNER JOIN Usuarios u ON u.id_usuario = p.id_usuario
                    ORDER BY m.id_mascota;";
                using (SqlCommand cmd = new SqlCommand(sqlMasc, conn))
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string nom = r["nombre_mascota"].ToString() ?? "";
                        int edad = 2;
                        if (r["fecha_nacimiento"] != DBNull.Value)
                        {
                            DateTime fn = Convert.ToDateTime(r["fecha_nacimiento"]);
                            edad = Math.Max(1, DateTime.Today.Year - fn.Year);
                        }
                        string img = nom == "Luna" ? "/recursos/Img/Luna.png" : (nom == "Max" ? "/recursos/Img/Max.png" : "/recursos/Img/Patitas.png");
                        int vacs = Convert.ToInt32(r["total_vacunas"]);
                        mascotasDb.Add(new Mascota
                        {
                            Id = Convert.ToInt32(r["id_mascota"]),
                            Nombre = nom,
                            Especie = r["especie"].ToString() ?? "Perro",
                            Raza = r["raza"].ToString() ?? "",
                            Edad = edad,
                            Peso = r["peso_kg"] != DBNull.Value ? Convert.ToDouble(r["peso_kg"]) : 5.0,
                            Sexo = (r["sexo"].ToString() == "M") ? "Macho" : "Hembra",
                            Propietario = r["propietario"].ToString() ?? "",
                            Imagen = img,
                            Vacunas = vacs > 0 ? "Al día" : "Pendiente",
                            Estado = "Activo",
                            UltimaVisita = DateTime.Today.ToString("dd/MM/yyyy"),
                            Alergias = "Color: " + (r["color"]?.ToString() ?? "N/A") + ". Sin alergias críticas."
                        });
                    }
                }
                if (mascotasDb.Count > 0) Mascotas = mascotasDb;

                List<Cita> citasDb = new List<Cita>();
                string sqlCitas = @"
                    SELECT c.id_cita, c.fecha_hora, m.nombre_mascota, m.especie,
                           (up.nombre + ' ' + up.apellido) AS propietario,
                           s.nombre_clinica,
                           ('Dr(a). ' + uv.nombre + ' ' + uv.apellido) AS veterinario,
                           v.especialidad, c.motivo_consulta, c.estado_cita
                    FROM CitasMedicas c
                    INNER JOIN Mascotas m ON m.id_mascota = c.id_mascota
                    INNER JOIN Propietarios p ON p.id_propietario = m.id_propietario
                    INNER JOIN Usuarios up ON up.id_usuario = p.id_usuario
                    INNER JOIN Veterinarios v ON v.id_veterinario = c.id_veterinario
                    INNER JOIN Usuarios uv ON uv.id_usuario = v.id_usuario
                    INNER JOIN Sucursales s ON s.id_sucursal = c.id_sucursal
                    ORDER BY c.fecha_hora DESC;";
                using (SqlCommand cmd = new SqlCommand(sqlCitas, conn))
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        DateTime fh = Convert.ToDateTime(r["fecha_hora"]);
                        citasDb.Add(new Cita
                        {
                            Id = Convert.ToInt32(r["id_cita"]),
                            Fecha = fh.ToString("yyyy-MM-dd"),
                            Hora = fh.ToString("HH:mm"),
                            Mascota = r["nombre_mascota"].ToString() ?? "",
                            Especie = r["especie"].ToString() ?? "",
                            Propietario = r["propietario"].ToString() ?? "",
                            Clinica = r["nombre_clinica"].ToString() ?? "",
                            Veterinario = r["veterinario"].ToString() ?? "",
                            Especialidad = r["especialidad"].ToString() ?? "",
                            Motivo = r["motivo_consulta"].ToString() ?? "",
                            Notas = "Registrada en PawPortDB",
                            Estado = r["estado_cita"].ToString() ?? "Programada"
                        });
                    }
                }
                if (citasDb.Count > 0) Citas = citasDb;

                List<Expediente> expDb = new List<Expediente>();
                string sqlExp = @"
                    SELECT e.id_expediente, e.id_mascota, m.nombre_mascota, m.peso_kg,
                           (up.nombre + ' ' + up.apellido) AS propietario,
                           ISNULL(s.nombre_clinica, 'PawPort Central San Salvador') AS clinica,
                           ('Dr(a). ' + uv.nombre + ' ' + uv.apellido) AS veterinario,
                           e.fecha_consulta,
                           CAST(e.sintomas AS VARCHAR(MAX)) AS sintomas,
                           CAST(e.diagnostico AS VARCHAR(MAX)) AS diagnostico,
                           CAST(e.tratamiento_recomendado AS VARCHAR(MAX)) AS tratamiento,
                           CAST(ISNULL(e.observaciones, '') AS VARCHAR(MAX)) AS observaciones,
                           ISNULL((SELECT TOP 1 va.nombre_vacuna + ' (' + va.dosis + ')' FROM VacunasAplicadas va WHERE va.id_expediente = e.id_expediente), 'Control general') AS vacuna
                    FROM ExpedientesClinicos e
                    INNER JOIN Mascotas m ON m.id_mascota = e.id_mascota
                    INNER JOIN Propietarios p ON p.id_propietario = m.id_propietario
                    INNER JOIN Usuarios up ON up.id_usuario = p.id_usuario
                    INNER JOIN Veterinarios v ON v.id_veterinario = e.id_veterinario
                    INNER JOIN Usuarios uv ON uv.id_usuario = v.id_usuario
                    LEFT JOIN Sucursales s ON s.id_sucursal = uv.id_sucursal
                    ORDER BY e.fecha_consulta DESC;";
                using (SqlCommand cmd = new SqlCommand(sqlExp, conn))
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        DateTime fc = Convert.ToDateTime(r["fecha_consulta"]);
                        expDb.Add(new Expediente
                        {
                            Id = Convert.ToInt32(r["id_expediente"]),
                            IdMascota = Convert.ToInt32(r["id_mascota"]),
                            Mascota = r["nombre_mascota"].ToString() ?? "",
                            Propietario = r["propietario"].ToString() ?? "",
                            Clinica = r["clinica"].ToString() ?? "",
                            Veterinario = r["veterinario"].ToString() ?? "",
                            Fecha = fc.ToString("dd/MM/yyyy"),
                            Diagnostico = r["diagnostico"].ToString() ?? "",
                            Tratamiento = r["tratamiento"].ToString() ?? "",
                            MedicamentoRecetado = r["sintomas"].ToString() ?? "",
                            VacunaAplicada = r["vacuna"].ToString() ?? "",
                            PesoRegistrado = r["peso_kg"] != DBNull.Value ? Convert.ToDouble(r["peso_kg"]) : 10.0,
                            Notas = r["observaciones"].ToString() ?? ""
                        });
                    }
                }
                if (expDb.Count > 0) Expedientes = expDb;

                List<ProductoInventario> invDb = new List<ProductoInventario>();
                string sqlInv = @"
                    SELECT i.id_inventario, pr.nombre_producto, pr.categoria, s.nombre_clinica,
                           i.stock_actual, i.stock_minimo, pr.precio_base_venta, i.numero_lote, i.fecha_vencimiento
                    FROM InventariosSucursal i
                    INNER JOIN Productos pr ON pr.id_producto = i.id_producto
                    INNER JOIN Sucursales s ON s.id_sucursal = i.id_sucursal
                    ORDER BY i.id_inventario;";
                using (SqlCommand cmd = new SqlCommand(sqlInv, conn))
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        DateTime fv = Convert.ToDateTime(r["fecha_vencimiento"]);
                        invDb.Add(new ProductoInventario
                        {
                            Id = Convert.ToInt32(r["id_inventario"]),
                            Nombre = r["nombre_producto"].ToString() ?? "",
                            Categoria = r["categoria"].ToString() ?? "",
                            Clinica = r["nombre_clinica"].ToString() ?? "",
                            StockActual = Convert.ToInt32(r["stock_actual"]),
                            StockMinimo = Convert.ToInt32(r["stock_minimo"]),
                            Precio = Convert.ToDecimal(r["precio_base_venta"]),
                            Proveedor = "Lote " + (r["numero_lote"].ToString() ?? "L-001"),
                            FechaCaducidad = fv.ToString("yyyy-MM-dd")
                        });
                    }
                }
                if (invDb.Count > 0) Inventario = invDb;

                List<Factura> facDb = new List<Factura>();
                string sqlFac = @"
                    SELECT f.id_factura, f.numero_factura, f.fecha_emision,
                           (u.nombre + ' ' + u.apellido) AS propietario,
                           ISNULL((SELECT TOP 1 m.nombre_mascota FROM Mascotas m WHERE m.id_propietario = f.id_propietario), 'Luna') AS mascota,
                           s.nombre_clinica, f.monto_total, f.estado_pago,
                           ISNULL((
                               SELECT TOP 1 ISNULL(pr.nombre_producto, sv.nombre_servicio)
                               FROM DetallesFactura df
                               LEFT JOIN Productos pr ON pr.id_producto = df.id_producto
                               LEFT JOIN Servicios sv ON sv.id_servicio = df.id_servicio
                               WHERE df.id_factura = f.id_factura
                           ), 'Consulta y servicios veterinarios') AS concepto
                    FROM Facturas f
                    INNER JOIN Sucursales s ON s.id_sucursal = f.id_sucursal
                    INNER JOIN Propietarios p ON p.id_propietario = f.id_propietario
                    INNER JOIN Usuarios u ON u.id_usuario = p.id_usuario
                    ORDER BY f.fecha_emision DESC;";
                using (SqlCommand cmd = new SqlCommand(sqlFac, conn))
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        DateTime fe = Convert.ToDateTime(r["fecha_emision"]);
                        facDb.Add(new Factura
                        {
                            Id = Convert.ToInt32(r["id_factura"]),
                            NumeroFactura = r["numero_factura"].ToString() ?? "",
                            Fecha = fe.ToString("yyyy-MM-dd"),
                            Propietario = r["propietario"].ToString() ?? "",
                            Mascota = r["mascota"].ToString() ?? "",
                            Clinica = r["nombre_clinica"].ToString() ?? "",
                            Concepto = r["concepto"].ToString() ?? "",
                            Monto = Convert.ToDecimal(r["monto_total"]),
                            MetodoPago = "Tarjeta / Caja",
                            Estado = r["estado_pago"].ToString() ?? "Pendiente"
                        });
                    }
                }
                if (facDb.Count > 0) Facturas = facDb;
            }
            catch (Exception)
            {
            }
        }

        public static void InsertarMascotaSql(string nombre, string especie, string raza, int edad, double peso, string sexo, string color)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                conn.Open();
                string sql = @"
                    INSERT INTO Mascotas (id_propietario, nombre_mascota, especie, raza, sexo, fecha_nacimiento, color, peso_kg)
                    VALUES (
                        (SELECT TOP 1 id_propietario FROM Propietarios ORDER BY id_propietario),
                        @Nombre, @Especie, @Raza, @Sexo, @FechaNac, @Color, @Peso
                    );";
                using SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre.Trim());
                cmd.Parameters.AddWithValue("@Especie", especie);
                cmd.Parameters.AddWithValue("@Raza", raza.Trim());
                cmd.Parameters.AddWithValue("@Sexo", sexo.StartsWith("M", StringComparison.OrdinalIgnoreCase) ? "M" : "H");
                cmd.Parameters.AddWithValue("@FechaNac", DateTime.Today.AddYears(-Math.Max(1, edad)));
                cmd.Parameters.AddWithValue("@Color", string.IsNullOrWhiteSpace(color) ? "Variado" : color.Trim());
                cmd.Parameters.AddWithValue("@Peso", Convert.ToDecimal(peso));
                cmd.ExecuteNonQuery();
                CargarDesdeSqlServer();
            }
            catch (Exception)
            {
            }
        }

        public static void InsertarCitaSql(string mascota, string clinica, string fecha, string hora, string motivo)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                conn.Open();
                DateTime fechaHora = DateTime.TryParse(fecha + " " + hora, out DateTime dt) ? dt : DateTime.Now.AddDays(1);
                string sql = @"
                    INSERT INTO CitasMedicas (id_mascota, id_veterinario, id_sucursal, fecha_hora, motivo_consulta, estado_cita)
                    VALUES (
                        ISNULL((SELECT TOP 1 id_mascota FROM Mascotas WHERE nombre_mascota = @Mascota), (SELECT TOP 1 id_mascota FROM Mascotas)),
                        (SELECT TOP 1 id_veterinario FROM Veterinarios),
                        ISNULL((SELECT TOP 1 id_sucursal FROM Sucursales WHERE nombre_clinica = @Clinica), (SELECT TOP 1 id_sucursal FROM Sucursales)),
                        @FechaHora, @Motivo, 'Programada'
                    );";
                using SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Mascota", mascota);
                cmd.Parameters.AddWithValue("@Clinica", clinica);
                cmd.Parameters.AddWithValue("@FechaHora", fechaHora);
                cmd.Parameters.AddWithValue("@Motivo", motivo);
                cmd.ExecuteNonQuery();
                CargarDesdeSqlServer();
            }
            catch (Exception)
            {
            }
        }


        public static void CambiarEstadoCitaSql(int idCita, string nuevoEstado)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                conn.Open();
                using SqlCommand cmd = new SqlCommand("UPDATE CitasMedicas SET estado_cita = @Estado WHERE id_cita = @Id;", conn);
                cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
                cmd.Parameters.AddWithValue("@Id", idCita);
                cmd.ExecuteNonQuery();
                CargarDesdeSqlServer();
            }
            catch (Exception)
            {
            }
        }

        public static void InsertarExpedienteSql(int idMascota, string clinica, string diagnostico, string tratamiento, string medicamento, string vacuna, string notas)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                conn.Open();

                string sqlCita = @"
                    INSERT INTO CitasMedicas (id_mascota, id_veterinario, id_sucursal, fecha_hora, motivo_consulta, estado_cita)
                    OUTPUT INSERTED.id_cita
                    VALUES (
                        ISNULL((SELECT TOP 1 id_mascota FROM Mascotas WHERE id_mascota = @IdMascota), (SELECT TOP 1 id_mascota FROM Mascotas)),
                        (SELECT TOP 1 id_veterinario FROM Veterinarios),
                        ISNULL((SELECT TOP 1 id_sucursal FROM Sucursales WHERE nombre_clinica = @Clinica), (SELECT TOP 1 id_sucursal FROM Sucursales)),
                        GETDATE(),
                        @Motivo,
                        'Completada'
                    );";
                int idCitaNueva;
                using (SqlCommand cmdCita = new SqlCommand(sqlCita, conn))
                {
                    cmdCita.Parameters.AddWithValue("@IdMascota", idMascota);
                    cmdCita.Parameters.AddWithValue("@Clinica", clinica ?? "PawPort Central San Salvador");
                    cmdCita.Parameters.AddWithValue("@Motivo", diagnostico.Length > 200 ? diagnostico.Substring(0, 200) : diagnostico);
                    idCitaNueva = Convert.ToInt32(cmdCita.ExecuteScalar());
                }

                string sqlExp = @"
                    INSERT INTO ExpedientesClinicos (id_cita, id_mascota, id_veterinario, fecha_consulta, sintomas, diagnostico, tratamiento_recomendado, observaciones)
                    OUTPUT INSERTED.id_expediente
                    VALUES (
                        @IdCita,
                        ISNULL((SELECT TOP 1 id_mascota FROM Mascotas WHERE id_mascota = @IdMascota), (SELECT TOP 1 id_mascota FROM Mascotas)),
                        (SELECT TOP 1 id_veterinario FROM Veterinarios),
                        GETDATE(),
                        @Sintomas,
                        @Diagnostico,
                        @Tratamiento,
                        @Observaciones
                    );";
                int idExpNuevo;
                using (SqlCommand cmdExp = new SqlCommand(sqlExp, conn))
                {
                    cmdExp.Parameters.AddWithValue("@IdCita", idCitaNueva);
                    cmdExp.Parameters.AddWithValue("@IdMascota", idMascota);
                    cmdExp.Parameters.AddWithValue("@Sintomas", string.IsNullOrWhiteSpace(medicamento) ? "Consulta clínica" : medicamento);
                    cmdExp.Parameters.AddWithValue("@Diagnostico", diagnostico);
                    cmdExp.Parameters.AddWithValue("@Tratamiento", tratamiento);
                    cmdExp.Parameters.AddWithValue("@Observaciones", string.IsNullOrWhiteSpace(notas) ? "Registrado desde PawPortMVC" : notas);
                    idExpNuevo = Convert.ToInt32(cmdExp.ExecuteScalar());
                }

                if (!string.IsNullOrWhiteSpace(vacuna) && vacuna != "Ninguna")
                {
                    string sqlVac = @"
                        INSERT INTO VacunasAplicadas (id_expediente, nombre_vacuna, dosis, fecha_aplicacion, proxima_fecha_refuerzo)
                        VALUES (@IdExp, @Vacuna, '1 ml', CAST(GETDATE() AS DATE), DATEADD(YEAR, 1, CAST(GETDATE() AS DATE)));";
                    using SqlCommand cmdVac = new SqlCommand(sqlVac, conn);
                    cmdVac.Parameters.AddWithValue("@IdExp", idExpNuevo);
                    cmdVac.Parameters.AddWithValue("@Vacuna", vacuna);
                    cmdVac.ExecuteNonQuery();
                }

                CargarDesdeSqlServer();
            }
            catch (Exception)
            {
            }
        }


        public static void InsertarProductoSql(string nombre, string categoria, string clinica, int stockActual, int stockMinimo, decimal precio, string fechaCaducidad)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                conn.Open();
                string codigo = "PP-" + Random.Shared.Next(100000, 999999);
                string sqlProd = @"
                    INSERT INTO Productos (codigo_barras, nombre_producto, descripcion, categoria, precio_base_venta, requiere_receta)
                    OUTPUT INSERTED.id_producto
                    VALUES (@Codigo, @Nombre, 'Registrado en sistema PawPortMVC', @Categoria, @Precio, 0);";
                int idProd;
                using (SqlCommand cmdP = new SqlCommand(sqlProd, conn))
                {
                    cmdP.Parameters.AddWithValue("@Codigo", codigo);
                    cmdP.Parameters.AddWithValue("@Nombre", nombre);
                    cmdP.Parameters.AddWithValue("@Categoria", categoria);
                    cmdP.Parameters.AddWithValue("@Precio", precio);
                    idProd = Convert.ToInt32(cmdP.ExecuteScalar());
                }

                DateTime venc = DateTime.TryParse(fechaCaducidad, out DateTime dt) ? dt : DateTime.Today.AddYears(1);
                string sqlInv = @"
                    INSERT INTO InventariosSucursal (id_sucursal, id_producto, stock_actual, stock_minimo, numero_lote, fecha_vencimiento)
                    VALUES (
                        ISNULL((SELECT TOP 1 id_sucursal FROM Sucursales WHERE nombre_clinica = @Clinica), (SELECT TOP 1 id_sucursal FROM Sucursales)),
                        @IdProd, @StockActual, @StockMinimo, @Lote, @Venc
                    );";
                using (SqlCommand cmdI = new SqlCommand(sqlInv, conn))
                {
                    cmdI.Parameters.AddWithValue("@Clinica", clinica ?? "PawPort Central San Salvador");
                    cmdI.Parameters.AddWithValue("@IdProd", idProd);
                    cmdI.Parameters.AddWithValue("@StockActual", stockActual);
                    cmdI.Parameters.AddWithValue("@StockMinimo", stockMinimo);
                    cmdI.Parameters.AddWithValue("@Lote", "L-" + Random.Shared.Next(100, 999));
                    cmdI.Parameters.AddWithValue("@Venc", venc);
                    cmdI.ExecuteNonQuery();
                }

                CargarDesdeSqlServer();
            }
            catch (Exception)
            {
            }
        }


        public static void ActualizarStockCompraSql(int idInventario, int cantidadSumar)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                conn.Open();
                using SqlCommand cmd = new SqlCommand("UPDATE InventariosSucursal SET stock_actual = stock_actual + @Cant WHERE id_inventario = @Id;", conn);
                cmd.Parameters.AddWithValue("@Cant", cantidadSumar);
                cmd.Parameters.AddWithValue("@Id", idInventario);
                cmd.ExecuteNonQuery();
                CargarDesdeSqlServer();
            }
            catch (Exception)
            {
            }
        }

        public static void InsertarFacturaSql(string clinica, decimal monto, string estado)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                conn.Open();
                string numFac = "FAC-2026-" + Random.Shared.Next(1000, 9999);
                decimal subtotal = Math.Round(monto / 1.13m, 2);
                decimal iva = Math.Round(monto - subtotal, 2);
                string sql = @"
                    INSERT INTO Facturas (id_sucursal, id_propietario, numero_factura, fecha_emision, subtotal, monto_iva, descuento_cooperativo, monto_total, estado_pago)
                    VALUES (
                        ISNULL((SELECT TOP 1 id_sucursal FROM Sucursales WHERE nombre_clinica = @Clinica), (SELECT TOP 1 id_sucursal FROM Sucursales)),
                        (SELECT TOP 1 id_propietario FROM Propietarios),
                        @NumFac, GETDATE(), @Subtotal, @Iva, 0.00, @Total, @Estado
                    );";
                using SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Clinica", clinica ?? "PawPort Central San Salvador");
                cmd.Parameters.AddWithValue("@NumFac", numFac);
                cmd.Parameters.AddWithValue("@Subtotal", subtotal);
                cmd.Parameters.AddWithValue("@Iva", iva);
                cmd.Parameters.AddWithValue("@Total", monto);
                cmd.Parameters.AddWithValue("@Estado", string.IsNullOrWhiteSpace(estado) ? "Pendiente" : estado);
                cmd.ExecuteNonQuery();
                CargarDesdeSqlServer();
            }
            catch (Exception)
            {
            }
        }


        public static void InsertarSucursalSql(string nombre, string direccion, string telefono, string correo)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                conn.Open();
                string sql = @"
                    INSERT INTO Sucursales (nombre_clinica, direccion, telefono, correo_contacto, fecha_afiliacion, estado)
                    VALUES (@Nombre, @Direccion, @Telefono, @Correo, CAST(GETDATE() AS DATE), 1);";
                using SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Direccion", direccion);
                cmd.Parameters.AddWithValue("@Telefono", telefono.Length > 15 ? telefono.Substring(0, 15) : telefono);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.ExecuteNonQuery();
                CargarDesdeSqlServer();
            }
            catch (Exception)
            {
            }
        }

        public static void CambiarEstadoFacturaSql(int idFactura, string nuevoEstado)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                conn.Open();
                using SqlCommand cmd = new SqlCommand("UPDATE Facturas SET estado_pago = @Estado WHERE id_factura = @Id;", conn);
                cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
                cmd.Parameters.AddWithValue("@Id", idFactura);
                cmd.ExecuteNonQuery();
                CargarDesdeSqlServer();
            }
            catch (Exception)
            {
            }
        }


        public static List<Clinica> Clinicas = new List<Clinica>
        {
            new Clinica
            {
                Id = 1,
                Nombre = "PawPort Central San Salvador",
                Pais = "El Salvador",
                Ciudad = "San Salvador",
                Direccion = "Colonia Escalon, San Salvador",
                Telefono = "2200-1001",
                Correo = "central@pawport.example",
                HorarioLunesViernes = "08:00 AM - 08:00 PM",
                HorarioSabado = "09:00 AM - 05:00 PM",
                HorarioDomingo = "Urgencias 24h",
                Imagen = "/recursos/Img/Ciudad_Mexico.png",
                Especialidad = "Medicina veterinaria general",
                Descripcion = "Sede central en San Salvador conectada a PawPortDB.",
                Calificacion = 4.9,
                TotalReseñas = 64
            },
            new Clinica
            {
                Id = 2,
                Nombre = "PawPort Santa Tecla",
                Pais = "El Salvador",
                Ciudad = "Santa Tecla",
                Direccion = "Centro de Santa Tecla, La Libertad",
                Telefono = "2200-1002",
                Correo = "santatecla@pawport.example",
                HorarioLunesViernes = "08:00 AM - 08:00 PM",
                HorarioSabado = "09:00 AM - 06:00 PM",
                HorarioDomingo = "Urgencias 24h",
                Imagen = "/recursos/Img/Madrid.png",
                Especialidad = "Cirugia veterinaria",
                Descripcion = "Sede especializada en cirugía veterinaria y traumatología.",
                Calificacion = 4.8,
                TotalReseñas = 42
            },
            new Clinica
            {
                Id = 3,
                Nombre = "PawPort Soyapango",
                Pais = "El Salvador",
                Ciudad = "Soyapango",
                Direccion = "Boulevard del Ejercito, Soyapango",
                Telefono = "2200-1003",
                Correo = "soyapango@pawport.example",
                HorarioLunesViernes = "08:00 AM - 07:30 PM",
                HorarioSabado = "09:00 AM - 04:00 PM",
                HorarioDomingo = "Urgencias únicamente",
                Imagen = "/recursos/Img/Buenos_Aires.png",
                Especialidad = "Dermatologia veterinaria",
                Descripcion = "Clínica integral en Soyapango dedicada a dermatología y medicina preventiva.",
                Calificacion = 4.9,
                TotalReseñas = 51
            }
        };

        public static List<string> EspecialidadesRed = new List<string>
        {
            "Medicina veterinaria general",
            "Cirugia veterinaria",
            "Dermatologia veterinaria",
            "Medicina interna",
            "Medicina felina",
            "Odontología"
        };

        public static List<Mascota> Mascotas = new List<Mascota>
        {
            new Mascota { Id = 1, Nombre = "Luna", Especie = "Perro", Raza = "Labrador Retriever", Edad = 5, Peso = 24.5, Sexo = "Hembra", Propietario = "Maria Hernandez", Imagen = "/recursos/Img/Luna.png", Vacunas = "Al día", Estado = "Activo", UltimaVisita = "10/09/2026", Alergias = "Gastritis aguda leve controlada." },
            new Mascota { Id = 2, Nombre = "Max", Especie = "Gato", Raza = "Siames", Edad = 3, Peso = 4.8, Sexo = "Macho", Propietario = "Maria Hernandez", Imagen = "/recursos/Img/Max.png", Vacunas = "Pendiente", Estado = "Activo", UltimaVisita = "09/10/2026", Alergias = "Ninguna registrada." }
        };

        public static List<Cita> Citas = new List<Cita>
        {
            new Cita { Id = 1, Fecha = "2026-10-20", Hora = "10:00", Mascota = "Maya", Especie = "Perro", Propietario = "Oscar Reyes", Clinica = "PawPort Soyapango", Veterinario = "Dr(a). Sofia Hernandez", Especialidad = "Dermatologia veterinaria", Motivo = "Seguimiento de irritacion dermatologica", Estado = "Programada" }
        };

        public static List<Expediente> Expedientes = new List<Expediente>
        {
            new Expediente { Id = 1, IdMascota = 1, Mascota = "Luna", Propietario = "Maria Hernandez", Clinica = "PawPort Central San Salvador", Veterinario = "Dr(a). Ana Lopez", Fecha = "10/09/2026", Diagnostico = "Gastritis aguda leve", Tratamiento = "Dieta blanda, hidratacion y protector gastrico durante 3 dias", MedicamentoRecetado = "Solucion de rehidratacion oral", VacunaAplicada = "Antirrabica", PesoRegistrado = 24.5, Notas = "Control en 72 horas si persisten los sintomas" }
        };

        public static List<ProductoInventario> Inventario = new List<ProductoInventario>
        {
            new ProductoInventario { Id = 1, Nombre = "Alimento premium perro adulto 3 kg", Categoria = "Alimentos", Clinica = "PawPort Central San Salvador", StockActual = 18, StockMinimo = 5, Precio = 32.50m, Proveedor = "Lote L-ALP-001", FechaCaducidad = "2027-05-30" }
        };

        public static List<CompraInventario> Compras = new List<CompraInventario>
        {
            new CompraInventario { Id = 1, Fecha = "2026-10-05", Producto = "Amoxicilina veterinaria", Proveedor = "VetPharma Internacional", Clinica = "PawPort Central San Salvador", Cantidad = 50, CostoUnitario = 9.00m }
        };

        public static List<Veterinario> Veterinarios = new List<Veterinario>
        {
            new Veterinario { Id = 1, Nombre = "Dr(a). Ana Lopez", Especialidad = "Medicina veterinaria general", Clinica = "PawPort Central San Salvador", Correo = "ana.lopez@pawport.example", Telefono = "7000-0007", Horario = "Lunes a Viernes 08:00 - 16:00 (JV-0001)", Estado = "Activo" }
        };

        public static List<Factura> Facturas = new List<Factura>
        {
            new Factura { Id = 1, NumeroFactura = "FAC-2026-0001", Fecha = "2026-09-10", Propietario = "Maria Hernandez", Mascota = "Luna", Clinica = "PawPort Central San Salvador", Concepto = "Consulta veterinaria general + Antiparasitario", Monto = 34.56m, MetodoPago = "Tarjeta", Estado = "Pagada" }
        };

        public static List<Reseña> Reseñas = new List<Reseña>
        {
            new Reseña { Id = 1, Autor = "Maria Hernandez", Pais = "El Salvador", Clinica = "PawPort Central San Salvador", Comentario = "Excelente atención médica para Luna y Max en la sucursal Escalón.", Calificacion = 5, Fecha = "Hoy" },
            new Reseña { Id = 2, Autor = "Laura Ramirez", Pais = "El Salvador", Clinica = "PawPort Soyapango", Comentario = "El tratamiento dermatológico de Nala con la Dra. Sofía Hernández funcionó de maravilla.", Calificacion = 5, Fecha = "Ayer" },
            new Reseña { Id = 3, Autor = "Roberto Castillo", Pais = "El Salvador", Clinica = "PawPort Santa Ana", Comentario = "El expediente digital unificado nos permitió atender a Toby rápidamente.", Calificacion = 5, Fecha = "Hace 3 días" }
        };

        public static List<Recordatorio> Recordatorios = new List<Recordatorio>
        {
            new Recordatorio { Id = 1, Mascota = "Luna", Propietario = "Maria Hernandez", Tipo = "Vacuna", Mensaje = "Refuerzo anual de vacuna antirrábica programado.", FechaLimite = "2026-10-30", Completado = false },
            new Recordatorio { Id = 2, Mascota = "Maya", Propietario = "Oscar Reyes", Tipo = "Cita", Mensaje = "Seguimiento dermatológico en PawPort Soyapango.", FechaLimite = "2026-10-20", Completado = false }
        };

        public static List<(Usuario Usuario, string Clave)> UsuariosRespaldo = new List<(Usuario, string)>
        {
            (new Usuario { IdUsuario = 1, IdSucursal = null, Nombre = "Josue", Apellido = "Martinez", CorreoElectronico = "admin@pawport.example", Rol = "AdminCentral", Estado = true }, "PawPort123*"),
            (new Usuario { IdUsuario = 2, IdSucursal = 1, Nombre = "Gabriela", Apellido = "Torres", CorreoElectronico = "gabriela.torres@pawport.example", Rol = "AdministradorSucursal", Estado = true }, "PawPort123*"),
            (new Usuario { IdUsuario = 7, IdSucursal = 1, Nombre = "Ana", Apellido = "Lopez", CorreoElectronico = "ana.lopez@pawport.example", Rol = "Veterinario", Estado = true }, "PawPort123*"),
            (new Usuario { IdUsuario = 12, IdSucursal = 1, Nombre = "Maria", Apellido = "Hernandez", CorreoElectronico = "maria.hernandez@correo.example", Rol = "Propietario", Estado = true }, "PawPort123*"),
            (new Usuario { IdUsuario = 101, IdSucursal = null, Nombre = "Alejandra", Apellido = "Gómez", CorreoElectronico = "propietario@pawport.com", Rol = "Propietario", Estado = true }, "1234"),
            (new Usuario { IdUsuario = 102, IdSucursal = 1, Nombre = "Carlos", Apellido = "Mendoza", CorreoElectronico = "veterinario@pawport.com", Rol = "Veterinario", Estado = true }, "1234"),
            (new Usuario { IdUsuario = 103, IdSucursal = 1, Nombre = "Elena", Apellido = "Santos", CorreoElectronico = "sucursal@pawport.com", Rol = "AdministradorSucursal", Estado = true }, "1234"),
            (new Usuario { IdUsuario = 104, IdSucursal = null, Nombre = "Admin", Apellido = "General", CorreoElectronico = "admin@pawport.com", Rol = "AdminCentral", Estado = true }, "1234")
        };

        public static Usuario? ValidarRespaldo(string correo, string clave)
        {
            var encontrado = UsuariosRespaldo.FirstOrDefault(u =>
                u.Usuario.CorreoElectronico.Equals(correo.Trim(), StringComparison.OrdinalIgnoreCase) &&
                u.Clave == clave &&
                u.Usuario.Estado);
            return encontrado.Usuario;
        }

        public static string ClaseEstado(string estado)
        {
            if (estado == "Confirmada" || estado == "Completada" || estado == "Programada" || estado == "Pagada" || estado == "En stock" || estado == "Activo" || estado == "Al día") return "ok";
            if (estado == "Pendiente" || estado == "Stock Bajo") return "wait";
            if (estado == "En consulta" || estado == "En Tratamiento") return "info";
            return "bad";
        }
    }
}
