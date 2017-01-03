using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACE;
using Lumen;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;

namespace Lumen
{
    public class AdminServices
    {
        // *************** PROGRAM Services *************************
        public static int GetItemCountInProgram(string strProgramID)
        {
            string strCommand = "select COUNT(*) from Emerge.tblProgramItem where fk_Program_Id = " + strProgramID;
            DataRow dr = DataOperations.GetSingleRow(strCommand);
            return Convert.ToInt32(dr[0]);
        }

        public static DataTable GetPrograms()
        {
            return DataOperations.GetDataTable("SELECT * FROM Emerge.tblProgram");
        }

        public static DataTable GetMarketPeriods()
        {
            return DataOperations.GetDataTable("SELECT DISTINCT MarketPeriod FROM Emerge.tblProgram");
        }

        public static bool DeleteProgram(string strProgramID, out string strResult)
        {            
            int RowsDeleted = DataOperations.ExecuteSQL("DELETE Emerge.tblProgram WHERE pk_Program_Id = " + strProgramID, out strResult);
            return true;
        }

        public static bool SaveProgram(string strProgramID, string strName, string strDescription, string strImageFileName, string strStartDate, string strEndDate, string strInMarketDate)
        {
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            string strCommand = "";
            strCommand += "EXEC Lumen.SaveProgram @ProgramID = " + strProgramID + ",";
            strCommand += "@Name = '" + strName.Replace("'","''") + "',";
            strCommand += "@Description = '" + strDescription.Replace("'", "''") + "',";
            strCommand += "@PreviewImageFileName = '" + strImageFileName + "',";
            strCommand += "@StartDate = '" + strStartDate + "',";
            strCommand += "@EndDate = '" + strEndDate + "',";
            strCommand += "@InMarketDate= '" + strInMarketDate + "',";
            strCommand += "@UpdatedBy = '" + strUserID + "'";

            int RowsAffected = DataOperations.ExecuteSQL(strCommand);
            return Convert.ToBoolean(RowsAffected);
        }


        public static DataTable GetItemsInProgram(string strProgramID)
        {
            string strCommand = "SELECT pk_Item_ID ItemID, i.PreviewImageFileName PreviewImage, i.Item_Name Item,  b.Brand,  Dimension, UnitQuantity PackOf, EstimatedPrice Price from Emerge.tblItem i ";
            strCommand += "left join Emerge.tblBrand b on i.fk_BrandId = b.pk_Brand_id ";            
            strCommand += "inner join Emerge.tblProgramItem ip on i.pk_item_id = ip.fk_Item_Id  ";
            strCommand += "WHERE ip.fk_program_id = " + strProgramID;
            return DataOperations.GetDataTable(strCommand);
        }

        public static DataTable GetItemsAvailableForProgram(string strProgramID)
        {
            string strCommand = "SELECT pk_Item_ID ItemID, i.PreviewImageFileName PreviewImage, i.Item_Name Item, b.Brand, Dimension, UnitQuantity PackOf, EstimatedPrice Price from Emerge.tblItem i ";
            strCommand += "left join Emerge.tblBrand b on i.fk_BrandId = b.pk_Brand_id  ";
            strCommand += "WHERE pk_Item_ID NOT IN (select fk_item_id from Emerge.tblProgramItem where fk_program_id = " + strProgramID + ") AND Active=1 AND PreOrderItem = 1";
            return DataOperations.GetDataTable(strCommand);
        }
        // *************** **************** *************************

        // *************** ITEM Services *************************
        public static bool SavePOSItem(string strItemID, string strItemName, string strItemNo, string strEstimatedPrice, string strActualPrice,
                                            string strDimension, string strUnitQty, string strImageFileName, string strBrandID, string strItemTypeID, string strDescription, string strLowLevelInv, string strMaxOrdQty, string strPreOrderItem, string strInvOrderItem, string strAppRequired)
        {
            string strCmd = "";
            strItemName = strItemName.Replace("'", "''");
            strItemNo = strItemNo.Replace("'", "''");
            strDimension = strDimension.Replace("'", "''");
            strDescription = strDescription.Replace("'", "''");
            if (String.IsNullOrEmpty(strEstimatedPrice)) strEstimatedPrice = "NULL";
            if (String.IsNullOrEmpty(strActualPrice)) strActualPrice= "NULL";
            if (String.IsNullOrEmpty(strUnitQty)) strUnitQty = "NULL";
            if (String.IsNullOrEmpty(strImageFileName)) strImageFileName = "NoImage.jpg";
            if (String.IsNullOrEmpty(strBrandID)) strBrandID = "NULL";
            if (String.IsNullOrEmpty(strItemTypeID)) strItemTypeID = "NULL";
            if (String.IsNullOrEmpty(strLowLevelInv)) strLowLevelInv = "NULL";
            if (String.IsNullOrEmpty(strMaxOrdQty)) strMaxOrdQty = "NULL";
            if (String.IsNullOrEmpty(strPreOrderItem)) strPreOrderItem = "NULL";
            if (String.IsNullOrEmpty(strInvOrderItem)) strInvOrderItem = "NULL";
            if (String.IsNullOrEmpty(strInvOrderItem)) strAppRequired = "NULL";

            if (strItemID == "0")
            {
                strCmd = "INSERT INTO Emerge.tblItem (Item_Name, Item_No, EstimatedPrice, ActualPrice, Dimension, UnitQuantity, PreviewImageFileName, fk_BrandId, fk_ItemType_ID, [Description],LastUpdatedOn, fk_LastUpdatedBy, inventory_threshold, MaxOrderQuantity, PreOrderItem, InvOrdItem, approval_required) ";
                strCmd += "VALUES('" + strItemName + "', '" + strItemNo + "', " + strEstimatedPrice + ", " + strActualPrice + ", ";
                strCmd += " '" + strDimension + "', " + strUnitQty + ", '" + strImageFileName + "', " + strBrandID + ", " + strItemTypeID + ", ";
                strCmd += " '" + strDescription + "',GETDATE(),'" + Membership.GetUser().ProviderUserKey.ToString() + "', " + strLowLevelInv + ", " + strMaxOrdQty + ", " + strPreOrderItem + ", " + strInvOrderItem + ", " + strAppRequired+" )"; 

            }
            else
            {
                strCmd = "UPDATE Emerge.tblItem ";
                strCmd += "SET Item_Name='" + strItemName + "', ";
                strCmd += "Item_No='" + strItemNo + "', ";
                strCmd += "EstimatedPrice=" + strEstimatedPrice + ", ";
                strCmd += "ActualPrice=" + strActualPrice + ", ";
                strCmd += "Dimension='" + strDimension + "',";
                strCmd += "UnitQuantity=" + strUnitQty + ",";
                strCmd += "PreviewImageFileName='" + strImageFileName + "', ";
                strCmd += "fk_BrandId=" + strBrandID + ", ";
                strCmd += "fk_ItemType_Id=" + strItemTypeID + ", ";
                strCmd += "[Description]='" + strDescription + "',";
                strCmd += "LastUpdatedOn=GETDATE(), ";
                strCmd += "fk_LastUpdatedBy='" + Membership.GetUser().ProviderUserKey.ToString() + "', ";
                strCmd += "inventory_threshold=" + strLowLevelInv + ", ";
                strCmd += "MaxOrderQuantity=" + strMaxOrdQty + ", ";
                strCmd += "PreOrderItem='" + strPreOrderItem + "', ";
                strCmd += "InvOrdItem='" + strInvOrderItem + "', ";
                strCmd += "approval_required='" + strAppRequired + "' ";

                strCmd += "WHERE pk_Item_ID = " + strItemID ;
            }            

            int RowsAffected = DataOperations.ExecuteSQL(strCmd);
            return Convert.ToBoolean(RowsAffected);
        }


        

        // *************** Admin BUDGET Services *************************
        public static bool AssignbAddProgramBudgetToUser(string strUserID, string strProgramID, decimal Budget)
        {
            string strCmd = "EXEC Emerge.SP_AddBudget @UserID = '" + strUserID + "', @ProgramID =" + strProgramID + " , @Budget = " + Budget;
            int intRowsAffected = DataOperations.ExecuteSQL(strCmd);
            if (intRowsAffected == 1)
                return true;
            else
                return false;
        }
        // *************** **************** *************************

        public static bool IsUserInLevel(string strLevel)
        {
            string strCmd = "";
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            if (strLevel == "Level1")
                strCmd += " SELECT COUNT(*) AS Cnt FROM Emerge.tblSM_Level1 WHERE level1_sales_manager_id = '" + strUserID + "' ";
            if (strLevel == "Level2")
                strCmd += " SELECT COUNT(*) AS Cnt FROM Emerge.tblSM_Level2 WHERE level2_sales_manager_id = '" + strUserID + "' ";
            if (strLevel == "Level3")
                strCmd += " SELECT COUNT(*) AS Cnt FROM Emerge.tblSM_Level3 WHERE level3_sales_manager_id = '" + strUserID + "' ";
            DataRow drUserLevel = DataOperations.GetSingleRow(strCmd);
            if (drUserLevel["Cnt"].ToString() == "0")
                return false;
            else
                return true;
        }
        public static bool IsUserInLevel(string strLevel, string strProgramId)
        {
            string strCmd = "";
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            if (strLevel == "Level1")
                strCmd += " SELECT COUNT(*) AS Cnt FROM Emerge.tblSM_Level1 WHERE (fk_Program_Id = "+strProgramId+") AND level1_sales_manager_id = '" + strUserID + "' ";
            if (strLevel == "Level2")
                strCmd += " SELECT COUNT(*) AS Cnt FROM Emerge.tblSM_Level2 WHERE (fk_Sm_Level1_id IN (SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramId + ")) AND  level2_sales_manager_id = '" + strUserID + "' ";
            if (strLevel == "Level3")
                strCmd += " SELECT COUNT(*) AS Cnt FROM Emerge.tblSM_Level3 WHERE (fk_Sm_Level2_id IN (SELECT pk_Sm_Level2_id FROM Emerge.tblSM_Level2 WHERE fk_Sm_Level1_id IN (SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramId + "))) AND level3_sales_manager_id = '" + strUserID + "' ";
            DataRow drUserLevel = DataOperations.GetSingleRow(strCmd);
            if (drUserLevel["Cnt"].ToString() == "0")
                return false;
            else
                return true;
        }

        public static bool IsUserAnApprover()
        {
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            return IsUserAnApprover(strUserID);
        }

        public static bool IsUserAnApprover(string strUserID)
        {
            string strCmd = "";

            strCmd += " SELECT COUNT(*) AS Cnt FROM Emerge.tblApprover WHERE fk_Approver = '" + strUserID + "' ";

            DataRow drUserLevel = DataOperations.GetSingleRow(strCmd);
            if (drUserLevel["Cnt"].ToString() == "0")
                return false;
            else
                return true;
        }






        // *************** **************** *************************

        /*
        public static DataSet getSeasonalProgramItemsByProgramIDList(string UserId, string ProgramIDList, string SeasonalOrderIds)
        {
            string strCommand = "EXEC Emerge.SP_getSeasonalProgramItemsByProgramID";
            strCommand += " @ProgramId_List = '" + ProgramIDList.ToString();
            strCommand += "', @UserId = '" + UserId.ToString();
            strCommand += "', @SeasonalOrderIds = '" + SeasonalOrderIds + "'";
            DataSet ds = DCEPOS2.Data.Operations.ExecuteSQL(strCommand);
            return ds;
        }

        public static DataSet getSeasonalOrderItemsForGridView(int SeasonalOrderId, string ListOfAddress, string ProgramIDList, string UserId, string Destination)
        {
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
            string strCommand = "EXEC Emerge.SP_getSeasonalOrderItemsForGridView ";
            strCommand += " @Seasonal_Order_Id = " + SeasonalOrderId.ToString();
            strCommand += " ,@Address_Id_List = '" + ListOfAddress.ToString() + "'";
            strCommand += " ,@passLocationName = '" + Destination.ToString() + "'";
            strCommand += " ,@Program_Id_List = '" + ProgramIDList.ToString() + "'";
            strCommand += " , @UserId = '" + UserId.ToString() + "'";
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                da.Fill(ds);
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(strCommand);
            }
            return ds;
        }

        public static DataSet ExistngOrderForSeasonalProgram(string UserId, string ListOfAddress, string strPreOrderID)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
            string strCommand = "EXEC Emerge.SP_ExistingOrderForSeasonalProg";
            strCommand += " @Seasonal_Order_id =" + Convert.ToInt16(strPreOrderID);
            strCommand += ", @Address_Id_List =" + ListOfAddress.ToString();
            strCommand += ", @UserId =" + UserId.ToString();
            SqlConnection conn = new SqlConnection();
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            return ds;
        }

        public static int SaveSeasonalOrder(clsSaveSeasonalOrder objSaveSeasonalOrder)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
                string strCommand = "EXEC Emerge.[SP_SaveSeasonalOrder] ";
                strCommand += "@UserId ='" + objSaveSeasonalOrder.UserId.ToString() + "', ";
                strCommand += "@Seasonal_Order_Id =" + objSaveSeasonalOrder.SeasonalOrderId.ToString();
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
                System.Data.DataSet ds = new System.Data.DataSet();
                da.Fill(ds);
                conn.Close();
                return Convert.ToInt32(ds.Tables[0].Rows[0]["pk_seasonal_order_id"].ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static void SaveSeasonalOrderPrograms(List<clsSaveSeasonalOrderProgram> listSaveSeasonalOrderProgram)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
                foreach (clsSaveSeasonalOrderProgram objSaveSeasonalOrderProgram in listSaveSeasonalOrderProgram)
                {
                    string strCommand = "EXEC Emerge.[SP_SaveSeasonalOrderPrograms] ";
                    strCommand += "@Seasonal_Order_Id =" + objSaveSeasonalOrderProgram.SeasonalOrderId.ToString();
                    strCommand += ", @ProgramId =" + objSaveSeasonalOrderProgram.ProgramId.ToString();
                    DCEPOS2.Data.Operations.ExecuteSQL(strCommand);
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public static void SaveSeasonalOrderDestinationItems(List<clsSaveSeasonalOrderDestinationItem> listSaveSeasonalOrderDestinationItem)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
                foreach (clsSaveSeasonalOrderDestinationItem objSaveSeasonalOrderDestinationItem in listSaveSeasonalOrderDestinationItem)
                {
                    string strCommand = "EXEC Emerge.[SP_SaveSeasonalOrderDestinationItems] ";
                    strCommand += "@Seasonal_Order_Id =" + objSaveSeasonalOrderDestinationItem.SeasonalOrderId.ToString();
                    strCommand += ", @Address_Id =" + objSaveSeasonalOrderDestinationItem.AddressId.ToString();
                    strCommand += ", @UserId ='" + objSaveSeasonalOrderDestinationItem.UserId.ToString() + "'";
                    strCommand += ", @Item_Id =" + objSaveSeasonalOrderDestinationItem.ItemId.ToString();
                    strCommand += ", @Quantity =" + objSaveSeasonalOrderDestinationItem.Quantity.ToString();

                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
                    System.Data.DataSet ds = new System.Data.DataSet();
                    da.Fill(ds);
                    conn.Close();
                    da.Dispose();
                    conn.Dispose();
                }
                //return true;
            }
            catch (Exception ex)
            {
                //return false;
            }
        }
               
        public static DataSet getSeasonalOrderDestinationItemDetails(int SeasonalOrderId, int ItemId, string AddressIdList)
        {
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
            string strCommand = "EXEC Emerge.SP_getSeasonalOrderDestinationItemDetails";
            strCommand += " @Seasonal_Order_Id = " + SeasonalOrderId.ToString();
            strCommand += ", @Item_Id = " + ItemId.ToString();
            strCommand += ", @Address_Id_List ='" + AddressIdList.ToString() + "'";
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();

            return ds;

        }
        
        public static DataSet GetSeasonalOrderList(string UserId)
        {
            string strCommand = "EXEC Emerge.SP_getSeasonalOrders";
            strCommand += "  @UserId ='" + UserId.ToString() + "'";
            DataSet ds = DCEPOS2.Data.Operations.ExecuteSQL(strCommand);
            return ds;
                //string strCommand = "EXEC Emerge.SP_getSeasonalOrderListByUserId";
                //strCommand += "  @UserId ='" + UserId.ToString() + "'";
                //strCommand += " , @selectedProgramId = " + ProgramID.ToString();
                //strCommand += " , @selectedDistributorId = " + DestinationId.ToString();
        }

        public static string getSeasonalProgramListByOrderID(int SeasonalOrderId)
        {
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
            string strCommand = "EXEC Emerge.SP_getSeasonalProgramListByOrderID";
            strCommand += " @Seasonal_Order_Id ='" + SeasonalOrderId.ToString() + "'";
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();

            return ds.Tables[0].Rows[0][0].ToString();

        }

        
        //Added by Deepak
        public static string GetOpenSeasonalOrderID(string ProgramId)
        {
            string userid = Membership.GetUser().ProviderUserKey.ToString();
            string strCommand = "select * from Emerge.tblSeasonal_Order SO ";
            strCommand+= "join Emerge.tblSeasonal_Order_Program SOP ";
	        strCommand+= "on SOP.fk_seasonal_order_id = SO.pk_seasonal_order_id ";
            strCommand += "WHERE order_status = 'OPEN' and fk_program_id = " + ProgramId + " ";
            strCommand += "AND fk_UserID = '" + userid + "'";
            DataSet ds = DCEPOS2.Data.Operations.ExecuteSQL(strCommand);
            if (ds.Tables[0].Rows.Count == 0)
                return "0";  //There is no Seasonal Order in OPEN status
            else           
                return Convert.ToString(ds.Tables[0].Rows[0][0]);        
        }

        //Commented out by Deepak as it is not needed in JFW
        //Added by Kishan        
        ////////////////public static string getSeasonalOrdersListByProgramIDs(string ProgramIds, string UserId)
        ////////////////{
        ////////////////    string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
        ////////////////    string strCommand = "EXEC Emerge.SP_getSeasonalOrdersListByProgramIDs";
        ////////////////    strCommand += " @Program_Id ='" + ProgramIds.ToString() + "'";
        ////////////////    strCommand += ", @UserId ='" + UserId.ToString() + "'"; 
        ////////////////    SqlConnection conn = new SqlConnection(ConnectionString);
        ////////////////    conn.Open();
        ////////////////    SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
        ////////////////    System.Data.DataSet ds = new System.Data.DataSet();
        ////////////////    da.Fill(ds);
        ////////////////    conn.Close();

        ////////////////    return ds.Tables[0].Rows[0][0].ToString();
        ////////////////}

        public static DataSet GetDistributor(string UserId)
        {
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
            string strCommand = "ExEC Emerge.sp_getDistributor";
            strCommand += " @UserId ='" + UserId.ToString() + "'"; 
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();
            return ds;
        }

        public static List<clsDashboardProgram> RetrieveDashboardPrograms()
        {
            EmergeDataContext dc = new EmergeDataContext();
            List<clsDashboardProgram> ActivePrograms =
                    (from Program in dc.tblPrograms
                     where Program.PreOrderStartDate <= DateTime.Now && Convert.ToDateTime(Program.PreOrderEndDate).AddDays(1) >= DateTime.Now
                     select new clsDashboardProgram
                     {
                         ProgramId = Program.pk_ProgramId,
                         ProgramName = Program.Name,
                         ProgramDescription = Program.Description,
                         CloseDate = Convert.ToDateTime(Program.PreOrderEndDate),
                         InMarketDate = Convert.ToDateTime(Program.InMarketDate),
                         ImageFileName = Program.PreviewImageFileName
                     }).ToList();


            return ActivePrograms.OrderBy(AP => AP.InMarketDate).ToList();
        }

        //Get Program Name by SeasonalOrderId .  Added by Manjunatha.C
        public static string getProgramNameBySeasonalOrderID(int nSeasonalOrderId)
        {
           
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
            string strCommand = "EXEC [Emerge].[SP_getProgramNameBySeasonalOrderID] ";
            strCommand += " @SeasonalOrderID =" + nSeasonalOrderId.ToString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();
            return ds.Tables[0].Rows[0]["Name"].ToString();
        }

        //Updated by Deepak
        public static void ApproveSeasonalOrderByOrderID(int nSeasonalOrderId)
        {
            string strCommand = "EXEC Emerge.SP_UpdateApprovedSeasonalOrderBySeasonalOrderId  ";
            strCommand += " @SeasonalOrderID = " + nSeasonalOrderId.ToString();
            System.Data.DataSet ds = DCEPOS2.Data.Operations.ExecuteSQL(strCommand);
        }

        public static string GetStatusOfOrderBySeasonalOrderID(int nSeasonalOrderId)
        {
            string orderStatus = "";
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
            string strCommand = "EXEC [Emerge].[SP_getStatusOfSeasonalOrderBySeasonalOrderId]";
            strCommand += " @SeasonalOrderID = " + nSeasonalOrderId.ToString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();
            orderStatus = ds.Tables[0].Rows[0]["order_status"].ToString();
            return orderStatus;
        }


        // Email Services

        public static DataTable GetApprovedOrderItemDetails(int SeasonalOrderID)
        {
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
            string strCommand = "EXEC [Emerge].[SP_GetApprovedOrderItemDetailsBySeasonalOrderID] ";
            strCommand += " @SeasonalOrderID = " + SeasonalOrderID.ToString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();
            return ds.Tables[0];
        }

        public static string GetApprovedOrderProgramName(int SeasonalOrderID)
        {
            try
            {
                string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
                string strCommand = "EXEC [Emerge].[SP_GetSeasonalProgramNameBySeasonalOrderID] ";
                strCommand += " @SeasonalOrderID = " + SeasonalOrderID.ToString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
                System.Data.DataSet ds = new System.Data.DataSet();
                da.Fill(ds);
                conn.Close();
                return ds.Tables[0].Rows[0]["ProgramName"].ToString();
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string GetEmailFormat(int SeasonalOrderID, string ProviderUserKey)
        {
            string strEmailBody = "";
            string strProgramName = "";
            strProgramName = GetApprovedOrderProgramName(SeasonalOrderID);
            DataTable dtApproItemDetails = GetApprovedOrderItemDetails(SeasonalOrderID);
            Guid userId = Guid.Parse(ProviderUserKey);
            MembershipUser mu = Membership.GetUser(userId);
            strEmailBody += "Dear " + mu.UserName.ToString();
            strEmailBody += "<br/><br/>";
            strEmailBody += "Please find the details of your order # " + SeasonalOrderID.ToString() + ".";
            strEmailBody += "<br/><br/>";
            strEmailBody += "<b>Submitted Order Details</b>";
            strEmailBody += "<br/><br/>";
            strEmailBody += "<table border='1px solid' style='border: 1px solid;border-collapse: collapse;padding:3px;'>";
            strEmailBody += "<tr><td>Status</td><td>Submitted</td></tr>";
            strEmailBody += "<tr><td>Program Name</td><td>" + strProgramName.ToString() + "</td></tr>";
            strEmailBody += "<tr><td>Order Date</td><td>" + System.DateTime.Now.ToShortDateString() + "</td></tr>";
            strEmailBody += "</table>";
            strEmailBody += "<br/><br/>";
            strEmailBody += "<b>Order Details</b>";
            strEmailBody += "<br/><br/>";
            strEmailBody += "<table border='1px solid' style='border: 1px solid;border-collapse: collapse;'>";
            strEmailBody += "<tr> <th>Sl No</th> <th>Emerge Order Number</th> <th>Ship to Name & City</th> <th>Item Name</th> <th>Quantity</th> </tr>";
            int i = 0;
            foreach (DataRow row in dtApproItemDetails.Rows) // Loop over the rows.
            {
                strEmailBody += "<tr> <td>" + ++i + "</td> <td>" + row["OrderID"].ToString() + "-" + row["DestinationID"].ToString() + "</td> <td>" + row["ShiptoName"].ToString() + " / " + row["City"].ToString() + "</td> <td>" + row["ItemName"].ToString() + "</td> <td>" + row["Quantity"].ToString() + "</td> </tr>";
            }
            strEmailBody += "</table>";
            strEmailBody += "<br/>";
            strEmailBody += "DFVWines - Emerge";
            strEmailBody += "<br/>";
            return strEmailBody;
        }


        public static DataSet GetSeasonalOrderDetails(int SeasonalOrderID)
        {
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
            string strCommand = "EXEC Emerge.SP_getSeasonalOrderDetailsByOrderID";
            strCommand += " @SeasonalOrderId ='" + SeasonalOrderID.ToString() + "'";
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();
            return ds;
        }

        public static DataSet GetMyOrderItems(string UserId)
        {
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DCEPOSConnectionString"].ConnectionString;
            string strCommand = "EXEC Emerge.SP_getSeasonalOrderListByUserId";
            strCommand += "  @UserId ='" + UserId.ToString() + "'";
            //strCommand += " , @selectedProgramId = " + ProgramID.ToString();
            //strCommand += " , @selectedDistributorId = " + DestinationId.ToString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();

            return ds;

        }
        */
    }
}