using Android.Widget;
using MTTO_App.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTTO_App.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaRegistro : ContentPage
    {
        //OBJETOS
        //SE CREAN LAS VARIABLES GLOBALES DE LA CLASE
        private ConfiguracionAdminViewModel ConexionDatos;

        private Personas Persona; private Usuarios Usuario;

        public PaginaRegistro(Personas persona, Usuarios usuario)
        {
            InitializeComponent();

            //SE CARGA LA INFORMACION DEL USUARIO QUE SE ENCUENTRA LOGGEADO ACTUALMENTE
            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);

            //SE GENERA LA CONEXION DE LA CLASE "PaginaRegistro2.xaml.cs" CON LA CLASE
            BindingContext = ConexionDatos = new ConfiguracionAdminViewModel(false, Persona, Usuario, Usuario.Cedula);

            ActivityIndicatorA.IsRunning = ActivityIndicatorA.IsVisible = false;
            DatosUsuarioGrid.IsVisible = false;
            nombresEntry.IsEnabled = apellidosEntry.IsEnabled = true;
            Boton.Text = "CONTINUAR";
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //METODO PARA VERIFICAR SI correoEntry TIENE ESPACIOS EN BLANCO
        private async void OnCompletedCorreo(object sender, EventArgs e)
        {
            //BUSQUEDA DE ESPACIOS EN BLANCO
            //BANDERA QUE VERIFICA SI LA CADENA DE CARACTERES ALMACENA UN ESPACIO EN BLANCO
            if (Metodos.EspacioBlanco(ConexionDatos.Correo.ToLower()))
            {
                correoEntry.TextColor = Color.Red;
                await DisplayAlert("Alerta", "El correo no puede contener espacios en blanco", "Entendido");
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //METODO PARA VERIFICAR QUE usernameEntry NO TENGA ESPACIOS EN BLANCO Y QUE NO TENGA LOS CARACTERES NO PERMITIDOS
        private async void OnCompletedUserName(object sender, EventArgs e)
        {
            //BUSQUEDA DE ESPACIOS EN BLANCO

            if (Metodos.EspacioBlanco(ConexionDatos.Username.ToLower()))
            {
                usernameEntry.TextColor = Color.Red;
                await DisplayAlert("Alerta", "El nombre de usuario no puede contener espacios en blanco", "Entendido");
            }

            //BUSQUEDA DE CARACTERES

            if (Metodos.Caracteres(ConexionDatos.Username.ToLower()))
            {
                usernameEntry.TextColor = Color.Red;
                await DisplayAlert("Alerta", "No se aceptan los siguientes caracteres: '!', '@', '#', '$', '%', '&', '(', ')', '+', '=', '/', '|'", "Entendido");
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //METODO PARA VERIFICAR QUE passwordEntry1 NO TENGA ESPACIOS EN BLANCO QUE NO SEA MENOR A 6 CARACTERES Y QUE NO POSEA LOS
        //CARACTERES NO PERMITIDOS
        private async void OnCompletedPassword1(object sender, EventArgs e)
        {
            //BUSQUEDA DE ESPACIOS EN BLANCO

            if (Metodos.EspacioBlanco(ConexionDatos.Password.ToLower()))
            {
                passwordEntry1.TextColor = Color.Red;
                await DisplayAlert("Alerta", "La contraseña no puede contener espacios en blanco", "Entendido");
            }
            else
            {
                //VERIFICACION DE CANTIDAD DE CARACTERES

                if (ConexionDatos.Password.Length < 6)
                {
                    await DisplayAlert("Alerta", "La contraseña no puede tener menos de 6 caracteres", "Entendido");
                }
            }

            //VERIFICACION DE CARACTERES NO PERMITIDOS

            if (Metodos.Caracteres(ConexionDatos.Password.ToLower()))
            {
                passwordEntry1.TextColor = Color.Red;
                await DisplayAlert("Alerta", "No se aceptan los siguientes caracteres: '!', '@', '#', '$', '%', '&', '(', ')', '+', '=', '/', '|'", "Entendido");
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //METODO PARA VERIFICAR QUE passwordEntry2 NO TENGA ESPACIOS EN BLANCO, QUE NO SEA MENOR A 6 CARACTERES Y QUE NO POSEA LOS
        //CARACTERES NO PERMITIDOS
        private async void OnCompletedPassword2(object sender, EventArgs e)
        {
            //BUSQUEDA DE ESPACIOS EN BLANCO

            //SI LAS CONTRASEÑAS SON DISTINTAS SE DISPARA LA BANDERA "flagdifferentPassword"
            if (ConexionDatos.flagdifferentPassword)
                await DisplayAlert("Alerta", "Las contraseñas ingresadas no coinciden. \n\nVerefique e intente nuevamente",
                    "Entendido");

            //SI LA CONTRASEÑA TIENE ESPACIOS EN BLANCO SE DESPLIEGA UN MENSAJE DE NOTIFICACION
            if (Metodos.EspacioBlanco(ConexionDatos.ConfirmacionPassword.ToLower()))
            {
                passwordEntry2.TextColor = Color.Red;
                await DisplayAlert("Alerta", "La contraseña no puede contener espacios en blanco", "Entendido");
            }
            else
            {
                //VERIFICACION DE CANTIDAD DE CARACTERES

                if (ConexionDatos.ConfirmacionPassword.Length < 6)
                {
                    await DisplayAlert("Alerta", "La contraseña no puede tener menos de 6 caracteres", "Entendido");
                }
            }

            //VERIFICACION DE CARACTERES NO PERMITIDOS

            if (Metodos.EspacioBlanco(ConexionDatos.ConfirmacionPassword.ToLower()))
            {
                passwordEntry2.TextColor = Color.Red;
                await DisplayAlert("Alerta", "No se aceptan los siguientes caracteres: '!', '@', '#', '$', '%', '&', '(', ')', '+', '=', '/', '|'", "Entendido");
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //METODO PARA REGRESAR A COLOR NEGRO CUANDO SE CORRIJAN LOS TEXTOS DE LOS ENTRY ASIGNADOS A: CORREO, USERNAME y PASSWORD(1 y 2)
        private void CorreccionCorreo(object sender, TextChangedEventArgs e)
        {
            //LA ACCION DE ESTE LLAMADO SOLO SERA PERCEPTIBLE CUANDO LA CADENA DE CARACTERES
            //QUE ARROJA correoEntry TENGRA UN ESPACIO EN BLANCO EN ELLA (' ' o " ")
            correoEntry.TextColor = Color.Black;
        }

        private void CorreccionPassword1(object sender, TextChangedEventArgs e)
        {
            //==================================================================================================================================
            //==================================================================================================================================
            //CONDICIONALES PARA REALIZAR EL CAMBIO DE COLOR DEL TEXTO ASIGNADO A LA ENTRADA "passwordEntry1"

            if (ConexionDatos.Password.Length >= ConexionDatos.PasswordYellowColor && ConexionDatos.Password.Length <= ConexionDatos.PasswordGreenColor)
                passwordEntry1.TextColor = Color.Yellow;

            if (ConexionDatos.Password.Length > ConexionDatos.PasswordGreenColor)
                passwordEntry1.TextColor = Color.Green;

            if (ConexionDatos.Password.Length <= ConexionDatos.PasswordYellowColor)
                passwordEntry1.TextColor = Color.Black;
        }

        private async void CorreccionPassword2(object sender, TextChangedEventArgs e)
        {
            //==================================================================================================================================
            //==================================================================================================================================
            //CONDICIONALES PARA REALIZAR EL CAMBIO DE COLOR DEL TEXTO ASIGNADO A LA ENTRADA "passwordEntry2"

            if (ConexionDatos.ConfirmacionPassword.Length >= ConexionDatos.PasswordYellowColor && ConexionDatos.ConfirmacionPassword.Length <= ConexionDatos.PasswordGreenColor)
                passwordEntry2.TextColor = Color.Yellow;

            if (ConexionDatos.ConfirmacionPassword.Length > ConexionDatos.PasswordGreenColor)
                passwordEntry2.TextColor = Color.Green;

            if (ConexionDatos.ConfirmacionPassword.Length <= ConexionDatos.PasswordYellowColor)
                passwordEntry2.TextColor = Color.Black;

            //==================================================================================================================================
            //==================================================================================================================================
            //SI LAS CONTRASEÑAS NO COINCIDEN SE DISPARA LA ALARMA "flagdifferentPassword"
            if (ConexionDatos.flagdifferentPassword)
                await DisplayAlert("Alerta", "Las contraseñas no coinciden", "Entendido");
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //VERIFICACION FECHA DE NACIMIENTO: METODO ACTIVADO CUANDO SE SELECCIONA UNA FECHA DEL "fechaNacimientoPicker"
        private async void OnDateSelected(object sender, DateChangedEventArgs args)
        {
            //SI LA FECHA DE NACIMIENTO SELECCIONADA ES IGUAL A LA FECHA ACTUAL
            if (ConexionDatos.FechaNacimiento == DateTime.Today)
            {
                fechaNacimientoPicker.TextColor = Color.Red;
                await DisplayAlert("Alerta", "No se permite seleccionar la fecha actual como fecha de nacimiento", "Entendido");
            }
            else
                fechaNacimientoPicker.TextColor = Color.Black;

            //SI LA FECHA DE NACIMIENTO SELECCIONADA ES MAYOR A LA FECHA ACTUAL
            if (ConexionDatos.FechaNacimiento > DateTime.Now)
            {
                fechaNacimientoPicker.TextColor = Color.Red;
                await DisplayAlert("Alerta", "No se permite seleccionar una fecha que no a existido todavia", "Entendido");
            }
            else
                fechaNacimientoPicker.TextColor = Color.Black;
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //METODO DE REGISTRO: METODO LLAMADO CUANDO SE PRECIONA EL BOTON DE LA PAGINA DE REGISTRO
        public async void OnButtonPush(object sender, EventArgs e)
        {
            //CREACION E INICIALIZACION DE LA VARIABLE QUE RECIBIRA LA RESPUESTA DE LOS LLAMADOS DE LOS METODOS DE LA CLASE "ConfiguracionAdminViewModel.cs"
            string respuesta = string.Empty;

            //------------------------------------------------------------------------------
            ActivityIndicatorA.IsRunning = ActivityIndicatorA.IsVisible = true;
            await Task.Delay(1250);
            ActivityIndicatorA.IsRunning = ActivityIndicatorA.IsVisible = false;
            //------------------------------------------------------------------------------

            //SE EVALUARA EL TEXTO QUE CONTIENE EL BOTON DE LA PAGINA
            switch (Boton.Text)
            {
                //TEXTO CONTINUAR: EN ESTA SECCION EL RECUADRO QUE CONTIENE LAS ENTRADAS PARA LA INFORMACION DE USUARIO SE ENCUENTRA
                //ESCONDIDA (NO ES VISIBLE). PRIMERO SE DEBEN LLENAR TODOS LOS CAMPOS DE LA INFORMACION PERSONAL
                case "CONTINUAR":
                    //EVALUAMOS SI LOS CAMPOS DE INFORMACION PERSONAL SE ENCUENTRAN NO VACIO O NO NULOS
                    if (ConexionDatos.ContinuarRegistro(true))
                    {
                        //TODOS LOS CAMPOS SE ENCUENTRAN LLENOS
                        //------------------------------------------------------------------------------
                        //SE CAMBIA EL TEXTO DEL BOTON
                        Boton.Text = "REGISTRAR";
                        //SE MUESTRA EL NOMBRE DE USUARIO GENERADO A PARTIR DE LA COMBINACION DE NOMBRE(S) Y APELLIDO(S)
                        usernameEntry.Text = ConexionDatos.Username;
                        //SE DESHABILITAN LAS ENTRADAS ASIGNADAS A LOS CAMPOS NOMBRE(S) Y APELLIDO(S)
                        nombresEntry.IsEnabled = apellidosEntry.IsEnabled = false;
                        //SE VUELVE VISIBLE LA SECCION QUE CONTIENE LOS CAMPOS DE INFORMACION PERSONAL
                        DatosUsuarioGrid.IsVisible = true;
                        //------------------------------------------------------------------------------
                    }
                    else
                    {
                        //ALGUNO DE LOS CAMPOS NO SE ENCUENTRAN LLENOS
                        //------------------------------------------------------------------------------
                        //SE NOTIFICA AL USUARIO POR MEDIO DE UN MENSAJE POP UP QUE DEBE LLENAR TODOS LOS CAMPOS
                        await DisplayAlert("Mensaje", "Todos los campos referentes a la informacion personal deben llenarse.", "Entendido");
                        //SE VUELVE INVISIBLE LA SECCION QUE CONTIENE LOS CAMPOS DE INFORMACION PERSONAL
                        DatosUsuarioGrid.IsVisible = false;
                        //------------------------------------------------------------------------------
                    }
                    break;
                //TEXTO REGISTRAR: EN ESTA SECCION SE ENCUENTRAN VISIBLES LAS DOS SECCIONES DE LA PAGINA REGISTRAR: "Informacion Personal"
                //e "Informacion de Usuario".
                case "REGISTRAR":
                    //SE REALIZA UNA PREGUNTA Y SE EVALUA LA RESPUESTA
                    if (await DisplayAlert("Alerta", "Esta a punto de realizar un nuevo registro." +
                        "\n\n¿Desea continuar?", "Si", "No"))
                    {
                        //------------------------------------------------------------------------------------------------------
                        //----------------------CODIGO PARA REGISTRAR UN USUARIO MEDIANTE CONSUMO DE API------------------------
                        //LLAMAMOS AL METODO "Save" DE LA CLASE "ConfiguracionAdminViewModel" Y GUARDAMOS LA RESPUESTA OBTENIDA
                        //INICIAMOS EL ACTIVITY INDICATOR
                        ActivityIndicatorA.IsRunning = true;

                        await Task.Run(async () =>
                        {
                            respuesta = await ConexionDatos.Save();
                            ActivityIndicatorA.IsRunning = false;
                        });

                        //SE MUESTRA EL MENSAJE OBTENIDO
                        Toast.MakeText(Android.App.Application.Context, respuesta, ToastLength.Short).Show();

                        //------------------------------------------------------------------------------------------------------
                        //------------------------------------------------------------------------------------------------------
                        //-----CODIGO PARA REGISTRAR UN USUARIO CUANDO LA APLICACION SE ENCUENTRA TRABAJANDO STAND ALONE--------
                        /*respuesta = await ConexionDatos.Save();
                        //SE EVALUA SI SE REALIZO ALGUN REGISTRO
                        if (App.RegistroFlag)
                            //SE DA SET A LA BANDERA
                            App.RegistroFlag = false;
                        else
                            //SE MUESTRA EN PANATALLA MEDIANTE UN MENSAJE POP UP EL PORQUE NO SE PUDO REALIZAR EL REGISTRO DE USUARIO
                            await DisplayAlert("Mensaje", respuesta, "Entendido");*/
                        //------------------------------------------------------------------------------------------------------
                        //------------------------------------------------------------------------------------------------------
                    }
                    break;
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //METODO QUE RETORNA A LA CLASE "ConfiguracionAdminViewModel.cs" EL NUMERO DEL ITEM SELECCIONADO EN EL PICKER "nivelusuarioPicker"
        private void OnSelectedNivelUsuarios(object sender, EventArgs e)
        {
            //---------------------------------------NOTA---------------------------------------
            /*PARA CONSULTAR O MODIFICAR LOS NIVELES DE USUARIOS DIRIJASE A LA SUB CLASE "Usuarios"
             DENTRO DE LA CLASE "Tablas.cs". En ella se encontrara con una funcion llamada
             "NivelUsuarioLista" la cual retornara un lista del tipo string*/
            //----------------------------------------------------------------------------------
            //CREAMOS UN OBJETO DEL TIPO "Picker" Y LO ENLAZAMOS "nivelusuarioPicker" (ubicado
            //EN "PaginaRegistro.xaml")
            Picker picker = sender as Picker;
            //EVALUAMOS EL VALOR DE LA POSICION DE LA OPCION SELECCIONADA
            switch (picker.SelectedIndex)
            {
                //NIVEL BAJO (0)
                case 0:
                    ConexionDatos.nivelusuario = 0;
                    break;
                //NIVEL MEDIO (5)
                case 1:
                    ConexionDatos.nivelusuario = 5;
                    break;
                //NIVEL ALTO (10)
                case 2:
                    ConexionDatos.nivelusuario = 10;
                    break;
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //LLAMADO A LA PAGINA DE INFORMACION
        [Obsolete]
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PaginaInformacionRegistro());
        }
    }
}