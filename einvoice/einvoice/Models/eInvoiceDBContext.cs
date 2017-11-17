using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using einvoice.Models;
using System.Data.Entity.Validation;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace einvoice.Models
{
    public class eInvoiceDBContext:DbContext
    {
        public DbSet<TURNKEY_MESSAGE_LOG> TurnkeyMessageLog { get; set; }
        public DbSet<TURNKEY_MESSAGE_LOG_DETAIL> TurnKeyMessageLogDetail { get; set; }
        public DbSet<TURNKEY_SYSEVENT_LOG> TurnKeySyseventLog { get; set; }

        public eInvoiceDBContext():base("eInvoiceDBContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, "[SaveChanges] The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}