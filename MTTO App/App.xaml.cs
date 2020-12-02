using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MTTO_App
{
    public partial class App : Application
    {
        //VARIABLE QUE ALMACENARA EL NOMBRE DEL ARCHIVO QUE TENDRA LA BASE DE DATOS 
        //LOCAL QUE MANEJARA LA APLICACION CUANDO SE ENCUENTRE FUNCIONANDO STAND ALONE
        public static string FileName;

        //SI SE REALIZO ALGUN CAMBIO DE ALGUN ATRIBUTO DEL USUARIO QUE SE ENCUENTRE
        //LOGGEADO EN ESE MOMENTO EN LA APLICACION ESTA BANDERA (FLAG) SE DISPARARA (TRUE)
        public static bool ConfigChangedFlag;

        //SI EL USUARIO QUE REALIZO ALGUNA MODIFICACION SOBRE ALGUN REGISTRO DE PERSONA Y DE USUARIO
        //SE ACTIVARA ESTA BANDERA. ESTO CON LA FUNCION DE NOTIFICAR CUANDO EL USUARIO MODIFICADOR
        //ES EL USUARIO ADMINISTRATOR (EN OTRAS PALABRAS UN USUARIO DE ALTO NIVEL
        public static bool ConfigChangedAdminFlag;

        //SI SE REALIZA ALGUN NUEVO REGISTRO DE USUARIO SE ACTIVA LA BANDERA
        public static bool RegistroFlag;

        //===============================================================================================
        //===============================================================================================
        //CONSTANTES QUE SERAN LLAMADOS EN LOS DISTINTOS "VIEWMODEL" EMPLEADOS PARA CADA PAGINA

        //DIMENSIONES DEL CODIGO QR
        public const int ByWSize = 22;

        //TAMAÑO DE LAS ETIQUETAS
        public const int LabelFontSize = 12;

        //TAMAÑO DE LAS ENTRADAS
        public const int EntryFontSize = 15;

        //TAMAÑO DE LOS TITULOS DE SECCION
        public const int HeaderFontSize = 25;

        //TAMAÑO DEL NOMBRE DE USUARIO EN LA CLASE "MasterPage.xaml.cs"
        public const int SmallHeaderFontSize = 20;

        //COLOR DEL FONDO
        public const string BackGroundColor = "#fcf3e3";

        //COLOR DEL FONTO (PAGINAS DE INFORMACION)
        public const string BackGroundColorPopUp = "#FFFDE7";

        //COLOR DE LOS BOTONES
        public const string ButtonColor = "#E53935";

        //-----------------------------------------------------------------------------------------------

        //===============================================================================================
        //===============================================================================================
        //PROPIEDADES DE LA APLICACION
        public static MasterDetailPage MasterDetail { get; set; }

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE
        public App()
        {
            InitializeComponent();
            //SE CONFIGURA A LA PAGINA "PaginaPrincipal" COMO LA PAGINA INICIAL QUE
            //APARECERA AL MOMENTO DE INICIAR LA APLICACION
            MainPage = new NavigationPage(new PaginaPrincipal());
            //SE INICIALIZAN LAS BANDERAS (FALSE)
            ConfigChangedFlag = false;
            ConfigChangedAdminFlag = false;
            RegistroFlag = false;
        }
        
        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE (EXISTEN DOS METODOS CONTRUCTORES, CON LA DIFERENCIA DE QUE UNO DE ELLOS 
        //REQUIERE QUE SEA ENVIADO UN PARAMETRO QUE LLEVA POR NOMBRE "filename" (PARAMETRO QUE LLEVARA EL 
        //NOMBRE DE ARCHIVO CON EL  CUAL SE IDENTIFICARA LA BASE DE DATOS LOCAL Sqlite QUE UTILIZA LA 
        //APLICACION CUANDO SE ENCUENTRA FUNCIONANDO STAND ALONE)
        //-----------------------------------------------------------------------------------------------
        //NOTA: ESTE METODO CONSTRUCTOR ES LLAMADO EN LAS CLASES "MainActivity" Y "AppDelegate" DE LOS 
        //PROYECTOS MTTO_App.Android Y MTTO_App.iOS RESPECTIVAMENTE
        //-----------------------------------------------------------------------------------------------
        public App(string fileName)
        {
            InitializeComponent();
            //SE CONFIGURA A LA PAGINA "PaginaPrincipal" COMO LA PAGINA INICIAL QUE
            //APARECERA AL MOMENTO DE INICIAR LA APLICACION
            MainPage = new NavigationPage(new PaginaPrincipal());
            FileName = fileName;
            //SE INICIALIZAN LAS BANDERAS (FALSE)
            ConfigChangedFlag = false;
            ConfigChangedAdminFlag = false;
            RegistroFlag = false;
        }

        protected override void OnStart()
        {
            Console.WriteLine("\n=================================================");
            Console.WriteLine("=================================================");
            Console.WriteLine("\nINICIO DE LA APLICACION");
            Console.WriteLine("=================================================");
            Console.WriteLine("=================================================\n");

            //=================================================================================
            //=================================================================================
            //NOTA: ESTE METODO ES LLAMADO LA APLICACION SE ENCUENTRA TRABAJANDO STAND ALONE
            //GetDefaultUsers();
            //=================================================================================
            //=================================================================================
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        //===============================================================================================
        //===============================================================================================
        //FUNCION LLAMADA PARA ACEPTAR EL CERTIFICADO DE SEGURIDAD GENERADO POR EL LOCALHOST
        public static HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            return handler;
        }

        //===============================================================================================
        //===============================================================================================
        //FUNCION PARA OBTENER LOS USUARIOS EXISTENTES POR DEFAULT
        //NOTA: ESTE METODO ES LLAMADO CUANDO LA APLICACION ES USADA STAND ALONE (AISLADA)
        public void GetDefaultUsers()
        {
            //SE APERTURA LA CONEXION CON LA BASE DE DATOS
            using (SQLiteConnection connection = new SQLiteConnection(App.FileName))
            {
                //DE NO EXISTIR SE CREAN LAS TABLAS "Personas" y "Usuarios"
                connection.CreateTable<Personas>();
                connection.CreateTable<Usuarios>();

                //SE LLENA UNA LISTA CON LOS ITEMS REGISTRADOS EN LA BASE DE DATOS
                List<Personas> personas = connection.Table<Personas>().ToList();
                List<Usuarios> usuarios = connection.Table<Usuarios>().ToList();

                //SE EVALUA SI LAS LISTAS TIENEN AL MENOS UN REGISTRO.
                //PUESTO QUE LOS REGISTROS DE PERSONAS Y DE USUARIOS ES PARALELO
                //LA CANTIDAD DE REGISTRO DE AMBAS DEBE SER LA MISMA
                if (personas.Any<Personas>() == false && usuarios.Any<Usuarios>() == false)
                {
                    //NO EXISTEN USUARIOS: DE NO EXISTIR NINGUN REGISTRO DE USUARIOS.
                    //SE LLENAN LAS LISTA CON LOS REGISTROS QUE EXISTIRAN POR DEFAULT
                    personas = new Personas().GetDefaultPersonas();
                    usuarios = new Usuarios().GetDefaultUsuarios();

                    //SE INSERTAN LOS REGISTROS EN SU RESPECTIVA TABLA
                    connection.InsertAll(personas);
                    connection.InsertAll(usuarios);

                    //SE CIERRA LA CONECCION CON LA BASE DE DATOS
                    connection.Close();
                }
                else
                {
                    //SE CIERRA LA CONECCION CON LA BASE DE DATOS
                    connection.Close();
                }
            }
        }
    }
}