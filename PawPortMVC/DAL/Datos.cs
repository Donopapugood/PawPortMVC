using System.Collections.Generic;
using PawPortMVC.Models;

namespace PawPortMVC.DAL
{
    //archivo temporal (mientras no se conecte SQL)
    public static class Datos
    {
        public static List<Clinica> Clinicas = new List<Clinica>
        {
            new Clinica { Nombre = "Clínica Lomas", Ciudad = "Ciudad de México", Imagen = "/recursos/Img/Ciudad_Mexico.png", Especialidad = "Medicina general", Calificacion = 4.8 },
            new Clinica { Nombre = "Clínica Retiro", Ciudad = "Madrid", Imagen = "/recursos/Img/Madrid.png", Especialidad = "Cirugía", Calificacion = 4.7 },
            new Clinica { Nombre = "Clínica Palermo", Ciudad = "Buenos Aires", Imagen = "/recursos/Img/Buenos_Aires.png", Especialidad = "Dermatología", Calificacion = 4.9 }
        };

        public static List<Mascota> Mascotas = new List<Mascota>
        {
            new Mascota { Nombre = "Luna", Especie = "Perro", Raza = "Golden Retriever", Edad = 3, Imagen = "/recursos/Img/Luna.png", Vacunas = "Al día" },
            new Mascota { Nombre = "Max", Especie = "Perro", Raza = "Beagle", Edad = 5, Imagen = "/recursos/Img/Max.png", Vacunas = "Pendiente" },
            new Mascota { Nombre = "Kiwi", Especie = "Ave", Raza = "Cacatúa", Edad = 2, Imagen = "/recursos/Img/Kiwi.png", Vacunas = "Al día" }
        };

        public static List<Cita> Citas = new List<Cita>
        {
            new Cita { Id = 1, Hora = "09:00", Mascota = "Luna", Propietario = "Ana Gómez", Motivo = "Vacunación anual", Estado = "Confirmada" },
            new Cita { Id = 2, Hora = "10:30", Mascota = "Max", Propietario = "Carlos Ruiz", Motivo = "Revisión de piel", Estado = "Pendiente" },
            new Cita { Id = 3, Hora = "11:15", Mascota = "Kiwi", Propietario = "Sofía Pérez", Motivo = "Control de plumaje", Estado = "En consulta" },
            new Cita { Id = 4, Hora = "14:00", Mascota = "Rocky", Propietario = "Luis Mena", Motivo = "Cirugía menor", Estado = "Pendiente" }
        };

        public static string ClaseEstado(string estado)
        {
            if (estado == "Confirmada") return "ok";
            if (estado == "Pendiente") return "wait";
            if (estado == "En consulta") return "info";
            return "bad";
        }
    }
}