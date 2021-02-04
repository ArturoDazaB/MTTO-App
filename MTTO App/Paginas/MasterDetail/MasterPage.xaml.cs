﻿using MTTO_App.Paginas;
using MTTO_App.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTTO_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        //============================================================================================
        //============================================================================================
        //OBJETOS
        //SE CREAN LAS VARIABLES Y OBJETOS GLOBALES DE LA CLASE
        private Personas Persona;
        private Usuarios Usuario;
        private UltimaConexion Ultimaconexion; //=> APP STANS ALONE
        private DateTime UltimaFechaIngreso; //=> APP CONSUMO DE API
        private MasterDetailPageUserInfoViewModel DatosPagina;
        private OpcionesViewModel OpcionesViewModel;
        //private string Codigo = string.Empty;

        //============================================================================================
        //============================================================================================
        //CONSTRUCTOR (APP STAND ALONE)
        public MasterPage(Personas per, Usuarios usu, UltimaConexion ultima)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS Persona, Usuario y UltimaConexion PARA LUEGO LLENARLOS CON LOS DATOS 
            //DEL USUARIO QUE SE ENCUENTRA NAVEGANDO
            Persona = new Personas().NewPersona(per);
            Usuario = new Usuarios().NewUsuario(usu);
            Ultimaconexion = new UltimaConexion().NewUltimaConexion(ultima);

            //SE INSTANCIA LOS DATOS DE LA PAGINA MEDIANTE EL LLAMADO DE LA CLASE "MasterDetailPageUserInfoViewModel.cs" 
            //JUNTO CON LOS DATOS DEL USUARIO QUE SE ENCUENTRE LOGGEADO
            BindingContext = DatosPagina = new MasterDetailPageUserInfoViewModel(Persona, Usuario, Ultimaconexion);


            //CONFIGURACION DEL MENU DE OPCIONES DEPENDIENDO DEL NIVEL DEL USUARIO QUE SE ENCUENTRE LOGGEADO
            //EL MENU LATERAL LLENARA LAS OPCIONES Y CARGARA LA INFORMACION DE MANERA DISTINTA. EN OTRAS PALABRAS, 
            //SE GENERO UNA LISTA DE OPCIONES PARA EL USUARIO ADMINISTRATOR Y OTRA PARA LOS USUARIOS DE BAJO NIVEL

            switch (Usuario.NivelUsuario)
            {
                //USUARIO DE BAJO NIVEL
                case 0:
                    ListaNavegacion.ItemsSource = new OpcionesModel().OpcionesNivelBajo();
                    break;
                //USUARIO DE MEDIO NIVEL 
                case 5:
                    ListaNavegacion.ItemsSource = new OpcionesModel().OpcionesNivelMedio();
                    break;
                //USUARIO DE ALTO NIVEL
                case 10:
                    ListaNavegacion.ItemsSource = new OpcionesModel().OpcionesNivelAlto();
                    break;
            }
        }

        //============================================================================================
        //============================================================================================
        //CONSTRUCTOR (APP CONSUMO DE API)
        public MasterPage(Personas per, Usuarios usu, DateTime ultima)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS Persona, Usuario y UltimaConexion PARA LUEGO
            //LLENARLOS CON LOS DATOS DEL USUARIO QUE SE ENCUENTRA NAVEGANDO
            Persona = new Personas().NewPersona(per);
            Usuario = new Usuarios().NewUsuario(usu);
            UltimaFechaIngreso = ultima;
            //SE INSTANCIA LOS DATOS DE LA PAGINA MEDIANTE EL LLAMADO DE LA CLASE "MasterDetailPageUserInfoViewModel.cs" 
            //JUNTO CON LOS DATOS DEL USUARIO QUE SE ENCUENTRE LOGGEADO
            BindingContext = DatosPagina = new MasterDetailPageUserInfoViewModel(Persona, Usuario, UltimaFechaIngreso);

            //CONFIGURACION DEL MENU DE OPCIONES DEPENDIENDO DEL NIVEL DEL USUARIO QUE SE ENCUENTRE LOGGEADO
            //EL MENU LATERAL LLENARA LAS OPCIONES Y CARGARA LA INFORMACION DE MANERA DISTINTA. EN OTRAS PALABRAS, 
            //SE GENERO UNA LISTA DE OPCIONES PARA EL USUARIO ADMINISTRATOR Y OTRA PARA LOS USUARIOS DE BAJO NIVEL
            switch (Usuario.NivelUsuario)
            {
                //USUARIO DE BAJO NIVEL
                case 0:
                    ListaNavegacion.ItemsSource = new OpcionesModel().OpcionesNivelBajo();
                    break;
                //USUARIO DE MEDIO NIVEL 
                case 5:
                    ListaNavegacion.ItemsSource = new OpcionesModel().OpcionesNivelMedio();
                    break;
                //USUARIO DE ALTO NIVEL
                case 10:
                    ListaNavegacion.ItemsSource = new OpcionesModel().OpcionesNivelAlto();
                    break;
            }
        }

        //============================================================================================
        //============================================================================================
        //FUNCION PARA IDENTIFICAR QUE OPCION A SELECCIONADO EL USUARIO DEL MENÚ Y REALIZAR EL 
        //LLAMADO DE LA PAGINA
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //SE EVALUA MEDIANTE UN CICLO "Switch" CUAL EL EL NIVEL DE USUARIO QUE POSEE
            //EL USUARIO QUE SE ENCUENTRA NAVEGANDO.
            //NOTA: ACTUALMENTE SOLO EXISTEN TRES NIVELES DE USUARIO
            switch (Usuario.NivelUsuario)
            {
                //NIVEL BAJO
                case 0:
                    //--------------------------------------------------------------------------------------------------------------
                    //OPCIONED NIVEL BAJO
                    //SE EVALUA MEDIANTE UN CICLO "Switch" CUAL ES LA OPCION SELECCIONADA POR EL USUARIO
                    switch (e.SelectedItemIndex)
                    {
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONSULTA
                        case 0:
                            //SE OCULTA EL MENÚ DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE HACE EL LLAMADO A LA PAGINA DE CONSULTA DE TABLEROS
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConsultaTablero(Persona, Usuario));
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONFIGURACION DE DATOS DEL USUARIO
                        case 1:
                            //SE OCULTA EL MENÚ DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE HACE EL LLAMADO A LA PAGINA DE CONFIGURACION DE DATOS
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConfiguracion(Persona, Usuario));
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //SALIDA
                        case 2:
                            //SE OCULTA EL MENÚ DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            //SE CIERRA LA PAGINA "UserMainPage"
                            await Navigation.PopModalAsync();
                            break;
                    }
                    //--------------------------------------------------------------------------------------------------------------
                    break;
                //NIVEL MEDIO
                case 5:
                    //--------------------------------------------------------------------------------------------------------------
                    //OPCIONES NIVEL MEDIO
                    switch (e.SelectedItemIndex)
                    {
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONSULTA
                        case 0:
                            //SE OCULTA EL MENÚ DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE HACE EL LLAMADO A LA PAGINA DE CONSULTA DE TABLEROS
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConsultaTablero(Persona, Usuario));
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE REGISTRO DE USUARIOS
                        case 1:
                            //SE OCULTA EL MENÚ DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE HACE EL LLAMADO A LA PAGINA DE REGISTRO DE USUARIOS
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaRegistro(Persona, Usuario));
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONFIGURACION (CAMBIO DE DATOS PERSONALES)
                        case 2:
                            //SE OCULTA EL MENÚ DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE HACE EL LLAMADO A LA PAGINA DE CONFIGURACION
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConfiguracion(Persona, Usuario));
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //SALIDA
                        case 3:
                            //SE OCULTA EL MENÚ DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            //SE CIERRA LA PAGINA "UserMainPage"
                            await Navigation.PopModalAsync();
                            break;
                    }
                    //--------------------------------------------------------------------------------------------------------------
                    break;
                //NIVEL ALTO (ACTUALMENTE EL USUARIO ADMINISTRATOR)
                case 10:
                    //--------------------------------------------------------------------------------------------------------------
                    //OPCIONES NIVEL ALTO (ACTUALMENTE EL USUARIO ADMINISTRATOR)
                    switch (e.SelectedItemIndex)
                    {
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONSULTA
                        case 0:
                            //SE OCULTA EL MENÚ DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE HACE EL LLAMADO A LA PAGINA DE CONSULTA DE TABLEROS
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConsultaTablero(Persona, Usuario));
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE ADICION DE NUEVO TABLERO
                        case 1:
                            //SE OCULTA EL MENÚ DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE HACE EL LLAMADO A LA PAGINA DE REGISTRO DE TABLEROS
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaRegistroTablero(Persona, Usuario));
                            //SE DESELECCIONA LA OPCION SELECIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE REGISTRO DE NUEVOS USUARIOS
                        case 2:
                            //SE OCULTA EL MENÚ DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE HACE EL LLAMADO A LA PAGINA DE REGISTRO DE USUARIOS
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaRegistro(Persona, Usuario));
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONFIGURACION DE USUARIOS (ADMINISTRATOR)
                        case 3:
                            //SE OCULTA EL MENU DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE HACE EL LLAMADO A LA PAGINA DE CONFIGURACION DE USUARIOS (PAGINA QUERY ADMIN)
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaQueryAdmin(Persona, Usuario));
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO 
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //SALIDA
                        case 4:
                            //SE OCULTA EL MENU DE OPCIONES
                            App.MasterDetail.IsPresented = false;
                            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
                            ((ListView)sender).SelectedItem = null;
                            //SE CIERRA LA PAGINA "UserMainPage"
                            await Navigation.PopModalAsync();
                            break;
                    }
                    //--------------------------------------------------------------------------------------------------------------
                    break;
            }
        }
    }
}