namespace mxd.DukeBuilder.Controls
{
	partial class StatisticsControl
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
			this.thingscount = new System.Windows.Forms.Label();
			this.sectorscount = new System.Windows.Forms.Label();
			this.wallscount = new System.Windows.Forms.Label();
			this.thingslabel = new System.Windows.Forms.Label();
			this.sectorslabel = new System.Windows.Forms.Label();
			this.wallslabel = new System.Windows.Forms.Label();
			this.linescount = new System.Windows.Forms.Label();
			this.lineslabel = new System.Windows.Forms.Label();
			this.tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.vertslabel = new System.Windows.Forms.Label();
			this.vertscount = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// thingscount
			// 
			this.thingscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.thingscount.Location = new System.Drawing.Point(4, 74);
			this.thingscount.Name = "thingscount";
			this.thingscount.Size = new System.Drawing.Size(120, 14);
			this.thingscount.TabIndex = 19;
			this.thingscount.Text = "1024 / 1024 / 1024";
			this.thingscount.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.tooltip.SetToolTip(this.thingscount, "Selected / created / maximum amount of sprites");
			// 
			// sectorscount
			// 
			this.sectorscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.sectorscount.Location = new System.Drawing.Point(4, 56);
			this.sectorscount.Name = "sectorscount";
			this.sectorscount.Size = new System.Drawing.Size(120, 14);
			this.sectorscount.TabIndex = 18;
			this.sectorscount.Text = "1024 / 1024 / 1024";
			this.sectorscount.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.tooltip.SetToolTip(this.sectorscount, "Selected / created / maximum amount of sectors");
			// 
			// wallscount
			// 
			this.wallscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.wallscount.Location = new System.Drawing.Point(4, 20);
			this.wallscount.Name = "wallscount";
			this.wallscount.Size = new System.Drawing.Size(120, 14);
			this.wallscount.TabIndex = 17;
			this.wallscount.Text = "1024 / 1024 / 1024";
			this.wallscount.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.tooltip.SetToolTip(this.wallscount, "Selected / created / maximum amount of walls");
			// 
			// thingslabel
			// 
			this.thingslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.thingslabel.Location = new System.Drawing.Point(126, 74);
			this.thingslabel.Name = "thingslabel";
			this.thingslabel.Size = new System.Drawing.Size(50, 15);
			this.thingslabel.TabIndex = 14;
			this.thingslabel.Text = "Sprites";
			// 
			// sectorslabel
			// 
			this.sectorslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.sectorslabel.Location = new System.Drawing.Point(126, 56);
			this.sectorslabel.Name = "sectorslabel";
			this.sectorslabel.Size = new System.Drawing.Size(50, 15);
			this.sectorslabel.TabIndex = 13;
			this.sectorslabel.Text = "Sectors";
			// 
			// wallslabel
			// 
			this.wallslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.wallslabel.Location = new System.Drawing.Point(126, 20);
			this.wallslabel.Name = "wallslabel";
			this.wallslabel.Size = new System.Drawing.Size(50, 15);
			this.wallslabel.TabIndex = 12;
			this.wallslabel.Text = "Walls";
			// 
			// linescount
			// 
			this.linescount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.linescount.Location = new System.Drawing.Point(4, 38);
			this.linescount.Name = "linescount";
			this.linescount.Size = new System.Drawing.Size(120, 14);
			this.linescount.TabIndex = 21;
			this.linescount.Text = "1024 / 1024 / 1024";
			this.linescount.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.tooltip.SetToolTip(this.linescount, "Selected / created / maximum amount of lines");
			// 
			// lineslabel
			// 
			this.lineslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lineslabel.Location = new System.Drawing.Point(126, 38);
			this.lineslabel.Name = "lineslabel";
			this.lineslabel.Size = new System.Drawing.Size(50, 15);
			this.lineslabel.TabIndex = 20;
			this.lineslabel.Text = "Lines";
			// 
			// vertslabel
			// 
			this.vertslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.vertslabel.Location = new System.Drawing.Point(126, 2);
			this.vertslabel.Name = "vertslabel";
			this.vertslabel.Size = new System.Drawing.Size(50, 15);
			this.vertslabel.TabIndex = 22;
			this.vertslabel.Text = "Verts";
			// 
			// vertscount
			// 
			this.vertscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.vertscount.Location = new System.Drawing.Point(4, 2);
			this.vertscount.Name = "vertscount";
			this.vertscount.Size = new System.Drawing.Size(120, 14);
			this.vertscount.TabIndex = 23;
			this.vertscount.Text = "1024 / 1024 / 1024";
			this.vertscount.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.tooltip.SetToolTip(this.vertscount, "Selected / created / maximum amount of walls");
			// 
			// StatisticsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.vertscount);
			this.Controls.Add(this.vertslabel);
			this.Controls.Add(this.linescount);
			this.Controls.Add(this.lineslabel);
			this.Controls.Add(this.thingscount);
			this.Controls.Add(this.sectorscount);
			this.Controls.Add(this.wallscount);
			this.Controls.Add(this.thingslabel);
			this.Controls.Add(this.sectorslabel);
			this.Controls.Add(this.wallslabel);
			this.ForeColor = System.Drawing.SystemColors.GrayText;
			this.Name = "StatisticsControl";
			this.Size = new System.Drawing.Size(180, 88);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label thingscount;
		private System.Windows.Forms.Label sectorscount;
		private System.Windows.Forms.Label wallscount;
		private System.Windows.Forms.Label thingslabel;
		private System.Windows.Forms.Label sectorslabel;
		private System.Windows.Forms.Label wallslabel;
		private System.Windows.Forms.Label linescount;
		private System.Windows.Forms.Label lineslabel;
		private System.Windows.Forms.ToolTip tooltip;
		private System.Windows.Forms.Label vertslabel;
		private System.Windows.Forms.Label vertscount;
	}
}
