namespace mxd.DukeBuilder.EditModes
{
	partial class DrawEllipseOptionsPanel
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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.subdivslabel = new System.Windows.Forms.ToolStripLabel();
			this.subdivs = new mxd.DukeBuilder.Controls.ToolStripNumericUpDown();
			this.anglelabel = new System.Windows.Forms.ToolStripLabel();
			this.angle = new mxd.DukeBuilder.Controls.ToolStripNumericUpDown();
			this.reset = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subdivslabel,
            this.subdivs,
            this.anglelabel,
            this.angle,
            this.reset});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(646, 25);
			this.toolStrip1.TabIndex = 6;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// subdivslabel
			// 
			this.subdivslabel.Name = "subdivslabel";
			this.subdivslabel.Size = new System.Drawing.Size(37, 22);
			this.subdivslabel.Text = "Sides:";
			// 
			// subdivs
			// 
			this.subdivs.AutoSize = false;
			this.subdivs.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.subdivs.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
			this.subdivs.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.subdivs.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.subdivs.Name = "subdivs";
			this.subdivs.Size = new System.Drawing.Size(56, 20);
			this.subdivs.Text = "0";
			this.subdivs.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// anglelabel
			// 
			this.anglelabel.Name = "anglelabel";
			this.anglelabel.Size = new System.Drawing.Size(41, 22);
			this.anglelabel.Text = "Angle:";
			// 
			// angle
			// 
			this.angle.AutoSize = false;
			this.angle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.angle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.angle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
			this.angle.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
			this.angle.Name = "angle";
			this.angle.Size = new System.Drawing.Size(56, 23);
			this.angle.Text = "0";
			this.angle.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// reset
			// 
			this.reset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.reset.Image = global::mxd.DukeBuilder.EditModes.Properties.Resources.Reset;
			this.reset.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.reset.Name = "reset";
			this.reset.Size = new System.Drawing.Size(23, 22);
			this.reset.Text = "Reset";
			this.reset.Click += new System.EventHandler(this.reset_Click);
			// 
			// DrawEllipseOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.toolStrip1);
			this.Name = "DrawEllipseOptionsPanel";
			this.Size = new System.Drawing.Size(646, 60);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel subdivslabel;
		private mxd.DukeBuilder.Controls.ToolStripNumericUpDown subdivs;
		private System.Windows.Forms.ToolStripButton reset;
		private System.Windows.Forms.ToolStripLabel anglelabel;
		private mxd.DukeBuilder.Controls.ToolStripNumericUpDown angle;
	}
}
