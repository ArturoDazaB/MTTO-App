using MTTO_App.Tablas;
using MTTO_App.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            //bool usernameFlag = false;

            if (string.IsNullOrEmpty(ConexionDatos.Username) ||     //LOS ENTRY DEBEN TENER CARACTERES PARA PODER
                string.IsNullOrEmpty(ConexionDatos.Password))       //REALIZAR LA BUSQUEDA DE USUARIO
            {
                await DisplayAlert("Mensaje", "Debe ingresar el nombre de usuario y la contraseña para poder ingresar", "Entendido");
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
                    await DisplayAlert("Mensaje", ConexionDatos.Result, "Entendido");
                    passwordEntry.Text = String.Empty;
                }
            }
        }
    }
}