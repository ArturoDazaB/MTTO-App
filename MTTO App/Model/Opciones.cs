using System.Collections.Generic;

namespace MTTO_App
{
    public class Opciones
    {
        //=======================================================================
        //=======================================================================
        //VARIABLE LOCALES
        private string nombreopcion, iconfilename;

        //=======================================================================
        //=======================================================================
        //CONSTRUCTOR
        public Opciones()
        {
            nombreopcion = iconfilename = string.Empty;
        }

        //=======================================================================
        //=======================================================================
        //ATRIBUTOS DE LA CLASE Opciones: nombreOpcion, imageFilename
        public string NombreOpcion
        {
            get { return nombreopcion; }
            set { nombreopcion = value; }
        }

        public string IconFileName
        {
            get { return iconfilename; }
            set { iconfilename = value; }
        }

        //=======================================================================
        //=======================================================================
        //FUNCION PARA RETORNAR LA LISTA DE OPCIONES DEL USUARIO ADMINISTRATOR

        public List<Opciones> OpcionesNivelAlto()
        {
            List<Opciones> listaDeOpciones = new List<Opciones>()
            {
                new Opciones()
                {
                    nombreopcion = "Consulta",
                    iconfilename = "Consulta.png"
                },
                new Opciones()
                {
                    nombreopcion = "Nuevo Tablero",
                    iconfilename = "Plus.png",
                },
                new Opciones()
                {
                    nombreopcion = "Registro",
                    iconfilename = "Registro.png"
                },
                new Opciones()
                {
                    nombreopcion = "Configuracion",
                    iconfilename = "Configuracion.png"
                },
                new Opciones()
                {
                    nombreopcion = "Salir",
                    iconfilename = "Salir.png"
                },
            };

            return listaDeOpciones;
        }

        //=======================================================================
        //=======================================================================
        //FUNCION PARA RETORNAR LA LISTA DE OPCIONES DEL USUARIOS DE BAJO NIVEL

        public List<Opciones> OpcionesNivelBajo()
        {
            List<Opciones> listaDeOpciones = new List<Opciones>()
            {
                new Opciones()
                {
                    nombreopcion = "Consulta",
                    iconfilename = "Consulta.png"
                },
                new Opciones()
                {
                    nombreopcion = "Configuracion",
                    iconfilename = "Configuracion.png"
                },
                new Opciones()
                {
                    nombreopcion = "Salir",
                    iconfilename = "Salir.png"
                },
            };

            return listaDeOpciones;
        }

        public List<Opciones> OpcionesNivelMedio()
        {
            return new List<Opciones>()
            {
                //OPCION CONSULTA
                new Opciones()
                {
                    nombreopcion = "Consulta",
                    iconfilename = "Consulta.png",
                },
                //OPCION REGISTRO
                new Opciones()
                {
                    nombreopcion = "Registro",
                    iconfilename = "Registro.png",
                },
                //OPCION CONFIGURACION
                new Opciones()
                {
                    nombreopcion = "Configuracion",
                    iconfilename = "Configuracion.png",
                },
                //SALIR
                new Opciones()
                {
                    nombreopcion = "Salir",
                    iconfilename = "Salir.png",
                }
            };
        }
    }
}