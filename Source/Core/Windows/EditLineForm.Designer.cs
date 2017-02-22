namespace mxd.DukeBuilder.Windows
{
	partial class EditLineForm
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
			this.cancel = new System.Windows.Forms.Button();
			this.apply = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.tabs = new System.Windows.Forms.TabControl();
			this.tabfront = new System.Windows.Forms.TabPage();
			this.front = new mxd.DukeBuilder.Controls.WallPropertiesControl();
			this.tabback = new System.Windows.Forms.TabPage();
			this.back = new mxd.DukeBuilder.Controls.WallPropertiesControl();
			this.tabs.SuspendLayout();
			this.tabfront.SuspendLayout();
			this.tabback.SuspendLayout();
			this.SuspendLayout();
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.cancel.Location = new System.Drawing.Point(346, 480);
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
			this.apply.Location = new System.Drawing.Point(227, 480);
			this.apply.Name = "apply";
			this.apply.Size = new System.Drawing.Size(112, 25);
			this.apply.TabIndex = 1;
			this.apply.Text = "OK";
			this.apply.UseVisualStyleBackColor = true;
			this.apply.Click += new System.EventHandler(this.apply_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(0, 0);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(104, 24);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// tabs
			// 
			this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabs.Controls.Add(this.tabfront);
			this.tabs.Controls.Add(this.tabback);
			this.tabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabs.Location = new System.Drawing.Point(10, 10);
			this.tabs.Margin = new System.Windows.Forms.Padding(1);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(448, 466);
			this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabs.TabIndex = 0;
			// 
			// tabfront
			// 
			this.tabfront.Controls.Add(this.front);
			this.tabfront.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabfront.Location = new System.Drawing.Point(4, 22);
			this.tabfront.Name = "tabfront";
			this.tabfront.Size = new System.Drawing.Size(440, 440);
			this.tabfront.TabIndex = 0;
			this.tabfront.Text = "Front";
			this.tabfront.UseVisualStyleBackColor = true;
			// 
			// front
			// 
			this.front.Dock = System.Windows.Forms.DockStyle.Fill;
			this.front.Location = new System.Drawing.Point(0, 0);
			this.front.Margin = new System.Windows.Forms.Padding(0);
			this.front.Name = "front";
			this.front.Size = new System.Drawing.Size(440, 440);
			this.front.TabIndex = 0;
			// 
			// tabback
			// 
			this.tabback.Controls.Add(this.back);
			this.tabback.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabback.Location = new System.Drawing.Point(4, 22);
			this.tabback.Name = "tabback";
			this.tabback.Size = new System.Drawing.Size(440, 440);
			this.tabback.TabIndex = 1;
			this.tabback.Text = "Back";
			this.tabback.UseVisualStyleBackColor = true;
			// 
			// back
			// 
			this.back.Dock = System.Windows.Forms.DockStyle.Fill;
			this.back.Location = new System.Drawing.Point(0, 0);
			this.back.Margin = new System.Windows.Forms.Padding(0);
			this.back.Name = "back";
			this.back.Size = new System.Drawing.Size(440, 440);
			this.back.TabIndex = 0;
			// 
			// EditLineForm
			// 
			this.AcceptButton = this.apply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(468, 511);
			this.Controls.Add(this.tabs);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.apply);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditLineForm";
			this.Opacity = 0;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Edit line";
			this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.LinedefEditForm_HelpRequested);
			this.tabs.ResumeLayout(false);
			this.tabfront.ResumeLayout(false);
			this.tabback.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button apply;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tabfront;
		private System.Windows.Forms.TabPage tabback;
		private mxd.DukeBuilder.Controls.WallPropertiesControl front;
		private mxd.DukeBuilder.Controls.WallPropertiesControl back;
	}
}