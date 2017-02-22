namespace mxd.DukeBuilder.Controls
{
	partial class ImageBrowserControl
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
				CleanUp();
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
			this.labelMixMode = new System.Windows.Forms.Label();
			this.label = new System.Windows.Forms.Label();
			this.splitter = new System.Windows.Forms.SplitContainer();
			this.list = new mxd.DukeBuilder.Controls.ImageBrowserPanel();
			this.filtergroup = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.alphacombo = new System.Windows.Forms.ComboBox();
			this.objectname = new System.Windows.Forms.TextBox();
			this.filterheightlabel = new System.Windows.Forms.Label();
			this.objectclear = new System.Windows.Forms.Button();
			this.filterHeight = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.filterwidthlabel = new System.Windows.Forms.Label();
			this.filterWidth = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.po2combo = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.sizecombo = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.showimagesizes = new System.Windows.Forms.CheckBox();
			this.usedimagesfirst = new System.Windows.Forms.CheckBox();
			this.refreshtimer = new System.Windows.Forms.Timer(this.components);
			this.splitter.Panel1.SuspendLayout();
			this.splitter.Panel2.SuspendLayout();
			this.splitter.SuspendLayout();
			this.filtergroup.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelMixMode
			// 
			this.labelMixMode.AutoSize = true;
			this.labelMixMode.Location = new System.Drawing.Point(210, 19);
			this.labelMixMode.Name = "labelMixMode";
			this.labelMixMode.Size = new System.Drawing.Size(30, 13);
			this.labelMixMode.TabIndex = 0;
			this.labelMixMode.Text = "Size:";
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Location = new System.Drawing.Point(21, 19);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(38, 13);
			this.label.TabIndex = 0;
			this.label.Text = "Name:";
			// 
			// splitter
			// 
			this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitter.IsSplitterFixed = true;
			this.splitter.Location = new System.Drawing.Point(0, 0);
			this.splitter.Name = "splitter";
			this.splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitter.Panel1
			// 
			this.splitter.Panel1.Controls.Add(this.list);
			// 
			// splitter.Panel2
			// 
			this.splitter.Panel2.Controls.Add(this.filtergroup);
			this.splitter.Panel2.Controls.Add(this.groupBox1);
			this.splitter.Size = new System.Drawing.Size(900, 346);
			this.splitter.SplitterDistance = 270;
			this.splitter.TabIndex = 0;
			this.splitter.TabStop = false;
			// 
			// list
			// 
			this.list.AutoScroll = true;
			this.list.BackColor = System.Drawing.Color.Black;
			this.list.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.list.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list.HideSelection = false;
			this.list.ImageSize = 128;
			this.list.Location = new System.Drawing.Point(0, 0);
			this.list.MultiSelect = false;
			this.list.Name = "list";
			this.list.ShowImageSizes = false;
			this.list.Size = new System.Drawing.Size(900, 270);
			this.list.TabIndex = 1;
			this.list.ItemDoubleClicked += new mxd.DukeBuilder.Controls.ImageBrowserPanel.ItemSelectedEventHandler(this.list_ItemDoubleClicked);
			this.list.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.list_KeyPress);
			// 
			// filtergroup
			// 
			this.filtergroup.Controls.Add(this.label2);
			this.filtergroup.Controls.Add(this.alphacombo);
			this.filtergroup.Controls.Add(this.objectname);
			this.filtergroup.Controls.Add(this.label);
			this.filtergroup.Controls.Add(this.filterheightlabel);
			this.filtergroup.Controls.Add(this.objectclear);
			this.filtergroup.Controls.Add(this.filterHeight);
			this.filtergroup.Controls.Add(this.labelMixMode);
			this.filtergroup.Controls.Add(this.filterwidthlabel);
			this.filtergroup.Controls.Add(this.filterWidth);
			this.filtergroup.Controls.Add(this.po2combo);
			this.filtergroup.Location = new System.Drawing.Point(235, 3);
			this.filtergroup.Name = "filtergroup";
			this.filtergroup.Size = new System.Drawing.Size(411, 67);
			this.filtergroup.TabIndex = 5;
			this.filtergroup.TabStop = false;
			this.filtergroup.Text = " Filtering ";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(46, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Opacity:";
			// 
			// alphacombo
			// 
			this.alphacombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.alphacombo.FormattingEnabled = true;
			this.alphacombo.Items.AddRange(new object[] {
            "Any",
            "Only with transparency",
            "Only fully opaque"});
			this.alphacombo.Location = new System.Drawing.Point(65, 40);
			this.alphacombo.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
			this.alphacombo.Name = "alphacombo";
			this.alphacombo.Size = new System.Drawing.Size(135, 21);
			this.alphacombo.TabIndex = 4;
			this.alphacombo.TabStop = false;
			this.alphacombo.SelectedIndexChanged += new System.EventHandler(this.alphacombo_SelectedIndexChanged);
			// 
			// objectname
			// 
			this.objectname.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.objectname.HideSelection = false;
			this.objectname.Location = new System.Drawing.Point(65, 16);
			this.objectname.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
			this.objectname.Name = "objectname";
			this.objectname.Size = new System.Drawing.Size(107, 20);
			this.objectname.TabIndex = 0;
			this.objectname.TabStop = false;
			this.objectname.TextChanged += new System.EventHandler(this.objectname_TextChanged);
			this.objectname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.objectname_KeyDown);
			// 
			// filterheightlabel
			// 
			this.filterheightlabel.AutoSize = true;
			this.filterheightlabel.Location = new System.Drawing.Point(305, 44);
			this.filterheightlabel.Name = "filterheightlabel";
			this.filterheightlabel.Size = new System.Drawing.Size(41, 13);
			this.filterheightlabel.TabIndex = 0;
			this.filterheightlabel.Text = "Height:";
			// 
			// objectclear
			// 
			this.objectclear.Image = global::mxd.DukeBuilder.Properties.Resources.SearchClear;
			this.objectclear.Location = new System.Drawing.Point(174, 15);
			this.objectclear.Name = "objectclear";
			this.objectclear.Size = new System.Drawing.Size(26, 23);
			this.objectclear.TabIndex = 3;
			this.objectclear.TabStop = false;
			this.objectclear.UseVisualStyleBackColor = true;
			this.objectclear.Click += new System.EventHandler(this.objectclear_Click);
			// 
			// filterHeight
			// 
			this.filterHeight.AllowDecimal = false;
			this.filterHeight.AllowNegative = false;
			this.filterHeight.AllowRelative = false;
			this.filterHeight.ButtonStep = 1;
			this.filterHeight.Location = new System.Drawing.Point(348, 39);
			this.filterHeight.Name = "filterHeight";
			this.filterHeight.Size = new System.Drawing.Size(54, 24);
			this.filterHeight.StepValues = null;
			this.filterHeight.TabIndex = 0;
			this.filterHeight.TabStop = false;
			this.filterHeight.WhenTextChanged += new System.EventHandler(this.filtersize_WhenTextChanged);
			// 
			// filterwidthlabel
			// 
			this.filterwidthlabel.AutoSize = true;
			this.filterwidthlabel.Location = new System.Drawing.Point(206, 44);
			this.filterwidthlabel.Name = "filterwidthlabel";
			this.filterwidthlabel.Size = new System.Drawing.Size(38, 13);
			this.filterwidthlabel.TabIndex = 0;
			this.filterwidthlabel.Text = "Width:";
			// 
			// filterWidth
			// 
			this.filterWidth.AllowDecimal = false;
			this.filterWidth.AllowNegative = false;
			this.filterWidth.AllowRelative = false;
			this.filterWidth.ButtonStep = 1;
			this.filterWidth.Location = new System.Drawing.Point(246, 39);
			this.filterWidth.Name = "filterWidth";
			this.filterWidth.Size = new System.Drawing.Size(54, 24);
			this.filterWidth.StepValues = null;
			this.filterWidth.TabIndex = 0;
			this.filterWidth.TabStop = false;
			this.filterWidth.WhenTextChanged += new System.EventHandler(this.filtersize_WhenTextChanged);
			// 
			// po2combo
			// 
			this.po2combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.po2combo.FormattingEnabled = true;
			this.po2combo.Items.AddRange(new object[] {
            "Any",
            "Only power of 2",
            "Only non-power of 2"});
			this.po2combo.Location = new System.Drawing.Point(246, 16);
			this.po2combo.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
			this.po2combo.Name = "po2combo";
			this.po2combo.Size = new System.Drawing.Size(156, 21);
			this.po2combo.TabIndex = 0;
			this.po2combo.TabStop = false;
			this.po2combo.SelectedIndexChanged += new System.EventHandler(this.po2combo_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.sizecombo);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.showimagesizes);
			this.groupBox1.Controls.Add(this.usedimagesfirst);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(226, 67);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = " Display ";
			// 
			// sizecombo
			// 
			this.sizecombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sizecombo.FormattingEnabled = true;
			this.sizecombo.Items.AddRange(new object[] {
            "1:1",
            "2:1",
            "64",
            "96",
            "128",
            "192",
            "256"});
			this.sizecombo.Location = new System.Drawing.Point(6, 40);
			this.sizecombo.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
			this.sizecombo.Name = "sizecombo";
			this.sizecombo.Size = new System.Drawing.Size(65, 21);
			this.sizecombo.TabIndex = 2;
			this.sizecombo.TabStop = false;
			this.sizecombo.SelectedIndexChanged += new System.EventHandler(this.sizecombo_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Preview size:";
			// 
			// showimagesizes
			// 
			this.showimagesizes.AutoSize = true;
			this.showimagesizes.Location = new System.Drawing.Point(86, 19);
			this.showimagesizes.Name = "showimagesizes";
			this.showimagesizes.Size = new System.Drawing.Size(110, 17);
			this.showimagesizes.TabIndex = 0;
			this.showimagesizes.TabStop = false;
			this.showimagesizes.Text = "Show image sizes";
			this.showimagesizes.UseVisualStyleBackColor = true;
			this.showimagesizes.CheckedChanged += new System.EventHandler(this.showimagesizes_CheckedChanged);
			// 
			// usedimagesfirst
			// 
			this.usedimagesfirst.AutoSize = true;
			this.usedimagesfirst.Location = new System.Drawing.Point(86, 44);
			this.usedimagesfirst.Name = "usedimagesfirst";
			this.usedimagesfirst.Size = new System.Drawing.Size(135, 17);
			this.usedimagesfirst.TabIndex = 0;
			this.usedimagesfirst.TabStop = false;
			this.usedimagesfirst.Text = "Used images at the top";
			this.usedimagesfirst.UseVisualStyleBackColor = true;
			this.usedimagesfirst.CheckedChanged += new System.EventHandler(this.usedimagesfirst_CheckedChanged);
			// 
			// refreshtimer
			// 
			this.refreshtimer.Interval = 500;
			this.refreshtimer.Tick += new System.EventHandler(this.refreshtimer_Tick);
			// 
			// ImageBrowserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.splitter);
			this.Name = "ImageBrowserControl";
			this.Size = new System.Drawing.Size(900, 346);
			this.splitter.Panel1.ResumeLayout(false);
			this.splitter.Panel2.ResumeLayout(false);
			this.splitter.ResumeLayout(false);
			this.filtergroup.ResumeLayout(false);
			this.filtergroup.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitter;
		private mxd.DukeBuilder.Controls.ImageBrowserPanel list;
		private System.Windows.Forms.Timer refreshtimer;
		private System.Windows.Forms.TextBox objectname;
		private System.Windows.Forms.ComboBox po2combo;
		private System.Windows.Forms.Label label;
		private System.Windows.Forms.Label labelMixMode;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox filterWidth;
		private System.Windows.Forms.Label filterheightlabel;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox filterHeight;
		private System.Windows.Forms.Label filterwidthlabel;
		private System.Windows.Forms.CheckBox showimagesizes;
		private System.Windows.Forms.CheckBox usedimagesfirst;
		private System.Windows.Forms.ComboBox sizecombo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button objectclear;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox filtergroup;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox alphacombo;

	}
}
