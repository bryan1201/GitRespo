//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CERLLAB.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Core.Objects.DataClasses;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    public partial class CERLEntities : DbContext
    {
        public CERLEntities()
            : base("name=CERLEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }


        [DbFunctionAttribute("CERLEntities", "FnCERLMenuDropDownList")]
        public virtual IQueryable<FnCERLMenuDropDownList_Result> FnCERLMenuDropDownList(string pid)
        {
            var pidParameter = pid != null ?
                new ObjectParameter("pid", pid) :
                new ObjectParameter("pid", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnCERLMenuDropDownList_Result>("[CERLEntities].[FnCERLMenuDropDownList](@pid)", pidParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGenInitInfo")]
        public virtual IQueryable<FnGenInitInfo_Result> FnGenInitInfo(string vchSet)
        {
            var vchSetParameter = vchSet != null ?
                new ObjectParameter("vchSet", vchSet) :
                new ObjectParameter("vchSet", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGenInitInfo_Result>("[CERLEntities].[FnGenInitInfo](@vchSet)", vchSetParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGenUserName")]
        public virtual IQueryable<FnGenUserName_Result> FnGenUserName(string vchSet)
        {
            var vchSetParameter = vchSet != null ?
                new ObjectParameter("vchSet", vchSet) :
                new ObjectParameter("vchSet", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGenUserName_Result>("[CERLEntities].[FnGenUserName](@vchSet)", vchSetParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGeneralDropDownList")]
        public virtual IQueryable<FnGeneralDropDownList_Result> FnGeneralDropDownList(string table, string pid)
        {
            var tableParameter = table != null ?
                new ObjectParameter("table", table) :
                new ObjectParameter("table", typeof(string));

            var pidParameter = pid != null ?
                new ObjectParameter("pid", pid) :
                new ObjectParameter("pid", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGeneralDropDownList_Result>("[CERLEntities].[FnGeneralDropDownList](@table, @pid)", tableParameter, pidParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGetAssignTo")]
        public virtual IQueryable<FnGetAssignTo_Result> FnGetAssignTo(Nullable<int> outState, string vchSet)
        {
            var outStateParameter = outState.HasValue ?
                new ObjectParameter("outState", outState) :
                new ObjectParameter("outState", typeof(int));

            var vchSetParameter = vchSet != null ?
                new ObjectParameter("vchSet", vchSet) :
                new ObjectParameter("vchSet", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetAssignTo_Result>("[CERLEntities].[FnGetAssignTo](@outState, @vchSet)", outStateParameter, vchSetParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGetCaseID")]
        public virtual IQueryable<FnGetCaseID_Result> FnGetCaseID(string vchSet)
        {
            var vchSetParameter = vchSet != null ?
                new ObjectParameter("vchSet", vchSet) :
                new ObjectParameter("vchSet", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetCaseID_Result>("[CERLEntities].[FnGetCaseID](@vchSet)", vchSetParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGetDivShow")]
        public virtual IQueryable<FnGetDivShow_Result> FnGetDivShow(string testitemid)
        {
            var testitemidParameter = testitemid != null ?
                new ObjectParameter("testitemid", testitemid) :
                new ObjectParameter("testitemid", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetDivShow_Result>("[CERLEntities].[FnGetDivShow](@testitemid)", testitemidParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGetFCERL")]
        public virtual IQueryable<FnGetFCERL_Result> FnGetFCERL(string fID, string badgeCode)
        {
            var fIDParameter = fID != null ?
                new ObjectParameter("fID", fID) :
                new ObjectParameter("fID", typeof(string));

            var badgeCodeParameter = badgeCode != null ?
                new ObjectParameter("BadgeCode", badgeCode) :
                new ObjectParameter("BadgeCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetFCERL_Result>("[CERLEntities].[FnGetFCERL](@fID, @BadgeCode)", fIDParameter, badgeCodeParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGetFormAuthority2")]
        public virtual IQueryable<FnGetFormAuthority2_Result> FnGetFormAuthority2(string userId)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetFormAuthority2_Result>("[CERLEntities].[FnGetFormAuthority2](@UserId)", userIdParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGetItemParentListById")]
        public virtual IQueryable<FnGetItemParentListById_Result> FnGetItemParentListById(string id)
        {
            var idParameter = id != null ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetItemParentListById_Result>("[CERLEntities].[FnGetItemParentListById](@Id)", idParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGetMailList")]
        public virtual IQueryable<FnGetMailList_Result> FnGetMailList(string assignTo)
        {
            var assignToParameter = assignTo != null ?
                new ObjectParameter("AssignTo", assignTo) :
                new ObjectParameter("AssignTo", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetMailList_Result>("[CERLEntities].[FnGetMailList](@AssignTo)", assignToParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGetMailList2")]
        public virtual IQueryable<FnGetMailList2_Result> FnGetMailList2(string memberRole)
        {
            var memberRoleParameter = memberRole != null ?
                new ObjectParameter("MemberRole", memberRole) :
                new ObjectParameter("MemberRole", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetMailList2_Result>("[CERLEntities].[FnGetMailList2](@MemberRole)", memberRoleParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnGetParentListById")]
        public virtual IQueryable<FnGetParentListById_Result> FnGetParentListById(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetParentListById_Result>("[CERLEntities].[FnGetParentListById](@Id)", idParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnMemberDropDownList")]
        public virtual IQueryable<FnMemberDropDownList_Result> FnMemberDropDownList(string table, string pid)
        {
            var tableParameter = table != null ?
                new ObjectParameter("table", table) :
                new ObjectParameter("table", typeof(string));

            var pidParameter = pid != null ?
                new ObjectParameter("pid", pid) :
                new ObjectParameter("pid", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnMemberDropDownList_Result>("[CERLEntities].[FnMemberDropDownList](@table, @pid)", tableParameter, pidParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnRoleMenuDropDownList")]
        public virtual IQueryable<FnRoleMenuDropDownList_Result> FnRoleMenuDropDownList(string roleid)
        {
            var roleidParameter = roleid != null ?
                new ObjectParameter("roleid", roleid) :
                new ObjectParameter("roleid", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnRoleMenuDropDownList_Result>("[CERLEntities].[FnRoleMenuDropDownList](@roleid)", roleidParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnRoleMenuFunction")]
        public virtual IQueryable<FnRoleMenuFunction_Result> FnRoleMenuFunction(Nullable<int> roleId)
        {
            var roleIdParameter = roleId.HasValue ?
                new ObjectParameter("RoleId", roleId) :
                new ObjectParameter("RoleId", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnRoleMenuFunction_Result>("[CERLEntities].[FnRoleMenuFunction](@RoleId)", roleIdParameter);
        }

        [DbFunctionAttribute("CERLEntities", "FnTestItemMenuDropDownList")]
        public virtual IQueryable<FnTestItemMenuDropDownList_Result> FnTestItemMenuDropDownList(string pid)
        {
            var pidParameter = pid != null ?
                new ObjectParameter("pid", pid) :
                new ObjectParameter("pid", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnTestItemMenuDropDownList_Result>("[CERLEntities].[FnTestItemMenuDropDownList](@pid)", pidParameter);
        }
    }
}
