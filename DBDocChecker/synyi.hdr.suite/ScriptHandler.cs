
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Dapper;
namespace synyi.hdr.suite
{
    public class ScriptHandler
    {

        public IList<ScriptSnippet> HandBackupScript(string fileName, string schema, IList<string> scripts)
        {
            List<int> locations = new List<int>();

            #region 整体扫描，计算出间隔点
            for (int i = 0; i < scripts.Count; i++)
            {
                var currentline = scripts[i];
                if (i == 0)
                {
                    locations.Add(i);
                }
                else if (i == scripts.Count - 1)
                {
                    locations.Add(i);
                }
                else if (currentline.StartsWith("-- TOC entry"))
                {
                    locations.Add(i);
                }
            }
            #endregion

            #region 裁切成一小块
            var sciptlst = scripts.ToList();
            IList<ScriptSnippet> scriptSnippets = new List<ScriptSnippet>();
            int startindex = 0;
            int endindex = 0;
            for (int j = 1; j < locations.Count; j++)
            {
                if (locations[j - 1] - 1 < 0)
                {
                    startindex = 0;
                }
                else
                {
                    startindex = locations[j - 1] - 1;
                }

                endindex = locations[j] - 1;

                var snippet = sciptlst.GetRange(startindex, endindex - startindex);
                var scriptsnippet = new ScriptSnippet { startline = locations[j - 1], endline = locations[j], filename = fileName, schemaname = schema, snippets = snippet };
                scriptSnippets.Add(scriptsnippet);
            }
            #endregion


            #region 生成对应的类型

            foreach (var item in scriptSnippets)
            {
                for (int i = 0; i < item.snippets.Count; i++)
                {
                    if (schema == "sd")
                    {
                        if (item.snippets[i].IndexOf("Name: COLUMN") > 0)
                        {

                        }
                    }

                    if (i >= 4)
                    {
                        break;
                    }
                    if (item.snippets[i] == "--")
                    {
                        continue;
                    }
                    else if (item.snippets[i].StartsWith("-- TOC entry "))
                    {
                        continue;
                    }
                    else if (item.snippets[i].StartsWith("-- Dependencies"))
                    {
                        continue;
                    }
                    else if (item.snippets[i].StartsWith("-- Name: "))
                    {
                        //取出对应的文件名

                        string ssfer = item.snippets[i];

                        var serer = ssfer.Trim('-', ' ');

                        var arrs = serer.Split(';', ':');

                        item.name = arrs[1];
                        item.tyype = arrs[3];

                        continue;
                    }
                    else
                    {
                        item.name = "UN";
                        item.tyype = "UN";//未知;
                    }
                }
            }
            #endregion

            return scriptSnippets;

        }


        public void ScriptSnippetToFile(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
            #region 创建目录
            var schemaPath = Path.Combine(basepath, schema);

            if (!Directory.Exists(schemaPath))
            {
                Directory.CreateDirectory(schemaPath);
            }
            #endregion

            #region 按照类sing来处理
            var groupsnippets = snippets.Select(p => p.tyype).Distinct();

            foreach (string item in groupsnippets)
            {

                var snippetsbyKind = snippets.Where(p => p.tyype == item).ToList();
                switch (item)
                {
                    case " SEQUENCE":
                        ProcessSequence(schemaPath, schema, snippetsbyKind);
                        break;

                    case " DEFAULT":
                        ProcessDefault(schemaPath, schema, snippetsbyKind);
                        break;

                    case " FUNCTION":
                        ProcessFunction(schemaPath, schema, snippetsbyKind);
                        break;

                    case " SCHEMA":
                        ProcessSchema(schemaPath, schema, snippetsbyKind);
                        break;

                    case " FK CONSTRAINT":
                        ProcessFkConstraint(schemaPath, schema, snippetsbyKind);
                        break;

                    case " AGGREGATE":
                        ProcessAggregate(schemaPath, schema, snippetsbyKind);
                        break;

                    case " TYPE":
                        ProcessType(schemaPath, schema, snippetsbyKind);
                        break;

                    case " CONSTRAINT":
                        ProcessConstraint(schemaPath, schema, snippetsbyKind);
                        break;

                    case " TABLE":
                        ProcessTable(schemaPath, schema, snippetsbyKind);
                        break;

                    case " VIEW":
                        ProcessView(schemaPath, schema, snippetsbyKind);
                        break;

                    case " COMMENT":
                        ProcessComment(schemaPath, schema, snippetsbyKind);
                        break;

                    case " INDEX":
                        ProcessIndex(schemaPath, schema, snippetsbyKind);
                        break;

                    case " TRIGGER":
                        ProcessTrigger(schemaPath, schema, snippetsbyKind);
                        break;

                    case " SEQUENCE OWNED BY":
                        ProcessSequenceOwnedBy(schemaPath, schema, snippetsbyKind);
                        break;

                    case "UN":
                        ProcessUn(schemaPath, schema, snippetsbyKind);
                        break;

                    default:
                        break;
                }

            }

            #endregion



        }

        public void ProcessSequence(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
            string filePath = Path.Combine(basepath, "sequence");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {
                foreach (var snippet in snippets)
                {
                    string filename = snippet.name;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(filePath, $"{filename}.sql"), false, Encoding.UTF8))
                    {
                        foreach (var item in snippet.snippets)
                        {
                            sw.WriteLine(item);
                        }
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(AppContext.BaseDirectory, $"log_{DateTime.Now.ToString("yyyyMMddHHmmss")}.log"), ex.Message);
            }
        }

        public void ProcessDefault(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
        }

        public void ProcessFunction(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
            string filePath = Path.Combine(basepath, "function");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {
                foreach (var snippet in snippets)
                {
                    string filename = snippet.name.Split('(',',')[0];
                    using (StreamWriter sw = new StreamWriter(Path.Combine(filePath, $"{filename}.sql"), false, Encoding.UTF8))
                    {
                        foreach (var item in snippet.snippets)
                        {
                            sw.WriteLine(item);
                        }
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(AppContext.BaseDirectory, $"log_{DateTime.Now.ToString("yyyyMMddHHmmss")}.log"), ex.Message);
            }
        }


        public void ProcessSchema(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
            System.IO.StreamWriter outfile = null;
            try
            {
                string filePath = Path.Combine(basepath, "schema");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);

                }
                string schemfile = Path.Combine(filePath, $"{schema}.sql");

                if (outfile == null)
                {
                    outfile = new System.IO.StreamWriter(schemfile, false, Encoding.UTF8);
                }

                foreach (var snippet in snippets)
                {

                    foreach (var item in snippet.snippets)
                    {
                        outfile.WriteLine(item);
                    }

                }
                outfile.Flush();
            }
            finally
            {
                if (outfile != null)
                    outfile.Dispose();
            }
        }

        public void ProcessFkConstraint(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
        }

        public void ProcessAggregate(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
        }

        public void ProcessType(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
        }

        public void ProcessConstraint(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
        }

        public void ProcessTable(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
            string filePath = Path.Combine(basepath, "table");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {
                foreach (var snippet in snippets)
                {
                    string filename = snippet.name;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(filePath, $"{filename}.sql"), false, Encoding.UTF8))
                    {
                        foreach (var item in snippet.snippets)
                        {
                            sw.WriteLine(item);
                        }
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(AppContext.BaseDirectory, $"log_{DateTime.Now.ToString("yyyyMMddHHmmss")}.log"), ex.Message);
            }

        }

        public void ProcessView(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
            string filePath = Path.Combine(basepath, "view");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {
                foreach (var snippet in snippets)
                {
                    string filename = snippet.name;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(filePath, $"{filename}.sql"), false, Encoding.UTF8))
                    {
                        foreach (var item in snippet.snippets)
                        {
                            sw.WriteLine(item);
                        }
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(AppContext.BaseDirectory, $"log_{DateTime.Now.ToString("yyyyMMddHHmmss")}.log"), ex.Message);
            }
        }

        public void ProcessComment(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
        }

        public void ProcessIndex(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
            string filePath = Path.Combine(basepath, "index");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {
                foreach (var snippet in snippets)
                {
                    string filename = snippet.name;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(filePath, $"{filename}.sql"), false, Encoding.UTF8))
                    {
                        foreach (var item in snippet.snippets)
                        {
                            sw.WriteLine(item);
                        }
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(AppContext.BaseDirectory, $"log_{DateTime.Now.ToString("yyyyMMddHHmmss")}.log"), ex.Message);
            }
        }

        public void ProcessTrigger(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
        }

        public void ProcessSequenceOwnedBy(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
            string filePath = Path.Combine(basepath, "sequenceownby");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {
                foreach (var snippet in snippets)
                {
                    string filename = snippet.name;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(filePath, $"{filename}.sql"), false, Encoding.UTF8))
                    {
                        foreach (var item in snippet.snippets)
                        {
                            sw.WriteLine(item);
                        }
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(AppContext.BaseDirectory, $"log_{DateTime.Now.ToString("yyyyMMddHHmmss")}.log"), ex.Message);
            }
        }

        public void ProcessUn(string basepath, string schema, IList<ScriptSnippet> snippets)
        {
        }




    }


    [Table("hdr_script_snippets")]
    public class ScriptSnippet
    {
        [ExplicitKey]
        [Key]
        public int id { get; set; } // serial primary key,


        public int startline { get; set; }

        public int endline { get; set; }

        public string filename { get; set; }

        public string schemaname { get; set; }

        public IList<string> snippets { get; set; }

        public string tyype { get; set; }

        public string name { get; set; }
    }

}
