using synyi.hdr.suite.mdm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace synyi.hdr.suite
{
    public partial class frmMdmMgr : Form
    {
        public frmMdmMgr()
        {
            InitializeComponent();
        }


        private void btnBuildMdmCodeSystem_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(this.txtCodeSysExcelPath.Text))
            {
                return;
            }
            if (!File.Exists(this.txtCodeSysExcelPath.Text))
            {
                return;
            }
            string filePathWithName = this.txtCodeSysExcelPath.Text;

            MdmBizHelper helper = new MdmBizHelper();


            helper.BuildMdmCodeSystem(filePathWithName, "", "",this.txtVersion.Text);
        }



        private void btnBuildCodeSet_Click(object sender, EventArgs e)
        {
            string filePathWithName = "";
         
            if (!File.Exists(filePathWithName))
            {
                return;
            }

            MdmBizHelper helper = new MdmBizHelper();

            Action<string, string, string> act = helper.InsertCodeSet;



            AsyncCallback callback = new AsyncCallback((x) =>
            {
            });

            var res = act.BeginInvoke(filePathWithName, "", "", callback, "object vlaue");

            act.EndInvoke(res);
            MessageBox.Show("完成");
        }


        private void btnExportMdmCodesysCodeSet_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(this.txtCodeSysExcelPath .Text))
            {
                return;
            }
            if (!File.Exists(this.txtCodeSysExcelPath.Text))
            {
                return;
            }
            string filePathWithName = this.txtCodeSysExcelPath.Text;
            MdmBizHelper helper = new MdmBizHelper();

            helper.ExportCodeSysCodeSet(filePathWithName, "代码系统", "hdr");
            MessageBox.Show("完成");
        }



        #region 导入excel至hdr的mdm中
        private void btnImportToHdr_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCodeSysExcelPath.Text) )
            {
                return;
            }
            if (!File.Exists(this.txtCodeSysExcelPath.Text) )
            {
                return;
            }
            string codesyspath = this.txtCodeSysExcelPath.Text;
            

            MdmBizHelper helper = new MdmBizHelper();

            Action<string, string, bool, string> act = helper.ImportCodeSysCodeSetToExcel;

            AsyncCallback callback = new AsyncCallback((x) =>
            {
            });

            var res = act.BeginInvoke(codesyspath, "", true, this.txtVersion.Text, callback, "object vlaue");

            act.EndInvoke(res);
            MessageBox.Show("完成");
        }

        #endregion


        #region 浏览定位文件
        private void btnBrowseFileCodeSys_Click(object sender, EventArgs e)
        {
            var ret = Browse_file();
            this.txtCodeSysExcelPath.Text = ret;
        }


        public string Browse_file()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Reset();
                //openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "Excel files (*.xls or *.xlsx)|*.xls;*.xlsx";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
                else
                {
                    return null;
                }
            }

        }
        
        #endregion
    }
}
