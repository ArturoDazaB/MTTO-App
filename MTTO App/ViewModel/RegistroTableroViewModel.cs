using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

using QRCoder;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;

namespace MTTO_App.ViewModel
{
    class RegistroTableroViewModel : INotifyPropertyChanging
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

        protected string sapid, tableroID, filial, area, descripcion;
        protected Personas Persona;
        protected Usuarios Usuario;
        protected string codigoqrdata;
        protected string codigoqrfilename;
        protected DateTime ultimafechaconsulta;

        //VARIABLE UTILIZADA CUANDO LA CLASE ES LLAMADA DESDE LA CLASE "PaginaConsultaTablero.xaml.cs"
        protected string tipodeconsulta;

        //--------------------------------------------------------------------------------------------------
        protected string resultadoscan;

        //--------------------------------------------------------------------------------------------------
        protected bool showresultadoscan;

        //--------------------------------------------------------------------------------------------------
        //                                               NOTA
        //ESTE OBJETO ALMACENA EL CODIGO QR GENERADO CON LA LIBRERIA 
        //QRCoder EN FORMATO DE byte[]
        protected byte[] codigoqrbyte;

        //--------------------------------------------------------------------------------------------------
        //BANDERAS
        //SourceOfInvoke => TRUE  => Se llama a la clase desde "PaginaRegistroTablero.xaml.cs"
        //SourceOfInvoke => FALSE => Se llama a la clase desde "PaginaConsultaTablero.xaml.cs"
        protected bool SourceOfCalling;

        //==================================================================================================
        //==================================================================================================
        //OBJETOS DE LA CLASE
        //---------------------------------------------NOTA-------------------------------------------------
        //Puesto que este proyecto es una aplicacion movil (es decir utiliza
        //la version PCL de la libreria QRCoder) los objteos del tipo
        //PngByteQRCode y BitMapByteQRCode son los unicos renders dispoibles
        protected PngByteQRCode codigoqr;

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

            //==========================================================================================
            //SE LLENAN LOS OBJETOS Persona e Usuario
            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);

            //==========================================================================================
            //SE INICIALIZAN LAS VARIABLES ASIGNADAS A LAS PROPIEDADES DE LA CLASE VIEWMODEL
            resultadoscan = string.Empty;
            showresultadoscan = false;
            SourceOfCalling = sourceofcalling;
        }

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
            get  { return tableroID; }
            set 
            {   
                //TODOS LOS ID DEBEN IR EN MINUSCULA
                tableroID = value;
                OnPropertyChanged();
                NotificacionCambio("TableroID", TableroID);
            }
        }

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

        public string ResultadoScan 
        { 
            get 
            {
                return resultadoscan;
            }
            set
            {
                resultadoscan = value;
                showresultadoscan = BuscarTablero(tipodeconsulta);
            }
        }

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
                if(SourceOfCalling)
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

        //--------------------------------------------------------------------------------------------------
        public string TipoDeConsulta { set { tipodeconsulta = value; } }

        public DateTime UltimaFechaConsulta { get { return ultimafechaconsulta; } }
        //--------------------------------------------------------------------------------------------------
        //==================================================================================================
        //==================================================================================================
        //SE DECLARAN LAS PROPIEDADES/ATRIBUTOS LOGICAS
        public bool ShowResultadoScan
        {
            get
            {
                return showresultadoscan;
            }
            set
            {
                showresultadoscan = value;
            }
        }

        //==================================================================================================
        //==================================================================================================
        //OBJETOS QUE CONTENDRAN LOS CODIGO QR

        //-------------------------------------------------NOTA---------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //SE UTILIZA EL RENDER DEL TIPO "PngByteQRCode", EL CUAL RETORNA UN ARRELO DE BYTES (byte[])
        //QUE CONTENDRA LA INFORMACION DE UNA IMAGEN DEL TIPO PNG.
        public PngByteQRCode CodigoQR
        {
            get { return codigoqr; }
        }

        //IMAGEN QUE SE MOSTRARA EN PANTALLA Y LA QUE SE GUARDARA EN EL TELEFONO
        public byte[] CodigoQRbyte { get { return codigoqrbyte; } }
        //==================================================================================================
        //==================================================================================================
        //PROPIEDADES INTERNAS DE LA PAGINA
        //--------------------------------------------NOTA--------------------------------------------------
        //EN ESTA SECCION PODREMOS CAMBIAR EL TEXTO DE ETIQUETAS, ENTRYS Y OTROS

        //----------------------------------PH PaginaRegistroTablero----------------------------------------
        public string TableroIDPH { get { return "Ingrese el codigo/ID del tablero"; } }
        public string SAPIDPH { get { return "Ingrese el codigo SAP asignado al tablero"; } }
        public string FilialPH { get { return "¿A que filial pertenece el tablero?"; } }
        public string AreaPH { get { return "¿A que area pertenece el tablero?"; } }

        //----------------------------------PH PaginaConsultaTablero----------------------------------------
        public string ConsultaTableroIDPH { get { return "Ingresa el ID del tablero"; } }

        //----------------------------------PH BotonesPaginaRegistroTablero---------------------------------
        public string GenerarTableroPH { get { return "Generar"; } }
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
        public string BackGroundColor { get { return App.BackGroundColor;} }

        //COLOR DE LOS BOTONES
        public string ButtonColor { get { return App.ButtonColor; } }

        //-------------------------------------------NOTA---------------------------------------------------
        //EN ESTA SECCION ASIGNAREMOS EL TAMAÑO DE LA FUENTE PARA LAS ETIQUETAS, TITULOS, ENTRYS, ETC.
        public int LabelFontSize { get { return App.LabelFontSize; } }
        public int EntryFontSize { get { return App.EntryFontSize; } }
        public int HeaderFontSize { get { return App.HeaderFontSize; } }

        //==================================================================================================
        //==================================================================================================
        //--------------------------------------------METODOS-----------------------------------------------
        //==================================================================================================
        //==================================================================================================

        //==================================================================================================
        //==================================================================================================
        //ACTUALIZA LA INFORMACION DE LA PROPIEDAD CADA QUE SE DECTECTA UN CAMBIO

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
        protected void OnPropertyChanged([CallerMemberName] string nombre = "")
        {
            PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(nombre));
        }

        //==================================================================================================
        //==================================================================================================
        //METODO DE IMPRESION POR CONSOLA
        //NOTA: METODO USADO PARA LAS PROPIEDADES DE LA CLASE QUE SE ENCUENTRAN EN CONSTANTE MODIFICACION
        //COMO LO SON => TableroID, Filial y Area
        protected void NotificacionCambio(string source, string value)
        {
            Console.WriteLine("\n\n=============================================");
            Console.WriteLine("=============================================");
            Console.WriteLine("\nPROPIEDAD: " + source);
            Console.WriteLine("\nValor Actual: " + value);
            Console.WriteLine("=============================================");
            Console.WriteLine("=============================================\n\n");
        }

        //METODO DE IMPRESION POR CONSOLA
        //NOTA: METODO QUE PUEDE SER LLAMADO DESDE CUALQUIER SECCION DE LA CLASE
        protected void Mensaje(string mensaje)
        {
            Console.WriteLine("\n\n\n=============================================");
            Console.WriteLine("=============================================");
            Console.WriteLine("\n" + mensaje);
            Console.WriteLine("=============================================");
            Console.WriteLine("=============================================\n\n\n");
        }

        //==================================================================================================
        //==================================================================================================
        //METODOS LLAMADOS PARA EVALUAR LOS CAMPOS/ATRIBUTOS PARA UN NUEVO REGISTRO DE TABLERO
        protected bool Evaluacion1()
        {
            if (!string.IsNullOrEmpty(tableroID)    &&  //EL ID DEL TABLERO NO SE PUEDE ENCONTRAR VACIO
                !string.IsNullOrEmpty(filial)       &&  //LA FILIAL NO SE PUEDE ENCONTRAR VACIA
                !string.IsNullOrEmpty(area))            //EL AREA NO SE PUEDE ENCONTRAR VACIA
                return true;
            else
                return false;
        }
        protected bool Evaluacion2()
        {
            if (!Metodos.EspacioBlanco(tableroID) &&    //EL IF DEL TABLERO NO PUEDE CONTENER ESPACIOS EN BLANCO
                !Metodos.Caracteres(tableroID) &&       //EL ID DEL TABLERO NO PUEDE CONTENER CARACTERES ESPECIFICOS
                !Metodos.Caracteres(filial) &&          //LA FILIAL NO PUEDE CONTENER CARACTERES ESPECIFICOS
                !Metodos.Caracteres(area))              //EL AREA NO PUEDE CONTENER CARACTERES ESPECIFICOS
                return true;
            else
                return false;
                
        }
        protected bool Evaluacion3(List<Tableros> registros)
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
        protected string RespuestaEvaluacion1()
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
        protected string RespuestaEvaluacion2()
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
        protected string RespuestaEvaluacion3(List<Tableros> registros)
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
        //------------------------------NOTA-------------------------------
        //Puesto que este proyecto es una aplicacion movil (es decir utiliza
        //la version PCL de la libreria QRCoder) los objteos del tipo
        //PngByteQRCode y BitMapByteQRCode son los unicos renders dispoibles
        protected PngByteQRCode GenerarCodigoQR(string codevalue)
        {
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

            if(Evaluacion1())
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
        public string RegistrarTablero()
        {
            //SE INICIALIZA LA VARIABLE LOCAL QUE FUNCIONARA PARA ALMACENAR Y RETORNAR
            //LA RESPUESTA PARA EL USUARIO SOBRE EL PROCESO DE REGISTRO DE TABLERO EN LA PLATAFORMA
            string respuesta = string.Empty;

            //SE GENERA LA PRIMERA EVALUACION: NINGUN CAMPO DEBE ESTAR EN BLANCO O VACIO
            if (Evaluacion1())
            {
                //SE GENERA LA SEGUNDA EVALUACION: NO SE PERMITEN ESPACIOS EN BLANCO O CARACTERES ESPECIFICOS
                if(Evaluacion2())
                {
                    //SE GENERA LA APERTURA CON LA BASE DE DATOS
                    using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.FileName))
                    {
                        //SE CREA LA TABLA TABLEROS (DE EXISTIR YA NO SE CREA)
                        connection.CreateTable<Tableros>();

                        //SE VERIFICA SI EXISTE AL MENOS ALGUN REGISTRO DENTRO DE LA TABLA
                        if(connection.Table<Tableros>().Any())
                        {
                            //SE VERIFICA QUE NO EXISTA UN REGISTRO PREVIO DE DICHO TABLERO
                            if (Evaluacion3(connection.Table<Tableros>().ToList())) 
                            {
                                //SE INSERTA EL NUEVO REGISTRO 
                                connection.Insert(Tableros.NuevoTablero(TableroID, SapID, Descripcion, Filial, Area, FechaRegistro,
                                    CodigoQRData, CodigoQRFileName, Usuario.Cedula));

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
                            connection.Insert(Tableros.NuevoTablero(TableroID, SapID, Descripcion, Filial, Area, FechaRegistro,
                                CodigoQRData, CodigoQRFileName, Usuario.Cedula));

                            //SE CIERRA LA CONEXION CON LA BASE DE DATOS
                            connection.Close();

                            //SE GENERA UN MENSAJE INFORMATIVO 
                            Toast.MakeText(Android.App.Application.Context, "Tablero registrado satisfactoriamente", ToastLength.Long).Show();
                        }
                    }
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
        private bool BuscarTablero(string TipoDeConsulta)
        {
            //SE CREA LA BANDERA Y SE INICIALIZA EN FALSE (NO HAY MATCH DE TABLERO)
            var flag = false;

            //SE EVALUA SI LA PROPIEDAD NO ES NULA
            //NOTA: ESTA PROPIEDAD SOLO ES LLAMADA EN LA CLASE
            //"PaginaConsultaTablero.xaml.cs" EN EL METODO "Escanear"

            if (resultadoscan != null)
            {
                //DE NO SER NULA SE PROCEDE A APERTURAR LA BASE DE DATOS PARA BUSCAR EL ID
                //OBTENIDO POR EL ESCANEO EN LA BASE DE DATOS
                using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.FileName))
                {
                    //SE CREA LA TABLA "Tableros" (SI YA EXISTE NO SE CREA)
                    connection.CreateTable<Tableros>();

                    //SE EVALUA SI EXISTE ALGUN REGISTRO 
                    if(connection.Table<Tableros>().Any())
                    {
                        //SE REALIZA UNA CONSULTA DE CADA UNO DE LOS TABLEROS REGISTRADOS
                        foreach (Tableros tablero in connection.Table<Tableros>().ToList())
                        {
                            //SE COMPARA SI EL PAYLOAD OBTENIDO DEL ESCANEO 
                            //ES IGUAL AL ID DEL TABLERO (tablero)
                            if (tablero.TableroID.ToLower() == resultadoscan.ToLower())
                            {
                                //SE ACTIVA LA BANDERA
                                flag = true;

                                //SE LLENAN LAS VARIABLES LOCALES
                                tableroID = tablero.TableroID;
                                filial = tablero.Filial;
                                area = tablero.AreaFilial;
                                //-------------------------------------------------------------------------------
                                //SE OBTIENE LA INFORMACION DE LA IMAGEN (codigoqrdata) PARA LUEGO REALIZAR LA 
                                //CONVERSION DE STRING A BITMAP
                                codigoqrdata = tablero.CodigoQRData;
                                codigoqrbyte = System.Convert.FromBase64String(codigoqrdata);
                                codigoqrfilename = tablero.CodigoQRFilename;
                                //-------------------------------------------------------------------------------

                                //SE CREA LA TABLA "HistorialTableros" (SI YA EXISTE SOLO LA SELECCIONA)
                                connection.CreateTable<HistorialTableros>();

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
            }
            else
            {
                //SI LA PROPIEDAD ES NULA (NO SE OBTUVO EL PAYLOAD DEL CODIGO QR) SE RETORNA FALSE
                flag = false;
            }

            //SE RETORNA EL VALOR QUE CONTIENE LA VARIABLE "flag"
            //TRUE => SE CONSIGUIO UN TABLERO CON EL ID ESCANEADO
            //FALSE => NO SE CONSIGUIO NINGUN TABLERO CON EL ID ESCANEADO
            return flag;
        }
    }
}
