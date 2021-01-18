namespace MTTO_App.ViewModel
{
    public class PaginaInformacionViewModel
    {
        //============================================================================================================
        //============================================================================================================
        //CLASE CREADA PARA MANIPULAR TODA LA CONFIGURACION DE PARAMETROS DE LAS PAGINAS DE INFORMACION
        //============================================================================================================
        //CREACION E INICIALIZACION DE VARIABLES LOCALES
        private string SourceOfCalling = string.Empty;

        //============================================================================================================
        //============================================================================================================
        //PROPIEDADES DE LA CLASE

        //-----------------------------------------TAMAÑO DE LA FUENTE------------------------------------------------
        public int HeaderFontSize { get { return App.HeaderFontSize; } }
        public int EntryFontSize { get { return App.EntryFontSize; } }
        public int LabelFontSize { get { return App.LabelFontSize; } }

        //--------------------------------------------COLOR DE FONDO--------------------------------------------------
        public string BackGroundColor { get { return App.BackGroundColorPopUp; } }

        public string FrameColor { get { return "Black"; } }

        //-----------------------------------------------IMAGENES-----------------------------------------------------
        public string CloseButton
        {
            /*https://iconos8.es/icons/set/close-window"
              Cerrar ventana icon by a target="_blank"
              href "https://iconos8.es"*/

            get { return "Cerrar24px2.png"; } 
        }

        //------------------------------------------TEXTOS DINAMICOS--------------------------------------------------
        //PROPIEDAD QUE EVALUA DESDE QUE CLASE SE ESTA LLAMANDO PARA LUEGO DECIDIR QUE TEXTO RETORNAR
        public string ListaDatosModificables
        {
            get
            {
                string Lista = string.Empty;

                if (!string.IsNullOrEmpty(SourceOfCalling))
                {
                    switch (SourceOfCalling)
                    {
                        case "CONFIGURACION":
                            Lista = "       -Numero telefonico.\n" +
                                    "       -Correo electronico.\n" +
                                    "       -Contraseña.";
                            break;

                        case "CONFIGURACIONADMIN":
                            Lista = "       -Nombre(s).\n" +
                                    "       -Apellido(s).\n" +
                                    "       -Fecha de nacimiento.\n" +
                                    "       -Numero telefonico.\n" +
                                    "       -Correo electronico.\n" +
                                    "       -Nivel de Usuario.\n" +
                                    "       -Contraseña.";
                            break;
                    }
                }

                return Lista;
            }
        }

        //PROPIEDAD QUE RETIENE LA LISTA DE LOS METODOS DE BUSQUEDA DE USUARIO DENTRO DE LA PAGINA QUERY ADMIn
        public string ListaMetodosDeConsulta
        {
            get
            {
                return "       -Consulta por ID (Cedula)\n"+
                       "       -Consulta por Ficha\n"+
                       "       -Consulta por Nombre(s)\n"+
                       "       -Consulta por Apellido(s)\n"+
                       "       -Consulta por Usuario";
            }
        }

        //PROPIEDAD QUE RETORNA EN UN TEXTO LOS ATRIBUTOS DE LA TABLA "ModificacionesUsuarios"
        public string ListaUltimasModificaciones
        {
            get
            {
                string Lista = string.Empty;

                Lista = "       -ID del usuario que acaba de ser modificado.\n" +
                        "       -ID del usuario que realizo la modificación.\n" +
                        "       -Fecha en la que se realizo la modificación.\n" +
                        "       -Atributo(s)/campo(s) modificado.";

                return Lista;
            }
        }

        //PROPIEDAD QUE RETORNA EN UN TEXTO LOS NIVELES DE USUARIO
        public string ListaNivelesUsuario
        {
            get
            {
                return "       -Nivel Bajo (0).\n" +
                         "       -Nivel Medio (5).\n" +
                         "       -Nivel Alto (10).\n";
            }
        }

        //PROPIEDAD QUE RETORNA EN UN TEXTO LAS OPCIONES DE METODO DE CONSULTA DE TABLEROS
        public string ListaOpcionesEscaneo
        {
            get
            {
                return "       -Consulta por escaneo.\n" +
                       "       -Consulta por ID (ID del tablero o ID Sap).";
            }
        }

        //PROPIEDAD QUE RETORNA EN UN TEXTO (LISTA) LA INFORMACION QUE SE DESPLIEGA EN PANTALLA
        public string ListaInformacionTablero
        {
            get
            {
                return "       -Codigo del Tablero (ID).\n" +
                       "       -Filial (filial a la que pertenece).\n" +
                       "       -Area (area/zona de la filial).\n" +
                       "       -Ultima consulta del tablero (fecha).\n" +
                       "       -Items del tablero (lista).\n"+
                       "       -Codigo QR asignado al tablero (imagen).";
            }
        }

        //PROPIEDAD QUE RETORNA EN UN TEXTO LOS CARACTERES NO PERMITIDOS
        public string Caracteres 
        {
            get { return CaracteresNoPermitidos(); }
        }

        //PROPIEDAD QUE RETORNA EL TEXTO QUE FUNCIONARA DE TITULO A LA PAGINA POP UP EMERGENTE
        public string TituloPH 
        { 
            get { return "INFORMACIÓN"; } 
        }

        //============================================================================================================
        //============================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaInformacionViewModel(string sourceofcalling)
        {
            //IDENFICAMOS DE QUE PAGINA SE ESTA HACIENDO EL LLAMADO DE LA CLASE
            SourceOfCalling = sourceofcalling.ToUpper();
        }

        //============================================================================================================
        //============================================================================================================
        //FUNCION QUE RETORNA EN FORMA DE TEXTO LOS CARACTERES NO PERMITIDOS
        public static string CaracteresNoPermitidos()
        {
            return "       !, @, #, $, %, (, ), +, =, /, | .";
        }
    }
}