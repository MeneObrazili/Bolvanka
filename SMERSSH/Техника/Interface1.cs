using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SMERSSH {
    unsafe class I1 {
        unsafe static public PictureBox InitPicture(string sFileName, Point PLV, Size PRazmer) {
            PictureBox buffer = new PictureBox();
            buffer.SizeMode = PictureBoxSizeMode.StretchImage;
            buffer.Location = PLV;
            buffer.ClientSize = PRazmer;
            buffer.Image = (Image)(new Bitmap(sFileName));
            return buffer;
        }

        static public string[] bomji = { "Zemlya.bmp", "Stena.bmp",  };
        unsafe static public void ChtenieBlyaMapyi(Form1 formOchka) {
            byte[] aFulovayaHueta;
            FileStream File = new FileStream("output.bin", FileMode.Open);
            aFulovayaHueta = new byte[File.Length];
            File.Read(aFulovayaHueta, 0, (int)File.Length);
            File.Close();

            formOchka.PSizeMap = new Point(0, 1);
            if (aFulovayaHueta[0] != 'A' && aFulovayaHueta[1] != 'U' && aFulovayaHueta[2] != 'E')
                return;
            int x = (((int)aFulovayaHueta[7] << 24) + ((int)aFulovayaHueta[6] << 16) + ((int)aFulovayaHueta[5] << 8) + ((int)aFulovayaHueta[4]));
            int y = (((int)aFulovayaHueta[0x0B] << 24) + ((int)aFulovayaHueta[0x0A] << 16) + ((int)aFulovayaHueta[9] << 8) + ((int)aFulovayaHueta[8]));
            formOchka.PSizeMap = new Point(x, y);
            int pointer = (((int)aFulovayaHueta[0x0F] << 24) + ((int)aFulovayaHueta[0x0E] << 16) + ((int)aFulovayaHueta[0x0D] << 8) + ((int)aFulovayaHueta[0x0C]));

            for (int i1 = 0; i1 < formOchka.PSizeMap.Y; i1++) {
                formOchka.Cmainmap.kvadratyi.Add(new List<tayl>());
                formOchka.PBMatrPict.Add(new List<PictureBox>());
                for (int i2 = 0; i2 < formOchka.PSizeMap.X; i2++) {
                    formOchka.Cmainmap.kvadratyi[i1].Add(new tayl(0xffffffff, (TipTayla)aFulovayaHueta[i1 * formOchka.PSizeMap.X + i2 + 16]));
                    formOchka.PBMatrPict[i1].Add(I1.InitPicture(bomji[aFulovayaHueta[i1 * formOchka.PSizeMap.X + i2 + 16]], new Point(i2 * formOchka.PKartSize.X, i1 * formOchka.PKartSize.Y), new Size(formOchka.PKartSize.X, formOchka.PKartSize.Y)));
                    formOchka.Controls.Add(formOchka.PBMatrPict[i1][i2]);
                    
                }
            }

            return;
        }



        unsafe static public void ZadelkaVisuala(Form1 formOchka) {
            for (int i1 = 0; i1 < formOchka.PSizeMap.X; i1++) {
                for (int i2 = 0; i2 < formOchka.PSizeMap.Y; i2++) {
                    formOchka.PBMatrPict[i2][i1].Visible = true;
                }
            }

            for (int i = 0; i < formOchka.boysyi.Count; i++) {
                if (formOchka.boysyi[i].bViden) {
                    formOchka.boysyi[i].IPikcha.Visible = true;
                    formOchka.PBMatrPict[formOchka.boysyi[i].PMesto.Y][formOchka.boysyi[i].PMesto.X].Visible = false;
                }
            }
        }
    }
}
