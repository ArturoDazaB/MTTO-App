using System;
using System.Collections.Generic;
using System.Text;

namespace MTTO_App
{
    public interface IPicture
    {
        void SavePicture(string filename, byte[] imgdata);
    }
}
