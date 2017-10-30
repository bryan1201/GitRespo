using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;

using TransformReport.Configuration;

namespace TransformReport
{
    interface IReportTransformer
    {
        void Transform(TransformSection transformSection, string hallName, string reportName, string importPath, HSSFSheet targetSheet);
        IList<string> GetHallsList(TransformSection transformSection);
        void SetExcelFiles(FormMain fm, TransformSection transformConfiguration, string dir);
    }
}