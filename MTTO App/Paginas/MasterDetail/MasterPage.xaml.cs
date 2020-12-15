using MTTO_App.Paginas;
using MTTO_App.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTTO_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        //=======================================================================
        //=======================================================================
        //OBJETOS DE LA CLASE
        private Personas Persona;
        private Usuarios Usuario;
        private UltimaConexion Ultimaconexion;
        private DateTime UltimaFechaIngreso;

        //=======================================================================
        //=======================================================================
        //OBJETO DE LA CLASE VIEWMODEL
        private MasterDetailPageUserInfoViewModel DatosPagina;

        private OpcionesViewModel OpcionesViewModel;

        //=======================================================================
        //=======================================================================
        //CREACION DE VARIABLES DE LA CLASE
        private string Codigo = string.Empty;

        //=======================================================================
        //=======================================================================
        //CONSTRUCTOR
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

            //SE INSTANCIA LA CLASE OpcionesViewModel.cs
            //OpcionesViewModel = new OpcionesViewModel();

            //CONFIGURACION DEL MENU DE OPCIONES DEPENDIENDO DEL NIVEL DEL USUARIO QUE SE ENCUENTRE LOGGEADO
            //EL MENU LATERAL LLENARA LAS OPCIONES Y CARGARA LA INFORMACION DE MANERA DISTINTA. EN OTRAS PALABRAS, 
            //SE GENERO UNA LISTA DE OPCIONES PARA EL USUARIO ADMINISTRATOR Y OTRA PARA LOS USUARIOS DE BAJO NIVEL

            switch (Usuario.NivelUsuario)
            {
                case 0:
                    ListaNavegacion.ItemsSource = new OpcionesModel().OpcionesNivelBajo();
                    break;

                case 5:
                    ListaNavegacion.ItemsSource = new OpcionesModel().OpcionesNivelMedio();
                    break;

                case 10:
                    ListaNavegacion.ItemsSource = new OpcionesModel().OpcionesNivelAlto();
                    break;
            }
        }

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
            //SE INSTANCIA LA CLASE OpcionesViewModel.cs
            OpcionesViewModel = new OpcionesViewModel();

            switch (Usuario.NivelUsuario)
            {
                case 0:
                    ListaNavegacion.ItemsSource = OpcionesViewModel.OpcionesNivelBajo;
                    break;

                case 5:
                    ListaNavegacion.ItemsSource = OpcionesViewModel.OpcionesNivelMedio;
                    break;

                case 10:
                    ListaNavegacion.ItemsSource = OpcionesViewModel.OpcionesNivelAlto;
                    break;
            }
        }

        //============================================================================================
        //============================================================================================
        //LLAMADO DE PAGINA EN EL MENU DE OPCIONES

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            switch (Usuario.NivelUsuario)
            {
                case 0:
                    //--------------------------------------------------------------------------------------------------------------
                    //OPCIONED NIVEL BAJO
                    switch (e.SelectedItemIndex)
                    {
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONSULTA
                        case 0:
                            App.MasterDetail.IsPresented = false;
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConsultaTablero(Persona, Usuario));
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONFIGURACION DE DATOS DEL USUARIO
                        case 1:
                            App.MasterDetail.IsPresented = false;
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConfiguracion(Persona, Usuario));
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //SALIDA
                        case 2:
                            App.MasterDetail.IsPresented = false;
                            ((ListView)sender).SelectedItem = null;
                            await Navigation.PopModalAsync();
                            break;
                    }
                    //--------------------------------------------------------------------------------------------------------------
                    break;

                case 5:
                    //--------------------------------------------------------------------------------------------------------------
                    //OPCIONES NIVEL MEDIO
                    switch (e.SelectedItemIndex)
                    {
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONSULTA
                        case 0:
                            App.MasterDetail.IsPresented = false;
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConsultaTablero(Persona, Usuario));
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE REGISTRO DE USUARIOS
                        case 1:
                            App.MasterDetail.IsPresented = false;
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaRegistro(Persona, Usuario));
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONFIGURACION (CAMBIO DE DATOS PERSONALES)
                        case 2:
                            App.MasterDetail.IsPresented = false;
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConfiguracion(Persona, Usuario));
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //SALIDA
                        case 3:
                            App.MasterDetail.IsPresented = false;
                            ((ListView)sender).SelectedItem = null;
                            await Navigation.PopModalAsync();
                            break;
                    }
                    //--------------------------------------------------------------------------------------------------------------
                    break;

                case 10:
                    //--------------------------------------------------------------------------------------------------------------
                    //OPCIONES NIVEL ALTO
                    switch (e.SelectedItemIndex)
                    {
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONSULTA
                        case 0:
                            App.MasterDetail.IsPresented = false;
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConsultaTablero(Persona, Usuario));
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE ADICION DE NUEVO TABLERO
                        case 1:
                            App.MasterDetail.IsPresented = false;
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaRegistroTablero(Persona, Usuario));
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE REGISTRO DE NUEVOS USUARIOS
                        case 2:
                            App.MasterDetail.IsPresented = false;
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaRegistro(Persona, Usuario));
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //PAGINA DE CONFIGURACION DE USUARIOS (ADMINISTRATOR)
                        case 3:
                            App.MasterDetail.IsPresented = false;
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaQueryAdmin(Persona, Usuario));
                            ((ListView)sender).SelectedItem = null;
                            break;
                        //===================================================================
                        //===================================================================
                        //SALIDA
                        case 4:
                            App.MasterDetail.IsPresented = false;
                            ((ListView)sender).SelectedItem = null;
                            await Navigation.PopModalAsync();
                            break;
                    }
                    //--------------------------------------------------------------------------------------------------------------
                    break;
            }
        }
    }
}