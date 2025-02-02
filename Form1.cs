﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolinoameInterpolareGrafice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Graphics graphics;
        Bitmap bitmap;

        decimal x0, xn, y0, yn;

        int m;
        decimal[] x, y, u;
        decimal[,] Nf; 

        private void button1_Click(object sender, EventArgs e)
        {
            m = 5;
            x = new decimal[] { 1, 1.1m, 1.3m, 1.5m, 1.6m };
            y = new decimal[] { 1, 1.032m, 1.091m, 1.145m, 1.17m };
            x0 = x[0];
            xn = x[m - 1];
            y0 = y[0];
            yn = y[m - 1];

            //pasul 1, calculam diferentele divizate

            decimal[,] d = new decimal[m, m];
            for (int i = 0; i < m; i++)
                d[0, i] = y[i];

            for (int j= 1; j < m; j++)
                for (int i= 0; i < m-j; i++)
                    d[j, i] = (d[j-1, i+1] - d[j-1, i]) / (x[i+1] - x[i]);

            //pasul 2, calculam Nf
            decimal h = (xn - x0) / 1000;
            u = new decimal[1000];
            Nf = new decimal[m, 1000];
            for (int j = 0; j < 1000; j++)
            {
                u[j] = x[0] + j * h;
                Nf[0, j] = y[0];
                decimal[] p = new decimal[m];
                p[0] = 1;

                for (int i = 1; i < m; i++)
                {
                    p[i] = p[i-1]* (u[j]- x[i-1]);
                    Nf[i,j] = Nf[i-1, j] + p[i] * d[i,0];
                }

            }

            decimal[] f = new decimal[1000];
            for(int i = 0; i < 1000; i++)
                f[i] = Nf[m-1, i];

            //pasul 3, desenare
            DrawGraph(u, f);
        }

        public void DrawGraph(decimal[] x, decimal[] y)
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);

            for (int i = 0; i < x.Length; i++)
            {
                PointF location = MapValuesToPointF(x[i], y[i]);
                graphics.DrawEllipse(new Pen(Color.Red, 9), 
                    location.X - 1,  location.Y - 1, 3, 3);
                
            }

            pictureBox1.Image = bitmap;
        }

        public PointF MapValuesToPointF(decimal x, decimal y)
        {
            decimal scaleX = xn - x0;
            decimal scaleY = yn - y0;

            float X = (float)((x - x0) * (pictureBox1.Width - 5)/ scaleX);
            float Y = pictureBox1.Height -(float)((y - y0) * (pictureBox1.Height -5) / scaleY);
            return new PointF(X, Y);
        }
    }
}
