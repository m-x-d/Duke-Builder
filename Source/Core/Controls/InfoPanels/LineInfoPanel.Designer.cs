namespace mxd.DukeBuilder.Controls
{
	partial class LineInfoPanel
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
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label2;
			this.infopanel = new System.Windows.Forms.GroupBox();
			this.angle = new System.Windows.Forms.Label();
			this.length = new System.Windows.Forms.Label();
			this.front = new mxd.DukeBuilder.Controls.WallInfoControl();
			this.back = new mxd.DukeBuilder.Controls.WallInfoControl();
			label3 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			this.infopanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(14, 34);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(37, 13);
			label3.TabIndex = 3;
			label3.Text = "Angle:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(9, 17);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(43, 13);
			label2.TabIndex = 2;
			label2.Text = "Length:";
			// 
			// infopanel
			// 
			this.infopanel.Controls.Add(this.angle);
			this.infopanel.Controls.Add(this.length);
			this.infopanel.Controls.Add(label3);
			this.infopanel.Controls.Add(label2);
			this.infopanel.Location = new System.Drawing.Point(3, 0);
			this.infopanel.Name = "infopanel";
			this.infopanel.Size = new System.Drawing.Size(103, 97);
			this.infopanel.TabIndex = 1;
			this.infopanel.TabStop = false;
			this.infopanel.Text = " Line ";
			// 
			// angle
			// 
			this.angle.AutoSize = true;
			this.angle.Location = new System.Drawing.Point(55, 34);
			this.angle.Name = "angle";
			this.angle.Size = new System.Drawing.Size(25, 13);
			this.angle.TabIndex = 6;
			this.angle.Text = "360";
			// 
			// length
			// 
			this.length.AutoSize = true;
			this.length.Location = new System.Drawing.Point(55, 17);
			this.length.Name = "length";
			this.length.Size = new System.Drawing.Size(31, 13);
			this.length.TabIndex = 5;
			this.length.Text = "1024";
			// 
			// front
			// 
			this.front.Location = new System.Drawing.Point(109, 0);
			this.front.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
			this.front.Name = "front";
			this.front.Size = new System.Drawing.Size(356, 100);
			this.front.TabIndex = 9;
			// 
			// back
			// 
			this.back.Location = new System.Drawing.Point(471, 0);
			this.back.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.back.Name = "back";
			this.back.Size = new System.Drawing.Size(356, 100);
			this.back.TabIndex = 0;
			// 
			// LineInfoPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.front);
			this.Controls.Add(this.back);
			this.Controls.Add(this.infopanel);
			this.DoubleBuffered = true;
			this.MaximumSize = new System.Drawing.Size(10000, 100);
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "LineInfoPanel";
			this.Size = new System.Drawing.Size(1047, 100);
			this.infopanel.ResumeLayout(false);
			this.infopanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label angle;
		private System.Windows.Forms.Label length;
		private System.Windows.Forms.GroupBox infopanel;
		private WallInfoControl front;
		private WallInfoControl back;

	}
}
