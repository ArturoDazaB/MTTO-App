using System.Collections.Generic;

namespace MTTO_App
{
    public class OpcionesModel
    {
        //============================================================================================================
        //============================================================================================================
        //VARIABLE LOCALES
        private string nombreopcion, iconfilename;

        //============================================================================================================
        //============================================================================================================
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

        //============================================================================================================
        //============================================================================================================
        //CONSTRUCTOR
        public OpcionesModel()
        {
            nombreopcion = iconfilename = string.Empty;
        }

        //============================================================================================================
        //============================================================================================================
        //FUNCION PARA RETORNAR LA LISTA DE OPCIONES DEL USUARIO ADMINISTRATOR
        public List<OpcionesModel> OpcionesNivelAlto()
        {
            //------------------------------------------------------------------------------------
            /*NOTA: LISTA DE OPCIONES DISPONIBLES PARA NAVEGAR DENTRO DE LA APLICACION A USUARIOS 
             * DE ALTO NIVEL. EJ: EL USUARIO ADMINISTRATOR*/
            //------------------------------------------------------------------------------------

            List<OpcionesModel> listaDeOpciones = new List<OpcionesModel>()
            {
                new OpcionesModel()
                {
                    nombreopcion = "Consulta",
                    iconfilename = "Consulta.png"
                },
                new OpcionesModel()
                {
                    nombreopcion = "Nuevo Tablero",
                    iconfilename = "Plus.png",
                },
                new OpcionesModel()
                {
                    nombreopcion = "Registro",
                    iconfilename = "Registro.png"
                },
                new OpcionesModel()
                {
                    nombreopcion = "Configuracion",
                    iconfilename = "Configuracion.png"
                },
                new OpcionesModel()
                {
                    nombreopcion = "Salir",
                    iconfilename = "Salir.png"
                },
            };

            return listaDeOpciones;
        }

        //============================================================================================================
        //============================================================================================================
        //FUNCION PARA RETORNAR LA LISTA DE OPCIONES DEL USUARIOS DE BAJO NIVEL
        public List<OpcionesModel> OpcionesNivelBajo()
        {
            //------------------------------------------------------------------------------------
            /*NOTA: LISTA DE OPCIONES DISPONIBLES PARA NAVEGAR DENTRO DE LA APLICACION A USUARIOS 
             *DE BAJO NIVEL. EJ: USUARIOS QUE SOLO TIENEN PERMITIDA LA CONSULTA Y MODIFICACION
              DE DATOS PERSONALES*/
            //------------------------------------------------------------------------------------

            List<OpcionesModel> listaDeOpciones = new List<OpcionesModel>()
            {
                new OpcionesModel()
                {
                    nombreopcion = "Consulta",
                    iconfilename = "Consulta.png"
                },
                new OpcionesModel()
                {
                    nombreopcion = "Configuracion",
                    iconfilename = "Configuracion.png"
                },
                new OpcionesModel()
                {
                    nombreopcion = "Salir",
                    iconfilename = "Salir.png"
                },
            };

            return listaDeOpciones;
        }

        //============================================================================================================
        //============================================================================================================
        //FUNCION PARA RETORNAR LA LISTA DE OPCIONES DEL USUARIOS DE MEDIO NIVEL
        public List<OpcionesModel> OpcionesNivelMedio()
        {
            //------------------------------------------------------------------------------------
            /*NOTA: LISTA DE OPCIONES DISPONIBLES PARA NAVEGAR DENTRO DE LA APLICACION A USUARIOS 
             *DE MEDIO NIVEL. EJ: USUARIOS ASIGNADOS A SUPERVISORES O GERENTES(?), PERMITIENDO
             *EL REGISTRO DE NUEVOS USUARIOS DENTRO DE LA PLATAFORMA, Y CONSULTA DE TABLEROS*/
            //------------------------------------------------------------------------------------

            return new List<OpcionesModel>()
            {
                //OPCION CONSULTA
                new OpcionesModel()
                {
                    nombreopcion = "Consulta",
                    iconfilename = "Consulta.png",
                },
                //OPCION REGISTRO
                new OpcionesModel()
                {
                    nombreopcion = "Registro",
                    iconfilename = "Registro.png",
                },
                //OPCION CONFIGURACION
                new OpcionesModel()
                {
                    nombreopcion = "Configuracion",
                    iconfilename = "Configuracion.png",
                },
                //SALIR
                new OpcionesModel()
                {
                    nombreopcion = "Salir",
                    iconfilename = "Salir.png",
                }
            };
        }
    }
}