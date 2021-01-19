using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace PSI_TD1
{
    class MyImage
    {
        string myfile;
        string typeI;
        int tailleF;
        int tailleO;
        int largeurI;
        int hauteurI;
        int nbbit;
        int[,] matRGB;
       


        //public MyImage (string typeI, int tailleF, int tailleO, int largeurI, int hauteurI, int nbbit, int[,] matRGB)
        //{
        //    this.typeI = typeI;
        //    this.tailleF = tailleF;
        //    this.tailleO = tailleO;
        //    this.largeurI = largeurI;
        //    this.hauteurI = hauteurI;
        //    this.nbbit = nbbit;
        //    this.matRGB = matRGB;


        //}

        public MyImage(string myfile)
        {

            byte[] FileByte = File.ReadAllBytes("myfile");
           
            //myfile est un vecteur composé d'octets représentant les métadonnées et les données de l'image

            //Métadonnées du fichier
            Console.WriteLine("\n Header \n");
            for (int i = 0; i < 14; i++)
                Console.Write(FileByte[i] + " ");
            //Métadonnées de l'image
            Console.WriteLine("\n HEADER INFO \n");
            for (int i = 14; i < 54; i++)
                Console.Write(FileByte[i] + " ");

            //mettre header dans un string puis parse dans un tableau 


            //L'image elle-même
            Console.WriteLine("\n IMAGE \n");
            for (int i = 54; i < FileByte.Length; i = i + 60)
            {
                int x = 0;
                for (int j = i; j < i + 60; j++)
                {
                    int y = 0;
                    Console.Write(FileByte[j] + " ");
                    matRGB[x, y] =
                       y++;
                }
                x++;
                Console.WriteLine();
            }

            File.WriteAllBytes("./Images/Sortie.bmp", FileByte);

            Console.ReadLine();




        }

        public void From_image_to_file(string file)
        {

        }

       public int Convertir_endian_to_int(byte[] tab)
        {


            return 0;
        }
        
        public byte[] Convertir_int_to_endian(int val)
        {
            byte[] tab = new byte[1];
            return tab;
        }




    }
}
