﻿using System.Collections.Generic;

namespace MTTO_App
{
    public class OpcionesModel
    {

        //============================================================================================================
        //============================================================================================================
        //PROPIEDADES
        //NOMBRE DE LA OPCION
        public string NombreOpcion { get; private set; }
        //ICONO DE LA OPCION
        public string IconFileName { get; private set; }

        public OpcionesModel()
        {
            NombreOpcion = IconFileName = string.Empty;
        }

        //============================================================================================================
        //============================================================================================================
        //FUNCION PARA RETORNAR LA LISTA DE OPCIONES DEL USUARIO DE NIVEL ALTO
        //NOTA: ACTUALMENTE EL UNICO USUARIO CON NIVEL ALTO ES EL USUARIO ADMINISTRATOR
        public List<OpcionesModel> OpcionesNivelAlto()
        {
            //------------------------------------------------------------------------------------
            /*NOTA: LISTA DE OPCIONES DISPONIBLES PARA NAVEGAR DENTRO DE LA APLICACION A USUARIOS 
             *DE ALTO NIVEL. EJ: USUARIOS QUE PUEDEN CREAR Y CONSULTAR TABLEROS; CREAR, CONSULTAR 
             Y MODIFICAR USUARIOS*/
            //------------------------------------------------------------------------------------
            //SE CREA E INICIALIZA UNA LISTA DE OBJETOS DE TIPO "OpcionesModel" (LISTA DE OPCIONES)
            return new List<OpcionesModel>()
            {
                //DENTRO DE LA LISTA DE OPCIONES SE CREAN E INICIALIZAN CADA UNA DE LAS OPCIONES
                //CREACION E INICIALIZACION DE LA OPCION "Consulta de Tableros"
                new OpcionesModel()
                {
                    NombreOpcion = "Consulta",
                    IconFileName = "Consulta.png"
                },
                //CREACION E INICIALIZACION DE LA OPCION "Registro de Tablero"
                new OpcionesModel()
                {
                    NombreOpcion = "Nuevo Tablero",
                    IconFileName = "Plus.png",
                },
                //CREACION E INICIALIZACION DE LA OPCION "Registro de Usuario"
                new OpcionesModel()
                {
                    NombreOpcion = "Registro",
                    IconFileName = "Registro.png"
                },
                //CREACION E INICIALIZACION DE LA OPCION "Configuracion" (Configuracion Administrator)
                new OpcionesModel()
                {
                    NombreOpcion = "Configuracion",
                    IconFileName = "Configuracion.png"
                },
                //CREACION E INICIALIZACION DE LA OPCION "Salir"
                new OpcionesModel()
                {
                    NombreOpcion = "Salir",
                    IconFileName = "Salir.png"
                },
            };
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
            //SE CREA E INICIALIZA UNA LISTA DE OBJETOS DE TIPO "OpcionesModel" (LISTA DE OPCIONES)
            return new List<OpcionesModel>()
            {
                //DENTRO DE LA LISTA DE OPCIONES SE CREAN E INICIALIZAN CADA UNA DE LAS OPCIONES
                //CREACION E INICIALIZACION DE LA OPCION "Consulta de Tableros"
                new OpcionesModel()
                {
                    NombreOpcion = "Consulta",
                    IconFileName = "Consulta.png"
                },
                //CREACION E INICIALIZACION DE LA OPCION "Configuracion de Informacion"
                new OpcionesModel()
                {
                    NombreOpcion = "Configuracion",
                    IconFileName = "Configuracion.png"
                },
                //CREACION E INICIALIZACION DE LA OPCION "Salir"
                new OpcionesModel()
                {
                    NombreOpcion = "Salir",
                    IconFileName = "Salir.png"
                },
            };
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
            //SE CREA E INICIALIZA UNA LISTA DE OBJETOS DE TIPO "OpcionesModel" (LISTA DE OPCIONES)
            return new List<OpcionesModel>()
            {
                //CREACION E INICIALIZACION DE LA OPCION "Consulta de Tableros"
                new OpcionesModel()
                {
                    NombreOpcion = "Consulta",
                    IconFileName = "Consulta.png",
                },
                //CREACION E INICIALIZACION DE LA OPCION "Registro de Usuarios"
                new OpcionesModel()
                {
                    NombreOpcion = "Registro",
                    IconFileName = "Registro.png",
                },
                //CREACION E INICIALIZACION DE LA OPCION "Configuracion"
                new OpcionesModel()
                {
                    NombreOpcion = "Configuracion",
                    IconFileName = "Configuracion.png",
                },
                //CREACION E INICIALIZACION DE LA OPCION "Salir"
                new OpcionesModel()
                {
                    NombreOpcion = "Salir",
                    IconFileName = "Salir.png",
                }
            };
        }
    }
}