using MTTO_App.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTTO_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        private Personas Persona;
        private Usuarios Usuario;
        private UltimaConexion UltimaConexion;
        private DateTime UltimaFechaIngreso;
        private MasterDetailPageUserInfoViewModel DatosPagina;

        public DetailPage(Personas per, Usuarios usu, UltimaConexion ulti)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS GLOBALES
            Persona = new Personas().NewPersona(per);
            Usuario = new Usuarios().NewUsuario(usu);
            UltimaConexion = new UltimaConexion().NewUltimaConexion(ulti);

            BindingContext = DatosPagina = new MasterDetailPageUserInfoViewModel(Persona, Usuario, UltimaConexion);
        }

        public DetailPage(Personas per, Usuarios usu, DateTime ulti)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS GLOBALES
            Persona = new Personas().NewPersona(per);
            Usuario = new Usuarios().NewUsuario(usu);
            UltimaFechaIngreso = ulti;

            BindingContext = DatosPagina = new MasterDetailPageUserInfoViewModel(Persona, Usuario, UltimaFechaIngreso);
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //FUNCION QUE INHABILITA EL BackButton NATIVO DE ANDROID
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
    }
}