
namespace TakymLib.UI.Internals
{
	partial class FormSplash
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.lblCaption = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.SuspendLayout();
			//
			// lblCaption
			//
			this.lblCaption.AutoSize = true;
			this.lblCaption.Location = new System.Drawing.Point(8, 8);
			this.lblCaption.Name = "lblCaption";
			this.lblCaption.Size = new System.Drawing.Size(61, 15);
			this.lblCaption.TabIndex = 0;
			this.lblCaption.Text = "lblCaption";
			//
			// lblCopyright
			//
			this.lblCopyright.AutoSize = true;
			this.lblCopyright.Location = new System.Drawing.Point(8, 280);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new System.Drawing.Size(72, 15);
			this.lblCopyright.TabIndex = 1;
			this.lblCopyright.Text = "lblCopyright";
			//
			// FormSplash
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(400, 300);
			this.Controls.Add(this.lblCopyright);
			this.Controls.Add(this.lblCaption);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximumSize = new System.Drawing.Size(400, 300);
			this.MinimumSize = new System.Drawing.Size(400, 300);
			this.Name = "FormSplash";
			this.Text = "FormSplash";
			this.Load += new System.EventHandler(this.FormSplash_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormSplash_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormSplash_MouseMove);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.Label lblCaption;
		private System.Windows.Forms.Label lblCopyright;
	}
}
