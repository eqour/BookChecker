
namespace WFInterface
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.StartCheckButton = new System.Windows.Forms.Button();
            this.LinksCheckingProgressBar = new System.Windows.Forms.ProgressBar();
            this.ProcessShowPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ProcessShowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartCheckButton
            // 
            resources.ApplyResources(this.StartCheckButton, "StartCheckButton");
            this.StartCheckButton.Name = "StartCheckButton";
            this.StartCheckButton.UseVisualStyleBackColor = true;
            this.StartCheckButton.Click += new System.EventHandler(this.StartCheckButton_Click);
            // 
            // LinksCheckingProgressBar
            // 
            resources.ApplyResources(this.LinksCheckingProgressBar, "LinksCheckingProgressBar");
            this.LinksCheckingProgressBar.MarqueeAnimationSpeed = 10;
            this.LinksCheckingProgressBar.Name = "LinksCheckingProgressBar";
            // 
            // ProcessShowPanel
            // 
            resources.ApplyResources(this.ProcessShowPanel, "ProcessShowPanel");
            this.ProcessShowPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ProcessShowPanel.Controls.Add(this.label4);
            this.ProcessShowPanel.Controls.Add(this.label3);
            this.ProcessShowPanel.Controls.Add(this.label2);
            this.ProcessShowPanel.Controls.Add(this.label1);
            this.ProcessShowPanel.Name = "ProcessShowPanel";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ProcessShowPanel);
            this.Controls.Add(this.LinksCheckingProgressBar);
            this.Controls.Add(this.StartCheckButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ProcessShowPanel.ResumeLayout(false);
            this.ProcessShowPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartCheckButton;
        private System.Windows.Forms.ProgressBar LinksCheckingProgressBar;
        private System.Windows.Forms.Panel ProcessShowPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
    }
}

