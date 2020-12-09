using Android.Widget;
using MTTO_App.Tablas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MTTO_App
{
    internal class QueryAdminViewModel
    {
        public QueryAdminViewModel(Personas persona, Usuarios usuario)
        {
            Persona = persona;
            Usuario = usuario;
        }

        //---------------------------------------------------------------------------------------------------------
        //PROPIEDADES DE LA CLASE
        private Personas Persona;
        private Usuarios Usuario;
        //---------------------------------------------------------------------------------------------------------
        //PROPIEDADES DE LA CLASE
        public List<QueryAdminModel> InfoConfig { get { return new QueryAdminModel().GetConfiguracion(); } } 
        //---------------------------------------------------------------------------------------------------------
        //TEXTOS
        public string TituloPagina { get { return "Busqueda de Usuario"; } }
        public string BusquedaPH { get { return "Busqueda:"; } }
        public string EntryDatosPH { get { return "Ingrese el dato a buscar"; } }
        public string ColumnaCedula { get { return "Cedula (ID)"; } }
        public string ColumnaNombres { get { return "Nombre(s)"; } }
        public string ColumnaApellidos { get { return "Apellido(s)"; } }

        //---------------------------------------------------------------------------------------------------------
        //COLOR DE FONTO Y DE BOTONES
        public string BackGroundColor { get { return App.BackGroundColor; } }

        public string ButtonColor { get { return App.ButtonColor; } }

        //---------------------------------------------------------------------------------------------------------
        //TAMAÑO DE LAS LETRAS
        public int LabelFontSize { get { return App.LabelFontSize; } }

        public int EntryFontSize { get { return App.EntryFontSize; } }
        public int HeaderFontSize { get { return App.HeaderFontSize; } }

        //=========================================================================================================
        //-------------------------------------------------METODOS-------------------------------------------------
        //=========================================================================================================
        //BUSQUEDA DE PERSONAS
        public async Task<List<Personas>> ListaPersonas(int SeleccionBusqueda, string Dato)
        {
            //SE RECIBE COMO PARAMETRO QUE TIPO DE BUSQUEDA VA A REALIZARSE (SeleccionBusqueda)
            //LA CUAL PERMITE BUSCAR POR: ID, NOMBRES, APELLIDOS y USERNAME. MIENTRAS QUE Dato
            //TENDRA LA REFERENCIA A BUSCAR

            //BUSQUEDA DE USUARIO STAND ALONE
            //return ListaPersonasStandAlone(SeleccionBusqueda, Dato);

            //BUSQUEDA DE USUARIO HTTPCLIENT

            return await ListaPersonasHttpClient(SeleccionBusqueda, Dato);
            
        }

        //==================================================================================
        //==================================================================================
        //FUNCION PARA GENERAR UN MENSAJE NO INTERACTIVO POR PANTALLA
        public void MensajePantalla(string mensaje)
        {
            Toast.MakeText(Android.App.Application.Context, mensaje, ToastLength.Short).Show();
        }

        //==================================================================================
        //==================================================================================
        //BUSQUEDA DE USUARIOS (STAND ALONE)
        private List<Personas> ListaPersonasStandAlone(int SeleccionBusqueda, string Dato)
        {
            List<Personas> QueryPersonas = new List<Personas>();
            List<double> ListaUsuarioID = new List<double>();

            //SE APERTURA LA BASE DE DATOS
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.FileName))
            {
                //LISTA DE PERSONAS Y USUARIOS REGISTRADOS
                List<Personas> Personas = connection.Table<Personas>().ToList();
                List<Usuarios> Usuarios = connection.Table<Usuarios>().ToList();

                //DEPENDIENDO DEL VALOR QUE POSEA SeleccionBusqueda:
                //(EN ESTE CASO SOLO PUEDE TENER 4, QUE SON: 0-ID, 1-NUMERO DE FICHA, 2-NOMBRES, 3-APELLIDOS, 4-USERNAME)
                switch (SeleccionBusqueda)
                {
                    //CASO BUSQUEDA POR CEDULA
                    case 0:
                        //=======================================================================================
                        //=======================================================================================
                        //BUSQUEDA DE USUARIOS POR NUMERO DE CEDULA (STAND ALONE)
                        foreach (Personas persona in Personas)
                        {
                            if (persona.Cedula != 0)
                            {
                                string w = persona.Cedula.ToString().Substring(0, Dato.Length);

                                if (Dato == w)
                                    QueryPersonas.Add(persona);
                            }
                        }
                        break;
                    //CASO BUSQUEDA POR NUMERO DE FICHA
                    case 1:
                        //=======================================================================================
                        //=======================================================================================
                        //BUSQUEDA DE USUARIOS POR NUMERO DE FICHA (STAND ALONE)
                        foreach (Personas persona in Personas)
                        {
                            if (persona.NumeroFicha != 0)
                            {
                                if (Dato == persona.NumeroFicha.ToString().Substring(0, Dato.Length))
                                    QueryPersonas.Add(persona);
                            }
                        }
                        break;
                    //CASO BUSQUEDA POR NOMBRE
                    case 2:
                        //=======================================================================================
                        //=======================================================================================
                        //BUSQUEDA DE USUARIOS POR NOMBRE PERSONAL (STAND ALONE)
                        foreach (Personas persona in Personas)
                        {
                            if (persona.Cedula != 0)
                            {
                                if (Dato.ToLower() == persona.Nombres.Substring(0, Dato.Length).ToLower())
                                {
                                    QueryPersonas.Add(persona);
                                }
                            }
                        }
                        break;
                    //CASO BUSQUEDA POR APELLIDO
                    case 3:
                        //=======================================================================================
                        //=======================================================================================
                        //BUSQUEDA DE USUARIOS POR APELLIDO PERSONAL (STAND ALONE)
                        foreach (Personas persona in Personas)
                        {
                            if (persona.Cedula != 0)
                            {
                                if (Dato.ToLower() == persona.Apellidos.Substring(0, Dato.Length).ToLower())
                                {
                                    QueryPersonas.Add(persona);
                                }
                            }
                        }
                        break;
                    //CASO BUSQUEDA POR USERNAME
                    case 4:
                        //=======================================================================================
                        //=======================================================================================
                        //BUSQUEDA DE USUARIOS POR NOMBRE DE USUARIO (STAND ALONE)
                        //SE GUARDAN LOS ID DE LOS USUARIOS QUE CUMPLEN CON EL DATO SUMINISTRADO
                        foreach (Usuarios usuario in Usuarios)
                        {
                            if (usuario.Cedula != 0)
                            {
                                if (Dato.ToLower() == usuario.Username.Substring(0, Dato.Length).ToLower())
                                {
                                    ListaUsuarioID.Add(usuario.Cedula);
                                }
                            }
                        }
                        //SE AÑADEN LAS PERSONAS QUE CUMPLAN CON EL ID DE LA LISTA PREVIAMENTE LLENADA
                        foreach (Personas persona in Personas)
                        {
                            foreach (int ID in ListaUsuarioID)
                            {
                                if (persona.Cedula == ID)
                                {
                                    QueryPersonas.Add(persona);
                                }
                            }
                        }
                        break;
                        //=======================================================================================
                        //======================================================================================= 
                }
                //SE CIERRA LA BASE DE DATOS
                connection.Close();
            }
            //SE RETORNA LA LISTA CON LAS PERSONAS QUE CUMPLAN CON LA INFORMACION SOLICITADA
            return QueryPersonas;
        }

        private async Task<List<Personas>> ListaPersonasHttpClient(int SeleccionBusqueda, string Dato)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE CONTENDRA EL URL 
            string url = App.BaseUrl + "/queryadmin";

            //SE CREA E INICIALIZA LA VARIABLE QUE RECIBIRA LA LISTA DE USUARIOS ENCONTRADOS
            List<Personas> lista = null;

            //SE CREA E INICIALIZA LA VARIABLE QUE SERVIRA DE MODELO PARA EL OBJETO JSON ENVIADO EN LA SOLICITUD
            var model = new RequestQueryAdmin() { Parametro = Dato, UserId = Persona.Cedula, };

            //SE CREA E INICIALIZA LA VARIABLE QUE RECIBIRA LA RESPUESTA DE LA SOLICITUD HTTP
            HttpResponseMessage response = new HttpResponseMessage();

            //SE CREA E INICIALIZA LA VARIABLE QUE VERIFICARA EL ESTADO DE CONEXION A INTERNET
            var current = Connectivity.NetworkAccess;

            //SE VERIFICA SI EL DISPOSITIVO SE ENCUENTRA CONECTADO A INTERNET
            if (current == NetworkAccess.Internet)
            {
                //EL EQUIPO SE ENCUENTRA CONECTADO A INTERNET
                //SE INICIA EL CICLO TRY...CATCH
                try
                {
                    //INICIAMOS EL SEGMENTO DEL CODIGO EN EL CUAL REALIZAREMOS EL CONSUMO DE SERVICIOS WEB MEDIANTE
                    //LA INICIALIZACION Y CREACION DE UNA VARIABLE QUE FUNCIONARA COMO CLIENTE EN LAS SOLICITUDES 
                    //Y RESPUESTAS ENVIADAS Y RECIBIDAS POR EL SERVIDOR (WEB API) 
                    //----------------------------------------------------------------------------------------------
                    //NOTA: CUANDO SE REALIZA LA CREACION E INICIALIZACION DE LA VARIABLE DEL TIPO HttpClient SE
                    //HACE UN LLAMADO A UN METODO ALOJADO EN LA CLASE "App" Y QUE ES ENVIADO COMO PARAMETRO DEL 
                    //TIPO HttpClientHandler => 
                    //----------------------------------------------------------------------------------------------
                    using (HttpClient client = new HttpClient(App.GetInsecureHandler()))
                    {
                        //SE DA SET AL TIEMPO MAXIMO DE ESPERA PARA RECIBIR UNA RESPUESTA DEL SERVIDOR
                        client.Timeout = TimeSpan.FromSeconds(App.TimeInSeconds);
                        //SE REALIZA LA CONVERSION A OBJETO JSON
                        var json = JsonConvert.SerializeObject(model);

                        HttpRequestMessage request = new HttpRequestMessage()
                        {
                            Method = HttpMethod.Get,
                            Content = new StringContent(json, Encoding.UTF8, "application/json"),
                        };

                        //SE HACE LA CONFIGURACION DE LOS HEADERS DEL REQUEST
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //--------------------------------------------------------------------------------------------------------
                        //DEPENDIENDO DEL VALOR QUE POSEA SeleccionBusqueda:
                        //(EN ESTE CASO SOLO PUEDE TENER 4, QUE SON: 0-ID, 1-NUMERO DE FICHA, 2-NOMBRES, 3-APELLIDOS, 4-USERNAME)
                        //--------------------------------------------------------------------------------------------------------
                        switch (SeleccionBusqueda)
                        {
                            //CASO BUSQUEDA POR CEDULA
                            case 0:
                                //SE TERMINA DE ESPECIFICAR EL URL AL CUAL SE DEBE REALIZAR LA SOLICITUD CUANDO ES BUSQUEDA POR CEDULA
                                request.RequestUri = new Uri(url + "/cedula");
                                //SE HACE EL ENVIO DE LA SOLICITUD
                                response = await client.SendAsync(request);
                                break;
                            //CASO BUSQUEDA POR NUMERO DE FICHA
                            case 1:
                                //SE TERMINA DE ESPECIFICAR EL URL AL CUAL SE DEBE REALIZAR LA SOLICITUD CUANDO ES BUSQUEDA POR CEDULA
                                request.RequestUri = new Uri(url + "/ficha");
                                //SE HACE EL ENVIO DE LA SOLICITUD
                                response = await client.SendAsync(request);
                                break;
                            //CASO BUSQUEDA POR NOMBRE
                            case 2:
                                //SE TERMINA DE ESPECIFICAR EL URL AL CUAL SE DEBE REALIZAR LA SOLICITUD CUANDO ES BUSQUEDA POR CEDULA
                                request.RequestUri = new Uri(url + "/nombres");
                                //SE HACE EL ENVIO DE LA SOLICITUD
                                response = await client.SendAsync(request);
                                break;
                            //CASO BUSQUEDA POR APELLIDO
                            case 3:
                                //SE TERMINA DE ESPECIFICAR EL URL AL CUAL SE DEBE REALIZAR LA SOLICITUD CUANDO ES BUSQUEDA POR CEDULA
                                request.RequestUri = new Uri(url + "/apellidos");
                                //SE HACE EL ENVIO DE LA SOLICITUD
                                response = await client.SendAsync(request);
                                break;
                            //CASO BUSQUEDA POR USERNAME
                            case 4:
                                //SE TERMINA DE ESPECIFICAR EL URL AL CUAL SE DEBE REALIZAR LA SOLICITUD CUANDO ES BUSQUEDA POR CEDULA
                                request.RequestUri = new Uri(url + "/username");
                                //SE HACE EL ENVIO DE LA SOLICITUD
                                response = await client.SendAsync(request);
                                break;
                        }
                    }
                }
                catch
                {

                }
            }

            //SE RETORNA LA RESPUESTA OBTENIDA POR EL SERVICIO WEB

            if (response.IsSuccessStatusCode)
                lista = JsonConvert.DeserializeObject<List<Personas>>(await response.Content.ReadAsStringAsync());

            return await Task.FromResult(lista);
        }
    } 
}   