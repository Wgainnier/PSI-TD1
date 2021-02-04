using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Media;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;


namespace PSI_TD1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConvertEndiantoInt()
        {
            byte[] tab = { 230, 4, 0, 0 };

            MyImage a = new MyImage("lena.bmp");
           
            int result = a.Convertir_endian_to_int(tab);

            Assert.AreEqual( 1254, result);

        }
       
        [TestMethod]
        public void ConvertinttoEndian()
        {

            MyImage a = new MyImage("lena.bmp");

            byte[] result = a.Convertir_int_to_endian(1254, 4);
            byte[] tab = { 230, 4, 0, 0 };
            Assert.AreEqual(tab[0], result[0]);
            Assert.AreEqual(tab[1], result[1]);
            Assert.AreEqual(tab[2], result[2]);
            Assert.AreEqual(tab[3], result[3]);

        }


    }
}
