using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SMERSSH {
    public enum TipBoyca : int {
        Strelok,
        Svyaznoy,
        Tank,
        Sniper
    }
    public enum NaprVzglyada {
        Vverh, Vlevo, Vniz, Vpravo
    }

    unsafe public class Boycuha {
        public static int[] adwHps = { 5, 4, 10, 2 };
        public static int[] adwDmg = { 2, 2, 1, 4 };
        public static int[] adwDal = { 2, 3, 2, 5 };
        public int          nHP;
        public TipBoyca     tip;

        public uint         dwID;
        public bool         bViden;
        public bool         bNash;
        public Point        PMesto; 
        public PictureBox   IPikcha;
        public NaprVzglyada vzglyad;
        public List<uint>   aVidimyie;

        public Boycuha() { }
        // Strelok:     5 hp,   2 dmg, 2 dal
        // Svyaznoy:    4 hp,   2 dmg, 3 dal
        // Tank:        10 hp,  1 dmg, 2 dal
        // Sniper:      2 hp,   4 dmg, 5 dal
        public Boycuha(Point Pmesto, TipBoyca tipok, bool nash_li, uint dwId,  Form1 froma) {
            this.PMesto = Pmesto;
            this.IPikcha = I1.InitPicture(Form1.FilesBoys[(int)tipok], new Point(this.PMesto.X * Form1.PKartSize.X, this.PMesto.Y * Form1.PKartSize.Y), (Size)Form1.PKartSize);
            this.IPikcha.MouseDown += Form1.MouseDown_send;
            if (nash_li)
                this.IPikcha.Visible = true;
            this.tip = tipok;
            this.bViden = nash_li;
            this.bNash = nash_li;
            this.nHP = adwHps[(int)tipok];//(tipok == TipBoyca.Strelok) ? 5 : (tipok == TipBoyca.Svyaznoy) ? 4 : (tipok == TipBoyca.Tank) ? 10 : 2;
            this.dwID = dwId;
            this.aVidimyie = new List<uint>();
            froma.Controls.Add(this.IPikcha);
        }

        public void Dvijenie(Point dlina) {
            if (dlina.X + PMesto.X < 0 || dlina.Y + PMesto.Y < 0 || dlina.X + PMesto.X > Form1.PSizeMap.X - 1 || dlina.Y + PMesto.Y > Form1.PSizeMap.Y - 1)
                throw new Exception("Error");
            //Form1.mainmap.kvadratyi[PMesto.Y][PMesto.X] = new tayl(0xffffffff, Form1.mainmap.kvadratyi[PMesto.Y][PMesto.X].tip);
            Form1.kartinki[PMesto.Y][PMesto.X].Visible = true;
            PMesto.X += dlina.X;
            PMesto.Y += dlina.Y;
            this.IPikcha.Location = new Point(PMesto.X * Form1.PKartSize.X, PMesto.Y * Form1.PKartSize.Y);
            vzglyad = (Math.Abs(dlina.X) >= Math.Abs(dlina.Y)) ? ((dlina.X < 0) ? NaprVzglyada.Vlevo : NaprVzglyada.Vpravo) : ((dlina.Y < 0) ? NaprVzglyada.Vverh : NaprVzglyada.Vniz);
        }

        public void Prosmotr() {
            if (aVidimyie.Count != 0)
                aVidimyie.Clear();
            switch (vzglyad) {
                case NaprVzglyada.Vverh:
                    for (int iY = PMesto.Y - 1; iY >= -adwDal[(int)tip] + PMesto.Y; iY--) {
                        if (iY < 0)
                            break;
                        for (int iX = PMesto.X - PMesto.Y + iY; iX <= PMesto.X + PMesto.Y - iY; iX++) {
                            if (!(iX < 0 || iX >= Form1.PSizeMap.X) && Form1.mainmap.kvadratyi[iY][iX].dwIDBoyca != 0xffffffff) {
                                aVidimyie.Add(Form1.mainmap.RetId(iY, iX));
                            }
                        }
                    }
                    break;
                case NaprVzglyada.Vlevo:
                    for (int iX = PMesto.X - 1; iX >= -adwDal[(int)tip] + PMesto.X; iX--) {
                        if (iX < 0)
                            break;
                        for (int iY = PMesto.Y - PMesto.X + iX; iY <= PMesto.Y + PMesto.X - iX; iY++) {
                            if (!(iY < 0 || iY >= Form1.PSizeMap.Y) && Form1.mainmap.kvadratyi[iY][iX].dwIDBoyca != 0xffffffff) {
                                aVidimyie.Add(Form1.mainmap.RetId(iY, iX));
                            }
                        }
                    }
                    break;
                case NaprVzglyada.Vniz:
                    for (int iY = PMesto.Y + 1; iY <= adwDal[(int)tip] + PMesto.Y; iY++) {
                        if (iY >= Form1.PSizeMap.Y)
                            break;
                        for (int iX = PMesto.X + PMesto.Y - iY; iX <= PMesto.X + iY - PMesto.Y; iX++) {
                            if (!(iX < 0 || iX >= Form1.PSizeMap.X) && Form1.mainmap.kvadratyi[iY][iX].dwIDBoyca != 0xffffffff) {
                                aVidimyie.Add(Form1.mainmap.RetId(iY, iX));
                            }
                        }
                    }
                    break;
                case NaprVzglyada.Vpravo:
                    for (int iX = PMesto.X + 1; iX <= adwDal[(int)tip] + PMesto.X; iX++) {
                        if (iX > Form1.PSizeMap.Y)
                            break;
                        for (int iY = PMesto.Y + PMesto.X - iX; iY <= PMesto.Y + iX - PMesto.X; iY++) {
                            if (!(iY < 0 || iY >= Form1.PSizeMap.Y) && Form1.mainmap.kvadratyi[iY][iX].dwIDBoyca != 0xffffffff) {
                                aVidimyie.Add(Form1.mainmap.RetId(iY, iX));
                            }
                        }
                    }
                    break;
            }
        }

        public void Vistrel(Point mesto) {
            if (Form1.boysyi[(int)Form1.mainmap.RetId(mesto.Y, mesto.X)].nHP - adwDmg[(int)this.tip] >= 0)
                Form1.boysyi[(int)Form1.mainmap.RetId(mesto.Y, mesto.X)].nHP -= adwDmg[(int)this.tip];
            else Form1.boysyi[(int)Form1.mainmap.RetId(mesto.Y, mesto.X)].nHP = 0;
        }

        public bool alive() {
            return nHP > 0;
        }
    }
}
