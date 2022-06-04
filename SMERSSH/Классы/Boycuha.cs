using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SMERSSH {
    unsafe partial class Form1 {
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
            public static Form1 form1;
            public static int[] adwHps = { 5, 4, 10, 2 };
            public static int[] adwDmg = { 2, 2, 1, 4 };
            public static int[] adwDal = { 2, 3, 2, 5 };
            public int nHP;
            public TipBoyca tip;

            public uint dwID;
            public bool bViden;
            public bool bNash;
            public Point PMesto;
            public PictureBox IPikcha;
            public NaprVzglyada vzglyad;
            public List<uint> aVidimyie;

            public Boycuha() { }
            // Strelok:     5 hp,   2 dmg, 2 dal
            // Svyaznoy:    4 hp,   2 dmg, 3 dal
            // Tank:        10 hp,  1 dmg, 2 dal
            // Sniper:      2 hp,   4 dmg, 5 dal
            public Boycuha(Point Pmesto, TipBoyca tipok, bool nash_li, uint dwId, Form1 froma) {
                this.PMesto = Pmesto;
                this.IPikcha = I1.InitPicture(Form1.FilesBoys[(int)tipok], new Point(this.PMesto.X * form1.PKartSize.X, this.PMesto.Y * form1.PKartSize.Y), (Size)form1.PKartSize);
                this.IPikcha.MouseDown += form1.MouseDown_send;
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
                if (dlina.X + PMesto.X < 0 || dlina.Y + PMesto.Y < 0 || dlina.X + PMesto.X > form1.PSizeMap.X - 1 || dlina.Y + PMesto.Y > form1.PSizeMap.Y - 1)
                    throw new Exception("Error");
                PMesto.X += dlina.X;
                PMesto.Y += dlina.Y;
                this.IPikcha.Location = new Point(PMesto.X * form1.PKartSize.X, PMesto.Y * form1.PKartSize.Y);
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
                                if (!(iX < 0 || iX >= form1.PSizeMap.X) && form1.Cmainmap.kvadratyi[iY][iX].dwIDBoyca != 0xffffffff) {
                                    aVidimyie.Add(form1.Cmainmap.RetId(iY, iX));
                                }
                            }
                        }
                        break;
                    case NaprVzglyada.Vlevo:
                        for (int iX = PMesto.X - 1; iX >= -adwDal[(int)tip] + PMesto.X; iX--) {
                            if (iX < 0)
                                break;
                            for (int iY = PMesto.Y - PMesto.X + iX; iY <= PMesto.Y + PMesto.X - iX; iY++) {
                                if (!(iY < 0 || iY >= form1.PSizeMap.Y) && form1.Cmainmap.kvadratyi[iY][iX].dwIDBoyca != 0xffffffff) {
                                    aVidimyie.Add(form1.Cmainmap.RetId(iY, iX));
                                }
                            }
                        }
                        break;
                    case NaprVzglyada.Vniz:
                        for (int iY = PMesto.Y + 1; iY <= adwDal[(int)tip] + PMesto.Y; iY++) {
                            if (iY >= form1.PSizeMap.Y)
                                break;
                            for (int iX = PMesto.X + PMesto.Y - iY; iX <= PMesto.X + iY - PMesto.Y; iX++) {
                                if (!(iX < 0 || iX >= form1.PSizeMap.X) && form1.Cmainmap.kvadratyi[iY][iX].dwIDBoyca != 0xffffffff) {
                                    aVidimyie.Add(form1.Cmainmap.RetId(iY, iX));
                                }
                            }
                        }
                        break;
                    case NaprVzglyada.Vpravo:
                        for (int iX = PMesto.X + 1; iX <= adwDal[(int)tip] + PMesto.X; iX++) {
                            if (iX > form1.PSizeMap.Y)
                                break;
                            for (int iY = PMesto.Y + PMesto.X - iX; iY <= PMesto.Y + iX - PMesto.X; iY++) {
                                if (!(iY < 0 || iY >= form1.PSizeMap.Y) && form1.Cmainmap.kvadratyi[iY][iX].dwIDBoyca != 0xffffffff) {
                                    aVidimyie.Add(form1.Cmainmap.RetId(iY, iX));
                                }
                            }
                        }
                        break;
                }
            }

            public void Vistrel(Point mesto) {
                if (form1.boysyi[(int)form1.Cmainmap.RetId(mesto.Y, mesto.X)].nHP - adwDmg[(int)this.tip] >= 0)
                    form1.boysyi[(int)form1.Cmainmap.RetId(mesto.Y, mesto.X)].nHP -= adwDmg[(int)this.tip];
                else form1.boysyi[(int)form1.Cmainmap.RetId(mesto.Y, mesto.X)].nHP = 0;
            }

            public bool alive() {
                return nHP > 0;
            }
        }
    }
}
