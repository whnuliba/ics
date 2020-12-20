using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NE.ICS.ORM.Xml
{
    public abstract class AbstractParseMapperXml : IParseXml
    {
        public abstract void Parse(string xmlfile, string assembly);
        public void ParseSelect(XmlNode node, SqlObject sqlObject)
        {
            if (node.Attributes["id"] == null)
                throw new Exception("有select节点的ID为空");
            BoundSql bSql = new BoundSql();
            bSql.Command = XmlSqlEnum.select;
            XmlNodeList nodes = node.ChildNodes;
            bSql.ParameterType = node.Attributes["ParameterType"] != null ? node.Attributes["ParameterType"].Value : null;
            bSql.ResultType = node.Attributes["ResultType"] != null ? node.Attributes["ResultType"].Value : null;
            bSql.Id = node.Attributes["id"].Value;
            sqlObject.BoundSql.Add($"{bSql.Id}", bSql);
            XmlNodeList nodeList = node.ChildNodes;
            StringBuilder sqlStr = new StringBuilder();
            foreach (XmlNode n in nodeList)
            {
                if (n.NodeType == XmlNodeType.Text) {
                    sqlStr.Append($" {n.Value}");
                }
                if (n.NodeType == XmlNodeType.Element && n.Name != null && n.Name.ToLower() == "where")
                {
                    sqlStr.Append(" #{{"+ n.Name + "}}");
                    ParseWhere(n, bSql);
                }
            }
            bSql.Sql = sqlStr.ToString();
        }
        public void ParseInsert(XmlNode node, SqlObject sqlObject)
        {
            if (node.Attributes["id"] == null)
                throw new Exception("有Insert节点的ID为空");
            BoundSql bSql = new BoundSql();
            bSql.Command = XmlSqlEnum.insert;
            bSql.Sql = node.FirstChild.Value;
            XmlNodeList nodes = node.ChildNodes;
            bSql.ParameterType = node.Attributes["ParameterType"] != null ? node.Attributes["ParameterType"].Value : null;
            bSql.Id = node.Attributes["id"].Value;
            sqlObject.BoundSql.Add($"{bSql.Id}", bSql);
            XmlNodeList nodeList = node.ChildNodes;
            StringBuilder sqlStr = new StringBuilder();
            foreach (XmlNode n in nodeList)
            {
                if (n.NodeType == XmlNodeType.Text)
                {
                    sqlStr.Append($" {n.Value}");
                }
                if (n.NodeType == XmlNodeType.Element && n.Name != null && n.Name.ToLower() == "where")
                {
                    sqlStr.Append(" #{{" + n.Name + "}}");
                    ParseWhere(n, bSql);
                }
            }
            bSql.Sql = sqlStr.ToString();

        }
        public void ParseDelete(XmlNode node, SqlObject sqlObject)
        {

            if (node.Attributes["id"] == null)
                throw new Exception("有Delete节点的ID为空");
            BoundSql bSql = new BoundSql();
            bSql.Command = XmlSqlEnum.delete;
            bSql.Sql = node.FirstChild.Value;
            XmlNodeList nodes = node.ChildNodes;
            bSql.ParameterType = node.Attributes["ParameterType"] != null ? node.Attributes["ParameterType"].Value : null;
            bSql.Id = node.Attributes["id"].Value;
            sqlObject.BoundSql.Add($"{bSql.Id}", bSql);
            XmlNodeList nodeList = node.ChildNodes;
            StringBuilder sqlStr = new StringBuilder();
            foreach (XmlNode n in nodeList)
            {
                if (n.NodeType == XmlNodeType.Text)
                {
                    sqlStr.Append($" {n.Value}");
                }
                if (n.NodeType == XmlNodeType.Element && n.Name != null && n.Name.ToLower() == "where")
                {
                    sqlStr.Append(" #{{" + n.Name + "}}");
                    ParseWhere(n, bSql);
                }
            }
            bSql.Sql = sqlStr.ToString();

        }
        public void ParseUpdate(XmlNode node, SqlObject sqlObject)
        {
            if (node.Attributes["id"] == null)
                throw new Exception("有Update节点的ID为空");
            BoundSql bSql = new BoundSql();
            bSql.Command = XmlSqlEnum.update;
            XmlNodeList nodes = node.ChildNodes;
            bSql.ParameterType = node.Attributes["ParameterType"] != null ? node.Attributes["ParameterType"].Value : null;
            bSql.Id = node.Attributes["id"].Value;
            sqlObject.BoundSql.Add($"{bSql.Id}", bSql);
            XmlNodeList nodeList = node.ChildNodes;
            StringBuilder sqlStr = new StringBuilder();
            foreach (XmlNode n in nodeList)
            {
                if (n.NodeType == XmlNodeType.Text)
                {
                    sqlStr.Append($" {n.Value}");
                }
                if (n.Name != null && n.Name.ToLower() == "where")
                {
                    sqlStr.Append(" #{{" + n.Name + "}}");
                    ParseWhere(n, bSql);
                }
                if (n.Name != null && n.Name.ToLower() == "set") {
                    sqlStr.Append(" #{{" + n.Name + "}}");
                    ParseSet(n, bSql);
                }
            }
            bSql.Sql = sqlStr.ToString();
        }
        public void ParseWhere(XmlNode node, BoundSql sql)
        {
            ExtCondition condition = new ExtCondition();
            StringBuilder where = new StringBuilder();
            string fVal = node.FirstChild.Value;
            if (!string.IsNullOrWhiteSpace(fVal))
                where.Append($" {fVal}");
            //where下有if节点
            condition.Content = where.ToString();
            XmlNodeList ifs = node.ChildNodes;
            foreach (XmlNode n in ifs)
            {
                if (n.NodeType != XmlNodeType.Text && (n.Name!=null && n.Name.ToLower() != "if"))
                {
                    throw new IcsOrmException($"当前节点不能出现{n.Name}");
                }
                ParseIf(n, condition);
            }
            sql.Condition = condition;
        }

        public void ParseSet(XmlNode node, BoundSql sql)
        {
            ExtCondition condition = new ExtCondition();
            StringBuilder set = new StringBuilder();
            string fVal = node.FirstChild.Value;
            if (!string.IsNullOrWhiteSpace(fVal))
                set.Append($" {fVal}");
            //where下有if节点
            condition.Content = set.ToString();
            XmlNodeList ifs = node.ChildNodes;
            foreach (XmlNode n in ifs)
            {
                if (n.NodeType != XmlNodeType.Text && (n.Name != null && n.Name.ToLower() != "if"))
                {
                    throw new IcsOrmException($"当前节点不能出现{n.Name}");
                }
                ParseIf(n, condition);
            }
            sql.Set = condition;
        }
        public void ParseIf(XmlNode node, ExtCondition condition)
        {
            foreach (XmlNode xn in node.ChildNodes) {
                if (xn.NodeType != XmlNodeType.Text) {
                    throw new IcsOrmException($"当前节点不能出现{xn.Name}");
                }                  
            }
            //if 只有一个test属性
            IfTest ifTest = new IfTest();
            string test = node.Attributes["test"].Value;
            char[] ch = test.ToCharArray();
            string start = null;
            string oper = "><=!";
            int n = 0;

            int m = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (oper.IndexOf(ch[i]) >=0 )
                {
                    start = ch[i].ToString();
                    n = i;
                    if (ch[i +1 ] == '=')
                    {
                        start += ch[i +1 ];
                        m = i +1 ;
                    }

                    break;
                }
            }
            ifTest.Left = test.Substring(0, n).Trim();
            ifTest.Right = test.Substring(m+1).Trim();
            ifTest.Operator = start;
            ifTest.Content = node.FirstChild.Value;
            condition.Properties.Add(ifTest.Left, ifTest);
        }

        public void ParseTrim(XmlNode node)
        {


        }

    }
}
