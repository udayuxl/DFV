﻿using System;
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
    public class OrderServices
    {
        public static DataTable GetDashboardPrograms()
        {
            string strCommand = "SELECT * FROM Emerge.tblProgram p ";            
            strCommand += "WHERE OrderWindowStart <= GETDATE() AND OrderWindowEnd >= GETDATE() ";
            strCommand += "AND p.pk_Program_Id IN (SELECT fk_Program_Id FROM Emerge.tblProgramItem)";
            return DataOperations.GetDataTable(strCommand);
        }        
       
        public static string GetProgramIdByOrderId(string strOrderId)
        {
            string strCommand = @"SELECT fk_program_id FROM Emerge.tblOrder WHERE pk_order_id = " + strOrderId;

            DataRow dr = DataOperations.GetSingleRow(strCommand);
            if (dr != null)
                return dr[0].ToString();
            else
                return "";
        }
        public static string GetAddressesInOrder(string strOrderId)
        {
            string strCommand = @"SELECT fk_Address_ID  FROM Emerge.tblOrderDestination  WHERE fk_Order_ID =" + strOrderId;

            DataRow dr = DataOperations.GetSingleRow(strCommand);
            if (dr != null)
                return dr[0].ToString();
            else
                return "";
        }
        public static DataTable GetCompositeCartAddresses(string strProgramID, string strOrderID, string strItemID, string strCommaSeparatedAddressIDs)
        {            
//            if (!String.IsNullOrEmpty(strProgramID)&& !String.IsNullOrEmpty(strCommaSeparatedAddressIDs))
            if ((!String.IsNullOrEmpty(strCommaSeparatedAddressIDs)) && (String.IsNullOrEmpty(strOrderID)))  
            {                
                string strCmd = "SELECT 0 Qty, a.* FROM Emerge.tblAddress a WHERE pk_address_id in (" + strCommaSeparatedAddressIDs + ")";                    
                return DataOperations.GetDataTable(strCmd);
            }
            else if (!String.IsNullOrEmpty(strOrderID))
            {
                string strCmd = "SELECT a.*, odi.quantity Qty FROM Emerge.tblOrderDestination od JOIN Emerge.tblAddress a ON a.pk_address_id = od.fk_Address_ID ";
                strCmd+= "LEFT JOIN Emerge.tblOrderDestinationItem odi ON od.pk_OrderDestination_ID = odi.fk_OrderDestination_ID AND  fk_Item_ID = " + strItemID + " ";
                strCmd+= "WHERE fk_Order_ID = " + strOrderID;
                
                return DataOperations.GetDataTable(strCmd);
            }
            else
                return null;
        }

        public static bool IsItemValidInState(string strItemID, string strState)
        {
            // Not Implemented yet
            return true;
        }

        public static bool CanPlaceOrder()
        {
            if (Roles.IsUserInRole("DVP") || Roles.IsUserInRole("RSM") || Roles.IsUserInRole("DM") || Roles.IsUserInRole("SM"))
                return true;
            else
                return false;
        }
        
        public static DataTable GetSeasonalOrderItems(string strProgramID, string strOrderID)
        {
            string strCmd = "";
            if (!String.IsNullOrEmpty(strProgramID))
            {
                strCmd = @"SELECT DISTINCT I.*,it.*,b.* FROM (SELECT fk_Program_ID,fk_Item_ID  FROM ( 	SELECT fk_Program_ID, fk_Item_ID FROM Emerge.tblProgramItem 	UNION  	SELECT DISTINCT O.fk_Program_ID, ODI.fk_Item_ID FROM Emerge.tblOrder O LEFT JOIN Emerge.tblOrderDestination OD ON ( OD.fk_Order_ID = O.pk_Order_ID) LEFT JOIN Emerge.tblOrderDestinationItem ODI ON (ODI.fk_OrderDestination_ID = OD.pk_OrderDestination_ID) ) itm) PI 
                            JOIN Emerge.tblItem I on I.pk_Item_ID = PI.fk_Item_Id
                            JOIN Emerge.tblBrand b on b.pk_Brand_id = i.fk_BrandId
                            JOIN Emerge.tblItemType it on it.pk_ItemType_ID = i.fk_ItemType_ID
                            WHERE PI.fk_Program_Id = ";
                strCmd += strProgramID;
                strCmd += " order by brand, Item_Name"; 
            }
            else if (!String.IsNullOrEmpty(strOrderID))
            {
                strCmd = @"SELECT  DISTINCT  I.*,it.*,b.* FROM Emerge.tblOrderDestination od
	                        JOIN Emerge.tblOrderDestinationItem odi ON od.pk_OrderDestination_ID = odi.fk_OrderDestination_ID
	                        JOIN Emerge.tblItem i ON i.pk_Item_ID = odi.fk_Item_ID
                            JOIN Emerge.tblBrand b on b.pk_Brand_id = i.fk_BrandId
                            JOIN Emerge.tblItemType it on it.pk_ItemType_ID = i.fk_ItemType_ID
	                        WHERE od.fk_Order_ID = ";
                strCmd += strOrderID;
                strCmd += " order by brand, Item_Name"; 
            }
            else
                return null;            

            return DataOperations.GetDataTable(strCmd);
        }

        public static DataTable GetSeasonalOrderItemsForGridViewFilter(int SeasonalOrderId, string ProgramIDList, string ListOfAddress, string UserId, string dist,string SelectedItem,string selectedBrand)
        {
            string strCommand = "EXEC Emerge.SP_getSeasonalOrderItemsForGridViewFilter";
            strCommand += " @Seasonal_Order_Id = " + SeasonalOrderId.ToString();
            strCommand += " ,@Address_Id_List = '" + ListOfAddress.ToString() + "'";
            strCommand += " ,@Program_Id_List = '" + ProgramIDList.ToString() + "'";
            strCommand += " ,@selectedDistributorId = " + dist + "";
            strCommand += " ,@SelectedItem = " + SelectedItem + "";
            strCommand += " ,@selectedBrandId = " + selectedBrand + "";

            return DataOperations.GetDataTable(strCommand);
        }

        public static string CreateEmptyOrder(string strProgramID, string strCommaSeparatedAddressIDs)
        {
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            string strCmd = "INSERT INTO Emerge.tblOrder (fk_Program_ID, OrderDate,fk_OrderPlacedBy,fk_LastUpdatedBy,LastUpdatedOn,Status) ";
            strCmd += "VALUES(" + strProgramID + ",GETDATE(),'" + strUserID + "','"+strUserID+"',GETDATE(), 'OPEN')";
            DataOperations.ExecuteSQL(strCmd);
            DataRow drOrderID = DataOperations.GetSingleRow("SELECT IDENT_CURRENT('Emerge.tblOrder')");
            string strOrderID = drOrderID[0].ToString();

            string[] arrAddressIDs = null;
            if(!String.IsNullOrEmpty(strCommaSeparatedAddressIDs))
                arrAddressIDs = strCommaSeparatedAddressIDs.Split(',');
            foreach (string strAddressID in arrAddressIDs)
            {
                strCmd = "INSERT INTO Emerge.tblOrderDestination (fk_Order_ID, fk_Address_ID) VALUES(" + strOrderID + "," + strAddressID + ")";
                DataOperations.ExecuteSQL(strCmd);
                DataRow drOrderDestinationID = DataOperations.GetSingleRow("SELECT IDENT_CURRENT('Emerge.tblOrderDestination')");
                string strOrderDestinationID = drOrderDestinationID[0].ToString();

                strCmd = "SELECT fk_Item_Id FROM Emerge.tblProgramItem WHERE fk_Program_Id=" + strProgramID;
                DataTable dtItemIDs = DataOperations.GetDataTable(strCmd);
                foreach (DataRow drItemID in dtItemIDs.Rows)
                {
                    string strItemID = drItemID[0].ToString();
                    strCmd = "INSERT INTO Emerge.tblOrderDestinationItem (fk_OrderDestination_ID, fk_Item_ID,quantity) VALUES(" + strOrderDestinationID + "," + strItemID + ",0)";
                    DataOperations.ExecuteSQL(strCmd);
                }
            }
            return strOrderID;
        }

        public static void UpdateOrder(string strOrderID, string strNotes, string strStatus)
        {   
            string strCmd = "UPDATE Emerge.tblOrder ";
            strCmd += "SET Status='" + strStatus + "', Notes = '"+strNotes.Replace("'","''") + "', ";
            strCmd += "LastUpdatedOn = '" + DateTime.Now.ToString() +"' ";
            strCmd += "WHERE pk_Order_ID = " + strOrderID;
            DataOperations.ExecuteSQL(strCmd);
            // Update item price in OrderDestinationItem table
            if (strStatus == "CONFIRMED")
            {
                strCmd = "UPDATE Emerge.tblOrderDestinationItem SET  Emerge.tblOrderDestinationItem.ItemPrice = Itm.ActualPrice ";
                strCmd += "FROM Emerge.tblOrderDestinationItem INNER JOIN Emerge.tblItem Itm ON ( Itm.pk_Item_ID =Emerge.tblOrderDestinationItem.fk_Item_ID) ";
                strCmd += "WHERE Emerge.tblOrderDestinationItem.fk_OrderDestination_ID IN (SELECT pk_OrderDestination_ID FROM Emerge.tblOrderDestination WHERE fk_Order_ID IN (" + strOrderID + "))";
                DataOperations.ExecuteSQL(strCmd);
            }

        }

        public static DataRow GetOrderMetaDetails(string strOrderID)
        {
            string strCmd = "SELECT * FROM Emerge.tblOrder WHERE pk_Order_ID = " + strOrderID;
            return DataOperations.GetSingleRow(strCmd);
        }

        public static bool ConfirmOrder(string strOrderID)
        {
            //string strCmd = "UPDATE Emerge.tblOrder SET Status = 'CONFIRMED' WHERE pk_Order_ID = " + strOrderID;
            string strCmd = "UPDATE Emerge.tblOrder ";
            strCmd += "SET tblOrder.inProcess_date = GETDATE(), tblOrder.Status = ";
            strCmd += "CASE ";
            strCmd += "WHEN fk_Program_ID IS NULL AND EXISTS (SELECT ISNULL(ODI.pk_OrderDestinationItem_ID,0) FROM Emerge.tblOrder O ";
            strCmd += "LEFT JOIN Emerge.tblOrderDestination OD ON (OD.fk_Order_ID = O.pk_Order_ID) ";
            strCmd += "LEFT JOIN Emerge.tblOrderDestinationItem ODI ON (ODI.fk_OrderDestination_ID =OD.pk_OrderDestination_ID) ";
            strCmd += "WHERE (O.fk_Program_ID IS NULL) AND (ODI.ApprovedQuantity = -1) AND (O.pk_Order_ID =  " + strOrderID + ") ) ";
            strCmd += "THEN 'PENDAPPROVAL' ";
            strCmd += "WHEN fk_Program_ID IS NOT NULL THEN 'CONFIRMED' "; // Seasonal Orders
            strCmd += "ELSE 'IN PROCESS' ";
            strCmd += "END ";
            strCmd += "FROM Emerge.tblOrder ";
            strCmd += "WHERE pk_Order_ID = " + strOrderID;
            int intRowsSaved = DataOperations.ExecuteSQL(strCmd);
            if (intRowsSaved == 1)
            {
                strCmd = "UPDATE Emerge.tblItem SET Emerge.tblItem.inProcess_inventory = 0";
                DataOperations.ExecuteSQL(strCmd);

                strCmd = "UPDATE Emerge.tblItem SET Emerge.tblItem.inProcess_inventory = InPInv.BlockedItemQty ";
                strCmd += "FROM Emerge.tblItem  INNER JOIN ";
                strCmd += "(";
                strCmd += "SELECT ODI.fk_item_id ,ISNULL(SUM(CASE WHEN ODI.ApprovedQuantity = -1 THEN ISNULL(ODI.quantity,0) WHEN ODI.ApprovedQuantity IS NULL THEN ISNULL(ODI.quantity,0) ELSE ISNULL(ODI.ApprovedQuantity,0) END),0) AS BlockedItemQty ";
                strCmd += "FROM Emerge.tblOrderDestinationItem ODI ";
                strCmd += "INNER JOIN Emerge.tblOrderDestination OD ON (OD.pk_OrderDestination_ID = ODI.fk_OrderDestination_ID) "; 
                strCmd += "INNER JOIN Emerge.tblOrder O ON (O.pk_Order_ID = OD.fk_Order_ID) "; 
                strCmd += "WHERE ((UPPER(O.Status) = 'IN PROCESS') OR (UPPER(O.Status) = 'PENDAPPROVAL')) ";
                strCmd += "GROUP BY ODI.fk_item_id ";
                strCmd += ")InPInv ";
                strCmd += "ON (Emerge.tblItem.pk_Item_Id = InPInv.fk_item_id) "; 
                /*
                strCmd = "UPDATE Emerge.tblItem SET Emerge.tblItem.inProcess_inventory = Emerge.tblItem.inProcess_inventory+InPInv.BlockedItemQty ";
                strCmd += "FROM Emerge.tblItem  INNER JOIN ";
                strCmd += " ( ";
                strCmd += " SELECT ODI.fk_item_id ,ISNULL(SUM(CASE WHEN ODI.ApprovedQuantity = -1 THEN ISNULL(ODI.quantity,0) WHEN ODI.ApprovedQuantity IS NULL THEN ISNULL(ODI.quantity,0) ELSE ISNULL(ODI.ApprovedQuantity,0) END),0) AS BlockedItemQty ";
                strCmd += "	FROM Emerge.tblOrderDestinationItem ODI ";
                strCmd += "	INNER JOIN Emerge.tblOrderDestination OD ON (OD.pk_OrderDestination_ID = ODI.fk_OrderDestination_ID) ";
                strCmd += "	INNER JOIN Emerge.tblOrder O ON (O.pk_Order_ID = OD.fk_Order_ID) AND (O.fk_Program_ID IS NULL)";
                strCmd += "	WHERE ((UPPER(O.Status) = 'IN PROCESS') OR (UPPER(O.Status) = 'PENDAPPROVAL')) AND O.pk_Order_ID= " + strOrderID;
                strCmd += "	GROUP BY ODI.fk_item_id ";
                strCmd += "	)InPInv ";
                strCmd += "	ON (Emerge.tblItem.pk_Item_Id = InPInv.fk_item_id) ";
                */
                DataOperations.ExecuteSQL(strCmd);
     
                return true;
            }
            else
                return false;
        }
        public static void SaveOrderQuantity(string strItemID, string strAddressID, string strOrderID, string strQuantity)
        {
            string strCmd = "SELECT pk_OrderDestination_ID FROM Emerge.tblOrderDestination WHERE fk_Order_ID = " + strOrderID + " AND fk_Address_ID = " + strAddressID;
            DataRow drOrderDestinationID = DataOperations.GetSingleRow(strCmd);
            string strOrderDestinationID = drOrderDestinationID[0].ToString();
            //strCmd = "UPDATE Emerge.tblOrderDestinationItem  SET quantity = " + strQuantity + " WHERE fk_Item_ID = " + strItemID + " AND fk_OrderDestination_ID = " +strOrderDestinationID;
            //DataOperations.ExecuteSQL(strCmd);
            strCmd = "[Emerge].[SaveItemQuantity] @quantity = " + strQuantity + ", @ItemID = " + strItemID + ", @OrderDestinationID= " + strOrderDestinationID;
            DataOperations.ExecuteSQL(strCmd);
        }

        public static DataTable GetMyOrders()
        {
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            string strCmd = @"  SELECT O.pk_Order_ID OrderNo, OrderDate, SUM(ODI.quantity * I.EstimatedPrice) OrderTotal, P.Name [ProgramName], O.Status
                                FROM Emerge.tblOrder O
                                join Emerge.tblOrderDestination OD on O.pk_Order_ID = OD.fk_Order_ID
                                join Emerge.tblOrderDestinationItem ODI on OD.pk_OrderDestination_ID = ODI.fk_OrderDestination_ID
                                join Emerge.tblItem I on I.pk_Item_ID = ODI.fk_Item_ID
                                join Emerge.tblProgram P on P.pk_Program_Id = O.fk_Program_ID ";
            strCmd += "WHERE O.fk_OrderPlacedBy = '" + strUserID + "' ";
            strCmd += "GROUP BY O.pk_Order_ID, O.OrderDate, P.Name,O.Status";
            return DataOperations.GetDataTable(strCmd);
        }

        public static bool IsOrderWindowOpen(string strOrderID)
        {
            string strCmd = "SELECT COUNT(*) FROM Emerge.tblOrder o JOIN Emerge.tblProgram p ON o.fk_Program_ID = p.pk_Program_Id ";
            strCmd += "WHERE OrderWindowStart <= GETDATE() AND OrderWindowEnd >= GETDATE() ";
            strCmd += "AND pk_Order_ID = " + strOrderID;
            return Convert.ToBoolean(DataOperations.GetSingleRow(strCmd)[0]);
        }
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


        //***********************************************************************************
        //************ Inventory Order Services *********************************************
        //***********************************************************************************

        public static DataTable GetInventoryOrderItems(string strOrderID)
        {
            string strCmd = "";

            strCmd = @"SELECT  I.*,it.*,b.* 
	                        FROM Emerge.tblItem i 
	                        JOIN Emerge.tblBrand b on b.pk_Brand_id = i.fk_BrandId
                            LEFT JOIN Emerge.tblItemType it on it.pk_ItemType_ID = i.fk_ItemType_ID
                            WHERE InvOrdItem = 1 AND Active = 1";
            return DataOperations.GetDataTable(strCmd);
        }


        public static DataTable GetInventoryOrderItems(string strOrderID, string strSearchText, string strBrands)
        {
            string strCmd = "";

            if (strOrderID == "")
                strOrderID = "0";
            //strCmd = "SELECT  I.*,it.*,b.* , [Emerge].[fn_GetAvailableStockByItemID] (I.pk_item_id, " + strOrderID + ") AS AvblStock, ISNULL(MaxOrderQuantity,0) AS MaxOrderQty ";
            strCmd = "SELECT  I.*,it.*,b.* ,CASE WHEN (ISNULL(i.Stock,0)-ISNULL(i.inProcess_inventory,0)) < 0 THEN 0 ELSE (ISNULL(i.Stock,0)-ISNULL(i.inProcess_inventory,0)) END AS AvblStock, ISNULL(MaxOrderQuantity,0) AS MaxOrderQty ";
	        strCmd += "FROM Emerge.tblItem i ";
	        strCmd += "JOIN Emerge.tblBrand b on b.pk_Brand_id = i.fk_BrandId ";
            strCmd += "LEFT JOIN Emerge.tblItemType it on it.pk_ItemType_ID = i.fk_ItemType_ID ";
            strCmd += "WHERE InvOrdItem = 1 ";
            if (strSearchText != "")
                strCmd = strCmd + " AND ((i.Item_No like '%" + strSearchText + "%') OR (i.Item_Name like '%" + strSearchText + "%')) ";
            if( strBrands != "")
                strCmd = strCmd + " AND b.pk_Brand_id IN (" + strBrands + ") AND Active = 1";
            return DataOperations.GetDataTable(strCmd);
        }

        public static string CreateEmptyOrder(string strCommaSeparatedAddressIDs)
        {
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            string strCmd = "INSERT INTO Emerge.tblOrder (OrderDate,fk_OrderPlacedBy,fk_LastUpdatedBy,LastUpdatedOn,Status) ";
            strCmd += "VALUES(GETDATE(),'" + strUserID + "','" + strUserID + "',GETDATE(), 'OPEN')";
            DataOperations.ExecuteSQL(strCmd);
            DataRow drOrderID = DataOperations.GetSingleRow("SELECT IDENT_CURRENT('Emerge.tblOrder')");
            string strOrderID = drOrderID[0].ToString();

            string[] arrAddressIDs = null;
            if (!String.IsNullOrEmpty(strCommaSeparatedAddressIDs))
                arrAddressIDs = strCommaSeparatedAddressIDs.Split(',');
            foreach (string strAddressID in arrAddressIDs)
            {
                strCmd = "INSERT INTO Emerge.tblOrderDestination (fk_Order_ID, fk_Address_ID) VALUES(" + strOrderID + "," + strAddressID + ")";
                DataOperations.ExecuteSQL(strCmd);
                DataRow drOrderDestinationID = DataOperations.GetSingleRow("SELECT IDENT_CURRENT('Emerge.tblOrderDestination')");
                string strOrderDestinationID = drOrderDestinationID[0].ToString();
            }
            return strOrderID;
        }

        public static string CreateEmptyInvOrder(string strCommaSeparatedAddressIDs, string strCommaSeparatedShippingInstructionIDs)
        {
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            string strCmd = "INSERT INTO Emerge.tblOrder (OrderDate,fk_OrderPlacedBy,fk_LastUpdatedBy,LastUpdatedOn,Status) ";
            strCmd += "VALUES(GETDATE(),'" + strUserID + "','" + strUserID + "',GETDATE(), 'OPEN')";
            DataOperations.ExecuteSQL(strCmd);
            DataRow drOrderID = DataOperations.GetSingleRow("SELECT IDENT_CURRENT('Emerge.tblOrder')");
            string strOrderID = drOrderID[0].ToString();

            string[] arrAddressIDs = null;
            if (!String.IsNullOrEmpty(strCommaSeparatedAddressIDs))
                arrAddressIDs = strCommaSeparatedAddressIDs.Split(',');
            string[] arrShippingInstuctionIDs = null;
            if (!String.IsNullOrEmpty(strCommaSeparatedShippingInstructionIDs))
                arrShippingInstuctionIDs = strCommaSeparatedShippingInstructionIDs.Split(',');
            int arryIndex = 0;
            string strShipInstID = "1";
            foreach (string strAddressID in arrAddressIDs)
            {
                strShipInstID = "";
                if (arrShippingInstuctionIDs.Length > arryIndex) 
                    strShipInstID = arrShippingInstuctionIDs[arryIndex].ToString();
                if (strShipInstID == "") strShipInstID = "1"; // Default Shipping Instruction
                arryIndex++;
                strCmd = "INSERT INTO Emerge.tblOrderDestination (fk_Order_ID, fk_Address_ID, fk_shipping_instruction_id) VALUES(" + strOrderID + "," + strAddressID + "," + strShipInstID + ")";
                DataOperations.ExecuteSQL(strCmd);
                DataRow drOrderDestinationID = DataOperations.GetSingleRow("SELECT IDENT_CURRENT('Emerge.tblOrderDestination')");
                string strOrderDestinationID = drOrderDestinationID[0].ToString();
            }
            return strOrderID;
        }

        public static DataTable GetMyInventoryOrders()
        {
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            string strCmd = @"  SELECT CAST(O.pk_Order_ID AS VARCHAR(10))+'-'+CAST(OD.pk_OrderDestination_ID AS VARCHAR(10)) AS OrderNo, OrderDate, SUM(ODI.quantity * I.EstimatedPrice) OrderTotal, 
                                SUBSTRING(OD.WarehouseShippingComments, CHARINDEX('TrackingNumbers : ', OD.WarehouseShippingComments) + LEN('TrackingNumbers : ')+1, LEN( OD.WarehouseShippingComments)) AS TrackingNos,
		                        CASE WHEN OD.WarehouseShippingStatus = 'Shipped'
	                            THEN 'SHIPPED'
	                            WHEN O.Status = 'Submitted' Then 'IN PROCESS'
	                            ELSE O.Status END AS Status
                                FROM Emerge.tblOrder O
		                        join Emerge.tblOrderDestination OD on O.pk_Order_ID = OD.fk_Order_ID
		                        join Emerge.tblOrderDestinationItem ODI on OD.pk_OrderDestination_ID = ODI.fk_OrderDestination_ID
		                        join Emerge.tblItem I on I.pk_Item_ID = ODI.fk_Item_ID  ";
            strCmd += "WHERE O.fk_Program_ID IS NULL AND O.fk_OrderPlacedBy = '" + strUserID + "' ";
            strCmd += "GROUP BY O.pk_Order_ID, O.OrderDate, O.Status, OD.pk_OrderDestination_ID, OD.WarehouseShippingStatus,OD.WarehouseShippingComments";
            return DataOperations.GetDataTable(strCmd);
        }

        public static DataTable GetInventoryOrderItemsForApprovalByUserID()
        {
            string strCmd = "";
            strCmd = "SELECT O.OrderDate AS [OrderDate], DATEDIFF(HH, OrderDate, GETDATE()) AS [PendingHours], ";
            strCmd += "CAST(O.pk_Order_ID AS VARCHAR(10)) AS [InventoryOrderID], ";
            strCmd += "OrdBy.UserName AS [SMName], B.pk_Brand_id AS BrandID,  B.Brand AS [BrandName], I.pk_Item_id AS ItemID ,I.Item_Name AS [ItemName], A.shipto_company AS [Distributor], ";
            //strCmd += "ODI.quantity AS Qty, [Emerge].[fn_GetAvailableStockByItemID] (I.pk_item_id, O.pk_Order_ID) AS [AvailableInvetory], ";
            strCmd += "ODI.quantity AS Qty, ISNULL(I.Stock,0) AS [AvailableInvetory], ";
            strCmd += " '' AS Comments, ODI.pk_OrderDestinationItem_ID AS InventoryOrderDestinationItem, 0 AS Status, OD.pk_OrderDestination_ID AS OrderDestinationID, ";
            strCmd += "ODI.ApprovedQuantity, O.Status AS OrderStatus , OrdBy.userid AS UserID ";
            strCmd += "FROM Emerge.tblOrderDestinationItem ODI ";
            strCmd += "LEFT JOIN Emerge.tblOrderDestination OD ON (OD.pk_OrderDestination_ID = ODI.fk_OrderDestination_ID) ";
            strCmd += "LEFT JOIN Emerge.tblOrder O ON (O.pk_Order_ID = OD.fk_Order_ID) ";
            strCmd += "LEFT JOIN Emerge.tblItem I ON (I.pk_Item_ID = ODI.fk_Item_ID) ";
            strCmd += "LEFT JOIN Emerge.tblApprover App ON ( App.fk_Brand_id = I.fk_BrandId) ";
            strCmd += "LEFT JOIN aspnet_Users U ON (U.UserId = App.fk_Approver) ";
            strCmd += "LEFT JOIN aspnet_Users OrdBy ON (OrdBy.UserId = O.fk_OrderPlacedBy) ";
            strCmd += "LEFT JOIN Emerge.tblBrand B ON ( B.pk_Brand_id = I.fk_BrandId) ";
            strCmd += "LEFT JOIN Emerge.tblAddress A ON ( A.pk_address_id = OD.fk_Address_ID) ";
            strCmd += "WHERE ApprovedQuantity = -1 AND fk_Approver = '" + Membership.GetUser().ProviderUserKey.ToString() + "' ";
            strCmd += "AND UPPER(O.Status) = 'PENDAPPROVAL' ";

            /////////////////////////
            //System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\wwwroot\\Lumen\\RodneyStrong\\Z-Old-Stage\\Logs\\temp.txt");
            //sw.Write(strCmd);
            //sw.Close();
            /////////////////////////
            return DataOperations.GetDataTable(strCmd);


        }


        public static void updateInventoryOrderItemApproval(List<clsInventoryOrderItem> listInventoryOrderItem)
        {
            /////////////////////////
            //System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\wwwroot\\Lumen\\RodneyStrong\\Z-Old-Stage\\Logs\\temp.txt");
            //sw.WriteLine("updateInventoryOrderItemApproval");
            /////////////////////////


            try
            {
                foreach (clsInventoryOrderItem objInventoryOrderItem in listInventoryOrderItem)
                {
                    

                    string strCmd = "";
                    strCmd = "UPDATE [Emerge].tblOrderDestinationItem ";
                    strCmd += "SET ApprovedQuantity = " + objInventoryOrderItem.ApprovedQuantity.ToString() + " ";
                    strCmd += ", ApprovedBy = '"+objInventoryOrderItem.Approver.ToString() + "' ";
                    strCmd += ", LastUpdatedOn = GETDATE() ";
                    strCmd += ", comments = '" + objInventoryOrderItem.Comments.ToString() + "' ";
                    strCmd += "WHERE pk_OrderDestinationItem_ID = "+objInventoryOrderItem.InventoryOrderDestinationItem.ToString();

                    string strResult = "";
                    DataOperations.ExecuteSQL(strCmd, out strResult);

                    //sw.WriteLine(strCmd);                    


                    strCmd = "";
                    strCmd = "UPDATE Emerge.tblOrder ";
                    strCmd += "SET Status  = 'IN PROCESS' ";
                    strCmd += "WHERE NOT EXISTS (SELECT ISNULL(ODI.pk_OrderDestinationItem_ID,0) FROM Emerge.tblOrder O ";
                    strCmd += "LEFT JOIN Emerge.tblOrderDestination OD ON (OD.fk_Order_ID = O.pk_Order_ID) ";
                    strCmd += "LEFT JOIN Emerge.tblOrderDestinationItem ODI ON (ODI.fk_OrderDestination_ID =OD.pk_OrderDestination_ID) ";
                    strCmd += "WHERE (O.fk_Program_ID IS NULL) AND (ODI.ApprovedQuantity = -1) AND (O.pk_Order_ID = " + objInventoryOrderItem.InventoryOrderID.ToString() + ") ) ";
                    strCmd += "AND pk_Order_ID = " + objInventoryOrderItem.InventoryOrderID.ToString();

                    //sw.WriteLine(strCmd);                

                    DataOperations.ExecuteSQL(strCmd, out strResult);

                    MailServices.SendOrderApprovalStatusToSM(objInventoryOrderItem.InventoryOrderID.ToString());
                        
                }
            }
            catch (Exception ex)
            {
               // sw.WriteLine(ex.ToString());                    
                // return 0;
            }

           // sw.Close();
        }
        public static void UpdateAvailableInventory(List<clsInventoryItemAllocation> listInventoryItemAllocation)
        {
            try
            {
                foreach (clsInventoryItemAllocation objInventoryItemAllocation in listInventoryItemAllocation)
                {
                    string strCmd = "";
                    strCmd = "UPDATE Emerge.tblItem SET Stock =((Stock + "+objInventoryItemAllocation.PreviousQty.ToString()+" ) - "+objInventoryItemAllocation.Quantity.ToString() +") ";
                    strCmd += "WHERE pk_Item_ID = "+objInventoryItemAllocation.ItemId;
                    string strResult = "";
                    DataOperations.ExecuteSQL(strCmd, out strResult);
                }
                //return true;
            }
            catch (Exception ex)
            {
                //return false;
            }
        }
        // For Brand Filter
        public static DataTable GetSeasonalOrderItems(string strProgramID, string strOrderID, String strBrandList, string searchText)
        {
            string strCmd = "";
            if (!String.IsNullOrEmpty(strProgramID))
            {
                strCmd = @"SELECT DISTINCT I.*,it.*,b.* FROM (SELECT fk_Program_ID,fk_Item_ID  FROM ( 	SELECT fk_Program_ID, fk_Item_ID FROM Emerge.tblProgramItem 	UNION  	SELECT DISTINCT O.fk_Program_ID, ODI.fk_Item_ID FROM Emerge.tblOrder O LEFT JOIN Emerge.tblOrderDestination OD ON ( OD.fk_Order_ID = O.pk_Order_ID) LEFT JOIN Emerge.tblOrderDestinationItem ODI ON (ODI.fk_OrderDestination_ID = OD.pk_OrderDestination_ID) ) itm) PI 
                            LEFT JOIN Emerge.tblItem I on I.pk_Item_ID = PI.fk_Item_Id
                            LEFT JOIN Emerge.tblBrand b on b.pk_Brand_id = i.fk_BrandId
                            LEFT JOIN Emerge.tblItemType it on it.pk_ItemType_ID = i.fk_ItemType_ID
                            WHERE PI.fk_Program_Id = ";
                strCmd += strProgramID;
                if (!String.IsNullOrEmpty(strBrandList))
                {
                    if (!strBrandList.Contains("9999"))
                        strCmd += " AND b.pk_Brand_id IN ( " + strBrandList + " )";
                }
                if (!String.IsNullOrEmpty(searchText))
                    strCmd += " AND ( ( I.Item_Name like  '%" + searchText + "%' ) OR ( I.Item_No like  '%" + searchText + "%' ) OR ( B.Brand like  '%" + searchText + "%' ) ) "; // I.Item_Name, I.Item_No, B.Brand
                strCmd += " order by brand, Item_Name";
            }
            else if (!String.IsNullOrEmpty(strOrderID))
            {
                strCmd = @"SELECT DISTINCT I.*,it.*,b.* FROM Emerge.tblOrderDestination od
	                        JOIN Emerge.tblOrderDestinationItem odi ON od.pk_OrderDestination_ID = odi.fk_OrderDestination_ID
	                        JOIN Emerge.tblItem i ON i.pk_Item_ID = odi.fk_Item_ID
                            JOIN Emerge.tblBrand b on b.pk_Brand_id = i.fk_BrandId
                            JOIN Emerge.tblItemType it on it.pk_ItemType_ID = i.fk_ItemType_ID
	                        WHERE od.fk_Order_ID = ";
                strCmd += strOrderID;
                if (!String.IsNullOrEmpty(strBrandList))
                {
                    if (!strBrandList.Contains("9999"))
                        strCmd += " AND b.pk_Brand_id IN ( " + strBrandList + " )";
                }
                if (!String.IsNullOrEmpty(searchText))
                    strCmd += " AND ( ( I.Item_Name like  '%" + searchText + "%' ) OR ( I.Item_No like  '%" + searchText + "%' ) OR ( B.Brand like  '%" + searchText + "%' ) ) "; // I.Item_Name, I.Item_No, B.Brand
                strCmd += " order by brand, Item_Name";
            }
            else
                return null;

            return DataOperations.GetDataTable(strCmd);
        }

        // ITem Type Filter
        public static DataTable GetSeasonalOrderItemsFilterByItemType(string strProgramID, string strOrderID, String strBrandList, string searchText)
        {
            string strCmd = "";
            if (!String.IsNullOrEmpty(strProgramID))
            {
                strCmd = @"SELECT DISTINCT I.*,it.*,b.* FROM (SELECT fk_Program_ID,fk_Item_ID  FROM ( 	SELECT fk_Program_ID, fk_Item_ID FROM Emerge.tblProgramItem 	UNION  	SELECT DISTINCT O.fk_Program_ID, ODI.fk_Item_ID FROM Emerge.tblOrder O LEFT JOIN Emerge.tblOrderDestination OD ON ( OD.fk_Order_ID = O.pk_Order_ID) LEFT JOIN Emerge.tblOrderDestinationItem ODI ON (ODI.fk_OrderDestination_ID = OD.pk_OrderDestination_ID) ) itm) PI 
                            LEFT JOIN Emerge.tblItem I on I.pk_Item_ID = PI.fk_Item_Id
                            LEFT JOIN Emerge.tblBrand b on b.pk_Brand_id = i.fk_BrandId
                            LEFT JOIN Emerge.tblItemType it on it.pk_ItemType_ID = i.fk_ItemType_ID
                            WHERE PI.fk_Program_Id = ";
                strCmd += strProgramID;
                if (!String.IsNullOrEmpty(strBrandList))
                {
                    if (!strBrandList.Contains("9999"))
                        strCmd += " AND it.pk_ItemType_ID IN ( " + strBrandList + " )";
                }
                if (!String.IsNullOrEmpty(searchText))
                    strCmd += " AND ( ( I.Item_Name like  '%" + searchText + "%' ) OR ( I.Item_No like  '%" + searchText + "%' ) OR ( B.Brand like  '%" + searchText + "%' ) ) "; // I.Item_Name, I.Item_No, B.Brand
                strCmd += " order by brand, Item_Name";
            }
            else if (!String.IsNullOrEmpty(strOrderID))
            {
                strCmd = @"SELECT DISTINCT I.*,it.*,b.* FROM Emerge.tblOrderDestination od
	                        JOIN Emerge.tblOrderDestinationItem odi ON od.pk_OrderDestination_ID = odi.fk_OrderDestination_ID
	                        JOIN Emerge.tblItem i ON i.pk_Item_ID = odi.fk_Item_ID
                            JOIN Emerge.tblBrand b on b.pk_Brand_id = i.fk_BrandId
                            JOIN Emerge.tblItemType it on it.pk_ItemType_ID = i.fk_ItemType_ID
	                        WHERE od.fk_Order_ID = ";
                strCmd += strOrderID;
                if (!String.IsNullOrEmpty(strBrandList))
                {
                    if (!strBrandList.Contains("9999"))
                        strCmd += " AND it.pk_ItemType_ID IN ( " + strBrandList + " )";
                }
                if (!String.IsNullOrEmpty(searchText))
                    strCmd += " AND ( ( I.Item_Name like  '%" + searchText + "%' ) OR ( I.Item_No like  '%" + searchText + "%' ) OR ( B.Brand like  '%" + searchText + "%' ) ) "; // I.Item_Name, I.Item_No, B.Brand
                strCmd += " order by brand, Item_Name";
            }
            else
                return null;

            return DataOperations.GetDataTable(strCmd);
        }

        public static DataTable GetAddressesForOrderID(string strOrderID)
        {
            string strCmd = "";
            if (!String.IsNullOrEmpty(strOrderID))
                strCmd = "SELECT fk_Address_ID FROM Emerge.tblOrderDestination WHERE fk_Order_ID = " + strOrderID;
            else
                strCmd = "SELECT 0 AS fk_Address_ID";
            return DataOperations.GetDataTable(strCmd);
        }
        

    }


    // Class Objects Need to be moved to proper location if needed
    public class clsInventoryItemAllocation
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public Guid UserId { get; set; }
        public int TotalQuantity { get; set; }
        public int PreviousQty { get; set; }

    }
    public class clsInventoryOrderItem
    {
        public int InventoryOrderID { get; set; }
        public Guid ProviderUserKey { get; set; }
        public int InventoryOrderDestinationItem { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApprovedQuantity { get; set; }
        public Guid Approver { get; set; }
        public string Comments { get; set; }

    }
}