using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using ScCO = System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Coding4Fun.Kinect.WinForm;
//using Microsoft.OfficeLabs.Update;

using Microsoft.Kinect;
using Kinect.Toolbox;
using Kinect.Toolbox.Voice;
using Microsoft.Speech;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Deployment;
using System.Reflection;
using Microsoft.Win32;
using System.Security.Permissions;
using System.Security.AccessControl;





namespace Kinect_Slider
{

     //[StructLayout(LayoutKind.Sequential)]
     //   struct MOUSEINPUT
     //   {
     //       int dx;
     //       int dy;
     //       int mouseData;
     //       public int dwFlags;
     //       int time;
     //       IntPtr dwExtraInfo;
     //   }
     //   struct INPUT
     //   {
     //       public uint dwType;
     //       public MOUSEINPUT mi;
     //   }
     //   [DllImport("user32.dll", SetLastError = true)]
     //   static extern uint SendInput(uint cInputs, INPUT input, int size);


    public partial class Ribbon1 
    {
       
        //Runtime runtime = new Runtime();
        KinectSensor runtime;
        SpeechRecognitionEngine sre;
        string Kinect_Status = "";
        bool IsPPT_running = false;
        Thread t;

        #region "Variables"

        SwipeGestureDetector sgd = new SwipeGestureDetector();
        AlgorithmicPostureDetector Postdec = new AlgorithmicPostureDetector();
        VoiceCommander VoiceCmder = new VoiceCommander("Next", "Previous");
           

        
       
        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }


        private const string RecognizerId = "SR_MS_en-US_Kinect_10.0";
        public  const int SPI_SETCURSORS = 0x0057;

        public const int SPIF_UPDATEINIFILE = 0x01;

        public const int SPIF_SENDCHANGE = 0x02;

        public const ushort ESCAPE = 0x1B;
        public const int INPUT_KEYBOARD = 1;

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

        // public struct KEYBDINPUT
        //{
        //    public ushort wVk;
        //    public ushort wScan;
        //    public uint dwFlags;
        //    public long time;
        //    public uint dwExtraInfo;
        //};
        //[StructLayout(LayoutKind.Explicit,Size=28)]
       
        //public struct INPUT
        //{
        //    public uint type;
        //    public KEYBDINPUT ki;
        //};

        [DllImport("user32.dll")]
        static extern IntPtr GetMessageExtraInfo();

        public string CurValue, OldCursorValue;
        private static frmKinectInit frmLoadInit; // = new frmKinectInit();

        // Form variables
        public bool IsKinectInitOpen;

        #endregion 

        public static RegistryKey Reg_CU = Registry.CurrentUser.OpenSubKey("Software", true);
        //public static RegistryKey Reg_KS = Reg_CU.OpenSubKey("Kinect Slider", true);
        
        //public void  KB_ESC()
        //{
        //    INPUT structInput;
        //    structInput = new INPUT();
        //    structInput.type = INPUT_KEYBOARD;
        //    structInput.ki.wScan = 0;
        //    structInput.ki.time = 0;
        //    structInput.ki.dwFlags = 0;
        //    structInput.ki.dwExtraInfo = (uint)GetMessageExtraInfo();
        //    if (ESCAPE)
        //    {

        //    }


        //}

        //public void Load_Kinect()
        //{
        //    Runtime runtime = new Runtime();
        //}

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            

            Globals.ThisAddIn.Application.SlideShowEnd += new PowerPoint.EApplication_SlideShowEndEventHandler(Application_SlideShowEnd);

            //AutoUpdate code

            //AutomaticUpdater.CheckAndDownloadAsync();


            // Get default Mouse cursor path 
            GetMouseRegValue();

            // Get Kinect Slider registry settings
            RegistryKey RK = Registry.CurrentUser.OpenSubKey("Software\\Kinect Slider");

            if (RK != null)
            {
                GetSettings();
            }
            else { FirstRun(); }

       
            //Initialize Kinect senser. 
           // InitializeKinect();

            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
           
            sgd.OnGestureDetected += new Action<string>(sgd_OnGestureDetected);
            Postdec.PostureDetected += new Action<string>(Postdec_PostureDetected);
            
            VoiceCmder.OrderDetected += new Action<string>(VoiceCmder_OrderDetected);
        }

        void VoiceCmder_OrderDetected(string order)
        {
            
            switch (order)
            {
                case "Next":
                    NxtSlide();
                    break;
                case "Previous":
                    PreSlide();
                    break;
            }
        }

        void Postdec_PostureDetected(string obj)
        {
            Debug.WriteLine("Posture Detected" + obj.ToString());
            
            if (obj == "HandsJoined")
            {
                ClosePPT();
            }
        }

        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {

            

            switch (e.Status)
            {
                case KinectStatus.Connected:
                    //FindKinectSensor();
                    InitializeKinect();
                    break;
                case KinectStatus.Disconnected:
                    if (IsPPT_running == true)
                    {
                        MessageBox.Show("Kinect is disconnected. Check the USB connection and try again", "Failed to start Kinect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ClosePPT();
                    }
                    break;
                case KinectStatus.NotPowered:
                    if (IsPPT_running == true)
                    {
                        MessageBox.Show("Kinect is not powered on. Check the power supply and try again", "Failed to start Kinect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ClosePPT();
                    }
                    break;
                    

            }

        }

        //private void  FindKinectSensor()
        //{
        //    foreach (KinectSensor sensor in KinectSensor.KinectSensors)
        //    {
        //        //if (runtime.Status == KinectStatus.Connected)
        //        //{
        //        runtime = sensor;
        //        //return true;
        //        //break;

        //        // }

        //    }

        //    if (runtime == null)
        //    {
        //        Kinect_Status = "Found none Kinect Sensors connected to USB";
        //        //return false;

        //    }
        //    else
        //    {

        //        if (runtime.Status == KinectStatus.Connected)
        //        {
        //            InitializeKinect();
        //        }
        //    }

        //}
              

        private bool InitializeKinect()
        {

          
                //runtime = KinectSensor.KinectSensors[0];
                runtime = (from sensorToCheck in KinectSensor.KinectSensors 
                           where sensorToCheck.Status == KinectStatus.Connected 
                           select sensorToCheck).FirstOrDefault();


            //if (runtime == null)
            //{
            //    MessageBox.Show("No Kinect sensors are attached to this computer or none of the ones that are  attached are connected",
            //                                   "Failed to connect to Kinect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
              
            //}else
            //    {
                runtime.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                runtime.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                //runtime.SkeletonStream.Enable();
                runtime.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(runtime_SkeletonFrameReady);
                return true;
             // }
          

        }


        void Application_SlideShowEnd(PowerPoint.Presentation Pres)
        {
           // TO DO: ClosePPT();
            
            
            
        }

        private void ClosePPT()
        {
            //MessageBox.Show("Hello");
            try
            {
               // PP_controls("close");
                IsPPT_running = false;
                Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.Exit();
                SetRegCursor(OldCursorValue);
                
                //KinectSensor runtime = KinectSensor.KinectSensors[0];
                //VoiceCmder.Stop();
                runtime.Stop();
            }
            catch
            {

            }
            finally
            {
                SetRegCursor(OldCursorValue);
                // runtime = new Runtime();
                //KinectSensor runtime = KinectSensor.KinectSensors[0];
                runtime.Stop();
            }
        }
        
        private void GetSettings()
        {

            RegistryKey RK = Registry.CurrentUser.OpenSubKey("Software\\Kinect Slider");
            string Hand = (string)RK.GetValue("Hand","none");
            string Pointer = (string)RK.GetValue("Pointer", "none");
            if (Hand != "none")
            {
                if (Hand == "Right")
                {
                    chkRight.Checked = true;
                    chkLeft.Checked = false;
                }
                else
                {
                    chkRight.Checked = false;
                    chkLeft.Checked = true;
                }
            } 
            //Pointer
            switch (Pointer)
            {
                case "Blue Big":
                    galImages.Label = "Blue Big";
                    galImages.Image = Properties.Resources.BlueB;

                    break;
                case "Blue Small":
                    galImages.Label = "Blue Small";
                    galImages.Image = Kinect_Slider.Properties.Resources.BlueS;
                    break;

                case "Yellow Big":
                    galImages.Label = "Yellow Big";
                    galImages.Image = Kinect_Slider.Properties.Resources.YellowB;
                    break;
                case "Yellow Small":
                    galImages.Label = "Yellow Small";
                    galImages.Image = Kinect_Slider.Properties.Resources.YellowS;
                    break;
                case "Red Small":
                    galImages.Label = "Red Small";
                    galImages.Image = Kinect_Slider.Properties.Resources.RedSs;
                    break;
                default:
                    galImages.SelectedItem.Label = "Red Big";
                    galImages.SelectedItem.Image = Kinect_Slider.Properties.Resources.RedB;
                    break;
            }
        }
        
        private void FirstRun()
        {
            Reg_CU.CreateSubKey("Kinect Slider");
            RegistryKey Reg_KS = Reg_CU.OpenSubKey("Kinect Slider", true);
            Reg_KS.SetValue("Hand", "Right", RegistryValueKind.String);           
            Reg_KS.SetValue("Pointer", "Red Big", RegistryValueKind.String);         
            
        }

        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IntPtr ptr = bmp.GetHicon();
            IconInfo tmp = new IconInfo();
            GetIconInfo(ptr, ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            ptr = CreateIconIndirect(ref tmp);
            return new Cursor(ptr);
        }
       
        private void GetMouseRegValue()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("Control Panel");
            RegistryKey rsubkey = rk.OpenSubKey("Cursors");
            OldCursorValue = (string)rsubkey.GetValue("Arrow");
           // return OldCursorValue;

                      
        }

        private string GetCustomCursor()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            switch (galImages.Label)
            {
                case "Blue Big":
                    path = path + "\\Cursors\\BlueB.cur";
                    break;
                case "Blue Small":
                    path = path + "\\Cursors\\BlueS.cur";
             
                    break;
                case "Yellow Big":
                    path = path + "\\Cursors\\YellowB.cur";
                    break;
                case "Yellow Small":
                    path = path + "\\Cursors\\YellowS.cur";
                    break;
                case "Red Small":
                    path = path + "\\Cursors\\RedS.cur";
                    break;
                default:
                    path = path + "\\Cursors\\RedB.cur";
                    break;
            }
            return path.Substring(6);
        }

        private void SetRegCursor(string CursorPath)
        {
           // MessageBox.Show(GetCustomCursor());

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("Control Panel");
            RegistryKey rsubkey = rk.OpenSubKey("Cursors", true);
            Debug.WriteLine("Arrow");
            rsubkey.SetValue("Arrow", CursorPath);
            SystemParametersInfo(SPI_SETCURSORS, 0, 0, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        private static void NxtSlide()
        {
           /* if (Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.CurrentShowPosition != Globals.ThisAddIn.Application.ActivePresentation.Slides.Count)
            {
                Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.GotoSlide(Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.CurrentShowPosition + 1);
                Thread.Sleep(2000);
            } */
            Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.Next();

        }

        private static void PreSlide()
        {
           /* if (Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.CurrentShowPosition != 1)
            {
                Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.GotoSlide(Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.CurrentShowPosition - 1);
                Thread.Sleep(2000);
            } */

            Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.Previous();
        }

        private void ribStartShow_Click(object sender, RibbonControlEventArgs e)
        {

            //if (InitializeKinect())
            //{
            KinectSensor testsensor = KinectSensor.KinectSensors.FirstOrDefault();

            if (testsensor == null)
            { 
                MessageBox.Show("No Kinect sensors are attached to this computer or none of the ones that are  attached are connected",
                               "Failed to connect to Kinect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else { StartPPT(); }

        }

        private void StartPPT()
        {
            Kinect_init_Form();

            Globals.ThisAddIn.Application.ActivePresentation.SlideShowSettings.Run();
            System.Windows.Forms.Cursor.Position = new Point(500, 500);

            //PP_controls("Open");

           InitializeKinect();
            //VoiceCmder.Start(runtime);



            //Change the cursor 
            SetRegCursor(GetCustomCursor());

            InsRuntime();
        }
   
        private void InsRuntime()
        {
            
            runtime.SkeletonStream.Enable();
            runtime.Start();
            //VoiceCmder.Start(runtime);
            //CreateSpeechRecognizer();
            //StartSpeechRecognition();

            

            //var t = new Thread(new ThreadStart(RunKinectAudio));
            //t.SetApartmentState(ApartmentState.MTA);
            //t.Start();

            
            #region Smoothing 
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.4f,
                Correction = 0.1f,
                Prediction = 0.1f,
                JitterRadius = 0.4f,
                MaxDeviationRadius = 0.5f
            };
           
            runtime.SkeletonStream.Enable(parameters);
            #endregion 
        }

        void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            try
            {
                
                SkeletonFrame skeletonSet = e.OpenSkeletonFrame();
                if (skeletonSet == null) return;

                Skeleton[] Skeletons = new Skeleton[skeletonSet.SkeletonArrayLength];

                skeletonSet.CopySkeletonDataTo(Skeletons);





                Skeleton data = (from ss in Skeletons
                                    where ss.TrackingState == SkeletonTrackingState.Tracked
                                    select ss).FirstOrDefault();
                
                    if (data.TrackingState == SkeletonTrackingState.Tracked)
                    {

                        //var head = data.Joints[JointID.Head];
                        var rightHand = data.Joints[JointType.HandRight];

                        var leftHand = data.Joints[JointType.HandLeft];
                        var ElbowLeft = data.Joints[JointType.ElbowLeft];
                        var ShoulderLeft = data.Joints[JointType.ShoulderLeft];

                        sgd.Add(data.Joints[JointType.HandRight].Position, runtime);
                        

                        Joint scaledRight = rightHand.ScaleTo(ScCO.Screen.PrimaryScreen.Bounds.Width, ScCO.Screen.PrimaryScreen.Bounds.Height, 0.5f, 0.5f);
                        Joint scaledLeft = leftHand.ScaleTo(ScCO.Screen.PrimaryScreen.Bounds.Width, ScCO.Screen.PrimaryScreen.Bounds.Height, 0.5f, 0.5f);


                        if (IsKinectInitOpen == true)
                        {
                            Kinect_init_Form();
                        }

                        MoveCursor(scaledLeft, scaledRight);
                    }
                }
            //}
            catch (NullReferenceException)
            {
                Debug.WriteLine("NullRef Error");
            }
        
           
        }


        void sgd_OnGestureDetected(string obj)
        {
            if (obj == "SwipeToRight")
            {
                Debug.WriteLine("Swipe");
                NxtSlide();
            }

            if (obj == "SwipeToLeft")
            {
                PreSlide();
            }


        }

        private void MoveCursor(Joint leftHand, Joint rightHand)
        {
       

                float MouX, MouY;

                if (chkRight.Checked == true)
                {
                    MouX = rightHand.Position.X;
                    MouY = rightHand.Position.Y;
                }
                else
                {
                    MouX = leftHand.Position.X;
                    MouY = leftHand.Position.Y;
                }

                if (((int)MouX > 0) || ((int)MouY > 0))
                {
                    System.Windows.Forms.Cursor.Position = new Point((int)MouX, (int)MouY);
                }

                else
                {
                    ClosePPT();
                    
                   // PP_controls("Close");
                   //// Kinect_init_Form("Close");
                   // Globals.ThisAddIn.Application.ActivePresentation.SlideShowWindow.View.Exit();
                   // SetRegCursor(OldCursorValue);
                    //runtime.Uninitialize();
                    
                    //runtime.SkeletonEngine.IsEnabled = false;

                }

                // Next Slide
                if ((int)MouX >= ScCO.Screen.PrimaryScreen.Bounds.Width)
                {
                    System.Windows.Forms.Cursor.Position = new Point((int)MouX, (int)MouY - 200);
                    //NxtSlide();
                }
                else // Previous Slide
                    if ((int)MouX <= 0)
                    {
                        System.Windows.Forms.Cursor.Position = new Point((int)MouX, (int)MouY + 200);
                       // PreSlide();
                    }

            //}
            


        }

 


        private void Kinect_init_Form()
        {
            
            if ((frmLoadInit == null) || (frmLoadInit.Visible == false))
            {
                frmLoadInit = new frmKinectInit();
                //frmLoadInit.Show();

                t = new Thread(NewForm_thread);
                t.IsBackground = true;
                
                t.Start();
                //frmLoadInit.ShowDialog();
                
                
                IsKinectInitOpen = true;
            }
            else
            {
                if (frmLoadInit.Visible == true)
                {
                    frmLoadInit.Invoke((MethodInvoker)(() => frmLoadInit.Close()));
                    IsKinectInitOpen = false;
                }

            }

        }

        private void NewForm_thread()
        {
            Debug.WriteLine("NewForm_thread");
            frmLoadInit = new frmKinectInit();

            frmLoadInit.ShowDialog();


        }

        private void galImages_Click(object sender, RibbonControlEventArgs e)
        {
           

            galImages.Image = galImages.SelectedItem.Image;
            galImages.Label = galImages.SelectedItem.Label;
            RegistryKey Reg_KS = Reg_CU.OpenSubKey("Kinect Slider", true);
            Reg_KS.SetValue("Pointer", galImages.SelectedItem.Label, RegistryValueKind.String);

            
        }

        private void chkLeft_Click(object sender, RibbonControlEventArgs e)
        {

            chkRight.Checked = false;
            RegistryKey Reg_KS = Reg_CU.OpenSubKey("Kinect Slider", true);
            Reg_KS.SetValue("Hand", "Left", RegistryValueKind.String);
             
        }

        private void chkRight_Click(object sender, RibbonControlEventArgs e)
        {

            chkLeft.Checked = false;
            RegistryKey Reg_KS = Reg_CU.OpenSubKey("Kinect Slider", true);
            Reg_KS.SetValue("Hand", "Right", RegistryValueKind.String);


        }

        private void ribFeedback_Click(object sender, RibbonControlEventArgs e)
        {
            
            frmFB frmfeedback = new frmFB();
            frmfeedback.Show();

             
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            SetRegCursor(GetCustomCursor());
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            SetRegCursor(OldCursorValue);
            
        }

        private void Ribbon1_Close(object sender, EventArgs e)
        {
            //if (AutomaticUpdater.IsUpdateEnabled)
            //{
            //    AutomaticUpdater.Apply();
            //}
        }


//        #region Speech...

//        private static RecognizerInfo GetKinectRecognizer()
//        {
//            Func<RecognizerInfo, bool> matchingFunc = r =>
//            {
//                string value;
//                r.AdditionalInfo.TryGetValue("Kinect", out value);
//                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
//            };
//            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
//        }



//        private void StartSpeechRecognition()
//        {
//            if (runtime == null || sre == null)
//                return;

//            var audioSource = this.runtime.AudioSource;
//            audioSource.BeamAngleMode = BeamAngleMode.Adaptive;
//            var kinectStream = audioSource.Start();

//            sre.SetInputToAudioStream(
//                    kinectStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
//            sre.RecognizeAsync(RecognizeMode.Multiple);

//        }

//        private SpeechRecognitionEngine CreateSpeechRecognizer()
//        {

//            RecognizerInfo ri = GetKinectRecognizer();
//            if (ri == null)
//            {
//                MessageBox.Show(
//                    @"There was a problem initializing Speech Recognition.
//                      Ensure you have the Microsoft Speech SDK installed.",
//                    "Failed to load Speech SDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                // need to add all the closing functions
//                return null;
//            }


//            try
//            {
//                sre = new SpeechRecognitionEngine(ri.Id);
//            }
//            catch
//            {
//                MessageBox.Show(
//                    @"There was a problem initializing Speech Recognition.
//Ensure you have the Microsoft Speech SDK installed and configured.",
//                    "Failed to load Speech SDK", MessageBoxButtons.OK, MessageBoxIcon.Error);

//                //this.Close();
//                // need to add all the closing functions
//                return null;
//            }

//            var grammar = new Choices();
//            grammar.Add("next");
//            grammar.Add("previous");
            
//            var gb = new GrammarBuilder { Culture = ri.Culture };
//            gb.Append(grammar);

//            // Create the actual Grammar instance, and then load it into the speech recognizer.
//            var g = new Grammar(gb);

//            sre.LoadGrammar(g);
//            //sre.SpeechRecognized += this.SreSpeechRecognized;
//            //sre.SpeechHypothesized += this.SreSpeechHypothesized;
//            //sre.SpeechRecognitionRejected += this.SreSpeechRecognitionRejected;
//            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
//            sre.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(sre_SpeechHypothesized);
//            sre.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sre_SpeechRecognitionRejected);



//            return sre;
//        }

//        void sre_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
//        {
//            //textBox1.Text += "Rejected " + e.Result.Text.ToString() + e.Result.Confidence.ToString();
//            Debug.WriteLine("Rejected " + e.Result.Text.ToString() + e.Result.Confidence.ToString());
//        }

//        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
//        {
            
//            //textBox1.Text += "Hypothesized " + e.Result.Text.ToString() + e.Result.Confidence.ToString();
//            Debug.WriteLine("Hypothesized " + e.Result.Text.ToString() + e.Result.Confidence.ToString());

//        }

//        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
//        {
//            //throw new NotImplementedException();
//            switch (e.Result.Text.ToUpperInvariant())
//            {
//                case "NEXT":
//                    //textBox1.Text += Environment.NewLine + "RED";
//                    Debug.WriteLine("NEXT");
//                    NxtSlide();
//                    break;
//                case "PREVIOUS":
//                    //textBox1.Text += Environment.NewLine + "GREEN";
//                    Debug.WriteLine("Previous");
//                    PreSlide();
//                    break;
              
//            }

//            string status = "Recognized: " + e.Result.Text + " " + e.Result.Confidence;

//        }


//        #endregion 

    }
}