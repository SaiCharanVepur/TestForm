namespace TestForm
{
    partial class Form1
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
            this.btnLoadPDF = new System.Windows.Forms.Button();
            this.pdfPictureBox = new System.Windows.Forms.PictureBox();
            this.btnCaptureSignature = new System.Windows.Forms.Button();
            this.signaturePictureBox = new System.Windows.Forms.PictureBox();
            this.btnSavePDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pdfPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.signaturePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadPDF
            // 
            this.btnLoadPDF.Location = new System.Drawing.Point(73, 74);
            this.btnLoadPDF.Name = "btnLoadPDF";
            this.btnLoadPDF.Size = new System.Drawing.Size(163, 23);
            this.btnLoadPDF.TabIndex = 0;
            this.btnLoadPDF.Text = "Load PDF";
            this.btnLoadPDF.UseVisualStyleBackColor = true;
            this.btnLoadPDF.Click += new System.EventHandler(this.btnLoadPDF_Click);
            // 
            // pdfPictureBox
            // 
            this.pdfPictureBox.Location = new System.Drawing.Point(55, 124);
            this.pdfPictureBox.Name = "pdfPictureBox";
            this.pdfPictureBox.Size = new System.Drawing.Size(204, 268);
            this.pdfPictureBox.TabIndex = 1;
            this.pdfPictureBox.TabStop = false;
           
            // 
            // btnCaptureSignature
            // 
            this.btnCaptureSignature.Location = new System.Drawing.Point(445, 107);
            this.btnCaptureSignature.Name = "btnCaptureSignature";
            this.btnCaptureSignature.Size = new System.Drawing.Size(91, 23);
            this.btnCaptureSignature.TabIndex = 2;
            this.btnCaptureSignature.Text = "Capture";
            this.btnCaptureSignature.UseVisualStyleBackColor = true;
            this.btnCaptureSignature.Click += new System.EventHandler(this.btnCaptureSignature_Click);
            // 
            // signaturePictureBox
            // 
            this.signaturePictureBox.Location = new System.Drawing.Point(370, 136);
            this.signaturePictureBox.Name = "signaturePictureBox";
            this.signaturePictureBox.Size = new System.Drawing.Size(240, 87);
            this.signaturePictureBox.TabIndex = 3;
            this.signaturePictureBox.TabStop = false;
            // 
            // btnSavePDF
            // 
            this.btnSavePDF.Location = new System.Drawing.Point(445, 348);
            this.btnSavePDF.Name = "btnSavePDF";
            this.btnSavePDF.Size = new System.Drawing.Size(91, 23);
            this.btnSavePDF.TabIndex = 5;
            this.btnSavePDF.Text = "Save PDF";
            this.btnSavePDF.UseVisualStyleBackColor = true;
            this.btnSavePDF.Click += new System.EventHandler(this.btnSavePDF_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSavePDF);
            this.Controls.Add(this.signaturePictureBox);
            this.Controls.Add(this.btnCaptureSignature);
            this.Controls.Add(this.pdfPictureBox);
            this.Controls.Add(this.btnLoadPDF);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pdfPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.signaturePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadPDF;
        private System.Windows.Forms.PictureBox pdfPictureBox;
        private System.Windows.Forms.Button btnCaptureSignature;
        private System.Windows.Forms.PictureBox signaturePictureBox;
        private System.Windows.Forms.Button btnSavePDF;

    }
}

