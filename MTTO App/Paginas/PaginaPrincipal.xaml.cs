using MTTO_App.Tablas;
using MTTO_App.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Essentials;

namespace MTTO_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPrincipal : ContentPage
    {
        //CREACION E INICIALIZACION DE LOS OBJETOS
        private PaginaPrincipalViewModel ConexionDatos;
        private LogInResponse loginresponse;

        //=====================================================================================================================
        //=====================================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaPrincipal()
        {
            InitializeComponent();

            BindingContext = ConexionDatos = new PaginaPrincipalViewModel();
        }

        //=====================================================================================================================
        //=====================================================================================================================
        //VALIDACION PARA EL INGRESO DE USUARIO
        async public void OnIngresar(Object sender, EventArgs e)
        {
            //SE DA SET A LA ALARMA DE MATCH DE USUARIO

            if(Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await DisplayAlert("Mensaje", ConexionDatos.NetWorkConnectivityChangeMessage, ConexionDatos.OkText);
            }
            else if (Connectivity.NetworkAccess != NetworkAccess.None)
            {
                if (string.IsNullOrEmpty(ConexionDatos.Username) ||     //LOS ENTRY DEBEN TENER CARACTERES PARA PODER
                string.IsNullOrEmpty(ConexionDatos.Password))       //REALIZAR LA BUSQUEDA DE USUARIO
                {
                    await DisplayAlert("Mensaje", ConexionDatos.UsernamePasswordEmptyMessage, ConexionDatos.OkText);
                }
                else
                {
                    //------------------------------------------------------------------------------
                    //CONEXION CON LA BASE DE DATOS (CLIENTE - SERVIDOR)
                    //SE LLAMA AL METODO QUE REALIZA LA SOLICITUD HTTP
                    ActivityIndicator.IsVisible = true;
                    ActivityIndicator.IsRunning = true;
                    await Task.Run(async () =>
                    {
                        loginresponse = await ConexionDatos.LogInRequest();
                        ActivityIndicator.IsRunning = false;
                    });

                    //SE DESACTIVA LA VISIBILIDAD DEL OBJETO ACTIVITYINDICATOR
                    ActivityIndicator.IsVisible = false;

                    //SE EVALUA EL ESTADO DE LA RESPUESTA
                    if (loginresponse != null)
                    {
                        //SE REALIZA EL LLAMADO A LA PAGINA "UserMainPage"
                        await Navigation.PushModalAsync(new UserMainPage(loginresponse.UserInfo.Persona, loginresponse.UserInfo.Usuario, loginresponse.UltimaFechaIngreso));

                        //SI LA RESPUESTA ES DIFERENTE DE NULL => SE OBTUVO UN RESULTADO
                        //SE LIMPIAN LAS ENTRADAS DE TEXTO PARA EL USERNAME Y PARA EL PASSWORD
                        usernameEntry.Text = string.Empty;
                        passwordEntry.Text = string.Empty;
                    }
                    else
                    {
                        //SI LA RESPUESTA ES NULA => NO SE OBTUVO RESULTADOS
                        //SE NOTIFICA AL USUARIO SOBRE EL INGRESO A LA PLATAFORMA
                        await DisplayAlert("Mensaje", ConexionDatos.Result, ConexionDatos.OkText);
                        passwordEntry.Text = String.Empty;
                    }
                }
            }
        }

        //=====================================================================================================================
        //=====================================================================================================================
        //FUNCION ACTIVADA CUANDO LA PAGINA APARECE
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        //=====================================================================================================================
        //=====================================================================================================================
        //FUNCION ACTIVADA CUANDO LA PAGINA DESAPARECE
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        //=====================================================================================================================
        //=====================================================================================================================
        //METODO LLAMADO CADA QUE SE GENERE UN CAMBIO EN LA CONEXION A RED
        async private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

            Console.WriteLine("\n\n==================================================");
            Console.WriteLine("==================================================");
            Console.WriteLine("\nDireccion IP del dispositivo movil: "+App.IPAddress+"\n");
            Console.WriteLine("==================================================");
            Console.WriteLine("==================================================\n\n");

            if (e.NetworkAccess == NetworkAccess.None)
            {
                await DisplayAlert("Mensaje", ConexionDatos.NetWorkConnectivityChangeMessage, "Entendido");
                return;
            }

        }
    }
}