using System;
using System.Configuration;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

    public static class LogAndTrace
    {
        public static void CodeTrace(string msg, Page page)
        {
            HttpContext.Current.Response.Write("<div style='display:none'>" + msg + "</div>");
            HtmlGenericControl divError = page.Master.FindControl("divError") as HtmlGenericControl;
            divError.InnerHtml = msg;
            return;
        }


        public static void CodeTrace(string msg)
        {
            string strLogFilePath = HttpContext.Current.Request.MapPath(@"~\Logs\");

            if (!Directory.Exists(strLogFilePath))
                Directory.CreateDirectory(strLogFilePath);

            StreamWriter sw = new StreamWriter(strLogFilePath + "CodeTrace.log", true);
            sw.WriteLine(msg);
            sw.Close();
        }

        public static void SendErrorEmail(Exception exc)
        {
            string UserName = HttpContext.Current.User.Identity.Name;
            string strException = exc.ToString();
            string strErrorMailBody = "<p>Username: " + UserName + "</p>";
            strErrorMailBody += "<div style=color:red>" + exc.Message + "</div>";
            strErrorMailBody += "<div>" + exc.StackTrace + "</div> <br/>";
            strErrorMailBody += "<div>" + exc.Source + "</div>";


            Lumen.GmailSMTP objGmail = new Lumen.GmailSMTP(false);
            objGmail.SendMail("uday@ambood.com", "Exception:DFV Seasonal Orders", strErrorMailBody);
        }

    }
