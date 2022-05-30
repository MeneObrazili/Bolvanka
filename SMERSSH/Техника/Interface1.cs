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

            Form1.PSizeMap = new Point(0, 1);
            if (aFulovayaHueta[0] != 'A' && aFulovayaHueta[1] != 'U' && aFulovayaHueta[2] != 'E')
                return;
            int x = (((int)aFulovayaHueta[7] << 24) + ((int)aFulovayaHueta[6] << 16) + ((int)aFulovayaHueta[5] << 8) + ((int)aFulovayaHueta[4]));
            int y = (((int)aFulovayaHueta[0x0B] << 24) + ((int)aFulovayaHueta[0x0A] << 16) + ((int)aFulovayaHueta[9] << 8) + ((int)aFulovayaHueta[8]));
            Form1.PSizeMap = new Point(x, y);
            int pointer = (((int)aFulovayaHueta[0x0F] << 24) + ((int)aFulovayaHueta[0x0E] << 16) + ((int)aFulovayaHueta[0x0D] << 8) + ((int)aFulovayaHueta[0x0C]));

            for (int i1 = 0; i1 < Form1.PSizeMap.Y; i1++) {
                Form1.mainmap.kvadratyi.Add(new List<tayl>());
                Form1.kartinki.Add(new List<PictureBox>());
                for (int i2 = 0; i2 < Form1.PSizeMap.X; i2++) {
                    Form1.mainmap.kvadratyi[i1].Add(new tayl(0xffffffff, (TipTayla)aFulovayaHueta[i1 * Form1.PSizeMap.X + i2 + 16]));
                    Form1.kartinki[i1].Add(I1.InitPicture(bomji[aFulovayaHueta[i1 * Form1.PSizeMap.X + i2 + 16]], new Point(i2 * Form1.PKartSize.X, i1 * Form1.PKartSize.Y), new Size(Form1.PKartSize.X, Form1.PKartSize.Y)));
                    formOchka.Controls.Add(Form1.kartinki[i1][i2]);
                }
            }

            return;
        }

        unsafe static public void ZadelkaVisuala() {
            for (int i1 = 0; i1 < Form1.PSizeMap.X; i1++) {
                for (int i2 = 0; i2 < Form1.PSizeMap.Y; i2++) {
                    Form1.kartinki[i2][i1].Visible = true;
                }
            }

            for (int i = 0; i < Form1.boysyi.Count; i++) {
                if (Form1.boysyi[i].bViden) {
                    Form1.boysyi[i].IPikcha.Visible = true;
                    Form1.kartinki[Form1.boysyi[i].PMesto.Y][Form1.boysyi[i].PMesto.X].Visible = false;
                }
            }
        }
    }
}
