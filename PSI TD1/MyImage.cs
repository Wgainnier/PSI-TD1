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

namespace PSI
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
                typeI = "bmp";
            }
            else
            {
                typeI = "Autre que bmp"; 
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

                    matRGB[i, j] = new Pixel(blue, green, red); // remplissage matrice de pixel BGR (blue green red car inversé);
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
                    val = val - temp*Convert.ToInt32(Math.Pow(256, compteur));
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
        
        public void NoirBlanc()
        {
            for(int i =0; i<matRGB.GetLength(0); i++)
            {
                for(int j =0; j<matRGB.GetLength(1); j++)
                {
                    int moyenne = (matRGB[i, j].blue + matRGB[i, j].green + matRGB[i, j].red)/3;

                    if ( moyenne>=128)
                    { 
                        matRGB[i, j].blue = 255;
                        matRGB[i, j].green = 255;
                        matRGB[i, j].red = 255;
                    }
                   
                    else
                    {
                        matRGB[i, j].blue = 0;
                        matRGB[i, j].green = 0;
                        matRGB[i, j].red = 0;
                    }
                }
            }
        }

        public void NuanceGris()
        {

            for (int i = 0; i < matRGB.GetLength(0); i++)
            {
                for (int j = 0; j < matRGB.GetLength(1); j++)
                {
                    double rouge = matRGB[i, j].red ;
                    double bleu = matRGB[i, j].blue ;
                    double vert = matRGB[i, j].green ;

                    double moyenne = (rouge + bleu + vert )/ 3;

                    matRGB[i,j].red = Convert.ToInt32(moyenne);
                    matRGB[i, j].blue = Convert.ToInt32(moyenne);
                    matRGB[i, j].green = Convert.ToInt32(moyenne);

                }
            }



        }


        /// <summary>
        /// de 2 à 10
        /// </summary>
        /// <param name="multiplicateur"></param>
        public Pixel[,] Agrandir(int multiplicateur)
        {
            //int x = 0;
            //int y = 0;



            hauteurI = hauteurI * multiplicateur;
            largeurI = largeurI * multiplicateur;

            //MyImage image = new MyImage(myfile);

            Pixel[,] matRGBR = new Pixel[(matRGB.GetLength(0)*multiplicateur), (matRGB.GetLength(1)*multiplicateur)];

            Pixel[,] matdouble = new Pixel[(matRGB.GetLength(0) * multiplicateur), (matRGB.GetLength(1) * multiplicateur)];

            for(int i = 0; i<matRGBR.GetLength(0); i++)
            {
                for(int j =0; j<matRGBR.GetLength(1);j++)
                {
                    matRGBR[i, j] = new Pixel(255, 255, 255);
                }
            }

     

            for (int i = 0; i < matRGB.GetLength(0); i++)

            {
                for (int j = 0; j < matRGB.GetLength(1); j++)

                {
                    Pixel a = matRGB[i, j];
                    for(int x = i*multiplicateur; x <multiplicateur+i*multiplicateur; x++)
                    {

                        matRGBR[x, j] = a;

                    }
                    
                    


                }


            }

            for (int i = 0; i < matRGBR.GetLength(0); i++)
            {
                for (int j = 0; j < matRGBR.GetLength(1); j++)
                {
                    matdouble[i, j] = matRGBR[i,j];
                }
            }


            for (int j = 0; j < matRGB.GetLength(1); j++)

            {
                for (int i = 0; i < matdouble.GetLength(0); i++)

                {
                    Pixel a = matdouble[i, j];
                    for (int x = j * multiplicateur; x < multiplicateur + j * multiplicateur; x++)
                    {

                        matRGBR[i, x] = a;

                    }




                }


            }

            return matRGBR;

            //for (int i = 0; i < matRGBR.GetLength(0); i++)
            //{
            //    for (int j = 0; j < matRGBR.GetLength(1); j++)
            //    {
            //        matRGB[i, j] = matRGBR[i, j];


            //    }


            //}



        }

        /// <summary>
        /// retrecit de 0 à 50%
        /// </summary>
        public void retrecir(double coeff)
        {
            int x = 0;
            int y = 0;

            int nbcol= Convert.ToInt32(matRGB.GetLength(1) * coeff);
            int intervC = matRGB.GetLength(1) / nbcol;

            int nbligne = Convert.ToInt32(matRGB.GetLength(0) * coeff);
            int intervL = matRGB.GetLength(0) / nbligne;

            Pixel[,] matRGBR = new Pixel[(matRGB.GetLength(0) -nbligne), (matRGB.GetLength(1) -nbcol)];



            for(int i =0; i< matRGBR.GetLength(0); i++)
            {
                for(int j =0; j<matRGBR.GetLength(1); j++)
                {

                    matRGBR[i, j] = new Pixel(255, 255, 255);

                }
            }


            for (int i = 0; i < matRGB.GetLength(0) ; i++)
            {
                if (i % intervL != 0 )
                {
                    for (int j = 0; j < matRGB.GetLength(1) ; j++)
                    {
                        if (j % intervC != 0)
                        {
                            matRGBR[x, y] = matRGB[i, j];

                            y++;
                        }
                    }
                    y = 0;
                    x++;
                }
            }



            for (int i = 0; i < matRGB.GetLength(0); i++)
            {
                for (int j = 0; j < matRGB.GetLength(1); j++)
                {
                    matRGB[i, j] = new Pixel(255, 255, 255);


                }


            }

            for (int i = 0; i < matRGBR.GetLength(0) ; i++)
            {
                for (int j = 0; j < matRGBR.GetLength(1); j++)
                {
                    matRGB[i, j] = matRGBR[i, j];


                }


            }


            //Pixel[,] matRGBR =  new Pixel[(matRGB.GetLength(0)/2) ,(matRGB.GetLength(1)/2)];

            //for (int i = 0; i < matRGB.GetLength(0); i= i+2)
            //{

            //    for (int j = 0; j < matRGB.GetLength(1) - 1; j = j+2)
            //    {
            //        matRGBR[x, y] = matRGB[i,j];
            //        y++;
            //    }
            //    y = 0;
            //    x++;
            //}



            //for (int i = 0; i < matRGBR.GetLength(0); i++)
            //{
            //    for (int j = 0; j < matRGBR.GetLength(1); j++)
            //    {
            //        matRGB[i, j] = matRGBR[i, j];
            //    }
            //}

            //for (int i = 0; i < matRGB.GetLength(0); i = i + 2)
            //{

            //    for (int j = 0; j < matRGB.GetLength(1) - 1; j = j + 2)
            //    {
            //        matRGBR[x, y] = matRGB[i, j];
            //        y++;
            //    }
            //    y = 0;
            //    x++;
            //}

        }

       
        public Pixel[,] Rotation(int angle) // il faut centrer l'image (i-hauteur de l'image divisée par deux ) pareil pour la largeur
        {
          
            hauteurI = hauteurI * 4;
            largeurI = largeurI * 4;
            int centrex = hauteurI / 2;
            int centrey = largeurI / 2;
            int a = 0;
            int b = 0;
            double anglerad = Convert.ToDouble(angle * (3.14 / 180));
            int x = 0;
            int y = 0;

            Pixel[,] matRGBR = new Pixel[hauteurI, largeurI];


            for (int i = 0; i < matRGBR.GetLength(0); i++)
            {
                for (int j = 0; j < matRGBR.GetLength(1); j++)
                {
                    matRGBR[i, j] = new Pixel(255, 255, 255);
                }
            }

            for (int i = centrex - matRGB.GetLength(0) / 2; i < centrex + matRGB.GetLength(0) / 2; i++)
            {
                for (int j = centrey - matRGB.GetLength(1) / 2; j < centrey + matRGB.GetLength(1) / 2; j++)
                {
                    x = Convert.ToInt32((i - centrex) * Math.Cos(anglerad) + (j - centrey) * Math.Sin(anglerad) + centrex);
                    y = Convert.ToInt32(-(i - centrex) * Math.Sin(anglerad) + (j - centrey) * Math.Cos(anglerad) + centrey);

                    //if(x %1 !=0)
                    //{
                    //    x= Convert.ToInt32(x);
                    //    y = Convert.ToInt32(y);
                    //}


                    matRGBR[x, y] = matRGB[a, b];
                    b++;
                }
                b = 0;
                a++;
            }

            return matRGBR;


        }
        public void MiroirV() // par vertical
        {
            int x = matRGB.GetLength(0) -1;
            int y = matRGB.GetLength(1) - 1;

            Pixel[,] matRGBR = new Pixel[(matRGB.GetLength(0) ), matRGB.GetLength(1)];


            for(int i =0; i< matRGB.GetLength(0); i ++)
            {
                for(int j =0; j < matRGB.GetLength(1); j++)
                {
                    matRGBR[i, j] = matRGB[i, j];


                }


            }

            for(int col = 0; col< matRGB.GetLength(1); col++)
            {
                for(int ligne =0; ligne < matRGB.GetLength(0); ligne++)
                {
                    matRGB[ligne, col] = matRGBR[ligne, y-col];
                    matRGB[ligne, y - col] = matRGBR[ligne, col];


                }


            }

        }

        public void MiroirH() // par horizontal
        {
            int x = matRGB.GetLength(0) - 1;
            int y = matRGB.GetLength(1) - 1;

            Pixel[,] matRGBR = new Pixel[(matRGB.GetLength(0)), matRGB.GetLength(1)];

            
            for (int i = 0; i<matRGB.GetLength(0); i++)
            {
                for (int j = 0; j<matRGB.GetLength(1); j++)
                {
                    matRGBR[i, j] = matRGB[i, j];


                }


            }

            for (int i = 0; i < matRGB.GetLength(0); i++)
{
                for (int j = 0; j < matRGB.GetLength(1); j++)
                {
                     matRGB[i, j] = matRGBR[x - i, y - j];
                      matRGB[x - i, y - j] = matRGBR[i, j];


                 }


            }
            
        }
        
        public void MatriceConvolution(int[,] NoyauM)
        {
            int AccumRed = 0;
            int AccumGreen = 0;
            int AccumBlue = 0;
            int somme=0;
            int diviseur = 0;
            Pixel[,] MatriceRGBcopie = new Pixel[matRGB.GetLength(0), matRGB.GetLength(1)];

            for(int i =0; i<MatriceRGBcopie.GetLength(0); i++)
            {
                for(int j =0; j<MatriceRGBcopie.GetLength(1); j++)
                {

                    MatriceRGBcopie[i, j] = new Pixel(255, 255, 255);
        
                }
            }

            for (int i = 0; i < NoyauM.GetLength(0); i++)
            {
                for (int j = 0; j < NoyauM.GetLength(1); j++)
                {

                    somme = somme + NoyauM[i, j];

                }
            }

            if(somme >0)
            {
                diviseur = somme;

            }
            else
            {
                diviseur = 1;
            }

            
                for (int i = 1; i < matRGB.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < matRGB.GetLength(1) - 1; j++) // parcours toute la matrice 
                    {

                        AccumRed = 0;
                        AccumGreen = 0;
                        AccumBlue = 0;
                        int LigneN = 0;
                        int ColonneN = 0;

                        //if (i == 0 && j==0)
                        //{
                        //    AccumRed = matRGB[i, j].red * NoyauM[0, 0] + matRGB[i,j].red * NoyauM[0,1] + matRGB[i,j].red* NoyauM[0,2] + matRGB[i,j].red * NoyauM[1,0] + matRGB[i,j].red * NoyauM[2,0];




                        //    AccumRed = AccumRed + matRGB[a, b].red * NoyauM[LigneN, ColonneN];
                        //    AccumGreen = AccumGreen + matRGB[a, b].green * NoyauM[LigneN, ColonneN];
                        //    AccumBlue = AccumBlue + matRGB[a, b].blue * NoyauM[LigneN, ColonneN];


                        //}
                        //if (i == 0 && j != 0 && j!= matRGB.GetLength(1))
                        //{
                        //    for(int a = j-1; a< j+2; a++)
                        //    {
                        //       AccumRed = AccumRed + matRGB[matRGB.GetLength(0)-1,a].red * NoyauM[0, ColonneN];
                        //       ColonneN++;

                        //    }
                        //}


                        //#region Cas coin supérieur gauche de l'image
                        //if(i==0 && j==0)
                        //{ 
                        //    int valueRED=matRGB[i,j].red;
                        //    int valueGREEN=matRGB[i,j].green;
                        //    int valueBLUE=matRGB[i,j].blue;
                        //    LigneN=1;
                        //    ColonneN=1;

                        //    for (int a = 0; a < 3; a++)
                        //    {
                        //        for (int b = 0; b <3; b++) //parcours le tour du point selectionné
                        //        {
                        //            if(a<1 && b<1)
                        //            {
                        //                AccumRed = AccumRed + valueRED* NoyauM[a, b];
                        //                AccumGreen = AccumGreen + valueGREEN* NoyauM[a, b];
                        //                AccumBlue = AccumBlue + valueBLUE* NoyauM[a, b];
                        //            }
                        //            else if((a==2) && (b==0))
                        //            {
                        //                valueRED=matRGB[i+1,j];
                        //                valuegGREEN=matRGB[i+1,j];
                        //                valueBLUE=matRGB[i+1,j];
                        //                AccumRed = AccumRed + valueRED* NoyauM[a, b];
                        //                AccumGreen = AccumGreen + valueGREEN* NoyauM[a, b];
                        //                AccumBlue = AccumBlue + valueBLUE* NoyauM[a, b];
                        //            }
                        //            else if((a==0) && (b==2))
                        //            { 
                        //                valueRED=matRGB[i,j+1];
                        //                valuegGREEN=matRGB[i,j+1];
                        //                valueBLUE=matRGB[i,j+1];
                        //                AccumRed = AccumRed + valueRED* NoyauM[a, b];
                        //                AccumGreen = AccumGreen + valueGREEN* NoyauM[a, b];
                        //                AccumBlue = AccumBlue + valueBLUE* NoyauM[a, b];
                        //            }
                        //            else
                        //            { 
                        //                AccumRed = AccumRed + matRGB[a, b].red * NoyauM[LigneN, ColonneN];
                        //                AccumGreen = AccumGreen + matRGB[a, b].green * NoyauM[LigneN, ColonneN];
                        //                AccumBlue = AccumBlue + matRGB[a, b].blue * NoyauM[LigneN, ColonneN];
                        //            }
                        //            ColonneN++;
                        //        }
                        //        LigneN++;
                        //    }
                        //}
                        //#endregion

                        #region Cas général
                        for (int a = i - 1; a < i + 2; a++)
                        {
                            for (int b = j - 1; b < j + 2; b++) //parcours le tour du point selectionné
                            {
                                AccumRed = AccumRed + matRGB[a, b].red * NoyauM[LigneN, ColonneN];
                                AccumGreen = AccumGreen + matRGB[a, b].green * NoyauM[LigneN, ColonneN];
                                AccumBlue = AccumBlue + matRGB[a, b].blue * NoyauM[LigneN, ColonneN];
                                ColonneN++;
                            }

                            ColonneN = 0;
                            LigneN++;
                        }
                        #endregion

                        if (AccumRed / diviseur > 255)
                        {
                        MatriceRGBcopie[i, j].red = 255;
                        }   
                        else if (AccumRed/diviseur <0)
                        {
                        MatriceRGBcopie[i, j].red = 0;
                        }
                        else 
                        {
                        MatriceRGBcopie[i, j].red = AccumRed / diviseur;
                        }

                    if (AccumGreen / diviseur > 255)
                    {
                        MatriceRGBcopie[i, j].green = 255;
                    }
                    else if (AccumGreen / diviseur < 0)
                    {
                        MatriceRGBcopie[i, j].green = 0;
                    }
                    else
                    {
                        MatriceRGBcopie[i, j].green = AccumGreen / diviseur;
                    }

                    if (AccumBlue / diviseur > 255)
                    {
                        MatriceRGBcopie[i, j].blue = 255;
                    }
                    else if (AccumBlue / diviseur < 0)
                    {
                        MatriceRGBcopie[i, j].blue = 0;
                    }
                    else
                    {
                        MatriceRGBcopie[i, j].blue = AccumBlue / diviseur;
                    }
                    }
                }


                for (int i = 0; i < matRGB.GetLength(0); i++)
                {
                    for (int j = 0; j < matRGB.GetLength(1); j++)
                    {
                        matRGB[i, j].red = MatriceRGBcopie[i, j].red;
                        matRGB[i, j].green = MatriceRGBcopie[i, j].green;
                        matRGB[i, j].blue = MatriceRGBcopie[i, j].blue;
                    }
                }





            
        }

    }
}
