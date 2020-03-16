using System;
using System.Windows.Forms;

namespace Kinect_Slider
{
    public partial class frmKinectInit : Form
    {
        public frmKinectInit()
        {
            InitializeComponent();
        }

        private void frmKinectInit_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = "Initializing Kinect Please wait..";
        }
    }
}
