using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeleVision
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        [DllImport("USER32.DLL")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        [DllImport("user32.dll")]
        static extern bool DrawMenuBar(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        private static string WINDOW_NAME = "";
        private const int GWL_STYLE = -16;
        private const uint WS_BORDER = 0x00800000;
        private const uint WS_CAPTION = 0x00C00000;
        private const uint WS_SYSMENU = 0x00080000;
        private const uint WS_MINIMIZEBOX = 0x00020000;
        private const uint WS_MAXIMIZEBOX = 0x00010000;
        private const uint WS_OVERLAPPED = 0x00000000;
        private const uint WS_POPUP = 0x80000000;
        private const uint WS_TABSTOP = 0x00010000;
        private const uint WS_VISIBLE = 0x10000000;
        public Bitmap picture;
        public static int x, y;
        private static int width = Screen.PrimaryScreen.Bounds.Width;
        private static int height = Screen.PrimaryScreen.Bounds.Height;
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
                picture = new Bitmap("television.png");
                picture = ResizeBitmap(picture, width, height);
            }
            catch
            {
                picture = new Bitmap("television.gif");
            }
            this.Size = new Size(width, height);
            this.Location = new Point(0, 0);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Image = picture;
        }
        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }
            return result;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            valchanged(0, GetAsyncKeyState(Keys.PageUp));
            if (wd[0] == 1)
            {
                this.TopMost = false;
                WINDOW_NAME = GetActiveWindowTitle();
                IntPtr window = FindWindowByCaption(IntPtr.Zero, WINDOW_NAME);
                SetWindowLong(window, GWL_STYLE, WS_CAPTION | WS_POPUP | WS_BORDER | WS_SYSMENU | WS_TABSTOP | WS_VISIBLE | WS_OVERLAPPED | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
                DrawMenuBar(window);
            }
            valchanged(1, GetAsyncKeyState(Keys.PageDown));
            if (wd[1] == 1)
            {
                width = Screen.PrimaryScreen.Bounds.Width;
                height = Screen.PrimaryScreen.Bounds.Height;
                WINDOW_NAME = GetActiveWindowTitle();
                IntPtr window = FindWindowByCaption(IntPtr.Zero, WINDOW_NAME);
                SetWindowLong(window, GWL_STYLE, WS_SYSMENU);
                SetWindowPos(window, -2, width / 20, height / 10, width * 8 / 11, height * 8 / 10, 0x0040);
                DrawMenuBar(window);
                this.Size = new Size(width, height);
                this.Location = new Point(0, 0);
                this.TopMost = true;
            }
        }
        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
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