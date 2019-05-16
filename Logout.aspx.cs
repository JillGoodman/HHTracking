using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Abandon();
        // Remove content from session.
        Session.Contents.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
}