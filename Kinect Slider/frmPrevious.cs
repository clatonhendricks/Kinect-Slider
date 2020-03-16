using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using ScCO = System.Windows.Forms;
using System.Windows.Forms;

namespace Kinect_Slider
{
    public partial class frmPrevious : Form
    {
        public frmPrevious()
        {
            InitializeComponent();
        }

        private void frmPrevious_Load(object sender, EventArgs e)
        {
            this.Paint +=new PaintEventHandler(frmPrevious_Paint);
            //this.Opacity = 100;
            this.Location = new Point(ScCO.Screen.PrimaryScreen.Bounds.Width / 2 - this.Width, ScCO.Screen.PrimaryScreen.Bounds.Height - this.Height);

        }

        private void frmPrevious_Paint(object sender, PaintEventArgs e)
        {
            Bitmap im = new Bitmap(Kinect_Slider.Properties.Resources.Next);// @"C:\Users\clatonh.000\Documents\Mesh\Kinect\Projects\Kinect Slider\Kinect Slider\Images\Next.png");

            Graphics g = e.Graphics;

            Rectangle mainRect = new Rectangle(0, 0, this.Width, this.Height);

            Region mainRegion = new Region(mainRect);

            e.Graphics.SetClip(mainRegion, CombineMode.Replace);

            GraphicsPath myPath = new GraphicsPath();

            Region ExcludeRegion3 = new Region(myPath);

            e.Graphics.ExcludeClip(ExcludeRegion3);
            
            e.Graphics.DrawImage(im, 0, 0, im.Width, im.Height);

            e.Graphics.ResetClip(); 

        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.Opacity = 100;
            if (Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.CurrentShowPosition != 1)
            {
                Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.GotoSlide(Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.CurrentShowPosition - 1);
            }
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            
            this.Opacity = 0.30;
           
        }
    }
}
