using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using gma.System.Windows;
using System.Diagnostics;

namespace GlobalHookDemo 
{
	class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.Label labelMousePosition;
		private System.Windows.Forms.TextBox textBox;
        private string pass = string.Empty;
        private bool ALT_F4 = false;

        public MainForm()
		{
			InitializeComponent();
		}
	
		// THIS METHOD IS MAINTAINED BY THE FORM DESIGNER
		// DO NOT EDIT IT MANUALLY! YOUR CHANGES ARE LIKELY TO BE LOST
		void InitializeComponent() {
            this.textBox = new System.Windows.Forms.TextBox();
            this.labelMousePosition = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.textBox.Location = new System.Drawing.Point(4, 55);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(322, 340);
            this.textBox.TabIndex = 3;
            // 
            // labelMousePosition
            // 
            this.labelMousePosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMousePosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMousePosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelMousePosition.Location = new System.Drawing.Point(4, 29);
            this.labelMousePosition.Name = "labelMousePosition";
            this.labelMousePosition.Size = new System.Drawing.Size(322, 23);
            this.labelMousePosition.TabIndex = 2;
            this.labelMousePosition.Text = "labelMousePosition";
            this.labelMousePosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonStop
            // 
            this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStop.Location = new System.Drawing.Point(85, 3);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.Click += new System.EventHandler(this.ButtonStopClick);
            // 
            // buttonStart
            // 
            this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStart.Location = new System.Drawing.Point(4, 3);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.Click += new System.EventHandler(this.ButtonStartClick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(328, 398);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.labelMousePosition);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Name = "MainForm";
            this.Text = "Gosho";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
			
		[STAThread]
		public static void Main(string[] args)
		{
            var newForm = new MainForm();
            Application.Run(newForm);
        }
		
		void ButtonStartClick(object sender, System.EventArgs e)
		{
			actHook.Start();
		}
		
		void ButtonStopClick(object sender, System.EventArgs e)
		{
            if(pass.Contains("12ab"))
            {
                actHook.Stop();
            }
            else
            {
                Process.Start(@"D:\Scripts\Kits\mf.jpg");
            }
        }
        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !pass.Contains("12ab"))
            {
                e.Cancel = true;
                Task.Factory.StartNew(() =>
                {
                    MessageBox.Show("Ne staa", "Security", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
                Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
                return;
            }
            if (e.CloseReason == CloseReason.TaskManagerClosing)
            {
                e.Cancel = true;
                Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
                return;
            }
        }

        UserActivityHook actHook;
        void MainFormLoad(object sender, System.EventArgs e)
		{
            actHook = new UserActivityHook(); // crate an instance with global hooks
            actHook.Stop();

            Process photoViewer = new Process();
            // hang on events
            actHook.OnMouseActivity+=new MouseEventHandler(MouseMoved);
			actHook.KeyDown+=new KeyEventHandler(MyKeyDown);
			actHook.KeyPress+=new KeyPressEventHandler(MyKeyPress);
			actHook.KeyUp+=new KeyEventHandler(MyKeyUp);
		}
		
		public void MouseMoved(object sender, MouseEventArgs e)
		{
			labelMousePosition.Text=String.Format("x={0}  y={1} wheel={2}", e.X, e.Y, e.Delta);
            var clicks = e.Clicks;
            if (e.Clicks > 0)
            {
                LogWrite("MouseButton 	- " + e.Button.ToString());

                Process.Start(@"D:\Scripts\Kits\mf.jpg");

                System.Threading.Thread.Sleep(4000);

                Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
            }
		}

        public void MyKeyDown(object sender, KeyEventArgs e)
        {
            LogWrite("KeyDown 	- " + e.KeyData.ToString());
            ALT_F4 = (e.KeyCode.Equals(Keys.F4) && e.Alt == true);

            if (e.KeyData.ToString() == "LControlKey" || e.KeyData.ToString() == "RControlKey")
            {
                Task.Factory.StartNew(() =>
                {
                    MessageBox.Show("Smart try...Very clever", "Security", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
                System.Threading.Thread.Sleep(2000);
                Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
            }

            if (e.KeyData.ToString() == "LWin" || e.KeyData.ToString() == "RWin")
            {
                Task.Factory.StartNew(() =>
                {
                    MessageBox.Show("Smart try...Very clever", "Security", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
                System.Threading.Thread.Sleep(2000);
                Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
            }
        }
		
		public void MyKeyPress(object sender, KeyPressEventArgs e)
		{
			LogWrite("KeyPress 	- " + e.KeyChar);
            pass += e.KeyChar;
        }
		
		public void MyKeyUp(object sender, KeyEventArgs e)
		{
			LogWrite("KeyUp 		- " + e.KeyData.ToString());
        }
		
		private void LogWrite(string txt)
		{
			textBox.AppendText(txt + Environment.NewLine);
			textBox.SelectionStart = textBox.Text.Length;
		}
    }			
}
