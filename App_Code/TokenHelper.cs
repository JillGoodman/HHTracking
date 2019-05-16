using Revelation.Cryptography;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for ParseAuthenticationToken
/// </summary>
public class TokenHelper
{
    public static bool AuthenticateToken(string token,out Guid parentGu)
    {
        AuthenticationToken authenticationToken = null;
        string AuthenticationTokenKey = ConfigurationManager.AppSettings["AuthenticationTokenKey"];
        authenticationToken = new SymmCrypto(SymmCrypto.SymmProvEnum.Rijndael, PaddingMode.Zeros).DecryptAuthenticationToken(Encoding.ASCII.GetString(HttpServerUtility.UrlTokenDecode(token)), AuthenticationTokenKey);
        TimeSpan time = DateTime.Now.Subtract(authenticationToken.TokenTime);
        int tokenTimeoutMinutes = int.Parse(ConfigurationManager.AppSettings["AuthenticationTokenTimeOutMinutes"]);
        if (time.TotalMinutes < tokenTimeoutMinutes)
        {
            string tokenUser = authenticationToken.TokenUser;
            string[] tu = tokenUser.Split('_');
            if (tu.Length == 3)
            {
                Guid parentId = Guid.Empty;
                if ((tu[1] == "P" || tu[1] == "S") && Guid.TryParse(tu[2], out parentId))
                {
                    parentGu = parentId;
                    return true;
                }
            }
        }
        else
        {
            throw new ApplicationException("Your request token has expired. Please launch this page again from ParentVUE.");
        }
        parentGu = Guid.Empty;
        return false;
    }
}