using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
using System.Net;
using System;
using System.Web.Configuration;
//using Revelation.Cryptography;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;


public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string commandString = string.Format(@"SELECT TOP 1 DOC_DATE = convert(varchar(10),DOC_DATE,101)
                                                      FROM rev.HCP_ATHLETIC_DOCS
                                                     WHERE DOC_CATEGORY = 'RES'
                                                       AND STU_GROUP_GU = '{0}'
                                                     ORDER BY DOC_DATE desc", Session["StuGroupGU"].ToString());

            DBHelper helper = new DBHelper();
            DataSet ds = helper.ExecuteQueryReturnDataSet(commandString);
            DataTable dt = ds.Tables[0];

            if(dt.Rows.Count > 0)
            {
                lblUpdateStatus.Text = string.Format("Last Uploaded Date: {0}", dt.Rows[0]["DOC_DATE"].ToString());
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.ForeColor = System.Drawing.Color.Black;
            }
        }
    }//Page_Load

    protected void btn_Save(object sender,EventArgs e)
    {
        Save();
    }

    protected void btn_SaveNext(object sender, EventArgs e)
    {
        Save(true);
    }

    protected void Save(bool isSaveNext = false)
    {
        if (filePhysical.HasFile)
        {
            Stream fileContent = filePhysical.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fileContent);
            byte[] file = br.ReadBytes((int)fileContent.Length);

            Guid atcDocGU = Guid.NewGuid();
            Guid stuGroupGU = new Guid(Session["StuGroupGU"].ToString());
            string docCategory = "RES";
            string fileName = System.IO.Path.GetFileName(filePhysical.FileName);
            string docType = "XXX";
            string[] breakupFileName = fileName.Split('.');
            if (breakupFileName.Length > 0 &&
                breakupFileName[breakupFileName.Length - 1].Length >= 1 &&
                breakupFileName[breakupFileName.Length - 1].Length <= 5)
                docType = breakupFileName[breakupFileName.Length - 1].ToUpper();

            string docDate = DateTime.Now.ToString("MM/dd/yyyy");

            List<string> commands = new List<string>();

            if (docType == "DOC" ||
                docType == "DOCX" ||
                docType == "PDF" ||
                docType == "JPG")
            {
                //Load Document
                DBHelper helper = new DBHelper();
                commands.Add(string.Format(@"INSERT INTO rev.HCP_ATHLETIC_DOCS
                                                    (ATC_DOC_GU, STU_GROUP_GU, DOC_CATEGORY, DOC_COMMENT, DOC_TYPE, DOC_DATE, DOC_FILE_NAME, ADD_DATE_TIME_STAMP, DOC_CONTENT)
                                                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',getdate(),@fileContent)",
                                                               atcDocGU,
                                                               stuGroupGU,
                                                               docCategory,
                                                               fileName,
                                                               docType,
                                                               docDate,
                                                               fileName));
                //Load Forms
                commands.Add(string.Format(@"INSERT INTO rev.HCP_ATHLETIC_FORMS
                                            (FORM_CODE, PARENT_GU, STU_GROUP_FORM_GU, STU_GROUP_GU)
                                            VALUES('RES','{0}',NEWID(),'{1}')", Session["ParentGU"].ToString(), stuGroupGU));

                foreach (string commandString in commands)
                {
                    SqlCommand cmd = new SqlCommand(commandString);

                    if (commandString.Contains("@fileContent"))
                        cmd.Parameters.Add("@fileContent", SqlDbType.VarBinary, file.Length).Value = (object)file;

                    helper.ExecuteCommand(cmd);
                }

                lblUpdateStatus.Text = "Proof of Residency File successfully uploaded.";
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.ForeColor = System.Drawing.Color.Black;

                //Go back to the default page
                if (!isSaveNext)
                    Response.Redirect("default.aspx");
                else
                    Response.Redirect("SuddenCardiacArrest.aspx");

            }
            else
            {
                lblUpdateStatus.Text = "DOC, DOCX, PDF, or JPG files only.";
                lblUpdateStatus.ForeColor = System.Drawing.Color.Red;
                lblUpdateStatus.Font.Bold = true;
                lblUpdateStatus.Visible = true;
            }
        }
        else
        {
            lblUpdateStatus.Text = "Please find file.";
            lblUpdateStatus.ForeColor = System.Drawing.Color.Red;
            lblUpdateStatus.Font.Bold = true;
            lblUpdateStatus.Visible = true;
        }

    }//btn_Save

    private byte[] GetFile(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);

        byte[] file = br.ReadBytes((int)fs.Length);

        br.Close();
        fs.Close();

        return file;
    }

    //protected void chk_Use(object sender, EventArgs e)
    //{
    //    if(chkUseThis.Checked)
    //    {
    //        lblUpload.Visible = false;
    //        filePhysical.Visible = false;
    //        btnUpload.Text = "Save";
    //        lblUpdateStatus.Visible = false;
    //    }
    //    else
    //    {
    //        lblUpload.Visible = true;
    //        filePhysical.Visible = true;
    //        btnUpload.Text = "Upload File";
    //        lblUpdateStatus.Visible = false;
    //    }
    //}//chkUse
}