using Android.Widget;
using System.Collections.Generic;

namespace MTTO_App
{
    internal class QueryAdminViewModel
    {
        //SE CREA LA LISTA QUE OBTENDRA LA INFORMACION
        //PROPORCIONADA POR LA CLASE "QueryAdminModel"
        public List<QueryAdminModel> InfoConfig { get; set; }

        public QueryAdminViewModel()
        {
            //SE INSTANCIA Y SE LLENA LA LISTA MEDIANTE LA FUNCION
            //"GetConfiguracion" de la clase "QueryAdminModel"
            InfoConfig = new QueryAdminModel().GetConfiguracion();
        }

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
        public List<Personas> ListaPersonas(int SeleccionBusqueda, string Dato)
        {
            //SE RECIBE COMO PARAMETRO QUE TIPO DE BUSQUEDA VA A REALIZARSE (SeleccionBusqueda)
            //LA CUAL PERMITE BUSCAR POR: ID, NOMBRES, APELLIDOS y USERNAME. MIENTRAS QUE Dato
            //TENDRA LA REFERENCIA A BUSCAR

            List<Personas> QueryPersonas = new List<Personas>();
            List<int> ListaUsuarioID = new List<int>();

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
                }
                //SE CIERRA LA BASE DE DATOS
                connection.Close();
            }
            //SE RETORNA LA LISTA CON LAS PERSONAS QUE CUMPLAN CON LA INFORMACION SOLICITADA
            return QueryPersonas;
        }

        //==================================================================================
        //==================================================================================
        //FUNCION PARA GENERAR UN MENSAJE NO INTERACTIVO POR PANTALLA
        public void MensajePantalla(string mensaje)
        {
            Toast.MakeText(Android.App.Application.Context, mensaje, ToastLength.Short).Show();
        }
    }
}