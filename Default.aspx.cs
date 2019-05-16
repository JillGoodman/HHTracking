using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System;

/* Called from TVUE */
public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string token = null;
            Guid teacherGU = Guid.Empty;

            try
            {
                token = Request.Params["token"];
            }
            catch { }


            //if (Request.Url.DnsSafeHost == "localhost")
            //{
            //    //LocalHost is always going to use 
            //    token = "%2btrVFVEdwFKvCcO6gUq8h2vTYet0giSnsFcxeWHBBqnA09pfU8h6q%2bJ2NwKYqxZ2NcG24hkyG0l1b%2boYmM%2bP72UqnqDdmaB4b379tzkUYa1swSkniXpLjauA8Cp6ajfH";
            //}
            //if (!string.IsNullOrWhiteSpace(token))
            //    Session["Token"] = token;
            //else
            //    token = Session["Token"].ToString();

            DBHelper helper = new DBHelper();
            if (TokenHelper.AuthenticateToken(token, out teacherGU))
            {
                string commandString = "Exec HCP_HHTRACKING_STUDENTS @teachergu= '" + teacherGU.ToString() + "'";
                DataSet stuInfo = helper.ExecuteQueryReturnDataSet(commandString);

            }
            else
            {
                Response.Write("Invalid token!");
                Response.End();
            }
        }//Not postback
    }//Page Load

    protected void DDLStudents_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}