using System.Collections.Generic;

namespace MTTO_App
{
    internal class OpcionesViewModel
    {
        //=================================================================
        //=================================================================
        //PROPIEDADES DE LA CLASE

        public List<Opciones> OpcionesNivelAlto
        {
            get { return new Opciones().OpcionesNivelAlto(); }
        }

        public List<Opciones> OpcionesNivelMedio
        {
            get { return new Opciones().OpcionesNivelMedio(); }
        }

        public List<Opciones> OpcionesNivelBajo
        {
            get { return new Opciones().OpcionesNivelBajo(); }
        }
    }
}