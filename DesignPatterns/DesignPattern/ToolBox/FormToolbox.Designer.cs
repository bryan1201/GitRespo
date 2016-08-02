namespace ToolBox
{
    partial class FormToolbox
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
            this.btnS1 = new System.Windows.Forms.Button();
            this.btnS2 = new System.Windows.Forms.Button();
            this.bthIsEqual = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnS1
            // 
            this.btnS1.Location = new System.Drawing.Point(56, 48);
            this.btnS1.Name = "btnS1";
            this.btnS1.Size = new System.Drawing.Size(119, 35);
            this.btnS1.TabIndex = 0;
            this.btnS1.Text = "s1";
            this.btnS1.UseVisualStyleBackColor = true;
            this.btnS1.Click += new System.EventHandler(this.btnS1_Click);
            // 
            // btnS2
            // 
            this.btnS2.Location = new System.Drawing.Point(56, 114);
            this.btnS2.Name = "btnS2";
            this.btnS2.Size = new System.Drawing.Size(119, 29);
            this.btnS2.TabIndex = 1;
            this.btnS2.Text = "s2";
            this.btnS2.UseVisualStyleBackColor = true;
            this.btnS2.Click += new System.EventHandler(this.btnS2_Click);
            // 
            // bthIsEqual
            // 
            this.bthIsEqual.Location = new System.Drawing.Point(12, 169);
            this.bthIsEqual.Name = "bthIsEqual";
            this.bthIsEqual.Size = new System.Drawing.Size(259, 60);
            this.bthIsEqual.TabIndex = 2;
            this.bthIsEqual.Text = "(S1==S2)?true:false;";
            this.bthIsEqual.UseVisualStyleBackColor = true;
            this.bthIsEqual.Click += new System.EventHandler(this.bthIsEqual_Click);
            // 
            // FormToolbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 253);
            this.Controls.Add(this.bthIsEqual);
            this.Controls.Add(this.btnS2);
            this.Controls.Add(this.btnS1);
            this.Name = "FormToolbox";
            this.Text = "FormToolbox";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnS1;
        private System.Windows.Forms.Button btnS2;
        private System.Windows.Forms.Button bthIsEqual;
    }
}