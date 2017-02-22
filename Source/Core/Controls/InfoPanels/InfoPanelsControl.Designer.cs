namespace mxd.DukeBuilder.Controls
{
	partial class InfoPanelsControl
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
			this.modename = new System.Windows.Forms.Label();
			this.labelcollapsedinfo = new System.Windows.Forms.Label();
			this.buttontoggleinfo = new System.Windows.Forms.Button();
			this.console = new mxd.DukeBuilder.DebugConsole();
			this.spriteinfo = new mxd.DukeBuilder.Controls.SpriteInfoPanel();
			this.sectorinfo = new mxd.DukeBuilder.Controls.SectorInfoPanel();
			this.lineinfo = new mxd.DukeBuilder.Controls.LineInfoPanel();
			this.vertexinfo = new mxd.DukeBuilder.Controls.VertexInfoPanel();
			this.statistics = new mxd.DukeBuilder.Controls.StatisticsControl();
			this.SuspendLayout();
			// 
			// modename
			// 
			this.modename.AutoSize = true;
			this.modename.Font = new System.Drawing.Font("Verdana", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.modename.ForeColor = System.Drawing.SystemColors.GrayText;
			this.modename.Location = new System.Drawing.Point(12, 20);
			this.modename.Name = "modename";
			this.modename.Size = new System.Drawing.Size(410, 59);
			this.modename.TabIndex = 5;
			this.modename.Text = "Vertices mode";
			this.modename.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.modename.Visible = false;
			// 
			// labelcollapsedinfo
			// 
			this.labelcollapsedinfo.AutoSize = true;
			this.labelcollapsedinfo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelcollapsedinfo.Location = new System.Drawing.Point(2, 2);
			this.labelcollapsedinfo.Name = "labelcollapsedinfo";
			this.labelcollapsedinfo.Size = new System.Drawing.Size(137, 13);
			this.labelcollapsedinfo.TabIndex = 8;
			this.labelcollapsedinfo.Text = "Collapsed Descriptions";
			this.labelcollapsedinfo.Visible = false;
			// 
			// buttontoggleinfo
			// 
			this.buttontoggleinfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttontoggleinfo.Font = new System.Drawing.Font("Marlett", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.buttontoggleinfo.Location = new System.Drawing.Point(1077, 1);
			this.buttontoggleinfo.Name = "buttontoggleinfo";
			this.buttontoggleinfo.Size = new System.Drawing.Size(22, 19);
			this.buttontoggleinfo.TabIndex = 7;
			this.buttontoggleinfo.TabStop = false;
			this.buttontoggleinfo.Tag = "dukebuilder_toggleinfopanel";
			this.buttontoggleinfo.Text = "6";
			this.buttontoggleinfo.UseVisualStyleBackColor = true;
			this.buttontoggleinfo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttontoggleinfo_MouseUp);
			// 
			// console
			// 
			this.console.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.console.Location = new System.Drawing.Point(0, 0);
			this.console.Name = "console";
			this.console.Size = new System.Drawing.Size(906, 100);
			this.console.TabIndex = 9;
			this.console.TabStop = false;
			// 
			// spriteinfo
			// 
			this.spriteinfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spriteinfo.Location = new System.Drawing.Point(0, 0);
			this.spriteinfo.MaximumSize = new System.Drawing.Size(10000, 100);
			this.spriteinfo.MinimumSize = new System.Drawing.Size(100, 100);
			this.spriteinfo.Name = "spriteinfo";
			this.spriteinfo.Size = new System.Drawing.Size(1100, 100);
			this.spriteinfo.TabIndex = 4;
			this.spriteinfo.TabStop = false;
			this.spriteinfo.Visible = false;
			// 
			// sectorinfo
			// 
			this.sectorinfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sectorinfo.Location = new System.Drawing.Point(0, 0);
			this.sectorinfo.MaximumSize = new System.Drawing.Size(10000, 100);
			this.sectorinfo.MinimumSize = new System.Drawing.Size(100, 100);
			this.sectorinfo.Name = "sectorinfo";
			this.sectorinfo.Size = new System.Drawing.Size(1100, 100);
			this.sectorinfo.TabIndex = 3;
			this.sectorinfo.TabStop = false;
			this.sectorinfo.Visible = false;
			// 
			// lineinfo
			// 
			this.lineinfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lineinfo.Location = new System.Drawing.Point(0, 0);
			this.lineinfo.MaximumSize = new System.Drawing.Size(10000, 100);
			this.lineinfo.MinimumSize = new System.Drawing.Size(100, 100);
			this.lineinfo.Name = "lineinfo";
			this.lineinfo.Size = new System.Drawing.Size(1100, 100);
			this.lineinfo.TabIndex = 2;
			this.lineinfo.TabStop = false;
			this.lineinfo.Visible = false;
			// 
			// vertexinfo
			// 
			this.vertexinfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vertexinfo.Location = new System.Drawing.Point(0, 0);
			this.vertexinfo.MaximumSize = new System.Drawing.Size(10000, 100);
			this.vertexinfo.MinimumSize = new System.Drawing.Size(100, 100);
			this.vertexinfo.Name = "vertexinfo";
			this.vertexinfo.Size = new System.Drawing.Size(1100, 100);
			this.vertexinfo.TabIndex = 1;
			this.vertexinfo.TabStop = false;
			this.vertexinfo.Visible = false;
			// 
			// statistics
			// 
			this.statistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.statistics.ForeColor = System.Drawing.SystemColors.GrayText;
			this.statistics.Location = new System.Drawing.Point(912, 6);
			this.statistics.Name = "statistics";
			this.statistics.Size = new System.Drawing.Size(180, 88);
			this.statistics.TabIndex = 10;
			this.statistics.TabStop = false;
			this.statistics.Visible = false;
			// 
			// InfoPanelsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.console);
			this.Controls.Add(this.labelcollapsedinfo);
			this.Controls.Add(this.buttontoggleinfo);
			this.Controls.Add(this.modename);
			this.Controls.Add(this.statistics);
			this.Controls.Add(this.spriteinfo);
			this.Controls.Add(this.sectorinfo);
			this.Controls.Add(this.lineinfo);
			this.Controls.Add(this.vertexinfo);
			this.Name = "InfoPanelsControl";
			this.Size = new System.Drawing.Size(1100, 100);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private VertexInfoPanel vertexinfo;
		private LineInfoPanel lineinfo;
		private SectorInfoPanel sectorinfo;
		private SpriteInfoPanel spriteinfo;
		private System.Windows.Forms.Label modename;
		private System.Windows.Forms.Label labelcollapsedinfo;
		private System.Windows.Forms.Button buttontoggleinfo;
		private DebugConsole console;
		private StatisticsControl statistics;

	}
}
