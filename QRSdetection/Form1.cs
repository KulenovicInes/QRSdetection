using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRSdetection
{
    public partial class Form1 : Form
    {
        double t = 0;
        string inputfile;
        int BPM, i;
        double[] RR = new double[10];
        string tip;
        QRS detektor = new QRS();
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            inputfile = openFileDialog1.FileName;
            button2.Enabled = true;
            button3.Enabled = true;
            button5.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clearChart();
        }

        private void clearChart()
        {
            chart1.Series[0].Points.Clear();

            for (int i = 0; i < 250; i++)
            {
                chart1.Series[0].Points.AddY(0);

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double[] a = new double[15];
            UlazniBuffer.ReadMany(out a, 15);
            double x;
  
            if (tip == "dat")
                x = (a.Last() - 1024) * 0.005;
            else
                x = a.Last();
            t += 0.04;
            bool check = detektor.QRSCheck(x);

            if (chart1.Series[0].Points.Count < 250)
            {

                chart1.Series[0].Points.AddY(x);
                if (check)
                {

                    chart1.Series[0].Points.Last().Label = "QRS";
                    chart1.Series[0].Points.Last().LabelAngle = 90;
                    i++;
                    RR[i] = t;
                    BPM = (int)(60 / (RR.Sum() / 9));
                    t = 0;
                    BPMMetar.Text = BPM.ToString() + " BPM";
                    if (i == 9)
                    {
                        i = 0;
                    }
                }
                //ako je true dodaj marker na vrijednost kao Qrs
            }
            else
            {
                for (int i = 0; i < chart1.Series[0].Points.Count - 1; i++)
                {
                    chart1.Series[0].Points[i] = chart1.Series[0].Points[i + 1];

                }

                chart1.Series[0].Points.RemoveAt(249);
                chart1.Series[0].Points.AddY(x);
                if (check)
                {


                    chart1.Series[0].Points.Last().Label = "QRS";
                   
                    chart1.Series[0].Points.Last().LabelAngle = 90;
                    i++;

                    RR[i] = t;
                    BPM = (int)(60 / (RR.Sum() / 9));
                    BPMMetar.Text = BPM.ToString() + " BPM";
                    t = 0;
                    if (i == 9)
                    {
                        i = 0;
                    }
                }
                //Ako je true dodaj marker na vrijednost kao Qrs

            }

            chart1.Update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Int16 chan = Convert.ToInt16(textBox1.Text);
            clearChart();
            tip = inputfile.Split('.').Last();
            UlazniBuffer.Close();
            if (tip == "txt")
                UlazniBuffer.Open(inputfile, chan, EKGFileType.TEXT);
            else
                UlazniBuffer.Open(inputfile, chan, EKGFileType.BINARY);
            timer1.Start();
            i = 0;
            t = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
           // UlazniBuffer.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
