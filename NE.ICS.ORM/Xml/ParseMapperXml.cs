using NE.ICS.ORM.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace NE.ICS.ORM.Xml
{
    public class ParseMapperXml : AbstractParseMapperXml
    {
        public override void Parse(string xmlfile,string assembly)
        {
            //#region 测试反射
            //InnerTest inner = new InnerTest();
            //inner.name = "wanghao";
            //inner.age = ;
            //RefTest re = new RefTest();
            //re.Id = "";
            //re.Test = inner;
            //string name = ReflectionUtil.GetFieldValue("Test.name", re).ToString();
            //ReflectionUtil.GetObject("NE.ICS.Common.Utils.RefTest,NE.ICS.Common");
            //#endregion
            var configName = $"{assembly}.{xmlfile}";
            var directory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
             Assembly assembly1 = Assembly.LoadFile($"{AppDomain.CurrentDomain.BaseDirectory}\\{assembly}.dll");          
            var configStream = assembly1.GetManifestResourceStream(configName);
            if (null == configStream)
            {
                return;
            }
            XmlDocument xml = new XmlDocument();
            XmlTextReader rt = null;
            string innrtNodeStr = null;
            MetaObject metaObject = MetaObject.Instance;
            SqlObject sqlObject = new SqlObject();
            try {
                // rt = new XmlTextReader(configStream);
                //while (rt.Read()) {
                //    if (rt.NodeType == XmlNodeType.Element) {
                //        if (rt.Name == "mapper") {
                //            sqlObject.NameSpace=rt.GetAttribute("Namespaces");
                //            innrtNodeStr = rt.ReadInnerXml();
                //            break;
                //        }
                //    }
                //}
                // xml.LoadXml(innrtNodeStr);
                xml.Load(configStream);
                XmlNode root = xml.SelectSingleNode("mapper");
                sqlObject.NameSpace = root.Attributes["Namespaces"].Value;
                XmlNodeList selectNodes = root.SelectNodes(XmlSqlEnum.select.ToString());
                foreach (XmlNode node in selectNodes) {
                    ParseSelect(node, sqlObject);
                }
                XmlNodeList insertNodes = root.SelectNodes(XmlSqlEnum.insert.ToString());
                foreach (XmlNode node in insertNodes)
                {
                    ParseInsert(node, sqlObject);
                }
                XmlNodeList updateNodes = root.SelectNodes(XmlSqlEnum.update.ToString());
                foreach (XmlNode node in updateNodes)
                {
                    ParseUpdate(node, sqlObject);
                }
                XmlNodeList deleteNodes = root.SelectNodes(XmlSqlEnum.delete.ToString());
                foreach (XmlNode node in deleteNodes)
                {
                    ParseDelete(node, sqlObject);
                }

            } catch (Exception ex) {
                throw ex;
            } finally {
                if (null != rt) {
                    rt.Close();
                }
            }
            metaObject.BoundSql.Add(sqlObject.NameSpace, sqlObject);
        }
       
    }
}
/*
 var assembly = Assembly.GetExecutingAssembly();
var directory = Path.GetDirectoryName(assembly.Location);
var configName = $"{assembly.GetName().Name}./*此处为文件文件目录路径（如Folder.FileName.json）
";
var configStream = assembly.GetManifestResourceStream(configName);
if (null != configStream)
{
    var bytes = new byte[configStream.Length];
    configStream.Read(bytes, , bytes.Length);
    var text = Encoding.ASCII.GetString(bytes);
}
*/
