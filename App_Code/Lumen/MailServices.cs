using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using ACE;
using System.Data;

namespace Lumen
{
    public static class MailServices
    {
        public static bool SendOrderConfirmationEmailToSM(string strOrderID)
        {
            try
            {
                string strCmd = @"SELECT U.UserName, M.Email, O.Status FROM EMERGE.tblORDER O 
                            JOIN aspnet_Membership M ON M.UserId = O.fk_OrderPlacedBy 
                            JOIN aspnet_Users U ON U.UserId = M.UserId WHERE pk_Order_ID = ";
                strCmd += strOrderID;

                DataRow drOrderedBy = DataOperations.GetSingleRow(strCmd);

                if (drOrderedBy["Status"].ToString() == "PENDAPPROVAL")
                    return SendPendingApprovalToSM(strOrderID);

                string strEmailLibrarySQL = "SELECT * FROM Emerge.tblEmailLibrary WHERE EmailName = 'OrderConfirmation'";
                DataRow drEmail = DataOperations.GetSingleRow(strEmailLibrarySQL);
                if (drEmail == null) return false;
                string strSubject = drEmail["Subject"].ToString();
                string strBody = drEmail["HtmlBody"].ToString();

                strSubject = MailMerge.SimpleMerge(strSubject, "@OrderNo", strOrderID);

                GmailSMTP objGmailSmtp = new GmailSMTP(false);

                List<string> toAddresses = new List<string>();
                toAddresses.Add(drOrderedBy["Email"].ToString());

                List<string> bccAddresses = new List<string>();
                List<string> ccAddresses = new List<string>();
                //bccAddresses.Add("gj@ambood.com");
                //bccAddresses.Add("deepak@uxl-technologies.com");

                string strTokens = "@UserName,@OrderNo";
                string strValues = drOrderedBy["UserName"].ToString() + "," + strOrderID;
                strBody = MailMerge.SimpleMerge(strBody, strTokens, strValues);

                //return objGmailSmtp.SendMail(toAddresses, strSubject, strBody, ccAddresses, bccAddresses);
                return false;
            }
            catch (Exception ex)
            {
                LogAndTrace.CodeTrace(ex.ToString());
                return false;
            }
        }

        public static bool SendNewOrderPlacedEmailToBMs(string strOrderID)
        {
            try
            {
                string strCmdOrderStatus = @"SELECT Status FROM EMERGE.tblORDER WHERE pk_Order_ID = " + strOrderID;
                DataRow drOrderStatus = DataOperations.GetSingleRow(strCmdOrderStatus);

                if (drOrderStatus["Status"].ToString() == "PENDAPPROVAL")
                    return SendOrderForApprovalToBMs(strOrderID);

                string strEmailLibrarySQL = "SELECT * FROM Emerge.tblEmailLibrary WHERE EmailName = 'NewOrderPlaced'";
                DataRow drEmail = DataOperations.GetSingleRow(strEmailLibrarySQL);
                if (drEmail == null) return false;
                string strSubject = drEmail["Subject"].ToString();
                string strBody = drEmail["HtmlBody"].ToString();

                strSubject = MailMerge.SimpleMerge(strSubject, "@OrderNo", strOrderID);

                GmailSMTP objGmailSmtp = new GmailSMTP(false);

                string strCmdOrderDetails = @"SELECT U.UserName OrderedBy, O.OrderDate FROM EMERGE.tblORDER O                             
                                JOIN aspnet_Users U ON U.UserId = O.fk_OrderPlacedBy WHERE pk_Order_ID = ";
                strCmdOrderDetails += strOrderID;

                DataRow drOrderedBy = DataOperations.GetSingleRow(strCmdOrderDetails);

                string strCmdOrderBrandManagers = @"SELECT DISTINCT M.Email BMEmail, U.UserName BMName FROM Emerge.tblOrderDestination OD 
                                                JOIN Emerge.tblOrderDestinationItem ODI ON OD.pk_OrderDestination_ID = ODI.fk_OrderDestination_ID
                                                JOIN Emerge.tblItem I ON i.pk_Item_ID = ODI.fk_Item_ID 
                                                JOIN Emerge.tblApprover BM ON BM.fk_Brand_id = I.fk_BrandId
                                                JOIN aspnet_Membership M on M.UserId = BM.fk_Approver
                                                JOIN aspnet_Users U ON u.UserId = M.UserId 
                                                WHERE OD.fk_Order_ID = ";
                strCmdOrderBrandManagers += strOrderID;
                
                List<string> bccAddresses = new List<string>();
                List<string> ccAddresses = new List<string>();
                //bccAddresses.Add("gj@ambood.com");
                //bccAddresses.Add("deepak@uxl-technologies.com");

                DataTable dtBrandManagers = DataOperations.GetDataTable(strCmdOrderBrandManagers);
                List<string> toAddresses = new List<string>();
                foreach (DataRow dr in dtBrandManagers.Rows)
                {
                    toAddresses.Clear();
                    toAddresses.Add(dr["BMEmail"].ToString());

                    string strTokens = "@BMName,@OrderedBy,@OrderDate";
                    string strValues = dr["BMName"].ToString() + "," + drOrderedBy["OrderedBy"].ToString() + "," + drOrderedBy["OrderDate"].ToString();
                    strBody = MailMerge.SimpleMerge(strBody, strTokens, strValues);

                    objGmailSmtp.SendMail(toAddresses, strSubject, strBody, ccAddresses, bccAddresses);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogAndTrace.CodeTrace(ex.ToString());
                return false;
            }
        }

        public static bool SendPendingApprovalToSM(string strOrderID)
        {
            string strCmd = @"SELECT U.UserName, M.Email FROM EMERGE.tblORDER O 
                            JOIN aspnet_Membership M ON M.UserId = O.fk_OrderPlacedBy 
                            JOIN aspnet_Users U ON U.UserId = M.UserId WHERE pk_Order_ID = ";
            strCmd += strOrderID;

            DataRow drOrderedBy = DataOperations.GetSingleRow(strCmd);

            string strEmailLibrarySQL = "SELECT * FROM Emerge.tblEmailLibrary WHERE EmailName = 'PendingApproval'";
            DataRow drEmail = DataOperations.GetSingleRow(strEmailLibrarySQL);
            if (drEmail == null) return false;

            string strSubject = drEmail["Subject"].ToString();
            string strBody = drEmail["HtmlBody"].ToString();

            strSubject = MailMerge.SimpleMerge(strSubject, "@OrderNo", strOrderID);

            GmailSMTP objGmailSmtp = new GmailSMTP(false);

            List<string> toAddresses = new List<string>();
            toAddresses.Add(drOrderedBy["Email"].ToString());

            List<string> bccAddresses = new List<string>();
            List<string> ccAddresses = new List<string>();
            //bccAddresses.Add("gj@ambood.com");
            //bccAddresses.Add("deepak@uxl-technologies.com");

            string strTokens = "@UserName,@OrderNo";
            string strValues = drOrderedBy["UserName"].ToString() + "," + strOrderID;
            strBody = MailMerge.SimpleMerge(strBody, strTokens, strValues);

            return objGmailSmtp.SendMail(toAddresses, strSubject, strBody, ccAddresses, bccAddresses);
        }

        public static bool SendOrderForApprovalToBMs(string strOrderID)
        {
            try
            {
                string strEmailLibrarySQL = "SELECT * FROM Emerge.tblEmailLibrary WHERE EmailName = 'NeedApproval'";
                DataRow drEmail = DataOperations.GetSingleRow(strEmailLibrarySQL);
                if (drEmail == null) return false;
                string strSubject = drEmail["Subject"].ToString();
                string strBody = drEmail["HtmlBody"].ToString();

                strSubject = MailMerge.SimpleMerge(strSubject, "@OrderNo", strOrderID);

                GmailSMTP objGmailSmtp = new GmailSMTP(false);

                string strCmdOrderDetails = @"SELECT U.UserName OrderedBy, O.OrderDate FROM EMERGE.tblORDER O                             
                                JOIN aspnet_Users U ON U.UserId = O.fk_OrderPlacedBy WHERE pk_Order_ID = ";
                strCmdOrderDetails += strOrderID;

                DataRow drOrderedBy = DataOperations.GetSingleRow(strCmdOrderDetails);

                string strCmdOrderBrandManagers = @"SELECT DISTINCT M.Email BMEmail, U.UserName BMName FROM Emerge.tblOrderDestination OD 
                                                JOIN Emerge.tblOrderDestinationItem ODI ON OD.pk_OrderDestination_ID = ODI.fk_OrderDestination_ID
                                                JOIN Emerge.tblItem I ON i.pk_Item_ID = ODI.fk_Item_ID 
                                                JOIN Emerge.tblApprover BM ON BM.fk_Brand_id = I.fk_BrandId
                                                JOIN aspnet_Membership M on M.UserId = BM.fk_Approver
                                                JOIN aspnet_Users U ON u.UserId = M.UserId 
                                                WHERE OD.fk_Order_ID = ";
                strCmdOrderBrandManagers += strOrderID;

                List<string> bccAddresses = new List<string>();
                List<string> ccAddresses = new List<string>();
                //bccAddresses.Add("gj@ambood.com");
                //bccAddresses.Add("deepak@uxl-technologies.com");

                DataTable dtBrandManagers = DataOperations.GetDataTable(strCmdOrderBrandManagers);
                List<string> toAddresses = new List<string>();
                foreach (DataRow dr in dtBrandManagers.Rows)
                {
                    toAddresses.Clear();
                    toAddresses.Add(dr["BMEmail"].ToString());

                    string strTokens = "@BMName,@OrderedBy,@OrderDate";
                    string strValues = dr["BMName"].ToString() + "," + drOrderedBy["OrderedBy"].ToString() + "," + drOrderedBy["OrderDate"].ToString();
                    strBody = MailMerge.SimpleMerge(strBody, strTokens, strValues);

                    objGmailSmtp.SendMail(toAddresses, strSubject, strBody, ccAddresses, bccAddresses);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogAndTrace.CodeTrace(ex.ToString());
                return false;
            }
        }

// *********************** Order Approval Notifications***************************************
        public static bool SendOrderApprovalStatusToSM(string strOrderID)
        {
            string strOrderDetailsSQL = @"SELECT O.Status, U.UserName, M.Email FROM Emerge.tblOrder O
                                        JOIN aspnet_Membership M ON O.fk_OrderPlacedBy = M.UserId
                                        JOIN aspnet_Users U ON U.UserId = M.UserId WHERE pk_Order_ID = ";
            strOrderDetailsSQL += strOrderID;
            DataRow drOrderDetails = DataOperations.GetSingleRow(strOrderDetailsSQL);

            if (drOrderDetails["Status"].ToString() == "PENDAPPROVAL") //If still pending Approval just return
                return false;

            string strEmailLibrarySQL = "SELECT * FROM Emerge.tblEmailLibrary WHERE EmailName = 'OrderApproved'";
            DataRow drEmail = DataOperations.GetSingleRow(strEmailLibrarySQL);
            if (drEmail == null) return false;

            string strSubject = drEmail["Subject"].ToString();
            string strBody = drEmail["HtmlBody"].ToString();

            strSubject = MailMerge.SimpleMerge(strSubject, "@OrderNo", strOrderID);

            GmailSMTP objGmailSmtp = new GmailSMTP(false);

            List<string> toAddresses = new List<string>();
            toAddresses.Add(drOrderDetails["Email"].ToString());

            List<string> bccAddresses = new List<string>();
            List<string> ccAddresses = new List<string>();
            //bccAddresses.Add("gj@ambood.com");
            //bccAddresses.Add("deepak@uxl-technologies.com");

            string strTableHTML = GetApprovedOrderSummaryHTML(strOrderID);
            strTableHTML = strTableHTML.Replace(",", "#Comma#");

            string strTokens = "@UserName,@OrderNo,@TableHTML";
            string strValues = drOrderDetails["UserName"].ToString() + "," + strOrderID + "," + strTableHTML;
            strBody = MailMerge.SimpleMerge(strBody, strTokens, strValues);
            strBody = strBody.Replace("#Comma#", ",");

            LogAndTrace.CodeTrace(System.DateTime.Now.ToString());
            LogAndTrace.CodeTrace(strBody);
            LogAndTrace.CodeTrace("------");
            return objGmailSmtp.SendMail(toAddresses, strSubject, strBody, ccAddresses, bccAddresses);
        }

        public static string GetApprovedOrderSummaryHTML(string strOrderID)
        {
            try
            {

                string strCmd = @"select I.Item_Name, A.destination_name, ODI.quantity QuantityOrdered, ODI.ApprovedQuantity from Emerge.tblOrder O 
                            JOIN Emerge.tblOrderDestination OD ON OD.fk_Order_ID = O.pk_Order_ID
                            JOIN Emerge.tblOrderDestinationItem ODI ON OD.pk_OrderDestination_ID = ODI.fk_OrderDestination_ID
                            JOIN Emerge.tblItem I on I.pk_Item_ID = ODI.fk_Item_ID
                            JOIN Emerge.tblAddress A ON A.pk_address_id = OD.fk_Address_ID WHERE O.pk_Order_ID = ";
                strCmd += strOrderID;

                DataTable dt = DataOperations.GetDataTable(strCmd);

                string strApprovedOrderSummaryHTML = "<table border='1'><tr><td>Item</td><td>Destination</td><td>Approved Quantity</td></tr>";

                foreach (DataRow dr in dt.Rows)
                {
                    string strApprovedQuantity = dr["ApprovedQuantity"].ToString() + " of " + dr["QuantityOrdered"].ToString();
                    strApprovedOrderSummaryHTML += "<tr><td>" + dr["Item_Name"].ToString() + "</td><td>" + dr["destination_name"].ToString() + "</td><td>" + strApprovedQuantity + "</td></tr>";
                }
                strApprovedOrderSummaryHTML += "</table>";
                return strApprovedOrderSummaryHTML;
            }
            catch (Exception ex)
            {
                return "Sorry, Order Details not available. A server error occured.";
            }
        }


// *********************** Seasonal Order Notifications ***************************************

        public static bool SendSeasonalOrderConfirmationEmailToSM(string strOrderID)
        {
            try
            {
                string strCmdOrderDetails = @"SELECT U.UserName, M.Email, P.Name Program, O.Status FROM EMERGE.tblORDER O 
							JOIN emerge.tblProgram P ON P.pk_Program_Id = O.fk_Program_ID
                            JOIN aspnet_Membership M ON M.UserId = O.fk_OrderPlacedBy 
                            JOIN aspnet_Users U ON U.UserId = M.UserId WHERE pk_Order_ID = ";
                strCmdOrderDetails += strOrderID;

                DataRow drOrderDetails = DataOperations.GetSingleRow(strCmdOrderDetails);

                string strEmailLibrarySQL = "SELECT * FROM Emerge.tblEmailLibrary WHERE EmailName = 'SeasonalOrderPlaced'";
                DataRow drEmail = DataOperations.GetSingleRow(strEmailLibrarySQL);
                if (drEmail == null) return false;
                string strSubject = drEmail["Subject"].ToString();
                string strBody = drEmail["HtmlBody"].ToString();

                strSubject = MailMerge.SimpleMerge(strSubject, "@OrderNo", strOrderID);

                GmailSMTP objGmailSmtp = new GmailSMTP(false);

                List<string> toAddresses = new List<string>();
                toAddresses.Add(drOrderDetails["Email"].ToString());

                List<string> bccAddresses = new List<string>();
                List<string> ccAddresses = new List<string>();
                //bccAddresses.Add("gj@ambood.com");
                //bccAddresses.Add("deepak@uxl-technologies.com");
                string strProgramName = drOrderDetails["Program"].ToString();
                strProgramName = strProgramName.Replace(",", "#Comma#");


                string strTokens = "@UserName,@OrderNo,@Program";
                string strValues = drOrderDetails["UserName"].ToString() + "," + strOrderID + "," + strProgramName;
                strBody = MailMerge.SimpleMerge(strBody, strTokens, strValues);
                strBody = strBody.Replace("#Comma#", ",");

                return objGmailSmtp.SendMail(toAddresses, strSubject, strBody, ccAddresses, bccAddresses);
            }
            catch (Exception ex)
            {
                LogAndTrace.CodeTrace(ex.ToString());
                return false;
            }
        }

        public static bool SendNewSeasonalOrderPlacedEmailToBMs(string strOrderID)
        {
            try
            {
                string strCmdOrderDetails = @"SELECT U.UserName, M.Email, P.Name Program, O.Status FROM EMERGE.tblORDER O 
							JOIN emerge.tblProgram P ON P.pk_Program_Id = O.fk_Program_ID
                            JOIN aspnet_Membership M ON M.UserId = O.fk_OrderPlacedBy 
                            JOIN aspnet_Users U ON U.UserId = M.UserId WHERE pk_Order_ID = ";
                strCmdOrderDetails += strOrderID;

                DataRow drOrderDetails = DataOperations.GetSingleRow(strCmdOrderDetails);

                string strEmailLibrarySQL = "SELECT * FROM Emerge.tblEmailLibrary WHERE EmailName = 'NewSeasonalOrderPlaced'";
                DataRow drEmail = DataOperations.GetSingleRow(strEmailLibrarySQL);
                if (drEmail == null) return false;
                string strSubject = drEmail["Subject"].ToString();
                string strBody = drEmail["HtmlBody"].ToString();

                strSubject = MailMerge.SimpleMerge(strSubject, "@OrderNo", strOrderID);

                GmailSMTP objGmailSmtp = new GmailSMTP(false);

                string strCmdOrderBrandManagers = @"SELECT DISTINCT M.Email BMEmail, U.UserName BMName FROM Emerge.tblOrderDestination OD 
                                                JOIN Emerge.tblOrderDestinationItem ODI ON OD.pk_OrderDestination_ID = ODI.fk_OrderDestination_ID
                                                JOIN Emerge.tblItem I ON i.pk_Item_ID = ODI.fk_Item_ID 
                                                JOIN Emerge.tblApprover BM ON BM.fk_Brand_id = I.fk_BrandId
                                                JOIN aspnet_Membership M on M.UserId = BM.fk_Approver
                                                JOIN aspnet_Users U ON u.UserId = M.UserId 
                                                WHERE OD.fk_Order_ID = ";
                strCmdOrderBrandManagers += strOrderID;
                DataTable dtBrandManagers = DataOperations.GetDataTable(strCmdOrderBrandManagers);

                List<string> bccAddresses = new List<string>();
                List<string> ccAddresses = new List<string>();
                //bccAddresses.Add("gj@ambood.com");
                //bccAddresses.Add("deepak@uxl-technologies.com");
                                
                List<string> toAddresses = new List<string>();
                foreach (DataRow dr in dtBrandManagers.Rows)
                {
                    toAddresses.Clear();
                    toAddresses.Add(dr["BMEmail"].ToString());

                    string strProgramName = drOrderDetails["Program"].ToString();
                    strProgramName = strProgramName.Replace(",", "#Comma#");
                    string strTokens = "@BMName,@OrderNo,@Program";
                    string strValues = dr["BMName"].ToString() + "," + strOrderID + "," + strProgramName;
                    strBody = MailMerge.SimpleMerge(strBody, strTokens, strValues);
                    strBody = strBody.Replace("#Comma#", ",");

                    objGmailSmtp.SendMail(toAddresses, strSubject, strBody, ccAddresses, bccAddresses);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogAndTrace.CodeTrace(ex.ToString());
                return false;
            }
        }


       /**************** Membership Emails ****************/
        public static bool SendPasswordEmail(string strUserName)
        {
            MembershipUser mu = Membership.GetUser(strUserName);
            mu.UnlockUser();
            string strEmail = mu.Email;
            string strPwd = mu.GetPassword();
            mu.ChangePassword(strPwd, "password!");
            strPwd = "password!";
            string strURL = "http://so.dfvwines.com/login.aspx";

            string strSubject = "DFV Wines Seasonal Orders - Emerge Online POS - Password";

            System.IO.StreamReader sr = new System.IO.StreamReader(System.Web.HttpContext.Current.Server.MapPath(@"~/Inc/Files/PasswordReminder.htm"));
            string strBody = sr.ReadToEnd();
            sr.Close();

            strBody = String.Format(strBody, strUserName, strPwd, strURL);

            GmailSMTP objGmailSmtp = new GmailSMTP(false);
            //objGmailSmtp.SendMail("deepak@ambood.com", "BCC:" + strSubject, strBody);
            return objGmailSmtp.SendMail(strEmail, strSubject, strBody);
        }
    }
}