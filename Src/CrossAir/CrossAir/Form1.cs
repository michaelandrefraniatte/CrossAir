using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrossAir
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        public Bitmap picture;
        public static int x, y;
        private static int width = Screen.PrimaryScreen.Bounds.Width;
        private static int height = Screen.PrimaryScreen.Bounds.Height;
        private static int picwidth = Screen.PrimaryScreen.Bounds.Width;
        private static int picheight = Screen.PrimaryScreen.Bounds.Height;
        public static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static bool[] ws = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        static void valchanged(int n, bool val)
        {
            if (val)
            {
                if (wd[n] <= 1)
                {
                    wd[n] = wd[n] + 1;
                }
                wu[n] = 0;
            }
            else
            {
                if (wu[n] <= 1)
                {
                    wu[n] = wu[n] + 1;
                }
                wd[n] = 0;
            }
            ws[n] = val;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                picture = new Bitmap("crossair.png");
            }
            catch
            {
                picture = new Bitmap("crossair.gif");
            }
            picwidth = picture.Width;
            picheight = picture.Height;
            this.Size = new Size(picwidth, picheight);
            this.Location = new Point(width / 2 - picwidth / 2, height / 2 - picheight / 2);
            this.pictureBox1.Image = picture;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            valchanged(0, GetAsyncKeyState(Keys.PageUp));
            if (wd[0] == 1)
            {
                this.TopMost = false;
            }
            valchanged(1, GetAsyncKeyState(Keys.PageDown));
            if (wd[1] == 1)
            {
                this.Size = new Size(picwidth, picheight);
                this.Location = new Point(width / 2 - picwidth / 2, height / 2 - picheight / 2);
                this.TopMost = true;
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            var borderHeight = this.Bounds.Height - this.ClientRectangle.Height;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == FormBorderStyle.FixedToolWindow)
                return;
            if (x != 0 & y != 0)
            {
                this.Left = x;
                this.Top = y;
            }
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
    }
}