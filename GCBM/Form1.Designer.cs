namespace GCBM
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.cbScrub = new System.Windows.Forms.CheckBox();
            this.cbDelete = new System.Windows.Forms.CheckBox();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.gbSource = new System.Windows.Forms.GroupBox();
            this.gbDest = new System.Windows.Forms.GroupBox();
            this.btnBrowseDestination = new System.Windows.Forms.Button();
            this.pbGameBox = new System.Windows.Forms.PictureBox();
            this.pbGameCover3D = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.gbSource.SuspendLayout();
            this.gbDest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameCover3D)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.pbGameCover3D);
            this.panel1.Controls.Add(this.pbGameBox);
            this.panel1.Controls.Add(this.btnTransfer);
            this.panel1.Controls.Add(this.cbScrub);
            this.panel1.Controls.Add(this.cbDelete);
            this.panel1.Location = new System.Drawing.Point(380, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 404);
            this.panel1.TabIndex = 4;
            // 
            // btnTransfer
            // 
            this.btnTransfer.Location = new System.Drawing.Point(3, 369);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(211, 32);
            this.btnTransfer.TabIndex = 6;
            this.btnTransfer.Text = "Start Transfer";
            this.btnTransfer.UseVisualStyleBackColor = true;
            // 
            // cbScrub
            // 
            this.cbScrub.AutoSize = true;
            this.cbScrub.Location = new System.Drawing.Point(5, 346);
            this.cbScrub.Name = "cbScrub";
            this.cbScrub.Size = new System.Drawing.Size(109, 17);
            this.cbScrub.TabIndex = 4;
            this.cbScrub.Text = "Compress (Scrub)";
            this.cbScrub.UseVisualStyleBackColor = true;
            // 
            // cbDelete
            // 
            this.cbDelete.AutoSize = true;
            this.cbDelete.Location = new System.Drawing.Point(120, 346);
            this.cbDelete.Name = "cbDelete";
            this.cbDelete.Size = new System.Drawing.Size(94, 17);
            this.cbDelete.TabIndex = 3;
            this.cbDelete.Text = "Delete Source";
            this.cbDelete.UseVisualStyleBackColor = true;
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(0, 369);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(377, 32);
            this.btnBrowseSource.TabIndex = 5;
            this.btnBrowseSource.Text = "Choose Source Folder";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // gbSource
            // 
            this.gbSource.Controls.Add(this.btnBrowseSource);
            this.gbSource.Location = new System.Drawing.Point(0, 24);
            this.gbSource.Name = "gbSource";
            this.gbSource.Size = new System.Drawing.Size(377, 404);
            this.gbSource.TabIndex = 5;
            this.gbSource.TabStop = false;
            this.gbSource.Text = "Source";
            // 
            // gbDest
            // 
            this.gbDest.Controls.Add(this.btnBrowseDestination);
            this.gbDest.Location = new System.Drawing.Point(607, 24);
            this.gbDest.Name = "gbDest";
            this.gbDest.Size = new System.Drawing.Size(377, 404);
            this.gbDest.TabIndex = 6;
            this.gbDest.TabStop = false;
            this.gbDest.Text = "Destination";
            // 
            // btnBrowseDestination
            // 
            this.btnBrowseDestination.Location = new System.Drawing.Point(0, 369);
            this.btnBrowseDestination.Name = "btnBrowseDestination";
            this.btnBrowseDestination.Size = new System.Drawing.Size(377, 32);
            this.btnBrowseDestination.TabIndex = 6;
            this.btnBrowseDestination.Text = "Choose Destination Folder";
            this.btnBrowseDestination.UseVisualStyleBackColor = true;
            // 
            // pbGameBox
            // 
            this.pbGameBox.Location = new System.Drawing.Point(5, 4);
            this.pbGameBox.Name = "pbGameBox";
            this.pbGameBox.Size = new System.Drawing.Size(216, 165);
            this.pbGameBox.TabIndex = 7;
            this.pbGameBox.TabStop = false;
            // 
            // pbGameCover3D
            // 
            this.pbGameCover3D.Location = new System.Drawing.Point(5, 175);
            this.pbGameCover3D.Name = "pbGameCover3D";
            this.pbGameCover3D.Size = new System.Drawing.Size(216, 165);
            this.pbGameCover3D.TabIndex = 8;
            this.pbGameCover3D.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 450);
            this.Controls.Add(this.gbDest);
            this.Controls.Add(this.gbSource);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbSource.ResumeLayout(false);
            this.gbDest.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameCover3D)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.CheckBox cbScrub;
        private System.Windows.Forms.CheckBox cbDelete;
        private System.Windows.Forms.GroupBox gbSource;
        private System.Windows.Forms.GroupBox gbDest;
        private System.Windows.Forms.Button btnBrowseDestination;
        private System.Windows.Forms.PictureBox pbGameCover3D;
        private System.Windows.Forms.PictureBox pbGameBox;
    }
}