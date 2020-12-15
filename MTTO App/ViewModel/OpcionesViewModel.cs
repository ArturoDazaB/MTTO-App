using System.Collections.Generic;

namespace MTTO_App
{
    internal class OpcionesViewModel
    {
        //============================================================================================================
        //============================================================================================================
        //NOTA: ESTA CLASE FUE DISE;ADA PARA 

        //=================================================================
        //=================================================================
        //PROPIEDADES DE LA CLASE

        public List<OpcionesModel> OpcionesNivelAlto
        {
            get { return new OpcionesModel().OpcionesNivelAlto(); }
        }

        public List<OpcionesModel> OpcionesNivelMedio
        {
            get { return new OpcionesModel().OpcionesNivelMedio(); }
        }

        public List<OpcionesModel> OpcionesNivelBajo
        {
            get { return new OpcionesModel().OpcionesNivelBajo(); }
        }
    }
}