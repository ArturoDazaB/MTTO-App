using MTTO_App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTTO_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        Personas Persona;
        Usuarios Usuario;
        UltimaConexion UltimaConexion;
        MasterDetailPageUserInfoViewModel DatosPagina;

        public DetailPage(Personas per, Usuarios usu, UltimaConexion ulti)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS GLOBALES
            Persona = new Personas().NewPersona(per);
            Usuario = new Usuarios().NewUsuario(usu);
            UltimaConexion = new UltimaConexion().NewUltimaConexion(ulti);

            BindingContext = DatosPagina = new MasterDetailPageUserInfoViewModel(Persona, Usuario, UltimaConexion);
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