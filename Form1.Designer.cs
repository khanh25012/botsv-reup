namespace botKMT
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
            txtLog = new TextBox();
            cmd1 = new Button();
            txt1 = new TextBox();
            SuspendLayout();
            // 
            // txtLog
            // 
            txtLog.Location = new Point(28, 11);
            txtLog.Margin = new Padding(3, 2, 3, 2);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(478, 62);
            txtLog.TabIndex = 0;
            txtLog.TextChanged += txtLog_TextChanged;
            // 
            // cmd1
            // 
            cmd1.Location = new Point(564, 260);
            cmd1.Margin = new Padding(3, 2, 3, 2);
            cmd1.Name = "cmd1";
            cmd1.Size = new Size(125, 37);
            cmd1.TabIndex = 1;
            cmd1.Text = "do something";
            cmd1.UseVisualStyleBackColor = true;
            cmd1.Click += cmd1_Click;
            // 
            // txt1
            // 
            txt1.Location = new Point(10, 99);
            txt1.Margin = new Padding(3, 2, 3, 2);
            txt1.Multiline = true;
            txt1.Name = "txt1";
            txt1.Size = new Size(535, 192);
            txt1.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(txt1);
            Controls.Add(cmd1);
            Controls.Add(txtLog);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtLog;
        private Button cmd1;
        private TextBox txt1;
    }
}