using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace einvoice.Models.eInvoiceMessage
{
    public class C0401
    {
        public clsMain Main;
        public clsDetail Details;
        public clsAmount Amount;
        public C0401()
        {

        }
    }

    public class clsMain
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceTime { get; set; }
        public Seller Seller { get; set; }
        public Buyer Buyer { get; set; }
        public string InvoiceType { get; set; }
        public string DonateMark { get; set; }
        public string CarrierType { get; set; }
        public string CarrierId1 { get; set; }
        public string CarrierId2 { get; set; }
        public string PrintMark { get; set; }
        public string NPOBAN { get; set; }
        public string RandomNumber { get; set; }
    }

    public abstract class Person
    {
        public  string Identifier { get; set; }
        public  string Name { get; set; }
        public  string Address { get; set; }
        public  string PersonInCharge { get; set; }
        public  string TelephoneNumber { get; set; }
        public  string FacsimileNumber { get; set; }
        public  string EmailAddress { get; set; }
        public  string CustomerNumber { get; set; }
        public  string RoleRemark { get; set; }
    }

    public class Seller: Person
    {
        public Seller():base()
        {

        }
    }

    public class Buyer:Person
    {
        public Buyer():base()
        {

        }
    }

    public class clsDetail
    {
        public clsProductItem ProductItem { get; set; }
    }

    public class clsProductItem
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int Amount { get; set; }
        public int SequenceNumber { get; set; }
    }

    public class clsAmount
    {
        public int SalesAmount { get; set; }
        public int FreeTaxSalesAmount { get; set; }
        public int ZeroTaxSalesAmount { get; set; }
        public int TaxType { get; set; }
        public decimal TaxRate { get; set; }
        public int TaxAmount { get; set; }
        public int TotalAmount { get; set; }

    }
}