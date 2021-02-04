using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_TD1
{
    public class Pixel
    {
        int red;
        int green;
        int blue;
       
        public Pixel(int red, int green, int blue)
        {

            this.red = red;
            this.green = green;
            this.blue = blue;


        }

        public string ToStringPix()
        {
            string Pixel = blue+ " " + green +" "+ red; // affichage format BM BGR
            return Pixel;

        }
        
    }
}
