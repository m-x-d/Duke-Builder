namespace mxd.DukeBuilder.Controls
{
	partial class WallPropertiesControl
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
			System.Windows.Forms.Label label7;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label taglabel;
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.Label label8;
			System.Windows.Forms.Label label9;
			System.Windows.Forms.Label label10;
			this.groupflags = new System.Windows.Forms.GroupBox();
			this.firstwall = new System.Windows.Forms.CheckBox();
			this.flags = new mxd.DukeBuilder.Controls.CheckboxArrayControl();
			this.groupproperties = new System.Windows.Forms.GroupBox();
			this.tex = new mxd.DukeBuilder.Controls.ImageSelectorControl();
			this.maskedtex = new mxd.DukeBuilder.Controls.ImageSelectorControl();
			this.palette = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.shade = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.repeaty = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.repeatx = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.offsety = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.offsetx = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.groupid = new System.Windows.Forms.GroupBox();
			this.extra = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.lotag = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.hitag = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			label7 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			taglabel = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			label8 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			label10 = new System.Windows.Forms.Label();
			this.groupflags.SuspendLayout();
			this.groupproperties.SuspendLayout();
			this.groupid.SuspendLayout();
			this.SuspendLayout();
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(46, 80);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(34, 13);
			label7.TabIndex = 34;
			label7.Text = "Extra:";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(39, 52);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(41, 13);
			label4.TabIndex = 33;
			label4.Text = "LoTag:";
			// 
			// taglabel
			// 
			taglabel.AutoSize = true;
			taglabel.Location = new System.Drawing.Point(41, 24);
			taglabel.Name = "taglabel";
			taglabel.Size = new System.Drawing.Size(39, 13);
			taglabel.TabIndex = 32;
			taglabel.Text = "HiTag:";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(33, 24);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(48, 13);
			label1.TabIndex = 34;
			label1.Text = "Offset X:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(33, 52);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(48, 13);
			label2.TabIndex = 36;
			label2.Text = "Offset Y:";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(26, 108);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(55, 13);
			label3.TabIndex = 40;
			label3.Text = "Repeat Y:";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(26, 80);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(55, 13);
			label5.TabIndex = 38;
			label5.Text = "Repeat X:";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(33, 136);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(41, 13);
			label6.TabIndex = 42;
			label6.Text = "Shade:";
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(10, 164);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(71, 13);
			label8.TabIndex = 44;
			label8.Text = "Palette index:";
			// 
			// label9
			// 
			label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			label9.Location = new System.Drawing.Point(166, 13);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(128, 13);
			label9.TabIndex = 47;
			label9.Text = "Image:";
			label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label10
			// 
			label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			label10.Location = new System.Drawing.Point(300, 13);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(128, 13);
			label10.TabIndex = 48;
			label10.Text = "Masked image:";
			label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// groupflags
			// 
			this.groupflags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupflags.Controls.Add(this.firstwall);
			this.groupflags.Controls.Add(this.flags);
			this.groupflags.Location = new System.Drawing.Point(3, 3);
			this.groupflags.Name = "groupflags";
			this.groupflags.Size = new System.Drawing.Size(434, 126);
			this.groupflags.TabIndex = 0;
			this.groupflags.TabStop = false;
			this.groupflags.Text = " Flags ";
			// 
			// firstwall
			// 
			this.firstwall.AutoSize = true;
			this.firstwall.Location = new System.Drawing.Point(21, 100);
			this.firstwall.Name = "firstwall";
			this.firstwall.Size = new System.Drawing.Size(66, 17);
			this.firstwall.TabIndex = 1;
			this.firstwall.Text = "First wall";
			this.firstwall.UseVisualStyleBackColor = true;
			// 
			// flags
			// 
			this.flags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flags.AutoScroll = true;
			this.flags.Columns = 3;
			this.flags.Location = new System.Drawing.Point(13, 19);
			this.flags.Name = "flags";
			this.flags.Size = new System.Drawing.Size(415, 75);
			this.flags.TabIndex = 0;
			// 
			// groupproperties
			// 
			this.groupproperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupproperties.Controls.Add(label10);
			this.groupproperties.Controls.Add(label9);
			this.groupproperties.Controls.Add(this.tex);
			this.groupproperties.Controls.Add(this.maskedtex);
			this.groupproperties.Controls.Add(this.palette);
			this.groupproperties.Controls.Add(label8);
			this.groupproperties.Controls.Add(this.shade);
			this.groupproperties.Controls.Add(label6);
			this.groupproperties.Controls.Add(this.repeaty);
			this.groupproperties.Controls.Add(label3);
			this.groupproperties.Controls.Add(this.repeatx);
			this.groupproperties.Controls.Add(label5);
			this.groupproperties.Controls.Add(this.offsety);
			this.groupproperties.Controls.Add(label2);
			this.groupproperties.Controls.Add(this.offsetx);
			this.groupproperties.Controls.Add(label1);
			this.groupproperties.Location = new System.Drawing.Point(3, 135);
			this.groupproperties.Name = "groupproperties";
			this.groupproperties.Size = new System.Drawing.Size(434, 190);
			this.groupproperties.TabIndex = 1;
			this.groupproperties.TabStop = false;
			this.groupproperties.Text = " Properties ";
			// 
			// tex
			// 
			this.tex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tex.Location = new System.Drawing.Point(166, 30);
			this.tex.Name = "tex";
			this.tex.Size = new System.Drawing.Size(128, 152);
			this.tex.TabIndex = 46;
			this.tex.TextureName = "";
			// 
			// maskedtex
			// 
			this.maskedtex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.maskedtex.Location = new System.Drawing.Point(300, 30);
			this.maskedtex.Name = "maskedtex";
			this.maskedtex.Size = new System.Drawing.Size(128, 152);
			this.maskedtex.TabIndex = 45;
			this.maskedtex.TextureName = "";
			// 
			// palette
			// 
			this.palette.AllowDecimal = false;
			this.palette.AllowNegative = false;
			this.palette.AllowRelative = true;
			this.palette.ButtonStep = 1;
			this.palette.Location = new System.Drawing.Point(87, 159);
			this.palette.Name = "palette";
			this.palette.Size = new System.Drawing.Size(68, 24);
			this.palette.StepValues = null;
			this.palette.TabIndex = 43;
			// 
			// shade
			// 
			this.shade.AllowDecimal = false;
			this.shade.AllowNegative = false;
			this.shade.AllowRelative = true;
			this.shade.ButtonStep = 1;
			this.shade.Location = new System.Drawing.Point(87, 131);
			this.shade.Name = "shade";
			this.shade.Size = new System.Drawing.Size(68, 24);
			this.shade.StepValues = null;
			this.shade.TabIndex = 41;
			// 
			// repeaty
			// 
			this.repeaty.AllowDecimal = false;
			this.repeaty.AllowNegative = false;
			this.repeaty.AllowRelative = true;
			this.repeaty.ButtonStep = 1;
			this.repeaty.Location = new System.Drawing.Point(87, 103);
			this.repeaty.Name = "repeaty";
			this.repeaty.Size = new System.Drawing.Size(68, 24);
			this.repeaty.StepValues = null;
			this.repeaty.TabIndex = 39;
			// 
			// repeatx
			// 
			this.repeatx.AllowDecimal = false;
			this.repeatx.AllowNegative = false;
			this.repeatx.AllowRelative = true;
			this.repeatx.ButtonStep = 1;
			this.repeatx.Location = new System.Drawing.Point(87, 75);
			this.repeatx.Name = "repeatx";
			this.repeatx.Size = new System.Drawing.Size(68, 24);
			this.repeatx.StepValues = null;
			this.repeatx.TabIndex = 37;
			// 
			// offsety
			// 
			this.offsety.AllowDecimal = false;
			this.offsety.AllowNegative = false;
			this.offsety.AllowRelative = true;
			this.offsety.ButtonStep = 1;
			this.offsety.Location = new System.Drawing.Point(87, 47);
			this.offsety.Name = "offsety";
			this.offsety.Size = new System.Drawing.Size(68, 24);
			this.offsety.StepValues = null;
			this.offsety.TabIndex = 35;
			// 
			// offsetx
			// 
			this.offsetx.AllowDecimal = false;
			this.offsetx.AllowNegative = false;
			this.offsetx.AllowRelative = true;
			this.offsetx.ButtonStep = 1;
			this.offsetx.Location = new System.Drawing.Point(87, 19);
			this.offsetx.Name = "offsetx";
			this.offsetx.Size = new System.Drawing.Size(68, 24);
			this.offsetx.StepValues = null;
			this.offsetx.TabIndex = 33;
			// 
			// groupid
			// 
			this.groupid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupid.Controls.Add(this.extra);
			this.groupid.Controls.Add(label7);
			this.groupid.Controls.Add(this.lotag);
			this.groupid.Controls.Add(label4);
			this.groupid.Controls.Add(this.hitag);
			this.groupid.Controls.Add(taglabel);
			this.groupid.Location = new System.Drawing.Point(3, 331);
			this.groupid.Name = "groupid";
			this.groupid.Size = new System.Drawing.Size(434, 108);
			this.groupid.TabIndex = 2;
			this.groupid.TabStop = false;
			this.groupid.Text = " Identification ";
			// 
			// extra
			// 
			this.extra.AllowDecimal = false;
			this.extra.AllowNegative = true;
			this.extra.AllowRelative = true;
			this.extra.ButtonStep = 1;
			this.extra.Location = new System.Drawing.Point(87, 75);
			this.extra.Name = "extra";
			this.extra.Size = new System.Drawing.Size(68, 24);
			this.extra.StepValues = null;
			this.extra.TabIndex = 31;
			// 
			// lotag
			// 
			this.lotag.AllowDecimal = false;
			this.lotag.AllowNegative = false;
			this.lotag.AllowRelative = true;
			this.lotag.ButtonStep = 1;
			this.lotag.Location = new System.Drawing.Point(87, 47);
			this.lotag.Name = "lotag";
			this.lotag.Size = new System.Drawing.Size(68, 24);
			this.lotag.StepValues = null;
			this.lotag.TabIndex = 30;
			// 
			// hitag
			// 
			this.hitag.AllowDecimal = false;
			this.hitag.AllowNegative = false;
			this.hitag.AllowRelative = true;
			this.hitag.ButtonStep = 1;
			this.hitag.Location = new System.Drawing.Point(87, 19);
			this.hitag.Name = "hitag";
			this.hitag.Size = new System.Drawing.Size(68, 24);
			this.hitag.StepValues = null;
			this.hitag.TabIndex = 29;
			// 
			// WallPropertiesControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.groupid);
			this.Controls.Add(this.groupproperties);
			this.Controls.Add(this.groupflags);
			this.Name = "WallPropertiesControl";
			this.Size = new System.Drawing.Size(440, 446);
			this.groupflags.ResumeLayout(false);
			this.groupflags.PerformLayout();
			this.groupproperties.ResumeLayout(false);
			this.groupproperties.PerformLayout();
			this.groupid.ResumeLayout(false);
			this.groupid.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupflags;
		private CheckboxArrayControl flags;
		private System.Windows.Forms.GroupBox groupproperties;
		private System.Windows.Forms.GroupBox groupid;
		private ButtonsNumericTextbox palette;
		private ButtonsNumericTextbox shade;
		private ButtonsNumericTextbox repeaty;
		private ButtonsNumericTextbox repeatx;
		private ButtonsNumericTextbox offsety;
		private ButtonsNumericTextbox offsetx;
		private ButtonsNumericTextbox extra;
		private ButtonsNumericTextbox lotag;
		private ButtonsNumericTextbox hitag;
		private ImageSelectorControl tex;
		private ImageSelectorControl maskedtex;
		private System.Windows.Forms.CheckBox firstwall;
	}
}
