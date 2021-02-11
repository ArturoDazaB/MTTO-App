using System;
using System.Collections.Generic;

namespace MTTO_App
{
    internal class ConfiguracionAdminModel
    {
        //====================================================================
        //====================================================================
        //VARIABLES LOCALES

        private DateTime fechacreacion, fechanacimiento;
        private string nombres, apellidos, correo, username, password, cedula, telefono;

        //====================================================================
        //====================================================================
        //ATRIBUTOS DE LA CLASE

        public DateTime FechaCreacion
        {
            get { return fechacreacion; }
            set { fechacreacion = value; }
        }

        public string Nombres
        {
            get { return nombres; }
            set { nombres = value; }
        }

        public string Apellidos
        {
            get { return apellidos; }
            set { nombres = value; }
        }

        public string Cedula
        {
            get { return cedula; }
            set { cedula = value; }
        }

        public DateTime FechaNacimiento
        {
            get { return fechanacimiento; }
            set { fechanacimiento = value; }
        }

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        //=====================================================================================================
        //=====================================================================================================
        //INFORMACION A DESPLEGAR PARA EL USUARIO ADMINISTRATOR

        public List<ConfiguracionAdminModel> GetPersonalInfo(Personas Persona)
        {
            return new List<ConfiguracionAdminModel>()
            {
                new ConfiguracionAdminModel()
                {
                    fechacreacion = Persona.FechaCreacion,
                    fechanacimiento = Persona.FechaNacimiento,
                    nombres = Persona.Nombres,
                    apellidos = Persona.Apellidos,
                    cedula = Persona.Cedula.ToString(),
                    correo = Persona.Correo,
                    telefono = Persona.Telefono.ToString(),
                }
            };
        }

        public List<ConfiguracionAdminModel> GetUserInfo(Usuarios Usuario)
        {
            return new List<ConfiguracionAdminModel>()
            {
                new ConfiguracionAdminModel()
                {
                    username = Usuario.Username,
                    password = Usuario.Password,
                }
            };
        }

        //=====================================================================================================
        //=====================================================================================================
    }
}