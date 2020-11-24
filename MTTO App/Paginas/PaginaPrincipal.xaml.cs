using MTTO_App.ViewModel;
using SQLite;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MTTO_App.Tablas;

namespace MTTO_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPrincipal : ContentPage
    {
        //CREACION E INICIALIZACION DE LOS OBJETOS
        private PaginaPrincipalViewModel ConexionDatos;

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
            bool usernameFlag = false;

            if (string.IsNullOrEmpty(ConexionDatos.Username) ||     //LOS ENTRY DEBEN TENER CARACTERES PARA PODER
                string.IsNullOrEmpty(ConexionDatos.Password))       //REALIZAR LA BUSQUEDA DE USUARIO
            {
                await DisplayAlert("Mensaje", "Debe ingresar el nombre de usuario y la contraseña para poder ingresar", "Entendido");
            }
            else
            {
                //------------------------------------------------------------------------------
                ActivityIndicator.IsRunning = ActivityIndicator.IsVisible = true;
                await Task.Delay(1250);
                ActivityIndicator.IsRunning = ActivityIndicator.IsVisible = false;
                //------------------------------------------------------------------------------
                //CONEXION CON LA BASE DE DATOS LOCAL (DB DEL MOVIL)
                using (SQLiteConnection connection = new SQLiteConnection(App.FileName))
                {
                    //SE CREA LA CONEXION
                    connection.CreateTable<Usuarios>();

                    //RECORREMOS LA TABLA "Usuarios"
                    foreach (Usuarios usuario in connection.Table<Usuarios>().ToList())
                    {
                        //SE PERMITIO QUE LA VALIDACION PARA EL NOMBRE DE USUARIO NO DISCRIMINARA
                        //ENTRE CARACTERES QUE ESTEN EN MAYUSCULA O EN MINUSCULA
                        if (usernameEntry.Text.ToLower() == usuario.Username.ToLower())
                        {
                            //SI EL USUARIO SE ENCUENTRA SE ACTIVA LA ALARMA
                            usernameFlag = true;

                            //LUEGO SE VERIFICA QUE LA CONTRASEÑA DEL USUARIO SEA LA MISMA
                            //QUE LA PROPORCIONADA POR EL passwordEntry
                            if (passwordEntry.Text == usuario.Password)
                            {
                                //EL USUARIO Y LA CONTRASEÑA SON LOS MISMOS. SE PROCEDE
                                //A BUSCAR EL OBJETO USUARIO QUE COMPARTA EL MISMO ID (CEDULA)
                                foreach (Personas persona in connection.Table<Personas>().ToList())
                                {
                                    //SI LOS ID SON IGUALES SE PROCEDE A HACER EL SALTO DE PAGINA
                                    if (persona.Cedula == usuario.Cedula)
                                    {
                                        Console.WriteLine("\n========================================");
                                        Console.WriteLine("========================================");
                                        Console.WriteLine("\nSE INGRESO");
                                        Console.WriteLine("========================================");
                                        Console.WriteLine("========================================\n");

                                        usernameEntry.Text = string.Empty;
                                        passwordEntry.Text = string.Empty;

                                        await Navigation.PushModalAsync(new UserMainPage(persona, usuario));

                                        break;
                                    }
                                }

                                break;
                            }
                            else
                            {
                                //LAS CONTRASEÑAS NO SON IDENTICAS
                                Console.WriteLine("\n========================================");
                                Console.WriteLine("========================================");
                                Console.WriteLine("CLAVE ERRONEA");
                                Console.WriteLine("========================================");
                                Console.WriteLine("========================================\n");

                                //SE PROPORCIONA UN MENSAJE DE AVISO Y SE LIMPIA EL passwordEntry
                                await DisplayAlert("Mensaje", "Contraseña incorrecta", "Ok");
                                passwordEntry.Text = String.Empty;
                            }
                        }
                    }

                    //SI LA BANDERA MATCH DE USUARIO NO SE DISPARO ESTO IMPLICA QUE
                    //EL NOMBRE DE USUARIO PROPORCIONADO POR EL usernameEntry NO SE
                    //ENCUENTRA REGISTRADO
                    if (usernameFlag == false)
                    {
                        await DisplayAlert("Mensaje", "Nombre de usuario no encontrado", "Ok");
                        passwordEntry.Text = String.Empty;
                    }
                }
                //------------------------------------------------------------------------------
                //CONEXION CON LA BASE DE DATOS (CLIENTE - SERVIDOR)
                /*
                //SE LLAMA AL METODO QUE REALIZA LA SOLICITUD HTTP
                await ConexionDatos.LogInResponse(ConexionDatos.Username, ConexionDatos.Password);

                //SE EVALUA EL ESTADO DE LA RESPUESTA
                if(ConexionDatos.LogIn != null)
                {
                    //SI LA RESPUESTA ES DIFERENTE DE NULL => SE OBTUVO UN RESULTADO
                    //SE LIMPIAN LAS ENTRADAS DE TEXTO PARA EL USERNAME Y PARA EL PASSWORD
                    usernameEntry.Text = string.Empty;
                    passwordEntry.Text = string.Empty;

                    //SE REALIZA EL LLAMADO A LA PAGINA "UserMainPage"
                    await Navigation.PushModalAsync(new UserMainPage(ConexionDatos.LogIn.UserInfo.Persona, ConexionDatos.LogIn.UserInfo.Usuario));
                }
                else
                {
                    //SI LA RESPUESTA ES NULA => NO SE OBTUVO RESULTADOS
                    //SE NOTIFICA AL USUARIO SOBRE EL INGRESO A LA PLATAFORMA
                    await DisplayAlert("Mensaje", ConexionDatos.Result, "Ok");
                    passwordEntry.Text = String.Empty;
                }
                */
                //------------------------------------------------------------------------------
            }
        }
    }
}