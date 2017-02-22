namespace mxd.DukeBuilder.Windows
{
	partial class EditSectorForm
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.GroupBox groupaction;
			System.Windows.Forms.Label label7;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label taglabel;
			System.Windows.Forms.GroupBox groupeffect;
			System.Windows.Forms.Label label12;
			System.Windows.Forms.Label label11;
			System.Windows.Forms.Label label10;
			System.Windows.Forms.Label label8;
			System.Windows.Forms.Label label9;
			System.Windows.Forms.GroupBox groupfloorceiling;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.GroupBox groupBox3;
			System.Windows.Forms.Label label13;
			System.Windows.Forms.Label label14;
			System.Windows.Forms.Label label15;
			System.Windows.Forms.Label label16;
			System.Windows.Forms.Label label17;
			this.extra = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.lotag = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.hitag = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.ceilslope = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.ceilpalette = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.ceilshade = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.ceiloffsety = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.ceiltex = new mxd.DukeBuilder.Controls.ImageSelectorControl();
			this.ceiloffsetx = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.visibility = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.floorheight = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.ceilheight = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.sectorheight = new System.Windows.Forms.Label();
			this.sectorheightlabel = new System.Windows.Forms.Label();
			this.floorslope = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.floorpalette = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.floorshade = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.flooroffsety = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.floortex = new mxd.DukeBuilder.Controls.ImageSelectorControl();
			this.flooroffsetx = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.cancel = new System.Windows.Forms.Button();
			this.apply = new System.Windows.Forms.Button();
			this.tabs = new System.Windows.Forms.TabControl();
			this.tabfloor = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.floorflags = new mxd.DukeBuilder.Controls.CheckboxArrayControl();
			this.tabceiling = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ceilflags = new mxd.DukeBuilder.Controls.CheckboxArrayControl();
			this.panel1 = new System.Windows.Forms.Panel();
			label1 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			groupaction = new System.Windows.Forms.GroupBox();
			label7 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			taglabel = new System.Windows.Forms.Label();
			groupeffect = new System.Windows.Forms.GroupBox();
			label12 = new System.Windows.Forms.Label();
			label11 = new System.Windows.Forms.Label();
			label10 = new System.Windows.Forms.Label();
			label8 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			groupfloorceiling = new System.Windows.Forms.GroupBox();
			label2 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			groupBox3 = new System.Windows.Forms.GroupBox();
			label13 = new System.Windows.Forms.Label();
			label14 = new System.Windows.Forms.Label();
			label15 = new System.Windows.Forms.Label();
			label16 = new System.Windows.Forms.Label();
			label17 = new System.Windows.Forms.Label();
			groupaction.SuspendLayout();
			groupeffect.SuspendLayout();
			groupfloorceiling.SuspendLayout();
			groupBox3.SuspendLayout();
			this.tabs.SuspendLayout();
			this.tabfloor.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabceiling.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.Location = new System.Drawing.Point(271, 18);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(83, 16);
			label1.TabIndex = 15;
			label1.Text = "Floor";
			label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			label3.Location = new System.Drawing.Point(363, 18);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(83, 16);
			label3.TabIndex = 14;
			label3.Text = "Ceiling";
			label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// groupaction
			// 
			groupaction.Controls.Add(this.extra);
			groupaction.Controls.Add(label7);
			groupaction.Controls.Add(this.lotag);
			groupaction.Controls.Add(label4);
			groupaction.Controls.Add(this.hitag);
			groupaction.Controls.Add(taglabel);
			groupaction.Location = new System.Drawing.Point(12, 177);
			groupaction.Name = "groupaction";
			groupaction.Size = new System.Drawing.Size(194, 160);
			groupaction.TabIndex = 2;
			groupaction.TabStop = false;
			groupaction.Text = " Identification ";
			// 
			// extra
			// 
			this.extra.AllowDecimal = false;
			this.extra.AllowNegative = true;
			this.extra.AllowRelative = true;
			this.extra.ButtonStep = 1;
			this.extra.Location = new System.Drawing.Point(98, 83);
			this.extra.Name = "extra";
			this.extra.Size = new System.Drawing.Size(88, 24);
			this.extra.StepValues = null;
			this.extra.TabIndex = 2;
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(57, 88);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(34, 13);
			label7.TabIndex = 28;
			label7.Text = "Extra:";
			// 
			// lotag
			// 
			this.lotag.AllowDecimal = false;
			this.lotag.AllowNegative = false;
			this.lotag.AllowRelative = true;
			this.lotag.ButtonStep = 1;
			this.lotag.Location = new System.Drawing.Point(98, 53);
			this.lotag.Name = "lotag";
			this.lotag.Size = new System.Drawing.Size(88, 24);
			this.lotag.StepValues = null;
			this.lotag.TabIndex = 1;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(50, 58);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(41, 13);
			label4.TabIndex = 26;
			label4.Text = "LoTag:";
			// 
			// hitag
			// 
			this.hitag.AllowDecimal = false;
			this.hitag.AllowNegative = false;
			this.hitag.AllowRelative = true;
			this.hitag.ButtonStep = 1;
			this.hitag.Location = new System.Drawing.Point(98, 23);
			this.hitag.Name = "hitag";
			this.hitag.Size = new System.Drawing.Size(88, 24);
			this.hitag.StepValues = null;
			this.hitag.TabIndex = 0;
			// 
			// taglabel
			// 
			taglabel.AutoSize = true;
			taglabel.Location = new System.Drawing.Point(52, 28);
			taglabel.Name = "taglabel";
			taglabel.Size = new System.Drawing.Size(39, 13);
			taglabel.TabIndex = 9;
			taglabel.Text = "HiTag:";
			// 
			// groupeffect
			// 
			groupeffect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			groupeffect.Controls.Add(this.ceilslope);
			groupeffect.Controls.Add(label12);
			groupeffect.Controls.Add(this.ceilpalette);
			groupeffect.Controls.Add(label11);
			groupeffect.Controls.Add(this.ceilshade);
			groupeffect.Controls.Add(label10);
			groupeffect.Controls.Add(this.ceiloffsety);
			groupeffect.Controls.Add(label8);
			groupeffect.Controls.Add(this.ceiltex);
			groupeffect.Controls.Add(this.ceiloffsetx);
			groupeffect.Controls.Add(label9);
			groupeffect.Location = new System.Drawing.Point(7, 110);
			groupeffect.Name = "groupeffect";
			groupeffect.Size = new System.Drawing.Size(308, 183);
			groupeffect.TabIndex = 1;
			groupeffect.TabStop = false;
			groupeffect.Text = " Properties ";
			// 
			// ceilslope
			// 
			this.ceilslope.AllowDecimal = true;
			this.ceilslope.AllowNegative = true;
			this.ceilslope.AllowRelative = true;
			this.ceilslope.ButtonStep = 5;
			this.ceilslope.Location = new System.Drawing.Point(94, 147);
			this.ceilslope.Name = "ceilslope";
			this.ceilslope.Size = new System.Drawing.Size(73, 24);
			this.ceilslope.StepValues = null;
			this.ceilslope.TabIndex = 4;
			// 
			// label12
			// 
			label12.AutoSize = true;
			label12.Location = new System.Drawing.Point(22, 152);
			label12.Name = "label12";
			label12.Size = new System.Drawing.Size(66, 13);
			label12.TabIndex = 32;
			label12.Text = "Slope angle:";
			// 
			// ceilpalette
			// 
			this.ceilpalette.AllowDecimal = false;
			this.ceilpalette.AllowNegative = false;
			this.ceilpalette.AllowRelative = true;
			this.ceilpalette.ButtonStep = 8;
			this.ceilpalette.Location = new System.Drawing.Point(94, 117);
			this.ceilpalette.Name = "ceilpalette";
			this.ceilpalette.Size = new System.Drawing.Size(73, 24);
			this.ceilpalette.StepValues = null;
			this.ceilpalette.TabIndex = 3;
			// 
			// label11
			// 
			label11.AutoSize = true;
			label11.Location = new System.Drawing.Point(17, 122);
			label11.Name = "label11";
			label11.Size = new System.Drawing.Size(71, 13);
			label11.TabIndex = 30;
			label11.Text = "Palette index:";
			// 
			// ceilshade
			// 
			this.ceilshade.AllowDecimal = false;
			this.ceilshade.AllowNegative = false;
			this.ceilshade.AllowRelative = true;
			this.ceilshade.ButtonStep = 8;
			this.ceilshade.Location = new System.Drawing.Point(94, 87);
			this.ceilshade.Name = "ceilshade";
			this.ceilshade.Size = new System.Drawing.Size(73, 24);
			this.ceilshade.StepValues = null;
			this.ceilshade.TabIndex = 2;
			// 
			// label10
			// 
			label10.AutoSize = true;
			label10.Location = new System.Drawing.Point(47, 92);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(41, 13);
			label10.TabIndex = 28;
			label10.Text = "Shade:";
			// 
			// ceiloffsety
			// 
			this.ceiloffsety.AllowDecimal = false;
			this.ceiloffsety.AllowNegative = false;
			this.ceiloffsety.AllowRelative = true;
			this.ceiloffsety.ButtonStep = 8;
			this.ceiloffsety.Location = new System.Drawing.Point(94, 57);
			this.ceiloffsety.Name = "ceiloffsety";
			this.ceiloffsety.Size = new System.Drawing.Size(73, 24);
			this.ceiloffsety.StepValues = null;
			this.ceiloffsety.TabIndex = 1;
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(40, 62);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(48, 13);
			label8.TabIndex = 26;
			label8.Text = "Offset Y:";
			// 
			// ceiltex
			// 
			this.ceiltex.Location = new System.Drawing.Point(173, 19);
			this.ceiltex.Name = "ceiltex";
			this.ceiltex.Size = new System.Drawing.Size(128, 152);
			this.ceiltex.TabIndex = 5;
			this.ceiltex.TextureName = "";
			// 
			// ceiloffsetx
			// 
			this.ceiloffsetx.AllowDecimal = false;
			this.ceiloffsetx.AllowNegative = false;
			this.ceiloffsetx.AllowRelative = true;
			this.ceiloffsetx.ButtonStep = 8;
			this.ceiloffsetx.Location = new System.Drawing.Point(94, 27);
			this.ceiloffsetx.Name = "ceiloffsetx";
			this.ceiloffsetx.Size = new System.Drawing.Size(73, 24);
			this.ceiloffsetx.StepValues = null;
			this.ceiloffsetx.TabIndex = 0;
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(40, 32);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(48, 13);
			label9.TabIndex = 2;
			label9.Text = "Offset X:";
			// 
			// groupfloorceiling
			// 
			groupfloorceiling.Controls.Add(this.visibility);
			groupfloorceiling.Controls.Add(label2);
			groupfloorceiling.Controls.Add(this.floorheight);
			groupfloorceiling.Controls.Add(this.ceilheight);
			groupfloorceiling.Controls.Add(this.sectorheight);
			groupfloorceiling.Controls.Add(this.sectorheightlabel);
			groupfloorceiling.Controls.Add(label5);
			groupfloorceiling.Controls.Add(label6);
			groupfloorceiling.Location = new System.Drawing.Point(12, 12);
			groupfloorceiling.Name = "groupfloorceiling";
			groupfloorceiling.Size = new System.Drawing.Size(194, 159);
			groupfloorceiling.TabIndex = 0;
			groupfloorceiling.TabStop = false;
			groupfloorceiling.Text = "Floor and Ceiling ";
			// 
			// visibility
			// 
			this.visibility.AllowDecimal = false;
			this.visibility.AllowNegative = false;
			this.visibility.AllowRelative = true;
			this.visibility.ButtonStep = 8;
			this.visibility.Location = new System.Drawing.Point(98, 126);
			this.visibility.Name = "visibility";
			this.visibility.Size = new System.Drawing.Size(88, 24);
			this.visibility.StepValues = null;
			this.visibility.TabIndex = 2;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(45, 131);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(46, 13);
			label2.TabIndex = 24;
			label2.Text = "Visibility:";
			// 
			// floorheight
			// 
			this.floorheight.AllowDecimal = false;
			this.floorheight.AllowNegative = true;
			this.floorheight.AllowRelative = true;
			this.floorheight.ButtonStep = 8;
			this.floorheight.Location = new System.Drawing.Point(98, 60);
			this.floorheight.Name = "floorheight";
			this.floorheight.Size = new System.Drawing.Size(88, 24);
			this.floorheight.StepValues = null;
			this.floorheight.TabIndex = 1;
			this.floorheight.WhenTextChanged += new System.EventHandler(this.floorheight_TextChanged);
			// 
			// ceilheight
			// 
			this.ceilheight.AllowDecimal = false;
			this.ceilheight.AllowNegative = true;
			this.ceilheight.AllowRelative = true;
			this.ceilheight.ButtonStep = 8;
			this.ceilheight.Location = new System.Drawing.Point(98, 27);
			this.ceilheight.Name = "ceilheight";
			this.ceilheight.Size = new System.Drawing.Size(88, 24);
			this.ceilheight.StepValues = null;
			this.ceilheight.TabIndex = 0;
			this.ceilheight.WhenTextChanged += new System.EventHandler(this.ceilingheight_TextChanged);
			// 
			// sectorheight
			// 
			this.sectorheight.AutoSize = true;
			this.sectorheight.Location = new System.Drawing.Point(97, 98);
			this.sectorheight.Name = "sectorheight";
			this.sectorheight.Size = new System.Drawing.Size(13, 13);
			this.sectorheight.TabIndex = 21;
			this.sectorheight.Text = "0";
			// 
			// sectorheightlabel
			// 
			this.sectorheightlabel.AutoSize = true;
			this.sectorheightlabel.Location = new System.Drawing.Point(18, 98);
			this.sectorheightlabel.Name = "sectorheightlabel";
			this.sectorheightlabel.Size = new System.Drawing.Size(73, 13);
			this.sectorheightlabel.TabIndex = 20;
			this.sectorheightlabel.Text = "Sector height:";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(26, 65);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(65, 13);
			label5.TabIndex = 17;
			label5.Text = "Floor height:";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(19, 32);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(73, 13);
			label6.TabIndex = 19;
			label6.Text = "Ceiling height:";
			// 
			// groupBox3
			// 
			groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			groupBox3.Controls.Add(this.floorslope);
			groupBox3.Controls.Add(label13);
			groupBox3.Controls.Add(this.floorpalette);
			groupBox3.Controls.Add(label14);
			groupBox3.Controls.Add(this.floorshade);
			groupBox3.Controls.Add(label15);
			groupBox3.Controls.Add(this.flooroffsety);
			groupBox3.Controls.Add(label16);
			groupBox3.Controls.Add(this.floortex);
			groupBox3.Controls.Add(this.flooroffsetx);
			groupBox3.Controls.Add(label17);
			groupBox3.Location = new System.Drawing.Point(7, 110);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(308, 183);
			groupBox3.TabIndex = 5;
			groupBox3.TabStop = false;
			groupBox3.Text = " Properties ";
			// 
			// floorslope
			// 
			this.floorslope.AllowDecimal = true;
			this.floorslope.AllowNegative = true;
			this.floorslope.AllowRelative = true;
			this.floorslope.ButtonStep = 5;
			this.floorslope.Location = new System.Drawing.Point(94, 147);
			this.floorslope.Name = "floorslope";
			this.floorslope.Size = new System.Drawing.Size(73, 24);
			this.floorslope.StepValues = null;
			this.floorslope.TabIndex = 4;
			// 
			// label13
			// 
			label13.AutoSize = true;
			label13.Location = new System.Drawing.Point(22, 152);
			label13.Name = "label13";
			label13.Size = new System.Drawing.Size(66, 13);
			label13.TabIndex = 32;
			label13.Text = "Slope angle:";
			// 
			// floorpalette
			// 
			this.floorpalette.AllowDecimal = false;
			this.floorpalette.AllowNegative = false;
			this.floorpalette.AllowRelative = true;
			this.floorpalette.ButtonStep = 8;
			this.floorpalette.Location = new System.Drawing.Point(94, 117);
			this.floorpalette.Name = "floorpalette";
			this.floorpalette.Size = new System.Drawing.Size(73, 24);
			this.floorpalette.StepValues = null;
			this.floorpalette.TabIndex = 3;
			// 
			// label14
			// 
			label14.AutoSize = true;
			label14.Location = new System.Drawing.Point(17, 122);
			label14.Name = "label14";
			label14.Size = new System.Drawing.Size(71, 13);
			label14.TabIndex = 30;
			label14.Text = "Palette index:";
			// 
			// floorshade
			// 
			this.floorshade.AllowDecimal = false;
			this.floorshade.AllowNegative = false;
			this.floorshade.AllowRelative = true;
			this.floorshade.ButtonStep = 8;
			this.floorshade.Location = new System.Drawing.Point(94, 87);
			this.floorshade.Name = "floorshade";
			this.floorshade.Size = new System.Drawing.Size(73, 24);
			this.floorshade.StepValues = null;
			this.floorshade.TabIndex = 2;
			// 
			// label15
			// 
			label15.AutoSize = true;
			label15.Location = new System.Drawing.Point(47, 92);
			label15.Name = "label15";
			label15.Size = new System.Drawing.Size(41, 13);
			label15.TabIndex = 28;
			label15.Text = "Shade:";
			// 
			// flooroffsety
			// 
			this.flooroffsety.AllowDecimal = false;
			this.flooroffsety.AllowNegative = false;
			this.flooroffsety.AllowRelative = true;
			this.flooroffsety.ButtonStep = 8;
			this.flooroffsety.Location = new System.Drawing.Point(94, 57);
			this.flooroffsety.Name = "flooroffsety";
			this.flooroffsety.Size = new System.Drawing.Size(73, 24);
			this.flooroffsety.StepValues = null;
			this.flooroffsety.TabIndex = 1;
			// 
			// label16
			// 
			label16.AutoSize = true;
			label16.Location = new System.Drawing.Point(40, 62);
			label16.Name = "label16";
			label16.Size = new System.Drawing.Size(48, 13);
			label16.TabIndex = 26;
			label16.Text = "Offset Y:";
			// 
			// floortex
			// 
			this.floortex.Location = new System.Drawing.Point(173, 19);
			this.floortex.Name = "floortex";
			this.floortex.Size = new System.Drawing.Size(128, 152);
			this.floortex.TabIndex = 5;
			this.floortex.TextureName = "";
			// 
			// flooroffsetx
			// 
			this.flooroffsetx.AllowDecimal = false;
			this.flooroffsetx.AllowNegative = false;
			this.flooroffsetx.AllowRelative = true;
			this.flooroffsetx.ButtonStep = 8;
			this.flooroffsetx.Location = new System.Drawing.Point(94, 27);
			this.flooroffsetx.Name = "flooroffsetx";
			this.flooroffsetx.Size = new System.Drawing.Size(73, 24);
			this.flooroffsetx.StepValues = null;
			this.flooroffsetx.TabIndex = 0;
			// 
			// label17
			// 
			label17.AutoSize = true;
			label17.Location = new System.Drawing.Point(40, 32);
			label17.Name = "label17";
			label17.Size = new System.Drawing.Size(48, 13);
			label17.TabIndex = 2;
			label17.Text = "Offset X:";
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.cancel.Location = new System.Drawing.Point(423, 348);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(112, 25);
			this.cancel.TabIndex = 2;
			this.cancel.Text = "Cancel";
			this.cancel.UseVisualStyleBackColor = true;
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// apply
			// 
			this.apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.apply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.apply.Location = new System.Drawing.Point(304, 348);
			this.apply.Name = "apply";
			this.apply.Size = new System.Drawing.Size(112, 25);
			this.apply.TabIndex = 1;
			this.apply.Text = "OK";
			this.apply.UseVisualStyleBackColor = true;
			this.apply.Click += new System.EventHandler(this.apply_Click);
			// 
			// tabs
			// 
			this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabs.Controls.Add(this.tabfloor);
			this.tabs.Controls.Add(this.tabceiling);
			this.tabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabs.Location = new System.Drawing.Point(210, 12);
			this.tabs.Margin = new System.Windows.Forms.Padding(1);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(329, 325);
			this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabs.TabIndex = 2;
			// 
			// tabfloor
			// 
			this.tabfloor.Controls.Add(this.groupBox2);
			this.tabfloor.Controls.Add(groupBox3);
			this.tabfloor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabfloor.Location = new System.Drawing.Point(4, 22);
			this.tabfloor.Name = "tabfloor";
			this.tabfloor.Padding = new System.Windows.Forms.Padding(3);
			this.tabfloor.Size = new System.Drawing.Size(321, 299);
			this.tabfloor.TabIndex = 1;
			this.tabfloor.Text = "Floor";
			this.tabfloor.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.floorflags);
			this.groupBox2.Location = new System.Drawing.Point(7, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(308, 98);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = " Flags ";
			// 
			// floorflags
			// 
			this.floorflags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.floorflags.AutoScroll = true;
			this.floorflags.Columns = 2;
			this.floorflags.Location = new System.Drawing.Point(14, 19);
			this.floorflags.Name = "floorflags";
			this.floorflags.Size = new System.Drawing.Size(287, 73);
			this.floorflags.TabIndex = 0;
			// 
			// tabceiling
			// 
			this.tabceiling.Controls.Add(this.groupBox1);
			this.tabceiling.Controls.Add(groupeffect);
			this.tabceiling.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabceiling.Location = new System.Drawing.Point(4, 22);
			this.tabceiling.Name = "tabceiling";
			this.tabceiling.Padding = new System.Windows.Forms.Padding(3);
			this.tabceiling.Size = new System.Drawing.Size(321, 299);
			this.tabceiling.TabIndex = 0;
			this.tabceiling.Text = "Ceiling";
			this.tabceiling.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.ceilflags);
			this.groupBox1.Location = new System.Drawing.Point(7, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(308, 98);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = " Flags ";
			// 
			// ceilflags
			// 
			this.ceilflags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ceilflags.AutoScroll = true;
			this.ceilflags.Columns = 2;
			this.ceilflags.Location = new System.Drawing.Point(14, 19);
			this.ceilflags.Name = "ceilflags";
			this.ceilflags.Size = new System.Drawing.Size(287, 73);
			this.ceilflags.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panel1.Controls.Add(this.tabs);
			this.panel1.Controls.Add(groupaction);
			this.panel1.Controls.Add(groupfloorceiling);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(545, 340);
			this.panel1.TabIndex = 3;
			// 
			// EditSectorForm
			// 
			this.AcceptButton = this.apply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(545, 379);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.apply);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditSectorForm";
			this.Opacity = 0;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Edit sector";
			this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.SectorEditForm_HelpRequested);
			groupaction.ResumeLayout(false);
			groupaction.PerformLayout();
			groupeffect.ResumeLayout(false);
			groupeffect.PerformLayout();
			groupfloorceiling.ResumeLayout(false);
			groupfloorceiling.PerformLayout();
			groupBox3.ResumeLayout(false);
			groupBox3.PerformLayout();
			this.tabs.ResumeLayout(false);
			this.tabfloor.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.tabceiling.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button apply;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tabceiling;
		private System.Windows.Forms.TabPage tabfloor;
		private System.Windows.Forms.Label sectorheight;
		private System.Windows.Forms.Label sectorheightlabel;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox ceilheight;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox floorheight;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox ceiloffsetx;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox hitag;
		private System.Windows.Forms.Panel panel1;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox extra;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox lotag;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox visibility;
		private mxd.DukeBuilder.Controls.ImageSelectorControl ceiltex;
		private System.Windows.Forms.GroupBox groupBox1;
		private mxd.DukeBuilder.Controls.CheckboxArrayControl ceilflags;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox ceilshade;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox ceiloffsety;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox ceilslope;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox ceilpalette;
		private System.Windows.Forms.GroupBox groupBox2;
		private mxd.DukeBuilder.Controls.CheckboxArrayControl floorflags;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox floorslope;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox floorpalette;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox floorshade;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox flooroffsety;
		private mxd.DukeBuilder.Controls.ImageSelectorControl floortex;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox flooroffsetx;
	}
}