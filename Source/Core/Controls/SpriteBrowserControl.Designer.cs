namespace mxd.DukeBuilder.Controls
{
	partial class SpriteBrowserControl
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
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Monsters");
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpriteBrowserControl));
			this.typelist = new mxd.DukeBuilder.Controls.MultiSelectTreeview();
			this.thingimages = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// typelist
			// 
			this.typelist.Dock = System.Windows.Forms.DockStyle.Fill;
			this.typelist.HideSelection = false;
			this.typelist.ImageIndex = 0;
			this.typelist.ImageList = this.thingimages;
			this.typelist.Location = new System.Drawing.Point(0, 0);
			this.typelist.Margin = new System.Windows.Forms.Padding(0);
			this.typelist.Name = "typelist";
			treeNode1.Name = "Node0";
			treeNode1.Text = "Monsters";
			this.typelist.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
			this.typelist.SelectedImageIndex = 0;
			this.typelist.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			this.typelist.SelectionMode = mxd.DukeBuilder.Controls.TreeViewSelectionMode.SingleSelect;
			this.typelist.Size = new System.Drawing.Size(304, 320);
			this.typelist.TabIndex = 0;
			this.typelist.DoubleClick += new System.EventHandler(this.typelist_DoubleClick);
			this.typelist.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.typelist_AfterSelect);
			// 
			// thingimages
			// 
			this.thingimages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("thingimages.ImageStream")));
			this.thingimages.TransparentColor = System.Drawing.SystemColors.Window;
			this.thingimages.Images.SetKeyName(0, "thing00.png");
			this.thingimages.Images.SetKeyName(1, "thing01.png");
			this.thingimages.Images.SetKeyName(2, "thing02.png");
			this.thingimages.Images.SetKeyName(3, "thing03.png");
			this.thingimages.Images.SetKeyName(4, "thing04.png");
			this.thingimages.Images.SetKeyName(5, "thing05.png");
			this.thingimages.Images.SetKeyName(6, "thing06.png");
			this.thingimages.Images.SetKeyName(7, "thing07.png");
			this.thingimages.Images.SetKeyName(8, "thing08.png");
			this.thingimages.Images.SetKeyName(9, "thing09.png");
			this.thingimages.Images.SetKeyName(10, "thing10.png");
			this.thingimages.Images.SetKeyName(11, "thing11.png");
			this.thingimages.Images.SetKeyName(12, "thing12.png");
			this.thingimages.Images.SetKeyName(13, "thing13.png");
			this.thingimages.Images.SetKeyName(14, "thing14.png");
			this.thingimages.Images.SetKeyName(15, "thing15.png");
			this.thingimages.Images.SetKeyName(16, "thing16.png");
			this.thingimages.Images.SetKeyName(17, "thing17.png");
			this.thingimages.Images.SetKeyName(18, "thing18.png");
			this.thingimages.Images.SetKeyName(19, "thing19.png");
			// 
			// SpriteBrowserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.typelist);
			this.Name = "SpriteBrowserControl";
			this.Size = new System.Drawing.Size(304, 320);
			this.ResumeLayout(false);

		}

		#endregion

		private mxd.DukeBuilder.Controls.MultiSelectTreeview typelist;
		private System.Windows.Forms.ImageList thingimages;
	}
}
