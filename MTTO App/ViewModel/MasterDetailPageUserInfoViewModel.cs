using MTTO_App.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTTO_App.ViewModel
{
    class MasterDetailPageUserInfoViewModel
    {
        //===============================================================================================
        //===============================================================================================
        //CREACION DEL OBJETO MODEL DE LA CLASE "MasterDetailPageUserInfoModel.cs"
        public MasterDetailPageUserInfoModel MasterPageUserInfo;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE
        public MasterDetailPageUserInfoViewModel(Personas Persona, Usuarios Usuario, UltimaConexion ultimaconexion)
        {
            //SE INSTANCIA EL OBJETO MODEL DE LA CLASE "MasterDetailPageUserInfoModel.cs" CON LA
            //INFORMACION DEL USUARIO LOGGEADO
            MasterPageUserInfo = new MasterDetailPageUserInfoModel(Persona, Usuario, ultimaconexion);
        }

        //===============================================================================================
        //===============================================================================================
        //PROPIEDADES DE LA CLASE

        public string UserName { get { return MasterPageUserInfo.UserName; } }

        public string FullName { get { return MasterPageUserInfo.FullName; } }

        public string UltimaConexion { get { return MasterPageUserInfo.UltimaConexion; } }

        public string UserIconFile { get { return MasterPageUserInfo.UserIconFile; } }

        //===============================================================================================
        //===============================================================================================
        //PROPIEDADES DE LA PAGINA
        public int LabelFontSize { get { return App.LabelFontSize; } }
        public int EntryFontSize { get { return App.EntryFontSize; } }
        public int HeaderFontSize { get { return App.HeaderFontSize; } }
        public int SmallHeaderFontSize { get { return App.SmallHeaderFontSize; } }

        //===============================================================================================
        //===============================================================================================
    }
}
