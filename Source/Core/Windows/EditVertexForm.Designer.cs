namespace mxd.DukeBuilder.Windows
{
	partial class EditVertexForm
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
			System.Windows.Forms.Label label6;
			this.groupposition = new System.Windows.Forms.GroupBox();
			this.posy = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.posx = new mxd.DukeBuilder.Controls.ButtonsNumericTextbox();
			this.cancel = new System.Windows.Forms.Button();
			this.apply = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			this.groupposition.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(159, 24);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(17, 14);
			label1.TabIndex = 23;
			label1.Text = "Y:";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(31, 24);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(17, 14);
			label6.TabIndex = 21;
			label6.Text = "X:";
			// 
			// groupposition
			// 
			this.groupposition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupposition.Controls.Add(this.posy);
			this.groupposition.Controls.Add(this.posx);
			this.groupposition.Controls.Add(label1);
			this.groupposition.Controls.Add(label6);
			this.groupposition.Location = new System.Drawing.Point(12, 12);
			this.groupposition.Name = "groupposition";
			this.groupposition.Size = new System.Drawing.Size(284, 56);
			this.groupposition.TabIndex = 0;
			this.groupposition.TabStop = false;
			this.groupposition.Text = " Position ";
			// 
			// positiony
			// 
			this.posy.AllowDecimal = false;
			this.posy.AllowNegative = true;
			this.posy.AllowRelative = true;
			this.posy.ButtonStep = 1;
			this.posy.Location = new System.Drawing.Point(183, 19);
			this.posy.Name = "posy";
			this.posy.Size = new System.Drawing.Size(90, 24);
			this.posy.StepValues = null;
			this.posy.TabIndex = 25;
			// 
			// positionx
			// 
			this.posx.AllowDecimal = false;
			this.posx.AllowNegative = true;
			this.posx.AllowRelative = true;
			this.posx.ButtonStep = 1;
			this.posx.Location = new System.Drawing.Point(54, 19);
			this.posx.Name = "posx";
			this.posx.Size = new System.Drawing.Size(90, 24);
			this.posx.StepValues = null;
			this.posx.TabIndex = 24;
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancel.Location = new System.Drawing.Point(186, 74);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(110, 25);
			this.cancel.TabIndex = 2;
			this.cancel.Text = "Cancel";
			this.cancel.UseVisualStyleBackColor = true;
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// apply
			// 
			this.apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.apply.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.apply.Location = new System.Drawing.Point(70, 74);
			this.apply.Name = "apply";
			this.apply.Size = new System.Drawing.Size(110, 25);
			this.apply.TabIndex = 1;
			this.apply.Text = "OK";
			this.apply.UseVisualStyleBackColor = true;
			this.apply.Click += new System.EventHandler(this.apply_Click);
			// 
			// EditVertexForm
			// 
			this.AcceptButton = this.apply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(308, 105);
			this.Controls.Add(this.groupposition);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.apply);
			this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditVertexForm";
			this.Opacity = 0;
			this.Text = "Edit vertex";
			this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.VertexEditForm_HelpRequested);
			this.groupposition.ResumeLayout(false);
			this.groupposition.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button apply;
		private System.Windows.Forms.GroupBox groupposition;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox posy;
		private mxd.DukeBuilder.Controls.ButtonsNumericTextbox posx;
	}
}