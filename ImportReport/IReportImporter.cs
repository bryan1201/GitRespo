using System;
using System.Collections.Generic;
using System.Text;
using NPOI.HSSF.UserModel;

using ImportReport.Configuration;

namespace ImportReport
{
    interface IReportImporter
    {
        void Import(ImportSection importSection, DateTime meetingDate, string hallName, HSSFSheet sourceSheet, ref StringBuilder sql);
    }
}
