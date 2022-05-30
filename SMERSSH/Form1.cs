using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMERSSH {
    unsafe public partial class Form1 : Form {
        static public Point POknaSiza = new Point(800, 800);
        static public Point PKartSize = new Point(40, 40);
        static public List<List<PictureBox>> kartinki = new List<List<PictureBox>>();
        static public List<List<int>> IDKartinok = new List<List<int>>();

        static public carta mainmap = new carta(new List<List<tayl>>());
        static public Point PSizeMap = new Point();


        static public List<Boycuha> boysyi = new List<Boycuha>();

        static public string[] FilesBoys = { "boycuha_1.bmp", "boycuha_2.bmp", "boycuha_3.bmp", "boycuha_4.bmp" };

        public Form1() {
            InitializeComponent();
            I1.ChtenieBlyaMapyi(this);
            Boycuha boec;
            boec = new Boycuha(new Point(0, 0), TipBoyca.Sniper, true, (uint)boysyi.Count, this);
            boysyi.Add(boec);

            boec = new Boycuha(new Point(2, 2), TipBoyca.Strelok, false, (uint)boysyi.Count, this);
            boysyi.Add(boec);

            boec = new Boycuha(new Point(6, 7), TipBoyca.Strelok, false, (uint)boysyi.Count, this);
            boysyi.Add(boec);

            mainmap.ProvaMapyi(boysyi);

            I1.ZadelkaVisuala();

            this.KeyDown += textBox1_KeyDown;

            this.MouseDown += MouseDown_send;

            return;
        }


        public static void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {

            switch (e.KeyCode) {
                case Keys.W: {
                        boysyi[0].Dvijenie(new Point(0, -1));
                        break;
                    }
                case Keys.A: {
                        boysyi[0].Dvijenie(new Point(-1, 0));
                        break;
                    }
                case Keys.S: {
                        boysyi[0].Dvijenie(new Point(0, 1));
                        break;
                    }
                case Keys.D: {
                        boysyi[0].Dvijenie(new Point(1, 0));
                        break;
                    }
                case Keys.D1: {
                        break;
                    }
            }

            hod();
        }

        static uint dwIDvibor = 0xffffffff;

        public static void MouseDown_send(object sender, System.Windows.Forms.MouseEventArgs e) {

            
            if (e.X > PKartSize.X * PSizeMap.X || e.Y > PKartSize.Y * PSizeMap.Y) { dwIDvibor = 0xffffffff; return; }

            var PNajKv = new Point(e.X / PKartSize.X, e.Y / PKartSize.Y);

            if (Form1.mainmap.RetId(PNajKv.Y, PNajKv.X) != 0xffffffff) {
                if (Form1.boysyi[(int)Form1.mainmap.RetId(PNajKv.Y, PNajKv.X)].bNash) {
                    dwIDvibor = Form1.mainmap.RetId(PNajKv.Y, PNajKv.X);
                }
                else {
                    if (dwIDvibor == 0xffffffff)
                        return;
                    uint dwIDVraga = Form1.mainmap.RetId(PNajKv.Y, PNajKv.X);

                    bool esty = false;
                    for (int i = 0; i < Form1.boysyi[(int)dwIDvibor].aVidimyie.Count; i++) {
                        if (Form1.boysyi[(int)dwIDvibor].aVidimyie[i] == dwIDVraga) { esty = true; break; }
                    }

                    if (!esty)
                        return;

                    Form1.boysyi[(int)dwIDvibor].Vistrel(PNajKv);

                    hod();
                }
            }
        }

        public static void hod() {
            //Скрытие врагов
            for (int i = 0; i < boysyi.Count; i++) {
                if (!boysyi[i].bNash)
                    boysyi[i].bViden = false; //boysyi[i].IPikcha.Visible = false;
            }

            //Итерация по бойцам
            for (int i = 0; i < boysyi.Count; i++) {
                boysyi[i].Prosmotr();
                if (boysyi[i].bNash) {
                    for (int i1 = 0; i1 < boysyi[i].aVidimyie.Count; i1++) {
                        if (boysyi[i].aVidimyie[i1] != 0xffffffff && boysyi[(int)boysyi[i].aVidimyie[i1]].alive())
                            boysyi[(int)boysyi[i].aVidimyie[i1]].bViden = true;
                    }
                }
                else {

                }
            }

            //Перенос ид бойцов на тайлы
            mainmap.ProvaMapyi(boysyi);

            //Готовность визуала
            I1.ZadelkaVisuala();
        }
    }

    unsafe public class carta {
        public List<List<tayl>> kvadratyi;

        public carta() { }
        
        public carta(List<List<tayl>> huita) {
            kvadratyi = huita;
        }

        public void ProvaMapyi(List<Boycuha> boysyi) {
            for (int i1 = 0; i1 < this.kvadratyi.Count; i1++) {
                for (int i2 = 0; i2 < this.kvadratyi[i1].Count; i2++) {
                    this.kvadratyi[i1][i2] = new tayl(0xffffffff, this.kvadratyi[i1][i2].tip);
                }
            }

            for (int i = 0; i < boysyi.Count; i++) {
                this.kvadratyi[boysyi[i].PMesto.Y][boysyi[i].PMesto.X] = new tayl(boysyi[i].dwID, this.kvadratyi[boysyi[i].PMesto.Y][boysyi[i].PMesto.X].tip);
            }
        }

        public uint RetId(int y, int x) {
            return kvadratyi[y][x].dwIDBoyca;
        }
    }
}
