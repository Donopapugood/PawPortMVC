using System.Collections.Generic;
using System.Linq;
using PawPortMVC.Models;

namespace PawPortMVC.DAL
{
    public static class Datos
    {
        public static List<Clinica> Clinicas = new List<Clinica>
        {
            new Clinica
            {
                Id = 1,
                Nombre = "Clínica Lomas",
                Pais = "México",
                Ciudad = "Ciudad de México",
                Direccion = "Av. Paseo de la Reforma 405, Cuauhtémoc, CDMX",
                Telefono = "+52 55 9876 5432",
                Correo = "lomas@pawport.com",
                HorarioLunesViernes = "08:00 AM - 08:00 PM",
                HorarioSabado = "09:00 AM - 05:00 PM",
                HorarioDomingo = "Urgencias únicamente",
                Imagen = "/recursos/Img/Ciudad_Mexico.png",
                Especialidad = "Medicina general",
                Descripcion = "Sede central en México equipada con quirófano digital, imagenología y hospitalización 24 horas conectada a la red global PawPort.",
                Calificacion = 4.8,
                TotalReseñas = 42
            },
            new Clinica
            {
                Id = 2,
                Nombre = "Clínica Retiro",
                Pais = "España",
                Ciudad = "Madrid",
                Direccion = "Calle de Alcalá 124, Barrio de Salamanca, Madrid",
                Telefono = "+34 91 555 0192",
                Correo = "retiro@pawport.com",
                HorarioLunesViernes = "08:00 AM - 08:00 PM",
                HorarioSabado = "09:00 AM - 06:00 PM",
                HorarioDomingo = "Urgencias 24h",
                Imagen = "/recursos/Img/Madrid.png",
                Especialidad = "Cirugía",
                Descripcion = "Centro europeo especializado en cirugía de tejidos blandos, traumatología y dermatología clínica con acceso al pasaporte médico unificado.",
                Calificacion = 4.7,
                TotalReseñas = 35
            },
            new Clinica
            {
                Id = 3,
                Nombre = "Clínica Palermo",
                Pais = "Argentina",
                Ciudad = "Buenos Aires",
                Direccion = "Av. del Libertador 2201, Palermo, Buenos Aires",
                Telefono = "+54 11 4321 8890",
                Correo = "palermo@pawport.com",
                HorarioLunesViernes = "08:30 AM - 07:30 PM",
                HorarioSabado = "09:00 AM - 04:00 PM",
                HorarioDomingo = "Urgencias únicamente",
                Imagen = "/recursos/Img/Buenos_Aires.png",
                Especialidad = "Dermatología",
                Descripcion = "Clínica integral en Sudamérica dedicada a dermatología, cardiología, odontología y medicina preventiva para mascotas viajeras.",
                Calificacion = 4.9,
                TotalReseñas = 51
            },
            new Clinica
            {
                Id = 4,
                Nombre = "Veterinaria Escalón Central",
                Pais = "El Salvador",
                Ciudad = "San Salvador",
                Direccion = "Colonia Escalón, Calle El Mirador #123, San Salvador",
                Telefono = "+503 2222 0000",
                Correo = "escalon@pawport.com",
                HorarioLunesViernes = "08:00 AM - 08:00 PM",
                HorarioSabado = "08:00 AM - 05:00 PM",
                HorarioDomingo = "Urgencias 24h",
                Imagen = "/recursos/Img/Ciudad_Mexico.png",
                Especialidad = "Medicina general",
                Descripcion = "Sede principal en Centroamérica con laboratorio clínico propio, vacunación internacional y seguimiento digital en tiempo real.",
                Calificacion = 4.9,
                TotalReseñas = 64
            }
        };

        public static List<string> EspecialidadesRed = new List<string>
        {
            "Medicina general",
            "Cirugía",
            "Dermatología",
            "Odontología",
            "Cardiología",
            "Oftalmología"
        };

        public static List<Mascota> Mascotas = new List<Mascota>
        {
            new Mascota
            {
                Id = 1,
                Nombre = "Luna",
                Especie = "Perro",
                Raza = "Golden Retriever",
                Edad = 3,
                Peso = 28.0,
                Sexo = "Hembra",
                Propietario = "Alejandra Gómez",
                Imagen = "/recursos/Img/Luna.png",
                Vacunas = "Al día",
                Estado = "Activo",
                UltimaVisita = "18/10/2026",
                Alergias = "Sensibilidad estomacal a alimentos altos en grasa. Dieta basada en prescripción médica."
            },
            new Mascota
            {
                Id = 2,
                Nombre = "Max",
                Especie = "Perro",
                Raza = "Beagle",
                Edad = 5,
                Peso = 14.5,
                Sexo = "Macho",
                Propietario = "Carlos Ruiz",
                Imagen = "/recursos/Img/Max.png",
                Vacunas = "Pendiente",
                Estado = "En Tratamiento",
                UltimaVisita = "15/10/2026",
                Alergias = "Alergia leve al polen estacional."
            },
            new Mascota
            {
                Id = 3,
                Nombre = "Kiwi",
                Especie = "Ave",
                Raza = "Cacatúa",
                Edad = 2,
                Peso = 0.9,
                Sexo = "Hembra",
                Propietario = "Sofía Pérez",
                Imagen = "/recursos/Img/Kiwi.png",
                Vacunas = "Al día",
                Estado = "Activo",
                UltimaVisita = "10/10/2026",
                Alergias = "Ninguna registrada."
            }
        };

        public static List<Cita> Citas = new List<Cita>
        {
            new Cita
            {
                Id = 1,
                Fecha = "2026-10-18",
                Hora = "09:00",
                Mascota = "Luna",
                Especie = "Perro",
                Propietario = "Alejandra Gómez",
                Clinica = "Veterinaria Escalón Central",
                Veterinario = "Dr. Carlos Mendoza",
                Especialidad = "Medicina general",
                Motivo = "Vacunación anual y chequeo preventivo",
                Notas = "Refuerzo anual antirrábico",
                Estado = "Confirmada"
            },
            new Cita
            {
                Id = 2,
                Fecha = "2026-10-18",
                Hora = "10:30",
                Mascota = "Max",
                Especie = "Perro",
                Propietario = "Carlos Ruiz",
                Clinica = "Clínica Lomas",
                Veterinario = "Dra. Elena Santos",
                Especialidad = "Dermatología",
                Motivo = "Revisión de piel y control de dermatitis",
                Notas = "Presenta enrojecimiento leve",
                Estado = "Pendiente"
            },
            new Cita
            {
                Id = 3,
                Fecha = "2026-10-18",
                Hora = "11:15",
                Mascota = "Kiwi",
                Especie = "Ave",
                Propietario = "Sofía Pérez",
                Clinica = "Clínica Retiro",
                Veterinario = "Dr. Javier Morales",
                Especialidad = "Medicina general",
                Motivo = "Control de plumaje y nutrición",
                Notas = "Chequeo semestral",
                Estado = "En consulta"
            },
            new Cita
            {
                Id = 4,
                Fecha = "2026-10-19",
                Hora = "14:00",
                Mascota = "Luna",
                Especie = "Perro",
                Propietario = "Alejandra Gómez",
                Clinica = "Clínica Palermo",
                Veterinario = "Dr. Carlos Mendoza",
                Especialidad = "Odontología",
                Motivo = "Limpieza dental programada",
                Notas = "Ayuno previo de 8 horas",
                Estado = "Pendiente"
            }
        };

        public static List<Expediente> Expedientes = new List<Expediente>
        {
            new Expediente
            {
                Id = 1,
                IdMascota = 1,
                Mascota = "Luna",
                Propietario = "Alejandra Gómez",
                Clinica = "Veterinaria Escalón Central",
                Veterinario = "Dr. Carlos Mendoza",
                Fecha = "18/10/2026",
                Diagnostico = "Revisión general de rutina. Sin anomalías detectadas en respiración ni auscultación cardíaca.",
                Tratamiento = "Dieta balanceada y control de peso.",
                MedicamentoRecetado = "Desparasitante NextGard Spectra Gdes",
                VacunaAplicada = "Rabia Anual y Múltiple Canina (Sextuple)",
                PesoRegistrado = 28.0,
                Notas = "Paciente activa y en excelente condición física."
            },
            new Expediente
            {
                Id = 2,
                IdMascota = 1,
                Mascota = "Luna",
                Propietario = "Alejandra Gómez",
                Clinica = "Clínica Lomas",
                Veterinario = "Dra. Elena Santos",
                Fecha = "02/08/2026",
                Diagnostico = "Infección leve en oreja derecha (otitis externa).",
                Tratamiento = "Limpieza ótica y aplicación de gotas óticas por 7 días cada 12 horas.",
                MedicamentoRecetado = "Antibiótico Amoxicilina 250mg",
                VacunaAplicada = "Parvovirus (Refuerzo)",
                PesoRegistrado = 27.4,
                Notas = "Evolución favorable."
            },
            new Expediente
            {
                Id = 3,
                IdMascota = 2,
                Mascota = "Max",
                Propietario = "Carlos Ruiz",
                Clinica = "Clínica Lomas",
                Veterinario = "Dra. Elena Santos",
                Fecha = "15/10/2026",
                Diagnostico = "Dermatitis alérgica estacional en extremidades.",
                Tratamiento = "Baños medicados semanales y antihistamínico oral por 10 días.",
                MedicamentoRecetado = "Antibiótico Amoxicilina 250mg",
                VacunaAplicada = "Pendiente refuerzo anual",
                PesoRegistrado = 14.5,
                Notas = "Reevaluar en dos semanas."
            },
            new Expediente
            {
                Id = 4,
                IdMascota = 3,
                Mascota = "Kiwi",
                Propietario = "Sofía Pérez",
                Clinica = "Clínica Retiro",
                Veterinario = "Dr. Javier Morales",
                Fecha = "10/10/2026",
                Diagnostico = "Plumaje saludable y pico en estado óptimo.",
                Tratamiento = "Suplemento vitamínico en agua durante 5 días al mes.",
                MedicamentoRecetado = "Complejo Vitamínico Aviar",
                VacunaAplicada = "Control sanitario al día",
                PesoRegistrado = 0.9,
                Notas = "Sin observaciones adicionales."
            }
        };

        public static List<ProductoInventario> Inventario = new List<ProductoInventario>
        {
            new ProductoInventario
            {
                Id = 1,
                Nombre = "Antibiótico Amoxicilina 250mg",
                Categoria = "Medicamentos",
                Clinica = "Veterinaria Escalón Central",
                StockActual = 120,
                StockMinimo = 20,
                Precio = 14.50m,
                Proveedor = "VetPharma Internacional",
                FechaCaducidad = "2027-06-30"
            },
            new ProductoInventario
            {
                Id = 2,
                Nombre = "Vacuna Rabia Virbac 10ml",
                Categoria = "Vacunas",
                Clinica = "Veterinaria Escalón Central",
                StockActual = 15,
                StockMinimo = 25,
                Precio = 32.00m,
                Proveedor = "BioVet Labs",
                FechaCaducidad = "2027-03-15"
            },
            new ProductoInventario
            {
                Id = 3,
                Nombre = "Jeringas Estériles 3ml (Caja de 100)",
                Categoria = "Insumos",
                Clinica = "Clínica Lomas",
                StockActual = 45,
                StockMinimo = 10,
                Precio = 18.90m,
                Proveedor = "MedSupply Global",
                FechaCaducidad = "2028-12-01"
            },
            new ProductoInventario
            {
                Id = 4,
                Nombre = "Alimento Prescripción Renal Perros 10kg",
                Categoria = "Alimentos",
                Clinica = "Clínica Retiro",
                StockActual = 0,
                StockMinimo = 5,
                Precio = 65.00m,
                Proveedor = "NutriPet Care",
                FechaCaducidad = "2026-12-20"
            },
            new ProductoInventario
            {
                Id = 5,
                Nombre = "Anestésico General Zoletil 5ml",
                Categoria = "Medicamentos",
                Clinica = "Clínica Palermo",
                StockActual = 8,
                StockMinimo = 10,
                Precio = 48.50m,
                Proveedor = "BioVet Labs",
                FechaCaducidad = "2027-05-10"
            },
            new ProductoInventario
            {
                Id = 6,
                Nombre = "Desparasitante NextGard Spectra Gdes",
                Categoria = "Medicamentos",
                Clinica = "Veterinaria Escalón Central",
                StockActual = 95,
                StockMinimo = 15,
                Precio = 22.00m,
                Proveedor = "VetPharma Internacional",
                FechaCaducidad = "2027-09-01"
            }
        };

        public static List<CompraInventario> Compras = new List<CompraInventario>
        {
            new CompraInventario
            {
                Id = 1,
                Fecha = "2026-10-05",
                Producto = "Antibiótico Amoxicilina 250mg",
                Proveedor = "VetPharma Internacional",
                Clinica = "Veterinaria Escalón Central",
                Cantidad = 50,
                CostoUnitario = 9.00m
            },
            new CompraInventario
            {
                Id = 2,
                Fecha = "2026-10-10",
                Producto = "Desparasitante NextGard Spectra Gdes",
                Proveedor = "VetPharma Internacional",
                Clinica = "Veterinaria Escalón Central",
                Cantidad = 40,
                CostoUnitario = 15.00m
            }
        };

        public static List<Veterinario> Veterinarios = new List<Veterinario>
        {
            new Veterinario
            {
                Id = 1,
                Nombre = "Dr. Carlos Mendoza",
                Especialidad = "Cirugía",
                Clinica = "Veterinaria Escalón Central",
                Correo = "cmendoza@pawport.com",
                Telefono = "+503 7111 2233",
                Horario = "Lunes a Viernes 08:00 - 16:00",
                Estado = "Activo"
            },
            new Veterinario
            {
                Id = 2,
                Nombre = "Dra. Elena Santos",
                Especialidad = "Dermatología",
                Clinica = "Clínica Lomas",
                Correo = "esantos@pawport.com",
                Telefono = "+52 55 4433 2211",
                Horario = "Lunes a Sábado 09:00 - 17:00",
                Estado = "Activo"
            },
            new Veterinario
            {
                Id = 3,
                Nombre = "Dr. Javier Morales",
                Especialidad = "Medicina general",
                Clinica = "Clínica Retiro",
                Correo = "jmorales@pawport.com",
                Telefono = "+34 91 667 8899",
                Horario = "Martes a Domingo 10:00 - 18:00",
                Estado = "Activo"
            },
            new Veterinario
            {
                Id = 4,
                Nombre = "Dra. Lucía Fernández",
                Especialidad = "Cardiología",
                Clinica = "Clínica Palermo",
                Correo = "lfernandez@pawport.com",
                Telefono = "+54 11 5566 7788",
                Horario = "Lunes a Viernes 08:30 - 16:30",
                Estado = "Activo"
            }
        };

        public static List<Factura> Facturas = new List<Factura>
        {
            new Factura
            {
                Id = 1,
                NumeroFactura = "FAC-2026-001",
                Fecha = "2026-10-18",
                Propietario = "Alejandra Gómez",
                Mascota = "Luna",
                Clinica = "Veterinaria Escalón Central",
                Concepto = "Consulta general + Vacuna Rabia Anual",
                Monto = 55.00m,
                MetodoPago = "Tarjeta",
                Estado = "Pagada"
            },
            new Factura
            {
                Id = 2,
                NumeroFactura = "FAC-2026-002",
                Fecha = "2026-10-18",
                Propietario = "Carlos Ruiz",
                Mascota = "Max",
                Clinica = "Clínica Lomas",
                Concepto = "Consulta dermatológica + Antibiótico Amoxicilina 250mg",
                Monto = 48.50m,
                MetodoPago = "Efectivo",
                Estado = "Pendiente"
            },
            new Factura
            {
                Id = 3,
                NumeroFactura = "FAC-2026-003",
                Fecha = "2026-10-15",
                Propietario = "Sofía Pérez",
                Mascota = "Kiwi",
                Clinica = "Clínica Retiro",
                Concepto = "Control de aves exóticas + Suplemento vitamínico",
                Monto = 42.00m,
                MetodoPago = "Transferencia",
                Estado = "Pagada"
            }
        };

        public static List<Reseña> Reseñas = new List<Reseña>
        {
            new Reseña
            {
                Id = 1,
                Autor = "Diana Rivera",
                Pais = "El Salvador",
                Clinica = "Veterinaria Escalón Central",
                Comentario = "El servicio en la sucursal de San Salvador fue increíble. Luna estuvo muy tranquila y el doctor contestó todas mis dudas.",
                Calificacion = 5,
                Fecha = "Hoy"
            },
            new Reseña
            {
                Id = 2,
                Autor = "Roberto Castaneda",
                Pais = "El Salvador",
                Clinica = "Veterinaria Escalón Central",
                Comentario = "Me encantan los recordatorios automáticos de vacunas y poder ver el expediente médico desde cualquier país.",
                Calificacion = 5,
                Fecha = "Ayer"
            },
            new Reseña
            {
                Id = 3,
                Autor = "Alejandra Gómez",
                Pais = "México",
                Clinica = "Clínica Lomas",
                Comentario = "Poder ir de viaje de México a España y que la clínica de Madrid pudiera ver todo el historial de Luna al instante fue asombroso.",
                Calificacion = 5,
                Fecha = "Hace 3 días"
            },
            new Reseña
            {
                Id = 4,
                Autor = "Javier Fernández",
                Pais = "España",
                Clinica = "Clínica Retiro",
                Comentario = "Increíble servicio rápido y sin estrés para mi mascota. La agenda en línea funciona excelente.",
                Calificacion = 4,
                Fecha = "Hace 1 semana"
            }
        };

        public static List<Recordatorio> Recordatorios = new List<Recordatorio>
        {
            new Recordatorio
            {
                Id = 1,
                Mascota = "Luna",
                Propietario = "Alejandra Gómez",
                Tipo = "Vacuna",
                Mensaje = "Refuerzo anual de vacuna séxtuple pendiente en 12 días.",
                FechaLimite = "2026-10-30",
                Completado = false
            },
            new Recordatorio
            {
                Id = 2,
                Mascota = "Max",
                Propietario = "Carlos Ruiz",
                Tipo = "Cita",
                Mensaje = "Control dermatológico de seguimiento programado.",
                FechaLimite = "2026-10-25",
                Completado = false
            },
            new Recordatorio
            {
                Id = 3,
                Mascota = "Luna",
                Propietario = "Alejandra Gómez",
                Tipo = "Desparasitación",
                Mensaje = "Próxima dosis de NextGard Spectra.",
                FechaLimite = "2026-11-05",
                Completado = false
            }
        };

        public static List<(Usuario Usuario, string Clave)> UsuariosRespaldo = new List<(Usuario, string)>
        {
            (new Usuario { IdUsuario = 1, IdSucursal = null, Nombre = "Alejandra", Apellido = "Gómez", CorreoElectronico = "propietario@pawport.com", Rol = "Propietario", Estado = true }, "1234"),
            (new Usuario { IdUsuario = 2, IdSucursal = 4, Nombre = "Carlos", Apellido = "Mendoza", CorreoElectronico = "veterinario@pawport.com", Rol = "Veterinario", Estado = true }, "1234"),
            (new Usuario { IdUsuario = 3, IdSucursal = 4, Nombre = "Elena", Apellido = "Santos", CorreoElectronico = "sucursal@pawport.com", Rol = "AdministradorSucursal", Estado = true }, "1234"),
            (new Usuario { IdUsuario = 4, IdSucursal = null, Nombre = "Admin", Apellido = "General", CorreoElectronico = "admin@pawport.com", Rol = "AdminCentral", Estado = true }, "1234")
        };

        public static Usuario? ValidarRespaldo(string correo, string clave)
        {
            var encontrado = UsuariosRespaldo.FirstOrDefault(u =>
                u.Usuario.CorreoElectronico.Equals(correo.Trim(), System.StringComparison.OrdinalIgnoreCase) &&
                u.Clave == clave &&
                u.Usuario.Estado);
            return encontrado.Usuario;
        }

        public static string ClaseEstado(string estado)
        {
            if (estado == "Confirmada" || estado == "Completada" || estado == "Pagada" || estado == "En stock" || estado == "Activo" || estado == "Al día") return "ok";
            if (estado == "Pendiente" || estado == "Stock Bajo") return "wait";
            if (estado == "En consulta" || estado == "En Tratamiento") return "info";
            return "bad";
        }
    }
}
