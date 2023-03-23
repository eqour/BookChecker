
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
            StartCheckButton = new System.Windows.Forms.Button();
            LinksCheckingProgressBar = new System.Windows.Forms.ProgressBar();
            ProcessShowPanel = new System.Windows.Forms.Panel();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            ProcessShowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // StartCheckButton
            // 
            resources.ApplyResources(StartCheckButton, "StartCheckButton");
            StartCheckButton.Name = "StartCheckButton";
            StartCheckButton.UseVisualStyleBackColor = true;
            StartCheckButton.Click += StartCheckButton_Click;
            // 
            // LinksCheckingProgressBar
            // 
            resources.ApplyResources(LinksCheckingProgressBar, "LinksCheckingProgressBar");
            LinksCheckingProgressBar.MarqueeAnimationSpeed = 10;
            LinksCheckingProgressBar.Name = "LinksCheckingProgressBar";
            // 
            // ProcessShowPanel
            // 
            resources.ApplyResources(ProcessShowPanel, "ProcessShowPanel");
            ProcessShowPanel.BackColor = System.Drawing.SystemColors.Control;
            ProcessShowPanel.Controls.Add(label4);
            ProcessShowPanel.Controls.Add(label3);
            ProcessShowPanel.Controls.Add(label2);
            ProcessShowPanel.Controls.Add(label1);
            ProcessShowPanel.Name = "ProcessShowPanel";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            label6.Name = "label6";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(ProcessShowPanel);
            Controls.Add(LinksCheckingProgressBar);
            Controls.Add(StartCheckButton);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            FormClosing += Form1_FormClosing;
            ProcessShowPanel.ResumeLayout(false);
            ProcessShowPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Label label6;
    }
}

