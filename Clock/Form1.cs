using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class mainForm : Form
    {
        private Bitmap buffer;
        public mainForm()
        {
            InitializeComponent();

            init();
        }

        Pen borderPen = new Pen(Brushes.Black);//圆形边框画笔
        Pen secondScalePen = new Pen(Brushes.Black);//秒刻度画笔
        Pen minuteScalePen = new Pen(Brushes.Black);//分刻度画笔
        Pen hourPen = new Pen(Brushes.Red);  //时针画笔
        Pen minutePen = new Pen(Brushes.Blue);//分针画笔
        Pen secondPen = new Pen(Brushes.Black);//秒针画笔
        int borderGap = 5;
        Font font = new System.Drawing.Font(FontFamily.GenericSerif, 16);
        int panelWidth = 0;
        int panelHeight = 0;
        int rdius = 0;

        private void init()
        {
            buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = buffer;
            borderPen.Width = 2;
            minuteScalePen.Width = 3;//分刻度线要比秒刻度线粗
            hourPen.Width = 5;                   //时针宽度
            minutePen.Width = 3;                  //分针宽度
            panelWidth = panel_clock.Width;
            panelHeight = panel_clock.Height;
            //绘制的时钟是圆形的，因此选择较短的边作为直径
            rdius = (panelWidth < panelHeight ? panelWidth : panelHeight) / 2;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (buffer == null) return;

            Graphics g = Graphics.FromImage(buffer);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            //绘制前，先清空内容
            g.Clear(Color.White);

            //先绘制时钟的边框(双边框)
            g.DrawEllipse(borderPen, (panelWidth / 2 - rdius + borderGap), 5, ((rdius - borderGap) * 2), ((rdius - borderGap) * 2));
            g.DrawEllipse(borderPen, (panelWidth / 2 - rdius), 0, rdius * 2, rdius * 2);

            //画圆心
            g.FillEllipse(Brushes.Black, (panelWidth-10) / 2, (panelHeight - 10) / 2, 10, 10);

            //画秒刻度 60根 每一秒一根刻度线
            for (int i = 0; i < 60; i++)
            {
                //绘出刻度线
                g.DrawLine(secondScalePen, panelWidth / 2, borderGap, panelWidth / 2, rdius / 30 + borderGap);

                //旋转角度6度
                g.TranslateTransform(panelWidth / 2, panelHeight / 2); //设置旋转中心为圆心
                g.RotateTransform(6); //旋转
                g.TranslateTransform(-panelWidth / 2, -panelHeight / 2);  //设置回原来的中心
            }

            for (int i = 0; i < 60; i += 5)
            {
                //绘出刻度线
                g.DrawLine(minuteScalePen, panelWidth / 2, borderGap, panelWidth / 2, rdius / 30 + borderGap + 5);

                //绘出时间
                SizeF size = g.MeasureString(("" + i / 5) == "0" ? "12" : ("" + i / 5), font);
                g.DrawString(("" + i / 5) == "0" ? "12" : ("" + i / 5), font, Brushes.Black, (panelWidth - size.Width) / 2, rdius / 30 + borderGap + 5);

                //旋转角度30度
                g.TranslateTransform(panelWidth / 2, panelHeight / 2);
                g.RotateTransform(30);
                g.TranslateTransform(-panelWidth / 2, -panelHeight / 2);
            }

            //画时针
            int hour = DateTime.Now.Hour;
            float angle = (hour % 12) / 12.0f * 360;

            //旋转角度至合适的位置，以便画出时针
            g.TranslateTransform(panelWidth / 2, panelHeight / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-panelWidth / 2, -panelHeight / 2);

            //开始画时针
            g.DrawLine(hourPen, panelWidth / 2, panelHeight / 2, panelWidth / 2, rdius / 3);

            //再旋转回来
            g.TranslateTransform(panelWidth / 2, panelHeight / 2);
            g.RotateTransform(-angle);
            g.TranslateTransform(-panelWidth / 2, -panelHeight / 2);

            //画分针
            int minute = DateTime.Now.Minute;
            angle = (minute % 60) / 60.0f * 360;

            //旋转角度至合适的位置，以便画出分针
            g.TranslateTransform(panelWidth / 2, panelHeight / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-panelWidth / 2, -panelHeight / 2);

            //开始画分针
            g.DrawLine(minutePen, panelWidth / 2, panelHeight / 2, panelWidth / 2, borderGap + 3);

            //再旋转回来
            g.TranslateTransform(panelWidth / 2, panelHeight / 2);
            g.RotateTransform(-angle);
            g.TranslateTransform(-panelWidth / 2, -panelHeight / 2);

            //画秒针
            int second = DateTime.Now.Second;
            angle = (second % 60) / 60.0f * 360;

            //旋转角度至合适的位置，以便画出秒针
            g.TranslateTransform(panelWidth / 2, panelHeight / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-panelWidth / 2, -panelHeight / 2);

            //开始画秒针
            g.DrawLine(secondPen, panelWidth / 2, panelHeight / 2, panelWidth / 2, borderGap + 3);

            //再旋转回来
            g.TranslateTransform(panelWidth / 2, panelHeight / 2);
            g.RotateTransform(-angle);
            g.TranslateTransform(-panelWidth / 2, -panelHeight / 2);

            pictureBox1.Image = buffer;
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if(buffer != null)
            {
                buffer.Dispose();
                buffer = null;
            }

            init();
        }
    }
}
