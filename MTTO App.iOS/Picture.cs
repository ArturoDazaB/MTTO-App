using Foundation;
using MTTO_App.iOS;
using System;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Picture_iOS))]

namespace MTTO_App.iOS
{
    public class Picture_iOS : IPicture
    {
        public void SavePicture(string filename, byte[] imgdata)
        {
            var charImage = new UIImage(NSData.FromArray(imgdata));
            charImage.SaveToPhotosAlbum((image, error) =>
            {
                if (error != null)
                {
                    Console.WriteLine("\n========================================");
                    Console.WriteLine("========================================");
                    Console.WriteLine("\n" + error.ToString());
                    Console.WriteLine("========================================");
                    Console.WriteLine("========================================\n");
                }
            });
        }
    }
}