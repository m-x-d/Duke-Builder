namespace mxd.DukeBuilder.Controls
{
	partial class FlagsInfoControl
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
			this.groupbox = new System.Windows.Forms.GroupBox();
			this.panel = new System.Windows.Forms.Panel();
			this.list = new System.Windows.Forms.ListView();
			this.groupbox.SuspendLayout();
			this.panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupbox
			// 
			this.groupbox.Controls.Add(this.panel);
			this.groupbox.Location = new System.Drawing.Point(0, 0);
			this.groupbox.Margin = new System.Windows.Forms.Padding(0);
			this.groupbox.Name = "groupbox";
			this.groupbox.Size = new System.Drawing.Size(200, 97);
			this.groupbox.TabIndex = 4;
			this.groupbox.TabStop = false;
			this.groupbox.Text = " Flags ";
			// 
			// panel
			// 
			this.panel.Controls.Add(this.list);
			this.panel.Location = new System.Drawing.Point(6, 19);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(188, 72);
			this.panel.TabIndex = 5;
			// 
			// list
			// 
			this.list.BackColor = System.Drawing.SystemColors.Control;
			this.list.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.list.CheckBoxes = true;
			this.list.LabelWrap = false;
			this.list.Location = new System.Drawing.Point(0, 0);
			this.list.Margin = new System.Windows.Forms.Padding(0);
			this.list.MultiSelect = false;
			this.list.Name = "list";
			this.list.Scrollable = false;
			this.list.Size = new System.Drawing.Size(188, 90);
			this.list.TabIndex = 2;
			this.list.TabStop = false;
			this.list.UseCompatibleStateImageBehavior = false;
			this.list.View = System.Windows.Forms.View.List;
			// 
			// FlagsInfoControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.groupbox);
			this.Name = "FlagsInfoControl";
			this.Size = new System.Drawing.Size(200, 100);
			this.groupbox.ResumeLayout(false);
			this.panel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupbox;
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.ListView list;
	}
}
