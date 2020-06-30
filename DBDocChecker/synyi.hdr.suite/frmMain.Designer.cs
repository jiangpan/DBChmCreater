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
            this.btnLoadJson = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExportHDRExcel
            // 
            this.btnExportHDRExcel.Location = new System.Drawing.Point(799, 83);
            this.btnExportHDRExcel.Name = "btnExportHDRExcel";
            this.btnExportHDRExcel.Size = new System.Drawing.Size(85, 34);
            this.btnExportHDRExcel.TabIndex = 0;
            this.btnExportHDRExcel.Text = "导出字典";
            this.btnExportHDRExcel.UseVisualStyleBackColor = true;
            this.btnExportHDRExcel.Click += new System.EventHandler(this.btnExportHDRExcel_Click);
            // 
            // btnLoadSchemaAndTables
            // 
            this.btnLoadSchemaAndTables.Location = new System.Drawing.Point(799, 43);
            this.btnLoadSchemaAndTables.Name = "btnLoadSchemaAndTables";
            this.btnLoadSchemaAndTables.Size = new System.Drawing.Size(85, 34);
            this.btnLoadSchemaAndTables.TabIndex = 1;
            this.btnLoadSchemaAndTables.Text = "1.加载配置";
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
            this.btnLoadHdr_v105.Location = new System.Drawing.Point(799, 218);
            this.btnLoadHdr_v105.Name = "btnLoadHdr_v105";
            this.btnLoadHdr_v105.Size = new System.Drawing.Size(103, 49);
            this.btnLoadHdr_v105.TabIndex = 7;
            this.btnLoadHdr_v105.Text = "读取HDR转换成hdr_columns";
            this.btnLoadHdr_v105.UseVisualStyleBackColor = true;
            this.btnLoadHdr_v105.Click += new System.EventHandler(this.btnLoadHdr_To_hdr_columns_Click);
            // 
            // btnLoadJson
            // 
            this.btnLoadJson.Location = new System.Drawing.Point(795, 123);
            this.btnLoadJson.Name = "btnLoadJson";
            this.btnLoadJson.Size = new System.Drawing.Size(85, 34);
            this.btnLoadJson.TabIndex = 8;
            this.btnLoadJson.Text = "加载JSON";
            this.btnLoadJson.UseVisualStyleBackColor = true;
            this.btnLoadJson.Click += new System.EventHandler(this.btnLoadJson_Click);
            // 
            // btnLoadHdr_v106
            // 
            this.btnLoadHdr_v106.Location = new System.Drawing.Point(18, 21);
            this.btnLoadHdr_v106.Name = "btnLoadHdr_v106";
            this.btnLoadHdr_v106.Size = new System.Drawing.Size(85, 34);
            this.btnLoadHdr_v106.TabIndex = 10;
            this.btnLoadHdr_v106.Text = "读取最新HDR";
            this.btnLoadHdr_v106.UseVisualStyleBackColor = true;
            this.btnLoadHdr_v106.Click += new System.EventHandler(this.btnLoadHdr_Click);
            // 
            // txtHDRExcelPath
            // 
            this.txtHDRExcelPath.Location = new System.Drawing.Point(12, 9);
            this.txtHDRExcelPath.Name = "txtHDRExcelPath";
            this.txtHDRExcelPath.Size = new System.Drawing.Size(712, 21);
            this.txtHDRExcelPath.TabIndex = 11;
            // 
            // btnLocationHDRExcel
            // 
            this.btnLocationHDRExcel.Location = new System.Drawing.Point(778, 1);
            this.btnLocationHDRExcel.Name = "btnLocationHDRExcel";
            this.btnLocationHDRExcel.Size = new System.Drawing.Size(124, 38);
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
            this.btnLoad_SD.Location = new System.Drawing.Point(799, 163);
            this.btnLoad_SD.Name = "btnLoad_SD";
            this.btnLoad_SD.Size = new System.Drawing.Size(103, 49);
            this.btnLoad_SD.TabIndex = 14;
            this.btnLoad_SD.Text = "读取最新SD生成hdr_columns";
            this.btnLoad_SD.UseVisualStyleBackColor = true;
            this.btnLoad_SD.Click += new System.EventHandler(this.btnLoad_SD_Click);
            // 
            // btnLoadHDR_SD
            // 
            this.btnLoadHDR_SD.Location = new System.Drawing.Point(18, 61);
            this.btnLoadHDR_SD.Name = "btnLoadHDR_SD";
            this.btnLoadHDR_SD.Size = new System.Drawing.Size(85, 34);
            this.btnLoadHDR_SD.TabIndex = 15;
            this.btnLoadHDR_SD.Text = "读取最新SD";
            this.btnLoadHDR_SD.UseVisualStyleBackColor = true;
            this.btnLoadHDR_SD.Click += new System.EventHandler(this.btnLoadHDR_SD_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoadHDR_SD);
            this.groupBox1.Controls.Add(this.btnLoadHdr_v106);
            this.groupBox1.Location = new System.Drawing.Point(797, 273);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(126, 103);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文档校对";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSplitScripts);
            this.groupBox2.Location = new System.Drawing.Point(797, 391);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(128, 67);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "脚本切割";
            // 
            // btnSplitScripts
            // 
            this.btnSplitScripts.Location = new System.Drawing.Point(18, 20);
            this.btnSplitScripts.Name = "btnSplitScripts";
            this.btnSplitScripts.Size = new System.Drawing.Size(85, 33);
            this.btnSplitScripts.TabIndex = 0;
            this.btnSplitScripts.Text = "切割HDR脚本";
            this.btnSplitScripts.UseVisualStyleBackColor = true;
            this.btnSplitScripts.Click += new System.EventHandler(this.btnSplitScripts_Click);
            // 
            // btnHDrMDMMaintain
            // 
            this.btnHDrMDMMaintain.Location = new System.Drawing.Point(799, 509);
            this.btnHDrMDMMaintain.Name = "btnHDrMDMMaintain";
            this.btnHDrMDMMaintain.Size = new System.Drawing.Size(75, 23);
            this.btnHDrMDMMaintain.TabIndex = 18;
            this.btnHDrMDMMaintain.Text = "MDM维护";
            this.btnHDrMDMMaintain.UseVisualStyleBackColor = true;
            this.btnHDrMDMMaintain.Click += new System.EventHandler(this.btnHDrMDMMaintain_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 681);
            this.Controls.Add(this.btnHDrMDMMaintain);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLoad_SD);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnLocationHDRExcel);
            this.Controls.Add(this.txtHDRExcelPath);
            this.Controls.Add(this.btnLoadJson);
            this.Controls.Add(this.btnLoadHdr_v105);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnLoadSchemaAndTables);
            this.Controls.Add(this.btnExportHDRExcel);
            this.Name = "frmMain";
            this.Text = "HDR模型文档工具";
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExportHDRExcel;
        private System.Windows.Forms.Button btnLoadSchemaAndTables;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnLoadHdr_v105;
        private System.Windows.Forms.Button btnLoadJson;
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
    }
}

