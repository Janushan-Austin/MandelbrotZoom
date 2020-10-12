namespace MandelbrotZoom
{
    partial class MandelbrotZoom
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
            this.ZoomInButton = new System.Windows.Forms.Button();
            this.IterativeColoringButton = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ZoomInButton
            // 
            this.ZoomInButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZoomInButton.Location = new System.Drawing.Point(13, 13);
            this.ZoomInButton.Name = "ZoomInButton";
            this.ZoomInButton.Size = new System.Drawing.Size(150, 150);
            this.ZoomInButton.TabIndex = 0;
            this.ZoomInButton.Text = "Start";
            this.ZoomInButton.UseVisualStyleBackColor = true;
            this.ZoomInButton.Click += new System.EventHandler(this.ZoomIn);
            // 
            // IterativeColoringButton
            // 
            this.IterativeColoringButton.AutoSize = true;
            this.IterativeColoringButton.Location = new System.Drawing.Point(13, 190);
            this.IterativeColoringButton.Name = "IterativeColoringButton";
            this.IterativeColoringButton.Size = new System.Drawing.Size(105, 17);
            this.IterativeColoringButton.TabIndex = 1;
            this.IterativeColoringButton.Text = "Iterative Coloring";
            this.IterativeColoringButton.UseVisualStyleBackColor = true;
            this.IterativeColoringButton.CheckedChanged += new System.EventHandler(this.IterativeColoringButton_CheckedChanged);
            // 
            // MandelbrotZoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 1020);
            this.Controls.Add(this.IterativeColoringButton);
            this.Controls.Add(this.ZoomInButton);
            this.Name = "MandelbrotZoom";
            this.Text = "Mandelbrot Zoom";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ZoomInButton;
        private System.Windows.Forms.CheckBox IterativeColoringButton;
    }
}

