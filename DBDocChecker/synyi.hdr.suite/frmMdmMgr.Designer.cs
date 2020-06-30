namespace synyi.hdr.suite
{
    partial class frmMdmMgr
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkClearMdmDataAndResetSeq = new System.Windows.Forms.CheckBox();
            this.btnImportToHdr = new System.Windows.Forms.Button();
            this.btnExportMdmCodesysCodeSet = new System.Windows.Forms.Button();
            this.btnBuildCodeSet = new System.Windows.Forms.Button();
            this.btnBuildMdmCodeSystem = new System.Windows.Forms.Button();
            this.txtCodeSysExcelPath = new System.Windows.Forms.TextBox();
            this.btnBrowseFileCodeSys = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtValueSetExcelPath = new System.Windows.Forms.TextBox();
            this.btnBrowseFileValueSet = new System.Windows.Forms.Button();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtVersion);
            this.groupBox3.Controls.Add(this.chkClearMdmDataAndResetSeq);
            this.groupBox3.Controls.Add(this.btnImportToHdr);
            this.groupBox3.Controls.Add(this.btnExportMdmCodesysCodeSet);
            this.groupBox3.Controls.Add(this.btnBuildCodeSet);
            this.groupBox3.Controls.Add(this.btnBuildMdmCodeSystem);
            this.groupBox3.Location = new System.Drawing.Point(12, 143);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(760, 210);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // chkClearMdmDataAndResetSeq
            // 
            this.chkClearMdmDataAndResetSeq.AutoSize = true;
            this.chkClearMdmDataAndResetSeq.Checked = true;
            this.chkClearMdmDataAndResetSeq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClearMdmDataAndResetSeq.Location = new System.Drawing.Point(148, 165);
            this.chkClearMdmDataAndResetSeq.Name = "chkClearMdmDataAndResetSeq";
            this.chkClearMdmDataAndResetSeq.Size = new System.Drawing.Size(180, 16);
            this.chkClearMdmDataAndResetSeq.TabIndex = 4;
            this.chkClearMdmDataAndResetSeq.Text = "导入前先清除数据，重置序列";
            this.chkClearMdmDataAndResetSeq.UseVisualStyleBackColor = true;
            // 
            // btnImportToHdr
            // 
            this.btnImportToHdr.Location = new System.Drawing.Point(9, 154);
            this.btnImportToHdr.Name = "btnImportToHdr";
            this.btnImportToHdr.Size = new System.Drawing.Size(126, 36);
            this.btnImportToHdr.TabIndex = 3;
            this.btnImportToHdr.Text = "导入至HDR中MDM域";
            this.btnImportToHdr.UseVisualStyleBackColor = true;
            this.btnImportToHdr.Click += new System.EventHandler(this.btnImportToHdr_Click);
            // 
            // btnExportMdmCodesysCodeSet
            // 
            this.btnExportMdmCodesysCodeSet.Location = new System.Drawing.Point(9, 102);
            this.btnExportMdmCodesysCodeSet.Name = "btnExportMdmCodesysCodeSet";
            this.btnExportMdmCodesysCodeSet.Size = new System.Drawing.Size(126, 36);
            this.btnExportMdmCodesysCodeSet.TabIndex = 2;
            this.btnExportMdmCodesysCodeSet.Text = "导出代码系统及值集";
            this.btnExportMdmCodesysCodeSet.UseVisualStyleBackColor = true;
            this.btnExportMdmCodesysCodeSet.Click += new System.EventHandler(this.btnExportMdmCodesysCodeSet_Click);
            // 
            // btnBuildCodeSet
            // 
            this.btnBuildCodeSet.Location = new System.Drawing.Point(9, 60);
            this.btnBuildCodeSet.Name = "btnBuildCodeSet";
            this.btnBuildCodeSet.Size = new System.Drawing.Size(126, 36);
            this.btnBuildCodeSet.TabIndex = 1;
            this.btnBuildCodeSet.Text = "构建代码集合";
            this.btnBuildCodeSet.UseVisualStyleBackColor = true;
            this.btnBuildCodeSet.Click += new System.EventHandler(this.btnBuildCodeSet_Click);
            // 
            // btnBuildMdmCodeSystem
            // 
            this.btnBuildMdmCodeSystem.Location = new System.Drawing.Point(9, 20);
            this.btnBuildMdmCodeSystem.Name = "btnBuildMdmCodeSystem";
            this.btnBuildMdmCodeSystem.Size = new System.Drawing.Size(126, 34);
            this.btnBuildMdmCodeSystem.TabIndex = 0;
            this.btnBuildMdmCodeSystem.Text = "构建代码系统";
            this.btnBuildMdmCodeSystem.UseVisualStyleBackColor = true;
            this.btnBuildMdmCodeSystem.Click += new System.EventHandler(this.btnBuildMdmCodeSystem_Click);
            // 
            // txtCodeSysExcelPath
            // 
            this.txtCodeSysExcelPath.Location = new System.Drawing.Point(110, 23);
            this.txtCodeSysExcelPath.Name = "txtCodeSysExcelPath";
            this.txtCodeSysExcelPath.Size = new System.Drawing.Size(507, 21);
            this.txtCodeSysExcelPath.TabIndex = 21;
            // 
            // btnBrowseFileCodeSys
            // 
            this.btnBrowseFileCodeSys.Location = new System.Drawing.Point(652, 22);
            this.btnBrowseFileCodeSys.Name = "btnBrowseFileCodeSys";
            this.btnBrowseFileCodeSys.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseFileCodeSys.TabIndex = 22;
            this.btnBrowseFileCodeSys.Text = "浏览";
            this.btnBrowseFileCodeSys.UseVisualStyleBackColor = true;
            this.btnBrowseFileCodeSys.Click += new System.EventHandler(this.btnBrowseFileCodeSys_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCodeSysExcelPath);
            this.groupBox1.Controls.Add(this.btnBrowseFileCodeSys);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 55);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "代码系统文件：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtValueSetExcelPath);
            this.groupBox2.Controls.Add(this.btnBrowseFileValueSet);
            this.groupBox2.Location = new System.Drawing.Point(12, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(760, 55);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "值集文件：";
            // 
            // txtValueSetExcelPath
            // 
            this.txtValueSetExcelPath.Location = new System.Drawing.Point(110, 22);
            this.txtValueSetExcelPath.Name = "txtValueSetExcelPath";
            this.txtValueSetExcelPath.Size = new System.Drawing.Size(507, 21);
            this.txtValueSetExcelPath.TabIndex = 21;
            // 
            // btnBrowseFileValueSet
            // 
            this.btnBrowseFileValueSet.Location = new System.Drawing.Point(652, 21);
            this.btnBrowseFileValueSet.Name = "btnBrowseFileValueSet";
            this.btnBrowseFileValueSet.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseFileValueSet.TabIndex = 22;
            this.btnBrowseFileValueSet.Text = "浏览";
            this.btnBrowseFileValueSet.UseVisualStyleBackColor = true;
            this.btnBrowseFileValueSet.Click += new System.EventHandler(this.btnBrowseFileValueSet_Click);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(338, 33);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(100, 21);
            this.txtVersion.TabIndex = 26;
            this.txtVersion.Text = "1.0.9";
            // 
            // frmMdmMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Name = "frmMdmMgr";
            this.Text = "HDR中MDM域维护";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkClearMdmDataAndResetSeq;
        private System.Windows.Forms.Button btnImportToHdr;
        private System.Windows.Forms.Button btnExportMdmCodesysCodeSet;
        private System.Windows.Forms.Button btnBuildCodeSet;
        private System.Windows.Forms.Button btnBuildMdmCodeSystem;
        private System.Windows.Forms.TextBox txtCodeSysExcelPath;
        private System.Windows.Forms.Button btnBrowseFileCodeSys;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtValueSetExcelPath;
        private System.Windows.Forms.Button btnBrowseFileValueSet;
        private System.Windows.Forms.TextBox txtVersion;
    }
}