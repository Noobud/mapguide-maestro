﻿namespace Maestro.Base.UI.Preferences
{
    partial class GeneralPreferencesCtrl
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
            if (disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralPreferencesCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.rdAjax = new System.Windows.Forms.RadioButton();
            this.rdFusion = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTemplatePath = new System.Windows.Forms.TextBox();
            this.btnBrowseTemplatePath = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkOutbound = new System.Windows.Forms.CheckBox();
            this.chkMessages = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBrowseFsPreview = new System.Windows.Forms.Button();
            this.txtFsPreview = new System.Windows.Forms.TextBox();
            this.btnBrowseMgCooker = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMgCooker = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbModifiedColor = new Maestro.Editors.Common.ColorComboBox();
            this.cmbOpenedColor = new Maestro.Editors.Common.ColorComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkValidateOnSave = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // rdAjax
            // 
            resources.ApplyResources(this.rdAjax, "rdAjax");
            this.rdAjax.Checked = true;
            this.rdAjax.Name = "rdAjax";
            this.rdAjax.TabStop = true;
            this.rdAjax.UseVisualStyleBackColor = true;
            // 
            // rdFusion
            // 
            resources.ApplyResources(this.rdFusion, "rdFusion");
            this.rdFusion.Name = "rdFusion";
            this.rdFusion.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtTemplatePath
            // 
            resources.ApplyResources(this.txtTemplatePath, "txtTemplatePath");
            this.txtTemplatePath.Name = "txtTemplatePath";
            this.txtTemplatePath.ReadOnly = true;
            // 
            // btnBrowseTemplatePath
            // 
            resources.ApplyResources(this.btnBrowseTemplatePath, "btnBrowseTemplatePath");
            this.btnBrowseTemplatePath.Name = "btnBrowseTemplatePath";
            this.btnBrowseTemplatePath.UseVisualStyleBackColor = true;
            this.btnBrowseTemplatePath.Click += new System.EventHandler(this.btnBrowseTemplatePath_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.chkOutbound);
            this.groupBox1.Controls.Add(this.chkMessages);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkOutbound
            // 
            resources.ApplyResources(this.chkOutbound, "chkOutbound");
            this.chkOutbound.Name = "chkOutbound";
            this.chkOutbound.UseVisualStyleBackColor = true;
            // 
            // chkMessages
            // 
            resources.ApplyResources(this.chkMessages, "chkMessages");
            this.chkMessages.Name = "chkMessages";
            this.chkMessages.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.btnBrowseFsPreview);
            this.groupBox2.Controls.Add(this.txtFsPreview);
            this.groupBox2.Controls.Add(this.btnBrowseMgCooker);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtMgCooker);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // btnBrowseFsPreview
            // 
            resources.ApplyResources(this.btnBrowseFsPreview, "btnBrowseFsPreview");
            this.btnBrowseFsPreview.Name = "btnBrowseFsPreview";
            this.btnBrowseFsPreview.UseVisualStyleBackColor = true;
            this.btnBrowseFsPreview.Click += new System.EventHandler(this.btnBrowseFsPreview_Click);
            // 
            // txtFsPreview
            // 
            resources.ApplyResources(this.txtFsPreview, "txtFsPreview");
            this.txtFsPreview.Name = "txtFsPreview";
            this.txtFsPreview.ReadOnly = true;
            // 
            // btnBrowseMgCooker
            // 
            resources.ApplyResources(this.btnBrowseMgCooker, "btnBrowseMgCooker");
            this.btnBrowseMgCooker.Name = "btnBrowseMgCooker";
            this.btnBrowseMgCooker.UseVisualStyleBackColor = true;
            this.btnBrowseMgCooker.Click += new System.EventHandler(this.btnBrowseMgCooker_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtMgCooker
            // 
            resources.ApplyResources(this.txtMgCooker, "txtMgCooker");
            this.txtMgCooker.Name = "txtMgCooker";
            this.txtMgCooker.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.cmbModifiedColor);
            this.groupBox3.Controls.Add(this.cmbOpenedColor);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // cmbModifiedColor
            // 
            this.cmbModifiedColor.FormattingEnabled = true;
            resources.ApplyResources(this.cmbModifiedColor, "cmbModifiedColor");
            this.cmbModifiedColor.Name = "cmbModifiedColor";
            // 
            // cmbOpenedColor
            // 
            this.cmbOpenedColor.FormattingEnabled = true;
            resources.ApplyResources(this.cmbOpenedColor, "cmbOpenedColor");
            this.cmbOpenedColor.Name = "cmbOpenedColor";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkValidateOnSave);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // chkValidateOnSave
            // 
            resources.ApplyResources(this.chkValidateOnSave, "chkValidateOnSave");
            this.chkValidateOnSave.Name = "chkValidateOnSave";
            this.chkValidateOnSave.UseVisualStyleBackColor = true;
            // 
            // GeneralPreferencesCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBrowseTemplatePath);
            this.Controls.Add(this.txtTemplatePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rdFusion);
            this.Controls.Add(this.rdAjax);
            this.Controls.Add(this.label1);
            this.Name = "GeneralPreferencesCtrl";
            resources.ApplyResources(this, "$this");
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdAjax;
        private System.Windows.Forms.RadioButton rdFusion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTemplatePath;
        private System.Windows.Forms.Button btnBrowseTemplatePath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkOutbound;
        private System.Windows.Forms.CheckBox chkMessages;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowseFsPreview;
        private System.Windows.Forms.TextBox txtFsPreview;
        private System.Windows.Forms.Button btnBrowseMgCooker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMgCooker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private Maestro.Editors.Common.ColorComboBox cmbModifiedColor;
        private Maestro.Editors.Common.ColorComboBox cmbOpenedColor;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkValidateOnSave;
    }
}
