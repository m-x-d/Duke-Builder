namespace mxd.DukeBuilder.Windows
{
	partial class EditSpriteForm
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
			System.Windows.Forms.GroupBox groupBox2;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label8;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.GroupBox groupaction;
			System.Windows.Forms.Label label7;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label taglabel;
			System.Windows.Forms.GroupBox groupBox3;
			System.Windows.Forms.Label label16;
			System.Windows.Forms.Label label13;
			System.Windows.Forms.Label label14;
			System.Windows.Forms.Label label15;
			System.Windows.Forms.Label label10;
			System.Windows.Forms.Label label12;
			System.Windows.Forms.Label label9;
			System.Windows.Forms.Label label11;
			this.spritetype = new mxd.DukeBuilder.Controls.SpriteBrowserControl();
			this.tex = new mxd.DukeBuilder.Controls.ImageSelectorControl();
			this.velz = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.vely = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.velx = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.posz = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.posy = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.posx = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.extra = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.lotag = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.hitag = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.owner = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.clipdistance = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.palette = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.shade = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.repeaty = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.repeatx = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.offsety = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.offsetx = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.angle = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.anglecontrol = new mxd.DukeBuilder.Controls.AngleControl();
			this.settingsgroup = new System.Windows.Forms.GroupBox();
			this.flags = new mxd.DukeBuilder.Controls.CheckboxArrayControl();
			this.cancel = new System.Windows.Forms.Button();
			this.apply = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			groupBox1 = new System.Windows.Forms.GroupBox();
			groupBox2 = new System.Windows.Forms.GroupBox();
			label6 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label8 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			groupaction = new System.Windows.Forms.GroupBox();
			label7 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			taglabel = new System.Windows.Forms.Label();
			groupBox3 = new System.Windows.Forms.GroupBox();
			label16 = new System.Windows.Forms.Label();
			label13 = new System.Windows.Forms.Label();
			label14 = new System.Windows.Forms.Label();
			label15 = new System.Windows.Forms.Label();
			label10 = new System.Windows.Forms.Label();
			label12 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			label11 = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			groupaction.SuspendLayout();
			groupBox3.SuspendLayout();
			this.settingsgroup.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(this.spritetype);
			groupBox1.Controls.Add(this.tex);
			groupBox1.Location = new System.Drawing.Point(12, 12);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(240, 390);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = " Sprite ";
			// 
			// spritetype
			// 
			this.spritetype.Location = new System.Drawing.Point(9, 19);
			this.spritetype.Name = "spritetype";
			this.spritetype.Size = new System.Drawing.Size(222, 210);
			this.spritetype.TabIndex = 0;
			this.spritetype.OnTypeDoubleClicked += new mxd.DukeBuilder.Controls.SpriteBrowserControl.TypeDoubleClickDeletegate(this.spritetype_OnTypeDoubleClicked);
			this.spritetype.OnTypeChanged += new mxd.DukeBuilder.Controls.SpriteBrowserControl.TypeChangedDeletegate(this.spritetype_OnTypeChanged);
			// 
			// tex
			// 
			this.tex.Location = new System.Drawing.Point(9, 232);
			this.tex.Name = "tex";
			this.tex.Size = new System.Drawing.Size(222, 152);
			this.tex.TabIndex = 3;
			this.tex.TextureName = "";
			this.tex.OnValueChanged += new System.EventHandler(this.tex_OnValueChanged);
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(label6);
			groupBox2.Controls.Add(label3);
			groupBox2.Controls.Add(this.velz);
			groupBox2.Controls.Add(this.vely);
			groupBox2.Controls.Add(this.velx);
			groupBox2.Controls.Add(label8);
			groupBox2.Controls.Add(this.posz);
			groupBox2.Controls.Add(label2);
			groupBox2.Controls.Add(this.posy);
			groupBox2.Controls.Add(label1);
			groupBox2.Controls.Add(this.posx);
			groupBox2.Controls.Add(label5);
			groupBox2.Location = new System.Drawing.Point(260, 289);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(260, 113);
			groupBox2.TabIndex = 2;
			groupBox2.TabStop = false;
			groupBox2.Text = " Position ";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(118, 84);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(57, 13);
			label6.TabIndex = 23;
			label6.Text = "Velocity Z:";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(118, 54);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(57, 13);
			label3.TabIndex = 22;
			label3.Text = "Velocity Y:";
			// 
			// velz
			// 
			this.velz.AllowDecimal = false;
			this.velz.AllowNegative = true;
			this.velz.AllowRelative = true;
			this.velz.ButtonStep = 8;
			this.velz.Location = new System.Drawing.Point(181, 79);
			this.velz.Name = "velz";
			this.velz.Size = new System.Drawing.Size(72, 24);
			this.velz.StepValues = null;
			this.velz.TabIndex = 21;
			// 
			// vely
			// 
			this.vely.AllowDecimal = false;
			this.vely.AllowNegative = true;
			this.vely.AllowRelative = true;
			this.vely.ButtonStep = 8;
			this.vely.Location = new System.Drawing.Point(181, 49);
			this.vely.Name = "vely";
			this.vely.Size = new System.Drawing.Size(72, 24);
			this.vely.StepValues = null;
			this.vely.TabIndex = 19;
			// 
			// velx
			// 
			this.velx.AllowDecimal = false;
			this.velx.AllowNegative = true;
			this.velx.AllowRelative = true;
			this.velx.ButtonStep = 8;
			this.velx.Location = new System.Drawing.Point(181, 19);
			this.velx.Name = "velx";
			this.velx.Size = new System.Drawing.Size(72, 24);
			this.velx.StepValues = null;
			this.velx.TabIndex = 17;
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(118, 24);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(57, 13);
			label8.TabIndex = 16;
			label8.Text = "Velocity X:";
			// 
			// posz
			// 
			this.posz.AllowDecimal = false;
			this.posz.AllowNegative = true;
			this.posz.AllowRelative = true;
			this.posz.ButtonStep = 8;
			this.posz.Location = new System.Drawing.Point(32, 79);
			this.posz.Name = "posz";
			this.posz.Size = new System.Drawing.Size(72, 24);
			this.posz.StepValues = null;
			this.posz.TabIndex = 15;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(9, 84);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(17, 13);
			label2.TabIndex = 14;
			label2.Text = "Z:";
			// 
			// posy
			// 
			this.posy.AllowDecimal = false;
			this.posy.AllowNegative = true;
			this.posy.AllowRelative = true;
			this.posy.ButtonStep = 8;
			this.posy.Location = new System.Drawing.Point(32, 49);
			this.posy.Name = "posy";
			this.posy.Size = new System.Drawing.Size(72, 24);
			this.posy.StepValues = null;
			this.posy.TabIndex = 13;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(9, 54);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(17, 13);
			label1.TabIndex = 12;
			label1.Text = "Y:";
			// 
			// posx
			// 
			this.posx.AllowDecimal = false;
			this.posx.AllowNegative = true;
			this.posx.AllowRelative = true;
			this.posx.ButtonStep = 8;
			this.posx.Location = new System.Drawing.Point(32, 19);
			this.posx.Name = "posx";
			this.posx.Size = new System.Drawing.Size(72, 24);
			this.posx.StepValues = null;
			this.posx.TabIndex = 11;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(9, 24);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(17, 13);
			label5.TabIndex = 8;
			label5.Text = "X:";
			// 
			// groupaction
			// 
			groupaction.Controls.Add(this.extra);
			groupaction.Controls.Add(label7);
			groupaction.Controls.Add(this.lotag);
			groupaction.Controls.Add(label4);
			groupaction.Controls.Add(this.hitag);
			groupaction.Controls.Add(taglabel);
			groupaction.Location = new System.Drawing.Point(560, 137);
			groupaction.Name = "groupaction";
			groupaction.Size = new System.Drawing.Size(152, 146);
			groupaction.TabIndex = 3;
			groupaction.TabStop = false;
			groupaction.Text = " Identification ";
			// 
			// extra
			// 
			this.extra.AllowDecimal = false;
			this.extra.AllowNegative = true;
			this.extra.AllowRelative = true;
			this.extra.ButtonStep = 1;
			this.extra.Location = new System.Drawing.Point(57, 84);
			this.extra.Name = "extra";
			this.extra.Size = new System.Drawing.Size(88, 24);
			this.extra.StepValues = null;
			this.extra.TabIndex = 2;
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(16, 89);
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
			this.lotag.Location = new System.Drawing.Point(57, 54);
			this.lotag.Name = "lotag";
			this.lotag.Size = new System.Drawing.Size(88, 24);
			this.lotag.StepValues = null;
			this.lotag.TabIndex = 1;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(9, 59);
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
			this.hitag.Location = new System.Drawing.Point(57, 24);
			this.hitag.Name = "hitag";
			this.hitag.Size = new System.Drawing.Size(88, 24);
			this.hitag.StepValues = null;
			this.hitag.TabIndex = 0;
			// 
			// taglabel
			// 
			taglabel.AutoSize = true;
			taglabel.Location = new System.Drawing.Point(11, 29);
			taglabel.Name = "taglabel";
			taglabel.Size = new System.Drawing.Size(39, 13);
			taglabel.TabIndex = 9;
			taglabel.Text = "HiTag:";
			// 
			// groupBox3
			// 
			groupBox3.Controls.Add(this.owner);
			groupBox3.Controls.Add(label16);
			groupBox3.Controls.Add(this.clipdistance);
			groupBox3.Controls.Add(label13);
			groupBox3.Controls.Add(this.palette);
			groupBox3.Controls.Add(label14);
			groupBox3.Controls.Add(this.shade);
			groupBox3.Controls.Add(label15);
			groupBox3.Controls.Add(this.repeaty);
			groupBox3.Controls.Add(label10);
			groupBox3.Controls.Add(this.repeatx);
			groupBox3.Controls.Add(label12);
			groupBox3.Controls.Add(this.offsety);
			groupBox3.Controls.Add(label9);
			groupBox3.Controls.Add(this.offsetx);
			groupBox3.Controls.Add(label11);
			groupBox3.Location = new System.Drawing.Point(260, 137);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(294, 146);
			groupBox3.TabIndex = 29;
			groupBox3.TabStop = false;
			groupBox3.Text = " Properties ";
			// 
			// owner
			// 
			this.owner.AllowDecimal = false;
			this.owner.AllowNegative = true;
			this.owner.AllowRelative = true;
			this.owner.ButtonStep = 1;
			this.owner.Location = new System.Drawing.Point(226, 114);
			this.owner.Name = "owner";
			this.owner.Size = new System.Drawing.Size(60, 24);
			this.owner.StepValues = null;
			this.owner.TabIndex = 22;
			// 
			// label16
			// 
			label16.AutoSize = true;
			label16.Location = new System.Drawing.Point(178, 119);
			label16.Name = "label16";
			label16.Size = new System.Drawing.Size(41, 13);
			label16.TabIndex = 23;
			label16.Text = "Owner:";
			// 
			// clipdistance
			// 
			this.clipdistance.AllowDecimal = false;
			this.clipdistance.AllowNegative = false;
			this.clipdistance.AllowRelative = true;
			this.clipdistance.ButtonStep = 1;
			this.clipdistance.Location = new System.Drawing.Point(226, 84);
			this.clipdistance.Name = "clipdistance";
			this.clipdistance.Size = new System.Drawing.Size(60, 24);
			this.clipdistance.StepValues = null;
			this.clipdistance.TabIndex = 20;
			// 
			// label13
			// 
			label13.AutoSize = true;
			label13.Location = new System.Drawing.Point(150, 89);
			label13.Name = "label13";
			label13.Size = new System.Drawing.Size(70, 13);
			label13.TabIndex = 21;
			label13.Text = "Clip distance:";
			// 
			// palette
			// 
			this.palette.AllowDecimal = false;
			this.palette.AllowNegative = false;
			this.palette.AllowRelative = true;
			this.palette.ButtonStep = 1;
			this.palette.Location = new System.Drawing.Point(226, 54);
			this.palette.Name = "palette";
			this.palette.Size = new System.Drawing.Size(60, 24);
			this.palette.StepValues = null;
			this.palette.TabIndex = 18;
			// 
			// label14
			// 
			label14.AutoSize = true;
			label14.Location = new System.Drawing.Point(149, 59);
			label14.Name = "label14";
			label14.Size = new System.Drawing.Size(71, 13);
			label14.TabIndex = 19;
			label14.Text = "Palette index:";
			// 
			// shade
			// 
			this.shade.AllowDecimal = false;
			this.shade.AllowNegative = false;
			this.shade.AllowRelative = true;
			this.shade.ButtonStep = 1;
			this.shade.Location = new System.Drawing.Point(226, 24);
			this.shade.Name = "shade";
			this.shade.Size = new System.Drawing.Size(60, 24);
			this.shade.StepValues = null;
			this.shade.TabIndex = 16;
			// 
			// label15
			// 
			label15.AutoSize = true;
			label15.Location = new System.Drawing.Point(179, 29);
			label15.Name = "label15";
			label15.Size = new System.Drawing.Size(41, 13);
			label15.TabIndex = 17;
			label15.Text = "Shade:";
			// 
			// repeaty
			// 
			this.repeaty.AllowDecimal = false;
			this.repeaty.AllowNegative = false;
			this.repeaty.AllowRelative = true;
			this.repeaty.ButtonStep = 1;
			this.repeaty.Location = new System.Drawing.Point(74, 114);
			this.repeaty.Name = "repeaty";
			this.repeaty.Size = new System.Drawing.Size(60, 24);
			this.repeaty.StepValues = null;
			this.repeaty.TabIndex = 14;
			// 
			// label10
			// 
			label10.AutoSize = true;
			label10.Location = new System.Drawing.Point(13, 119);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(55, 13);
			label10.TabIndex = 15;
			label10.Text = "Repeat Y:";
			// 
			// repeatx
			// 
			this.repeatx.AllowDecimal = false;
			this.repeatx.AllowNegative = false;
			this.repeatx.AllowRelative = true;
			this.repeatx.ButtonStep = 1;
			this.repeatx.Location = new System.Drawing.Point(74, 84);
			this.repeatx.Name = "repeatx";
			this.repeatx.Size = new System.Drawing.Size(60, 24);
			this.repeatx.StepValues = null;
			this.repeatx.TabIndex = 12;
			// 
			// label12
			// 
			label12.AutoSize = true;
			label12.Location = new System.Drawing.Point(13, 89);
			label12.Name = "label12";
			label12.Size = new System.Drawing.Size(55, 13);
			label12.TabIndex = 13;
			label12.Text = "Repeat X:";
			// 
			// offsety
			// 
			this.offsety.AllowDecimal = false;
			this.offsety.AllowNegative = false;
			this.offsety.AllowRelative = true;
			this.offsety.ButtonStep = 1;
			this.offsety.Location = new System.Drawing.Point(74, 54);
			this.offsety.Name = "offsety";
			this.offsety.Size = new System.Drawing.Size(60, 24);
			this.offsety.StepValues = null;
			this.offsety.TabIndex = 10;
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(19, 59);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(48, 13);
			label9.TabIndex = 11;
			label9.Text = "Offset Y:";
			// 
			// offsetx
			// 
			this.offsetx.AllowDecimal = false;
			this.offsetx.AllowNegative = false;
			this.offsetx.AllowRelative = true;
			this.offsetx.ButtonStep = 1;
			this.offsetx.Location = new System.Drawing.Point(74, 24);
			this.offsetx.Name = "offsetx";
			this.offsetx.Size = new System.Drawing.Size(60, 24);
			this.offsetx.StepValues = null;
			this.offsetx.TabIndex = 0;
			// 
			// label11
			// 
			label11.AutoSize = true;
			label11.Location = new System.Drawing.Point(19, 29);
			label11.Name = "label11";
			label11.Size = new System.Drawing.Size(48, 13);
			label11.TabIndex = 9;
			label11.Text = "Offset X:";
			// 
			// angle
			// 
			this.angle.AllowDecimal = false;
			this.angle.AllowNegative = true;
			this.angle.AllowRelative = true;
			this.angle.ButtonStep = 45;
			this.angle.Location = new System.Drawing.Point(6, 19);
			this.angle.Name = "angle";
			this.angle.Size = new System.Drawing.Size(84, 24);
			this.angle.StepValues = null;
			this.angle.TabIndex = 10;
			this.angle.WhenTextChanged += new System.EventHandler(this.angle_TextChanged);
			// 
			// anglecontrol
			// 
			this.anglecontrol.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.anglecontrol.Location = new System.Drawing.Point(92, 14);
			this.anglecontrol.Name = "anglecontrol";
			this.anglecontrol.Size = new System.Drawing.Size(89, 89);
			this.anglecontrol.TabIndex = 2;
			this.anglecontrol.Value = 0;
			this.anglecontrol.ValueChanged += new System.EventHandler(this.anglecontrol_ValueChanged);
			// 
			// settingsgroup
			// 
			this.settingsgroup.Controls.Add(this.flags);
			this.settingsgroup.Location = new System.Drawing.Point(260, 12);
			this.settingsgroup.Name = "settingsgroup";
			this.settingsgroup.Size = new System.Drawing.Size(452, 119);
			this.settingsgroup.TabIndex = 1;
			this.settingsgroup.TabStop = false;
			this.settingsgroup.Text = " Flags ";
			// 
			// flags
			// 
			this.flags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flags.AutoScroll = true;
			this.flags.Columns = 3;
			this.flags.Location = new System.Drawing.Point(6, 19);
			this.flags.Name = "flags";
			this.flags.Size = new System.Drawing.Size(440, 93);
			this.flags.TabIndex = 0;
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.cancel.Location = new System.Drawing.Point(602, 414);
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
			this.apply.Location = new System.Drawing.Point(483, 414);
			this.apply.Name = "apply";
			this.apply.Size = new System.Drawing.Size(112, 25);
			this.apply.TabIndex = 1;
			this.apply.Text = "OK";
			this.apply.UseVisualStyleBackColor = true;
			this.apply.Click += new System.EventHandler(this.apply_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panel1.Controls.Add(this.groupBox4);
			this.panel1.Controls.Add(groupBox3);
			this.panel1.Controls.Add(groupaction);
			this.panel1.Controls.Add(this.settingsgroup);
			this.panel1.Controls.Add(groupBox2);
			this.panel1.Controls.Add(groupBox1);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(723, 407);
			this.panel1.TabIndex = 3;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.anglecontrol);
			this.groupBox4.Controls.Add(this.angle);
			this.groupBox4.Location = new System.Drawing.Point(526, 289);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(186, 113);
			this.groupBox4.TabIndex = 30;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = " Angle ";
			// 
			// EditSpriteForm
			// 
			this.AcceptButton = this.apply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(724, 445);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.apply);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditSpriteForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Edit sprite";
			this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.ThingEditForm_HelpRequested);
			groupBox1.ResumeLayout(false);
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			groupaction.ResumeLayout(false);
			groupaction.PerformLayout();
			groupBox3.ResumeLayout(false);
			groupBox3.PerformLayout();
			this.settingsgroup.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button apply;
		private System.Windows.Forms.GroupBox settingsgroup;
		private mxd.DukeBuilder.Controls.CheckboxArrayControl flags;
		private mxd.DukeBuilder.Controls.AngleControl anglecontrol;
		private mxd.DukeBuilder.Controls.SpriteBrowserControl spritetype;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox angle;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox posx;
		private mxd.DukeBuilder.Controls.ImageSelectorControl tex;
		private System.Windows.Forms.Panel panel1;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox velz;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox vely;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox velx;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox posz;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox posy;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox offsety;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox offsetx;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox extra;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox lotag;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox hitag;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox repeaty;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox repeatx;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox clipdistance;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox palette;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox shade;
		private System.Windows.Forms.GroupBox groupBox4;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox owner;
	}
}