using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACE;

namespace Lumen
{
    public static class LookupServices
    {
        public static string GetProgramName(string strProgramId)
        {
            return DataOperations.GetSingleRow("SELECT Name from Emerge.tblProgram WHERE pk_Program_ID=" + strProgramId)[0].ToString();
        }

        public static string GetProgramPreviewImageFileName(string strProgramId)
        {
            string strCommand = "SELECT ISNULL(PreviewImageFileName,'NoImage.jpg') FileName from Emerge.tblProgram p WHERE pk_Program_Id = " + strProgramId;
            //string strCommand = "SELECT ISNULL(IL.FileName,'NoImage.jpg') FileName from Emerge.tblProgram p ";
            //strCommand += "LEFT JOIN Emerge.tblImageLibrary il ON il.pk_Image_ID = p.fk_Image_ID ";
            //strCommand += "WHERE pk_Program_Id = " + strProgramId;
            return DataOperations.GetSingleRow(strCommand)[0].ToString();            
        }

        public static string GetItemName(string strItemId)
        {
            return DataOperations.GetSingleRow("SELECT Item_Name from Emerge.tblItem WHERE pk_Item_ID = " + strItemId)[0].ToString();
        }

        public static string GetItemPreviewImageFileName(string strItemId)
        {
            string strCommand = "SELECT ISNULL(PreviewImageFileName,'NoImage.jpg') FileName from Emerge.tblItem WHERE pk_Item_Id = " + strItemId;
            //string strCommand = "SELECT IL.FileName from Emerge.tblItem i ";
            //strCommand += "INNER JOIN Emerge.tblImageLibrary il ON il.pk_Image_ID = i.fk_Image_ID ";
            //strCommand += "WHERE pk_Item_Id = " + strItemId;
            return DataOperations.GetSingleRow(strCommand)[0].ToString();            
        }
    }
}