using System;
using System.Windows.Forms;
using mxd.DukeBuilder.Controls;

namespace mxd.DukeBuilder.Windows
{
	partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
			System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
			System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
			System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
			System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.seperatorfileopen = new System.Windows.Forms.ToolStripSeparator();
			this.seperatorfilerecent = new System.Windows.Forms.ToolStripSeparator();
			this.seperatoreditgrid = new System.Windows.Forms.ToolStripSeparator();
			this.seperatoreditcopypaste = new System.Windows.Forms.ToolStripSeparator();
			this.seperatorfile = new System.Windows.Forms.ToolStripSeparator();
			this.seperatorundo = new System.Windows.Forms.ToolStripSeparator();
			this.seperatorcopypaste = new System.Windows.Forms.ToolStripSeparator();
			this.seperatormodes = new System.Windows.Forms.ToolStripSeparator();
			this.poscommalabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.menumain = new System.Windows.Forms.MenuStrip();
			this.menufile = new System.Windows.Forms.ToolStripMenuItem();
			this.itemnewmap = new System.Windows.Forms.ToolStripMenuItem();
			this.itemopenmap = new System.Windows.Forms.ToolStripMenuItem();
			this.itemclosemap = new System.Windows.Forms.ToolStripMenuItem();
			this.itemsavemap = new System.Windows.Forms.ToolStripMenuItem();
			this.itemsavemapas = new System.Windows.Forms.ToolStripMenuItem();
			this.itemsavemapinto = new System.Windows.Forms.ToolStripMenuItem();
			this.seperatorfilesave = new System.Windows.Forms.ToolStripSeparator();
			this.itemnorecent = new System.Windows.Forms.ToolStripMenuItem();
			this.itemexit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuedit = new System.Windows.Forms.ToolStripMenuItem();
			this.itemundo = new System.Windows.Forms.ToolStripMenuItem();
			this.itemredo = new System.Windows.Forms.ToolStripMenuItem();
			this.seperatoreditundo = new System.Windows.Forms.ToolStripSeparator();
			this.itemcut = new System.Windows.Forms.ToolStripMenuItem();
			this.itemcopy = new System.Windows.Forms.ToolStripMenuItem();
			this.itempaste = new System.Windows.Forms.ToolStripMenuItem();
			this.itempastespecial = new System.Windows.Forms.ToolStripMenuItem();
			this.itemsnaptogrid = new System.Windows.Forms.ToolStripMenuItem();
			this.itemautomerge = new System.Windows.Forms.ToolStripMenuItem();
			this.seperatoreditgeometry = new System.Windows.Forms.ToolStripSeparator();
			this.itemgridinc = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgriddec = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgridsetup = new System.Windows.Forms.ToolStripMenuItem();
			this.itemmapoptions = new System.Windows.Forms.ToolStripMenuItem();
			this.menuview = new System.Windows.Forms.ToolStripMenuItem();
			this.itemthingsfilter = new System.Windows.Forms.ToolStripMenuItem();
			this.seperatorviewthings = new System.Windows.Forms.ToolStripSeparator();
			this.itemviewnormal = new System.Windows.Forms.ToolStripMenuItem();
			this.itemviewbrightness = new System.Windows.Forms.ToolStripMenuItem();
			this.itemviewfloors = new System.Windows.Forms.ToolStripMenuItem();
			this.itemviewceilings = new System.Windows.Forms.ToolStripMenuItem();
			this.seperatorviewviews = new System.Windows.Forms.ToolStripSeparator();
			this.menuzoom = new System.Windows.Forms.ToolStripMenuItem();
			this.item2zoom200 = new System.Windows.Forms.ToolStripMenuItem();
			this.item2zoom100 = new System.Windows.Forms.ToolStripMenuItem();
			this.item2zoom50 = new System.Windows.Forms.ToolStripMenuItem();
			this.item2zoom25 = new System.Windows.Forms.ToolStripMenuItem();
			this.item2zoom10 = new System.Windows.Forms.ToolStripMenuItem();
			this.item2zoom5 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemfittoscreen = new System.Windows.Forms.ToolStripMenuItem();
			this.itemtoggleinfo = new System.Windows.Forms.ToolStripMenuItem();
			this.seperatorviewzoom = new System.Windows.Forms.ToolStripSeparator();
			this.itemscripteditor = new System.Windows.Forms.ToolStripMenuItem();
			this.menumode = new System.Windows.Forms.ToolStripMenuItem();
			this.menutools = new System.Windows.Forms.ToolStripMenuItem();
			this.itemreloadresources = new System.Windows.Forms.ToolStripMenuItem();
			this.itemshowerrors = new System.Windows.Forms.ToolStripMenuItem();
			this.seperatortoolsresources = new System.Windows.Forms.ToolStripSeparator();
			this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.seperatortoolsconfig = new System.Windows.Forms.ToolStripSeparator();
			this.itemtestmap = new System.Windows.Forms.ToolStripMenuItem();
			this.menuhelp = new System.Windows.Forms.ToolStripMenuItem();
			this.itemhelprefmanual = new System.Windows.Forms.ToolStripMenuItem();
			this.itemhelpeditmode = new System.Windows.Forms.ToolStripMenuItem();
			this.seperatorhelpmanual = new System.Windows.Forms.ToolStripSeparator();
			this.itemhelpabout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolbar = new System.Windows.Forms.ToolStrip();
			this.buttonnewmap = new System.Windows.Forms.ToolStripButton();
			this.buttonopenmap = new System.Windows.Forms.ToolStripButton();
			this.buttonsavemap = new System.Windows.Forms.ToolStripButton();
			this.buttonundo = new System.Windows.Forms.ToolStripButton();
			this.buttonredo = new System.Windows.Forms.ToolStripButton();
			this.buttoncut = new System.Windows.Forms.ToolStripButton();
			this.buttoncopy = new System.Windows.Forms.ToolStripButton();
			this.buttonpaste = new System.Windows.Forms.ToolStripButton();
			this.buttonthingsfilter = new System.Windows.Forms.ToolStripButton();
			this.thingfilters = new System.Windows.Forms.ToolStripComboBox();
			this.buttonviewnormal = new System.Windows.Forms.ToolStripButton();
			this.buttonviewbrightness = new System.Windows.Forms.ToolStripButton();
			this.buttonviewfloors = new System.Windows.Forms.ToolStripButton();
			this.buttonviewceilings = new System.Windows.Forms.ToolStripButton();
			this.seperatorviews = new System.Windows.Forms.ToolStripSeparator();
			this.buttonsnaptogrid = new System.Windows.Forms.ToolStripButton();
			this.buttonautomerge = new System.Windows.Forms.ToolStripButton();
			this.seperatorgeometry = new System.Windows.Forms.ToolStripSeparator();
			this.buttontest = new System.Windows.Forms.ToolStripSplitButton();
			this.seperatortesting = new System.Windows.Forms.ToolStripSeparator();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statuslabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.configlabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.gridlabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.buttongrid = new System.Windows.Forms.ToolStripDropDownButton();
			this.itemgrid1024 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgrid512 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgrid256 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgrid128 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgrid64 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgrid32 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgrid16 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgrid8 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgrid4 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemgridcustom = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomlabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.buttonzoom = new System.Windows.Forms.ToolStripDropDownButton();
			this.itemzoom200 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemzoom100 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemzoom50 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemzoom25 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemzoom10 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemzoom5 = new System.Windows.Forms.ToolStripMenuItem();
			this.itemzoomfittoscreen = new System.Windows.Forms.ToolStripMenuItem();
			this.xposlabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.yposlabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.infopanel = new mxd.DukeBuilder.Controls.InfoPanelsControl();
			this.redrawtimer = new System.Windows.Forms.Timer(this.components);
			this.display = new mxd.DukeBuilder.Controls.RenderTargetControl();
			this.processor = new System.Windows.Forms.Timer(this.components);
			this.statusflasher = new System.Windows.Forms.Timer(this.components);
			this.statusresetter = new System.Windows.Forms.Timer(this.components);
			this.dockersspace = new System.Windows.Forms.Panel();
			this.dockerspanel = new mxd.DukeBuilder.Controls.DockersControl();
			this.dockerscollapser = new System.Windows.Forms.Timer(this.components);
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.menumain.SuspendLayout();
			this.toolbar.SuspendLayout();
			this.statusbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
			// 
			// toolStripSeparator9
			// 
			toolStripSeparator9.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			toolStripSeparator9.Name = "toolStripSeparator9";
			toolStripSeparator9.Size = new System.Drawing.Size(6, 23);
			// 
			// toolStripSeparator12
			// 
			toolStripSeparator12.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			toolStripSeparator12.Name = "toolStripSeparator12";
			toolStripSeparator12.Size = new System.Drawing.Size(6, 23);
			// 
			// toolStripMenuItem4
			// 
			toolStripMenuItem4.Name = "toolStripMenuItem4";
			toolStripMenuItem4.Size = new System.Drawing.Size(150, 6);
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new System.Drawing.Size(153, 6);
			// 
			// seperatorfileopen
			// 
			this.seperatorfileopen.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatorfileopen.Name = "seperatorfileopen";
			this.seperatorfileopen.Size = new System.Drawing.Size(199, 6);
			// 
			// seperatorfilerecent
			// 
			this.seperatorfilerecent.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatorfilerecent.Name = "seperatorfilerecent";
			this.seperatorfilerecent.Size = new System.Drawing.Size(199, 6);
			// 
			// seperatoreditgrid
			// 
			this.seperatoreditgrid.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatoreditgrid.Name = "seperatoreditgrid";
			this.seperatoreditgrid.Size = new System.Drawing.Size(160, 6);
			// 
			// seperatoreditcopypaste
			// 
			this.seperatoreditcopypaste.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatoreditcopypaste.Name = "seperatoreditcopypaste";
			this.seperatoreditcopypaste.Size = new System.Drawing.Size(160, 6);
			// 
			// seperatorfile
			// 
			this.seperatorfile.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.seperatorfile.Name = "seperatorfile";
			this.seperatorfile.Size = new System.Drawing.Size(6, 25);
			// 
			// seperatorundo
			// 
			this.seperatorundo.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.seperatorundo.Name = "seperatorundo";
			this.seperatorundo.Size = new System.Drawing.Size(6, 25);
			// 
			// seperatorcopypaste
			// 
			this.seperatorcopypaste.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.seperatorcopypaste.Name = "seperatorcopypaste";
			this.seperatorcopypaste.Size = new System.Drawing.Size(6, 25);
			// 
			// seperatormodes
			// 
			this.seperatormodes.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.seperatormodes.Name = "seperatormodes";
			this.seperatormodes.Size = new System.Drawing.Size(6, 25);
			this.seperatormodes.Visible = false;
			// 
			// poscommalabel
			// 
			this.poscommalabel.Name = "poscommalabel";
			this.poscommalabel.Size = new System.Drawing.Size(11, 18);
			this.poscommalabel.Text = ",";
			this.poscommalabel.ToolTipText = "Current X, Y coordinates on map";
			// 
			// menumain
			// 
			this.menumain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menufile,
            this.menuedit,
            this.menuview,
            this.menumode,
            this.menutools,
            this.menuhelp});
			this.menumain.Location = new System.Drawing.Point(0, 0);
			this.menumain.Name = "menumain";
			this.menumain.Size = new System.Drawing.Size(1012, 24);
			this.menumain.TabIndex = 0;
			// 
			// menufile
			// 
			this.menufile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemnewmap,
            this.itemopenmap,
            this.itemclosemap,
            this.seperatorfileopen,
            this.itemsavemap,
            this.itemsavemapas,
            this.itemsavemapinto,
            this.seperatorfilesave,
            this.itemnorecent,
            this.seperatorfilerecent,
            this.itemexit});
			this.menufile.Name = "menufile";
			this.menufile.Size = new System.Drawing.Size(37, 20);
			this.menufile.Text = "&File";
			// 
			// itemnewmap
			// 
			this.itemnewmap.Image = global::mxd.DukeBuilder.Properties.Resources.File;
			this.itemnewmap.Name = "itemnewmap";
			this.itemnewmap.ShortcutKeyDisplayString = "";
			this.itemnewmap.Size = new System.Drawing.Size(202, 22);
			this.itemnewmap.Tag = "dukebuilder_newmap";
			this.itemnewmap.Text = "&New Map";
			this.itemnewmap.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemopenmap
			// 
			this.itemopenmap.Image = global::mxd.DukeBuilder.Properties.Resources.OpenMap;
			this.itemopenmap.Name = "itemopenmap";
			this.itemopenmap.Size = new System.Drawing.Size(202, 22);
			this.itemopenmap.Tag = "dukebuilder_openmap";
			this.itemopenmap.Text = "&Open Map...";
			this.itemopenmap.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemclosemap
			// 
			this.itemclosemap.Name = "itemclosemap";
			this.itemclosemap.Size = new System.Drawing.Size(202, 22);
			this.itemclosemap.Tag = "dukebuilder_closemap";
			this.itemclosemap.Text = "&Close Map";
			this.itemclosemap.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemsavemap
			// 
			this.itemsavemap.Image = global::mxd.DukeBuilder.Properties.Resources.SaveMap;
			this.itemsavemap.Name = "itemsavemap";
			this.itemsavemap.Size = new System.Drawing.Size(202, 22);
			this.itemsavemap.Tag = "dukebuilder_savemap";
			this.itemsavemap.Text = "&Save Map";
			this.itemsavemap.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemsavemapas
			// 
			this.itemsavemapas.Name = "itemsavemapas";
			this.itemsavemapas.Size = new System.Drawing.Size(202, 22);
			this.itemsavemapas.Tag = "dukebuilder_savemapas";
			this.itemsavemapas.Text = "Save Map &As...";
			this.itemsavemapas.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemsavemapinto
			// 
			this.itemsavemapinto.Name = "itemsavemapinto";
			this.itemsavemapinto.Size = new System.Drawing.Size(202, 22);
			this.itemsavemapinto.Tag = "dukebuilder_savemapinto";
			this.itemsavemapinto.Text = "Save Map &Into...";
			this.itemsavemapinto.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatorfilesave
			// 
			this.seperatorfilesave.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatorfilesave.Name = "seperatorfilesave";
			this.seperatorfilesave.Size = new System.Drawing.Size(199, 6);
			// 
			// itemnorecent
			// 
			this.itemnorecent.Enabled = false;
			this.itemnorecent.Name = "itemnorecent";
			this.itemnorecent.Size = new System.Drawing.Size(202, 22);
			this.itemnorecent.Text = "No recently opened files";
			// 
			// itemexit
			// 
			this.itemexit.Name = "itemexit";
			this.itemexit.Size = new System.Drawing.Size(202, 22);
			this.itemexit.Text = "E&xit";
			this.itemexit.Click += new System.EventHandler(this.itemexit_Click);
			// 
			// menuedit
			// 
			this.menuedit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemundo,
            this.itemredo,
            this.seperatoreditundo,
            this.itemcut,
            this.itemcopy,
            this.itempaste,
            this.itempastespecial,
            this.seperatoreditcopypaste,
            this.itemsnaptogrid,
            this.itemautomerge,
            this.seperatoreditgeometry,
            this.itemgridinc,
            this.itemgriddec,
            this.itemgridsetup,
            this.seperatoreditgrid,
            this.itemmapoptions});
			this.menuedit.Name = "menuedit";
			this.menuedit.Size = new System.Drawing.Size(39, 20);
			this.menuedit.Text = "&Edit";
			// 
			// itemundo
			// 
			this.itemundo.Image = global::mxd.DukeBuilder.Properties.Resources.Undo;
			this.itemundo.Name = "itemundo";
			this.itemundo.Size = new System.Drawing.Size(163, 22);
			this.itemundo.Tag = "dukebuilder_undo";
			this.itemundo.Text = "&Undo";
			this.itemundo.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemredo
			// 
			this.itemredo.Image = global::mxd.DukeBuilder.Properties.Resources.Redo;
			this.itemredo.Name = "itemredo";
			this.itemredo.Size = new System.Drawing.Size(163, 22);
			this.itemredo.Tag = "dukebuilder_redo";
			this.itemredo.Text = "&Redo";
			this.itemredo.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatoreditundo
			// 
			this.seperatoreditundo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatoreditundo.Name = "seperatoreditundo";
			this.seperatoreditundo.Size = new System.Drawing.Size(160, 6);
			// 
			// itemcut
			// 
			this.itemcut.Image = global::mxd.DukeBuilder.Properties.Resources.Cut;
			this.itemcut.Name = "itemcut";
			this.itemcut.Size = new System.Drawing.Size(163, 22);
			this.itemcut.Tag = "dukebuilder_cutselection";
			this.itemcut.Text = "Cu&t";
			this.itemcut.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemcopy
			// 
			this.itemcopy.Image = global::mxd.DukeBuilder.Properties.Resources.Copy;
			this.itemcopy.Name = "itemcopy";
			this.itemcopy.Size = new System.Drawing.Size(163, 22);
			this.itemcopy.Tag = "dukebuilder_copyselection";
			this.itemcopy.Text = "&Copy";
			this.itemcopy.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itempaste
			// 
			this.itempaste.Image = global::mxd.DukeBuilder.Properties.Resources.Paste;
			this.itempaste.Name = "itempaste";
			this.itempaste.Size = new System.Drawing.Size(163, 22);
			this.itempaste.Tag = "dukebuilder_pasteselection";
			this.itempaste.Text = "&Paste";
			this.itempaste.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itempastespecial
			// 
			this.itempastespecial.Image = global::mxd.DukeBuilder.Properties.Resources.PasteSpecial;
			this.itempastespecial.Name = "itempastespecial";
			this.itempastespecial.Size = new System.Drawing.Size(163, 22);
			this.itempastespecial.Tag = "dukebuilder_pasteselectionspecial";
			this.itempastespecial.Text = "Paste Special...";
			this.itempastespecial.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemsnaptogrid
			// 
			this.itemsnaptogrid.Checked = true;
			this.itemsnaptogrid.CheckState = System.Windows.Forms.CheckState.Checked;
			this.itemsnaptogrid.Image = global::mxd.DukeBuilder.Properties.Resources.Grid4;
			this.itemsnaptogrid.Name = "itemsnaptogrid";
			this.itemsnaptogrid.Size = new System.Drawing.Size(163, 22);
			this.itemsnaptogrid.Tag = "dukebuilder_togglesnap";
			this.itemsnaptogrid.Text = "&Snap to Grid";
			this.itemsnaptogrid.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemautomerge
			// 
			this.itemautomerge.Checked = true;
			this.itemautomerge.CheckState = System.Windows.Forms.CheckState.Checked;
			this.itemautomerge.Image = global::mxd.DukeBuilder.Properties.Resources.mergegeometry2;
			this.itemautomerge.Name = "itemautomerge";
			this.itemautomerge.Size = new System.Drawing.Size(163, 22);
			this.itemautomerge.Tag = "dukebuilder_toggleautomerge";
			this.itemautomerge.Text = "&Merge Geometry";
			this.itemautomerge.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatoreditgeometry
			// 
			this.seperatoreditgeometry.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatoreditgeometry.Name = "seperatoreditgeometry";
			this.seperatoreditgeometry.Size = new System.Drawing.Size(160, 6);
			// 
			// itemgridinc
			// 
			this.itemgridinc.Name = "itemgridinc";
			this.itemgridinc.Size = new System.Drawing.Size(163, 22);
			this.itemgridinc.Tag = "dukebuilder_griddec";
			this.itemgridinc.Text = "&Increase Grid";
			this.itemgridinc.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemgriddec
			// 
			this.itemgriddec.Name = "itemgriddec";
			this.itemgriddec.Size = new System.Drawing.Size(163, 22);
			this.itemgriddec.Tag = "dukebuilder_gridinc";
			this.itemgriddec.Text = "&Decrease Grid";
			this.itemgriddec.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemgridsetup
			// 
			this.itemgridsetup.Image = global::mxd.DukeBuilder.Properties.Resources.Grid2;
			this.itemgridsetup.Name = "itemgridsetup";
			this.itemgridsetup.Size = new System.Drawing.Size(163, 22);
			this.itemgridsetup.Tag = "dukebuilder_gridsetup";
			this.itemgridsetup.Text = "&Grid Setup...";
			this.itemgridsetup.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemmapoptions
			// 
			this.itemmapoptions.Image = global::mxd.DukeBuilder.Properties.Resources.Properties;
			this.itemmapoptions.Name = "itemmapoptions";
			this.itemmapoptions.Size = new System.Drawing.Size(163, 22);
			this.itemmapoptions.Tag = "dukebuilder_mapoptions";
			this.itemmapoptions.Text = "Map &Options....";
			this.itemmapoptions.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// menuview
			// 
			this.menuview.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemthingsfilter,
            this.seperatorviewthings,
            this.itemviewnormal,
            this.itemviewbrightness,
            this.itemviewfloors,
            this.itemviewceilings,
            this.seperatorviewviews,
            this.menuzoom,
            this.itemfittoscreen,
            this.itemtoggleinfo,
            this.seperatorviewzoom,
            this.itemscripteditor});
			this.menuview.Name = "menuview";
			this.menuview.Size = new System.Drawing.Size(44, 20);
			this.menuview.Text = "&View";
			// 
			// itemthingsfilter
			// 
			this.itemthingsfilter.Image = global::mxd.DukeBuilder.Properties.Resources.Filter;
			this.itemthingsfilter.Name = "itemthingsfilter";
			this.itemthingsfilter.Size = new System.Drawing.Size(209, 22);
			this.itemthingsfilter.Tag = "dukebuilder_thingsfilterssetup";
			this.itemthingsfilter.Text = "Configure &Things Filters...";
			this.itemthingsfilter.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatorviewthings
			// 
			this.seperatorviewthings.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatorviewthings.Name = "seperatorviewthings";
			this.seperatorviewthings.Size = new System.Drawing.Size(206, 6);
			// 
			// itemviewnormal
			// 
			this.itemviewnormal.Image = global::mxd.DukeBuilder.Properties.Resources.ViewNormal;
			this.itemviewnormal.Name = "itemviewnormal";
			this.itemviewnormal.Size = new System.Drawing.Size(209, 22);
			this.itemviewnormal.Tag = "dukebuilder_viewmodenormal";
			this.itemviewnormal.Text = "&Wireframe";
			this.itemviewnormal.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemviewbrightness
			// 
			this.itemviewbrightness.Image = global::mxd.DukeBuilder.Properties.Resources.ViewBrightness;
			this.itemviewbrightness.Name = "itemviewbrightness";
			this.itemviewbrightness.Size = new System.Drawing.Size(209, 22);
			this.itemviewbrightness.Tag = "dukebuilder_viewmodebrightness";
			this.itemviewbrightness.Text = "&Brightness Levels";
			this.itemviewbrightness.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemviewfloors
			// 
			this.itemviewfloors.Image = global::mxd.DukeBuilder.Properties.Resources.ViewTextureFloor;
			this.itemviewfloors.Name = "itemviewfloors";
			this.itemviewfloors.Size = new System.Drawing.Size(209, 22);
			this.itemviewfloors.Tag = "dukebuilder_viewmodefloors";
			this.itemviewfloors.Text = "&Floor Textures";
			this.itemviewfloors.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemviewceilings
			// 
			this.itemviewceilings.Image = global::mxd.DukeBuilder.Properties.Resources.ViewTextureCeiling;
			this.itemviewceilings.Name = "itemviewceilings";
			this.itemviewceilings.Size = new System.Drawing.Size(209, 22);
			this.itemviewceilings.Tag = "dukebuilder_viewmodeceilings";
			this.itemviewceilings.Text = "&Ceiling Textures";
			this.itemviewceilings.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatorviewviews
			// 
			this.seperatorviewviews.Name = "seperatorviewviews";
			this.seperatorviewviews.Size = new System.Drawing.Size(206, 6);
			// 
			// menuzoom
			// 
			this.menuzoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.item2zoom200,
            this.item2zoom100,
            this.item2zoom50,
            this.item2zoom25,
            this.item2zoom10,
            this.item2zoom5});
			this.menuzoom.Image = global::mxd.DukeBuilder.Properties.Resources.Zoom;
			this.menuzoom.Name = "menuzoom";
			this.menuzoom.Size = new System.Drawing.Size(209, 22);
			this.menuzoom.Text = "&Zoom";
			// 
			// item2zoom200
			// 
			this.item2zoom200.Name = "item2zoom200";
			this.item2zoom200.Size = new System.Drawing.Size(102, 22);
			this.item2zoom200.Tag = "200";
			this.item2zoom200.Text = "200%";
			this.item2zoom200.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// item2zoom100
			// 
			this.item2zoom100.Name = "item2zoom100";
			this.item2zoom100.Size = new System.Drawing.Size(102, 22);
			this.item2zoom100.Tag = "100";
			this.item2zoom100.Text = "100%";
			this.item2zoom100.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// item2zoom50
			// 
			this.item2zoom50.Name = "item2zoom50";
			this.item2zoom50.Size = new System.Drawing.Size(102, 22);
			this.item2zoom50.Tag = "50";
			this.item2zoom50.Text = "50%";
			this.item2zoom50.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// item2zoom25
			// 
			this.item2zoom25.Name = "item2zoom25";
			this.item2zoom25.Size = new System.Drawing.Size(102, 22);
			this.item2zoom25.Tag = "25";
			this.item2zoom25.Text = "25%";
			this.item2zoom25.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// item2zoom10
			// 
			this.item2zoom10.Name = "item2zoom10";
			this.item2zoom10.Size = new System.Drawing.Size(102, 22);
			this.item2zoom10.Tag = "10";
			this.item2zoom10.Text = "10%";
			this.item2zoom10.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// item2zoom5
			// 
			this.item2zoom5.Name = "item2zoom5";
			this.item2zoom5.Size = new System.Drawing.Size(102, 22);
			this.item2zoom5.Tag = "5";
			this.item2zoom5.Text = "5%";
			this.item2zoom5.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// itemfittoscreen
			// 
			this.itemfittoscreen.Name = "itemfittoscreen";
			this.itemfittoscreen.Size = new System.Drawing.Size(209, 22);
			this.itemfittoscreen.Tag = "dukebuilder_centerinscreen";
			this.itemfittoscreen.Text = "Fit to screen";
			this.itemfittoscreen.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemtoggleinfo
			// 
			this.itemtoggleinfo.Name = "itemtoggleinfo";
			this.itemtoggleinfo.Size = new System.Drawing.Size(209, 22);
			this.itemtoggleinfo.Tag = "dukebuilder_toggleinfopanel";
			this.itemtoggleinfo.Text = "&Expanded Info Panel";
			this.itemtoggleinfo.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatorviewzoom
			// 
			this.seperatorviewzoom.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatorviewzoom.Name = "seperatorviewzoom";
			this.seperatorviewzoom.Size = new System.Drawing.Size(206, 6);
			// 
			// itemscripteditor
			// 
			this.itemscripteditor.Image = global::mxd.DukeBuilder.Properties.Resources.Close;
			this.itemscripteditor.Name = "itemscripteditor";
			this.itemscripteditor.Size = new System.Drawing.Size(209, 22);
			this.itemscripteditor.Tag = "dukebuilder_openscripteditor";
			this.itemscripteditor.Text = "&Script Editor...";
			this.itemscripteditor.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// menumode
			// 
			this.menumode.Name = "menumode";
			this.menumode.Size = new System.Drawing.Size(50, 20);
			this.menumode.Text = "&Mode";
			// 
			// menutools
			// 
			this.menutools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemreloadresources,
            this.itemshowerrors,
            this.seperatortoolsresources,
            this.configurationToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.seperatortoolsconfig,
            this.itemtestmap});
			this.menutools.Name = "menutools";
			this.menutools.Size = new System.Drawing.Size(48, 20);
			this.menutools.Text = "&Tools";
			// 
			// itemreloadresources
			// 
			this.itemreloadresources.Name = "itemreloadresources";
			this.itemreloadresources.Size = new System.Drawing.Size(196, 22);
			this.itemreloadresources.Tag = "dukebuilder_reloadresources";
			this.itemreloadresources.Text = "&Reload Resources";
			this.itemreloadresources.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// itemshowerrors
			// 
			this.itemshowerrors.Image = global::mxd.DukeBuilder.Properties.Resources.Warning;
			this.itemshowerrors.Name = "itemshowerrors";
			this.itemshowerrors.Size = new System.Drawing.Size(196, 22);
			this.itemshowerrors.Tag = "dukebuilder_showerrors";
			this.itemshowerrors.Text = "&Errors and Warnings...";
			this.itemshowerrors.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatortoolsresources
			// 
			this.seperatortoolsresources.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatortoolsresources.Name = "seperatortoolsresources";
			this.seperatortoolsresources.Size = new System.Drawing.Size(193, 6);
			// 
			// configurationToolStripMenuItem
			// 
			this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
			this.configurationToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.configurationToolStripMenuItem.Tag = "dukebuilder_configuration";
			this.configurationToolStripMenuItem.Text = "&Game Configurations...";
			this.configurationToolStripMenuItem.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// preferencesToolStripMenuItem
			// 
			this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
			this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.preferencesToolStripMenuItem.Tag = "dukebuilder_preferences";
			this.preferencesToolStripMenuItem.Text = "Preferences...";
			this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatortoolsconfig
			// 
			this.seperatortoolsconfig.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatortoolsconfig.Name = "seperatortoolsconfig";
			this.seperatortoolsconfig.Size = new System.Drawing.Size(193, 6);
			// 
			// itemtestmap
			// 
			this.itemtestmap.Image = global::mxd.DukeBuilder.Properties.Resources.Test;
			this.itemtestmap.Name = "itemtestmap";
			this.itemtestmap.Size = new System.Drawing.Size(196, 22);
			this.itemtestmap.Tag = "dukebuilder_testmap";
			this.itemtestmap.Text = "&Test Map";
			this.itemtestmap.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// menuhelp
			// 
			this.menuhelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemhelprefmanual,
            this.itemhelpeditmode,
            this.seperatorhelpmanual,
            this.itemhelpabout});
			this.menuhelp.Name = "menuhelp";
			this.menuhelp.Size = new System.Drawing.Size(44, 20);
			this.menuhelp.Text = "&Help";
			// 
			// itemhelprefmanual
			// 
			this.itemhelprefmanual.Image = global::mxd.DukeBuilder.Properties.Resources.Help;
			this.itemhelprefmanual.Name = "itemhelprefmanual";
			this.itemhelprefmanual.Size = new System.Drawing.Size(203, 22);
			this.itemhelprefmanual.Text = "Reference &Manual";
			this.itemhelprefmanual.Click += new System.EventHandler(this.itemhelprefmanual_Click);
			// 
			// itemhelpeditmode
			// 
			this.itemhelpeditmode.Image = global::mxd.DukeBuilder.Properties.Resources.Question;
			this.itemhelpeditmode.Name = "itemhelpeditmode";
			this.itemhelpeditmode.Size = new System.Drawing.Size(203, 22);
			this.itemhelpeditmode.Text = "About this &Editing Mode";
			this.itemhelpeditmode.Click += new System.EventHandler(this.itemhelpeditmode_Click);
			// 
			// seperatorhelpmanual
			// 
			this.seperatorhelpmanual.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.seperatorhelpmanual.Name = "seperatorhelpmanual";
			this.seperatorhelpmanual.Size = new System.Drawing.Size(200, 6);
			// 
			// itemhelpabout
			// 
			this.itemhelpabout.Name = "itemhelpabout";
			this.itemhelpabout.Size = new System.Drawing.Size(203, 22);
			this.itemhelpabout.Text = "&About Duke Builder...";
			this.itemhelpabout.Click += new System.EventHandler(this.itemhelpabout_Click);
			// 
			// toolbar
			// 
			this.toolbar.AutoSize = false;
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonnewmap,
            this.buttonopenmap,
            this.buttonsavemap,
            this.seperatorfile,
            this.buttonundo,
            this.buttonredo,
            this.seperatorundo,
            this.buttoncut,
            this.buttoncopy,
            this.buttonpaste,
            this.seperatorcopypaste,
            this.seperatormodes,
            this.buttonthingsfilter,
            this.thingfilters,
            this.buttonviewnormal,
            this.buttonviewbrightness,
            this.buttonviewfloors,
            this.buttonviewceilings,
            this.seperatorviews,
            this.buttonsnaptogrid,
            this.buttonautomerge,
            this.seperatorgeometry,
            this.buttontest,
            this.seperatortesting});
			this.toolbar.Location = new System.Drawing.Point(0, 24);
			this.toolbar.Name = "toolbar";
			this.toolbar.Size = new System.Drawing.Size(1012, 25);
			this.toolbar.TabIndex = 1;
			// 
			// buttonnewmap
			// 
			this.buttonnewmap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonnewmap.Image = global::mxd.DukeBuilder.Properties.Resources.NewMap;
			this.buttonnewmap.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonnewmap.Margin = new System.Windows.Forms.Padding(6, 1, 0, 2);
			this.buttonnewmap.Name = "buttonnewmap";
			this.buttonnewmap.Size = new System.Drawing.Size(23, 22);
			this.buttonnewmap.Tag = "dukebuilder_newmap";
			this.buttonnewmap.Text = "New Map";
			this.buttonnewmap.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttonopenmap
			// 
			this.buttonopenmap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonopenmap.Image = global::mxd.DukeBuilder.Properties.Resources.OpenMap;
			this.buttonopenmap.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonopenmap.Name = "buttonopenmap";
			this.buttonopenmap.Size = new System.Drawing.Size(23, 22);
			this.buttonopenmap.Tag = "dukebuilder_openmap";
			this.buttonopenmap.Text = "Open Map";
			this.buttonopenmap.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttonsavemap
			// 
			this.buttonsavemap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonsavemap.Image = global::mxd.DukeBuilder.Properties.Resources.SaveMap;
			this.buttonsavemap.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonsavemap.Name = "buttonsavemap";
			this.buttonsavemap.Size = new System.Drawing.Size(23, 22);
			this.buttonsavemap.Tag = "dukebuilder_savemap";
			this.buttonsavemap.Text = "Save Map";
			this.buttonsavemap.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttonundo
			// 
			this.buttonundo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonundo.Image = global::mxd.DukeBuilder.Properties.Resources.Undo;
			this.buttonundo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonundo.Name = "buttonundo";
			this.buttonundo.Size = new System.Drawing.Size(23, 22);
			this.buttonundo.Tag = "dukebuilder_undo";
			this.buttonundo.Text = "Undo";
			this.buttonundo.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttonredo
			// 
			this.buttonredo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonredo.Image = global::mxd.DukeBuilder.Properties.Resources.Redo;
			this.buttonredo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonredo.Name = "buttonredo";
			this.buttonredo.Size = new System.Drawing.Size(23, 22);
			this.buttonredo.Tag = "dukebuilder_redo";
			this.buttonredo.Text = "Redo";
			this.buttonredo.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttoncut
			// 
			this.buttoncut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttoncut.Image = global::mxd.DukeBuilder.Properties.Resources.Cut;
			this.buttoncut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttoncut.Name = "buttoncut";
			this.buttoncut.Size = new System.Drawing.Size(23, 22);
			this.buttoncut.Tag = "dukebuilder_cutselection";
			this.buttoncut.Text = "Cut Selection";
			this.buttoncut.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttoncopy
			// 
			this.buttoncopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttoncopy.Image = global::mxd.DukeBuilder.Properties.Resources.Copy;
			this.buttoncopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttoncopy.Name = "buttoncopy";
			this.buttoncopy.Size = new System.Drawing.Size(23, 22);
			this.buttoncopy.Tag = "dukebuilder_copyselection";
			this.buttoncopy.Text = "Copy Selection";
			this.buttoncopy.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttonpaste
			// 
			this.buttonpaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonpaste.Image = global::mxd.DukeBuilder.Properties.Resources.Paste;
			this.buttonpaste.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonpaste.Name = "buttonpaste";
			this.buttonpaste.Size = new System.Drawing.Size(23, 22);
			this.buttonpaste.Tag = "dukebuilder_pasteselection";
			this.buttonpaste.Text = "Paste Selection";
			this.buttonpaste.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttonthingsfilter
			// 
			this.buttonthingsfilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonthingsfilter.Enabled = false;
			this.buttonthingsfilter.Image = global::mxd.DukeBuilder.Properties.Resources.Filter;
			this.buttonthingsfilter.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonthingsfilter.Name = "buttonthingsfilter";
			this.buttonthingsfilter.Size = new System.Drawing.Size(23, 22);
			this.buttonthingsfilter.Tag = "dukebuilder_thingsfilterssetup";
			this.buttonthingsfilter.Text = "Configure Things Filters";
			this.buttonthingsfilter.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// thingfilters
			// 
			this.thingfilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.thingfilters.Enabled = false;
			this.thingfilters.Items.AddRange(new object[] {
            "(none)",
            "(custom)",
            "Easy skill items only",
            "Medium skill items only",
            "Hard skill items only"});
			this.thingfilters.Margin = new System.Windows.Forms.Padding(1, 0, 6, 0);
			this.thingfilters.Name = "thingfilters";
			this.thingfilters.Size = new System.Drawing.Size(130, 25);
			this.thingfilters.ToolTipText = "Things Filter";
			this.thingfilters.SelectedIndexChanged += new System.EventHandler(this.thingfilters_SelectedIndexChanged);
			this.thingfilters.DropDownClosed += new System.EventHandler(this.LoseFocus);
			// 
			// buttonviewnormal
			// 
			this.buttonviewnormal.CheckOnClick = true;
			this.buttonviewnormal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonviewnormal.Image = global::mxd.DukeBuilder.Properties.Resources.ViewNormal;
			this.buttonviewnormal.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonviewnormal.Name = "buttonviewnormal";
			this.buttonviewnormal.Size = new System.Drawing.Size(23, 22);
			this.buttonviewnormal.Tag = "dukebuilder_viewmodenormal";
			this.buttonviewnormal.Text = "View Wireframe";
			this.buttonviewnormal.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttonviewbrightness
			// 
			this.buttonviewbrightness.CheckOnClick = true;
			this.buttonviewbrightness.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonviewbrightness.Image = global::mxd.DukeBuilder.Properties.Resources.ViewBrightness;
			this.buttonviewbrightness.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonviewbrightness.Name = "buttonviewbrightness";
			this.buttonviewbrightness.Size = new System.Drawing.Size(23, 22);
			this.buttonviewbrightness.Tag = "dukebuilder_viewmodebrightness";
			this.buttonviewbrightness.Text = "View Brightness Levels";
			this.buttonviewbrightness.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttonviewfloors
			// 
			this.buttonviewfloors.CheckOnClick = true;
			this.buttonviewfloors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonviewfloors.Image = global::mxd.DukeBuilder.Properties.Resources.ViewTextureFloor;
			this.buttonviewfloors.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonviewfloors.Name = "buttonviewfloors";
			this.buttonviewfloors.Size = new System.Drawing.Size(23, 22);
			this.buttonviewfloors.Tag = "dukebuilder_viewmodefloors";
			this.buttonviewfloors.Text = "View Floor Textures";
			this.buttonviewfloors.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttonviewceilings
			// 
			this.buttonviewceilings.CheckOnClick = true;
			this.buttonviewceilings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonviewceilings.Image = global::mxd.DukeBuilder.Properties.Resources.ViewTextureCeiling;
			this.buttonviewceilings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonviewceilings.Name = "buttonviewceilings";
			this.buttonviewceilings.Size = new System.Drawing.Size(23, 22);
			this.buttonviewceilings.Tag = "dukebuilder_viewmodeceilings";
			this.buttonviewceilings.Text = "View Ceiling Textures";
			this.buttonviewceilings.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatorviews
			// 
			this.seperatorviews.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.seperatorviews.Name = "seperatorviews";
			this.seperatorviews.Size = new System.Drawing.Size(6, 25);
			// 
			// buttonsnaptogrid
			// 
			this.buttonsnaptogrid.Checked = true;
			this.buttonsnaptogrid.CheckState = System.Windows.Forms.CheckState.Checked;
			this.buttonsnaptogrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonsnaptogrid.Image = global::mxd.DukeBuilder.Properties.Resources.Grid4;
			this.buttonsnaptogrid.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonsnaptogrid.Name = "buttonsnaptogrid";
			this.buttonsnaptogrid.Size = new System.Drawing.Size(23, 22);
			this.buttonsnaptogrid.Tag = "dukebuilder_togglesnap";
			this.buttonsnaptogrid.Text = "Snap to Grid";
			this.buttonsnaptogrid.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// buttonautomerge
			// 
			this.buttonautomerge.Checked = true;
			this.buttonautomerge.CheckState = System.Windows.Forms.CheckState.Checked;
			this.buttonautomerge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonautomerge.Image = global::mxd.DukeBuilder.Properties.Resources.mergegeometry2;
			this.buttonautomerge.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonautomerge.Name = "buttonautomerge";
			this.buttonautomerge.Size = new System.Drawing.Size(23, 22);
			this.buttonautomerge.Tag = "dukebuilder_toggleautomerge";
			this.buttonautomerge.Text = "Merge Geometry";
			this.buttonautomerge.Click += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatorgeometry
			// 
			this.seperatorgeometry.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.seperatorgeometry.Name = "seperatorgeometry";
			this.seperatorgeometry.Size = new System.Drawing.Size(6, 25);
			// 
			// buttontest
			// 
			this.buttontest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttontest.Image = global::mxd.DukeBuilder.Properties.Resources.Test;
			this.buttontest.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.buttontest.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttontest.Name = "buttontest";
			this.buttontest.Size = new System.Drawing.Size(32, 22);
			this.buttontest.Tag = "dukebuilder_testmap";
			this.buttontest.Text = "Test Map";
			this.buttontest.ButtonClick += new System.EventHandler(this.InvokeTaggedAction);
			// 
			// seperatortesting
			// 
			this.seperatortesting.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.seperatortesting.Name = "seperatortesting";
			this.seperatortesting.Size = new System.Drawing.Size(6, 25);
			// 
			// statusbar
			// 
			this.statusbar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statuslabel,
            this.configlabel,
            toolStripSeparator12,
            this.gridlabel,
            this.buttongrid,
            toolStripSeparator1,
            this.zoomlabel,
            this.buttonzoom,
            toolStripSeparator9,
            this.xposlabel,
            this.poscommalabel,
            this.yposlabel});
			this.statusbar.Location = new System.Drawing.Point(0, 670);
			this.statusbar.Name = "statusbar";
			this.statusbar.ShowItemToolTips = true;
			this.statusbar.Size = new System.Drawing.Size(1012, 23);
			this.statusbar.TabIndex = 2;
			// 
			// statuslabel
			// 
			this.statuslabel.Image = global::mxd.DukeBuilder.Properties.Resources.Status2;
			this.statuslabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.statuslabel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.statuslabel.Name = "statuslabel";
			this.statuslabel.Size = new System.Drawing.Size(396, 18);
			this.statuslabel.Spring = true;
			this.statuslabel.Text = "Initializing user interface...";
			this.statuslabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// configlabel
			// 
			this.configlabel.AutoSize = false;
			this.configlabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.configlabel.Name = "configlabel";
			this.configlabel.Size = new System.Drawing.Size(280, 18);
			this.configlabel.Text = "ZDoom (Doom in Hexen Format)";
			this.configlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.configlabel.ToolTipText = "Current Game Configuration";
			// 
			// gridlabel
			// 
			this.gridlabel.AutoSize = false;
			this.gridlabel.AutoToolTip = true;
			this.gridlabel.Name = "gridlabel";
			this.gridlabel.Size = new System.Drawing.Size(62, 18);
			this.gridlabel.Text = "32 mp";
			this.gridlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.gridlabel.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
			this.gridlabel.ToolTipText = "Grid size";
			// 
			// buttongrid
			// 
			this.buttongrid.AutoToolTip = false;
			this.buttongrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttongrid.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemgrid1024,
            this.itemgrid512,
            this.itemgrid256,
            this.itemgrid128,
            this.itemgrid64,
            this.itemgrid32,
            this.itemgrid16,
            this.itemgrid8,
            this.itemgrid4,
            toolStripMenuItem4,
            this.itemgridcustom});
			this.buttongrid.Image = global::mxd.DukeBuilder.Properties.Resources.Grid2_arrowup;
			this.buttongrid.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.buttongrid.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.buttongrid.Name = "buttongrid";
			this.buttongrid.ShowDropDownArrow = false;
			this.buttongrid.Size = new System.Drawing.Size(29, 21);
			this.buttongrid.Text = "Grid";
			// 
			// itemgrid1024
			// 
			this.itemgrid1024.Name = "itemgrid1024";
			this.itemgrid1024.Size = new System.Drawing.Size(153, 22);
			this.itemgrid1024.Tag = "1024";
			this.itemgrid1024.Text = "1024 mp";
			this.itemgrid1024.Click += new System.EventHandler(this.itemgridsize_Click);
			// 
			// itemgrid512
			// 
			this.itemgrid512.Name = "itemgrid512";
			this.itemgrid512.Size = new System.Drawing.Size(153, 22);
			this.itemgrid512.Tag = "512";
			this.itemgrid512.Text = "512 mp";
			this.itemgrid512.Click += new System.EventHandler(this.itemgridsize_Click);
			// 
			// itemgrid256
			// 
			this.itemgrid256.Name = "itemgrid256";
			this.itemgrid256.Size = new System.Drawing.Size(153, 22);
			this.itemgrid256.Tag = "256";
			this.itemgrid256.Text = "256 mp";
			this.itemgrid256.Click += new System.EventHandler(this.itemgridsize_Click);
			// 
			// itemgrid128
			// 
			this.itemgrid128.Name = "itemgrid128";
			this.itemgrid128.Size = new System.Drawing.Size(153, 22);
			this.itemgrid128.Tag = "128";
			this.itemgrid128.Text = "128 mp";
			this.itemgrid128.Click += new System.EventHandler(this.itemgridsize_Click);
			// 
			// itemgrid64
			// 
			this.itemgrid64.Name = "itemgrid64";
			this.itemgrid64.Size = new System.Drawing.Size(153, 22);
			this.itemgrid64.Tag = "64";
			this.itemgrid64.Text = "64 mp";
			this.itemgrid64.Click += new System.EventHandler(this.itemgridsize_Click);
			// 
			// itemgrid32
			// 
			this.itemgrid32.Name = "itemgrid32";
			this.itemgrid32.Size = new System.Drawing.Size(153, 22);
			this.itemgrid32.Tag = "32";
			this.itemgrid32.Text = "32 mp";
			this.itemgrid32.Click += new System.EventHandler(this.itemgridsize_Click);
			// 
			// itemgrid16
			// 
			this.itemgrid16.Name = "itemgrid16";
			this.itemgrid16.Size = new System.Drawing.Size(153, 22);
			this.itemgrid16.Tag = "16";
			this.itemgrid16.Text = "16 mp";
			this.itemgrid16.Click += new System.EventHandler(this.itemgridsize_Click);
			// 
			// itemgrid8
			// 
			this.itemgrid8.Name = "itemgrid8";
			this.itemgrid8.Size = new System.Drawing.Size(153, 22);
			this.itemgrid8.Tag = "8";
			this.itemgrid8.Text = "8 mp";
			this.itemgrid8.Click += new System.EventHandler(this.itemgridsize_Click);
			// 
			// itemgrid4
			// 
			this.itemgrid4.Name = "itemgrid4";
			this.itemgrid4.Size = new System.Drawing.Size(153, 22);
			this.itemgrid4.Tag = "4";
			this.itemgrid4.Text = "4 mp";
			this.itemgrid4.Click += new System.EventHandler(this.itemgridsize_Click);
			// 
			// itemgridcustom
			// 
			this.itemgridcustom.Name = "itemgridcustom";
			this.itemgridcustom.Size = new System.Drawing.Size(153, 22);
			this.itemgridcustom.Text = "Customize...";
			this.itemgridcustom.Click += new System.EventHandler(this.itemgridcustom_Click);
			// 
			// zoomlabel
			// 
			this.zoomlabel.AutoSize = false;
			this.zoomlabel.AutoToolTip = true;
			this.zoomlabel.Name = "zoomlabel";
			this.zoomlabel.Size = new System.Drawing.Size(54, 18);
			this.zoomlabel.Text = "50%";
			this.zoomlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.zoomlabel.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
			this.zoomlabel.ToolTipText = "Zoom level";
			// 
			// buttonzoom
			// 
			this.buttonzoom.AutoToolTip = false;
			this.buttonzoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonzoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemzoom200,
            this.itemzoom100,
            this.itemzoom50,
            this.itemzoom25,
            this.itemzoom10,
            this.itemzoom5,
            toolStripSeparator2,
            this.itemzoomfittoscreen});
			this.buttonzoom.Image = global::mxd.DukeBuilder.Properties.Resources.Zoom_arrowup;
			this.buttonzoom.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.buttonzoom.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.buttonzoom.Name = "buttonzoom";
			this.buttonzoom.ShowDropDownArrow = false;
			this.buttonzoom.Size = new System.Drawing.Size(29, 21);
			this.buttonzoom.Text = "Zoom";
			// 
			// itemzoom200
			// 
			this.itemzoom200.Name = "itemzoom200";
			this.itemzoom200.Size = new System.Drawing.Size(156, 22);
			this.itemzoom200.Tag = "200";
			this.itemzoom200.Text = "200%";
			this.itemzoom200.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// itemzoom100
			// 
			this.itemzoom100.Name = "itemzoom100";
			this.itemzoom100.Size = new System.Drawing.Size(156, 22);
			this.itemzoom100.Tag = "100";
			this.itemzoom100.Text = "100%";
			this.itemzoom100.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// itemzoom50
			// 
			this.itemzoom50.Name = "itemzoom50";
			this.itemzoom50.Size = new System.Drawing.Size(156, 22);
			this.itemzoom50.Tag = "50";
			this.itemzoom50.Text = "50%";
			this.itemzoom50.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// itemzoom25
			// 
			this.itemzoom25.Name = "itemzoom25";
			this.itemzoom25.Size = new System.Drawing.Size(156, 22);
			this.itemzoom25.Tag = "25";
			this.itemzoom25.Text = "25%";
			this.itemzoom25.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// itemzoom10
			// 
			this.itemzoom10.Name = "itemzoom10";
			this.itemzoom10.Size = new System.Drawing.Size(156, 22);
			this.itemzoom10.Tag = "10";
			this.itemzoom10.Text = "10%";
			this.itemzoom10.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// itemzoom5
			// 
			this.itemzoom5.Name = "itemzoom5";
			this.itemzoom5.Size = new System.Drawing.Size(156, 22);
			this.itemzoom5.Tag = "5";
			this.itemzoom5.Text = "5%";
			this.itemzoom5.Click += new System.EventHandler(this.itemzoomto_Click);
			// 
			// itemzoomfittoscreen
			// 
			this.itemzoomfittoscreen.Name = "itemzoomfittoscreen";
			this.itemzoomfittoscreen.Size = new System.Drawing.Size(156, 22);
			this.itemzoomfittoscreen.Text = "Fit to screen";
			this.itemzoomfittoscreen.Click += new System.EventHandler(this.itemzoomfittoscreen_Click);
			// 
			// xposlabel
			// 
			this.xposlabel.AutoSize = false;
			this.xposlabel.Name = "xposlabel";
			this.xposlabel.Size = new System.Drawing.Size(50, 18);
			this.xposlabel.Text = "0";
			this.xposlabel.ToolTipText = "Current X, Y coordinates on map";
			// 
			// yposlabel
			// 
			this.yposlabel.AutoSize = false;
			this.yposlabel.Name = "yposlabel";
			this.yposlabel.Size = new System.Drawing.Size(50, 18);
			this.yposlabel.Text = "0";
			this.yposlabel.ToolTipText = "Current X, Y coordinates on map";
			// 
			// infopanel
			// 
			this.infopanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.infopanel.Location = new System.Drawing.Point(0, 570);
			this.infopanel.Name = "infopanel";
			this.infopanel.Size = new System.Drawing.Size(1012, 100);
			this.infopanel.TabIndex = 8;
			// 
			// redrawtimer
			// 
			this.redrawtimer.Interval = 1;
			this.redrawtimer.Tick += new System.EventHandler(this.redrawtimer_Tick);
			// 
			// display
			// 
			this.display.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.display.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.display.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.display.CausesValidation = false;
			this.display.Dock = System.Windows.Forms.DockStyle.Fill;
			this.display.Location = new System.Drawing.Point(0, 49);
			this.display.Name = "display";
			this.display.Size = new System.Drawing.Size(1012, 621);
			this.display.TabIndex = 5;
			this.display.MouseLeave += new System.EventHandler(this.display_MouseLeave);
			this.display.Paint += new System.Windows.Forms.PaintEventHandler(this.display_Paint);
			this.display.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.display_PreviewKeyDown);
			this.display.MouseMove += new System.Windows.Forms.MouseEventHandler(this.display_MouseMove);
			this.display.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.display_MouseDoubleClick);
			this.display.MouseClick += new System.Windows.Forms.MouseEventHandler(this.display_MouseClick);
			this.display.KeyUp += new System.Windows.Forms.KeyEventHandler(this.display_KeyUp);
			this.display.MouseDown += new System.Windows.Forms.MouseEventHandler(this.display_MouseDown);
			this.display.Resize += new System.EventHandler(this.display_Resize);
			this.display.MouseUp += new System.Windows.Forms.MouseEventHandler(this.display_MouseUp);
			this.display.MouseEnter += new System.EventHandler(this.display_MouseEnter);
			// 
			// processor
			// 
			this.processor.Interval = 10;
			this.processor.Tick += new System.EventHandler(this.processor_Tick);
			// 
			// statusflasher
			// 
			this.statusflasher.Tick += new System.EventHandler(this.statusflasher_Tick);
			// 
			// statusresetter
			// 
			this.statusresetter.Tick += new System.EventHandler(this.statusresetter_Tick);
			// 
			// dockersspace
			// 
			this.dockersspace.Dock = System.Windows.Forms.DockStyle.Left;
			this.dockersspace.Location = new System.Drawing.Point(0, 49);
			this.dockersspace.Name = "dockersspace";
			this.dockersspace.Size = new System.Drawing.Size(26, 521);
			this.dockersspace.TabIndex = 6;
			// 
			// dockerspanel
			// 
			this.dockerspanel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dockerspanel.Location = new System.Drawing.Point(62, 67);
			this.dockerspanel.Name = "dockerspanel";
			this.dockerspanel.Size = new System.Drawing.Size(236, 467);
			this.dockerspanel.TabIndex = 7;
			this.dockerspanel.TabStop = false;
			this.dockerspanel.UserResize += new System.EventHandler(this.dockerspanel_UserResize);
			this.dockerspanel.Collapsed += new System.EventHandler(this.LoseFocus);
			this.dockerspanel.MouseContainerEnter += new System.EventHandler(this.dockerspanel_MouseContainerEnter);
			// 
			// dockerscollapser
			// 
			this.dockerscollapser.Interval = 200;
			this.dockerscollapser.Tick += new System.EventHandler(this.dockerscollapser_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(1012, 693);
			this.Controls.Add(this.dockersspace);
			this.Controls.Add(this.infopanel);
			this.Controls.Add(this.dockerspanel);
			this.Controls.Add(this.display);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Controls.Add(this.menumain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.menumain;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Duke Builder";
			this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.MouseCaptureChanged += new System.EventHandler(this.MainForm_MouseCaptureChanged);
			this.Activated += new System.EventHandler(this.MainForm_Activated);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.menumain.ResumeLayout(false);
			this.menumain.PerformLayout();
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		
		#endregion

		private System.Windows.Forms.MenuStrip menumain;
		private System.Windows.Forms.ToolStrip toolbar;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.ToolStripMenuItem menufile;
		private System.Windows.Forms.ToolStripMenuItem itemnewmap;
		private System.Windows.Forms.ToolStripMenuItem itemopenmap;
		private System.Windows.Forms.ToolStripMenuItem itemsavemap;
		private System.Windows.Forms.ToolStripMenuItem itemsavemapas;
		private System.Windows.Forms.ToolStripMenuItem itemsavemapinto;
		private System.Windows.Forms.ToolStripMenuItem itemexit;
		private System.Windows.Forms.ToolStripStatusLabel statuslabel;
		private System.Windows.Forms.ToolStripMenuItem itemclosemap;
		private System.Windows.Forms.Timer redrawtimer;
		private System.Windows.Forms.ToolStripMenuItem menuhelp;
		private System.Windows.Forms.ToolStripMenuItem itemhelpabout;
		private mxd.DukeBuilder.Controls.RenderTargetControl display;
		private System.Windows.Forms.ToolStripMenuItem itemnorecent;
		private System.Windows.Forms.ToolStripStatusLabel xposlabel;
		private System.Windows.Forms.ToolStripStatusLabel yposlabel;
		private System.Windows.Forms.ToolStripButton buttonnewmap;
		private System.Windows.Forms.ToolStripButton buttonopenmap;
		private System.Windows.Forms.ToolStripButton buttonsavemap;
		private System.Windows.Forms.ToolStripStatusLabel zoomlabel;
		private System.Windows.Forms.ToolStripDropDownButton buttonzoom;
		private System.Windows.Forms.ToolStripMenuItem itemzoomfittoscreen;
		private System.Windows.Forms.ToolStripMenuItem itemzoom100;
		private System.Windows.Forms.ToolStripMenuItem itemzoom200;
		private System.Windows.Forms.ToolStripMenuItem itemzoom50;
		private System.Windows.Forms.ToolStripMenuItem itemzoom25;
		private System.Windows.Forms.ToolStripMenuItem itemzoom10;
		private System.Windows.Forms.ToolStripMenuItem itemzoom5;
		private System.Windows.Forms.ToolStripMenuItem menutools;
		private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem menuedit;
		private System.Windows.Forms.ToolStripMenuItem itemmapoptions;
		private System.Windows.Forms.ToolStripMenuItem itemreloadresources;
		private System.Windows.Forms.ToolStripButton buttonthingsfilter;
		private System.Windows.Forms.ToolStripComboBox thingfilters;
		private System.Windows.Forms.ToolStripSeparator seperatorviews;
		private System.Windows.Forms.ToolStripStatusLabel gridlabel;
		private System.Windows.Forms.ToolStripDropDownButton buttongrid;
		private System.Windows.Forms.ToolStripMenuItem itemgrid1024;
		private System.Windows.Forms.ToolStripMenuItem itemgrid256;
		private System.Windows.Forms.ToolStripMenuItem itemgrid128;
		private System.Windows.Forms.ToolStripMenuItem itemgrid64;
		private System.Windows.Forms.ToolStripMenuItem itemgrid32;
		private System.Windows.Forms.ToolStripMenuItem itemgrid16;
		private System.Windows.Forms.ToolStripMenuItem itemgrid4;
		private System.Windows.Forms.ToolStripMenuItem itemgrid8;
		private System.Windows.Forms.ToolStripMenuItem itemgridcustom;
		private System.Windows.Forms.ToolStripMenuItem itemgrid512;
		private System.Windows.Forms.ToolStripStatusLabel poscommalabel;
		private System.Windows.Forms.ToolStripMenuItem itemundo;
		private System.Windows.Forms.ToolStripMenuItem itemredo;
		private System.Windows.Forms.ToolStripButton buttonundo;
		private System.Windows.Forms.ToolStripButton buttonredo;
		private System.Windows.Forms.ToolStripButton buttonsnaptogrid;
		private System.Windows.Forms.ToolStripMenuItem itemsnaptogrid;
		private System.Windows.Forms.ToolStripButton buttonautomerge;
		private System.Windows.Forms.ToolStripMenuItem itemautomerge;
		private System.Windows.Forms.ToolStripSeparator seperatormodes;
		private System.Windows.Forms.Timer processor;
		private System.Windows.Forms.ToolStripSeparator seperatorgeometry;
		private System.Windows.Forms.ToolStripSeparator seperatorfilesave;
		private System.Windows.Forms.ToolStripSeparator seperatortesting;
		private System.Windows.Forms.ToolStripSeparator seperatoreditgeometry;
		private System.Windows.Forms.ToolStripMenuItem itemgridinc;
		private System.Windows.Forms.ToolStripMenuItem itemgriddec;
		private System.Windows.Forms.ToolStripMenuItem itemgridsetup;
		private System.Windows.Forms.Timer statusflasher;
		private System.Windows.Forms.ToolStripSplitButton buttontest;
		private System.Windows.Forms.ToolStripButton buttoncut;
		private System.Windows.Forms.ToolStripButton buttoncopy;
		private System.Windows.Forms.ToolStripButton buttonpaste;
		private System.Windows.Forms.ToolStripSeparator seperatoreditundo;
		private System.Windows.Forms.ToolStripMenuItem itemcut;
		private System.Windows.Forms.ToolStripMenuItem itemcopy;
		private System.Windows.Forms.ToolStripMenuItem itempaste;
		private System.Windows.Forms.ToolStripStatusLabel configlabel;
		private System.Windows.Forms.ToolStripMenuItem menumode;
		private System.Windows.Forms.ToolStripButton buttonviewnormal;
		private System.Windows.Forms.ToolStripButton buttonviewbrightness;
		private System.Windows.Forms.ToolStripButton buttonviewfloors;
		private System.Windows.Forms.ToolStripButton buttonviewceilings;
		private System.Windows.Forms.ToolStripSeparator seperatortoolsresources;
		private System.Windows.Forms.ToolStripMenuItem menuview;
		private System.Windows.Forms.ToolStripMenuItem itemthingsfilter;
		private System.Windows.Forms.ToolStripSeparator seperatorviewthings;
		private System.Windows.Forms.ToolStripMenuItem itemviewnormal;
		private System.Windows.Forms.ToolStripMenuItem itemviewbrightness;
		private System.Windows.Forms.ToolStripMenuItem itemviewfloors;
		private System.Windows.Forms.ToolStripMenuItem itemviewceilings;
		private System.Windows.Forms.ToolStripSeparator seperatorviewzoom;
		private System.Windows.Forms.ToolStripMenuItem itemscripteditor;
		private System.Windows.Forms.ToolStripSeparator seperatortoolsconfig;
		private System.Windows.Forms.ToolStripMenuItem itemtestmap;
		private System.Windows.Forms.Timer statusresetter;
		private System.Windows.Forms.ToolStripMenuItem itemshowerrors;
		private System.Windows.Forms.ToolStripSeparator seperatorviewviews;
		private System.Windows.Forms.ToolStripMenuItem menuzoom;
		private System.Windows.Forms.ToolStripMenuItem item2zoom5;
		private System.Windows.Forms.ToolStripMenuItem item2zoom10;
		private System.Windows.Forms.ToolStripMenuItem itemfittoscreen;
		private System.Windows.Forms.ToolStripMenuItem item2zoom200;
		private System.Windows.Forms.ToolStripMenuItem item2zoom100;
		private System.Windows.Forms.ToolStripMenuItem item2zoom50;
		private System.Windows.Forms.ToolStripMenuItem item2zoom25;
		private System.Windows.Forms.ToolStripMenuItem itemhelprefmanual;
		private System.Windows.Forms.ToolStripSeparator seperatorhelpmanual;
		private System.Windows.Forms.ToolStripMenuItem itemhelpeditmode;
		private System.Windows.Forms.ToolStripMenuItem itemtoggleinfo;
		private System.Windows.Forms.ToolStripMenuItem itempastespecial;
		private System.Windows.Forms.Panel dockersspace;
		private mxd.DukeBuilder.Controls.DockersControl dockerspanel;
		private System.Windows.Forms.Timer dockerscollapser;
		private System.Windows.Forms.ToolStripSeparator seperatorfile;
		private System.Windows.Forms.ToolStripSeparator seperatorundo;
		private System.Windows.Forms.ToolStripSeparator seperatorcopypaste;
		private System.Windows.Forms.ToolStripSeparator seperatorfileopen;
		private System.Windows.Forms.ToolStripSeparator seperatorfilerecent;
		private System.Windows.Forms.ToolStripSeparator seperatoreditgrid;
		private System.Windows.Forms.ToolStripSeparator seperatoreditcopypaste;
		private mxd.DukeBuilder.Controls.InfoPanelsControl infopanel;
	}
}