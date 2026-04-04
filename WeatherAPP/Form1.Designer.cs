namespace WeatherAPP
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            guna2ProgressIndicator1 = new Guna.UI2.WinForms.Guna2ProgressIndicator();
            guna2Panel1.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.Controls.Add(guna2ProgressIndicator1);
            guna2Panel1.CustomizableEdges = customizableEdges2;
            guna2Panel1.Location = new Point(12, 12);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges3;
            guna2Panel1.Size = new Size(1185, 651);
            guna2Panel1.TabIndex = 0;
            // 
            // guna2ProgressIndicator1
            // 
            guna2ProgressIndicator1.Location = new Point(536, 244);
            guna2ProgressIndicator1.Name = "guna2ProgressIndicator1";
            guna2ProgressIndicator1.ShadowDecoration.CustomizableEdges = customizableEdges1;
            guna2ProgressIndicator1.Size = new Size(135, 135);
            guna2ProgressIndicator1.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1209, 675);
            Controls.Add(guna2Panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Weather App";
            guna2Panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2ProgressIndicator guna2ProgressIndicator1;
    }
}
