using System.Drawing;

namespace Stopwatch.View
{

    partial class WInFormsView : IView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartStopButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.RunningRadioButton = new System.Windows.Forms.RadioButton();
            this.StoppedRadioButton = new System.Windows.Forms.RadioButton();
            this.RunStateGroupBox = new System.Windows.Forms.GroupBox();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.MessageBorder = new System.Windows.Forms.GroupBox();
            this.ElapsedTimeLabel = new System.Windows.Forms.RichTextBox();
            this.ElapsedTimeGroupbox = new System.Windows.Forms.GroupBox();
            this.RunStateGroupBox.SuspendLayout();
            this.MessageBorder.SuspendLayout();
            this.ElapsedTimeGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartStopButton
            // 
            this.StartStopButton.Location = new System.Drawing.Point(70, 77);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(82, 36);
            this.StartStopButton.TabIndex = 0;
            this.StartStopButton.Text = "&Start";
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.StartStopButtonPressed);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(200, 77);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(89, 36);
            this.ResetButton.TabIndex = 1;
            this.ResetButton.Text = "&Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButtonPressed);
            // 
            // RunningRadioButton
            // 
            this.RunningRadioButton.AutoSize = true;
            this.RunningRadioButton.ForeColor = System.Drawing.Color.Green;
            this.RunningRadioButton.Location = new System.Drawing.Point(5, 9);
            this.RunningRadioButton.Name = "RunningRadioButton";
            this.RunningRadioButton.Size = new System.Drawing.Size(14, 13);
            this.RunningRadioButton.TabIndex = 2;
            this.RunningRadioButton.UseVisualStyleBackColor = false;
            // 
            // StoppedRadioButton
            // 
            this.StoppedRadioButton.AutoSize = true;
            this.StoppedRadioButton.Location = new System.Drawing.Point(5, 28);
            this.StoppedRadioButton.Name = "StoppedRadioButton";
            this.StoppedRadioButton.Size = new System.Drawing.Size(14, 13);
            this.StoppedRadioButton.TabIndex = 3;
            this.StoppedRadioButton.UseVisualStyleBackColor = false;
            // 
            // RunStateGroupBox
            // 
            this.RunStateGroupBox.Controls.Add(this.StoppedRadioButton);
            this.RunStateGroupBox.Controls.Add(this.RunningRadioButton);
            this.RunStateGroupBox.Location = new System.Drawing.Point(12, 19);
            this.RunStateGroupBox.Name = "RunStateGroupBox";
            this.RunStateGroupBox.Size = new System.Drawing.Size(24, 46);
            this.RunStateGroupBox.TabIndex = 4;
            this.RunStateGroupBox.TabStop = false;
            // 
            // MessageLabel
            // 
            this.MessageLabel.Location = new System.Drawing.Point(10, 11);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(295, 19);
            this.MessageLabel.TabIndex = 6;
            this.MessageLabel.Text = "Message Here";
            // 
            // MessageBorder
            // 
            this.MessageBorder.Controls.Add(this.MessageLabel);
            this.MessageBorder.ForeColor = System.Drawing.Color.Black;
            this.MessageBorder.Location = new System.Drawing.Point(4, 120);
            this.MessageBorder.Name = "MessageBorder";
            this.MessageBorder.Size = new System.Drawing.Size(312, 32);
            this.MessageBorder.TabIndex = 7;
            this.MessageBorder.TabStop = false;
            // 
            // ElapsedTimeLabel
            // 
            this.ElapsedTimeLabel.Font = new System.Drawing.Font("Century", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ElapsedTimeLabel.Location = new System.Drawing.Point(7, 8);
            this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
            this.ElapsedTimeLabel.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.ElapsedTimeLabel.Size = new System.Drawing.Size(256, 46);
            this.ElapsedTimeLabel.TabIndex = 0;
            this.ElapsedTimeLabel.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.ElapsedTimeLabel.Rtf = @"{\rtf1\ansi{00:00:00\super.000\nosupersub}}";

            // 
            // ElapsedTimeGroupbox
            // 
            this.ElapsedTimeGroupbox.Controls.Add(this.ElapsedTimeLabel);
            this.ElapsedTimeGroupbox.ForeColor = System.Drawing.Color.Black;
            this.ElapsedTimeGroupbox.Location = new System.Drawing.Point(46, 12);
            this.ElapsedTimeGroupbox.Name = "ElapsedTimeGroupbox";
            this.ElapsedTimeGroupbox.Size = new System.Drawing.Size(270, 59);
            this.ElapsedTimeGroupbox.TabIndex = 5;
            this.ElapsedTimeGroupbox.TabStop = false;
            // 
            // WInFormsView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(327, 153);
            this.Controls.Add(this.MessageBorder);
            this.Controls.Add(this.ElapsedTimeGroupbox);
            this.Controls.Add(this.RunStateGroupBox);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.StartStopButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WInFormsView";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stopwatch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinFormsView_FormClosing);
            this.RunStateGroupBox.ResumeLayout(false);
            this.RunStateGroupBox.PerformLayout();
            this.MessageBorder.ResumeLayout(false);
            this.ElapsedTimeGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.RadioButton RunningRadioButton;
        private System.Windows.Forms.RadioButton StoppedRadioButton;
        private System.Windows.Forms.GroupBox RunStateGroupBox;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.GroupBox MessageBorder;
        private System.Windows.Forms.RichTextBox ElapsedTimeLabel;
        private System.Windows.Forms.GroupBox ElapsedTimeGroupbox;
    }
}

