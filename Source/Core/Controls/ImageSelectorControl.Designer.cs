namespace mxd.DukeBuilder.Controls
{
	partial class ImageSelectorControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.preview = new mxd.DukeBuilder.Controls.ConfigurablePictureBox();
			this.name = new mxd.DukeBuilder.Controls.AutoSelectTextbox();
			this.updatetimer = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.preview)).BeginInit();
			this.SuspendLayout();
			// 
			// preview
			// 
			this.preview.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.preview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.preview.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
			this.preview.Cursor = System.Windows.Forms.Cursors.Hand;
			this.preview.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			this.preview.Location = new System.Drawing.Point(0, 0);
			this.preview.Name = "preview";
			this.preview.PageUnit = System.Drawing.GraphicsUnit.Pixel;
			this.preview.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
			this.preview.Size = new System.Drawing.Size(68, 60);
			this.preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.preview.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
			this.preview.TabIndex = 1;
			this.preview.TabStop = false;
			this.preview.MouseLeave += new System.EventHandler(this.preview_MouseLeave);
			this.preview.Click += new System.EventHandler(this.preview_Click);
			this.preview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.preview_MouseDown);
			this.preview.MouseEnter += new System.EventHandler(this.preview_MouseEnter);
			// 
			// name
			// 
			this.name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.name.Location = new System.Drawing.Point(0, 64);
			this.name.MaxLength = 8;
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(68, 20);
			this.name.TabIndex = 2;
			this.name.TextChanged += new System.EventHandler(this.name_TextChanged);
			// 
			// updatetimer
			// 
			this.updatetimer.Tick += new System.EventHandler(this.updatetimer_Tick);
			// 
			// ImageSelectorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.name);
			this.Controls.Add(this.preview);
			this.Name = "ImageSelectorControl";
			this.Size = new System.Drawing.Size(115, 136);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ImageSelectorControl_Layout);
			this.Resize += new System.EventHandler(this.ImageSelectorControl_Resize);
			((System.ComponentModel.ISupportInitialize)(this.preview)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected mxd.DukeBuilder.Controls.ConfigurablePictureBox preview;
		protected mxd.DukeBuilder.Controls.AutoSelectTextbox name;
		private System.Windows.Forms.Timer updatetimer;

	}
}
