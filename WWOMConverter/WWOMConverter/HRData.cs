using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWOMConverter
{
    /*
     # pa.txt 
	    卡號 EmployeeID
	    姓名 DisplayName
	    部門代號 DepartmentID
	    部門名稱 DepartmentName
	    eMail
	    主管卡號 ManagerID
	    主管姓名 ManagerName
	    主管eMail ManagerEMail
        分機 EXT
	    租戶	Tenant
    
    # dept.txt
	    廠區 Site
	    部門代號 DepartmentID
	    部門名稱 DisplayName
	    主管卡號 ManagerID
	    主管姓名 ManagerName
	    上層部門代號 ParentDeptID
	    上層部門名稱 ParentDeptName
     */

    class HRDept
    {
        public string Site { get; set; }
        public string DepartmentID { get; set; }
        public string DisplayName { get; set; }
        public string ManagerID { get; set; }
        public string ManagerName { get; set; }
        public string ParentDeptID { get; set; }
        public string ParentDeptName { get; set; }
    }

    class HRPa
    {
        public string EmployeeID { get; set; }
        public string DisplayName { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string eMail { get; set; }
        public string ManagerID { get; set; }
        public string ManagerName { get; set; }
        public string ManagereMail { get; set; }
        public string EXT { get; set; }
        public string Tenant { get; set; }
    }
}
