using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Lumen
{
  public class GmailSMTP
  {
    private SmtpClient mailclient;
    private string fromaddress;
    private bool mAsync;
    private string sMode = "LOCAL";// LIVE/STAGE/LOCAL

    public GmailSMTP(string gmailaddress, string password, bool Async)
    {
      this.mailclient = new SmtpClient();
      this.mailclient.Host = "smtp.gmail.com";
      this.mailclient.Port = 587;
      this.mailclient.Credentials = (ICredentialsByHost) new NetworkCredential(gmailaddress, password);
      this.fromaddress = gmailaddress;
      this.mailclient.EnableSsl = true;
      this.mAsync = Async;
    }

    public GmailSMTP(bool Async)
    {
      string userName = ConfigurationManager.AppSettings["gmailaddress"];
      string password = ConfigurationManager.AppSettings["gmailpassword"];
      this.mailclient = new SmtpClient();
      this.mailclient.Host = "smtp.gmail.com";
      this.mailclient.Port = 587;
      this.mailclient.Credentials = (ICredentialsByHost) new NetworkCredential(userName, password);
      this.fromaddress = userName;
      this.mailclient.EnableSsl = true;
      this.mAsync = Async;
    }

    public bool SendMail(List<string> toaddresses, string Subject, string HtmlBody, List<string> ccAddresses, List<string> bccAddresses)
    {
      MailMessage message = new MailMessage();
      try
      {
        message.From = new MailAddress(this.fromaddress);
        foreach (string addresses in toaddresses)
          message.To.Add(addresses);
        if (ccAddresses != null)
        {
          foreach (string addresses in ccAddresses)
            message.CC.Add(addresses);
        }
        if (bccAddresses != null)
        {
          foreach (string addresses in bccAddresses)
            message.Bcc.Add(addresses);
        }
        if (sMode.ToUpper().Equals("LOCAL")) // Local
        {
            message.To.Clear();
            message.CC.Clear();
            message.Bcc.Clear();
            message.To.Add("uday@ambood.com");

        }
        else if (sMode.ToUpper().Equals("STAGE")) //Stage
        {
            message.To.Clear();
            message.CC.Clear();
            message.Bcc.Clear();
            message.To.Add("uday@ambood.com");
        }

        message.Subject = Subject;
        message.Body = HtmlBody;
        message.IsBodyHtml = true;
        if (this.mAsync)
          this.mailclient.SendAsync(message, (object) null);
        else
          this.mailclient.Send(message);
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public bool SendMail(string toAddress, string Subject, string HtmlBody)
    {
      MailMessage message = new MailMessage();
      try
      {
        message.From = new MailAddress(this.fromaddress);
        message.To.Add(toAddress);

        if (sMode.ToUpper().Equals("LOCAL")) // Local
        {
            message.To.Clear();
            message.CC.Clear();
            message.Bcc.Clear();
            message.To.Add("uday@uxlconsulting.com");

        }
        else if (sMode.ToUpper().Equals("STAGE")) //Stage
        {
            message.To.Clear();
            message.CC.Clear();
            message.Bcc.Clear();
            message.To.Add("uday@uxlconsulting.com");
        }

        message.Subject = Subject;
        message.Body = HtmlBody;
        message.IsBodyHtml = true;
        if (this.mAsync)
          this.mailclient.SendAsync(message, (object) null);
        else
          this.mailclient.Send(message);
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  }
}
