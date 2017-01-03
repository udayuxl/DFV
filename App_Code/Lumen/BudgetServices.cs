using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ACE;
using System.Data;

namespace Lumen
{
    public static class BudgetServices
    {
        ////public static decimal GetBudgetAllocated(string strUserID, string strProgramID)
        ////{
        ////    string strCmd = String.Format("SELECT ISNULL(Budget, 0)Budget FROM Emerge.tblUserBudget WHERE fk_User_ID ='{0}'AND fk_Program_ID ={1}", strUserID, strProgramID);
        ////    DataRow dr = DataOperations.GetSingleRow(strCmd);
        ////    if (dr == null)
        ////        return 0;

        ////    return Convert.ToDecimal(dr["Budget"]);            
        ////}

        public static DataRow GetMyBudgetDetails(string strProgramID, string strOrderID)
        {
            if (strOrderID == "") strOrderID = "0";
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            string strCmd = "";
            strCmd += " Emerge.SP_GetAvailableBudget @SeasonalOrderID = " + strOrderID + ", @ProgramID = " + strProgramID + ", @UserID = '" + strUserID + "'";
            DataRow drMyBudgetDetails = DataOperations.GetSingleRow(strCmd);
            return drMyBudgetDetails;
        }  
                
        public static Decimal GetMarketBudgetAvailableForAllocation(string strProgramID)
        {            
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            string strCmd = "";
            strCmd += " Emerge.SP_GetAvailableBudget @SeasonalOrderID = 0, @ProgramID = " + strProgramID + ", @UserID = '" + strUserID + "'";
            Decimal MarketBudgetAllocatedtoMe =  Convert.ToDecimal(DataOperations.GetSingleRow(strCmd)["BudgetAllocatedToMe"].ToString());
            
            strCmd = "";
            strCmd += " SELECT ISNULL( SUM( ISNULL(ub.Budget,0) ),0) TeamBudget FROM aspnet_Users u ";
		    strCmd += "JOIN Emerge.tblUserPortfolio up on up.fk_User_ID = u.UserId ";
		    strCmd += "LEFT JOIN Emerge.tblUserBudget ub ON ub.fk_User_ID = u.UserId AND fk_Program_ID = " + strProgramID + " ";
            strCmd += "WHERE up.fk_Manager_ID = '" + strUserID + "' ";
            Decimal MarketBudgetDistributedToMyTeam = Convert.ToDecimal(DataOperations.GetSingleRow(strCmd)["TeamBudget"].ToString());

            return MarketBudgetAllocatedtoMe - MarketBudgetDistributedToMyTeam;
        }        

        public static string AddProgramBudgetToUser(string strAllocateToUserID, string strProgramID, decimal Budget)
        {
            try
            {
                string strManagerID = Membership.GetUser().ProviderUserKey.ToString();
                string strCmd = "";
                DataRow drResult = null;

                //if (Roles.IsUserInRole("Admin")) // if admin no need to check current user's budget first
                if (Roles.IsUserInRole("Admin") || Roles.IsUserInRole("MM"))
                {
                    strCmd = "EXEC Emerge.SP_AddBudget @UserID = '" + strAllocateToUserID + "', @ProgramID =" + strProgramID + " , @Budget = " + Budget + ", @ManagerID = '" + strManagerID + "'";
                    drResult = DataOperations.GetSingleRow(strCmd);
                    if (drResult !=null && drResult[0].ToString() == "1")
                        return "true";
                    else
                        return "false";
                }

                if (!Roles.IsUserInRole("Level1") && !Roles.IsUserInRole("Level2")) // if not admin and not RVP and not AM return false
                    return "false";

                //From Here on we know it is a DVP (or DM who is allocating budget)


                decimal BudgetAvailableForAllocation = Convert.ToDecimal(BudgetServices.GetMyBudgetDetails(strProgramID,"0")["BudgetAllocatedToMe"].ToString());

                strCmd = "EXEC Emerge.SP_AddBudget @UserID = '" + strAllocateToUserID + "', @ProgramID =" + strProgramID + " , @Budget = " + Budget + ", @ManagerID = '" + strManagerID + "'";
                drResult = DataOperations.GetSingleRow(strCmd);
                if (drResult == null || drResult[0].ToString() != "1")                    
                    return "false";

                BudgetAvailableForAllocation = BudgetAvailableForAllocation - Budget;

                strCmd = "UPDATE Emerge.tblUserBudget SET Budget = " + BudgetAvailableForAllocation + " ";
                strCmd += "WHERE fk_User_ID = '" + strManagerID + "' AND fk_Program_ID = " + strProgramID;
                //Reducing the Manager's Budget by as much as allocated to Child User
                
                DataOperations.ExecuteSQL(strCmd);
                return "true";
            }
            catch (Exception ex)
            {
                return ex.ToString();                
            }            
        }

        public static DataTable   GetTopLevelBudgetAllocations(string strProgramID)
        {
            //string strCmd = "SELECT  l.Location as UserName, u.UserID, 0 Budget,l.*,0 AS SortWeight FROM aspnet_Users u join aspnet_UsersInRoles uir on uir.UserId = u.UserId ";
            //strCmd+="JOIN aspnet_Roles r ON r.RoleId = uir.RoleId ";      
            //strCmd+="JOIN Emerge.tblUserPortfolio up ON up.fk_User_ID = u.UserId ";
            //strCmd += "JOIN Emerge.tblLocation l ON l.pk_location_ID = up.fk_Location_ID ";
            //strCmd += "WHERE r.RoleName = 'DVP' ORDER BY 1";

            //string strCmd = "EXEC [Emerge].[SP_GetBudgetAllocations] @ProgramID = " + strProgramID;

            string strCmd = "SELECT 0 AS SortWeight, 1 AS Level, SM1.pk_Sm_Level1_id AS Level1, 0 AS Level2, 0 AS Level3, ";
            strCmd += "SM1.designation_name, U.UserName, ISNULL(SMB1.budget,0) AS Budget, ISNULL(SMB1.pk_Sm_Level1_Budget_id,0) AS Level_Budget_id ";
            strCmd += "FROM Emerge.tblSM_Level1 SM1 ";
            strCmd += "LEFT JOIN aspnet_Users U ON (U.UserId = SM1.level1_sales_manager_id) ";
            strCmd += "INNER JOIN aspnet_Membership AM ON (AM.userid = U.UserId ) AND (AM.IsLockedOut = 0) ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level1_Budget SMB1 ON (SMB1.fk_Sm_Level1_id = SM1.pk_Sm_Level1_id) AND (SMB1.fk_Program_Id = " + strProgramID + ")";
            strCmd += " WHERE SM1.fk_Program_Id = " + strProgramID;
            // To Allocate Budget to Second Level
            /*
            strCmd += "UNION ";
            strCmd += "SELECT 0 AS SortWeight, 2 AS Level, SM2.fk_Sm_Level1_id AS Level1, SM2.pk_Sm_Level2_id AS Level2, 0 AS Level3,  ";
            strCmd += "SM2.designation_name, U.UserName, ISNULL(SMB2.budget,0) AS Budget, ISNULL(SMB2.pk_Sm_Level2_Budget_id,0) AS Level_Budget_id ";
            strCmd += "FROM Emerge.tblSM_Level2 SM2 ";
            strCmd += "LEFT JOIN aspnet_Users U ON (U.UserId = SM2.level2_sales_manager_id) ";
            strCmd += "INNER JOIN aspnet_Membership AM ON (AM.userid = U.UserId ) AND (AM.IsLockedOut = 0) ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level2_Budget SMB2 ON (SMB2.fk_Sm_Level2_id = SM2.pk_Sm_Level2_id) AND (SMB2.fk_Program_Id = " + strProgramID + ")";
            */
            // To Allocate Budget to Third Level
            /*
            strCmd += "UNION ";
            strCmd += "SELECT 0 AS SortWeight, 3 AS Level, SM2.fk_Sm_Level1_id AS Level1, SM2.pk_Sm_Level2_id AS Level2, SM3.pk_Sm_Level3_id AS Level3, ";
            strCmd += "SM3.designation_name, U.UserName, ISNULL(SMB3.budget,0) AS Budget, ISNULL(SMB3.pk_Sm_Level3_Budget_id,0) AS Level_Budget_id  "; 
            strCmd += "FROM Emerge.tblSM_Level3 SM3 ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level2 SM2 ON (SM2.pk_Sm_Level2_id = SM3.fk_Sm_Level2_id) ";
            strCmd += "LEFT JOIN aspnet_Users U ON (U.UserId = SM3.level3_sales_manager_id) ";
            strCmd += "INNER JOIN aspnet_Membership AM ON (AM.userid = U.UserId ) AND (AM.IsLockedOut = 0) ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level3_Budget SMB3 ON (SMB3.fk_Sm_Level3_id = SM3.pk_Sm_Level3_id)  AND (SMB3.fk_Program_Id = " + strProgramID + ")";
            */
            strCmd += "ORDER BY 3,4,5";
            return DataOperations.GetDataTable(strCmd);
        }

        public static DataTable GetMyTeamBudgetAllocations(string strProgramID)
        {
            string strManagerID = Membership.GetUser().ProviderUserKey.ToString();
            //string strCmd = "EXEC [Emerge].[SP_GetBudgetAllocations] @ManagerID = '" + strManagerID + "', @ProgramID = " + strProgramID;
            string strCmd = "SELECT 1 AS SortWeight, 1 AS Level, SM1.pk_Sm_Level1_id AS Level1, 0 AS Level2, 0 AS Level3, ";
            strCmd += "SM1.designation_name, U.UserName, ISNULL(SMB1.budget,0) AS Budget, ISNULL(SMB1.pk_Sm_Level1_Budget_id,0) AS Level_Budget_id ";
            strCmd += ", SM1.level1_sales_manager_id AS UserID, '00000000-0000-0000-0000-000000000000' AS MgrID ";
            strCmd += "FROM (SELECT * FROM Emerge.tblSM_Level1 WHERE (fk_Program_Id = "+strProgramID+" )) SM1 ";
            strCmd += "LEFT JOIN aspnet_Users U ON (U.UserId = SM1.level1_sales_manager_id) ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level1_Budget SMB1 ON (SMB1.fk_Sm_Level1_id = SM1.pk_Sm_Level1_id) AND (SMB1.fk_Program_Id =  " + strProgramID + ")";
            strCmd += "WHERE UserId = '" + strManagerID + "' ";
            strCmd += "UNION ";
            strCmd += "SELECT 1 AS SortWeight, 2 AS Level, SM2.fk_Sm_Level1_id AS Level1, SM2.pk_Sm_Level2_id AS Level2, 0 AS Level3, ";
            strCmd += "SM2.designation_name, U.UserName, ISNULL(SMB2.budget,0) AS Budget, ISNULL(SMB2.pk_Sm_Level2_Budget_id,0) AS Level_Budget_id ";
            strCmd += ", SM2.level2_sales_manager_id AS UserID, SM1.level1_sales_manager_id AS MgrID ";
            strCmd += "FROM (SELECT * FROM Emerge.tblSM_Level2 WHERE fk_Sm_Level1_id IN (SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramID + ")) SM2 ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level1 SM1 ON (SM1.pk_Sm_Level1_id = SM2.fk_Sm_Level1_id) ";
            strCmd += "LEFT JOIN aspnet_Users U ON (U.UserId = SM2.level2_sales_manager_id) ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level2_Budget SMB2 ON (SMB2.fk_Sm_Level2_id = SM2.pk_Sm_Level2_id) AND (SMB2.fk_Program_Id = " + strProgramID + ")";
            strCmd += "WHERE UserId = '" + strManagerID + "' ";
            strCmd += "UNION ";
            strCmd += "SELECT 0 AS SortWeight, 2 AS Level, SM2.fk_Sm_Level1_id AS Level1, SM2.pk_Sm_Level2_id AS Level2, 0 AS Level3, ";
            strCmd += "SM2.designation_name, U.UserName, ISNULL(SMB2.budget,0) AS Budget, ISNULL(SMB2.pk_Sm_Level2_Budget_id,0) AS Level_Budget_id ";
            strCmd += ", SM2.level2_sales_manager_id AS UserID, SM1.level1_sales_manager_id AS MgrID ";
            strCmd += "FROM (SELECT * FROM Emerge.tblSM_Level2 WHERE fk_Sm_Level1_id IN (SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramID + ")) SM2 ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level1 SM1 ON (SM1.pk_Sm_Level1_id = SM2.fk_Sm_Level1_id) ";
            strCmd += "LEFT JOIN aspnet_Users U ON (U.UserId = SM2.level2_sales_manager_id) ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level2_Budget SMB2 ON (SMB2.fk_Sm_Level2_id = SM2.pk_Sm_Level2_id) AND (SMB2.fk_Program_Id =  " + strProgramID + ")"; ;
            strCmd += "WHERE SM1.level1_sales_manager_id = '" + strManagerID + "' ";
            strCmd += "UNION ";
            strCmd += "SELECT 1 AS SortWeight, 3 AS Level, SM2.fk_Sm_Level1_id AS Level1, SM2.pk_Sm_Level2_id AS Level2, SM3.pk_Sm_Level3_id AS Level3, ";
            strCmd += "SM3.designation_name, U.UserName, ISNULL(SMB3.budget,0) AS Budget, ISNULL(SMB3.pk_Sm_Level3_Budget_id,0) AS Level_Budget_id ";
            strCmd += ", SM3.level3_sales_manager_id AS UserID, SM2.level2_sales_manager_id AS [MgrID] ";
            strCmd += "FROM (SELECT * FROM Emerge.tblSM_Level3 WHERE  fk_Sm_Level2_id IN (SELECT pk_Sm_Level2_id FROM Emerge.tblSM_Level2 WHERE fk_Sm_Level1_id IN (SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramID + "))) SM3 ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level2 SM2 ON (SM2.pk_Sm_Level2_id = SM3.fk_Sm_Level2_id) ";
            strCmd += "LEFT JOIN aspnet_Users U ON (U.UserId = SM3.level3_sales_manager_id) ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level3_Budget SMB3 ON (SMB3.fk_Sm_Level3_id = SM3.pk_Sm_Level3_id)  AND (SMB3.fk_Program_Id = " + strProgramID + ")";
            strCmd += "WHERE UserId = '" + strManagerID + "' ";
            strCmd += "UNION ";
            strCmd += "SELECT 0 AS SortWeight, 3 AS Level, SM2.fk_Sm_Level1_id AS Level1, SM2.pk_Sm_Level2_id AS Level2, SM3.pk_Sm_Level3_id AS Level3, ";
            strCmd += "SM3.designation_name, U.UserName, ISNULL(SMB3.budget,0) AS Budget, ISNULL(SMB3.pk_Sm_Level3_Budget_id,0) AS Level_Budget_id ";
            strCmd += ", SM3.level3_sales_manager_id AS UserID, SM2.level2_sales_manager_id AS [MgrID] ";
            strCmd += "FROM (SELECT * FROM Emerge.tblSM_Level3 WHERE  fk_Sm_Level2_id IN (SELECT pk_Sm_Level2_id FROM Emerge.tblSM_Level2 WHERE fk_Sm_Level1_id IN (SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramID + "))) SM3 ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level2 SM2 ON (SM2.pk_Sm_Level2_id = SM3.fk_Sm_Level2_id) ";
            strCmd += "LEFT JOIN aspnet_Users U ON (U.UserId = SM3.level3_sales_manager_id) ";
            strCmd += "LEFT JOIN Emerge.tblSM_Level3_Budget SMB3 ON (SMB3.fk_Sm_Level3_id = SM3.pk_Sm_Level3_id)  AND (SMB3.fk_Program_Id =  " + strProgramID + ")";
            strCmd += "WHERE SM2.level2_sales_manager_id = '" + strManagerID + "' ";
            strCmd += "ORDER BY 3,4,5 ";

            return DataOperations.GetDataTable(strCmd);
        }

        public static DataTable GetDVPList() // GetLevel1List(), this is used in Reports
        {
            string strCmd = "";
            //strCmd = @"SELECT	u.UserName, u.UserId FROM aspnet_Users u 		
		    //            join aspnet_UsersInRoles uir on uir.UserId = u.UserId 
		    //            JOIN aspnet_Roles r ON r.RoleId = uir.RoleId 		
		    //            WHERE r.RoleName = 'Level1'";
            strCmd = @"SELECT U.UserName, U.UserId FROM Emerge.tblSM_Level1 SM1
                        LEFT JOIN aspnet_Users U ON ( U.UserId = SM1.level1_sales_manager_id)";
            return DataOperations.GetDataTable(strCmd); 	
        }


        public static string IsProgramStarted(string strProgramID)
        {
            string strCmd = "";
            strCmd = "SELECT * FROM Emerge.tblProgram p WHERE OrderWindowStart <= GETDATE() AND p.pk_Program_Id =  "+strProgramID;
            DataRow drProgramStarted = DataOperations.GetSingleRow(strCmd);
            if (drProgramStarted == null)
                return "No";
            else
                return "Yes";
        }


        public static string AddProgramBudgetToPosition(string level, string level1, string level2, string level3, string strBudRecID, string strProgramID, decimal Budget)
        {
            try
            {
                string strManagerID = Membership.GetUser().ProviderUserKey.ToString();
                string strCmd = "";
                DataRow drResult = null;

                //if (Roles.IsUserInRole("Admin")) // if admin no need to check current user's budget first
                if (Roles.IsUserInRole("Admin") || Roles.IsUserInRole("MM"))
                {
                    strCmd = "EXEC Emerge.SP_AddPosBudget @BudRecID = '" + strBudRecID + "', @ProgramID =" + strProgramID + " , @Budget = " + Budget + ", @ManagerID = '" + strManagerID + "', @Level = " + level + ", @Level1 = " + level1 + ", @Level2 = " + level2 + ", @Level3 = " + level3;
                    drResult = DataOperations.GetSingleRow(strCmd);
                    if (drResult != null && drResult[0].ToString() == "1")
                        return "true";
                    else
                        return "false";
                }

                if (!AdminServices.IsUserInLevel("Level1") && !AdminServices.IsUserInLevel("Level2")) // if not admin and not RVP and not AM return false
                    return "false";

                //From Here on we know it is a DVP (or DM who is allocating budget)
                decimal BudgetAvailableForAllocation = Convert.ToDecimal(BudgetServices.GetMyBudgetDetails(strProgramID, "0")["BudgetAllocatedToMe"].ToString());
                strCmd = "EXEC Emerge.SP_AddPosBudget @BudRecID = '" + strBudRecID + "', @ProgramID =" + strProgramID + " , @Budget = " + Budget + ", @ManagerID = '" + strManagerID + "', @Level = " + level + ", @Level1 = " + level1 + ", @Level2 = " + level2 + ", @Level3 = " + level3;
                    
                drResult = DataOperations.GetSingleRow(strCmd);
                if (drResult == null || drResult[0].ToString() != "1")
                    return "false";
                return "true";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static DataRow GetBudgetForProgram(string strProgramID)
        {
            string strCmd = "";
            strCmd += " SELECT ISNULL(Budget,0) AS Budget FROM Emerge.tblProgramBudget WHERE fk_Program_ID = " + strProgramID;
            DataRow drProgramBudget = DataOperations.GetSingleRow(strCmd);
            return drProgramBudget;
        }


        public static string AddProgramBudget(string strProgramID, decimal Budget)
        {
            string returnMessage;
            try
            {
                string strUserID = Membership.GetUser().ProviderUserKey.ToString();
                string strCmd = "";
                strCmd = "IF NOT EXISTS (SELECT * FROM Emerge.tblProgramBudget WHERE fk_Program_ID = " + strProgramID + ") ";
                strCmd += "INSERT INTO Emerge.tblProgramBudget(fk_Program_ID,Budget,fk_LastUpdatedBy,LastUpdatedOn) ";
                strCmd += "VALUES (" + strProgramID + ", " + Budget.ToString() + ", '" + strUserID + "', GETDATE()) ";
                strCmd += "ELSE ";
                strCmd += "UPDATE Emerge.tblProgramBudget SET Budget = " + Budget.ToString() + ", fk_LastUpdatedBy = '" + strUserID + "', LastUpdatedOn = GETDATE() ";
                strCmd += " WHERE fk_Program_ID =" + strProgramID;
                DataOperations.ExecuteSQL(strCmd);
                strCmd = "INSERT INTO Emerge.tblAud_ProgramBudget (fk_Program_ID, fk_LoggedinUser,BudgetValue) ";
                strCmd += "VALUES ( " + strProgramID + ", '" + strUserID + "', " + Budget.ToString() + ")";
                DataOperations.ExecuteSQL(strCmd);

                returnMessage = "Updated";
            }
            catch (Exception ex)
            {
                returnMessage = ex.ToString();
            }
            return returnMessage;
        }

        public static DataRow GetBudgetUtilizedForProgram(string strProgramID)
        {
            string strCmd = "";
            strCmd += "SELECT ISNULL(SUM(budget_Allocated),0) AS BudgetUtilized FROM Emerge.tblSM_Level1_Budget WHERE fk_Program_id  = " + strProgramID;
            DataRow drProgramUtilizedBudget = DataOperations.GetSingleRow(strCmd);
            return drProgramUtilizedBudget;
        }

        public static DataRow GetMyAllocatedBudget(string strProgramID)
        {
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            string strCmd = "";
            strCmd += "SELECT budget_Allocated FROM ( ";
            strCmd += "SELECT budget_Allocated, SM1B.fk_Program_id,level1_sales_manager_id AS Usrid FROM Emerge.tblSM_Level1_Budget SM1B LEFT JOIN Emerge.tblSM_Level1 SM1 ON (SM1.pk_Sm_Level1_id = SM1B.fk_Sm_Level1_id) ";
            strCmd += "UNION ";
            strCmd += "SELECT budget_Allocated, SM2B.fk_Program_id,level2_sales_manager_id AS Usrid FROM Emerge.tblSM_Level2_Budget SM2B LEFT JOIN Emerge.tblSM_Level2 SM2 ON (SM2.pk_Sm_Level2_id = SM2B.fk_Sm_Level2_id) ";
            strCmd += "UNION ";
            strCmd += "SELECT budget_Allocated, SM3B.fk_Program_id,level3_sales_manager_id AS Usrid FROM Emerge.tblSM_Level3_Budget SM3B LEFT JOIN Emerge.tblSM_Level3 SM3 ON (SM3.pk_Sm_Level3_id = SM3B.fk_Sm_Level3_id) ";
            strCmd += ") AlloBud ";
            strCmd += "WHERE fk_Program_id = " + strProgramID + " AND Usrid = '" + userId + "'";
            DataRow drMyAllocatedBudget = DataOperations.GetSingleRow(strCmd);
            return drMyAllocatedBudget;
        } 
    }
}