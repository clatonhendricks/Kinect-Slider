namespace Kinect_Slider
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl1 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl2 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl3 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl4 = this.Factory.CreateRibbonDropDownItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ribbon1));
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl5 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl6 = this.Factory.CreateRibbonDropDownItem();
            this.tab1 = this.Factory.CreateRibbonTab();
            this.tab2 = this.Factory.CreateRibbonTab();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.ribStartShow = this.Factory.CreateRibbonButton();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.chkLeft = this.Factory.CreateRibbonCheckBox();
            this.chkRight = this.Factory.CreateRibbonCheckBox();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.galImages = this.Factory.CreateRibbonGallery();
            this.group4 = this.Factory.CreateRibbonGroup();
            this.ribFeedback = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.tab2.SuspendLayout();
            this.group2.SuspendLayout();
            this.group1.SuspendLayout();
            this.group3.SuspendLayout();
            this.group4.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // tab2
            // 
            this.tab2.Groups.Add(this.group2);
            this.tab2.Groups.Add(this.group1);
            this.tab2.Groups.Add(this.group3);
            this.tab2.Groups.Add(this.group4);
            this.tab2.Label = "Kinect Slider";
            this.tab2.Name = "tab2";
            // 
            // group2
            // 
            this.group2.Items.Add(this.ribStartShow);
            this.group2.Label = "Slide Show";
            this.group2.Name = "group2";
            // 
            // ribStartShow
            // 
            this.ribStartShow.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ribStartShow.Label = "Start SlideShow";
            this.ribStartShow.Name = "ribStartShow";
            this.ribStartShow.OfficeImageId = "SlideShowFromBeginning";
            this.ribStartShow.ShowImage = true;
            this.ribStartShow.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ribStartShow_Click);
            // 
            // group1
            // 
            this.group1.Items.Add(this.chkLeft);
            this.group1.Items.Add(this.chkRight);
            this.group1.Label = "Hand Selection";
            this.group1.Name = "group1";
            // 
            // chkLeft
            // 
            this.chkLeft.Label = "Left Hand";
            this.chkLeft.Name = "chkLeft";
            this.chkLeft.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.chkLeft_Click);
            // 
            // chkRight
            // 
            this.chkRight.Checked = true;
            this.chkRight.Label = "Right Hand";
            this.chkRight.Name = "chkRight";
            this.chkRight.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.chkRight_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.galImages);
            this.group3.Label = "Pointer";
            this.group3.Name = "group3";
            // 
            // galImages
            // 
            this.galImages.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.galImages.Image = global::Kinect_Slider.Properties.Resources.RedSs;
            this.galImages.ItemImageSize = new System.Drawing.Size(20, 20);
            ribbonDropDownItemImpl1.Image = global::Kinect_Slider.Properties.Resources.RedB;
            ribbonDropDownItemImpl1.Label = "Red Big";
            ribbonDropDownItemImpl2.Image = global::Kinect_Slider.Properties.Resources.RedSs;
            ribbonDropDownItemImpl2.Label = "Red Small";
            ribbonDropDownItemImpl3.Image = global::Kinect_Slider.Properties.Resources.BlueB;
            ribbonDropDownItemImpl3.Label = "Blue Big";
            ribbonDropDownItemImpl4.Image = ((System.Drawing.Image)(resources.GetObject("ribbonDropDownItemImpl4.Image")));
            ribbonDropDownItemImpl4.Label = "Blue Small";
            ribbonDropDownItemImpl5.Image = global::Kinect_Slider.Properties.Resources.YellowB;
            ribbonDropDownItemImpl5.Label = "Yellow Big";
            ribbonDropDownItemImpl6.Image = global::Kinect_Slider.Properties.Resources.YellowS;
            ribbonDropDownItemImpl6.Label = "Yellow Small";
            this.galImages.Items.Add(ribbonDropDownItemImpl1);
            this.galImages.Items.Add(ribbonDropDownItemImpl2);
            this.galImages.Items.Add(ribbonDropDownItemImpl3);
            this.galImages.Items.Add(ribbonDropDownItemImpl4);
            this.galImages.Items.Add(ribbonDropDownItemImpl5);
            this.galImages.Items.Add(ribbonDropDownItemImpl6);
            this.galImages.Label = "Red Small";
            this.galImages.Name = "galImages";
            this.galImages.ShowImage = true;
            this.galImages.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.galImages_Click);
            // 
            // group4
            // 
            this.group4.Items.Add(this.ribFeedback);
            this.group4.Name = "group4";
            // 
            // ribFeedback
            // 
            this.ribFeedback.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ribFeedback.Image = global::Kinect_Slider.Properties.Resources.bug;
            this.ribFeedback.Label = "Bug/Feedback";
            this.ribFeedback.Name = "ribFeedback";
            this.ribFeedback.ShowImage = true;
            this.ribFeedback.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ribFeedback_Click);
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.PowerPoint.Presentation";
            this.Tabs.Add(this.tab1);
            this.Tabs.Add(this.tab2);
            this.Close += new System.EventHandler(this.Ribbon1_Close);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.tab2.ResumeLayout(false);
            this.tab2.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.group4.ResumeLayout(false);
            this.group4.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab2;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ribStartShow;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox chkLeft;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox chkRight;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonGallery galImages;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group4;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ribFeedback;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
