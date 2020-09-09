using MTTO_App.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTTO_App.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaConfiguracion : ContentPage
    {
        //================================================================================
        //================================================================================
        //SE CREAN LOS OBJETOS

        Personas Persona;
        Usuarios Usuario;
        ConfiguracionAdminViewModel DatosPagina;

        public PaginaConfiguracion(Personas persona, Usuarios usuario)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS Y SE EJECUTA EL ENLAZE
            //DE LA VISTA (VIEW) Y LA CLASE (VIEWMODEL)

            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);
            BindingContext = DatosPagina = new ConfiguracionAdminViewModel(true, Persona, Usuario);

            //==============================================================================
            //==============================================================================
            //Error -> TRUE: LAS CEDULAS (ID) DE LOS OBJETOS Persona Y Usuario SON DISTINTOS
            //         FALSE: LAS CEDULAS (ID) DE LOS OBJETOS Persona Y Usuario SON IGUALES

            if (DatosPagina.Error)
                Navigation.PopAsync();

            //INDICAMOS AL "nivelusuarioPicker" QUE NIVEL DE USUARIO POSEE EL USUARIO A MODIFICAR
            switch(DatosPagina.NivelUsuario)
            {
                //----------------------------------------------
                //NIVEL BAJO (0)
                case 0:
                    nivelusuarioPicker.SelectedIndex = 0;
                    break;
                //----------------------------------------------
                //NIVEL MEDIO (5)
                case 5:
                    nivelusuarioPicker.SelectedIndex = 1;
                    break;
                //----------------------------------------------
                //NIVEL ALTO (10)
                case 10:
                    nivelusuarioPicker.SelectedIndex = 2;
                    break;
            }

        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        [Obsolete]
        async void OnInfoClicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new PaginaInformacionConfiguracion("CONFIGURACION"));
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //FUNCION QUE INHABILITA EL BackButton NATIVO DE ANDROID
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        //=========================================================================================================================================================
        //=========================================================================================================================================================
        //FUNCION ACTIVADA LUEGO DE QUE SE DEJE DE ENFOCAR EL ENTRY DEL pickerFechaNacimiento

        async protected void OnUnfocusedDate(object sender, EventArgs e)
        {
            //SE VERIFICA SI LA FECHA DE NACIMIENTO ES LA MISMA QUE EXISTE ACTUALMENTE
            if (DatosPagina.flagsameFecha)
            {
                //DE SER LA MISMA SE GENERA UN AVISO AL USUARIO PARA NOTIFICARLE
                //QUE NO SE HA REALIZADO NINGUN CAMBIO EN ESTE CAMPO A PESAR DE 
                //HABER SIDO INSPECCIONADO
                await DisplayAlert("Alerta", "La fecha es igual a la que se encontraba previamente registrada.", "Entendido");
            }

            if (DatosPagina.FechaNacimiento > DateTime.Now)
            {
                //SE GENERA UN MENSAJE EL CUAL NOTIFICA AL USUARIO
                await DisplayAlert("Alerta", "La fecha que acaba de ingresar no puede ser una que exista despues de la fecha actual", "Ok");
            }
        }

        //=========================================================================================================================================================
        //=========================================================================================================================================================
        //FUNCION ACTIVADA LUEGO DE QUE SE DEJE DE ENFOCAR EL ENTRY DEL entryTelefono

        //SE VERIFICA SI EL NUMERO TELEFONICO ES EL MISMO QUE EXISTE ACTUALMENTE
        async protected void OnUnfocusedTelefono(object sender, EventArgs e)
        {
            //DE SER EL MISMO SE GENERA UN AVISO AL USUARIO PARA NOTIFICARLE
            //QUE NO SE HA REALIZADO NINGUN CAMBIO EN ESTE CAMPO A PESAR DE 
            //HABER SIDO INSPECCIONADO
            if (DatosPagina.flagsameTelefono)
                await DisplayAlert("Alerta", "El numero de telefono es igual al que se encontraba previamente registrado.", "Entendido");
        }

        //=========================================================================================================================================================
        //=========================================================================================================================================================
        //FUNCION ACTIVADA LUEGO DE QUE SE DEJE DE ENFOCAR EL ENTRY DEL entryCorreo

        //SE VERIFICA SI LA DIRECCION DE CORREO ES LA MISMA QUE EXISTE ACTUALMENTE
        async protected void OnUnfocusedCorreo(object sender, EventArgs e)
        {
            //DE SER LA MISMA SE GENERA UN AVISO AL USUARIO PARA NOTIFICARLE
            //QUE NO SE HA REALIZADO NINGUN CAMBIO EN ESTE CAMPO A PESAR DE 
            //HABER SIDO INSPECCIONADO
            if (DatosPagina.flagsameCorreo)
                await DisplayAlert("Alerta", "El correo es igual al que se encontraba previamente registrado.", "Entendido");
        }

        //===================================================================================================================================================
        //===================================================================================================================================================

        //CUANDO SE TOQUE EL entryPassword LA CONTRASEÑA
        //SERA VISIBLE
        void FocusedPassword(object sender, EventArgs e)
        {
            //SE DESACTIVA LA VISTA COMO PASSWORD
            entryPassword.IsPassword = false;
        }

        //METODO PARA LA VERIFICACION DE LA CONTRASEÑA CUANDO ESTA NO
        //SE ENCUENTRE ENFOCADA
        async void UnFocusedPassword(object sender, EventArgs e)
        {
            //SE VUELVE A ACTIVAR LA VISTA COMO PASSWORD
            entryPassword.IsPassword = true;

            //SE DA SET AL COLOR DE TEXTO
            //entryPassword.TextColor = Color.Black;

            //SE VERIFICA SI LA CONTRASEÑA ES LA MISMA QUE EXISTE ACTUALMENTE
            if (DatosPagina.flagsamePassword)
            {
                //DE SER LA MISMA CONTRASEÑA SE GENERA UNA PREGUNTA AL USUARIO PARA
                //NOTIFICARLE SI DEASEA CONTINUAR CON LA MISMA CONTRASEÑA O DESEA CAMBIARLA
                await DisplayAlert("Alerta", "La contraseña es igual a la que se encontraba previamente registrada.", "Entendido");
            }
            else
            {
                //SI LA CONTRASEÑA ES DISTINTA, SE PROCEDE A VERIFICAR QUE LA NUEVA CONTRASEÑA
                //CUMPLA CON LOS REQUISITOS MINIMOS ESTABLECIDOS

                //VERIFICACION DE CARACTERES NO PERMITIDOS
                if (Metodos.Caracteres(DatosPagina.Password))
                {
                    entryPassword.TextColor = Color.Red;
                    await DisplayAlert("Alerta", "No se aceptan los siguientes caracteres: '!', '@', '#', '$', '%', '&', '(', ')', '+', '=', '/', '|'", "Entendido");
                }
                else
                    entryPassword.TextColor = Color.Black;

                //VERIFICACION DE ESPACIOS EN BLANCO
                if (Metodos.EspacioBlanco(DatosPagina.Password))
                {
                    entryPassword.TextColor = Color.Red;
                    await DisplayAlert("Alerta", "La contraseña no puede contener espacios en blanco", "Entendido");
                }
                else
                    entryPassword.TextColor = Color.Black;
            }
        }

        async protected void OnActualizar(object sender, EventArgs e)
        {
            //VARIABLE LOCAL
            string respuesta = string.Empty;

            //SE ACTIVA EL COMANDO SAVE CONTACT
            await DatosPagina.SaveContact();

            //EVALUACION DE LA RESPUESTA
            if (await DisplayAlert("Alerta", "Esta a punto de realizar una modificacion de datos, toda la informacion" +
                " suministrada sera modificada. ¿Desea continuar?", "Si", "No"))
            {
                //SE RECIBE EL MENSAJE DE RESPUESTA
                respuesta = DatosPagina.Save();

                if (App.ConfigChangedFlag)
                {
                    App.ConfigChangedFlag = false;
                    await Navigation.PopModalAsync();
                }
                else
                    await DisplayAlert("Mensaje", respuesta, "Entendido");
            }
        }
    }
}