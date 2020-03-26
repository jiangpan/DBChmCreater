using DBChmCreater.Chm.Common;
using DBChmCreater.Common;
using DBChmCreater.Core;
using DBChmCreater.DB;
using Synyi.DBChmCreater.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace DBChmCreater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 属性
        private string defaultHtml = "数据库表目录.html";
        private IniFileHelp ini = new IniFileHelp(".//set.ini");
        private IDAL dal;
        private ChmHelp chmhlp;
        private IList<IList<string>> hdrtables;

        #endregion

        #region 构造函数
        public MainWindow()
        {
            InitializeComponent();
            txtTitle.Text = ini.GetString("Set", "title", "数据库帮助文档");
            cbbDbtype.SelectedIndex = ini.GetInt32("Set", "index", 0);

            ckbTables.SelectAll();

            ckbTables.UnselectAll();
        }

        #endregion

        #region click事件处理
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button textBox = e.Source as Button;

            //MessageBox.Show($"Width:{textBox.Width} Height:{textBox.Height} MinWidth:{textBox.MinWidth} MinHeight:{textBox.MinHeight} MaxWidth:{textBox.MaxWidth} MaxHeight:{textBox.MaxHeight}");

            if (textBox.Tag != null)
            {
                string tag = textBox.Tag.ToString();
                switch (tag)
                {
                    //tablesSelectAll
                    case "tablesSelectAll":
                        ckbTables.SelectAll();
                        break;
                    case "tablesUnSelectAll":
                        ckbTables.UnselectAll();
                        break;
                    case "tablesDefaultTable":
                        tablesDefaultTable_Click(sender, e);
                        break;
                    case "dataSelectAll":
                        ckbData.SelectAll();
                        break;
                    case "dataUnSelectAll":
                        ckbData.UnselectAll();
                        break;
                    case "dataDefaultTableData":
                        dataDefaultTableData_Click(sender, e);
                        break;
                    case "btnGenHtml":
                        btnGenHtml_Click(sender, e);
                        break;
                    case "btnGenChmProject":
                        btnGenChmProject_Click(sender, e);
                        break;
                    case "btnExport":
                        btnExport_Click(sender, e);
                        break;
                    case "btnConn":
                        btnConn_Click(sender, e);
                        break;
                    case "btnHelp":
                        btnHelp_Click(sender, e);
                        break;
                    default:
                        break;
                }
                if (true)
                {

                }

            }
        }

        /// <summary>
        /// 数据库中默认的表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tablesDefaultTable_Click(object sender, RoutedEventArgs e)
        {
            var result = ini.GetString("Set", "hdrtables", string.Empty);
            if (hdrtables == null)
            {
                hdrtables = JsonConvert.DeserializeObject<IList<IList<string>>>(result);
            }
            if (hdrtables == null)
            {
                MessageBox.Show("标准表集合获取失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            IList<string> selected = new List<string>();

            for (int i = 0; i < this.ckbTables.Items.Count; i++)
            {
                var tablefullname = this.ckbTables.Items[i].ToString();

                var hdrtableitems = hdrtables.FirstOrDefault(p => tablefullname == $"{p[0]}.{p[1]}");

                if (hdrtableitems != null)
                {
                    this.ckbTables.SelectedItems.Add(this.ckbTables.Items.GetItemAt(i));
                    selected.Add(tablefullname);
                }
            }
            if ( !(selected.Count == hdrtables.Count))
            {
                var logpath = System.IO.Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.log");

                var difftables = hdrtables.Where(p => !selected.Contains($"{p[0]}.{p[1]}")).Select(p=> $"{p[0]}.{p[1]}");

                File.WriteAllText(logpath, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}日志:标准库中{string.Join(";",difftables)}");
                MessageBox.Show("当前库中表不完整！", "提示", MessageBoxButton.OK,  MessageBoxImage.Information);
            }
           

        }

        /// <summary>
        /// 默认导出的数据的表 mdm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataDefaultTableData_Click(object sender, RoutedEventArgs e)
        {
            var result = ini.GetString("Set", "hdrtables", string.Empty);
            if (hdrtables == null)
            {
                hdrtables = JsonConvert.DeserializeObject<IList<IList<string>>>(result);
            }

            for (int i = 0; i < this.ckbData.Items.Count; i++)
            {
                var tablefullname = this.ckbData.Items[i].ToString();

                var hdrtableitems = hdrtables.FirstOrDefault(p => tablefullname == $"{p[0]}.{p[1]}");

                if (hdrtableitems != null && (hdrtableitems[0] == "mdm" || hdrtableitems[1] == "hdr_versions" || hdrtableitems[1] == "hdr_versions_modify_rec"))
                {
                    this.ckbData.SelectedItems.Add(this.ckbData.Items.GetItemAt(i));
                }
            }
        }

        private void txtConn_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.groupbox1.IsEnabled = false;
        }

        private void cbbDbtype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtConn.Text = ini.GetString("Set", "index_" + cbbDbtype.SelectedIndex, "");
        }

        private void btnConn_Click(object sender, RoutedEventArgs e)
        {
            if (txtConn.Text.Length == 0)
            {
                lblMessage.Content = "连接字符串不能为空";
                return;
            }
            try
            {
                dal = DALFacotry.Create(cbbDbtype.Text, txtConn.Text);
                if (dal == null)
                {
                    lblMessage.Content = "暂时不支持该数据库 敬请期待";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Content = $"数据库异常 请确认！{ex.Message}";
                return;
            }

            lblMessage.Content = "成功连接数据库";
            //成功连接之后将配置保存
            ini.WriteValue("Set", "index", cbbDbtype.SelectedIndex);
            ini.WriteValue("Set", "index_" + cbbDbtype.SelectedIndex, txtConn.Text);
            //加载表信息
            ckbTables.Items.Clear();
            ckbData.Items.Clear();
            var dt = dal.GetTables();
            if (dt.Count == 0)
            {
                lblMessage.Content = "查询表信息异常，请选择正确的数据库!";
                return;
            }

            ObservableCollection<string> tablesBind = new ObservableCollection<string>();

            foreach (var dr in dt)
            {
                tablesBind.Add($"{dr.Domain}.{dr.TableName}");
            }

            ckbTables.ItemsSource = tablesBind;
            ckbData.ItemsSource = tablesBind;

            groupbox1.IsEnabled = true;
        }

        private void btnGenHtml_Click(object sender, RoutedEventArgs e)
        {
            ExportHtml();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            //输入验证
            if (ckbData.SelectedItems.Count == 0 && ckbTables.SelectedItems.Count == 0)
            {
                lblMessage.Content = "请至少选择一张表";
                return;
            }
            if (txtTitle.Text.Length == 0)
            {
                lblMessage.Content = "请输入CHM文件标题";
                return;
            }
            //保存配置
            ini.WriteValue("Set", "title", txtTitle.Text);
            //使用后台线程去导出

            Task.Factory.StartNew(
                (x) =>
                {
                    BuildChmAndExport(x);
                },
                chmhlp);

            this.EnableControl(false);
        }

        private void btnGenChmProject_Click(object sender, RoutedEventArgs e)
        {
            ExportChmProject(chmhlp);
        }

        #endregion

        #region 帮助
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            ConnWindow cw = new ConnWindow(cbbDbtype.SelectedIndex);
            cw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cw.ShowDialog();
            if (cw.chosed)
            {
                //
                //cbbDbtype.SelectedIndex = fc.index;
                //txtConn.Text = fc.conn;
                cbbDbtype.SelectedIndex = cw.index;
                txtConn.Text = cw.conn;
                //

            }
        }
        #endregion

        #region 导出逻辑
        private void ExportHtml()
        {
            string tableDataDirName = "表数据示例";
            string tableStructureDirName = "表结构说明";

            Directory.CreateDirectory(".//tmp");

            //定义目录DataTable 结构
            Synyi.DBChmCreater.Entity.DataTableCollection dataTableStructures = new Synyi.DBChmCreater.Entity.DataTableCollection("<b>数据库表目录</b>");

            //将选中项的总数 设置为进度条最大值 +1项是表目录文件
            Dispatcher.Invoke(new Action(() =>
            {
                tpbExport.Value = 0;
                tpbExport.Maximum = ckbData.SelectedItems.Count + ckbTables.SelectedItems.Count + 1;// ckbData.CheckedItems.Count + ckbTables.CheckedItems.Count + 1;
            }));

            //获取需要导出的表结构 选中项
            List<string> selectTabStructList = new List<string>();
            foreach (var item in ckbTables.SelectedItems) // CheckedItems)
            {
                selectTabStructList.Add(item.ToString());
            }

            #region 导出表结构 数据字典部分 导出为html
            if (selectTabStructList.Count > 0)
            {
                lblMessage.Content = "准备表结构文件...";
                //得到选中的表结构的字段信息
                var lstDt = dal.GetTableStruct(selectTabStructList);
                var pathTables = $"./tmp/{tableStructureDirName}";
                Directory.CreateDirectory(pathTables);
                var tables = dal.GetTables();
                var allSchemas = tables.Select(p => p.Domain).Distinct();
                foreach (var item in allSchemas)
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(pathTables,item));
                }
                var tableIndex = 1;
                foreach (var dt in lstDt)
                {
                    //得到表描述
                    var array = dt.TableName.Split('.');
                    var domain = array[0];
                    var tabname = array[1];
                    var drs = tables.Where(p => p.TableName == tabname).Where(p => p.Domain == domain).FirstOrDefault();// Select("表名='" + tabname + "' and 域='" + domain + "'");
                    var desp = string.Empty;
                    if (drs != null)
                    {
                        desp = drs.TableDescription;
                    }
                    //创建表字段信息的html
                    HtmlHelp.CreateHtml(dt, true, System.IO.Path.Combine(pathTables, dt[0].tableschema, $"{dt.TableName}.html"), true, desp);
                    //构建表目录
                    dataTableStructures.Add(new DataTableItem { TableNo = tableIndex++, Domain = drs.Domain, TableName = $"<a href=\"{tableStructureDirName}\\{drs.Domain}\\{ dt.TableName}.html\">{ dt.TableName.Split('.').GetValue(1)}</a>", TableDescription = desp });
                    //改变进度
                    Dispatcher.Invoke(new Action(() => { tpbExport.Value++; }));

                }
                //导出表目录
                HtmlHelp.CreateHtml(dataTableStructures, false, "./tmp/" + defaultHtml, false); //默认页面
                Dispatcher.Invoke(new Action(() => { tpbExport.Value++; }));
            }
            #endregion

            #region 导出表数据  表中的数据示例 导出为html
            //传递需要导出数据的table选中项  得到数据内容
            selectTabStructList.Clear();
            foreach (var item in ckbData.SelectedItems)  //CheckedItems)
            {
                selectTabStructList.Add(item.ToString());
            }
            if (selectTabStructList.Count > 0)
            {
                Dispatcher.Invoke(new Action(() => { lblMessage.Content = "正在生成表数据数据..."; }));

                //读取ini
                int result = ini.GetInt32 ("Set", "maxrow", -1);

                var lstDt = dal.GetTableData(selectTabStructList, result);
                //创建常用数据的html
                var pathTables = $"./tmp/{tableDataDirName}";
                Directory.CreateDirectory(pathTables);
                var selecttabdataschemas = selectTabStructList.Select(p => p.Split('.')[0]).Distinct();
                foreach (var item in selecttabdataschemas)
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(pathTables, item));
                }

                foreach (var dt in lstDt)
                {
                    HtmlHelp.CreateHtml2(dt, true, System.IO.Path.Combine(System.IO.Path.Combine(pathTables, dt.TableName.Split('.')[0]), dt.TableName + ".html"));
                    Dispatcher.Invoke(new Action(() => { tpbExport.Value++; }));
                }
            }

            Dispatcher.Invoke(new Action(() => { lblMessage.Content = "成功生成HTML..."; }));
            #endregion
        }

        private void ExportChmProject(ChmHelp chmHelp)
        {
            Dispatcher.Invoke(new Action(() => { lblMessage.Content = "正在生成CHM工程..."; }));
            try
            {
                //编译CHM文档
                if (chmHelp == null)
                {
                    chmhlp = new ChmHelp();
                }
                chmhlp.DefaultPage = defaultHtml;
                chmhlp.Title = txtTitle.Text;
                //chmhlp.ChmFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), chmhlp.Title + ".chm");
                chmhlp.ChmFileName = System.IO.Path.Combine("..", $"{chmhlp.Title}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{".chm"}");
                chmhlp.RootPath = @"./tmp";
                chmhlp.logHandle += chmhelp_logHandle;
                chmhlp.BuildProject();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action<bool>(this.EnableControl), true);
                Dispatcher.Invoke(new Action(() => { lblMessage.Content = ex.Message; }));
            }
            finally
            {
                Dispatcher.Invoke(new Action(() => { lblMessage.Content = "成功生成CHM工程..."; }));
            }

        }

        public void BuildChmAndExport(object obj)
        {
            #region 编译CHM文件
            Dispatcher.Invoke(new Action(() => { lblMessage.Content = "正在编译CHM文件..."; }));
            if (obj == null)
            {
                Dispatcher.Invoke(new Action(() => { lblMessage.Content = "编译CHM文件失败..."; }));
                Dispatcher.Invoke(new Action<bool>(this.EnableControl), true);
                return;
            }
            try
            {
                ChmHelp chmhlp = (obj as ChmHelp);
                chmhlp.Compile();
                Dispatcher.Invoke(new Action(() => { lblMessage.Content = "导出完毕 文件存储在:" + chmhlp.ChmFileName; }));

                Directory.Delete("./tmp", true);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() => { lblMessage.Content = "导出发生异常"; }));

                MessageBox.Show(ex.Message);
            }
            finally
            {
                Dispatcher.Invoke(new Action<bool>(this.EnableControl), true);
            }
            #endregion
        }

        private void chmhelp_logHandle(string arg1, string arg2)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";

            string path = System.IO.Path.Combine(AppContext.BaseDirectory, fileName);


            File.AppendAllText(path, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [{arg1}] {arg2} " + Environment.NewLine);

        }

        private void EnableControl(bool enbale)
        {
            this.sp_dbconn.IsEnabled = enbale;
            this.groupbox1.IsEnabled = enbale;
            this.sp_bottom1.IsEnabled = enbale;
            //this.EnableControl(enbale);

        }

        #endregion
    }
}
