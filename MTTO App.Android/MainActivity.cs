using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using System.IO;

namespace MTTO_App.Droid
{
    [Activity(Label = "MTTOApp",
               Icon = "@mipmap/icon",
               Theme = "@style/MainTheme",
               MainLauncher = true,
               ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //==============================================================================================
            //==============================================================================================
            //SE VERIFICA SI LOS PERMISOS DE LECTURA Y ESCRITURA ESTAN HABILITADOS
            //DE NO ESTAR HABILITADOS, SE HABILITAN MANUALMENTE

            //PERMISO PARA ESCRITURA
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 0);
            }

            //PERMISO PARA LECTURA
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 0);
            }

            //==============================================================================================
            //==============================================================================================
            //INICIALIZACION DE LA BASE DE DATOS
            string fileName = "DB4_2_5_1.db3";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string CompletePath = Path.Combine(folderPath, fileName);
            LoadApplication(new App(CompletePath));

            //==============================================================================================
            //==============================================================================================
            //INICIALIZACION DE Rg.Plugins.Popup
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            //==============================================================================================
            //==============================================================================================
            //INICIALIZACION DEL PLUGIN Zxing EN ANDROID
            Xamarin.Essentials.Platform.Init(Application);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            //==============================================================================================
            //==============================================================================================

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /*public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }*/

        //==========================================================================================================================
        //==========================================================================================================================
    }
}