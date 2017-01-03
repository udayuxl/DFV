<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        //log4net.Config.DOMConfigurator.Configure();
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        Exception exc = Server.GetLastError();

        //string UserName =  HttpContext.Current.User.Identity.Name; 
        string strException = exc.ToString();

        

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        if (Request.Url.PathAndQuery.EndsWith(".html") && !Request.Url.PathAndQuery.EndsWith("google119ce637d1e52034.html"))
        {
            string strNewPath = Request.Url.PathAndQuery.Replace(".html", ".aspx");
            Context.RewritePath(strNewPath);
        }		
		
    } 
       
</script>
