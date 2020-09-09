using Android.Widget;
using MTTO_App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using Rg.Plugins.Popup.Services;
using MTTO_App.Paginas.Paginas_de_Informacion;
using System.Threading.Tasks;

namespace MTTO_App.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaConsultaTablero : ContentPage
    {
        //=================================================================================================
        //=================================================================================================
        //DECLARACION DE OBJETOS
        RegistroTableroViewModel DatosPagina;
        Personas Persona; Usuarios Usuario;

        //=================================================================================================
        //=================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaConsultaTablero(Personas persona, Usuarios usuario)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS CON LA INFORMACION DEL USUARIO QUE SE ENCUENTRA LOGGEADO
            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);

            //SE GENERA LA CONEXION CON LA CLASE VIEWMODEL
            BindingContext = DatosPagina = new RegistroTableroViewModel(Persona, Usuario, false);

            //SE DA SET (FALSE) AL FRAME QUE CONTENDRA LA INFORMACION DEL CODIGO QR 
            FrameResultado.IsVisible = DatosPagina.ShowResultadoScan;

            //SE DA SET (FALSE) AL ActivityIndicator QUE INDICARA AL USUARIO CUANDO SE ESTA CUMPLIENDO ALGUN PROCESO
            ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;

        }

        //=================================================================================================
        //=================================================================================================
        //METODO PARA ESCANEAR (CAMARA)
        async private void Escanear(object sender, EventArgs e)
        {
            //===============================================================
            //===============================================================
            //SE NOTIFICA DESDE DONDE SE HACE LA CONSULTA DE TABLERO
            DatosPagina.TipoDeConsulta = "CONSULTA_ESCANER";

            //===============================================================
            //===============================================================
            //SE LLENAN EL OBJETO Opciones CON LAS OPCIONES QUE 
            //SE VERAN DISPONIBLES EN LA PANTALLA DE SCANEO
            MobileBarcodeScanningOptions Opciones = new MobileBarcodeScanningOptions()
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
            };

            //===============================================================
            //===============================================================
            //SE CREA LA PAGINA SCAN
            ZXingScannerPage PaginaScan = new ZXingScannerPage(Opciones)
            {
                DefaultOverlayShowFlashButton = true,
                DefaultOverlayBottomText = "Escanea el codigo QR",
            };

            //===============================================================
            //===============================================================
            //SE GENERA UN LLAMADO DE NAVEGACION A LA PAGINA SCAN
            await App.MasterDetail.Navigation.PushModalAsync(PaginaScan);

            PaginaScan.OnScanResult += (result) =>
            {
                //SI SE TIENE UN RESULTADO SE DETIENE EL SCANEO
                PaginaScan.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    //SE CIERRA LA PAGINA DE SCANEO
                    await App.MasterDetail.Navigation.PopModalAsync();

                    //SE GUARDA EL RESULTADO EN LA PROPIEDAD "ResultadoScan" DEL OBJETO "DatosPagina"
                    DatosPagina.ResultadoScan = result.Text;

                    //=============================================================================
                    //=============================================================================
                    //SE EVALUA LA PROPIEDAD "ShowResultadoScan", DE SER TRUE
                    //SE PROCEDE A EXTRAER MANUALMENTE TODA LA INFORMACION DEL CODIGO QR
                    //OBTENIDO MEDIANTE LAS PROPIEDADES DE LA CLASE "RegistroTableroViewModel.cs"
                    if(DatosPagina.ShowResultadoScan)
                    {
                        //------------------------------------------------------------------------------------------------
                        ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = true;
                        await Task.Delay(750);
                        ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
                        //------------------------------------------------------------------------------------------------

                        //SE CAMBIA LA VISIBILIDAD DEL "FrameResultado" CON LOS RESULTADOS
                        FrameResultado.IsVisible = DatosPagina.ShowResultadoScan;

                        //------------------------------------------------------------------------------------------------
                        //SE LLENAN (MANUALMENTE) CADA UNO DE LOS CAMPOS DE INFORMACION
                        idtablero.Text = DatosPagina.TableroID;
                        filialtablero.Text = DatosPagina.Filial;
                        areatablero.Text = DatosPagina.Area;
                        ultimaconsultatablero.Text = DatosPagina.UltimaFechaConsulta.ToString();
                        codigoqrtablero.Source = ImageSource.FromStream(() => new MemoryStream(DatosPagina.CodigoQRbyte));
                        //------------------------------------------------------------------------------------------------                   
                    }
                    else
                    {
                        //SE CAMBIA SI ES O NO VISIBLE EL FRAME CON LOS RESULTADOS
                        FrameResultado.IsVisible = DatosPagina.ShowResultadoScan;

                        //SE INFORMA AL USUARIO QUE EL TABLERO QUE ACABA DE SER ESCANEADO NO FUE LOCALIZADO
                        Toast.MakeText(Android.App.Application.Context, "No se encontro la informacion del tablero introducido...", ToastLength.Long).Show();
                    }

                    //=============================================================================
                    //=============================================================================

                });
            };
        }

        //=================================================================================================
        //=================================================================================================
        //METODO PARA BUSCAR POR ID DEL TABLERO
        async private void ConsultaID (object sender, EventArgs e)
        {
            //===============================================================
            //===============================================================
            //SE NOTIFICA DESDE DONDE SE HACE LA CONSULTA DE TABLERO
            DatosPagina.TipoDeConsulta = "CONSULTA_POR_ID";

            //===============================================================
            //===============================================================
            //SE EVALUA SI EL VALOR DEL ENTRY "entryTableroID" TIENE ALGUN VALOR
            if(!string.IsNullOrEmpty(entryTableroID.Text))
            {
                //ENVIAMOS EL VALOR INGRESADO POR EL USUARIO
                DatosPagina.ResultadoScan = entryTableroID.Text;

                //EVALUAMOS SI EL TABLERO SE ENCUENTRA EN LA BASE DE DATOS
                if (DatosPagina.ShowResultadoScan)
                {
                    //------------------------------------------------------------------------------------------------
                    ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = true;
                    await Task.Delay(750);
                    ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
                    //------------------------------------------------------------------------------------------------

                    //SE CAMBIA LA VISIBILIDAD DEL "FrameResultado" CON LOS RESULTADOS
                    FrameResultado.IsVisible = DatosPagina.ShowResultadoScan;

                    //------------------------------------------------------------------------------------------------
                    //SE LLENAN (MANUALMENTE) CADA UNO DE LOS CAMPOS DE INFORMACION
                    idtablero.Text = DatosPagina.TableroID;
                    filialtablero.Text = DatosPagina.Filial;
                    areatablero.Text = DatosPagina.Area;
                    ultimaconsultatablero.Text = DatosPagina.UltimaFechaConsulta.ToString();
                    codigoqrtablero.Source = ImageSource.FromStream(() => new MemoryStream(DatosPagina.CodigoQRbyte));
                    //------------------------------------------------------------------------------------------------                   
                }
                else
                {
                    //SE CAMBIA SI ES O NO VISIBLE EL FRAME CON LOS RESULTADOS
                    FrameResultado.IsVisible = DatosPagina.ShowResultadoScan;

                    //SE INFORMA AL USUARIO QUE EL TABLERO QUE ACABA DE SER ESCANEADO NO FUE LOCALIZADO
                    Toast.MakeText(Android.App.Application.Context, "No se encontro la informacion del tablero...", ToastLength.Long).Show();
                }
            }
            //SI EL VALOR DEL ENTRY "entryTableroID" ESTA VACIO O NULO SE NOTIFICA AL USUARIO
            else
            {
                await DisplayAlert("Error", "Para realizar una consulta por ID ingrese el ID del tablero que desea buscar.", "Entendido");
            }
        }

        //=================================================================================================
        //=================================================================================================
        //METODO PARA GUARDAR EL CODIGO QR DENTRO DE LA GELERIA DEL TELEFONO
        private void GuardarCodigoQR (object sender, EventArgs e)
        {
            DatosPagina.SaveImage();
        }

        //==========================================================================
        //==========================================================================
        //FUNCION PARA LLAMAR A LA PAGINA DE INFORMACION

        [Obsolete]
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new PaginaInformacionConsultaTablero());
        }
    }
}