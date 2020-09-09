using MTTO_App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode.Internal;

using Rg.Plugins.Popup.Services;
using MTTO_App.Paginas.Paginas_de_Informacion;


using System.IO;

namespace MTTO_App.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaRegistroTablero : ContentPage
    {
        //==========================================================================
        //==========================================================================
        //OBJETOS DE LA PAGINA:
        Personas Persona; Usuarios Usuario;

        //--------------------------------------------------------------------------
        //  DatosPagina: Sera el objeto que mantendra la comunicacion con las 
        //               entradas de datos y los metodos para organizar dichos datos
        RegistroTableroViewModel DatosPagina;
        //--------------------------------------------------------------------------

        //==========================================================================
        //==========================================================================
        //CONSTRUCTOR
        public PaginaRegistroTablero(Personas persona, Usuarios usuario)
        {
            InitializeComponent();
            //===================================================================
            //===================================================================
            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);

            //===================================================================
            //===================================================================
            //SE GENERA EL ENLACE CON LA CLASE VIEWMODEL DE LA PAGINA
            BindingContext = DatosPagina = new RegistroTableroViewModel(Persona, Usuario, true);
            CODIGO.IsVisible = ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
        }

        //==========================================================================
        //==========================================================================
        //FUNCION PARA GENERAR EL CODIGO QR
        async protected void GenerarCodigo (object sender, EventArgs e)
        {
            //SE MANDA A GENERAR EL CODIGO QR MEDIANTE
            //LA FUNCION "GenerarCodigo"
            var respuesta = DatosPagina.GenerarCodigo();

            //SI LA FUNCION DEVUELTE UNA CADENA DE TEXTO
            //NO VACIA O NULA ENTONCES SE DESPLIEGA EL MENSAJE
            //CON LA INFORMACION
            if (!string.IsNullOrEmpty(respuesta))
            {
                await DisplayAlert("Alerta", respuesta, "Entendido");
            }
            else
            {
                //------------------------------------------------------------------------------------------------
                ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = true;
                await Task.Delay(750);
                ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
                //------------------------------------------------------------------------------------------------

                //SE VUELVE VISIBLE LA SECCION (FRAME) QUE
                //CONTENDRA EL CODIGO QR
                CODIGO.IsVisible = true;

                //SE DEHABILITA EL BOTON DE GENERAR CODIGO
                BotonGenerar.IsVisible = BotonGenerar.IsEnabled = false;

                //SE HABILITA EL BOTON DE GUARDAR EL TABLERO (CODIGOQR)
                BotonImagen.IsVisible = BotonImagen.IsEnabled = true;

                //SE HABILITA EL BOTON DE REGISTRAR EL TABLERO
                BotonRegistrar.IsVisible = BotonRegistrar.IsEnabled = true;

                //==============================================================================================
                //==============================================================================================
                //SE CREA Y ADICIONA EL CODIGO QR AL STACKLAYOUT QUE LO CONTENDRA

                //SE CREA UN OBJETO IMAGEN
                var imagen = new Image();

                //SE INDICA LA FUENTE DE LA IMAGEN
                imagen.Source = ImageSource.FromStream(() => new MemoryStream(DatosPagina.CodigoQRbyte));
                
                //SE AÑADE LA IMAGEN AL "StackQR"
                StackQR.Children.Add(imagen);
                //==============================================================================================
                //==============================================================================================
            }
        }

        //==========================================================================
        //==========================================================================
        //METODO PARA ALMACENAR EL CODIGO QR EN LA GALERIA DEL TELEFONO
        protected void GuardarImagen(object sender, EventArgs e)
        {
            //------------------------------------------------------------------------------------------------
            ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = true;
            Task.Delay(750);
            ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
            //------------------------------------------------------------------------------------------------

            DatosPagina.SaveImage();
        }

        //==========================================================================
        //==========================================================================
        //METODO PARA GENERAR UN NUEVO REGISTRO DE TABLERO
        private async void RegistrarTablero(object sender, EventArgs e)
        {
            //VARIABLE QUE RECIBIRA LA RESPUESTA AL METODO DE GUARDADO
            string respuesta = string.Empty;

            //SE REALIZA LA PREGUNTA AL USUARIO SOBRE SI DERESEA REGISTRAR EL TABLERO EN LA BASE DE DATOS
            //SE EVALUA LA RESPUESTA OBTENIDA DIRECTAMENTE EN EL CONDICIONAL
            if(await DisplayAlert("Alerta", "Esta apunto de realizar un nuevo registro de tablero." +
                "\n¿Desea continuat?", "Si", "No, volver"))
            {
                //------------------------------------------------------------------------------------------------
                ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = true;
                await Task.Delay(750);
                ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
                //------------------------------------------------------------------------------------------------

                //SE EJECUTA EL METODO REGISTRAR TABLERO, ESTA FUNCION RETORNARA UNA VARIABLE TEXTO DEL TIPO STRING
                respuesta = DatosPagina.RegistrarTablero();

                //SE EVALUA SI LA VARIABLE RETORNADA SE ENCUENTRA VACIA O NULA
                //VACIA O NULA => SE REGISTRO SATISFACTORIAMENTE
                //NO VACIA O NULA => NO SE PUDO REALIZAR EL REGISTRO
                if(!string.IsNullOrEmpty(respuesta))
                {
                    //SE MUESTRA POR MENSAJE DE CONSOLA Y DE ALERTA LA RESPUESTA OBTENIDA POR EL METODO REGISTRO TABLERO
                    Mensaje(respuesta);
                    await DisplayAlert("Mensaje", respuesta, "Entendido");

                    //SE ESCONDE LA SECCION (FRAME) QUE CONTENDRA EL CODIGO QR
                    CODIGO.IsVisible = false;

                    //SE HABILITA EL BOTON DE GENERAR CODIGO
                    BotonGenerar.IsVisible = BotonGenerar.IsEnabled = true;

                    //SE DESHABILITA EL BOTON DE GUARDAR EL TABLERO (CODIGOQR)
                    BotonImagen.IsVisible = BotonImagen.IsEnabled = false;

                    //SE DESHABILITA EL BOTON DE REGISTRAR EL TABLERO
                    BotonRegistrar.IsVisible = BotonRegistrar.IsEnabled = false;

                    //SE REMUEVE TODO ELEMENTO QUE SE ENCUENTRE DENTRO DEL STACKLAYOUT DEL FRAME "CODIGO"
                    StackQR.Children.Clear();
                }
            }

        }

        //===========================================================================
        //===========================================================================
        //FUNCION QUE SE ACTIVA CUANDO SE DEJE DE ENFOCAR LA ENTRADA "entryTableroID"
        private void OnUnfocusedTableroID(object sender, FocusEventArgs e)
        {
            //SE EVALUA QUE LA PROPIEDAD "TableroID" NO SE ENCUENTRE VACIA
            if(!string.IsNullOrEmpty(DatosPagina.TableroID))
            {
                //SE EVALUA SI SE CUMPLEN LAS CONDICIONES MINIMAS
                if (Metodos.EspacioBlanco(DatosPagina.TableroID))
                {
                    //SE MANDA A NOTIFICAR AL USUARIO
                    Mensaje("El ID del tablero no puede contener espacios en blanco");
                }
                else
                {
                    //SE EVALUA QUE LA PROPIEDAD "TableroID" NO POSEEA LOS CARACTERES NO PERMITIDOS
                    if (Metodos.Caracteres(DatosPagina.TableroID))
                    {
                        Mensaje("El ID del tablero no puede contener los siguientes caracteres:\n " +
                            "'!', '@', '#', '$', '%', '&', '(', ')', '+', '=', '/', '|'");
                    }
                }
            }
        }

        //===========================================================================
        //===========================================================================
        //FUNCION QUE SE ACTIVA CUANDO SE DEJE DE ENFOCAR LA ENTRADA "entryFilial"
        private void OnUnfocusedFilial(object sender, FocusEventArgs e)
        {
            if(!string.IsNullOrEmpty(DatosPagina.Filial))
            {
                //SE EVALUA SI EL TEXTO INGRESADO CUMPLE CON LAS CONDICIONES MINIMAS
                if (Metodos.Caracteres(DatosPagina.Filial))
                { 
                    Mensaje("El nombre de la filial no puede contener los siguientes caracteres:\n " +
                            "'!', '@', '#', '$', '%', '&', '(', ')', '+', '=', '/', '|'");
                }
                else
                {
                    DatosPagina.Filial = Metodos.Mayuscula(DatosPagina.Filial);
                }
            }
        }

        //===========================================================================
        //===========================================================================
        //FUNCION QUE SE ACTIVA CUANDO SE DEJE DE ENFOCAR LA ENTRADA "pickerFilial"
        //private void OnUnfocusedFilial(object sender, FocusEventArgs e){}

        //===========================================================================
        //===========================================================================
        //FUNCION QUE SE ACTIVA CUANDO SE DEJE DE ENFOCAR LA ENTRADA "entryArea"
        private void OnUnfocusedArea(object sender, FocusEventArgs e)
        {
            //SE EVALUA QUE LA PROPIEDAD "Area" NO SE ENCUENTRE NULA O VACIA
            if(!string.IsNullOrEmpty(DatosPagina.Area))
            {
                //SE EVALUA SI EL TEXTO INGRESADO CUMPLE CON LAS CONDICIONES MINIMAS
                if (Metodos.Caracteres(DatosPagina.Area))
                {
                    Mensaje("El nombre del area no puede contener los siguientes caracteres:\n " +
                            "'!', '@', '#', '$', '%', '&', '(', ')', '+', '=', '/', '|'");
                }
                else
                {
                    DatosPagina.Area = Metodos.Mayuscula(DatosPagina.Area);
                }
            }
        }

        //==========================================================================
        //==========================================================================
        //FUNCION QUE SE ACTIVA CUANDO SE NECESITA MOSTRAR UN MENSAJE
        //AL USUARIO QUE SE ENCUENTRE LOGGEADO
        private async void Mensaje(string Mensaje)
        {
            await DisplayAlert("Alerta", Mensaje, "Entendido");

            Console.WriteLine("\n\n=============================================");
            Console.WriteLine("=============================================");
            Console.WriteLine("\n" + Mensaje.ToUpper());
            Console.WriteLine("=============================================");
            Console.WriteLine("=============================================\n\n");
        }

        //==========================================================================
        //==========================================================================
        //FUNCION PARA LLAMAR A LA PAGINA DE INFORMACION

        [Obsolete]
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new PaginaInformacionRegistroTablero());
        }
    }
}