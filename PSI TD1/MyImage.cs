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
    public class MyImage
    {
        public string myfile;
        public string typeI;
        public int tailleF;
        public int tailleO;
        public int largeurI;
        public int hauteurI;
        public int bitC;
        public int header;
        public int headerinfo;
        public Pixel[,] matRGB;

        /// <summary>
        /// Constructeur MyImage a partir du nom de l'image: myfile
        /// </summary>
        /// <param name="myfile"> string nom de l'image à ouvrir</param>
        public MyImage(string myfile)
        {
            this.myfile = myfile;

            byte[] FileByte = File.ReadAllBytes(myfile);
            byte[] tab = new byte[4];

            // initialisation des differentes variables en fonctions des différents Bytes
            // les 2 premiers octets si 66 et 77 = format bitmap
            if( FileByte[0] == 66 && FileByte[1]== 77)
            {
                typeI = "BM";
            }
            else
            {
                typeI = "Autre que BM"; 
            }

            for(int i = 2; i<6; i++)
            {
                tab[i - 2] = FileByte[i]; // taille Fichier

            }
            tailleF = Convertir_endian_to_int(tab);

            for(int i=10; i<14;i++)
            {

                tab[i - 10] = FileByte[i]; // taille offset

            }
            tailleO = Convertir_endian_to_int(tab);

                for(int i =18; i< 22; i++)
            {
                 tab[i-18] = FileByte[i];   // largeur
            }
             largeurI = Convertir_endian_to_int(tab);

            for(int i = 22; i<26; i++)
            {
                tab[i - 22] = FileByte[i]; // hauteur image
            }
            hauteurI = Convertir_endian_to_int(tab);

            for(int i = 28; i<30; i++)
            {
                tab[i - 28] = FileByte[i]; // nb bit couleur
            }
            bitC = Convertir_endian_to_int(tab);

            int compteur = 54;
            matRGB = new Pixel[hauteurI,largeurI];
            for (int i = 0; i < matRGB.GetLength(0); i++)
            {
                for (int j = 0; j < matRGB.GetLength(1); j++)
                {
                    int taille = FileByte.Length;
                    int red = FileByte[compteur];
                    int green = FileByte[compteur + 1];
                    int blue = FileByte[compteur + 2];

                    matRGB[i, j] = new Pixel(blue, green, red); // remplissage matrice de pixel BGR (blue green red car inverser);
                    compteur = compteur + 3;
                }
            }

           
        }

        /// <summary>
        /// transforme instance MyImage en fichier binaire respectant la structure BM
        /// </summary>
        /// <param name="file"> nom de l'image </param>
        public void From_image_to_file(string file)
        {
            byte[] FichierByte = new byte[3*largeurI*hauteurI + 54]; // dimension fichier

            FichierByte[0] = 66; FichierByte[1] = 77;
            // Header
            for (int i = 0; i <4; i++)
            {
                
                FichierByte[i+2] = Convertir_int_to_endian(tailleF, 4)[i];
            }
            for(int i=0; i< 4; i++)
            {
                FichierByte[i + 6] = 0;
            }
            for (int i = 0; i < 4; i++)
            {
                FichierByte[i + 10] = Convertir_int_to_endian(tailleO, 4)[i];
            }

            FichierByte[14] = 40;
            // headerInfo
            for (int i=0; i< 3; i++)
            {

                FichierByte[i + 15] = 0;

            }
            for(int i=0; i<4; i++)
            {

                FichierByte[i + 18] = Convertir_int_to_endian(largeurI, 4)[i];

            }
            for(int i=0; i<4; i++)
            {

                FichierByte[i + 22] = Convertir_int_to_endian(hauteurI, 4)[i];

            }
            FichierByte[26] = 1; FichierByte[27] = 0; FichierByte[28] = 24;

            for (int i =0; i<5; i++)
            {

                FichierByte[i + 29] = 0;

            }
            FichierByte[34] = 176;
            FichierByte[35] = 4;
            for(int i = 0; i< 18; i++)
            {

                FichierByte[i + 36] = 0;

            }

            int deb = 54;
            for(int i =0; i <hauteurI; i++)
            {
                for(int j=0; j<largeurI; j++)
                {

                    FichierByte[deb] = Convert.ToByte(matRGB[i,j].blue);
                    deb++;
                    FichierByte[deb] = Convert.ToByte(matRGB[i, j].green);
                    deb++;
                    FichierByte[deb] = Convert.ToByte(matRGB[i, j].red);
                    deb++;
                }


            }

            File.WriteAllBytes(file, FichierByte);






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

        /// <summary>
        /// Affiche Matrice RGB
        /// </summary>
        public void ToStringmatRGB()
        {
            Console.WriteLine("nom :"+  myfile + "\n");
            Console.WriteLine("Type image : " + typeI +"\n");
            Console.WriteLine("taille fichier :" + tailleF + "\n");
            Console.WriteLine("taille offset :" + tailleO + "\n");
            Console.WriteLine("largeur image :" + largeurI + "\n");
            Console.WriteLine("hauteur image :" + hauteurI + "\n");
            Console.WriteLine("nombre de bit couleur :" + bitC + "\n");

            for (int i = 0; i < matRGB.GetLength(0); i++)
            {
                for (int j = 0; j < matRGB.GetLength(1); j++)
                {
                    Console.Write(matRGB[i, j].ToStringPix() + " ");
                }
                Console.WriteLine();
            }

        }




    }
}
