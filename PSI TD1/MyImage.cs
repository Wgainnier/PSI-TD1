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
            this.myfile = myfile;

            byte[] FileByte = File.ReadAllBytes(myfile);   

            //myfile est un vecteur composé d'octets représentant les métadonnées et les données de l'image

            //Métadonnées du fichier
            Console.WriteLine("\n Header \n");

            for (int i = 0; i < 14; i++)
            {
                myfile += FileByte[i] + " ";
            }
            //Métadonnées de l'image
            Console.WriteLine("\n HEADER INFO \n");

            myfile = myfile + "\n";
            for (int i = 14; i < 54; i++)
            {
                myfile += FileByte[i] + " ";
            }
            myfile = myfile + "\n";

            //mettre header dans un string puis parse dans un tableau 

            int largeur = FileByte[18] + FileByte[19] * 2 ^ 8 + FileByte[20] * 2 ^ 16 + FileByte[21] * 2 ^ 24;
            int longueur = FileByte[22] + FileByte[23] * 2 ^ 8 + FileByte[24] * 2 ^ 16 + FileByte[25] * 2 ^ 24;




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

        /// <summary>
        /// permet de convertir une séquence d'octet au format little endian en entier
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>

       public int Convertir_endian_to_int(byte[] tab)
        {
            int endianint = 0;
           
            for(int i =0; i< tab.Length; i++ )
            {

                endianint = endianint + tab[i] * Convert.ToInt32(Math.Pow(256, i));

            }

            return endianint;
        }

        /// <summary>
        /// permet de convertir un entier en séquence d'octets au format little endian
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        
        public byte[] Convertir_int_to_endian(int val, int bit)
        {
            int compteur = bit-1;
            byte[] tab = new byte[bit];
            byte temp = 0;

            for(int i =0; i<bit; i++)
            {
                temp = Convert.ToByte(val / Convert.ToInt32(Math.Pow(256, compteur)));
                if (temp >= 1)
                {
                    val = val - 256 * temp;
                    tab[i] = temp;
                }
                else tab[i] = 0;
                compteur--;

            }
            Array.Reverse(tab);
            return tab;
        }




    }
}
