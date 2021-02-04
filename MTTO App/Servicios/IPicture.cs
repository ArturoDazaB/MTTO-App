namespace MTTO_App.Servicios
{
    public interface IPicture
    {
        void SavePicture(string filename, byte[] imgdata);
    }
}