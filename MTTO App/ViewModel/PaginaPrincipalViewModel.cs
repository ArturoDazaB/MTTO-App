using System;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.Threading.Tasks;
using MTTO_App.Tablas;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Essentials;
using System.Net.Http.Headers;

namespace MTTO_App.ViewModel
{
    public class PaginaPrincipalViewModel : INotifyPropertyChanging
    {
        //=========================================================================================================
        //=========================================================================================================
        //VARIABLES DE LA CLASE
        protected string username = string.Empty,
                         password = string.Empty;

        protected string result;

        //=========================================================================================================
        //=========================================================================================================
        //PROPIEDADES DE LA CLASE
        public string Username
        {
            get { return username; }
            set
            {
                OnPropertyChanged();
                username = value;
                ConsoleWriteline("USERNAME/NOMBRE DE USUARIO", username);
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                OnPropertyChanged();
                password = value;
                ConsoleWriteline("PASSWORD/CONTRASEÑA", password);
            }
        }

        public string Result
        {
            get { return result; }
        }

        //=========================================================================================================
        //=========================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaPrincipalViewModel()
        {
            //SE NOTIFICA MENDIANTE UN MENSAJE POR CONSOLA
            ConsoleWriteline("SE HA INICIADO LA APLICACION");
        }

        //=========================================================================================================
        //---------------------------------------------------METODOS-----------------------------------------------
        //=========================================================================================================
        //METODO PARA IMPRIMIR POR CONSOLA EL CAMBIO DE LA VARIABLE
        private void ConsoleWriteline(string PROPIEDAD, string mensaje)
        {
            Console.WriteLine("\n\n==================================================");
            Console.WriteLine("==================================================");
            Console.WriteLine("\nPROPIEDAD: " + PROPIEDAD + "\nVALOR: " + mensaje);
            Console.WriteLine("==================================================");
            Console.WriteLine("==================================================\n\n");
        }

        private void ConsoleWriteline(string mensaje)
        {
            Console.WriteLine("\n\n==================================================");
            Console.WriteLine("==================================================");
            Console.WriteLine("\nMENSAJE: \n" + mensaje);
            Console.WriteLine("==================================================");
            Console.WriteLine("==================================================\n\n");
        }

        
        public async Task<LogInResponse> LogInRequest()
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE RETENDRA EL URL PARA REALIZAR LA SOLICITUD HTTP
            string url = App.BaseUrl + $"/login?username={Username}&password={Password}";

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

                        //SE CONFIGURAN LOS HEADERS DE LA SOLICITUD HTTP (HTTP REQUEST)
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //SE REALIZA EL LLAMADO Y SE RECIBE UNA RESPUESTA
                        HttpResponseMessage response = await client.GetAsync(url);

                        //SE EVALUA SI EL CODIGO DE ESTATUS RETORNADO ES EL CODIGO 200 OK
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //EL CODIGO DE ESTATUS RETORNADO ES EL CODIGO 200 OK
                            //SE RECIBE LA RESPUESTA COMPLETA OBTENIDA POR EL SERVIDOR (STRING) 
                            result = await response.Content.ReadAsStringAsync();
                            //SE REALIZA LA EL MAPEO DE UN OBJETO JSON A UN OBJETO "LogInResponse"
                            return await Task.FromResult(JsonConvert.DeserializeObject<LogInResponse>(result));
                        }
                        else
                        {
                            //EL CODIGO DE ESTATUS RETORNADO ES DIFERENTE AL CODIGO DE ESTATUS 200 OK
                            //NOTA: EN EL CASO DEL CONTROLADOR ASIGNADO A PROCESAR LAS SOLICITUDES CON DICHA DIRECCION URL
                            //DENTRO DEL PROYECTO API SOLO TIENE TRES TIPOS DE RESPUESTA:
                            // - Ok (Se consiguo el nombre de usuario y la contraseña coincide)
                            // - Bad Request (Se consiguio el nombre de usuario pero la contraseña no coincide)
                            // - NotFound (No se consiguio el nombre de usuario)
                            result = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                            //PUESTO QUE NO SE OBTUVO EL CODIGO DE ESTATUS 200 OK SE DESERIALIZA LA RESPUESTA OBTENIDA
                            //Y SE ALMACENA EN LA VARIABLE LOCAL "result"
                            //PUESTO QUE NO SE OBTUVO EL CODIGO 200 OK RETORNAREMOS NULL COMO RESPUESTA AL LLAMADO DEL METODO
                            return null;
                        }
                    }
                }
                //DE OCURRIR UNA EXCEPCION DENTRO DEL CICLO TRY...CATCH SE PROCEDE A EJECUTAR LAS LINEAS DE CODIGO
                //CONTENIDAS DENTRO DE LA SECCION CATCH. AQUI SE EVALUARAN LAS EXCEPCIONES MAS COMUNES OBTENIDAS DURANTE
                //LAS PRUEBAS DE CONEXION CON EL SERVICIO WEB API
                catch (Exception ex) when (ex is HttpRequestException ||                
                                           ex is Javax.Net.Ssl.SSLException ||
                                           ex is Javax.Net.Ssl.SSLHandshakeException ||
                                           ex is System.Threading.Tasks.TaskCanceledException)
                {
                    //SE MANDA A IMPRIMIR POR CONSOLA EL ERROR OBTENIDO (EJECUTADO SOLO CUANDO SE DEPURA EL PROYECTO)
                    ConsoleWriteline("\nOcurrio un error => \n\n" + ex.Message.ToString());
                    //SE NOTIFICA AL USUARIO QUE NO SE PUDO REALIZAR LA SOLICITUD WEB.
                    result = "\nProblemas de conexion con el servidor";
                    //PUESTO QUE NO SE OBTUVO EL CODIGO 200 OK RETORNAREMOS NULL COMO RESPUESTA AL LLAMADO DEL METODO
                    return null;
                }
            }
            else
            {
                //EL EQUIPO NO ENCUENTRA CONECTADO A INTERNET, SE LE NOTIFICA AL USUARIO.
                result = "No hay conexion a internet";
                //PUESTO QUE NO SE OBTUVO EL CODIGO 200 OK RETORNAREMOS NULL COMO RESPUESTA AL LLAMADO DEL METODO
                return null;
            }
        }


        //=========================================================================================================
        //=========================================================================================================
        public event PropertyChangingEventHandler PropertyChanging;

        //ACTUALIZA LA INFORMACION DE LA PROPIEDAD CADA QUE SE DECTECTA UN CAMBIO MINIMO
        private void OnPropertyChanged([CallerMemberName] string nombre = "")
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nombre));
        }

        //=========================================================================================================
        //----------------------------------------PROPIEDADES DE LA PAGINA-----------------------------------------
        //=========================================================================================================
        //COLORES
        public string BackGroundColor { get { return App.BackGroundColor; } }

        public string HeaderBackGroundColor { get { return "#E53835"; } }
        public string EntryBackGroundColor { get { return "#424242"; } }
        public string IngresoDatosBackGroundColor { get { return "#ff6e60"; } }

        //--------------------------------------------------------------------------------------------------------
        //TEXTOS
        public string HeaderText { get { return "MTTO App"; } }

        public string UsernamePH { get { return "Nombre de usuario"; } }
        public string PasswordPH { get { return "Contraseña"; } }
        public string ButtonPH { get { return "INGRESAR"; } }

        //--------------------------------------------------------------------------------------------------------
        //TAMAÑO DE LA FUENTE
        public int LabelFontSize { get { return App.LabelFontSize; } }

        public int EntryFontSize { get { return App.EntryFontSize; } }
        public int HeaderFontSize { get { return App.HeaderFontSize; } }
    }
}