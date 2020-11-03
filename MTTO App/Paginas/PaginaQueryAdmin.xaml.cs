using MTTO_App.Paginas;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTTO_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaQueryAdmin : ContentPage
    {
        //LISTA QUE ALMACENARA TODOS LOS OBJETOS PERSONAS
        //QUE SON OBTENIDOS LUEGO DE REALIZAR LA BUSQUEDA
        private List<Personas> Lista;

        //SE CREAN LAS VARIABLES
        private int Seleccion;

        //SE CREAN LAS CONSTANTES
        private const int HeightRow = 45;

        //SE CREAN LOS OBJETOS
        private QueryAdminViewModel ConexionDatos;

        public PaginaQueryAdmin()
        {
            InitializeComponent();

            //============================================================================
            //============================================================================

            //LLENADO DEL STACKLAYOUT MENU
            BindableLayout.SetItemsSource(MenuBusqueda, new QueryAdminViewModel().InfoConfig);

            //============================================================================
            //============================================================================
            //REALIZAMOS LA CONEXION CON LA CLASE VIEW MODEL "PaginaQueryViewModel.cs"
            BindingContext = ConexionDatos = new QueryAdminViewModel();

            //PUESTO QUE NO SE HA REALIZADO NINGUNA BUSQUEDA, LA SECCION QUE ORDENARA
            //EN FORMA DE LISTA TODOS LOS RESULTADOS ENCONTRADOS ESTARA OCULTA HASTA QUE
            //SE GENERE UNA NUEVA BUSQUEDA
            FrameListaBusqueda.IsVisible = false;
            //ADEMAS EL OBJETO ActivityIndicator DEBE SER SETTEADO (FALSE)
            ActivityIndicatorBusqueda.IsVisible = ActivityIndicatorBusqueda.IsRunning = false;
        }

        //========================================================================================================
        //========================================================================================================
        //METODO ACTIVADO CUANDO SE SELECCIONA UNA OPCION DE BUSQUEDA, ESTO CON LA FINALIDAD DE LIMITAR AL USUARIO
        //A USAR EL TECLADO NUMERICO CUANDO SELECCIONE BUSQUEDA POR ID
        private void OnSelectedOpciones(object sender, EventArgs args)
        {
            //============================================
            //============================================
            //Seleccion INDICA QUE OPCION FUE SELECCIONADA
            Picker picker = sender as Picker;
            Seleccion = picker.SelectedIndex;

            if (picker.SelectedIndex < 2)
                EntryDatos.Keyboard = Keyboard.Numeric;
            else
                EntryDatos.Keyboard = Keyboard.Text;
        }

        //========================================================================================================
        //========================================================================================================
        //METODO ACTIVADO AL PRESIONAR EL BOTON BUSCAR
        protected async void OnBuscar(Object sender, EventArgs e)
        {
            Lista = new List<Personas>();
            FrameListaBusqueda.IsVisible = false;
            ListViewPersonas.ItemsSource = Lista = null;
            int Cant = 0;

            //================================================================================
            //================================================================================

            //PRIMERO SE VERIFICA QUE LA ENTRADA "EntryDatos" NO SE ENCUENTRE VACIO

            if (string.IsNullOrEmpty(EntryDatos.Text))
                //DE ESTARLO SE LE NOTIFICARA AL USUARIO
                ConexionDatos.MensajePantalla("Debe ingresar los datos a buscar");
            else
            {
                //----------------------------------------------------------------------------
                ActivityIndicatorBusqueda.IsVisible = ActivityIndicatorBusqueda.IsRunning = true;
                await Task.Delay(1125);
                ActivityIndicatorBusqueda.IsVisible = ActivityIndicatorBusqueda.IsRunning = false;
                //----------------------------------------------------------------------------

                //LUEGO SE EVALUA CUAL ES LA OPCION DE BUSQUEDA SELECCIONADA
                switch (Seleccion)
                {
                    //OPCION BUSQUEDA POR ID
                    case 0:
                        //SE VERIFICA QUE NO EXISTAN ESPACIOS EN BLANCO
                        //AL MOMENTO DE TOMAR EL ID PROPORCIONADO POR
                        //EL USUARIO
                        if (!Metodos.EspacioBlanco(EntryDatos.Text))
                        {
                            //SE LLENA LA LISTA CON EL RESULTADO ARROJADO POR EL
                            //METODO "ListasPersonas", PERTENECIENTE A LA CLASE
                            //"Metodos"
                            Lista = ConexionDatos.ListaPersonas(Seleccion, EntryDatos.Text);

                            //SE EVALUA EL ESTADO DE LA LISTA (SI ESTA POPULADA O VACIA)
                            if (Lista == null)
                            {
                                //DE ESTAR VACIA SE LE NOTIFICARA AL USUARIO QUE
                                //NO SE ENCONTRO NINGUN USUARIO QUE RESPONDA A ESE ID (CEDULA)
                                ConexionDatos.MensajePantalla("No se obtuvo ningun resultado de busqueda");
                            }
                            else
                            {
                                //DE ENCONTRAR RESULTADOS (LISTA POPULADA) SE PROCEDE A
                                //LLENAR EL LISTVIEW "ListViewPersonas" CON EL RESULTADO
                                //OBTENIDO PREVIAMENTE
                                ListViewPersonas.ItemsSource = Lista;
                                //SE PROCEDE A CONTAR LA CANTIDAD DE PERSONAS QUE POSEE LA LISTA
                                //PARA LUEGO DIMENSIONAR EL TAMAÑO DEL LISTVIEW
                                Cant = Lista.Count;
                                ListViewPersonas.HeightRequest = (Cant * HeightRow);
                                //POR ULTIMO SE HABILITA LA VISIBILIDAD DEL "FrameListaBusqueda"
                                //QUE ALBERGA A "ListViewPersonas".
                                FrameListaBusqueda.IsVisible = true;
                            }
                        }
                        else
                            //SI EL ID INTEGRADO TIENE UN ESPACIO EN BLANCO
                            //SE LE NOTIFICA AL USUARIO MEDIANTE UN MENSAJE DE ALERTA
                            await DisplayAlert("Alerta", "El numero de ID (cedula) no puede contener espacios en blanco", "Entendido");
                        break;
                    //OPCION BUSQUEDA POR NUMERO DE FICHA
                    case 1:
                        //SE VERIFICA QUE NO EXISTAN MAS DE DOS ESPACIOS EN BLANCO
                        if (!Metodos.EspacioBlanco(EntryDatos.Text))
                        {
                            //SE LLENA LA LISTA CON EL RESULTADO ARROJADO POR EL METODO "ListaPersonas"
                            Lista = ConexionDatos.ListaPersonas(Seleccion, EntryDatos.Text);

                            if (Lista == null)
                            {
                                //LA LISTA SE ENCUENTRA VACIA: NO SE ENCONTRO NINGUN RESULTADO
                                ConexionDatos.MensajePantalla("No se obtuvo ningun resultado de busqueda");
                            }
                            else
                            {
                                //DE ENCONTRAR RESULTADOS (LISTA POPULADA) SE PROCEDE A LLENAR EL LISTVIEW "ListViewPersonas" CON EL RESULTADO
                                //OBTENIDO PREVIAMENTE
                                ListViewPersonas.ItemsSource = Lista;
                                //SE PROCEDE A CONTAR LA CANTIDAD DE PERSONAS QUE POSEE LA LISTA PARA LUEGO DIMENSIONAR EL TAMAÑO DEL LISTVIEW
                                Cant = Lista.Count;
                                ListViewPersonas.HeightRequest = (Cant * HeightRow);
                                //POR ULTIMO SE HABILITA LA VISIBILIDAD DEL "FrameListaBusqueda" QUE ALBERGA A "ListViewPersonas".
                                FrameListaBusqueda.IsVisible = true;
                            }
                        }
                        else
                        {
                            //SI EL NUMERO DE FICHA INGRESADO TIENE UN ESPACIO EN BLANCO SE LE NOTIFICA AL USUARIO MEDIANTE UN MENSAJE DE ALERTA
                            await DisplayAlert("Alerta", "El numero de ficha no puede contener espacios en blanco", "Entendido");
                        }
                        break;
                    //OPCION BUSQUEDA POR NOMBRE(S)
                    case 2:
                        //SE VERIFICA QUE NO EXISTAN MAS DE DOS ESPACIOS EN BLANCO
                        //(ESTO TOMANDO EN CUENTA QUE SOLO SE PUEDE REGISTRAR UN MAXIMO
                        //DE DOS NOMBRES POR REGISTRO)
                        if (Metodos.CuantosEspaciosBlanco(EntryDatos.Text) < 2)
                        {
                            Lista = ConexionDatos.ListaPersonas(Seleccion, EntryDatos.Text);

                            if (Lista == null)
                                ConexionDatos.MensajePantalla("No se obtuvo ningun resultado de busqueda");
                            else
                            {
                                ListViewPersonas.ItemsSource = Lista;
                                Cant = Lista.Count;
                                ListViewPersonas.HeightRequest = (Cant * HeightRow);
                                FrameListaBusqueda.IsVisible = true;
                            }
                        }
                        else
                            //SI EL TEXTO INGRESADO TIENE MAS DE DOS ESPACIOS EN BLANCO
                            //SE PROCEDE A NOTIFICARLE AL USUARIO
                            await DisplayAlert("Alerta", "Verifique la cantidad de espacios en blanco ingresados", "Entendido");
                        break;
                    //OPCION BUSQUEDA POR APELLIDO(S)
                    case 3:
                        //BASICAMENTE CUMPLE LA MISMA RUTINA DE BUSQUEDA
                        //QUE LA OPCION DE BUSQUEDA POR NOMBRE(S)
                        if (Metodos.CuantosEspaciosBlanco(EntryDatos.Text) < 2)
                        {
                            Lista = ConexionDatos.ListaPersonas(Seleccion, EntryDatos.Text);

                            if (Lista == null)
                                ConexionDatos.MensajePantalla("No se obtuvo ningun resultado de busqueda");
                            else
                            {
                                ListViewPersonas.ItemsSource = Lista;
                                Cant = Lista.Count;
                                ListViewPersonas.HeightRequest = (Cant * HeightRow);
                                FrameListaBusqueda.IsVisible = true;
                            }
                        }
                        else
                            await DisplayAlert("Alerta", "Verifique la cantidad de espacios en blanco ingresados", "Entendido");
                        break;
                    //OPCION BUSQUEDA POR NOMBRE DE USUARIO
                    case 4:
                        //BASICAMENTE CUMPLE LA MISMA RUTINA DE BUSQUEDA
                        //QUE LA OPCION DE BUSQUEDA POR ID
                        if (!Metodos.EspacioBlanco(EntryDatos.Text))
                        {
                            Lista = ConexionDatos.ListaPersonas(Seleccion, EntryDatos.Text);

                            if (Lista == null)
                                ConexionDatos.MensajePantalla("No se obtuvo ningun resultado de busqueda");
                            else
                            {
                                ListViewPersonas.ItemsSource = Lista;
                                Cant = Lista.Count;
                                ListViewPersonas.HeightRequest = (Cant * HeightRow);
                                FrameListaBusqueda.IsVisible = true;
                            }
                        }
                        else
                            await DisplayAlert("Alerta", "El numero de ID (cedula) no puede contener espacios en blanco", "Entendido");
                        break;
                }
            }
        }

        //========================================================================================================
        //========================================================================================================
        //METODO ACTIVADO AL SELECCIONAR UN ITEM DE LA LISTA DE RESULTADOS DE BUSQUEDA
        async private void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            //TOMAMOS LA INFORMACION DEL ITEM SELECCIONADO
            var Item = e.Item as Personas;

            //REALIZAMOS LA PREGUNTA SOBRE SI SE DESEA MODIFICAR LA INFORMACION DEL USUARIO
            bool response = await DisplayAlert("Mensaje", "¿Desea modificar la informacion de este usuario?", "Si", "No, gracias");

            if (response)
            {
                //----------------------------------------------------------------------------
                /*ActivityIndicatorSeleccion.IsVisible = ActivityIndicatorSeleccion.IsRunning = true;
                await Task.Delay(750);
                ActivityIndicatorSeleccion.IsVisible = ActivityIndicatorSeleccion.IsRunning = false;*/
                //----------------------------------------------------------------------------

                //----------------------------------------------------------------------------
                ActivityIndicatorBusqueda.IsVisible = ActivityIndicatorBusqueda.IsRunning = true;
                await Task.Delay(750);
                ActivityIndicatorBusqueda.IsVisible = ActivityIndicatorBusqueda.IsRunning = false;
                //----------------------------------------------------------------------------

                //SE APERTURA LA CONEXION CON LA BASE DE DATOS
                using (SQLiteConnection connection = new SQLiteConnection(App.FileName))
                {
                    //SE CREA LA LISTA DE USUARIOS
                    List<Usuarios> ListaUsuarios = connection.Table<Usuarios>().ToList();

                    //SE BUSCA UN MATCH DE ID
                    foreach (Usuarios usuario in ListaUsuarios)
                    {
                        //SE COMPRARAN LOS ID
                        if (usuario.Cedula == Item.Cedula)
                        {
                            //SI LOS ID DEL OBJETO usuario ES IGUAL AL ID DEL OBJETO item (DEL TIPO Persona) SE PROCEDE A:

                            //PUESTO QUE SE HA INGRESADO DESDE LA CUENTA DE ADMINISTRADOR
                            //SE ACTIVA UNA BANDERA QUE MARQUE ESTE INGRESO
                            App.ConfigChangedAdminFlag = true;
                            //SE LLAMA A LA PAGINA DE CONFIGURACION DE ADMINISTRADOR
                            await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConfiguracionAdmin(Item, usuario));
                            //SE BORRA LA INFORMACION SUMINISTRADA POR EL USUARIO
                            EntryDatos.Text = string.Empty;
                            //SE LIMPIA LA LISTA
                            ListViewPersonas.ItemsSource = null;
                            //SE ESCONDE LA LISTA
                            FrameListaBusqueda.IsVisible = false;
                            break;
                        }
                    }
                    connection.Close();
                }
            }

            ((ListView)sender).SelectedItem = null;
        }
    }
}