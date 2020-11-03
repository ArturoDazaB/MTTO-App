using System;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MTTO_App.ViewModel
{
    public class PaginaPrincipalViewModel : INotifyPropertyChanging
    {
        //=========================================================================================================
        //=========================================================================================================
        //VARIABLES DE LA CLASE
        protected string username = string.Empty,
                         password = string.Empty;

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