using System.Collections.Generic;

namespace MTTO_App
{
    internal class QueryAdminModel
    {
        //TITULO DE LA SECCION DE BUSQUEDA
        public string Titulo { get; set; }

        //LISTA CON TODAS LAS OPCIONES DE BUSQUEDA DISPONIBLE
        public List<string> Opciones { get; set; }

        public List<QueryAdminModel> GetConfiguracion()
        {
            //SE INSTANCIA LA LISTA
            List<QueryAdminModel> Lista = new List<QueryAdminModel>();

            //SE LLENA LA LISTA CON LOS VALORES NECESARIOS
            Lista.Add
                (new QueryAdminModel()
                {
                    Titulo = "Busqueda",
                    Opciones = new List<string>() { "ID", "Numero de Ficha", "Nombre", "Apellido", "Usuario", },
                });

            //SE RETORNA LA LISTA
            return Lista;
        }
    }
}