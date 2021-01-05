namespace synyi.hdr.suite
{
    partial class frmMain
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
            this.btnExportHDRExcel = new System.Windows.Forms.Button();
            this.btnLoadSchemaAndTables = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnLoadHdr_v105 = new System.Windows.Forms.Button();
            this.btnLoadHdr_v106 = new System.Windows.Forms.Button();
            this.txtHDRExcelPath = new System.Windows.Forms.TextBox();
            this.btnLocationHDRExcel = new System.Windows.Forms.Button();
            this.grid = new System.Windows.Forms.DataGridView();
            this.btnLoad_SD = new System.Windows.Forms.Button();
            this.btnLoadHDR_SD = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSplitScripts = new System.Windows.Forms.Button();
            this.btnHDrMDMMaintain = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExportHDRExcel
            // 
            this.btnExportHDRExcel.Location = new System.Drawing.Point(18, 70);
            this.btnExportHDRExcel.Name = "btnExportHDRExcel";
            this.btnExportHDRExcel.Size = new System.Drawing.Size(133, 42);
            this.btnExportHDRExcel.TabIndex = 0;
            this.btnExportHDRExcel.Text = "导出字典";
            this.btnExportHDRExcel.UseVisualStyleBackColor = true;
            this.btnExportHDRExcel.Click += new System.EventHandler(this.btnExportHDRExcel_Click);
            // 
            // btnLoadSchemaAndTables
            // 
            this.btnLoadSchemaAndTables.Location = new System.Drawing.Point(18, 20);
            this.btnLoadSchemaAndTables.Name = "btnLoadSchemaAndTables";
            this.btnLoadSchemaAndTables.Size = new System.Drawing.Size(133, 42);
            this.btnLoadSchemaAndTables.TabIndex = 1;
            this.btnLoadSchemaAndTables.Text = "加载配置";
            this.btnLoadSchemaAndTables.UseVisualStyleBackColor = true;
            this.btnLoadSchemaAndTables.Click += new System.EventHandler(this.btnLoadSchemaAndTables_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(5, 464);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(767, 210);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // btnLoadHdr_v105
            // 
            this.btnLoadHdr_v105.Location = new System.Drawing.Point(17, 75);
            this.btnLoadHdr_v105.Name = "btnLoadHdr_v105";
            this.btnLoadHdr_v105.Size = new System.Drawing.Size(134, 49);
            this.btnLoadHdr_v105.TabIndex = 7;
            this.btnLoadHdr_v105.Text = "读取HDR转换成hdr_columns";
            this.btnLoadHdr_v105.UseVisualStyleBackColor = true;
            this.btnLoadHdr_v105.Click += new System.EventHandler(this.btnLoadHdr_To_hdr_columns_Click);
            // 
            // btnLoadHdr_v106
            // 
            this.btnLoadHdr_v106.Location = new System.Drawing.Point(17, 20);
            this.btnLoadHdr_v106.Name = "btnLoadHdr_v106";
            this.btnLoadHdr_v106.Size = new System.Drawing.Size(134, 49);
            this.btnLoadHdr_v106.TabIndex = 10;
            this.btnLoadHdr_v106.Text = "读取最新HDR";
            this.btnLoadHdr_v106.UseVisualStyleBackColor = true;
            this.btnLoadHdr_v106.Click += new System.EventHandler(this.btnLoadHdr_Click);
            // 
            // txtHDRExcelPath
            // 
            this.txtHDRExcelPath.Location = new System.Drawing.Point(84, 9);
            this.txtHDRExcelPath.Name = "txtHDRExcelPath";
            this.txtHDRExcelPath.Size = new System.Drawing.Size(688, 21);
            this.txtHDRExcelPath.TabIndex = 15;
            // 
            // btnLocationHDRExcel
            // 
            this.btnLocationHDRExcel.Location = new System.Drawing.Point(796, 3);
            this.btnLocationHDRExcel.Name = "btnLocationHDRExcel";
            this.btnLocationHDRExcel.Size = new System.Drawing.Size(124, 36);
            this.btnLocationHDRExcel.TabIndex = 12;
            this.btnLocationHDRExcel.Text = "浏览";
            this.btnLocationHDRExcel.UseVisualStyleBackColor = true;
            this.btnLocationHDRExcel.Click += new System.EventHandler(this.btnLocationHDRExcel_Click);
            // 
            // grid
            // 
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Location = new System.Drawing.Point(5, 36);
            this.grid.Name = "grid";
            this.grid.RowTemplate.Height = 23;
            this.grid.Size = new System.Drawing.Size(767, 422);
            this.grid.TabIndex = 13;
            // 
            // btnLoad_SD
            // 
            this.btnLoad_SD.Location = new System.Drawing.Point(17, 20);
            this.btnLoad_SD.Name = "btnLoad_SD";
            this.btnLoad_SD.Size = new System.Drawing.Size(134, 49);
            this.btnLoad_SD.TabIndex = 14;
            this.btnLoad_SD.Text = "读取最新SD生成hdr_columns";
            this.btnLoad_SD.UseVisualStyleBackColor = true;
            this.btnLoad_SD.Click += new System.EventHandler(this.btnLoad_SD_Click);
            // 
            // btnLoadHDR_SD
            // 
            this.btnLoadHDR_SD.Location = new System.Drawing.Point(18, 74);
            this.btnLoadHDR_SD.Name = "btnLoadHDR_SD";
            this.btnLoadHDR_SD.Size = new System.Drawing.Size(133, 49);
            this.btnLoadHDR_SD.TabIndex = 15;
            this.btnLoadHDR_SD.Text = "读取最新SD";
            this.btnLoadHDR_SD.UseVisualStyleBackColor = true;
            this.btnLoadHDR_SD.Click += new System.EventHandler(this.btnLoadHDR_SD_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoadHDR_SD);
            this.groupBox1.Controls.Add(this.btnLoadHdr_v106);
            this.groupBox1.Location = new System.Drawing.Point(778, 329);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(163, 129);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "读取HDR生成Excel校对文档";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSplitScripts);
            this.groupBox2.Location = new System.Drawing.Point(778, 480);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(163, 83);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "脚本切割";
            // 
            // btnSplitScripts
            // 
            this.btnSplitScripts.Location = new System.Drawing.Point(18, 20);
            this.btnSplitScripts.Name = "btnSplitScripts";
            this.btnSplitScripts.Size = new System.Drawing.Size(133, 49);
            this.btnSplitScripts.TabIndex = 0;
            this.btnSplitScripts.Text = "切割HDR脚本";
            this.btnSplitScripts.UseVisualStyleBackColor = true;
            this.btnSplitScripts.Click += new System.EventHandler(this.btnSplitScripts_Click);
            // 
            // btnHDrMDMMaintain
            // 
            this.btnHDrMDMMaintain.Location = new System.Drawing.Point(17, 33);
            this.btnHDrMDMMaintain.Name = "btnHDrMDMMaintain";
            this.btnHDrMDMMaintain.Size = new System.Drawing.Size(134, 49);
            this.btnHDrMDMMaintain.TabIndex = 18;
            this.btnHDrMDMMaintain.Text = "MDM维护";
            this.btnHDrMDMMaintain.UseVisualStyleBackColor = true;
            this.btnHDrMDMMaintain.Click += new System.EventHandler(this.btnHDrMDMMaintain_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnLoadHdr_v105);
            this.groupBox3.Controls.Add(this.btnLoad_SD);
            this.groupBox3.Location = new System.Drawing.Point(778, 184);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(163, 139);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "读取HDR的Excel说明";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnHDrMDMMaintain);
            this.groupBox4.Location = new System.Drawing.Point(778, 569);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(163, 100);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "HDR的MDM域维护";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnExportHDRExcel);
            this.groupBox5.Controls.Add(this.btnLoadSchemaAndTables);
            this.groupBox5.Location = new System.Drawing.Point(778, 45);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(163, 121);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "读取HDR的Excel说明";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 22;
            this.label1.Text = "文件路径：";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 681);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnLocationHDRExcel);
            this.Controls.Add(this.txtHDRExcelPath);
            this.Controls.Add(this.richTextBox1);
            this.Name = "frmMain";
            this.Text = "HDR模型文档工具";
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExportHDRExcel;
        private System.Windows.Forms.Button btnLoadSchemaAndTables;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnLoadHdr_v105;
        private System.Windows.Forms.Button btnLoadHdr_v106;
        private System.Windows.Forms.TextBox txtHDRExcelPath;
        private System.Windows.Forms.Button btnLocationHDRExcel;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Button btnLoad_SD;
        private System.Windows.Forms.Button btnLoadHDR_SD;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSplitScripts;
        private System.Windows.Forms.Button btnHDrMDMMaintain;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label1;
    }
}

