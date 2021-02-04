using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PSI_TD1
{
    class Program
    {
        static void Main(string[] args)
        {

            #region TD1
            //Bitmap lena = new Bitmap("lena.bmp");
            ///*
            //Bitmap coco1 = new Bitmap("coco.bmp");
            //coco1.RotateFlip(RotateFlipType.Rotate180FlipNone);
            //coco1.Save("cococopie.bmp");
            //*/

            ///*RectangleF rec = new RectangleF(10.0f, 10.0f, 100.0f, 100.0f);
            //Bitmap l = lena.Clone(rec, PixelFormat.DontCare);
            //l.Save("lenarec.bmp");*/ // A dé commenter pour avoir une sous partie de l'image

            //for (int i = 0; i < lena.Height; i++)
            //    for (int j = 0; j < lena.Width; j++)
            //    {
            //        Color mycolor = lena.GetPixel(i, j);
            //        lena.SetPixel(i, j, Color.FromArgb(255 - mycolor.R, 255 - mycolor.R, 255 - mycolor.R));

            //        //   c.SetPixel(i, j, Color.Coral);
            //    }

            //lena.Save("lenanegative.bmp");
            //Process.Start("lenanegative.bmp");
            //Console.ReadLine();

            #endregion TD1 

            #region TestAffichage
            //MyImage a = new MyImage("Test.bmp");
            //a.ToStringmatRGB();

            //Bitmap Test = new Bitmap("Test.bmp");
            //Test.Save("TestImFile.bmp");


            //MyImage b = new MyImage("TestImFile.bmp");
            //b.From_image_to_file("TestImFile.bmp");
            //Process.Start("TestImFile.bmp");
            //Console.ReadLine();

            #endregion TestAffichage

            #region TraiterImage

            //MyImage lena = new MyImage("lena.bmp");

            //lena.NoirBlanc();

            //lena.From_image_to_file("lena.bmp");

            //Process.Start("lena.bmp");

            #endregion TraiterImage

            #region NuanceGris
            
            

            Bitmap lena = new Bitmap("lena.bmp");
            lena.Save("lena1.bmp");
            

            MyImage lena1 = new MyImage("lena1.bmp");


            lena1.NuanceGris();

            lena1.From_image_to_file("lena1.bmp");



            Process.Start("lena1.bmp");
            Console.ReadLine();


            #endregion Nuance Gris

        }
    }
}
