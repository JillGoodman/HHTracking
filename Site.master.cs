using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class SiteMaster : MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;

    protected void Page_Init(object sender, EventArgs e)
    {
        // The code below helps to protect against XSRF attacks
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        else
        {
            // Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["ParentGU"]!=null && Session["StudentGU"]!=null && Convert.ToString(Session["ParentChange"]) == "Y")
        {
            liPriorCareForm.Visible = true;
        }
        if (Session["ParentGU"] != null && Session["StudentGU"] != null && Session["RegistrationFlag"] != null && Convert.ToString(Session["RegistrationFlag"])=="Y" && Convert.ToString(Session["ParentChange"]) == "Y")
        {

            //if (Session["StuGroupSchYearGU"] != null)
            //{
            //    string commandString = string.Format(@"SELECT CATEGORY_CODE
            //                                         FROM rev.HCP_ATHLETIC_GROUPS
            //                                         WHERE STU_GROUP_SCH_YEAR_GU = '{0}'", Session["StuGroupSchYearGU"]);
            //    DBHelper helper = new DBHelper();

            //    DataSet sportCategory = helper.ExecuteQueryReturnDataSet(commandString);
            //    string categoryCode = sportCategory.Tables[0].Rows[0]["CATEGORY_CODE"].ToString();
            //    Session["CategoryCode"] = categoryCode;

            //    if (categoryCode == "IT" || categoryCode == "OT")
            //        liPoleVaultPermission.Visible = true;
            //    else
            //        liPoleVaultPermission.Visible = false;
            //}
            //else
            //{
            //    liPoleVaultPermission.Visible = true;
            //}
        }
        if(Convert.ToString(Session["ParentChange"]) == "N")
        {
            liPriorCareForm.Visible = false;
        }
    }

    protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Context.GetOwinContext().Authentication.SignOut();
    }
}