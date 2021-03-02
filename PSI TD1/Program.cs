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

namespace PSI
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
            
            Bitmap lena = new Bitmap("coco.bmp");
            lena.Save("coco1.bmp");
            MyImage lena1 = new MyImage("coco1.bmp");


            Menu1(lena1);


            

            lena1.From_image_to_file("coco1.bmp");
            Process.Start("coco1.bmp");
            Console.ReadLine();


            #endregion Nuance Gris
            
        }




        static void Menu1(MyImage a)
        {
            int choix = 0;

            Console.Write("Que Voulez vous faire sur l'image");
            Console.WriteLine("\n1 - Traitement d'Image");
            Console.WriteLine("2 - Matrice Convolution");
            Console.WriteLine("3 - QR Code");
            Console.WriteLine("4 - ");
            choix = Convert.ToInt32(Console.ReadLine());


            switch (choix)
            {
                case 1:
                    SousMenu1(a);

                    break;


                default:
                    break;


            }
        }


        static void SousMenu1(MyImage a)
        {
            int choix = 0;

            Console.Write("Que Voulez vous faire sur l'image");
            Console.WriteLine("1 - Noir et Blanc");
            Console.WriteLine("2 - Nuance de gris");
            Console.WriteLine("3 - Miroir par horizontal");
            Console.WriteLine("4 - Miroir par Vertical");
            Console.WriteLine("5 - Rotation Image");
            Console.WriteLine("6 - Agrandir Image");
            Console.WriteLine("7 - Retrecir Image");
            choix = Convert.ToInt32(Console.ReadLine());
            switch (choix)
            {
                case 1:
                    a.NoirBlanc();

                    break;
                case 2:
                    a.NuanceGris();

                    break;
                case 3:
                   a.Miroir();

                    break;
                case 4:
                    a.Miroir();

                    break;
                case 5:
                    a.Rotation();

                    break;
                case 6:
                    a.Agrandir();

                    break;
                case 7:
                    a.retrecir();

                    break;

                default:
                    break;


            }


        }


    }
}
