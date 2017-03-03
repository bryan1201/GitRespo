using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

using TransformReport.Configuration;

namespace TransformReport
{
    class BaseTransformer : IReportTransformer
    {
        protected TransformSection transformSection;
        protected string hallName;
        protected string reportName;

        protected int sourceDataRowNum;
        protected int sourceDataColNum;
        protected int targetDataRowNum;
        protected int targetDataColNum;

        #region IReportTransformer Members

        public void Transform(TransformSection transformSection, string hallName, string reportName, string importPath, HSSFSheet targetSheet)
        {
            this.transformSection = transformSection;
            this.hallName = hallName;
            this.reportName = reportName;

            sourceDataRowNum = transformSection.SourceLeftHeaderRow;
            sourceDataColNum = transformSection.SourceTopHeaderCol;
            targetDataRowNum = transformSection.Halls[hallName].TargetLeftHeaderRow;
            targetDataColNum = transformSection.Halls[hallName].TargetTopHeaderCol;

            using (FileStream sourceStream = new FileStream(importPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                HSSFWorkbook sourceWorkbook = new HSSFWorkbook(sourceStream);

                HSSFSheet sourceSheet = sourceWorkbook.GetSheetAt(0) as HSSFSheet;
                HSSFRow sourceHeaderRow = sourceSheet.GetRow(transformSection.SourceTopHeaderRow) as HSSFRow;

                int numbertOfSourceHeaderCols = sourceSheet.GetRow(transformSection.SourceTopHeaderRow).LastCellNum;
                int sourceRowIndex = transformSection.SourceLeftHeaderRow;
                int sourceCellIndex;
                string[] sourceLeftHeaders = new string[transformSection.Halls[hallName].Reports[reportName].HeaderCols];

                targetSheet.ForceFormulaRecalculation = true;
                while (true)
                {
                    HSSFRow sourceDataRow = sourceSheet.GetRow(sourceRowIndex) as HSSFRow;
                    HSSFCell sourceDataCell = sourceDataRow.GetCell(0) as HSSFCell;

                    if (sourceDataCell.StringCellValue == "總計") break;

                    for (sourceCellIndex = sourceDataColNum; sourceCellIndex < numbertOfSourceHeaderCols; sourceCellIndex++)
                    {
                        WriteToTarget(sourceRowIndex, sourceCellIndex, sourceSheet, targetSheet, sourceLeftHeaders);
                    }

                    sourceRowIndex++;
                }

                sourceStream.Close();
            }
        }

        #endregion

        protected string GetFieldValueString(HSSFCell cell)
        {
            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue.Trim();
                case CellType.Boolean:
                    return Convert.ToString(cell.BooleanCellValue);
                case CellType.Numeric:
                    return Convert.ToString(cell.NumericCellValue);
                default:
                    return cell.ToString();
            }
        }

        protected void WriteToTarget(int sourceRowIndex, int sourceCellIndex, HSSFSheet sourceSheet, HSSFSheet targetSheet, string[] sourceLeftHeaders)
        {
            #region 來源資料

            // 來源上方標頭
            HSSFRow sourceTopHeaderRow = sourceSheet.GetRow(transformSection.SourceTopHeaderRow) as HSSFRow;
            HSSFCell sourceTopHeaderCell = sourceTopHeaderRow.GetCell(sourceCellIndex) as HSSFCell;
            string sourceTopHeader = sourceTopHeaderCell.StringCellValue.Trim();

            // 來源左方標頭
            HSSFRow sourceLeftHeaderRow = sourceSheet.GetRow(sourceRowIndex) as HSSFRow;
            HSSFCell sourceLeftHeaderCell;
            for (int index = 0; index < transformSection.Halls[hallName].Reports[reportName].HeaderCols; index++)
            {
                sourceLeftHeaderCell = sourceLeftHeaderRow.GetCell(transformSection.SourceLeftHeaderCol + index) as HSSFCell;
                string value = GetFieldValueString(sourceLeftHeaderCell);
                if (!string.IsNullOrEmpty(value) || (index == transformSection.Halls[hallName].Reports[reportName].HeaderCols - 1))
                    sourceLeftHeaders[index] = value;
            }

            // 來源資料
            HSSFRow sourceDataRow = sourceSheet.GetRow(sourceRowIndex) as HSSFRow;
            HSSFCell sourceDataCell = sourceDataRow.GetCell(sourceCellIndex) as HSSFCell;
            int sourceData = Convert.ToInt32(sourceDataCell.NumericCellValue);

            #endregion

            #region 用來源標頭找出目的標頭

            string targetTopHeader = string.Empty;
            string targetLeftHeader = string.Empty;

            // 目的上方標頭
            if (null == transformSection.Halls[hallName].Reports[reportName].Cols[sourceTopHeader]) return;
            targetTopHeader = transformSection.Halls[hallName].Reports[reportName].Cols[sourceTopHeader].Value;

            // 目的左方標頭
            string sourceLeftHeader = string.Join(transformSection.Seperator, sourceLeftHeaders);

            if (null != transformSection.Halls[hallName].Reports[reportName].Rows[sourceLeftHeader])
                targetLeftHeader = transformSection.Halls[hallName].Reports[reportName].Rows[sourceLeftHeader].Value;

            if (string.IsNullOrEmpty(targetLeftHeader))
            {
                if (null != transformSection.Halls[hallName].Rows[sourceLeftHeader])
                    targetLeftHeader = transformSection.Halls[hallName].Rows[sourceLeftHeader].Value;
            }

            #endregion

            if (string.IsNullOrEmpty(targetTopHeader) ||
                string.IsNullOrEmpty(targetLeftHeader))
                return;

            #region 用目的標頭找出目的資料的索引

            int targetDataRowIndex = -1;
            int targetDataCellIndex = -1;

            // 找出目的左方標頭
            string[] targetLeftHeaders = targetLeftHeader.Split(transformSection.Seperator[0]);
            bool[] targetLeftHeaderFound = new bool[targetLeftHeaders.Length];

            for (int targetRowIndex = transformSection.Halls[hallName].TargetLeftHeaderRow; ; targetRowIndex++)
            {
                HSSFRow targetLeftHeaderRow = targetSheet.GetRow(targetRowIndex) as HSSFRow;
                string targetLeftHeaderCellValue = GetFieldValueString(targetLeftHeaderRow.GetCell(transformSection.Halls[hallName].TargetLeftHeaderCol) as HSSFCell);
                bool allFound = true;

                if (targetLeftHeaderCellValue == "總計") return;
                if (targetLeftHeaderCellValue == "合計") continue;

                for (int index = 0; index < targetLeftHeaders.Length; index++)
                {
                    if (targetLeftHeaderFound[index])
                        continue;

                    HSSFCell targetLeftHeaderCell = targetLeftHeaderRow.GetCell(transformSection.Halls[hallName].TargetLeftHeaderCol + index) as HSSFCell;
                    targetLeftHeaderCellValue = GetFieldValueString(targetLeftHeaderCell);

                    if (targetLeftHeaderCellValue == targetLeftHeaders[index])
                        targetLeftHeaderFound[index] = true;
                    else
                        break;
                }

                for (int index = 0; index < targetLeftHeaders.Length; index++)
                {
                    if (!targetLeftHeaderFound[index])
                    {
                        allFound = false;
                        break;
                    }
                }

                if (allFound)
                {
                    targetDataRowIndex = targetRowIndex;
                    break;
                }
            }

            // 找出目的上方標頭
            string[] targetTopHeaders = targetTopHeader.Split(transformSection.Seperator[0]);
            bool[] targetTopHeaderFound = new bool[targetTopHeaders.Length];

            HSSFRow targetTopHeaderRow = targetSheet.GetRow(transformSection.Halls[hallName].TargetTopHeaderRow) as HSSFRow;
            int numberOfTargetHeaderCols = targetTopHeaderRow.LastCellNum;

            for (int targetCellIndex = transformSection.Halls[hallName].TargetTopHeaderCol; targetCellIndex < numberOfTargetHeaderCols; targetCellIndex++)
            {
                string targetTopHeaderCellValue = string.Empty;
                bool allFound = true;

                for (int index = 0; index < targetTopHeaders.Length; index++)
                {
                    if (targetTopHeaderFound[index])
                        continue;

                    targetTopHeaderRow = targetSheet.GetRow(transformSection.Halls[hallName].TargetTopHeaderRow + index) as HSSFRow;
                    HSSFCell targetTopHeaderCell = targetTopHeaderRow.GetCell(targetCellIndex) as HSSFCell;
                    targetTopHeaderCellValue = targetTopHeaderCell.StringCellValue.Trim();

                    if (targetTopHeaderCellValue == targetTopHeaders[index])
                        targetTopHeaderFound[index] = true;
                    else
                        break;
                }

                for (int index = 0; index < targetTopHeaders.Length; index++)
                {
                    if (!targetTopHeaderFound[index])
                    {
                        allFound = false;
                        break;
                    }
                }

                if (allFound)
                {
                    targetDataCellIndex = targetCellIndex;
                    break;
                }
            }

            #endregion

            #region 目的資料

            if ((targetDataRowIndex != -1) && (targetDataCellIndex != -1))
            {
                HSSFRow targetDataRow = targetSheet.GetRow(targetDataRowIndex) as HSSFRow;
                HSSFCell targetDataCell = targetDataRow.GetCell(targetDataCellIndex) as HSSFCell;

                targetDataCell.SetCellValue(Convert.ToDouble(sourceData));
            }

            #endregion
        }
    }
}
