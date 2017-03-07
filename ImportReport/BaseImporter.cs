using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

using ImportReport.Configuration;

namespace ImportReport
{
    class BaseImporter : IReportImporter
    {
        #region IReportImporter Members

        public void Import(ImportSection importSection, DateTime meetingDate, string hallName, HSSFSheet sourceSheet, ref StringBuilder sql)
        {
            HallElement hall = importSection.Halls[hallName];
            string leftHeaderString = string.Empty;
            string leftSubHeaderString = string.Empty;
            string left2SubHeaderString = string.Empty;
            string topHeaderString = string.Empty;
            string topSubHeaderString = string.Empty;
            string top2SubHeaderString = string.Empty;

            using (DataTable lifeNumberTable = InitializeTable(importSection.Cols))
            {
                for (int dataRowIndex = hall.SourceLeftHeaderRow; ; dataRowIndex++)
                {
                    HSSFRow sourceTopHeaderRow = sourceSheet.GetRow(hall.SourceTopHeaderRow) as HSSFRow;
                    HSSFRow sourceTopSubHeaderRow = sourceSheet.GetRow(hall.SourceTopSubHeaderRow) as HSSFRow;
                    HSSFRow sourceTop2SubHeaderRow = sourceSheet.GetRow(hall.SourceTopSubHeaderRow + 1) as HSSFRow;
                    HSSFRow sourceDataRow = sourceSheet.GetRow(dataRowIndex) as HSSFRow;
                    string key = string.Empty;
                    if (!string.IsNullOrEmpty(sourceDataRow.GetCell(hall.SourceLeftHeaderCol).StringCellValue))
                        leftHeaderString = sourceDataRow.GetCell(hall.SourceLeftHeaderCol).StringCellValue;

                    if (leftHeaderString == "總計")
                        break;

                    if (leftHeaderString == "合計")
                        continue;

                    switch (sourceDataRow.GetCell(hall.SourceLeftSubHeaderCol).CellType)
                    {
                        case CellType.String:
                            leftSubHeaderString = sourceDataRow.GetCell(hall.SourceLeftSubHeaderCol).StringCellValue;
                            break;
                        case CellType.Numeric:
                            leftSubHeaderString = Convert.ToString(sourceDataRow.GetCell(hall.SourceLeftSubHeaderCol).NumericCellValue);
                            break;
                    }

                    switch (sourceDataRow.GetCell(hall.SourceLeftSubHeaderCol + 1).CellType)
                    {
                        case CellType.String:
                            left2SubHeaderString = sourceDataRow.GetCell(hall.SourceLeftSubHeaderCol + 1).StringCellValue;
                            break;
                        case CellType.Numeric:
                            left2SubHeaderString = Convert.ToString(sourceDataRow.GetCell(hall.SourceLeftSubHeaderCol + 1).NumericCellValue);
                            break;
                    }

                    //if (left2SubHeaderString == "區小計")
                    //{
                        key = string.Format("{0}{1}{2}", leftHeaderString, importSection.Seperator, leftSubHeaderString);
                    //}
                    //else
                    //{
                    //    key = string.Format("{0}{1}{2}{1}{3}", leftHeaderString, importSection.Seperator, leftSubHeaderString, left2SubHeaderString);
                    //}

                    DataRow dataRow = lifeNumberTable.NewRow();
                    dataRow["MeetingDate"] = meetingDate;
                    dataRow["TableID"] = FindTableID(hall, key);
                    dataRow["BaseNum"] = sourceDataRow.GetCell(hall.SourceLeftSubHeaderCol + 2).NumericCellValue;
                    dataRow["TargetNum"] = sourceDataRow.GetCell(hall.SourceLeftSubHeaderCol + 3).NumericCellValue;

                    for (int dataColIndex = hall.SourceTopHeaderCol; dataColIndex < sourceTopHeaderRow.LastCellNum; dataColIndex++)
                    {
                        string value;

                        if (!string.IsNullOrEmpty(sourceTopHeaderRow.GetCell(dataColIndex).StringCellValue))
                        {
                            topHeaderString = sourceTopHeaderRow.GetCell(dataColIndex).StringCellValue;
                            topSubHeaderString = string.Empty;
                            top2SubHeaderString = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(sourceTopSubHeaderRow.GetCell(dataColIndex).StringCellValue))
                            topSubHeaderString = sourceTopSubHeaderRow.GetCell(dataColIndex).StringCellValue;

                        if (!string.IsNullOrEmpty(sourceTop2SubHeaderRow.GetCell(dataColIndex).StringCellValue))
                        {
                            top2SubHeaderString = sourceTop2SubHeaderRow.GetCell(dataColIndex).StringCellValue;
                            value = FindField(importSection.Cols, string.IsNullOrEmpty(top2SubHeaderString) ? topHeaderString : string.Format("{0}{1}{2}{1}{3}", topHeaderString, importSection.Seperator, topSubHeaderString, top2SubHeaderString));
                        }
                        else
                        {
                            value = FindField(importSection.Cols, string.IsNullOrEmpty(topSubHeaderString) ? topHeaderString : string.Format("{0}{1}{2}", topHeaderString, importSection.Seperator, topSubHeaderString));
                        }

                        
                        if (!string.IsNullOrEmpty(value))
                        {
                            dataRow[value] = sourceDataRow.GetCell(dataColIndex).NumericCellValue;
                        }
                    }

                    WriteTo(lifeNumberTable, dataRow, ref sql);
                }
            }
        }

        #endregion

        private DataTable InitializeTable(NameValueConfigurationCollection cols)
        {
            DataTable lifeNumberTable = new DataTable();

            lifeNumberTable.Columns.Add("MeetingDate", typeof(DateTime));
            lifeNumberTable.Columns.Add("TableID", typeof(int));
            lifeNumberTable.Columns.Add("BaseNum", typeof(short));
            lifeNumberTable.Columns.Add("TargetNum", typeof(short));

            foreach (string name in cols.AllKeys)
                lifeNumberTable.Columns.Add(cols[name].Value, typeof(short));

            return lifeNumberTable;
        }

        private string FindField(NameValueConfigurationCollection cols, string key)
        {
            foreach (string name in cols.AllKeys)
            {
                if (name == key)
                    return cols[name].Value;
            }

            return string.Empty;
        }

        private int FindTableID(HallElement hall, string key)
        {
            foreach (string name in hall.Rows.AllKeys)
            {
                if (name == key)
                    return Convert.ToInt32(hall.Rows[name].Value);
            }

            return 0;
        }

        private void WriteTo(DataTable lifeNumberTable, DataRow dataRow, ref StringBuilder sql)
        {
            List<string> fieldList = new List<string>();
            List<string> valueList = new List<string>();

            foreach (DataColumn col in lifeNumberTable.Columns)
            {
                fieldList.Add(col.ColumnName);
                if (col.DataType == typeof(DateTime))
                    valueList.Add(string.Format("'{0}'", Convert.ToDateTime(dataRow[col]).ToString("yyyy/MM/dd")));
                else if ((col.DataType == typeof(int)) || col.DataType == typeof(short))
                    valueList.Add(dataRow[col].ToString());
            }

            sql.AppendFormat("INSERT INTO LifeNumber({0}) ", string.Join(",", fieldList.ToArray()));
            sql.AppendFormat("VALUES({0}) ", string.Join(",", valueList.ToArray()));
            sql.AppendLine();
        }
    }
}
