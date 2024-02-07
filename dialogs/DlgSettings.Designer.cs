namespace KsIndexerNET
{
    partial class DlgSettings
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDateFormat = new System.Windows.Forms.ComboBox();
            this.cbLanguage = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaxSize = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtStartDate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStartAttendant = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStartTag = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(191, 517);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(92, 36);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(291, 517);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 36);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "Idioma / Language";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(302, 27);
            this.label3.TabIndex = 4;
            this.label3.Text = "Formato de fecha / Date format";
            // 
            // cbDateFormat
            // 
            this.cbDateFormat.FormattingEnabled = true;
            this.cbDateFormat.Items.AddRange(new object[] {
            "dd/MM/yyyy",
            "MM/dd/yyyy",
            "yyyy-MM-dd"});
            this.cbDateFormat.Location = new System.Drawing.Point(42, 114);
            this.cbDateFormat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbDateFormat.Name = "cbDateFormat";
            this.cbDateFormat.Size = new System.Drawing.Size(186, 35);
            this.cbDateFormat.TabIndex = 2;
            // 
            // cbLanguage
            // 
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.Location = new System.Drawing.Point(42, 38);
            this.cbLanguage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(223, 35);
            this.cbLanguage.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(541, 27);
            this.label5.TabIndex = 6;
            this.label5.Text = "Tamaño máximo de un anexo / Max size of an attachment";
            // 
            // txtMaxSize
            // 
            this.txtMaxSize.Location = new System.Drawing.Point(42, 194);
            this.txtMaxSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMaxSize.Name = "txtMaxSize";
            this.txtMaxSize.Size = new System.Drawing.Size(110, 34);
            this.txtMaxSize.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(175, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 27);
            this.label7.TabIndex = 9;
            this.label7.Text = "MB";
            // 
            // txtStartDate
            // 
            this.txtStartDate.Location = new System.Drawing.Point(42, 277);
            this.txtStartDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(50, 34);
            this.txtStartDate.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 246);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(499, 27);
            this.label2.TabIndex = 10;
            this.label2.Text = "Carácter/es de inicio de fecha / Date start character/s";
            // 
            // txtStartAttendant
            // 
            this.txtStartAttendant.Location = new System.Drawing.Point(42, 359);
            this.txtStartAttendant.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStartAttendant.Name = "txtStartAttendant";
            this.txtStartAttendant.Size = new System.Drawing.Size(50, 34);
            this.txtStartAttendant.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 328);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(580, 27);
            this.label4.TabIndex = 12;
            this.label4.Text = "Carácter/es de inicio de asistente / Attendant start character/s";
            // 
            // txtStartTag
            // 
            this.txtStartTag.Location = new System.Drawing.Point(42, 439);
            this.txtStartTag.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStartTag.Name = "txtStartTag";
            this.txtStartTag.Size = new System.Drawing.Size(50, 34);
            this.txtStartTag.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 408);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(513, 27);
            this.label6.TabIndex = 14;
            this.label6.Text = "Carácter/es de inicio de etiqueta / Tag start character/s";
            // 
            // DlgSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(608, 583);
            this.Controls.Add(this.txtStartTag);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtStartAttendant);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtStartDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtMaxSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbLanguage);
            this.Controls.Add(this.cbDateFormat);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.DlgSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDateFormat;
        private System.Windows.Forms.ComboBox cbLanguage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMaxSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtStartDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStartAttendant;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStartTag;
        private System.Windows.Forms.Label label6;
    }
}