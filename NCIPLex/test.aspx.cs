﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIPLex
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Redirect("default.aspx", true);
            
            Response.Write(System.Security.Principal.WindowsIdentity.GetCurrent().Name);

            Response.Redirect("verify.aspx", true);
        }
    }
}
