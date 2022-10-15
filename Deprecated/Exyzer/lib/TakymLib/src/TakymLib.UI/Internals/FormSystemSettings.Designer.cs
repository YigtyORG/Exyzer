
namespace TakymLib.UI.Internals
{
	partial class FormSystemSettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSystemSettings));
			this.labelDesc = new System.Windows.Forms.Label();
			this.btnReload = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.panel = new System.Windows.Forms.Panel();
			this.cboxDisallowExtensions = new System.Windows.Forms.CheckBox();
			this.cboxShowSplash = new System.Windows.Forms.CheckBox();
			this.btnApply = new System.Windows.Forms.Button();
			this.btnAccept = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelDesc
			// 
			resources.ApplyResources(this.labelDesc, "labelDesc");
			this.labelDesc.Name = "labelDesc";
			// 
			// btnReload
			// 
			resources.ApplyResources(this.btnReload, "btnReload");
			this.btnReload.Name = "btnReload";
			this.btnReload.UseVisualStyleBackColor = true;
			this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
			// 
			// btnReset
			// 
			resources.ApplyResources(this.btnReset, "btnReset");
			this.btnReset.Name = "btnReset";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// panel
			// 
			resources.ApplyResources(this.panel, "panel");
			this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel.Controls.Add(this.cboxDisallowExtensions);
			this.panel.Controls.Add(this.cboxShowSplash);
			this.panel.Name = "panel";
			// 
			// cboxDisallowExtensions
			// 
			resources.ApplyResources(this.cboxDisallowExtensions, "cboxDisallowExtensions");
			this.cboxDisallowExtensions.Name = "cboxDisallowExtensions";
			this.cboxDisallowExtensions.UseVisualStyleBackColor = true;
			// 
			// cboxShowSplash
			// 
			resources.ApplyResources(this.cboxShowSplash, "cboxShowSplash");
			this.cboxShowSplash.Name = "cboxShowSplash";
			this.cboxShowSplash.UseVisualStyleBackColor = true;
			// 
			// btnApply
			// 
			resources.ApplyResources(this.btnApply, "btnApply");
			this.btnApply.Name = "btnApply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// btnAccept
			// 
			resources.ApplyResources(this.btnAccept, "btnAccept");
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.UseVisualStyleBackColor = true;
			this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// FormSystemSettings
			// 
			this.AcceptButton = this.btnAccept;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAccept);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnReload);
			this.Controls.Add(this.labelDesc);
			this.MaximizeBox = false;
			this.Name = "FormSystemSettings";
			this.Load += new System.EventHandler(this.FormSystemSettings_Load);
			this.panel.ResumeLayout(false);
			this.panel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.Label labelDesc;
		private System.Windows.Forms.Button btnReload;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Button btnAccept;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox cboxDisallowExtensions;
		private System.Windows.Forms.CheckBox cboxShowSplash;
	}
}
