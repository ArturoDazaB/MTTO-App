﻿using Android.Widget;
using MTTO_App.Paginas.Paginas_de_Informacion;
using MTTO_App.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace MTTO_App.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaConsultaTablero : ContentPage
    {
        //=================================================================================================
        //=================================================================================================
        //SE DECLARACION DE OBJETOS
        private RegistroTableroViewModel DatosPagina;
        private Personas Persona;
        private Usuarios Usuario;

        //SE DECLARAN LAS CONSTANTES
        private const int HeightRow = 45;

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
            FrameResultado.IsVisible = false;

            //SE DA SET (FALSE) AL ActivityIndicator QUE INDICARA AL USUARIO CUANDO SE ESTA CUMPLIENDO ALGUN PROCESO
            ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
            listViewItems.ItemsSource = null;
        }

        //=================================================================================================
        //=================================================================================================
        //METODO PARA ESCANEAR (CAMARA)
        async private void Escanear(object sender, EventArgs e)
        {
            //SE CREA E INICIALIZA LA VARIABLE "SearchStatus" 
            //VARIABLE QUE INDICARA SI EL ESTATUS DE BUSQUEDA FUE SATISFACTORIO (SE OBTUVO UN RESULTADO) => TRUE
            //VARIABLE QUE INDICARA SI EL ESTATUS DE BUSQUEDA NO FUE SATISFACTORIO (NO SE OBTUVO UN RESULTADO) => FALSE
            bool SearchStatus = false;

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

                    //SE OCULTA EL FRAME QUE CONTENDRAN LOS RESULTADOS DE BUSQUEDA
                    FrameResultado.IsVisible = false;

                    //SE ACTIVA EL "ActivityIndicator" MIENTRAS DE MANERA ASINCRONA SE REALIZA EL LLAMADO DEL TABLERO
                    ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = true;
                    await Task.Run(async () =>
                    {
                        //SE HACE UN LLAMADO AL METODO BUSQUEDA DEL OBJETO "DatosPagina" EL CUAL RETORNARA:
                        //TRUE = SE CONSIGUIO EL TABLERO
                        //FALSE = NO SE CONSIGUIO EL TABLERO O HUBO ALGUN ERROR EN EL PROCESO DE BUSQUEDA
                        SearchStatus = await DatosPagina.BuscarTablero(result.Text);
                        ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
                    });

                    if (SearchStatus)
                    {
                        //SE CAMBIA LA VISIBILIDAD DEL "FrameResultado" CON LOS RESULTADOS
                        FrameResultado.IsVisible = SearchStatus;
                        //------------------------------------------------------------------------------------------------
                        //SE LLENAN (MANUALMENTE) CADA UNO DE LOS CAMPOS DE INFORMACION
                        idtablero.Text = DatosPagina.TableroID;
                        sapid.Text = DatosPagina.SapID;
                        filialtablero.Text = DatosPagina.Filial;
                        areatablero.Text = DatosPagina.Area;
                        ultimaconsultatablero.Text = DatosPagina.UltimaFechaConsulta.ToString();
                        listViewItems.ItemsSource = DatosPagina.Items;
                        listViewItems.HeightRequest = (DatosPagina.Items.Count * HeightRow);
                        codigoqrtablero.Source = ImageSource.FromStream(() => new MemoryStream(DatosPagina.CodigoQRbyte));
                        //------------------------------------------------------------------------------------------------
                    }
                    else
                    {
                        //SE CAMBIA SI ES O NO VISIBLE EL FRAME CON LOS RESULTADOS
                        FrameResultado.IsVisible = SearchStatus;
                        //SE VUELVE A REAJUSTAR EL TAMAÑO DE LA LISTA DE ITEMS 
                        listViewItems.HeightRequest = 0;
                        //SE INFORMA AL USUARIO QUE EL TABLERO QUE ACABA DE SER ESCANEADO NO FUE LOCALIZADO
                        Toast.MakeText(Android.App.Application.Context, DatosPagina.HttpErrorResponse, ToastLength.Long).Show();
                    }
                    //=============================================================================
                    //=============================================================================
                });
            };
        }

        //=================================================================================================
        //=================================================================================================
        //METODO PARA BUSCAR POR ID DEL TABLERO
        async private void ConsultaID(object sender, EventArgs e)
        {
            //SE CREA E INICIALIZA LA VARIABLE "SearchStatus" 
            //VARIABLE QUE INDICARA SI EL ESTATUS DE BUSQUEDA FUE SATISFACTORIO (SE OBTUVO UN RESULTADO) => TRUE
            //VARIABLE QUE INDICARA SI EL ESTATUS DE BUSQUEDA NO FUE SATISFACTORIO (NO SE OBTUVO UN RESULTADO) => FALSE
            bool SearchStatus = false;

            //===============================================================
            //===============================================================
            //SE NOTIFICA DESDE DONDE SE HACE LA CONSULTA DE TABLERO
            DatosPagina.TipoDeConsulta = "CONSULTA_POR_ID";

            //===============================================================
            //===============================================================
            //SE EVALUA SI EL VALOR DEL ENTRY "entryTableroID" TIENE ALGUN VALOR
            //SE EVALUA QUE SE HAYA SELECCIONADO UNA OPCION DE BUSQUEDA
            if (!string.IsNullOrEmpty(entryTableroID.Text) &&
                PickerOpciones.SelectedIndex > -1)
            {
                //SE OCULTA EL FRAME QUE CONTENDRAN LOS RESULTADOS DE BUSQUEDA
                FrameResultado.IsVisible = false;

                //SE ACTIVA EL "ActivityIndicator" MIENTRAS DE MANERA ASINCRONA SE REALIZA EL LLAMADO DEL TABLERO
                ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = true;
                await Task.Run(async () =>
                {
                    //SE HACE UN LLAMADO AL METODO BUSQUEDA DEL OBJETO "DatosPagina" EL CUAL RETORNARA:
                    //TRUE = SE CONSIGUIO EL TABLERO
                    //FALSE = NO SE CONSIGUIO EL TABLERO O HUBO ALGUN ERROR EN EL PROCESO DE BUSQUEDA
                    DatosPagina.OpcionConsultaID = PickerOpciones.SelectedIndex;
                    SearchStatus = await DatosPagina.BuscarTablero(entryTableroID.Text);
                    ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
                });

                //EVALUAMOS SI EL TABLERO SE ENCUENTRA EN LA BASE DE DATOS
                if (SearchStatus)
                {
                    //SE CAMBIA LA VISIBILIDAD DEL "FrameResultado" CON LOS RESULTADOS
                    FrameResultado.IsVisible = SearchStatus;
                    //------------------------------------------------------------------------------------------------
                    //SE LLENAN (MANUALMENTE) CADA UNO DE LOS CAMPOS DE INFORMACION
                    idtablero.Text = DatosPagina.TableroID;
                    sapid.Text = DatosPagina.SapID;
                    filialtablero.Text = DatosPagina.Filial;
                    areatablero.Text = DatosPagina.Area;
                    ultimaconsultatablero.Text = DatosPagina.UltimaFechaConsulta.ToString();
                    listViewItems.ItemsSource = DatosPagina.Items;
                    listViewItems.HeightRequest = (DatosPagina.Items.Count * HeightRow);
                    codigoqrtablero.Source = ImageSource.FromStream(() => new MemoryStream(DatosPagina.CodigoQRbyte));
                    //------------------------------------------------------------------------------------------------
                }
                else
                {
                    //SE CAMBIA SI ES O NO VISIBLE EL FRAME CON LOS RESULTADOS
                    //FrameResultado.IsVisible = DatosPagina.ShowResultadoScan;
                    FrameResultado.IsVisible = SearchStatus;
                    //SE VUELVE A REAJUSTAR EL TAMAÑO DE LA LISTA DE ITEMS 
                    listViewItems.HeightRequest = 0;
                    //SE INFORMA AL USUARIO QUE EL TABLERO QUE ACABA DE SER ESCANEADO NO FUE LOCALIZADO
                    Toast.MakeText(Android.App.Application.Context, DatosPagina.HttpErrorResponse, ToastLength.Long).Show();
                }
            }
            //SI EL VALOR DEL ENTRY "entryTableroID" ESTA VACIO O NULO SE NOTIFICA AL USUARIO
            else
            {
                //-----------------------------------------------------------------------------------------
                //DE RETORNAR FALSA O FALLAR ALGUNA DE LAS EVALUACIONES QUE SE REALIZARON EN EL
                //CONDICIONAL ANTERIOR SE RETORNA UN MENSAJE INFORMANDOLE AL USUARIO CUAL DE LAS
                //CONDICIONES PLANTEADAS NO SE CUMPLE
                if (PickerOpciones.SelectedIndex == -1)
                    await DisplayAlert("Mensaje", "Debe seleccionar la opcion de consulta", "Entendido");
                //-----------------------------------------------------------------------------------------
                if (string.IsNullOrEmpty(entryTableroID.Text))
                    await DisplayAlert("Mensaje", "Debe ingresar el parametro de consulta", "Entendido");
                //-----------------------------------------------------------------------------------------
            }
        }

        //=================================================================================================
        //=================================================================================================
        //METODO PARA GUARDAR EL CODIGO QR DENTRO DE LA GELERIA DEL TELEFONO
        private void GuardarCodigoQR(object sender, EventArgs e)
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