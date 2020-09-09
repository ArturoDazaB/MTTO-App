using System;
using System.Collections.Generic;
using System.Text;

namespace MTTO_App
{
    class OpcionesViewModel
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
