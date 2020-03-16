using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outlook =   Microsoft.Office.Interop.Outlook;


namespace Kinect_Slider
{
    public partial class frmFB : Form
    {
        
        public frmFB()
        {
            InitializeComponent();
            

        }
        
        private bool check()
        {
            if ((txtBody.Text == "") || (txtSub.Text == ""))
            {
                MessageBox.Show("Please fill the Body & the Subject boxes");
                return false;
            } return true;
                
        }

        private void frmFB_Load(object sender, EventArgs e)

        {
            System.Reflection.Assembly thisAssembly = this.GetType().Assembly;

            lblVer.Text = "Ver: " +  thisAssembly.GetName().Version.ToString();
            

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (check())
                {
                    Microsoft.Office.Interop.Outlook.Application opp = new Microsoft.Office.Interop.Outlook.Application();
                    if (opp.DefaultProfileName != null)
                    {
                        Microsoft.Office.Interop.Outlook.MailItem mail = (Microsoft.Office.Interop.Outlook.MailItem)opp.CreateItem(0);

                        mail.To = "clatonh@microsoft.com";

                        mail.Subject = this.cboSub.Text + " for Kinect Slider: " + txtSub.Text;
                        mail.Body = this.txtBody.Text + System.Environment.NewLine + System.Environment.NewLine + "Kinect Slider" + System.Environment.NewLine + "Sent using " + lblVer.Text;
                        mail.Send();
                        this.Close();

                        MessageBox.Show("Thank you for your feedback!", "Kinect Slider", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else { MessageBox.Show("Couldnt find a Outlook profile. Please send a mail to clatonh@microsoft.com", "Kinect Slider", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PowerPoint Kinect Slider", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }


           
            
 
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Outlook.Application opp = new Microsoft.Office.Interop.Outlook.Application();
            MessageBox.Show(opp.DefaultProfileName.ToString());
        }

       
        
    }
}
