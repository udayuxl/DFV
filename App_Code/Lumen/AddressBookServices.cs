using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using ACE;
using System.Data;

namespace Lumen
{
    public static class AddressBookServices
    {
        public static void SaveAddress(string strAddressID, string strShipToName, string strShipToCompany, string strAddress1, string strAddress2, string strCity, string strDestinationCode, string strState, string strZIP, string strPhone, string strEmail, string strActive, string strRegion)
        {
            if (String.IsNullOrEmpty(strDestinationCode))
                strDestinationCode = strShipToName + "-" + strCity;
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();
            string strCommand = "";
            if (strAddressID == "0")
            {
                strCommand = "INSERT INTO Emerge.tblAddress ";
                strCommand += "(shipto_name,shipto_company,address1,address2,city,destination_name,[state],zip,phone,email,active,fk_User_ID,last_updated_by,last_updated_on, fk_Location_id) ";
                strCommand += "VALUES('" + strShipToName.Replace("'", "''") + "','" + strShipToCompany.Replace("'", "''") + "',";
                strCommand += "'" + strAddress1.Replace("'", "''") + "','" + strAddress2.Replace("'", "''") + "',";
                strCommand += "'" + strCity.Replace("'", "''") + "','" + strDestinationCode.Replace("'", "''") + "',";
                strCommand += "'" + strState + "','" + strZIP + "',";
                strCommand += "'" + strPhone + "','" + strEmail + "'," + strActive + ",'" + strUserID + "','" + strUserID + "',GETDATE(), " + strRegion + ")";
            }
            else
            {
                strCommand = "UPDATE Emerge.tblAddress ";
                strCommand += "SET shipto_name = '" + strShipToName.Replace("'", "''") + "', shipto_company = '" + strShipToCompany.Replace("'", "''") + "', ";
                strCommand += "address1 = '" + strAddress1.Replace("'", "''") + "',address2='" + strAddress2.Replace("'", "''") + "', ";
                strCommand += "city='" + strCity.Replace("'", "''") + "',state='" + strState + "',zip='" + strZIP + "',phone='" + strPhone + "',email='" + strEmail + "', ";
                strCommand += "destination_name='" + strDestinationCode.Replace("'", "''") + "', ";
                strCommand += "active=" + strActive + ",last_updated_by='" + strUserID + "',last_updated_on=GETDATE() ";
                strCommand += ", fk_Location_id=" + strRegion +" ";
                strCommand += "WHERE pk_address_id = " + strAddressID;

            }
            DataOperations.ExecuteSQL(strCommand);
  
        }

        public static DataTable GetAddressBook(bool boolOrderFlow, string strSearchTerm)
        {
            string strUserID = Membership.GetUser().ProviderUserKey.ToString();

            string strCommand = "EXEC Emerge.SP_GetAddressBook @UserID ='" + strUserID + "', @SearchTerm ='" + strSearchTerm + "'";

            return DataOperations.GetDataTable(strCommand);
            
        }
    
        public static bool IsAddressUsedInOrder(string strOrderID, string strAddressID)
        {
            if (strOrderID == null)
                return false;
            string strCmd = "SELECT COUNT(*) FROM Emerge.tblOrderDestination ";
            strCmd +=  "WHERE fk_Order_ID = " + strOrderID + " AND fk_Address_ID = " + strAddressID;
            DataRow drAddressCount = DataOperations.GetSingleRow(strCmd);
            return Convert.ToBoolean(drAddressCount[0]);
        }
    }
}