using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class SaveOrder : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["ComplemarBridge"].ConnectionString;
        con = new SqlConnection(connstring);

        string strOrderName="";
        string strOrderedByFirstName="";
        string strOrderedByLastName="";
        string strCustName="";
        ShipToAddress Address = new ShipToAddress();
        List<OrderItem> OrderItems = new List<OrderItem>();

        Response.Write("In Save order and Order Details");


        if (Request.Form["OrderName"]  != null )
        {
            strOrderName = Request.Form["OrderName"].ToString();
            Response.Write("OrderName: " + Request.Form["OrderName"] + "<br/> ");
        }

        if (Request.Form["OrderedByFirstName"]  != null )
        {
            strOrderedByFirstName = Request.Form["OrderedByFirstName"].ToString();
	        Response.Write("OrderedByFirstName: " + Request.Form["OrderedByFirstName"] +"<br/> ");
        }

        if (Request.Form["OrderedByLastName"]  != null )
        {
            strOrderedByLastName = Request.Form["OrderedByLastName"].ToString();
	        Response.Write("OrderedByLastName: " + Request.Form["OrderedByLastName"] +"<br/> ");
        }

        if (Request.Form["ShipToFirstName"]  != null )
        {
            Address.FirstName = Request.Form["ShipToFirstName"].ToString();
	        Response.Write("ShipToFirstName: " + Request.Form["ShipToFirstName"] +"<br/> ");
        }

        if (Request.Form["ShipToLastName"]  != null )
        {
            Address.LastName = Request.Form["ShipToLastName"].ToString();
	        Response.Write("ShipToLastName: " + Request.Form["ShipToLastName"] +"<br/> ");
        }

        if (Request.Form["Address1"]  != null )
        {
            Address.StreetAddress1 = Request.Form["Address1"].ToString();
	        Response.Write("Address1: " + Request.Form["Address1"] +"<br/> ");
        }

        if (Request.Form["Address2"]  != null )
        {
            Address.StreetAddress2 = Request.Form["Address2"].ToString();
	        Response.Write("Address2: " + Request.Form["Address2"] +"<br/> ");
        }

        if (Request.Form["City"]  != null )
        {
            Address.City = Request.Form["City"].ToString();
	        Response.Write("City: " + Request.Form["City"] +"<br/> ");
        }

        if (Request.Form["State"]  != null )
        {
            Address.State = Request.Form["State"].ToString();
	        Response.Write("State: " + Request.Form["State"] +"<br/> ");
        }

        if (Request.Form["Zip"]  != null )
        {
            Address.Zip = Request.Form["Zip"].ToString();
	        Response.Write("Zip: " + Request.Form["Zip"] +"<br/> ");
        }

        if (Request.Form["Phone"]  != null )
        {
            Address.Phone = Request.Form["Phone"].ToString();
	        Response.Write("Phone: " + Request.Form["Phone"] +"<br/> ");
        }

        if (Request.Form["Email"]  != null )
        {
            Address.Email = Request.Form["Email"].ToString();
	        Response.Write("Email: " + Request.Form["Email"] +"<br/> ");
        }
        /*
        if (Request.Form["TransactionNo"]  != null )
        {
	        Response.Write("TransactionNo: " + Request.Form["TransactionNo"] +"<br/> ");
        }

        if (Request.Form["SequenceNo"]  != null )
        {
	        Response.Write("SequenceNo: " + Request.Form["SequenceNo"] +"<br/> ");
        }

        if (Request.Form["Status"]  != null )
        {
	        Response.Write("Status: " + Request.Form["Status"] +"<br/> ");
        }

        if (Request.Form["SubmittedOrderNo"]  != null )
        {
	        Response.Write("SubmittedOrderNo: " + Request.Form["SubmittedOrderNo"] +"<br/> ");
        }

        if (Request.Form["ProcessStatus"]  != null )
        {
	        Response.Write("ProcessStatus: " + Request.Form["ProcessStatus"] +"<br/> ");
        }

        if (Request.Form["ShippingStatus"]  != null )
        {
	        Response.Write("ShippingStatus: " + Request.Form["ShippingStatus"] +"<br/> ");
        }

        if (Request.Form["ShippingInfo"]  != null )
        {
	        Response.Write("ShippingInfo: " + Request.Form["ShippingInfo"] +"<br/> ");
        }
        */
        if (Request.Form["CustName"]  != null )
        {
            strCustName =  Request.Form["CustName"].ToString();
	        Response.Write("CustName: " + Request.Form["CustName"] +"<br/> ");
        }

        if (Request.Form["shipping_instruction"]  != null )
        {
            Address.ShippingInst = Request.Form["shipping_instruction"].ToString();
	        Response.Write("shipping_instruction: " + Request.Form["shipping_instruction"] +"<br/> ");
        }

        /*
        if (Request.Form["freight_billing_code"]  != null )
        {
	        Response.Write("freight_billing_code: " + Request.Form["freight_billing_code"] +"<br/> ");
        }
        */



        string[] OrdItmQtys = null;
        if (Request.Form.GetValues("OrdItmQty") != null)
        {
            OrdItmQtys = Request.Form.GetValues("OrdItmQty");
            Response.Write("Qty: " + OrdItmQtys.ToString() + "<br/> ");
        }
        OrderItem orditm = new OrderItem();
        int i = 0;
        Response.Write(" <br/>Order Items <br/>");
        if (OrdItmQtys != null)
        {
            foreach (string ordqty in OrdItmQtys)
            {
                orditm = new OrderItem();
                string[] items = ordqty.Split(new string[] { "#emerge#" }, StringSplitOptions.None);
                orditm.StockNumber = items[0].ToString();
                orditm.Quantity = Int32.Parse(items[1].ToString());
                OrderItems.Add(orditm);

                Response.Write("Item No: " + orditm.StockNumber + " and Quantity is " +  orditm.Quantity.ToString() + "<br/> ");
                i++;
            }
        }
        //strOrderName = strOrderName.Replace("IO", "Test");
        string strOrdMsg = CreateOrder(strOrderName, strOrderedByFirstName, strOrderedByLastName, strCustName, Address, OrderItems);


        /*
        string[] itemNos= null;
        if (Request.Form.GetValues("ItemNo") != null)
        {
            itemNos = Request.Form.GetValues("ItemNo");
            Response.Write("ItemNos: " + itemNos.ToString() + "<br/> ");

        }

        string[] quantities =null;
        if (Request.Form.GetValues("Quantity") != null)
        {
            quantities = Request.Form.GetValues("Quantity");
            Response.Write("Qty: " + quantities.ToString() + "<br/> ");
        }
        OrderItem orditm = new OrderItem();
        int i = 0;
        Response.Write(" <br/>Order Items <br/>");
        if (quantities != null)
        {
            foreach (var ordqty in quantities)
            {
                orditm = new OrderItem();
                orditm.StockNumber = itemNos[i].ToString();
                orditm.Quantity = Int32.Parse(quantities[i].ToString());
                OrderItems.Add(orditm);

                Response.Write("Item No: " + itemNos[i].ToString() + " and Quantity is " + quantities[i].ToString() + "<br/> ");
                i++;
            }
        }
        string strOrdMsg = CreateOrder(strOrderName, strOrderedByFirstName, strOrderedByLastName, strCustName, Address, OrderItems);
        */
    }


    public string CreateOrder(string OrderName, string OrderedByFirstName, string OrderedByLastName,  string Customer, ShipToAddress Address, List<OrderItem> OrderItems)
    {
        string strCmd = "EXEC dbo.CreateOrder @OrderName = '" + OrderName + "', ";
        try
        {
            strCmd += "@OrderByFirstName = '" + OrderedByFirstName + "', ";
            strCmd += "@OrderByLastName ='" + OrderedByLastName + "', ";
            strCmd += "@ShipToFirstName ='" + Address.FirstName + "', ";
            strCmd += "@ShipToLastName ='" + Address.LastName + "', ";
            strCmd += "@Address1 ='" + Address.StreetAddress1 + "', ";
            strCmd += "@Address2 ='" + Address.StreetAddress2 + "', ";
            strCmd += "@City ='" + Address.City + "',  ";
            strCmd += "@State ='" + Address.State + "', ";
            strCmd += "@Zip ='" + Address.Zip + "', ";
            strCmd += "@Phone ='" + Address.Phone + "', ";
            strCmd += "@Email ='" + Address.Email + "', ";
            strCmd += "@Customer ='" + Customer + "', ";
            strCmd += "@ShippingInstr ='" + Address.ShippingInst + "' ";

            DataRow drNewOrder = GetSingleRow(strCmd);
            string OrderID = drNewOrder[0].ToString();
            
            foreach (OrderItem objItem in OrderItems)
            {
                strCmd = "INSERT INTO dbo.tblOrderItem (fk_OrderDestination_ID, ItemNo, Quantity) VALUES(" + OrderID + ",'" + objItem.StockNumber + "'," + objItem.Quantity + ")";
                int rowsUpdated = ExecuteSQL(strCmd);
            }

            string strMessage = "";
            strMessage += "Received Order: " + OrderName + " with the following details:<br/>";
            strMessage += "Ship To Address: <br/>";
            strMessage += Address.FirstName + " " + Address.LastName + "<br/>";
            strMessage += Address.StreetAddress1 + " " + Address.StreetAddress1 + "<br/>";
            strMessage += Address.City + " " + Address.State + "-" + Address.Zip + "<br/>";
            strMessage += "Country: " + Address.Country + "<br/><br/>";

            strMessage += "Order Items:<br/>";
            foreach (OrderItem objItem in OrderItems)
            {
                strMessage += "ItemNo: " + objItem.StockNumber + " | <b>" + objItem.Quantity.ToString() + "</b></br>";
            }


            return strMessage;
        }
        catch (Exception ex)
        {
            return strCmd;
        }
    }


    public DataRow GetSingleRow(string strCommand)
    {
        string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["ComplemarBridge"].ConnectionString;
        SqlConnection conn = new SqlConnection(connstring);
        conn.Open();
        SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
        System.Data.DataSet ds = new System.Data.DataSet();
        da.Fill(ds);
        conn.Close();
        da.Dispose();
        conn.Dispose();
        if (ds == null)
        {
            return null;
        }
        if (ds.Tables[0].Rows.Count == 0)
        {
            return null;
        }
        return ds.Tables[0].Rows[0];
    }

    public static int ExecuteSQL(string strCommand)
    {
        SqlConnection conn = null;
        try
        {
            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["ComplemarBridge"].ConnectionString;
            conn = new SqlConnection(connstring);
            conn.Open();
            SqlCommand sqlcmd = new SqlCommand(strCommand, conn);
            int intRowsAffected = sqlcmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            return intRowsAffected;
        }
        catch (SqlException exSql)
        {
            conn.Close();
            conn.Dispose();
            throw exSql;
        }
        catch (Exception ex)
        {
            conn.Close();
            conn.Dispose();
            throw ex;
        }
    }
    /*
    public DataSet ExecuteSQL(string strCommand)
        {
            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["ComplemarBridge"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            da.SelectCommand.CommandTimeout = 0;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();
            da.Dispose();
            conn.Dispose();
            return ds;
        }
     * */
}

    public class ShipToAddress
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }

        public string AddressTypeName { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ShippingInst { get; set; }
    }

    public class OrderItem
    {
        public string StockNumber { get; set; }
        public int Quantity { get; set; }
    }

