using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class Program
    {
        static string WebServiceHRLoginID = Constant.S_ID;
        static string WebServiceHRPWD = Constant.S_PWD;

        static void Main(string[] args)
        {
            string fileDept = Constant.S_DestFilePath + Constant.S_DestFileDept;
            string fileUser = Constant.S_DestFilePath + Constant.S_DestFileUser;
            
            //fn_GenXML(fileName1,fileName2);
            General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: "\n");
            General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: @"Main().Start TFGHRWWOM.Program.cs");
            try
            {
                General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: @"Main().Start fn_GenDeptXML(fileDept)");
                fn_GenDeptXML(fileDept);
                General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: @"Main().Finish fn_GenDeptXML(fileDept)");

                General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: @"Main().Start fn_GenUserXML2(fileUser)");
                fn_GenUserXML2(fileUser);
                General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: @"Main().Finish fn_GenUserXML2(fileUser)");

                General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: @"Main().Start fn_WriteXMLtoSQL()");
                fn_WriteXMLtoSQL();
                General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: @"Main().Finish fn_WriteXMLtoSQL()");
            }
            catch (Exception ex)
            {
                string message = "TFGHRWWOM Program.cs Main()." + ex.Message;
                General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: message);
                Console.WriteLine("Error: {0}\n", message);
            }
            General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: @"Main().Finish TFGHRWWOM.Program.cs");
        } // Main

        static void fn_WriteXMLtoSQL()
        {
            string message = "";
            int p = 0;
            wwom.WebServiceSoapClient ws = new wwom.WebServiceSoapClient("WebServiceSoap");
            System.Data.DataTable srcdept = ws.OmPaData(p.ToString(), WebServiceHRLoginID, WebServiceHRPWD);
            p = 1;
            System.Data.DataTable srcuser = ws.OmPaData(p.ToString(), WebServiceHRLoginID, WebServiceHRPWD);
            
            string sqlCmd = "EXEC sp_ClearWWOMDBData";
            // Clear temp tables data, tmpDepts, tmpUsers

            DAO.sqlCmd(Constant.S_SqlConnStr, sqlCmd);
            General.Method.WriteLog(Constant.S_ProgramLog, message = @"fn_WriteXMLtoSQL().Finish " + sqlCmd);

            DAO.DatatableToSQL(Constant.S_SqlConnStr, srcdept, Constant.S_DestTableDepts);
            General.Method.WriteLog(Constant.S_ProgramLog, message = @"fn_WriteXMLtoSQL().Finish bulkcopy " + Constant.S_DestTableDepts);

            DAO.DatatableToSQL(Constant.S_SqlConnStr, srcuser, Constant.S_DestTableUsers);
            General.Method.WriteLog(Constant.S_ProgramLog, message = @"fn_WriteXMLtoSQL().Finish bulkcopy " + Constant.S_DestTableUsers);

            // Move to Production tables, WWDepts, WWUsers
            sqlCmd = "EXEC sp_WWOMDB";
            DAO.sqlCmd(Constant.S_SqlConnStr, sqlCmd);
            General.Method.WriteLog(Constant.S_ProgramLog, message = @"fn_WriteXMLtoSQL().Finish " + sqlCmd);
            // End
        } // fn_WriteXMLtoSQL

        static void fn_GenDeptXML(string fileName1)
        {
            int p = 0;
            string chkSpace = "";
            string repSpace = "Empty";

            XmlTextWriter xmldept = new XmlTextWriter(fileName1, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 4
                ,
                Namespaces = true
            };
            xmldept.WriteStartDocument(false);

            wwom.WebServiceSoapClient ws = new wwom.WebServiceSoapClient("WebServiceSoap");
            System.Data.DataTable dtdept = ws.OmPaData(p.ToString(), WebServiceHRLoginID, WebServiceHRPWD);
            dtdept.WriteXml(Constant.S_DestFilePath + "dept20141017.xml");
            General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: @"fn_GenDeptXML().Finish WriteXml dept20141017.xml");

            xmldept.WriteStartElement("depts");
            foreach (DataRow dr1 in dtdept.Rows)
            {
                xmldept.WriteStartElement("dept");
                xmldept.WriteStartElement("id");
                xmldept.WriteString(dr1["Did"].ToString());
                xmldept.WriteEndElement();
                xmldept.WriteStartElement("name");
                chkSpace = dr1["ChiName"].ToString();
                if (chkSpace.Trim() == "")
                {
                    xmldept.WriteString(repSpace + p.ToString());
                    p++;
                }
                else
                    xmldept.WriteString(dr1["ChiName"].ToString() + dr1["Did"].ToString());
                xmldept.WriteEndElement();
                xmldept.WriteStartElement("superior");
                xmldept.WriteString(dr1["UpDid"].ToString());
                xmldept.WriteEndElement();
                xmldept.WriteStartElement("approve");
                xmldept.WriteString("0");
                xmldept.WriteEndElement();
                xmldept.WriteEndElement();
            }
            xmldept.WriteEndElement();
            xmldept.WriteEndDocument();
            xmldept.Close();
            xmldept = null;        
        } // fn_GenDeptXML

        static void fn_GenUserXML(string fileName2)
        {
            int p = 1;

            XmlTextWriter xmluser = new XmlTextWriter(fileName2, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 4
                ,
                Namespaces = true
            };
            xmluser.WriteStartDocument(false);

            wwom.WebServiceSoapClient ws = new wwom.WebServiceSoapClient("WebServiceSoap");
            p = 0;
            System.Data.DataTable dtdept = ws.OmPaData(p.ToString(), WebServiceHRLoginID, WebServiceHRPWD);
            p = 1;
            System.Data.DataTable dtuser = ws.OmPaData(p.ToString(), WebServiceHRLoginID, WebServiceHRPWD);
            // ***10/27 Zander*** dtuser.Select(filter, sort) by user, dept
            // <depts><dept>1</dept><dept>2</dept></depts>

            dtuser.DefaultView.Sort = "Eid";
            dtuser.DefaultView.Table.Select("Eid>='0'", "Eid DESC");

            xmluser.WriteStartElement("users");
            foreach (DataRow dr1 in dtuser.DefaultView.Table.Select("Eid>='0'", "Eid ASC"))
            {
                xmluser.WriteStartElement("user");
                    xmluser.WriteStartElement("name");
                    xmluser.WriteString(dr1["Eid"].ToString());
                    xmluser.WriteEndElement();

                    xmluser.WriteStartElement("alias");
                    xmluser.WriteString(dr1["displayName"].ToString());
                    xmluser.WriteEndElement();
                
                    xmluser.WriteStartElement("admin");
                    xmluser.WriteString("0");
                    xmluser.WriteEndElement();

                    xmluser.WriteStartElement("depts");
                        xmluser.WriteStartElement("dept");
                        xmluser.WriteString(dr1["Dept"].ToString());
                        xmluser.WriteEndElement();
                    xmluser.WriteEndElement();
                xmluser.WriteEndElement();
            }
            xmluser.WriteEndElement();
            
            xmluser.WriteEndDocument();
            xmluser.Close();
            xmluser = null;
            //dt.WriteXml(xml);
            //xml.WriteEndDocument();
        }

        static void fn_GenUserXML2(string fileName2)
        {
            /*
             一個人在User Table只會出現一次
             * 若是主管，則會出現在Dept Table中，manager才會重覆出現
             */
            int p = 1;
            XmlTextWriter xmluser = new XmlTextWriter(fileName2, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 4
                ,
                Namespaces = true
            };
            xmluser.WriteStartDocument(false);

            wwom.WebServiceSoapClient ws = new wwom.WebServiceSoapClient("WebServiceSoap");
            System.Data.DataTable dtuser = ws.OmPaData(p.ToString(), WebServiceHRLoginID, WebServiceHRPWD);
            dtuser.WriteXml(Constant.S_DestFilePath + "user20141017.xml");
            General.Method.WriteLog(filePath: Constant.S_ProgramLog, textToAdd: @"fn_GenUserXML2().Finish WriteXml user20141017.xml");
            // ***10/27 Zander*** dtuser.Select(filter, sort) by user, dept
            // <depts><dept>1</dept><dept>2</dept></depts>
            //dt.TableName = "dept";
            dtuser.DefaultView.Sort = "Eid DESC";
            xmluser.WriteStartElement("users");
            //DataRow[] foundrows = dtuser.DefaultView.Table.Select("Eid>='ICZ600250' AND Eid<='ICZ600259'", "Eid DESC");
            DataRow[] foundrows = dtuser.DefaultView.Table.Select("Eid>='0'", "Eid DESC");
            int iTCnt = foundrows.Length;
            string Eid1 = "";
            string Eid2 = "";
            for (int iIndex = 0; iIndex < iTCnt; iIndex++)
            {
                xmluser.WriteStartElement("user");
                xmluser.WriteStartElement("name");
                xmluser.WriteString(foundrows[iIndex]["Eid"].ToString());
                xmluser.WriteEndElement();

                xmluser.WriteStartElement("alias");
                xmluser.WriteString(foundrows[iIndex]["displayName"].ToString());
                xmluser.WriteEndElement();
                xmluser.WriteStartElement("admin");
                xmluser.WriteString("0");
                xmluser.WriteEndElement();

                xmluser.WriteStartElement("depts");

                xmluser.WriteStartElement("dept");
                xmluser.WriteString(foundrows[iIndex]["Dept"].ToString());
                xmluser.WriteEndElement();

                string Dept1 = "";
                string Dept2 = "";
                Eid1 = foundrows[iIndex]["Eid"].ToString();
                Dept1 = foundrows[iIndex]["Dept"].ToString();

                for (int x = iIndex+1; x < iTCnt; x++)
                {
                    Eid2 = foundrows[x]["Eid"].ToString();
                    Dept2 = foundrows[x]["Dept"].ToString();
                    if (Eid1 == Eid2 && Dept1!=Dept2)
                    {
                        xmluser.WriteStartElement("dept");
                        xmluser.WriteString(foundrows[x]["Dept"].ToString());
                        xmluser.WriteEndElement();
                        iIndex = x;
                        Eid2 = "2";
                        Dept1 = foundrows[x]["Dept"].ToString();
                    }
                }

                xmluser.WriteEndElement();
                xmluser.WriteEndElement();
            }
            xmluser.WriteEndElement();
            xmluser.WriteEndDocument();
            xmluser.Close();
            xmluser = null;
            //dt.WriteXml(xml);
            //xml.WriteEndDocument();
        } // fn_GenUserXML2

        static void fn_GenXML(string fileName1, string fileName2)
        {
            int p = 0;
            string chkSpace = "";
            string repSpace = "Empty";
            
            XmlTextWriter xmldept = new XmlTextWriter(fileName1, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 4
                ,
                Namespaces = true
            };
            xmldept.WriteStartDocument(false);

            XmlTextWriter xmluser = new XmlTextWriter(fileName2, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 4
                ,
                Namespaces = true
            };
            xmluser.WriteStartDocument(false);

            wwom.WebServiceSoapClient ws = new wwom.WebServiceSoapClient("WebServiceSoap");
            System.Data.DataTable dtdept = ws.OmPaData(p.ToString(), WebServiceHRLoginID, WebServiceHRPWD);
            p = 1;
            System.Data.DataTable dtuser = ws.OmPaData(p.ToString(), WebServiceHRLoginID, WebServiceHRPWD);
            // ***10/27 Zander*** dtdept.Select(filter, sort) by user, dept
            // <depts><dept>1</dept><dept>2</dept></depts>
            //dt.TableName = "dept";
            
            xmldept.WriteStartElement("depts");
            foreach (DataRow dr1 in dtdept.Rows)
            {
                xmldept.WriteStartElement("dept");
                xmldept.WriteStartElement("id");
                xmldept.WriteString(dr1["Did"].ToString());
                xmldept.WriteEndElement();
                xmldept.WriteStartElement("name");
                chkSpace = dr1["ChiName"].ToString();
                if (chkSpace.Trim() == "")
                {
                    xmldept.WriteString(repSpace + p.ToString());
                    p++;
                }
                else
                    xmldept.WriteString(dr1["ChiName"].ToString() + dr1["Did"].ToString());
                xmldept.WriteEndElement();
                xmldept.WriteStartElement("superior");
                xmldept.WriteString(dr1["UpDid"].ToString());
                xmldept.WriteEndElement();
                xmldept.WriteStartElement("approve");
                xmldept.WriteString("0");
                xmldept.WriteEndElement();
                xmldept.WriteEndElement();
            }
            xmldept.WriteEndElement();
            xmldept.WriteEndDocument();
            xmldept.Close();
            xmldept = null;
            
            dtuser.DefaultView.Sort = "Eid";
            
            xmluser.WriteStartElement("users");
            int iTCnt = dtuser.DefaultView.Table.Rows.Count;
            string Eid1 = "";
            string Eid2 = "";
            for (int iIndex = 0; iIndex < iTCnt; iIndex++)
            {
                Eid1 = dtuser.DefaultView.Table.Rows[iIndex]["Eid"].ToString();
                if ((iIndex + 1) < iTCnt)
                {
                    Eid2 = dtuser.DefaultView.Table.Rows[iIndex + 1]["Eid"].ToString();
                }
                else
                {
                    Eid2 = Eid1;
                }

                if (Eid1 != Eid2 && (iIndex+1)<iTCnt)
                {
                    xmluser.WriteStartElement("user");
                    xmluser.WriteStartElement("name");
                    xmluser.WriteString(dtuser.DefaultView.Table.Rows[iIndex]["Eid"].ToString());
                    xmluser.WriteEndElement();

                    xmluser.WriteStartElement("alias");
                    xmluser.WriteString(dtuser.DefaultView.Table.Rows[iIndex]["displayName"].ToString());
                    xmluser.WriteEndElement();
                    xmluser.WriteStartElement("admin");
                    xmluser.WriteString("0");
                    xmluser.WriteEndElement();

                    xmluser.WriteStartElement("depts");
                }

                if (Eid1 == Eid2 && (iIndex + 1) < iTCnt)
                {
                    xmluser.WriteStartElement("dept");
                    xmluser.WriteString(dtuser.DefaultView.Table.Rows[iIndex]["Dept"].ToString());
                    xmluser.WriteEndElement();
                    xmluser.WriteStartElement("dept");
                    xmluser.WriteString(dtuser.DefaultView.Table.Rows[iIndex + 1]["Dept"].ToString());
                    xmluser.WriteEndElement();
                    iIndex++;
                }
                else
                {
                    xmluser.WriteStartElement("dept");
                    xmluser.WriteString(dtuser.DefaultView.Table.Rows[iIndex]["Dept"].ToString());
                    xmluser.WriteEndElement();                
                }

                if (Eid1 != Eid2 && (iIndex+2) < iTCnt)
                {
                    xmluser.WriteEndElement();
                    xmluser.WriteEndElement();
                }
            }

            xmluser.WriteEndElement();
            xmluser.WriteEndDocument();
            xmluser.Close();
            xmluser = null;
            //dt.WriteXml(xml);
            //xml.WriteEndDocument();
        }
    }
}
