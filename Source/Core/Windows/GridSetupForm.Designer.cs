namespace mxd.DukeBuilder.Windows
{
	partial class GridSetupForm
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
			if(disposing && (components != null))
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
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.Label label1;
			this.gridsize = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.groupbackground = new System.Windows.Forms.GroupBox();
			this.backscaley = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.backscalex = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.backoffsety = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.backoffsetx = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.backscale = new System.Windows.Forms.Label();
			this.selectfile = new System.Windows.Forms.Button();
			this.backoffset = new System.Windows.Forms.Label();
			this.backgroundimage = new System.Windows.Forms.Panel();
			this.showbackground = new System.Windows.Forms.CheckBox();
			this.cancel = new System.Windows.Forms.Button();
			this.apply = new System.Windows.Forms.Button();
			this.browsefile = new System.Windows.Forms.OpenFileDialog();
			groupBox1 = new System.Windows.Forms.GroupBox();
			label1 = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			this.groupbackground.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			groupBox1.Controls.Add(this.gridsize);
			groupBox1.Controls.Add(label1);
			groupBox1.Location = new System.Drawing.Point(12, 12);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(272, 58);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = " Grid ";
			// 
			// gridsize
			// 
			this.gridsize.AllowDecimal = false;
			this.gridsize.AllowNegative = false;
			this.gridsize.AllowRelative = true;
			this.gridsize.ButtonStep = 8;
			this.gridsize.Location = new System.Drawing.Point(146, 20);
			this.gridsize.Name = "gridsize";
			this.gridsize.Size = new System.Drawing.Size(75, 24);
			this.gridsize.StepValues = null;
			this.gridsize.TabIndex = 1;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(25, 25);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(115, 14);
			label1.TabIndex = 0;
			label1.Text = "Grid size in mappixels:";
			// 
			// groupbackground
			// 
			this.groupbackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupbackground.Controls.Add(this.backscaley);
			this.groupbackground.Controls.Add(this.backscalex);
			this.groupbackground.Controls.Add(this.backoffsety);
			this.groupbackground.Controls.Add(this.backoffsetx);
			this.groupbackground.Controls.Add(this.backscale);
			this.groupbackground.Controls.Add(this.selectfile);
			this.groupbackground.Controls.Add(this.backoffset);
			this.groupbackground.Controls.Add(this.backgroundimage);
			this.groupbackground.Location = new System.Drawing.Point(12, 76);
			this.groupbackground.Name = "groupbackground";
			this.groupbackground.Size = new System.Drawing.Size(272, 201);
			this.groupbackground.TabIndex = 1;
			this.groupbackground.TabStop = false;
			this.groupbackground.Text = " Background ";
			// 
			// backscaley
			// 
			this.backscaley.AllowDecimal = false;
			this.backscaley.AllowNegative = false;
			this.backscaley.AllowRelative = true;
			this.backscaley.ButtonStep = 1;
			this.backscaley.Enabled = false;
			this.backscaley.Location = new System.Drawing.Point(195, 167);
			this.backscaley.Name = "backscaley";
			this.backscaley.Size = new System.Drawing.Size(67, 24);
			this.backscaley.StepValues = null;
			this.backscaley.TabIndex = 13;
			// 
			// backscalex
			// 
			this.backscalex.AllowDecimal = false;
			this.backscalex.AllowNegative = false;
			this.backscalex.AllowRelative = true;
			this.backscalex.ButtonStep = 1;
			this.backscalex.Enabled = false;
			this.backscalex.Location = new System.Drawing.Point(122, 167);
			this.backscalex.Name = "backscalex";
			this.backscalex.Size = new System.Drawing.Size(67, 24);
			this.backscalex.StepValues = null;
			this.backscalex.TabIndex = 12;
			// 
			// backoffsety
			// 
			this.backoffsety.AllowDecimal = false;
			this.backoffsety.AllowNegative = true;
			this.backoffsety.AllowRelative = true;
			this.backoffsety.ButtonStep = 1;
			this.backoffsety.Enabled = false;
			this.backoffsety.Location = new System.Drawing.Point(195, 128);
			this.backoffsety.Name = "backoffsety";
			this.backoffsety.Size = new System.Drawing.Size(67, 24);
			this.backoffsety.StepValues = null;
			this.backoffsety.TabIndex = 11;
			// 
			// backoffsetx
			// 
			this.backoffsetx.AllowDecimal = false;
			this.backoffsetx.AllowNegative = true;
			this.backoffsetx.AllowRelative = true;
			this.backoffsetx.ButtonStep = 1;
			this.backoffsetx.Enabled = false;
			this.backoffsetx.Location = new System.Drawing.Point(122, 128);
			this.backoffsetx.Name = "backoffsetx";
			this.backoffsetx.Size = new System.Drawing.Size(67, 24);
			this.backoffsetx.StepValues = null;
			this.backoffsetx.TabIndex = 10;
			// 
			// backscale
			// 
			this.backscale.AutoSize = true;
			this.backscale.Enabled = false;
			this.backscale.Location = new System.Drawing.Point(28, 172);
			this.backscale.Name = "backscale";
			this.backscale.Size = new System.Drawing.Size(88, 14);
			this.backscale.TabIndex = 9;
			this.backscale.Text = "Scale in percent:";
			// 
			// selectfile
			// 
			this.selectfile.Enabled = false;
			this.selectfile.Location = new System.Drawing.Point(109, 22);
			this.selectfile.Name = "selectfile";
			this.selectfile.Size = new System.Drawing.Size(117, 25);
			this.selectfile.TabIndex = 3;
			this.selectfile.Text = "Select File...";
			this.selectfile.UseVisualStyleBackColor = true;
			this.selectfile.Click += new System.EventHandler(this.selectfile_Click);
			// 
			// backoffset
			// 
			this.backoffset.AutoSize = true;
			this.backoffset.Enabled = false;
			this.backoffset.Location = new System.Drawing.Point(13, 133);
			this.backoffset.Name = "backoffset";
			this.backoffset.Size = new System.Drawing.Size(103, 14);
			this.backoffset.TabIndex = 4;
			this.backoffset.Text = "Offset in mappixels:";
			// 
			// backgroundimage
			// 
			this.backgroundimage.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.backgroundimage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.backgroundimage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.backgroundimage.Location = new System.Drawing.Point(12, 22);
			this.backgroundimage.Name = "backgroundimage";
			this.backgroundimage.Size = new System.Drawing.Size(91, 87);
			this.backgroundimage.TabIndex = 1;
			// 
			// showbackground
			// 
			this.showbackground.AutoSize = true;
			this.showbackground.Location = new System.Drawing.Point(24, 74);
			this.showbackground.Name = "showbackground";
			this.showbackground.Size = new System.Drawing.Size(84, 18);
			this.showbackground.TabIndex = 0;
			this.showbackground.Text = "Background";
			this.showbackground.UseVisualStyleBackColor = true;
			this.showbackground.CheckedChanged += new System.EventHandler(this.showbackground_CheckedChanged);
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(172, 284);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(112, 25);
			this.cancel.TabIndex = 3;
			this.cancel.Text = "Cancel";
			this.cancel.UseVisualStyleBackColor = true;
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// apply
			// 
			this.apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.apply.Location = new System.Drawing.Point(54, 284);
			this.apply.Name = "apply";
			this.apply.Size = new System.Drawing.Size(112, 25);
			this.apply.TabIndex = 2;
			this.apply.Text = "OK";
			this.apply.UseVisualStyleBackColor = true;
			this.apply.Click += new System.EventHandler(this.apply_Click);
			// 
			// browsefile
			// 
			this.browsefile.Filter = "All supported images|*.bmp;*.gif;*.png|All Files|*.*";
			this.browsefile.RestoreDirectory = true;
			this.browsefile.Title = "Select Background Image File";
			// 
			// GridSetupForm
			// 
			this.AcceptButton = this.apply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(296, 315);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.apply);
			this.Controls.Add(this.showbackground);
			this.Controls.Add(this.groupbackground);
			this.Controls.Add(groupBox1);
			this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GridSetupForm";
			this.Opacity = 0;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Custom Grid Setup";
			this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.GridSetupForm_HelpRequested);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			this.groupbackground.ResumeLayout(false);
			this.groupbackground.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel backgroundimage;
		private System.Windows.Forms.CheckBox showbackground;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button apply;
		private System.Windows.Forms.Label backoffset;
		private System.Windows.Forms.Button selectfile;
		private System.Windows.Forms.Label backscale;
		private System.Windows.Forms.OpenFileDialog browsefile;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox gridsize;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox backscaley;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox backscalex;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox backoffsety;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox backoffsetx;
		private System.Windows.Forms.GroupBox groupbackground;
	}
}