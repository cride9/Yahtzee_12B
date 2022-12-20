namespace KockaPoker
{
    partial class Yahtzee
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
            this.throwButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // throwButton
            // 
            this.throwButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.throwButton.Location = new System.Drawing.Point(12, 686);
            this.throwButton.Name = "throwButton";
            this.throwButton.Size = new System.Drawing.Size(274, 55);
            this.throwButton.TabIndex = 0;
            this.throwButton.Text = "Dobás";
            this.throwButton.UseVisualStyleBackColor = true;
            this.throwButton.Click += new System.EventHandler(this.throwButton_Click);
            // 
            // Yahtzee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(582, 753);
            this.Controls.Add(this.throwButton);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.MaximumSize = new System.Drawing.Size(600, 800);
            this.MinimumSize = new System.Drawing.Size(600, 800);
            this.Name = "Yahtzee";
            this.ShowIcon = false;
            this.Text = "KockaPóker";
            this.ResumeLayout(false);

        }

        #endregion

        private Button throwButton;
    }
}