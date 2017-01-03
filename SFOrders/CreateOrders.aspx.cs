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

public partial class CreateOrders : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["ComplemarBridge"].ConnectionString;
        con = new SqlConnection(connstring);
    }

    public void refress()
    {
        OrderName.Text = "";
        OrderedByFirstName.Text = "";
        OrderedByLastName.Text = "";
        ShipToFirstName.Text = "";
        ShipToLastName.Text = "";
        Address1.Text = "";
        Address2.Text = "";
        City.Text = "";
        State.Text = "";
        Zip.Text = "";
        Phone.Text = "";
        Email.Text = "";
        TransactionNo.Text = "";
        SequenceNo.Text = "";
        Status.Text = "";
        SubmittedOrderNo.Text = "";
        ProcessStatus.Text = "";
        ShippingStatus.Text = "";
        ShippingInfo.Text = "";
        CustName.Text = "";
        shipping_instruction.Text = "";
        freight_billing_code.Text = "";

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Response.Write("Saving Order");
       
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        refress();
    }
}