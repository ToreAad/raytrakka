namespace RaytrAkkar.Winforms
{
    partial class RaytrAkkaForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBoxRender = new System.Windows.Forms.PictureBox();
            this.comboBoxScene = new System.Windows.Forms.ComboBox();
            this.richTextBox_lispScene = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRender)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(15, 459);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Render";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBoxRender
            // 
            this.pictureBoxRender.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxRender.Location = new System.Drawing.Point(15, 11);
            this.pictureBoxRender.Name = "pictureBoxRender";
            this.pictureBoxRender.Size = new System.Drawing.Size(446, 333);
            this.pictureBoxRender.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxRender.TabIndex = 1;
            this.pictureBoxRender.TabStop = false;
            // 
            // comboBoxScene
            // 
            this.comboBoxScene.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxScene.FormattingEnabled = true;
            this.comboBoxScene.Location = new System.Drawing.Point(110, 459);
            this.comboBoxScene.Name = "comboBoxScene";
            this.comboBoxScene.Size = new System.Drawing.Size(351, 23);
            this.comboBoxScene.TabIndex = 2;
            this.comboBoxScene.SelectedIndexChanged += new System.EventHandler(this.comboBoxScene_SelectedIndexChanged);
            // 
            // richTextBox_lispScene
            // 
            this.richTextBox_lispScene.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_lispScene.Location = new System.Drawing.Point(15, 349);
            this.richTextBox_lispScene.Name = "richTextBox_lispScene";
            this.richTextBox_lispScene.Size = new System.Drawing.Size(445, 104);
            this.richTextBox_lispScene.TabIndex = 3;
            this.richTextBox_lispScene.Text = "(none 0 0)";
            // 
            // RaytrAkkaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 494);
            this.Controls.Add(this.richTextBox_lispScene);
            this.Controls.Add(this.comboBoxScene);
            this.Controls.Add(this.pictureBoxRender);
            this.Controls.Add(this.button1);
            this.Name = "RaytrAkkaForm";
            this.Text = "RaytrAKKA";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRender)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBoxRender;
        private System.Windows.Forms.ComboBox comboBoxScene;
        private System.Windows.Forms.RichTextBox richTextBox_lispScene;
    }
}

