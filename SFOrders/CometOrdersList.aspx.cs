using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using Obout.Grid;
using System.Collections;
using Obout.Interface;


public partial class CometOrdersList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["ComplemarBridge"].ConnectionString;
        con = new SqlConnection(connstring);
        try
        {
            
            if (!IsPostBack)
            {
            
            }
        }
        catch (Exception ex)
        {
            lblServerError.Visible = true;
            lblServerError.Text = ex.Message;
        }
    }

    protected void OrderDestGrid_ColumnsCreated(object sender, EventArgs e)
    {
        Grid grid = sender as Grid;
        int i = 0;
        foreach (Column column in grid.Columns)
        {
            if (i == 0) // OrderDestination Name
            {
                column.TemplateSettings.TemplateId = "EmptyTemplate";
            }
            i++;
        }
    }
    protected void OrderDestGrid_RowDataBound(object sender, GridRowEventArgs e)
    {
        Grid grid = sender as Grid;
        Label lbl;
        int i = 0;
        if (e.Row.RowType == GridRowType.DataRow)
        {
            foreach (Column column in grid.Columns)
            {
                lbl = new Label();
                lbl.ID = "DPName" + e.Row.RowIndex.ToString() + i.ToString();

                if (i == 0) // 0 - OrderDestination Name
                {
                    lbl.Text = e.Row.Cells[i].Text;
                    e.Row.Cells[i].Controls.Add(new LiteralControl("<div class='ob_gCc2' Style='margin-left: 20px !important;'>"));
                    e.Row.Cells[i].Controls.Add(lbl);
                    e.Row.Cells[i].Controls.Add(new LiteralControl("</div></div>"));
                }
                i++;
            }
        }
    }
    protected void Grid_RowCreated(object sender, GridRowEventArgs e)
    {

    }
    protected void OrderDestForm_DataBound(object sender, EventArgs e)
    {
    }
    protected void UpdateOrderDestination(object sender, GridRecordEventArgs e)
    {
        string pk_OrderDestination_ID = e.Record["pk_OrderDestination_ID"].ToString();
        string Address1 = e.Record["Address1"].ToString();
        string City = e.Record["City"].ToString();
        string State = e.Record["State"].ToString();
        string Zip = e.Record["Zip"].ToString();
        string TransactionNo = e.Record["TransactionNo"].ToString();
        string Status = e.Record["Status"].ToString();
        string ProcessStatus = e.Record["ProcessStatus"].ToString();
        string CustName = e.Record["CustName"].ToString();
        string shipping_instruction = e.Record["shipping_instruction"].ToString();
        SaveOrderDestination(Convert.ToInt32(pk_OrderDestination_ID), Address1, City, State, Zip, TransactionNo, Status, ProcessStatus, CustName, shipping_instruction);
        OrderDestForm.Visible = false;
    }

    protected void InsertOrderDestination(object sender, GridRecordEventArgs e)
    {
        string pk_OrderDestination_ID = "0";
        string Address1 = e.Record["Address1"].ToString();
        string City = e.Record["City"].ToString();
        string State = e.Record["State"].ToString();
        string Zip = e.Record["Zip"].ToString();
        string TransactionNo = e.Record["TransactionNo"].ToString();
        string Status = e.Record["Status"].ToString();
        string ProcessStatus = e.Record["ProcessStatus"].ToString();
        string CustName = e.Record["CustName"].ToString();
        string shipping_instruction = e.Record["shipping_instruction"].ToString();
        SaveOrderDestination(Convert.ToInt32(pk_OrderDestination_ID), Address1, City, State, Zip, TransactionNo, Status, ProcessStatus, CustName, shipping_instruction);
        OrderDestForm.Visible = false;
    }



    public bool SaveOrderDestination(Int32 pk_OrderDestination_ID, string Address1, string City, string State, string Zip, string TransactionNo, string Status, string ProcessStatus, string CustName, string shipping_instruction)
    {
        try
        {
            string strCommand = "EXEC dbo.SP_SaveOrderDest ";
            strCommand += " @pk_OrderDestination_ID =" + pk_OrderDestination_ID.ToString();
            strCommand += " ,@Address1 ='" + Address1.ToString() + "'";
            strCommand += " ,@City ='" + City.ToString() + "'";
            strCommand += " ,@State ='" + State.ToString() + "'";
            strCommand += " ,@Zip ='" + Zip.ToString() + "'";
            strCommand += " ,@TransactionNo ='" + TransactionNo.ToString() + "'";
            strCommand += " ,@Status ='" + Status.ToString() + "'";
            strCommand += " ,@ProcessStatus ='" + ProcessStatus.ToString() + "'";
            strCommand += " ,@CustName ='" + CustName.ToString() + "'";
            strCommand += " ,@shipping_instruction ='" + shipping_instruction.ToString() + "'";
            ExecuteSQL(strCommand);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    
   

    protected void UpdateOrderItems(object sender, GridRecordEventArgs e)
    {
        string pk_OrderItem_ID = e.Record["pk_OrderItem_ID"].ToString();
        string pk_OrderDestination_ID = e.Record["pk_OrderDestination_ID"].ToString();
        string ItemNo = e.Record["ItemNo"].ToString();
        string Quantity = e.Record["Quantity"].ToString();
        SaveOrderItems(Convert.ToInt32(pk_OrderItem_ID), Convert.ToInt32(pk_OrderDestination_ID), ItemNo, Quantity);
    }

    protected void InsertOrderItems(object sender, GridRecordEventArgs e)
    {
        string pk_OrderItem_ID = "0";
        string pk_OrderDestination_ID = e.Record["pk_OrderDestination_ID"].ToString();
        string ItemNo = e.Record["ItemNo"].ToString();
        string Quantity = e.Record["Quantity"].ToString();
        SaveOrderItems(Convert.ToInt32(pk_OrderItem_ID), Convert.ToInt32(pk_OrderDestination_ID), ItemNo, Quantity);
    }

    public bool SaveOrderItems(Int32 pk_OrderItem_ID, Int32 pk_OrderDestination_ID, string ItemNo, string Quantity)
    {
        try
        {
            string strCommand = "EXEC dbo.SP_SaveOrderItems ";
            strCommand += " @pk_OrderItem_ID =" + pk_OrderItem_ID.ToString();
            strCommand += " ,@pk_OrderDestination_ID =" + pk_OrderDestination_ID.ToString();
            strCommand += " ,@ItemNo ='" + ItemNo.ToString() + "'";
            strCommand += " ,@Quantity =" + Quantity.ToString();

            ExecuteSQL(strCommand);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


    protected void DeleteOrderDestination(object sender, GridRecordEventArgs e)
    {
        string OrderDest_Id = "";

        if (e.RecordsCollection != null)
        {
            foreach (Hashtable oRecord in e.RecordsCollection)
            {
                OrderDest_Id = oRecord["OrderDestination_Id"].ToString();
                DeleteOrderDestination(Convert.ToInt32(OrderDest_Id));
            }
        }
    }

    public bool DeleteOrderDestination(Int32 OrderDest_Id)
    {
        try
        {
            string strCommand = "EXEC dbo.SP_DeleteOrderDestination ";
            strCommand += " @OrderDest_Id =" + OrderDest_Id.ToString();
            ExecuteSQL(strCommand);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


    protected void DeleteOrderItems(object sender, GridRecordEventArgs e)
    {
        string pk_OrderItem_ID = "";

        if (e.RecordsCollection != null)
        {
            foreach (Hashtable oRecord in e.RecordsCollection)
            {
                pk_OrderItem_ID = oRecord["pk_OrderItem_ID"].ToString();
                DeleteOrderItems(Convert.ToInt32(pk_OrderItem_ID));
            }
        }
    }

    public bool DeleteOrderItems(Int32 pk_OrderItem_ID)
    {
        try
        {
            string strCommand = "EXEC dbo.SP_DeleteOrderItem ";
            strCommand += " @pk_OrderItem_ID=" + pk_OrderItem_ID.ToString();
            ExecuteSQL(strCommand);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
   
    public int ExecuteSQL(string strCommand)
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
    public DataSet ExecuteSQL_ds(string strCommand)
    {
        string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["RedBullConnectionString"].ConnectionString;
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

 

 }
