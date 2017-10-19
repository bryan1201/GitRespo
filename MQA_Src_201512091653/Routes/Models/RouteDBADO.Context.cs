//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Routes.Models
{
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    public partial class RouteDBEntities : DbContext
    {
        public RouteDBEntities()
            : base("name=RouteDBEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }


        [DbFunctionAttribute("RouteDBEntities", "FnFlowPath")]
        public virtual IQueryable<FnFlowPath_Result> FnFlowPath(Nullable<int> flowCode, string action)
        {
            var flowCodeParameter = flowCode.HasValue ?
                new ObjectParameter("FlowCode", flowCode) :
                new ObjectParameter("FlowCode", typeof(int));

            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnFlowPath_Result>("[RouteDBEntities].[FnFlowPath](@FlowCode, @Action)", flowCodeParameter, actionParameter);
        }

        [DbFunctionAttribute("RouteDBEntities", "FnFlowPathTreeList")]
        public virtual IQueryable<FnFlowPathTreeList_Result> FnFlowPathTreeList(Nullable<int> flowCode, Nullable<int> inState, string action)
        {
            var flowCodeParameter = flowCode.HasValue ?
                new ObjectParameter("FlowCode", flowCode) :
                new ObjectParameter("FlowCode", typeof(int));

            var inStateParameter = inState.HasValue ?
                new ObjectParameter("inState", inState) :
                new ObjectParameter("inState", typeof(int));

            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnFlowPathTreeList_Result>("[RouteDBEntities].[FnFlowPathTreeList](@FlowCode, @inState, @Action)", flowCodeParameter, inStateParameter, actionParameter);
        }

        [DbFunctionAttribute("RouteDBEntities", "FnGetFormAction")]
        public virtual IQueryable<FnGetFormAction_Result> FnGetFormAction(Nullable<int> flowCode, Nullable<int> inState)
        {
            var flowCodeParameter = flowCode.HasValue ?
                new ObjectParameter("FlowCode", flowCode) :
                new ObjectParameter("FlowCode", typeof(int));

            var inStateParameter = inState.HasValue ?
                new ObjectParameter("inState", inState) :
                new ObjectParameter("inState", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetFormAction_Result>("[RouteDBEntities].[FnGetFormAction](@FlowCode, @inState)", flowCodeParameter, inStateParameter);
        }

        [DbFunctionAttribute("RouteDBEntities", "FnGetTask")]
        public virtual IQueryable<FnGetTask_Result> FnGetTask(string fID, Nullable<int> state, string applicant)
        {
            var fIDParameter = fID != null ?
                new ObjectParameter("fID", fID) :
                new ObjectParameter("fID", typeof(string));

            var stateParameter = state.HasValue ?
                new ObjectParameter("State", state) :
                new ObjectParameter("State", typeof(int));

            var applicantParameter = applicant != null ?
                new ObjectParameter("Applicant", applicant) :
                new ObjectParameter("Applicant", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetTask_Result>("[RouteDBEntities].[FnGetTask](@fID, @State, @Applicant)", fIDParameter, stateParameter, applicantParameter);
        }

        [DbFunctionAttribute("RouteDBEntities", "FnGetTaskDetail")]
        public virtual IQueryable<FnGetTaskDetail_Result> FnGetTaskDetail(string taskID)
        {
            var taskIDParameter = taskID != null ?
                new ObjectParameter("TaskID", taskID) :
                new ObjectParameter("TaskID", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetTaskDetail_Result>("[RouteDBEntities].[FnGetTaskDetail](@TaskID)", taskIDParameter);
        }

        [DbFunctionAttribute("RouteDBEntities", "udf_SplitText2Table")]
        public virtual IQueryable<udf_SplitText2Table_Result> udf_SplitText2Table(string data, string delimiter)
        {
            var dataParameter = data != null ?
                new ObjectParameter("data", data) :
                new ObjectParameter("data", typeof(string));

            var delimiterParameter = delimiter != null ?
                new ObjectParameter("delimiter", delimiter) :
                new ObjectParameter("delimiter", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<udf_SplitText2Table_Result>("[RouteDBEntities].[udf_SplitText2Table](@data, @delimiter)", dataParameter, delimiterParameter);
        }

        [DbFunctionAttribute("RouteDBEntities", "FnGetFlowStateList")]
        public virtual IQueryable<FnGetFlowStateList_Result> FnGetFlowStateList(Nullable<int> flowCode)
        {
            var flowCodeParameter = flowCode.HasValue ?
                new ObjectParameter("FlowCode", flowCode) :
                new ObjectParameter("FlowCode", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetFlowStateList_Result>("[RouteDBEntities].[FnGetFlowStateList](@FlowCode)", flowCodeParameter);
        }
    }
}
