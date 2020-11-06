using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MTTO_App.ViewModel
{
    internal class ConfiguracionAdminViewModel : INotifyPropertyChanging
    {
        //===============================================================================================
        //===============================================================================================
        //VARIABLES LOCALES
        protected DateTime fechacreacion, fechanacimiento;

        protected string nombres, apellidos, correo, username, password, cedula, telefono, confirmacionpassword, numeroficha;

        //----------------------------------------------------------------------------
        public int nivelusuario;

        //----------------------------------------------------------------------------
        protected bool onsave = false;

        protected bool isbusy = true;

        protected bool flagLecturaEscritura = false;
        protected bool flagUsername = false;

        //===============================================================================================
        //===============================================================================================
        //CONSTANTES
        protected const int telefonomaxlengt = 11;

        protected const int passwordmaxlengt = 12;
        protected const int passwordyellowcolor = 6;
        protected const int passwordgreencolor = 9;

        //===============================================================================================
        //===============================================================================================
        //BANDERAS:
        //flagCedula -> TRUE: LAS CEDULAS (ID) SON DISTINTAS
        //              FALSE: LAS CEDULAS (ID) SON IGUALES
        public bool flagCedula = false,
                    flagNumeroFicha = false;

        //BANDERAS DE VERIFICACION (Paginas de Lectura y Escritura)
        public bool flagsameNombre = false,
                    flagsameApellido = false,
                    flagsameFecha = false,
                    flagsameTelefono = false,
                    flagsameCorreo = false,
                    flagsameUsername = false,
                    flagsamePassword = false,
                    flagdifferentPassword = false;

        //BANDERAS DE VERIFICACION (Paginas de Escritura)
        protected bool flagExistingCedula = false,
                       flagExistingNumeroFicha = false,
                       flagExistingUsername = false,
                       flagExistingCorreo = false;

        //===============================================================================================
        //===============================================================================================
        //OBJETOS LOCALES
        protected Personas Persona;

        protected Usuarios Usuario;

        //===============================================================================================
        //===============================================================================================
        //PROPIEDADES DE LA CLASE
        public DateTime FechaCreacion
        {
            //NOTA: EN EL CASO DE LA PROPIEAD "FechaCreacion" NO SE UTILIZA NINGUNO DE LOS EVENTOS ASIGNADOS
            //A LAS DEMAS PROPIEDADES (COMO ES EL CASO DE: "OnPropertyChanged" O "ConsoleWriteline", ESTO
            //DEBIDO A QUE NINGUN USUARIO TIENE LA FACULTAD DE MODIFICAR ESTE CAMPO DE LA TABLA "Personas"

            get
            {
                //SE LEE LA INFORMACION QUE fechaCreacion
                return fechacreacion;
            }
            set
            {
                //LE ASIGNA EL VALOR A LA VARIABLE LOCAL "fechaCreacion"
                //LA INFORMACION QUE "Persona.FechaCreacion" ALMACENA
                fechacreacion = value;
            }
        }

        //-------------------------------------------------------------------------------------------------
        //PARA EL RESTO DE PRPIEDADES DE LA CLASE SE APLICA EL MISMO METODO DE
        //ESCRITURA Y LECTURA (propiedades get; set;)
        //-------------------------------------------------------------------------------------------------
        public string Nombres
        {
            get { return nombres; }
            set
            {
                nombres = value;

                //=================================================================================================
                //=================================================================================================
                //EVENTO QUE SE DISPARA CUANDO SE MODIFICA O ACTUALIZA
                //LA PROPIEDAD (CUANDO EL USUARIO SE ENCUENTRA INTERACTUANDO
                //CON LAS ENTRADAS DE LA APLICACION: Entry, Picker, DatePicker, etc).
                OnPropertyChanged();

                if (flagLecturaEscritura == false)
                {
                    //SI SE ACCEDE DESDE UNA PAGINA DE ESCRITURA (PaginaRegistro.xaml.cs o PaginaRegistro2.xaml.cs)
                    //SE PROCEDE A ACTUALIZAR EL NOMBRE DE USUARIO CADA QUE ESTE SEA MODIFICADO
                    OnPropertyChanged(Username);
                }

                //=================================================================================================
                //=================================================================================================
                //EVENTOS QUE, DEPENDIENDO DESDE QUE PAGINA SE INVOCA A LA CLASE
                //"ConfiguracionAdmin2ViewModel.cs", MOSTRARA EL VALOR QUE SE ESTA
                //INGREANDO (CASO PAGINA "PaginaRegistro.xaml.cs") O MOSTRANDO EL
                //VALOR PREVIO Y EL VALOR MODIFICADO (CASO PAGINA "PaginaConfiguracion2.xaml.cs"
                //o "PaginaConfiguracionAdmin2.xaml.cs"
                if (flagLecturaEscritura)
                    ConsoleWriteline("Nombre(s)", Persona.Nombres, Nombres);
                else
                    ConsoleWriteline("Nombres(s)", Nombres);

                //=================================================================================================
                //=================================================================================================
                //PARA EL CASO DE LAS PAGINAS DE LECTURA Y ESCRITURA ("PaginaConfiguracion2.xaml.cs"
                // o "PaginaConfiguracionAdmin2.xaml.cs"), CUANDO EL USUARIO INTERACTUE CON ALGUNA
                //DE LAS PROPIEDADES DE LA CLASE (MEDIANTE LAS ENTRADAS CONFIGURADAS EN "PaginaCOnfiguracion2.xaml"
                // y "PaginaConfiguracionAdmin2.xaml") ESTAS EVALUARAN SI EL VALOR QUE POSEEN ES IGUAL
                //AL VALOR QUE YA SE ENCUENTRA REGISTRADO, DE SER CIERTO SE DISPARA UNA BANDERA, LA CUAL
                //PUEDE SER VERIFICADA DESDE LAS CLASES DONDE SE INVOCO LA CLASE "ConfiguracionAdmin2ViewModel.cs"
                if (Nombres == Persona.Nombres)
                    flagsameNombre = true;
                else
                    flagsameNombre = false;
            }
        }

        //-------------------------------------------------------------------------------------------------
        //PARA EL RESTO DE PROPIEDADES DE LA CLASE (EXCEPTO LA PROPIEDAD CEDULA y FECHACREACION)
        //SE APLICA EL MISMO CODIGO DE:
        //	1)DETECCION DE CAMBIOS
        //	2)IMPRESION POR CONSOLA
        //	3)DISPARO DE BANDERAS
        //-------------------------------------------------------------------------------------------------
        public string Apellidos
        {
            get { return apellidos; }
            set
            {
                apellidos = value;
                OnPropertyChanged();

                if (flagLecturaEscritura == false)
                {
                    //SI SE ACCEDE DESDE UNA PAGINA DE ESCRITURA (PaginaRegistro.xaml.cs o PaginaRegistro2.xaml.cs)
                    //SE PROCEDE A ACTUALIZAR EL NOMBRE DE USUARIO CADA QUE ESTE SEA MODIFICADO
                    OnPropertyChanged(Username);
                }

                if (flagLecturaEscritura)
                    ConsoleWriteline("Apellido(s)", Persona.Apellidos, Apellidos);
                else
                    ConsoleWriteline("Apellido(s)", Apellidos);

                if (Apellidos == Persona.Apellidos)
                    flagsameApellido = true;
                else
                    flagsameApellido = false;
            }
        }

        public string Cedula
        {
            get { return cedula; }
            set
            {
                cedula = value;

                ConsoleWriteline("Cedula (ID)", Cedula);
            }
        }

        public string NumeroFicha
        {
            get { return numeroficha; }
            set
            {
                //SE LLAMA A ESTA FUNCION PARA DETECTAR CAMBIOS EN LA PROPIEDAD
                OnPropertyChanged();
                //SE REGRESA EL VALOR QUE FUE INGRESADO
                numeroficha = value;

                if (flagLecturaEscritura)
                    ConsoleWriteline("Numero de Ficha", Persona.NumeroFicha.ToString(), FechaNacimiento.ToString());
                else
                    ConsoleWriteline("Numero de Ficha", NumeroFicha.ToString());

                if (NumeroFicha == Persona.NumeroFicha.ToString())
                    flagNumeroFicha = true;
                else
                    flagNumeroFicha = false;
            }
        }

        public DateTime FechaNacimiento
        {
            get { return fechanacimiento; }
            set
            {
                fechanacimiento = value;
                OnPropertyChanged();

                if (flagLecturaEscritura)
                    ConsoleWriteline("Fecha de Nacimiento", Persona.FechaNacimiento.ToString(), FechaNacimiento.ToString());
                else
                    ConsoleWriteline("Fecha de Nacimiento", FechaNacimiento.ToString());

                if (FechaNacimiento == Persona.FechaNacimiento)
                    flagsameFecha = true;
                else
                    flagsameFecha = false;
            }
        }

        public string Telefono
        {
            get { return telefono; }
            set
            {
                telefono = value;
                OnPropertyChanged();

                if (flagLecturaEscritura)
                    ConsoleWriteline("Telefono", Persona.Telefono.ToString(), Telefono);
                else
                    ConsoleWriteline("Telefono", Telefono);

                if (Telefono == Persona.Telefono.ToString())
                    flagsameTelefono = true;
                else
                    flagsameTelefono = false;
            }
        }

        public string Correo
        {
            get { return correo; }
            set
            {
                correo = value;
                OnPropertyChanged();

                if (flagLecturaEscritura)
                    ConsoleWriteline("Correo", Persona.Correo, Correo);
                else
                    ConsoleWriteline("Correo", Correo);

                if (Correo == Persona.Correo)
                    flagsameCorreo = true;
                else
                    flagsameCorreo = false;
            }
        }

        public string Username
        {
            get
            {
                //EVALUAMOS DESDE QUE PAGINA ESTAMOS INGRESANDO
                if (!flagLecturaEscritura)
                {
                    //SE INICIALIZA LA VARIABLE
                    username = string.Empty;

                    //SE EVALUA QUE LOS ATRIBUTOS "Nombre(s)" Y "Apellido(s)" NO SE ENCUENTREN VACIAS
                    if (!string.IsNullOrEmpty(Nombres) && !string.IsNullOrEmpty(Apellidos))
                    {
                        //SE APERTURA LA CONEXION CON LA BASE DE DATOS
                        using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.FileName))
                        {
                            //CREAMOS E INICIALIZAMOS UNA VARIABLE BANDERA
                            bool flagregistro = false;

                            //DE NO EXISTIR SE CREA LA TABLA Usuarios
                            connection.CreateTable<Usuarios>();

                            //SE GENERA EL NOMBRE
                            username = char.ToUpper(Nombres[0]) + Metodos.FirstString(Apellidos.ToLower());

                            //RECORREMOS TODOS LOS REGISTROS EN LA TABLA USUARIOS
                            foreach (Usuarios registro in connection.Table<Usuarios>().ToList())
                            {
                                //COMPARAMOS EL Username DE CADA REGISTRO CON EL Username QUE ACABAMOS DE GENERAR
                                if (registro.Username.ToLower() == username.ToLower())
                                {
                                    //DAMOS SET A LA BANDERA DE REGISTRO
                                    flagregistro = true;
                                    //DETENEMOS EL RECORRIDO DE LOS REGISTROS
                                    break;
                                }
                            }

                            //EVALUAMOS EL ESTADO DE LA BANDERA
                            if (flagregistro)
                            {
                                //SE VUELVE A GENERAR EL NOMBRE DE USUARIO BAJO LA SIGUIENTE PREMISA
                                //USERNAME = INICIAL PRIMER NOMBRE + INICIAL SEGUNDO NOMBRE + PRIMER APELLIDO
                                username = char.ToUpper((Metodos.FirstString(Nombres))[0]) + char.ToUpper((Metodos.SecondString(Nombres))[0]) + Metodos.FirstString(Apellidos.ToLower());
                            }

                            //SE IMPRIME POR CONSOLA EL NOMBRE DE USUARIO
                            ConsoleWriteline("Username", username);
                        }
                    }
                }

                return username;
            }
            set
            {
                if (flagLecturaEscritura)
                {
                    username = value;
                    OnPropertyChanged();

                    ConsoleWriteline("Username", Usuario.Username, Username);

                    if (Username == Usuario.Username)
                        flagsameUsername = true;
                    else
                        flagsameUsername = false;
                }
            }
        }

        public int NivelUsuario
        {
            get
            {
                OnPropertyChanged();
                return nivelusuario;
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();

                if (flagLecturaEscritura)
                    ConsoleWriteline("Password", Usuario.Password, Password);
                else
                    ConsoleWriteline("Password", Password);

                if (Password == Usuario.Password)
                    flagsamePassword = true;
                else
                    flagsamePassword = false;
            }
        }

        public string ConfirmacionPassword
        {
            get { return confirmacionpassword; }
            set
            {
                confirmacionpassword = value;
                OnPropertyChanged();

                ConsoleWriteline("Confirmacion Password", ConfirmacionPassword);

                if (ConfirmacionPassword.Length >= Password.Length)
                {
                    if (ConfirmacionPassword != Password)
                        flagdifferentPassword = true;
                    else
                        flagdifferentPassword = false;
                }
            }
        }

        //=========================================================================================================
        //=========================================================================================================
        //COLORES DEL entryPassword1 y entryPassword2
        public int PasswordMaxLegnt { get { return passwordmaxlengt; } }

        public int PasswordYellowColor { get { return passwordyellowcolor; } }
        public int PasswordGreenColor { get { return passwordgreencolor; } }

        //---------------------------------------------------------------------------------------------------------
        //MAXIMA CANTIDAD DE NUMEROS PARA EL CAMPO TELEFONO
        public int TelefonoMaxLegnt { get { return telefonomaxlengt; } }

        //---------------------------------------------------------------------------------------------------------
        //TAMAÑO DE LAS LETRAS
        public int LabelFontSize { get { return App.LabelFontSize; } }

        public int EntryFontSize { get { return App.EntryFontSize; } }
        public int HeaderFontSize { get { return App.HeaderFontSize; } }

        //---------------------------------------------------------------------------------------------------------
        //COLOR DE FONTO Y DE BOTONES
        public string BackGroundColor { get { return App.BackGroundColor; } }

        public string ButtonColor { get { return App.ButtonColor; } }

        //---------------------------------------------------------------------------------------------------------
        //TEXTOS
        public string RegistroPH { get { return "Pagina de Registro"; } }

        public string ConfiguracionPH { get { return "Pagina de Configuracion"; } }
        public string InformacionP { get { return "Datos Personales"; } }
        public string InformacionU { get { return "Datos de Usuario"; } }
        public string FechaRegistroPH { get { return "Fecha de Registro: "; } }
        public string NombresPH { get { return "Nombre(s): "; } }
        public string ApellidosPH { get { return "Apellido(s): "; } }
        public string CedulaPH { get { return "Cedula: "; } }
        public string NumeroFichaPH { get { return "Numero de Ficha"; } }
        public string FechaNacimientoPH { get { return "Fecha de Nacimiento: "; } }
        public string TelefonoPH { get { return "Telefono celular/movil: "; } }
        public string CorreoPH { get { return "Correo electronico (email): "; } }
        public string UsernamePH { get { return "Nombre de usuario (username): "; } }
        public string NivelUsuarioPH { get { return "Nivel de Usuario"; } }
        public string PickerTitulo { get { return "Opciones"; } }
        public string PasswordPH { get { return "Contraseña (password): "; } }
        public string Actualizar { get { return "ACTUALIZAR"; } }
        public string Registrar { get { return "REGISTRAR"; } }

        //----------------------------------------------------------------------------------------------------------
        //LISTA DE NIVELES DE USUARIO (USADO EN LA PAGINA "PaginaRegistro.xaml.cs")
        public List<string> NivelUsuarioList
        {
            get
            {
                //LISTA DE NIVELES DE USUARIO
                return new Usuarios().NivelUsuarioLista();
            }
        }

        //=========================================================================================================
        //=========================================================================================================
        public bool Error
        {
            get
            {
                if (flagCedula)
                    MensajeConsole("OCURRIO UN ERROR AL ABRIR LA PAGINA");
                return flagCedula;
            }
        }

        public bool OnSave
        {
            get
            {
                return onsave;
            }

            set
            {
                onsave = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get
            {
                return isbusy;
            }
            set
            {
                isbusy = value;

                OnPropertyChanged();
            }
        }

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE
        public ConfiguracionAdminViewModel(bool flag, Personas persona, Usuarios usuario)
        {
            //=================================================================================================================================
            //=================================================================================================================================
            //LA VARIABLE "flagLecturaEscritura" (DEL TIPO BOOL) INDICARA SI SE ESTA INVOCANDO LA CLASE "ConfiguracionAdminViewModel" DESDE UNA
            //PAGINA DE LECTURA Y ESCRITURA (COMO ES EL CASO DE LA PAGINA "ConfiguracionAdmin.xaml.cs" LA CUAL LEE Y MODIFICA
            //LOS ATRIBUTOS DE LOS OBJETOS Persona y Usuario) O SIMPLEMENTE ESCRITURA (COMO ES EL CASO DE LA PAGINA "PaginaRegistro.xaml.cs")

            //flagLecturaEscritura => true => Pagina de LecturaEscritura
            //flagLecturaEscritura => false => Pagina de Escritura

            flagLecturaEscritura = flag;

            //=================================================================================================================================
            //=================================================================================================================================
            //SI "ConfiguracionAdminViewModel.cs" ES INVOCADA DESDE LAS PAGINAS "PaginaConfiguracion.xaml.cs" y "PaginaConfiguracionAdmin2.xaml.cs"
            //SE PROCEDE A CARGAR TODA LA INFORMACION DE LA PERSONA A MODIFICAR. POR OTRO LADO, SI "ConfiguracionAdminViewModel.CS" ES INVOCADA
            //DESDE LA PAGINA "PaginaRegistro.xaml.cs" SE DEJAN VACIOS LOS CAMPOS

            if (flagLecturaEscritura == false)
            {
                //SE INICIALIZAN LAS VARIABLES LOCALES
                nombres = apellidos = cedula = numeroficha = telefono = correo = username = password = confirmacionpassword = string.Empty;
                nivelusuario = 0;
                fechacreacion = fechanacimiento = DateTime.Now;

                //SE ALMACENA LA INFORMACION DEL USUARIO QUE SE ENCUENTRA LOGGEADO EN EL MOMENTO
                Persona = new Personas().NewPersona(persona);
                Usuario = new Usuarios().NewUsuario(usuario);

                MensajeConsole("PUNTO DE ACCESO => PAGINA REGISTRO");
            }
            else
            {
                //SE CREAN LOS OBJETOS QUE CONTENDRAN LA INFORMACION DEL USUARIO A MODIFICAR
                Persona = new Personas().NewPersona(persona);
                Usuario = new Usuarios().NewUsuario(usuario);

                //SE CARGA LA INFORMACION EN LAS VARIABLES LOCALES
                nombres = Persona.Nombres;
                apellidos = Persona.Apellidos;
                cedula = Persona.Cedula.ToString();
                numeroficha = Persona.NumeroFicha.ToString();
                fechacreacion = Persona.FechaCreacion;
                fechanacimiento = Persona.FechaNacimiento;
                telefono = Persona.Telefono.ToString();
                correo = Persona.Correo;

                username = Usuario.Username;
                nivelusuario = Usuario.NivelUsuario;
                password = Usuario.Password;

                MensajeConsole("PUNTO DE ACCESO => PAGINA CONFIGURACION");
            }

            //=================================================================================================================================
            //=================================================================================================================================
            //SE VERIFICA QUE LOS DOS OBJETOS CONTENGAN EL MISMO IDENTIFICATIVO
            if (Persona.Cedula != Usuario.Cedula)
                flagCedula = true;
            else
                flagCedula = false;
        }

        //===============================================================================================
        //===============================================================================================
        public event PropertyChangingEventHandler PropertyChanging;

        //ACTUALIZA LA INFORMACION DE LA PROPIEDAD CADA QUE SE DECTECTA UN CAMBIO MINIMO
        protected void OnPropertyChanged([CallerMemberName] string nombre = "")
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nombre));
        }

        //===============================================================================================
        //===============================================================================================
        //METODO PARA ACTUALIZAR ("PaginaConfiguracion2.xaml.cs" y "PaginaConfiguracionAdmin2.xaml.cs"
        //Y GUARDAR DATOS ("PaginaRegistro.xaml.cs")
        public string Save()
        {
            //SE CREA E INICIALIZA EL MENSAJE QUE SERA RETORNADO A LA PAGINA DESDE
            //DONDE SE HAYA INVOCADO A LA CLASE
            string respuesta = string.Empty;

            //SE VERIFICA SI SE HA REALIZADO AL MENOS ALGUN CAMBIO A ALGUNO DE LOS CAMPOS
            //NOTA: DE INVOCAR EL METODO DESDE LA PAGINA "PaginaRegistro.xaml.cs" EL METODO
            //RETORNARA TRUE POR DEFECTO
            if (EvaluacionDeCampos1())
            {
                //SE VERIFICA QUE LAS PROPIEDADES/ATRIBUTOS DE LA PAGINA NO SEAN VACIOS O NULOS
                //ADEMAS DE VERIFICAR QUE LA FECHA SELECCIONADA NO SEA LA ACTUAL Y NO SEA UNA FECHA QUE
                //NO HA EXISTIDO O SUCEDIDO.
                if (EvaluacionDeCampos2(false))
                {
                    //==========================================================================================
                    //==========================================================================================

                    //SE VERIFICA QUE CADA UNA DE LAS PROPIEDADES CUMPLA CON LOS REQUISITOS MINIMOS
                    if (EvaluacionDeCampos3())
                    {
                        //TODAS LAS PROPIEDADES CUMPLEN CON LOS REQUISITOS MINIMOS
                        //SE APERTURA LA BASE DE DATOS
                        using (SQLiteConnection connection = new SQLiteConnection(App.FileName))
                        {
                            //POR ULTIMO SE VERIFICA SI SE ESTAN MODIFICANDO DATOS O
                            //REALIZANDO UN NUEVO REGISTRO

                            //flagLecturaEscritura = true => Acceso desde "PaginaConfiguracionAdmin.xaml.cs" o "PaginaConfiguracion.xaml.cs"
                            //flagLecturaEscritura = false => Acceso desde "PaginaRegistro.xaml.cs"
                            if (flagLecturaEscritura)
                            {
                                //----------------------------------------------------------------------------------------------------
                                //----------SECCION QUE LUEGO SERA MODIFICADA PARA INGRESAR LOS COMANDOS DE HTTPCLIENT----------------
                                //---------------------------API CONTROLLER CLASS: "ConfiguracionController"--------------------------
                                //----------------------------------------------------------------------------------------------------
                                //DE NO EXISTIR SE CREA LA TABLA "ModificacionesUsuarios"
                                connection.CreateTable<ModificacionesUsuarios>();

                                //SE CREAN LOS OBJETOS QUE CONTENDRAN LA INFORMACION ACTUALIZADA
                                Personas NewPersona = new Personas().NewPersona(FechaCreacion, Metodos.Mayuscula(Nombres), Metodos.Mayuscula(Apellidos),
                                    Cedula, NumeroFicha, FechaNacimiento, Telefono, Correo);
                                Usuarios NewUsuario = new Usuarios().NewUsuario(Username, Password, Cedula, FechaCreacion, NivelUsuario);

                                //SE ACTUALIZAN LOS OBJETOS DENTRO DE SU RESPECTIVA TABLA
                                connection.Update(NewPersona);
                                connection.Update(NewUsuario);

                                //SE CREA UN NUEVO OBJETO QUE ALMACENARA LA INFORMACION DEL REGISTRO DE UNA
                                //NUEVA ENTRADA DE MODIFICACIONES
                                ModificacionesUsuarios Modificaciones = new ModificacionesUsuarios()
                                    .NewModificacionesUsuarios(NewPersona, Persona, NewUsuario, Usuario, DateTime.Now);

                                //SE INSERTA EN LA TABLA EL NUEVO REGISTRO DE LAS MODIFICACIONES
                                connection.Insert(Modificaciones);

                                //SE CIERRA LA CONEXION CON LA BASE DE DATOS
                                connection.Close();

                                //SE GENERA UN MENSAJE TOAST (System Message) EL CUAL NOTIFICA AL USUARIO QUE LOS DATOS
                                //HAN SIDO MODIFICADOS SATISFACTORIAMENTE
                                MensajePantalla("Datos Modificados Satisfactoriamente");

                                //SE ACTIVA LA BANDERA DE CAMBIO DE DATOS PARA EXPULSAR AL USUARIO LOGGEADO
                                //ASI CUANDO EL USUARIO VUELVA LOGGEARSE LOS NUEVOS DATOS SE VERAN DESPLEGADOS
                                App.ConfigChangedFlag = true;
                                //----------------------------------------------------------------------------------------------------
                                //----------------------------------------------------------------------------------------------------
                            }
                            else
                            {
                                //----------------------------------------------------------------------------------------------------
                                //----------SECCION QUE LUEGO SERA MODIFICADA PARA INGRESAR LOS COMANDOS DE HTTPCLIENT----------------
                                //------------------------API CONTROLLER CLASS: "RegistroUsuariosController"--------------------------
                                //----------------------------------------------------------------------------------------------------
                                //SE CREAN LAS TABLAS. LA LIBRERIA SQLite EVITA ESTA FUNCION DE
                                //YA EXISTIR UNA TABLA DEL TIPO <objeto> YA CREADA PREVIAMENTE
                                connection.CreateTable<Personas>();
                                connection.CreateTable<Usuarios>();

                                //SE VERIFICA UN ATRIBUTO DE CADA UNO DE LOS ITEMS Y SE COMPARA
                                //CON EL VALOR DEL ATRIBUTO DEL NUEVO ITEM A REGISTRAR, DE EXISTIR
                                //UN MATCH SE CANCELA EL REGISTRO
                                flagExistingCedula = Metodos.MatchCedula(connection.Table<Personas>().ToList(), Cedula);
                                flagExistingUsername = Metodos.MatchUsername(connection.Table<Usuarios>().ToList(), Username.ToLower());
                                flagExistingNumeroFicha = Metodos.MatchNumeroFicha(connection.Table<Personas>().ToList(), NumeroFicha);

                                if (!flagExistingCedula &&      //TRUE: SE ENCONTRO UN REGISTRO CON LA MISMA CEDULA (ID)
                                    !flagExistingUsername &&    //TRUE: SE ENCONTRO UN REGISTRO CON EL MISMO NOMBRE DE USUARIO
                                    !flagExistingNumeroFicha)   //TRUE: SE ENCONTRO UN REGISTRO CON EL MISMO NUMERO DE FICHA
                                {
                                    //SE INSERTAN LOS NUEVOS REGISTROS EN SUS RESPECTIVAS TABLAS
                                    //TABLA PERSONAS:
                                    connection.Insert(new Personas().NewPersona(FechaCreacion, Metodos.Mayuscula(Nombres), Metodos.Mayuscula(Apellidos),
                                        Cedula, NumeroFicha, FechaNacimiento, Telefono, Correo.ToLower()));

                                    //TABLA USUARIOS
                                    connection.Insert(new Usuarios().NewUsuario(Username.ToLower(), Password, Cedula, FechaCreacion, NivelUsuario));

                                    //SE GENERA UN MENSAJE DE NOTIFICACION DE ALMACENAMIENTO
                                    MensajePantalla("Registro completado satisfactoriamente");

                                    //SE MANDA A IMPRIMIR POR CONSOLA LOS RESULTADOS ALMACENADOS
                                    NotificacionRegistro();

                                    //SE NOTIFICA QUE SE REALIZO UN NUEVO REGISTRO
                                    App.RegistroFlag = true;

                                    //SE CIERRA LA BASE DE DATOS
                                    connection.Close();
                                }
                                else
                                {
                                    //SI ALGUNA DE LAS BANDERAS SE DISPARA SE GENERARA UN MENSAJE DE NOTIFICACION DEPENDIENTO DE
                                    //CUAL DE ELLAS SE HAYA DISPARADO
                                    if (flagExistingCedula)
                                        respuesta = "El numero de cedula que desea registrar ya ha sido previamente registrado." +
                                            "\nVerifique la cedula e intente nuevamente";

                                    if (flagExistingUsername)
                                        respuesta = "El nombre de usuario que intenta registrar ya ha sido previamente registrado. " +
                                            "\nIntente con un nombre distinto";

                                    if (flagExistingNumeroFicha)
                                        respuesta = "El Numero de Ficha que intenta registrar ya ha sido previamente registrado. " +
                                            "\nIntente con un numero de ficha distinto";
                                }
                                //----------------------------------------------------------------------------------------------------
                                //----------------------------------------------------------------------------------------------------
                            }
                        }
                    }
                    else
                    {
                        //EN CASO DE QUE ALGUNA DE LAS CONDICIONES NO SE CUMPLA SE VERIFICA CUAL DE ELLAS ES
                        //Y SE PROCEDE A ENVIAR UN MENSAJE AL USUARIO.

                        if (Metodos.EspacioBlanco(Correo))
                            respuesta = "El correo no puede contener espacios en blanco";

                        if (Metodos.EspacioBlanco(Password))
                            respuesta = "La contraseña no puede contener espacios en blanco";

                        if (Metodos.EspacioBlanco(Username))
                            respuesta = "El nombre de usuario no puede contener espacios en blanco";

                        if (Metodos.Caracteres(Password))
                            respuesta = "La contraseña no puede contener los siguientes caracteres: " + PaginaInformacionViewModel.CaracteresNoPermitidos();

                        if (Metodos.Caracteres(Username))
                            respuesta = "El nombre de usuario no puede contener los siguientes caracteres: " + PaginaInformacionViewModel.CaracteresNoPermitidos();

                        if (Metodos.Caracteres(Nombres))
                            respuesta = "El nombre no puede contener los siguientes caracteres: " + PaginaInformacionViewModel.CaracteresNoPermitidos();

                        if (Metodos.Caracteres(Apellidos))
                            respuesta = "El apellido no puede contener los siguientes caracteres: " + PaginaInformacionViewModel.CaracteresNoPermitidos();
                    }
                }
                else
                {
                    respuesta = "Ningun campo debe quedar en blanco";
                }
            }
            else
            {
                respuesta = "No se ha modificado ninguno de los campos";
            }

            return respuesta;
        }

        //===================================================================
        //===================================================================
        //FUNCION DE ACTIVADA PARA DAR UN TIEMPO DE ESPERA
        public async Task SaveContact()
        {
            onsave = isbusy = true;
            await Task.Delay(500);

            onsave = isbusy = false;
        }

        public bool ContinuarRegistro(bool flag)
        {
            //SE EVALUAN QUE LOS CAMPOS CORRESPONDIENTES A LOS CAMPOS PERSONALES NO SE ENCUENTREN VACIOS O NULOS
            if (EvaluacionDeCampos2(flag))
                return true;
            else
                return false;
        }

        //====================================================================
        //====================================================================
        //FUNCION QUE EVALUA SI SE HA REALIZADO ALGUN CAMBIO SIGNIFICATIVO
        //SOBRE ALGUNO DE LOS ATRIBUTOS DEL OBJETO PERSONA Y USUARIO

        protected bool EvaluacionDeCampos1()
        {
            //SE VERIFICA DESDE QUE TIPO DE PAGINA FUE INVOCADA LA CLASE
            //SI FUE INVOCADA DESDE UNA PAGINA DE LECTURAESCRITURA SE TIENE
            //  flagLecturaEscritura => true => Pagina de LecturaEscritura
            //  flagLecturaEscritura => false => Pagina de Escritura

            if (flagLecturaEscritura)
            {
                if (Persona.Nombres == Nombres &&                   //VERIFICA SI LOS NOMBRES SON IGUALES
                    Persona.Apellidos == Apellidos &&               //VERIFICA SI LOS APELLIDOS SON IGUALES
                    Persona.FechaNacimiento == FechaNacimiento &&   //VERIFICA SI LAS FECHAS DE NACIMIENTO SON IGUALES
                    Persona.Telefono.ToString() == Telefono &&      //VERIFICA SI LOS TELEFONOS SON IGUALES
                    Persona.Correo == Correo &&                     //VERIFICA SI LOS CORREOS SON IGUALES
                    Usuario.Username == Username &&                 //VERIFICA SI LOS NOMBRES DE USUARIOS SON IGUALES
                    Usuario.Password == Password &&                 //VERFICIA SI LAS CONTRASEÑAS SON IGUALES
                    Usuario.NivelUsuario == NivelUsuario)   //VERIFICA SI EL NIVEL DE USUARIO ES IGUAL(?)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }

        //===================================================================================
        //===================================================================================
        //FUNCION QUE VERIFICA QUE NINGUN CAMPO SE ENCUENTRE NULO O VACIO

        protected bool EvaluacionDeCampos2(bool modo)
        {
            bool resultado = false;

            //EL PARAMETRO RECIBIDO DEL TIPO BOOL "modo" INDICA SI ESTA FUNCION ES LLAMADA PARA CONTINUAR LLENANDO
            //INFORMACION DE REGISTRO (INFORMACION DE USUARIO) O ES LLAMADA PARA REALIZAR EL REGISTRO COMPLETO DE USUARIO
            //modo = true => CONTINUAR LLENANDO INFORMACION DE REGISTRO
            //modo = false => LLAMADO PARA REGISTRAR EN LA BASE DE DATOS EL NUEVO USUARIO
            if (modo)
            {
                if (!string.IsNullOrEmpty(Nombres) &&   //EL NOMBRE NO PUEDE SER VACIO O NULO
                !string.IsNullOrEmpty(Apellidos) &&     //EL APELLIDO NO PUEDE SER VACIO O NULO
                FechaNacimiento != DateTime.Now &&      //NO SE PUEDE REGISTRAR LA FECHA ACTUAL
                FechaNacimiento < DateTime.Now &&       //LA FECHA NO PUEDE SER MAYOR A LA FECHA ACTUAL
                !string.IsNullOrEmpty(Telefono) &&      //EL TELEFONO NO PUEDE SER VACIO O NULLO
                !string.IsNullOrEmpty(Correo) &&        //EL CORREO NO PUEDE SER VACIO O NULLO
                !Error)                                 //EL ATRIBUTO ERROR NO PUEDE SER VERDADERO
                {
                    resultado = true;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Nombres) &&  //EL NOMBRE NO PUEDE SER VACIO O NULO
                !string.IsNullOrEmpty(Apellidos) &&  //EL APELLIDO NO PUEDE SER VACIO O NULO
                FechaNacimiento != DateTime.Now &&  //NO SE PUEDE REGISTRAR LA FECHA ACTUAL
                FechaNacimiento < DateTime.Now &&  //LA FECHA NO PUEDE SER MAYOR A LA FECHA ACTUAL
                !string.IsNullOrEmpty(Telefono) &&  //EL TELEFONO NO PUEDE SER VACIO O NULLO
                !string.IsNullOrEmpty(Correo) &&  //EL CORREO NO PUEDE SER VACIO O NULLO
                !string.IsNullOrEmpty(Username) &&  //EL USERNAME NO PUEDE SER VACIO O NULLO
                !string.IsNullOrEmpty(Password) &&  //EL PASSWORD NO PUEDE SER VACIO O NULLO
                !Error)                                 //EL ATRIBUTO ERROR NO PUEDE SER VERDADERO
                {
                    resultado = true;
                }
            }

            return resultado;
        }

        //===================================================================================
        //===================================================================================
        //FUNCION QUE VERIFICA QUE TODOS LOS CAMPOS CUMPLAN CON LAS CONDICIONES MINIMAS

        protected bool EvaluacionDeCampos3()
        {
            if (!Metodos.EspacioBlanco(Correo) &&        //CORREO NO PUEDE CONTENER ESPACIOS EN BLANCO
                !Metodos.EspacioBlanco(Password) &&        //PASSWORD NO PUEDE CONTENER ESPACIOS EN BLANCO
                !Metodos.Caracteres(Password) &&        //PASSWORD NO PUEDE CONTENER CARACTERES ESPECIFICOS
                !Metodos.Caracteres(Username) &&        //USERNAME NO PUEDE CONTENER CARACTERES ESPECIFICOS
                !Metodos.EspacioBlanco(Username) &&        //USERNAME NO PUEDE CONTENER ESPACIOS EN BLANCO
                !Metodos.Caracteres(Nombres) &&        //NOMBRES NO PUEDE CONTENER CARACTERES ESPECIFICOS
                !Metodos.Caracteres(Apellidos))               //APELLIDOS NO PUEDE CONTENER CARACTERES ESPECIFICOS
                return true;
            else
                return false;
        }

        //===================================================================================
        //===================================================================================
        //FUNCION PARA CARGAR LOS DATOS DE CADA UNO DE LOS OBJETOS DE LA CLASE

        public void GetInfo(Personas persona, Usuarios usuario)
        {
            //=====================================================================
            //=====================================================================
            //SE CARGA LA INFORMACION PERSONAL Y DE USUARIO DEL USUARIO
            //QUE FUE SELECCIONADO.
            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);

            //================================================================
            //================================================================
            //SE CARGA LA INFORMACION A LAS VARIABLES LOCALES
            nombres = Persona.Nombres;
            apellidos = Persona.Apellidos;
            cedula = Persona.Cedula.ToString();
            fechacreacion = Persona.FechaCreacion;
            fechanacimiento = Persona.FechaNacimiento;
            telefono = Persona.Telefono.ToString();
            correo = Persona.Correo;

            username = Usuario.Username;
            password = Usuario.Password;
            //================================================================
            //================================================================
        }

        //===================================================================================
        //===================================================================================
        //FUNCION LLAMADA CADA QUE SE EJECUTA UN CAMBIO EN ALGUNA DE LAS PROPIEDADES DE LA
        //CLASE

        //FUNCION LLAMADA CUANDO SE LLAMA LA CLASE "ConfiguracionAdmin2ViewModel.CS" DESDE
        //LA CLASE "PaginaConfiguracionAdmin2.xaml.cs"
        protected void ConsoleWriteline(string WHO, string ValorPrevio, string ValorActual)
        {
            Console.WriteLine("\n\n=============================================");
            Console.WriteLine("=============================================");
            Console.WriteLine("\nPROPIEDAD: " + WHO);
            Console.WriteLine("\n\nValor previo: " + ValorPrevio + "\nValor actual: " + ValorActual);
            Console.WriteLine("=============================================");
            Console.WriteLine("=============================================\n\n");
        }

        //===================================================================================
        //===================================================================================
        //FUNCION LLAMADA CUANDO SE LLAMA LA CLASE "ConfiguracionAdmin2ViewModel.CS" DESDE
        //LA CLASE "PaginaRegistro.xaml.cs"
        protected void ConsoleWriteline(string WHO, string ValorActual)
        {
            Console.WriteLine("\n\n=============================================");
            Console.WriteLine("=============================================");
            Console.WriteLine("\nPROPIEDAD: " + WHO);
            Console.WriteLine("\n\nValor actual: " + ValorActual);
            Console.WriteLine("=============================================");
            Console.WriteLine("=============================================\n\n");
        }

        protected void NotificacionRegistro()
        {
            Console.WriteLine("\n\n=================================================");
            Console.WriteLine("=================================================");
            Console.WriteLine("\nREGISTRO EXITOSO: \n");
            Console.WriteLine("\nNombres: " + Nombres);
            Console.WriteLine("\nApellidos: " + Apellidos);
            Console.WriteLine("\nCedula: " + Cedula);
            Console.WriteLine("\nFecha de Nacimiento: " + FechaNacimiento);
            Console.WriteLine("\nTelefono: " + Telefono);
            Console.WriteLine("\nCorreo: " + Correo);
            Console.WriteLine("\n\nUsername: " + Username);
            Console.WriteLine("\nPassword: " + Password);
            Console.WriteLine("=================================================");
            Console.WriteLine("=================================================\n\n");
        }

        //===================================================================================
        //===================================================================================
        //FUNCION PARA RETORNAR UN MENSAJE POR CONSOLA
        protected void MensajeConsole(string mensaje)
        {
            Console.WriteLine("\n\n=================================================");
            Console.WriteLine("=================================================");
            Console.WriteLine("Mensaje: " + mensaje);
            Console.WriteLine("=================================================");
            Console.WriteLine("=================================================\n\n");
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