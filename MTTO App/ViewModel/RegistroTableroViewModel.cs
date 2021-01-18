using Android.Widget;
using MTTO_App.Tablas;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MTTO_App.ViewModel
{
    internal class RegistroTableroViewModel : INotifyPropertyChanging
    {
        //==================================================================================================
        //==================================================================================================
        //SE CREAN LAS VARIABLES LOCALES DE LA CLASE

        //ATRIBUTOS DEL OBJETO TABLERO: TableroID (Pagina de registro y consulta)
        //                              Filial (Pagina de registro y consulta)
        //                              Area (Pagina de registro y consulta)
        //                              Fecha de Creacion (Pagina de registro y consulta)
        //                              CodigoQRData (Pagina de consulta)
        //                              CodigoQRFilename (Pagina de registro y consulta)

        protected string sapid, tableroID, filial, area, descripcion, cantidad;
        protected Personas Persona;
        protected Usuarios Usuario;
        protected string codigoqrdata;

        //--------------------------------------------------NOTA--------------------------------------------
        //ESTE OBJETO ALMACENA EL CODIGO QR GENERADO CON LA LIBRERIA
        //QRCoder EN FORMATO DE byte[]
        protected byte[] codigoqrbyte;
        protected string codigoqrfilename;
        protected DateTime ultimafechaconsulta;
        protected List<ItemTablero> items;
        protected string tipodeconsulta;    // => VARIABLE UTILIZADA CUANDO LA CLASE ES LLAMADA DESDE LA CLASE "PaginaConsultaTablero.xaml.cs"
        protected int opcionconsultaid;
        protected string httperrorresponse;

        //---------------------------------------------NOTA-------------------------------------------------
        //Puesto que este proyecto es una aplicacion movil (es decir utiliza la version PCL de la libreria 
        //QRCoder) los objteos del tipo PngByteQRCode y BitMapByteQRCode son los unicos renders dispoibles
        protected PngByteQRCode codigoqr;

        //--------------------------------------------------------------------------------------------------
        //BANDERAS
        //SourceOfInvoke => TRUE  => Se llama a la clase desde "PaginaRegistroTablero.xaml.cs"
        //SourceOfInvoke => FALSE => Se llama a la clase desde "PaginaConsultaTablero.xaml.cs"
        protected bool SourceOfCalling;

        //==================================================================================================
        //==================================================================================================
        //SE DECLARAN LAS PROPIEDADES/ATRIBUTOS DE LA CLASE

        public string SapID
        {
            get { return sapid; }
            set
            {
                //TODOS LOS ID DEBEN IR EN MINUSCULA
                sapid = value;
                OnPropertyChanged();
                NotificacionCambio("SapID", SapID);
            }
        }

        public string TableroID
        {
            get { return tableroID; }
            set
            {
                //TODOS LOS ID DEBEN IR EN MINUSCULA
                tableroID = value;
                OnPropertyChanged();
                NotificacionCambio("TableroID", TableroID);
            }
        }

        public string Filial
        {
            get { return filial; }
            set
            {
                filial = value;
                OnPropertyChanged();

                NotificacionCambio("Filial", Filial);
            }
        }

        public string Area
        {
            get { return area; }
            set
            {
                area = value;
                OnPropertyChanged();

                NotificacionCambio("Area", Area);
            }
        }

        public DateTime FechaRegistro { get { return DateTime.Now; } }

        public string CodigoQRData
        {
            get
            {
                //SE OBTIENE UN STRING QUE FUNCIONA COMO REPRESENTACION
                //EQUIVALENTE UN ARREGLO DE 8 BITS (byte[])
                codigoqrdata = System.Convert.ToBase64String(codigoqrbyte);

                return codigoqrdata;
            }
        }

        public string CodigoQRFileName
        {
            get
            {
                if (SourceOfCalling)
                {
                    //SE EVALUA SI LA VARIABLE tableroID CONTIENE ALGUN DATO
                    if (!string.IsNullOrEmpty(tableroID))
                    {
                        //DE CONTENERLO SE RETORNA EL NOMBRE QUE LLEVARA LA IMAGEN CON EL CODIGO QR
                        codigoqrfilename = tableroID + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + ".png";
                        return codigoqrfilename;
                    }
                    else
                    {
                        //DE ESTAR VACIO NO SE DARA VALOR A LA PROPIEDAD CodigoQRFileName
                        return string.Empty;
                    }
                }
                else
                {
                    return codigoqrfilename;
                }
            }
        }

        public DateTime UltimaFechaConsulta { get { return ultimafechaconsulta; } }

        //--------------------------------------------------------------------------------------------------
        //PROPIEDADES USADAS PARA REPRESENTAR LOS ITEMS QUE FORMAN PARTE DEL TABLERO CONSULTADO

        public string Descripcion
        {
            get { return descripcion; }
            set
            {
                descripcion = value;
                OnPropertyChanged();

                NotificacionCambio("Descripcion", Descripcion);
            }
        }

        public string Cantidad
        {
            get { return cantidad; }
            set
            {
                cantidad = value;
                OnPropertyChanged();

                NotificacionCambio("Cantidad", Cantidad);
            }
        }

        public List<ItemTablero> Items
        {
            get { return items; }
        }

        //--------------------------------------------------------------------------------------------------
        //PROPIEDADES USADAS EXCLUSIVAMENTE EN LA PAGINA "PaginaConsultaTableros"
        public List<string> Opciones 
        {
            //LISTA DE OPCIONES DE BUSQUEDA CUANDO SE CONSULTA UN TABLERO MEDIANTE CONSULTA DE ID

            get { return new List<string>() { "Tablero ID", "SAP ID" }; } 
        }
        public string TipoDeConsulta 
        {
            //PROPIEDAD QUE RECIBE QUE TIPO DE CONSULTA SE VA A EFECTUAR:
            //"CONSULTA_ESCANER" => CONSULTA POR MEDIO DE ESCANEO DE CODIGO QR
            //"CONSULTA_POR_ID" => CONSULTA POR MEDIO DEL INGRESO DE UN ID (Tablero ID o SAP ID)

            get { return tipodeconsulta; } 
            set { tipodeconsulta = value; } 
        }
        public int OpcionConsultaID { set { opcionconsultaid = value; } }
        public string HttpErrorResponse { get { return httperrorresponse; } }

        //--------------------------------------------------------------------------------------------------
        public byte[] CodigoQRbyte 
        {
            //-------------------------------------------------NOTA---------------------------------------------
            //PROPIEDAD QUE CONTENDRAN LOS CODIGO QR IMAGEN QUE SE MOSTRARA EN PANTALLA Y LA QUE SE GUARDARA EN 
            //EL TELEFONO. SE UTILIZA EL RENDER DEL TIPO "PngByteQRCode", EL CUAL RETORNA UN ARRELO DE BYTES 
            //(byte[]) QUE CONTENDRA LA INFORMACION DE UNA IMAGEN DEL TIPO PNG.

            get { return codigoqrbyte; } 
        }

        //==================================================================================================
        //===============================PROPIEDADES INTERNAS DE LA PAGINA==================================
        //----------------------------Place Holder(PH) PaginaRegistroTablero--------------------------------
        public string TableroIDPH { get { return "Ingrese el codigo/ID del tablero"; } }
        public string SAPIDPH { get { return "Ingrese el codigo SAP asignado al tablero"; } }
        public string FilialPH { get { return "¿A que filial pertenece el tablero?"; } }
        public string AreaPH { get { return "¿A que area pertenece el tablero?"; } }
        public string DescripcionPH { get { return "Descripcion corta del item"; } }
        public string CantidadPH { get { return "Cantidad de items (unidades)"; } }
        public string ColumnaCant { get { return "Cantidad"; } }
        public string ColumnaDescripcion { get { return "Descripcion"; } }
        //----------------------------------PH PaginaConsultaTablero----------------------------------------
        public string ConsultaTableroIDPH { get { return "Ingresa el ID del tablero"; } }
        //----------------------------------PH BotonesPaginaRegistroTablero---------------------------------
        public string GenerarTableroPH { get { return "Generar"; } }
        public string AddItemPHP { get { return "Añadir Item"; } }
        public string GuardarTableroPH { get { return "Guardar Imagen"; } }
        public string RegistrarTableroPH { get { return "Registrar"; } }

        //----------------------------------PH BotonesPaginaConsultaTablero---------------------------------
        public string BotonScanPH { get { return "Escanear Codigo"; } }
        public string BotonConsultaIDPH { get { return "Busqueda por ID"; } }
        //----------------------------------PH Titulo de las Paginas----------------------------------------
        public string TituloRegistro { get { return "Pagina de Registro"; } }
        public string TituloConsulta { get { return "Pagina de Consulta"; } }
        //-----------------------------------------COLORES--------------------------------------------------
        //COLOR DEL FONDO
        public string BackGroundColor { get { return App.BackGroundColor; } }
        //COLOR DE LOS BOTONES
        public string ButtonColor { get { return App.ButtonColor; } }
        //-------------------------------------------NOTA---------------------------------------------------
        //EN ESTA SECCION ASIGNAREMOS EL TAMAÑO DE LA FUENTE PARA LAS ETIQUETAS, TITULOS, ENTRYS, ETC.
        public int LabelFontSize { get { return App.LabelFontSize; } }
        public int EntryFontSize { get { return App.EntryFontSize; } }
        public int HeaderFontSize { get { return App.HeaderFontSize; } }

        //==================================================================================================
        //==================================================================================================
        //--------------------------------------------EVENTOS-----------------------------------------------
        //NOTA => ESTE EVENTO SE ENCUENTRA CONECTADO O LIGADO CON EL METODO "OnPropertyChange"
        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        //==================================================================================================
        //==================================================================================================
        //--------------------------------------------METODOS-----------------------------------------------

        //==================================================================================================
        //==================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public RegistroTableroViewModel(Personas persona, Usuarios usuario, bool sourceofcalling)
        {
            //==========================================================================================
            //SE INICIALIZAN LAS VARIABLES LOCALES (Tablero e HistorialTablero)
            tableroID = filial = area = codigoqrdata = codigoqrfilename = tipodeconsulta = string.Empty;
            codigoqr = null;
            codigoqrbyte = null;
            ultimafechaconsulta = DateTime.Now;
            items = new List<ItemTablero>();

            //==========================================================================================
            //SE LLENAN LOS OBJETOS Persona e Usuario
            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);

            //==========================================================================================
            //SE INICIALIZAN LAS VARIABLES ASIGNADAS A LAS PROPIEDADES DE LA CLASE VIEWMODEL
            //showresultadoscan = false;
            SourceOfCalling = sourceofcalling;
        }

        //==================================================================================================
        //==================================================================================================
        //METODO QUE ACTUALIZA LA INFORMACION DE LA PROPIEDAD CADA QUE SE DECTECTA UN CAMBIO
        protected void OnPropertyChanged([CallerMemberName] string nombre = "")
        {
            PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(nombre));
        }

        //==================================================================================================
        //==================================================================================================
        //ADICION DE ITEMS A LA LISTA
        public List<ItemTablero> AddItem(string tableroID, string descripcion,
            int cantidad, List<ItemTablero> lista)
        {
            var item = ItemTablero.NewItem(tableroID, descripcion, cantidad);
            lista.Add(item);

            return lista;
        }

        //==================================================================================================
        //==================================================================================================
        //METODO DE IMPRESION POR CONSOLA
        protected void NotificacionCambio(string source, string value)
        {
            //------------------------------------------------------------------------------------------------
            /*
             * ESTE METODO FUE DISENADO PARA NOTIFICAR POR MENSAJE DE CONSOLA EL CAMBIO DE LAS PROPIEDADES
             * DE LA PAGINA QUE SE ENCUENTRAN EN CONSTANTE INTERACCION CON EL USUARIO EN LA PAGINA: "Pagina-
             * Registro". ES IMPORTANTE QUE ESTE METODO ES DE UTILIDAD PARA PRUEBAS DE DESARROLLADOR CUANDO
             * EL PROYECTO SE ENCUENTRE DEBUGIANDO
             */
            //------------------------------------------------------------------------------------------------

            Console.WriteLine("\n\n=============================================");
            Console.WriteLine("=============================================");
            Console.WriteLine("\nPROPIEDAD: " + source);
            Console.WriteLine("\nValor Actual: " + value);
            Console.WriteLine("=============================================");
            Console.WriteLine("=============================================\n\n");
        }

        //==================================================================================================
        //==================================================================================================
        //METODO DE IMPRESION POR CONSOLA
        
        protected void Mensaje(string mensaje)
        {
            //------------------------------------------------------------------------------------------------
            //NOTA: METODO QUE PUEDE SER LLAMADO DESDE CUALQUIER SECCION DE LA CLASE
            //------------------------------------------------------------------------------------------------

            Console.WriteLine("\n\n\n=============================================");
            Console.WriteLine("=============================================");
            Console.WriteLine("\n" + mensaje);
            Console.WriteLine("=============================================");
            Console.WriteLine("=============================================\n\n\n");
        }

        //==================================================================================================
        //==================================================================================================
        //METODOS LLAMADOS PARA EVALUAR LOS CAMPOS/ATRIBUTOS PARA UN NUEVO REGISTRO DE TABLERO
        private bool Evaluacion1()
        {
            if (!string.IsNullOrEmpty(tableroID) &&  //EL ID DEL TABLERO NO SE PUEDE ENCONTRAR VACIO
                !string.IsNullOrEmpty(filial) &&  //LA FILIAL NO SE PUEDE ENCONTRAR VACIA
                !string.IsNullOrEmpty(area))            //EL AREA NO SE PUEDE ENCONTRAR VACIA
                return true;
            else
                return false;
        }

        private bool Evaluacion2()
        {
            if (!Metodos.EspacioBlanco(tableroID) &&    //EL IF DEL TABLERO NO PUEDE CONTENER ESPACIOS EN BLANCO
                !Metodos.Caracteres(tableroID) &&       //EL ID DEL TABLERO NO PUEDE CONTENER CARACTERES ESPECIFICOS
                !Metodos.Caracteres(filial) &&          //LA FILIAL NO PUEDE CONTENER CARACTERES ESPECIFICOS
                !Metodos.Caracteres(area))              //EL AREA NO PUEDE CONTENER CARACTERES ESPECIFICOS
                return true;
            else
                return false;
        }

        private bool Evaluacion3(List<Tableros> registros)
        {
            if (!Metodos.MatchTableroID(registros, TableroID) &&      //SE EVALUA SI EL TableroID YA SE ENCONTRABA REGISTRADO PREVIAMENTE
               (!Metodos.MatchCodigoQRData(registros, CodigoQRData))) //SE EVALUA SI EL CODIGOQRDATA YA SE ENCONTRABA REGISTRADO PREVIAMENTE
                //SI NO EXISTE ALGUN REGISTRO CON EL MISMO ID O CON LA MISMA DATA DEL CODIGO QR SE PROCEDE CON EL REGISTRO (TRUE)
                return true;
            else
                //SI EXISTE ALGUN REGISTRO CON EL MISMO ID O CON LA MISMA DATA DEL CODIGO QR SE DETIENE EL REGISTRO (FALSE)
                return false;
        }

        //==================================================================================================
        //==================================================================================================
        //METODOS QUE RETORNAN UNA RESPUESTA CUANDO LAS FUNCIONES Evaluacion1, Evaluacion2 y Evaluacion3
        //RETORNAN UN VALOR NEGATIVO O FALSO
        private string RespuestaEvaluacion1()
        {
            //DE NO CUMPLIRSE ALGUNA DE LAS CONDICIONES MINIMAS SE ARROJA
            //UN MENSAJE DE NOTIFICACION AL USUARIO CUALES SON LOS ELEMENTOS
            //QUE NO CUMPLEN CON LAS CONDICIONES MINIMAS.

            string respuesta = string.Empty;

            //CASO DE NO TENER NINGUN VALOR INGRESADO
            if (string.IsNullOrEmpty(tableroID) &&
                string.IsNullOrEmpty(filial) &&
                string.IsNullOrEmpty(area))
                respuesta = "No debe existir ningun espacio en blanco";

            //CASO TableroID
            if (string.IsNullOrEmpty(tableroID))
                respuesta = "Para generar un codigo QR debe ingresar el ID del tablero a registrar";

            //CASO Filial
            if (string.IsNullOrEmpty(filial))
                respuesta = "Para generar un codigo QR debe ingresar a que filial pertenece el tablero";

            //CASO Area
            if (string.IsNullOrEmpty(area))
                respuesta = "Para generar un codigo QR debe ingresar a que area pertenece el tablero";

            return respuesta;
        }

        private string RespuestaEvaluacion2()
        {
            string respuesta = string.Empty;

            if (Metodos.EspacioBlanco(tableroID))
                respuesta = "El ID del tablero no puede contener espacios en blanco";

            if (Metodos.Caracteres(tableroID))
                respuesta = "El ID del tablero no puede contener los siguientes caracteres:\n " +
                        PaginaInformacionViewModel.CaracteresNoPermitidos();

            if (Metodos.Caracteres(filial))
                respuesta = "La filial a la cual pertenece el tablero no puede contener los siguientes caracteres:\n " +
                        PaginaInformacionViewModel.CaracteresNoPermitidos();

            if (Metodos.Caracteres(area))
                respuesta = "El area a la cual pertenece el tablero no puede contener los siguientes caracteres:\n " +
                        PaginaInformacionViewModel.CaracteresNoPermitidos();

            return respuesta;
        }

        private string RespuestaEvaluacion3(List<Tableros> registros)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE CONTENDRA
            string respuesta = string.Empty;

            //SE EVALUA SI YA EXISTE UN TABLERO QUE CONTENGA EL ID DEL TABLERO QUE SE INTENTA REGISTRAR
            if (Metodos.MatchTableroID(registros, TableroID))
                respuesta = "El ID del tablero ya se encuentra registrado.\nIntente con un ID distinto";

            //SE EVALUA SI YA EXISTE UN TABLERO QUE CONTENGA EL CODIGOQRDATA DEL TABLERO QUE SE INTENTA REGISTRAR
            if (Metodos.MatchCodigoQRData(registros, CodigoQRData))
                respuesta = "El codigo QR del tablero ya se encuentra registrado.\nIntente realizar el registro nuevamente";

            //SE RETORNA EL VALOR DE LA VARIABLE RESPUESTA
            return respuesta;
        }

        //==================================================================================================
        //==================================================================================================
        //METODO PARA LA CREACION DE CODIGOS QR (QRCoder)
        private PngByteQRCode GenerarCodigoQR(string codevalue)
        {
            //------------------------------NOTA-------------------------------
            //Puesto que este proyecto es una aplicacion movil (es decir utiliza
            //la version PCL de la libreria QRCoder) los objteos del tipo
            //PngByteQRCode y BitMapByteQRCode son los unicos renders dispoibles

            //======================================================
            //======================================================
            var mensaje = "El texto del codigo QR es: " + codevalue;
            Mensaje(mensaje);
            //======================================================
            //======================================================

            QRCodeGenerator generador = new QRCodeGenerator();
            QRCodeData datos = generador.CreateQrCode(codevalue, QRCodeGenerator.ECCLevel.M);

            return new PngByteQRCode(datos);
        }

        //==================================================================================================
        //==================================================================================================
        //FUNCION QUE GENERA EL CODIGO QR Y RETORNA UN MENSAJE CON LA RESPUESTA DE LA OPERACION
        public string GenerarCodigo()
        {
            //VARIABLE QUE RECIBE LA RESPUESTA DEL PROCESO SOLICITADO
            var respuesta = string.Empty;

            if (Evaluacion1())
            {
                //SE MANDA A GENERAR UN CODIGO QR
                codigoqr = GenerarCodigoQR(tableroID);      //METODO CON EL PLUGIN QRCoder

                //==============================================================================
                //==============================================================================
                //-------------------------------------NOTA-------------------------------------
                //SE OBTIENE UNA IMAGEN (DEL TIPO BITMAP)
                //DEL CODIGO QR UTILIZANDO LA FUNCION
                //GetGraphic del plugin QRCoder: GetGraphic(Parametro)
                //==============================================================================
                //Nombre del Parametro  |   Tipo    |   Descripcion
                //PixelPorModulo            int         El tamaño de cada modulo blanco y negro
                //==============================================================================
                //==============================================================================

                //SE RECIBE EL CODIGO QR EN FORMA DE BITMAP
                codigoqrbyte = codigoqr.GetGraphic(App.ByWSize);
            }
            else
            {
                //SE LLAMA AL METODO Respuesta PARA QUE NOS INDIQUE
                //QUE CAMPO O INFORMACION FALTA
                respuesta = RespuestaEvaluacion1();
            }

            return respuesta;
        }

        //==================================================================================================
        //==================================================================================================
        //METODO PARA ALMACENAR IMAGENES (CodigoQR) EN LA GALERIA DEL TELEFONO
        public void SaveImage()
        {
            //SE LLAMA LA FUNCION SavePicture
            //(CADA PLATAFORMA EJECUTARA METODOS DISTINTOS PARA GUARDAR LA IMAGEN)
            DependencyService.Get<IPicture>().SavePicture(CodigoQRFileName, CodigoQRbyte);
        }

        //==================================================================================================
        //==================================================================================================
        //METODO PARA REGISTRO DE TABLERO
        public async Task<string> RegistrarTablero(List<ItemTablero> items)
        {
            //SE INICIALIZA LA VARIABLE LOCAL QUE FUNCIONARA PARA ALMACENAR Y RETORNAR
            //LA RESPUESTA PARA EL USUARIO SOBRE EL PROCESO DE REGISTRO DE TABLERO EN LA PLATAFORMA
            string respuesta = string.Empty;

            //SE GENERA LA PRIMERA EVALUACION: NINGUN CAMPO DEBE ESTAR EN BLANCO O VACIO
            if (Evaluacion1())
            {
                //SE GENERA LA SEGUNDA EVALUACION: NO SE PERMITEN ESPACIOS EN BLANCO O CARACTERES ESPECIFICOS
                if (Evaluacion2())
                {
                    //SE GENERA LA APERTURA CON LA BASE DE DATOS
                    //respuesta = await RegistroTableroStandAlone(items); //=> APP FUNCIONANDO STAND ALONE
                    respuesta = await RegistroTableroHttpClient(items); //=> APP FUNCIONANDO POR CONSUMO DE SERVICIOS WEB
                }
                else
                {
                    //SE RECIBE LA RESPUESTA DE CUAL ATRIBUTO HAY QUE MODIFICAR
                    respuesta = RespuestaEvaluacion2();
                }
            }
            else
            {
                //SE RECIBE LA RESPUESTA DE CUAL ATRIBUTO HAY QUE MODIFICAR
                respuesta = RespuestaEvaluacion1();
            }

            return respuesta;
        }

        //==================================================================================================
        //==================================================================================================
        //METODO PARA LA BUSQUEDA DE UN TABLERO EN LA BASE DE DATOS
        public async Task<bool> BuscarTablero(string id)
        {
            //SE CREA LA BANDERA Y SE INICIALIZA EN FALSE (NO HAY MATCH DE TABLERO)
            var flag = false;

            //SE EVALUA SI LA PROPIEDAD NO ES NULA
            //NOTA: ESTA PROPIEDAD SOLO ES LLAMADA EN LA CLASE
            //"PaginaConsultaTablero.xaml.cs" EN EL METODO "Escanear"

            if (id != null)
            {
                //DE NO SER NULA SE PROCEDE A REALIZAR LA CONSULTA EN LA BASE DE DATOS
                //flag = await BuscarTableroStandAlone(id); // FUNCION STAND ALONE => NO REALIZA CONSUMO DE SERVICIOS WEB
                flag = await BusquedaTableroHttpClient(id); // FUNCION HTTP CLIENT => SE REALIZA CONSUMO DE SERVICIOS WEB
            }
            else
            {
                //SI LA PROPIEDAD ES NULA (NO SE OBTUVO EL PAYLOAD DEL CODIGO QR) SE RETORNA FALSE
                flag = false;
            }

            //SE RETORNA EL VALOR QUE CONTIENE LA VARIABLE "flag"
            //TRUE => SE CONSIGUIO UN TABLERO CON EL ID ENVIADO COMO PARAMETRO
            //FALSE => NO SE CONSIGUIO NINGUN TABLERO CON EL ID ENVIADO COMO PARAMETRO
            return await Task.FromResult(flag);
        }

        //==================================================================================================
        //==================================================================================================
        //FUNCIONES UTILIZADAS PARA REGISTRAR Y CONSULTAR TABLEROS CUANDO LA APLICACION SE ENCUENTRE
        //TRABAJANDO STAND ALONE
        private async Task<bool> BuscarTableroStandAlone(string id)
        {
            //SE CREA E INICIALIZA LA VARIABLE DE TIPO BOOL "flag", LA CUAL FUNCIONARA COMO VARIABLE DE RETORNO
            bool flag = false;

            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.FileName))
            {
                //EVALUAMOS QUE TIPO DE CONSULTA ES:
                if (tipodeconsulta == "CONSULTA_ESCANER")
                {
                    //SE CREA LAS TABLAS "Tableros", "ItemTablero", "HistorialTableros"(SI YA EXISTE NO SE CREA)
                    connection.CreateTable<Tableros>();
                    connection.CreateTable<ItemTablero>();
                    connection.CreateTable<HistorialTableros>();

                    //SE EVALUA SI EXISTE ALGUN REGISTRO
                    if (connection.Table<Tableros>().Any())
                    {
                        //SE REALIZA UNA CONSULTA DE CADA UNO DE LOS TABLEROS REGISTRADOS
                        foreach (Tableros tablero in connection.Table<Tableros>().ToList())
                        {
                            //SE COMPARA SI EL PAYLOAD OBTENIDO DEL ESCANEO
                            //ES IGUAL AL ID DEL TABLERO (tablero)
                            if (tablero.TableroID.ToLower() == id.ToLower())
                            {
                                //SE ACTIVA LA BANDERA
                                flag = true;

                                //SE LLENAN LAS VARIABLES LOCALES CON LA INFORMACION DEL TABLERO
                                await Task.Run(() =>
                                {
                                    tableroID = tablero.TableroID;
                                    sapid = tablero.SapID;
                                    filial = tablero.Filial;
                                    area = tablero.AreaFilial;
                                    //-------------------------------------------------------------------------------
                                    //SE OBTIENE LA INFORMACION DE LA IMAGEN (codigoqrdata) PARA LUEGO REALIZAR LA
                                    //CONVERSION DE STRING A BITMAP
                                    codigoqrdata = tablero.CodigoQRData;
                                    codigoqrbyte = System.Convert.FromBase64String(codigoqrdata);
                                    codigoqrfilename = tablero.CodigoQRFilename;
                                    //-------------------------------------------------------------------------------
                                });

                                //SE LLENA LA LISTA DE LOS ITEMS QUE FORMAN PARTE DEL TABLERO CONSULTADO
                                foreach (ItemTablero x in connection.Table<ItemTablero>().ToList())
                                {
                                    if (tableroID.ToLower() == x.TableroId.ToLower())
                                    {
                                        items.Add(x);
                                    }
                                }

                                //SE EVALUA SI EXISTE AL MENOS UN REGISTRO EN LA TABLA "HistorialTableros"
                                if (connection.Table<HistorialTableros>().Any())
                                {
                                    //SE CREA UN OBJETO DEL TIPO "HistorialTableros"
                                    var Historial = new HistorialTableros().NewRegistroHistorial(TableroID, Usuario.Cedula, DateTime.Now, TipoDeConsulta);

                                    //SE INSERTA EN LA TABLA EL NUEVO REGISTRO.
                                    connection.Insert(Historial);

                                    //CREAMOS UNA LISTA AUXILIAR
                                    List<HistorialTableros> HistorialTableroAux = new List<HistorialTableros>();

                                    //SE BUSCA EL ULTIMO REGISTRO EN LA TABLA "HistorialTableros"
                                    foreach (HistorialTableros registro in connection.Table<HistorialTableros>().ToList())
                                    {
                                        //SE COMPARA SI EL REGISTRO QUE SE ESTA EVALUANDO
                                        //EN EL MOMENTO POSEE EL ID DEL TABLERO QUE ACABA DE SER ESCANEADO
                                        if (registro.TableroID == TableroID)
                                        {
                                            //SE AÑADE ESTE TABLERO A LA LISTA "HistorialTableroAux"
                                            HistorialTableroAux.Add(registro);
                                        }
                                    }

                                    //SE CREA UNA VARIABLE CONTADOR
                                    int cont = 0;

                                    //SE VUELVE A RECORRER LA LISTA AUXILIAR
                                    foreach (HistorialTableros registro in HistorialTableroAux)
                                    {
                                        if (cont == (HistorialTableroAux.Count - 2))
                                        {
                                            //SE LE ASIGNA A LA VARIABLE "ultimafechaconsulta" LA FECHA DEL PENULTILMO REGISTRO DE LA LISTA AUXILIAR
                                            ultimafechaconsulta = registro.FechaDeConsulta;
                                            //SE CIERRA EL CICLO DE LECTURA
                                            break;
                                        }

                                        //SE PASA A LA SIGUIENTE POSICION
                                        cont++;
                                    }

                                    //SE CIERRA LA CONEXION CON LA BASE DE DATOS
                                    connection.Close();

                                    //SE DETIENE LA COMPARACION DE TABLEROS
                                    break;
                                }
                                //DE NO EXISTIR NINGUN REGISTRO DE TABLEROS SE REALIZA EL PRIMER REGISTRO E IMPRESION DE LA ULTIMA FECHA DE CONSULTA
                                else
                                {
                                    //DE NO EXISTIR NINGUN REGISTRO SE PROCEDE A CREAR EL PRIMERO
                                    var Historial = new HistorialTableros().NewRegistroHistorial(TableroID, Usuario.Cedula, DateTime.Now, TipoDeConsulta);

                                    //SE INSERTA EN LA TABLA EL NUEVO REGISTRO.
                                    connection.Insert(Historial);

                                    //SE COLOCA AUTOMATICAMENTE ESA FECHA COMO LA ULTIMA FECHA DE CONSULTA
                                    ultimafechaconsulta = Historial.FechaDeConsulta;

                                    //SE CIERRA LA CONEXION CON LA BASE DE DATOS
                                    connection.Close();

                                    //SE DETIENE LA COMPARACION DE TABLEROS
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //SI NO EXISTE NINGUN REGISTRO SE RETORNA FALSE
                        flag = false;
                    }
                }

                if (tipodeconsulta == "CONSULTA_POR_ID")
                {
                    switch (opcionconsultaid)
                    {
                        //CONSULTA POR TABLERO ID
                        case 0:
                            //----------------------------------------------------------------------------------------------
                            //SE CREA LAS TABLAS "Tableros", "ItemTablero", "HistorialTableros"(SI YA EXISTE NO SE CREA)
                            connection.CreateTable<Tableros>();
                            connection.CreateTable<ItemTablero>();
                            connection.CreateTable<HistorialTableros>();

                            //SE EVALUA SI EXISTE ALGUN REGISTRO
                            if (connection.Table<Tableros>().Any())
                            {
                                //SE REALIZA UNA CONSULTA DE CADA UNO DE LOS TABLEROS REGISTRADOS
                                foreach (Tableros tablero in connection.Table<Tableros>().ToList())
                                {
                                    //SE COMPARA SI EL TEXTO ENVIADO (TABLERO ID)
                                    //ES IGUAL AL ID DEL TABLERO (tablero)
                                    if (tablero.TableroID.ToLower() == id.ToLower())
                                    {
                                        //SE ACTIVA LA BANDERA
                                        flag = true;

                                        //SE LLENAN LAS VARIABLES LOCALES CON LA INFORMACION DEL TABLERO
                                        await Task.Run(() =>
                                        {
                                            //SE LLENAN LAS VARIABLES LOCALES
                                            tableroID = tablero.TableroID;
                                            sapid = tablero.SapID;
                                            filial = tablero.Filial;
                                            area = tablero.AreaFilial;
                                            //-------------------------------------------------------------------------------
                                            //SE OBTIENE LA INFORMACION DE LA IMAGEN (codigoqrdata) PARA LUEGO REALIZAR LA
                                            //CONVERSION DE STRING A BITMAP
                                            codigoqrdata = tablero.CodigoQRData;
                                            codigoqrbyte = System.Convert.FromBase64String(codigoqrdata);
                                            codigoqrfilename = tablero.CodigoQRFilename;
                                            //-------------------------------------------------------------------------------
                                        });

                                        //SE LLENA LA LISTA DE LOS ITEMS QUE FORMAN PARTE DEL TABLERO CONSULTADO
                                        foreach (ItemTablero x in connection.Table<ItemTablero>().ToList())
                                        {
                                            if (tableroID.ToLower() == x.TableroId.ToLower())
                                            {
                                                items.Add(x);
                                            }
                                        }

                                        //SE EVALUA SI EXISTE AL MENOS UN REGISTRO EN LA TABLA "HistorialTableros"
                                        if (connection.Table<HistorialTableros>().Any())
                                        {
                                            //SE CREA UN OBJETO DEL TIPO "HistorialTableros"
                                            var Historial = new HistorialTableros().NewRegistroHistorial(TableroID, Usuario.Cedula, DateTime.Now, TipoDeConsulta);

                                            //SE INSERTA EN LA TABLA EL NUEVO REGISTRO.
                                            connection.Insert(Historial);

                                            //CREAMOS UNA LISTA AUXILIAR
                                            List<HistorialTableros> HistorialTableroAux = new List<HistorialTableros>();

                                            //SE BUSCA EL ULTIMO REGISTRO EN LA TABLA "HistorialTableros"
                                            foreach (HistorialTableros registro in connection.Table<HistorialTableros>().ToList())
                                            {
                                                //SE COMPARA SI EL REGISTRO QUE SE ESTA EVALUANDO
                                                //EN EL MOMENTO POSEE EL ID DEL TABLERO QUE ACABA DE SER ESCANEADO
                                                if (registro.TableroID == TableroID)
                                                {
                                                    //SE AÑADE ESTE TABLERO A LA LISTA "HistorialTableroAux"
                                                    HistorialTableroAux.Add(registro);
                                                }
                                            }

                                            //SE CREA UNA VARIABLE CONTADOR
                                            int cont = 0;

                                            //SE VUELVE A RECORRER LA LISTA AUXILIAR
                                            foreach (HistorialTableros registro2 in HistorialTableroAux)
                                            {
                                                if (cont == (HistorialTableroAux.Count - 2))
                                                {
                                                    //SE LE ASIGNA A LA VARIABLE "ultimafechaconsulta" LA FECHA DEL PENULTILMO REGISTRO DE LA LISTA AUXILIAR
                                                    ultimafechaconsulta = registro2.FechaDeConsulta;
                                                    //SE CIERRA EL CICLO DE LECTURA
                                                    break;
                                                }

                                                //SE PASA A LA SIGUIENTE POSICION
                                                cont++;
                                            }

                                            //SE CIERRA LA CONEXION CON LA BASE DE DATOS
                                            connection.Close();

                                            //SE DETIENE LA COMPARACION DE TABLEROS
                                            break;
                                        }
                                        //DE NO EXISTIR NINGUN REGISTRO DE TABLEROS SE REALIZA EL PRIMER REGISTRO E IMPRESION DE LA ULTIMA FECHA DE CONSULTA
                                        else
                                        {
                                            //DE NO EXISTIR NINGUN REGISTRO SE PROCEDE A CREAR EL PRIMERO
                                            var Historial = new HistorialTableros().NewRegistroHistorial(TableroID, Usuario.Cedula, DateTime.Now, TipoDeConsulta);

                                            //SE INSERTA EN LA TABLA EL NUEVO REGISTRO.
                                            connection.Insert(Historial);

                                            //SE COLOCA AUTOMATICAMENTE ESA FECHA COMO LA ULTIMA FECHA DE CONSULTA
                                            ultimafechaconsulta = Historial.FechaDeConsulta;

                                            //SE CIERRA LA CONEXION CON LA BASE DE DATOS
                                            connection.Close();

                                            //SE DETIENE LA COMPARACION DE TABLEROS
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //SI NO EXISTE NINGUN REGISTRO SE RETORNA FALSE
                                flag = false;
                            }

                            break;
                        //----------------------------------------------------------------------------------------------
                        //CONSULTA POR SAP ID
                        case 1:
                            //----------------------------------------------------------------------------------------------
                            //SE CREA LA TABLA "Tableros" (SI YA EXISTE NO SE CREA)
                            connection.CreateTable<Tableros>();
                            connection.CreateTable<ItemTablero>();
                            connection.CreateTable<HistorialTableros>();

                            //SE EVALUA SI EXISTE ALGUN REGISTRO
                            if (connection.Table<Tableros>().Any())
                            {
                                //SE REALIZA UNA CONSULTA DE CADA UNO DE LOS TABLEROS REGISTRADOS
                                foreach (Tableros tablero in connection.Table<Tableros>().ToList())
                                {
                                    //SE COMPARA SI EL TEXTO ENVIADO (SAP ID)
                                    //ES IGUAL AL ID DEL TABLERO (tablero)
                                    if (tablero.SapID.ToLower() == id.ToLower())
                                    {
                                        //SE ACTIVA LA BANDERA
                                        flag = true;

                                        //SE LLENAN LAS VARIABLES LOCALES CON LA INFORMACION DEL TABLERO
                                        await Task.Run(() =>
                                        {
                                            //SE LLENAN LAS VARIABLES LOCALES
                                            tableroID = tablero.TableroID;
                                            sapid = tablero.SapID;
                                            filial = tablero.Filial;
                                            area = tablero.AreaFilial;
                                            //-------------------------------------------------------------------------------
                                            //SE OBTIENE LA INFORMACION DE LA IMAGEN (codigoqrdata) PARA LUEGO REALIZAR LA
                                            //CONVERSION DE STRING A BITMAP
                                            codigoqrdata = tablero.CodigoQRData;
                                            codigoqrbyte = System.Convert.FromBase64String(codigoqrdata);
                                            codigoqrfilename = tablero.CodigoQRFilename;
                                            //-------------------------------------------------------------------------------
                                        });

                                        //SE LLENA LA LISTA DE LOS ITEMS QUE FORMAN PARTE DEL TABLERO CONSULTADO
                                        foreach (ItemTablero x in connection.Table<ItemTablero>().ToList())
                                        {
                                            if (tableroID.ToLower() == x.TableroId.ToLower())
                                            {
                                                items.Add(x);
                                            }
                                        }

                                        //SE EVALUA SI EXISTE AL MENOS UN REGISTRO EN LA TABLA "HistorialTableros"
                                        if (connection.Table<HistorialTableros>().Any())
                                        {
                                            //SE CREA UN OBJETO DEL TIPO "HistorialTableros"
                                            var Historial = new HistorialTableros().NewRegistroHistorial(TableroID, Usuario.Cedula, DateTime.Now, TipoDeConsulta);

                                            //SE INSERTA EN LA TABLA EL NUEVO REGISTRO.
                                            connection.Insert(Historial);

                                            //CREAMOS UNA LISTA AUXILIAR
                                            List<HistorialTableros> HistorialTableroAux = new List<HistorialTableros>();

                                            //SE BUSCA EL ULTIMO REGISTRO EN LA TABLA "HistorialTableros"
                                            foreach (HistorialTableros registro in connection.Table<HistorialTableros>().ToList())
                                            {
                                                //SE COMPARA SI EL REGISTRO QUE SE ESTA EVALUANDO
                                                //EN EL MOMENTO POSEE EL ID DEL TABLERO QUE ACABA DE SER ESCANEADO
                                                if (registro.TableroID == TableroID)
                                                {
                                                    //SE AÑADE ESTE TABLERO A LA LISTA "HistorialTableroAux"
                                                    HistorialTableroAux.Add(registro);

                                                    //SE CREA UNA VARIABLE CONTADOR
                                                    int cont = 0;

                                                    //SE VUELVE A RECORRER LA LISTA AUXILIAR
                                                    foreach (HistorialTableros registro2 in HistorialTableroAux)
                                                    {
                                                        if (cont == (HistorialTableroAux.Count - 2))
                                                        {
                                                            //SE LE ASIGNA A LA VARIABLE "ultimafechaconsulta" LA FECHA DEL PENULTILMO REGISTRO DE LA LISTA AUXILIAR
                                                            ultimafechaconsulta = registro2.FechaDeConsulta;
                                                            //SE CIERRA EL CICLO DE LECTURA
                                                            break;
                                                        }

                                                        //SE PASA A LA SIGUIENTE POSICION
                                                        cont++;
                                                    }

                                                    //SE CIERRA LA CONEXION CON LA BASE DE DATOS
                                                    connection.Close();

                                                    //SE DETIENE LA COMPARACION DE TABLEROS
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //DE NO EXISTIR NINGUN REGISTRO SE PROCEDE A CREAR EL PRIMERO
                                            var Historial = new HistorialTableros().NewRegistroHistorial(TableroID, Usuario.Cedula, DateTime.Now, TipoDeConsulta);

                                            //SE INSERTA EN LA TABLA EL NUEVO REGISTRO.
                                            connection.Insert(Historial);

                                            //SE COLOCA AUTOMATICAMENTE ESA FECHA COMO LA ULTIMA FECHA DE CONSULTA
                                            ultimafechaconsulta = Historial.FechaDeConsulta;

                                            //SE CIERRA LA CONEXION CON LA BASE DE DATOS
                                            connection.Close();

                                            //SE DETIENE LA COMPARACION DE TABLEROS
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //SI NO EXISTE NINGUN REGISTRO SE RETORNA FALSE
                                flag = false;
                            }
                            break;
                            //----------------------------------------------------------------------------------------------
                    }
                }
            }

            return await Task.FromResult(flag);
        }
        private async Task<string> RegistroTableroStandAlone(List<ItemTablero> items)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE RETORNARA LA FUNCION
            string respuesta = string.Empty;

            //SE GENERA LA APERTURA CON LA BASE DE DATOS
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.FileName))
            {
                //SE CREA LA TABLA TABLEROS (DE EXISTIR YA NO SE CREA)
                connection.CreateTable<Tableros>();
                connection.CreateTable<ItemTablero>();

                //SE VERIFICA SI EXISTE AL MENOS ALGUN REGISTRO DENTRO DE LA TABLA
                if (connection.Table<Tableros>().Any())
                {
                    //SE VERIFICA QUE NO EXISTA UN REGISTRO PREVIO DE DICHO TABLERO
                    if (Evaluacion3(connection.Table<Tableros>().ToList()))
                    {
                        //SE INSERTA EL NUEVO REGISTRO
                        connection.Insert(Tableros.NuevoTablero(TableroID, SapID, Filial, Area, FechaRegistro,
                            CodigoQRData, CodigoQRFileName, Usuario.Cedula));

                        //SE RECORREN TODOS LOS ELEMENTOS DE LA LISTA "items"
                        foreach (ItemTablero x in items)
                        {
                            //SE INSERTAN TODOS LOS ELEMENTOS DENTRO DE LA TABLA
                            connection.Insert(x);
                        }

                        //SE CIERRA LA CONEXION CON LA BASE DE DATOS
                        connection.Close();

                        //SE GENERA UN MENSAJE INFORMATIVO
                        Toast.MakeText(Android.App.Application.Context, "Tablero registrado satisfactoriamente", ToastLength.Long).Show();
                    }
                    else
                    {
                        //SE DA RETORNO AL MENSAJE QUE INDICARA AL USUARIO QUE NO SE LOGRO REGISTRAR EL NUEVO TABLERO
                        respuesta = RespuestaEvaluacion3(connection.Table<Tableros>().ToList());
                    }
                }
                //SI NO EXISTE NINGUN REGISTRO PROCEDEMOS A CREAR EL PRIMERO (ESTE CODIGO SOLO SE EJECUTARA UNA VEZ)
                else
                {
                    //SE INSERTA LA INFORMACION DEL TABLERO DENTRO DE LA TABLA "Tableros"
                    connection.Insert(Tableros.NuevoTablero(TableroID, SapID, Filial, Area, FechaRegistro,
                        CodigoQRData, CodigoQRFileName, Usuario.Cedula));

                    //SE RECORREN TODOS LOS ELEMENTOS DE LA LISTA "items"
                    foreach (ItemTablero x in items)
                    {
                        //SE INSERTAN TODOS LOS ELEMENTOS DENTRO DE LA TABLA
                        connection.Insert(x);
                    }

                    int cont = connection.Table<ItemTablero>().Count();

                    //SE CIERRA LA CONEXION CON LA BASE DE DATOS
                    connection.Close();

                    //SE GENERA UN MENSAJE INFORMATIVO
                    Toast.MakeText(Android.App.Application.Context, "Tablero registrado satisfactoriamente", ToastLength.Long).Show();
                }
            }

            return await Task.FromResult(respuesta);
        }

        //==================================================================================================
        //==================================================================================================
        //FUNCIONES UTILIZADAS PARA REGISTRAR Y CONSULTAR TABLEROS CUANDO LA APLICACION SE ENCUENTRE 
        //CONSUMIENTO SERVICIOS WEB
        private async Task<string> RegistroTableroHttpClient(List<ItemTablero> items)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE RETORNARA LA FUNCION
            string respuesta = string.Empty;

            //SE CREA E INICIALIZA LA VARIABLE QUE RETENDRA EL URL PARA REALIZAR LA SOLICITUD HTTP
            string url = App.BaseUrl + "/registrotableros";

            //SE CREA E INICIALIZA LA VARIABLE QUE FUNCIONARA COMO MODELO PARA EL OBJETO JSON ENVIADO
            var model = new MTTO_App.Tablas.RegistroTablero()
            {
                //INFORMACION DEL TABLERO 
                tableroInfo = new Tableros()
                {
                    TableroID = TableroID,
                    SapID = SapID,
                    IDCreador = Usuario.Cedula,
                    Filial = Filial,
                    AreaFilial = Area,
                    FechaRegistro = FechaRegistro,
                    CodigoQRData = CodigoQRData,
                    CodigoQRFilename = CodigoQRFileName,
                },

                //LISTA DE ITEMS QUE POSEE EL TABLERO
                itemsTablero = items
            };

            //SE CREA E INICIALIZA LA VARIABLE QUE VERIFICARA EL ESTADO DE CONEXION A INTERNET
            var current = Xamarin.Essentials.Connectivity.NetworkAccess;
            //SE VERIFICA SI EL DISPOSITIVO SE ENCUENTRA CONECTADO A INTERNET
            if (current == Xamarin.Essentials.NetworkAccess.Internet)
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
                        //SE AÑADE EL OBJETO JSON RECIEN CREADO COMO CONTENIDO BODY DEL NUEVO REQUEST
                        HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        //SE HACE LA CONFIGURACION DE LOS HEADERS DEL REQUEST
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //SE REALIZA LA SOLICITUD HTTP
                        HttpResponseMessage response = await client.PostAsync(url, httpContent);
                        //SE RETORNA EL MENSAJE OBTENIDO POR 
                        respuesta = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
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
                    respuesta = "Problemas de conexion con el servidor";
                }
            }
            else
            //NO HAY ACCESO A INTERNET
            {
                respuesta = "No hay conexion a internet";
            }

            return await Task.FromResult(respuesta);
        }
        private async Task<bool> BusquedaTableroHttpClient(string id)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE SERA RETORNADA POR LA FUNCION
            bool flag = false;

            //SE CREA E INICIALIZA LA VARIABLE QUE RETENDRA EL URL PARA REALIZAR LA SOLICITUD HTTP
            string url = App.BaseUrl + "/consultatableros";

            //SE CREA E INICIALIZA EL OBJETO QUE SERVIRA COMO MODELO PARA EL OBJETO JSON ENVIADO EN LA SOLICITUD HTTP
            RequestConsultaTablero model = null;

            //SE CREA E INICIALIZA LA VARIABLE QUE RECIBIRA LA RESPUESTA DE LA SOLICITUD HTTP
            HttpResponseMessage response = new HttpResponseMessage();

            //EVALUAMOS QUE TIPO DE CONSULTA ES:
            if (tipodeconsulta == "CONSULTA_ESCANER")
            {
                //SE CREA E INICIALIZA EL OBJETO QUE SERVIRA COMO MODELO PARA EL OBJETO JSON ENVIADO EN LA SOLICITUD HTTP
                model = new RequestConsultaTablero()
                {
                    UserId = Persona.Cedula,
                    TableroId = id,
                    SapId = null,
                };

                //SE TERMINA DE ESCRIBIR LA DIRECCION A LA CUAL SE REALIZARA LA SOLICITUD HTTP
                url = url + "/tableroid"; 
            }

            if (tipodeconsulta == "CONSULTA_POR_ID")
            {

                switch (opcionconsultaid)
                {
                    //CONSUTA POR ID => PARAMETRO ENVIADO -> TableroId
                    case 0:
                        //SE CREA E INICIALIZA EL OBJETO QUE SERVIRA COMO MODELO PARA EL OBJETO JSON ENVIADO EN LA SOLICITUD HTTP
                        model = new RequestConsultaTablero()
                        {
                            UserId = Persona.Cedula,
                            TableroId = id,
                            SapId = null,
                        };
                        //SE TERMINA DE ESCRIBIR LA DIRECCION A LA CUAL SE REALIZARA LA SOLICITUD HTTP
                        url = url + "/tableroid";
                        break;
                    //CONSUTA POR ID => PARAMETRO ENVIADO -> SapId
                    case 1:
                        //SE CREA E INICIALIZA EL OBJETO QUE SERVIRA COMO MODELO PARA EL OBJETO JSON ENVIADO EN LA SOLICITUD HTTP
                        model = new RequestConsultaTablero()
                        {
                            UserId = Persona.Cedula,
                            TableroId = null,
                            SapId = id,
                        };
                        //SE TERMINA DE ESCRIBIR LA DIRECCION A LA CUAL SE REALIZARA LA SOLICITUD HTTP
                        url = url + "/sapid";
                        break;
                }

            }

            //SE CREA E INICIALIZA LA VARIABLE QUE VERIFICARA EL ESTADO DE CONEXION A INTERNET
            var current = Xamarin.Essentials.Connectivity.NetworkAccess;
            //SE VERIFICA SI EL DISPOSITIVO SE ENCUENTRA CONECTADO A INTERNET
            if (current == Xamarin.Essentials.NetworkAccess.Internet)
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
                        //SE AÑADE EL OBJETO JSON RECIEN CREADO COMO CONTENIDO BODY DEL NUEVO REQUEST
                        HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        //HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        //HttpRequestMessage request = new HttpRequestMessage() { Method = HttpMethod.Get, Content = new StringContent(json, Encoding.UTF8, "application/json"), RequestUri = new Uri(url)};
                        //SE HACE LA CONFIGURACION DE LOS HEADERS DEL REQUEST
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //SE REALIZA LA SOLICITUD HTTP
                        response = await client.PostAsync(url, httpContent);
                        //response = await client.SendAsync(request);

                        //SE EVALUA SI EL CODIGO DE ESTADO RETORNADO ES: 200 OK
                        if (response.IsSuccessStatusCode)
                        {
                            //EL CODIGO DE ESTATUS OBTENIDO ES EL 200 OK
                            //SE ACTIVA LA BANDERA
                            flag = true;

                            //SE DESERIALIZA EL OBJETO JSON CONTENIDO EN LA RESPUESTA HTTP
                            var tablero = JsonConvert.DeserializeObject<RegistroTablero>(await response.Content.ReadAsStringAsync());

                            //SE LLENAN LAS VARIABLES LOCALES CON LA INFORMACION DEL TABLERO
                            await Task.Run(() =>
                            {
                                //SE LLENAN LAS VARIABLES LOCALES
                                tableroID = tablero.tableroInfo.TableroID;
                                sapid = tablero.tableroInfo.SapID;
                                filial = tablero.tableroInfo.Filial;
                                area = tablero.tableroInfo.AreaFilial;
                                //-------------------------------------------------------------------------------
                                //SE OBTIENE LA INFORMACION DE LA IMAGEN (codigoqrdata) PARA LUEGO REALIZAR LA
                                //CONVERSION DE STRING A BITMAP
                                codigoqrdata = tablero.tableroInfo.CodigoQRData;
                                codigoqrbyte = System.Convert.FromBase64String(tablero.tableroInfo.CodigoQRData);
                                codigoqrfilename = tablero.tableroInfo.CodigoQRFilename;
                                //-------------------------------------------------------------------------------
                                //SE OBTIENE LA LISTA DE ITEMS QUE FORMAN PARTE DEL TABLERO
                                items = tablero.itemsTablero;
                                //-------------------------------------------------------------------------------
                            });
                        }
                        else
                        {
                            httperrorresponse = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                        }


                    }
                }
                catch (Exception ex) when (ex is HttpRequestException ||
                                           ex is Javax.Net.Ssl.SSLException ||
                                           ex is Javax.Net.Ssl.SSLHandshakeException ||
                                           ex is System.Threading.Tasks.TaskCanceledException)
                {
                    httperrorresponse = "Error de conexion, intente nuevamente";
                }
            }

                return await Task.FromResult(flag);
        }

    }
}