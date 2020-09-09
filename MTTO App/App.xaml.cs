using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace MTTO_App
{
    public partial class App : Application
    {
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

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new PaginaPrincipal());
        }

        public App(string fileName)
        {
            //ESTABLECER CONEXION CON LA BASE DE DATOS
            InitializeComponent();
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

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
