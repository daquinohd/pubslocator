﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PubEntAdmin.BLL;

namespace PubEntAdmin
{
    public partial class FeaturedPubsSeq : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Response.Redirect("Home.aspx");
            }
            this.Title = "Featured Publications Sequence";
            this.PageTitle = "Featured Publications Sequence";
            //this.AddJS(Server.MapPath("JS/LUMgmt.js"));

            if (!((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
            {
                PubEntAdminManager.UnathorizedAccess();
            }

            this.plcHldContent.Controls.Clear();

            System.Web.UI.UserControl usrCtrl = null;
            usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/FeaturedPubsSeq.ascx");
            this.plcHldContent.Controls.Add(usrCtrl);
        }


        #region Properties
        public string PageTitle
        {
            set
            {
                ((Label)this.Master.FindControl("lblPageTitle")).Text = value;
            }
            get
            {
                return ((Label)this.Master.FindControl("lblPageTitle")).Text;
            }
        }
        #endregion
    }
}
