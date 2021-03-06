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
using AjaxControlToolkit;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class Language : System.Web.UI.UserControl
    {
        protected System.Web.UI.WebControls.Label lblMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.BindData();
            this.SecVal();
        }

        #region Sec Val
        private void SecVal()
        {
            this.LenVal();
            this.TagVal();
            this.SpecialVal();
        }

        private void LenVal()
        {
            if ((!PubEntAdminManager.LenVal(this.txtLngName.Text, 50)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtLngName.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

        }

        private void SpecialVal()
        {

            if ((PubEntAdminManager.SpecialVal2(this.txtLngName.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

        }
        #endregion

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            ((LookupMgmt)this.Page).ShowHideSpellChecker(true);

            ScriptManager.RegisterClientScriptBlock(this,
                    this.GetType(), "Subject_ClientScript",
                @"
                    function LkupParticipants()
                    {
                        return '" + this.GetSpellCheckParticipants() + @"';
                    }
                ", true);

        }

        protected void BindData()
        {
            this.gvResult.DataSource = LU_DAL.GetLanguageLU();
            this.gvResult.DataBind();

        }

        protected void gvResult_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.gvResult.EditItemIndex = -1;
            this.BindData();
        }

        protected void gvResult_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.gvResult.EditItemIndex = e.Item.ItemIndex;
            this.BindData();
        }

        protected void gvResult_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string vInactive = ((System.Web.UI.WebControls.Button)(e.CommandSource)).Text;

            if ((vInactive == "Inactive") || (vInactive == "Active"))
                gvResult_DeleteCommand(source, e);
        }

        protected void gvResult_SortCommand(object source, DataGridSortCommandEventArgs e)
        {

        }

        protected void gvResult_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            LanguageCollection dt = ((LanguageCollection)this.gvResult.DataSource);

            PubEntAdmin.BLL.Language l_conf = dt[e.Item.ItemIndex];
            int Lngid = l_conf.LngID;
            string Lngname = ((TextBox)e.Item.Cells[1].Controls[1]).Text;

            int Active = 0;
            string sActive = ((Label)e.Item.Cells[2].Controls[1]).Text;
            if (sActive == "Active")
            { Active = 1; }
            else
            { Active = 0; }

            Boolean valid = false;
            Boolean validnum = false;
            Boolean validlen = false;

            if (Lngname.Length != 0)
            {
                valid = PubEntAdminManager.OtherVal(Lngname);
                validnum = PubEntAdminManager.SpecialVal2(Lngname);
                validlen = PubEntAdminManager.LenVal(Lngname, 50);
            }

            if ((valid == false) && (validnum == false) && (validlen == true))
            {
                LU_DAL.UpdateLanguageLU(Lngid, Lngname, Active);
                Response.Redirect("~/LookupMgmt.aspx?sub=language");
            }
            else
            {
                Response.Redirect("InvalidInput.aspx");
            }

        }

        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LanguageCollection dt = ((LanguageCollection)this.gvResult.DataSource);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PubEntAdmin.BLL.Language l_conf = dt[e.Item.ItemIndex];
                ((Label)e.Item.Cells[1].Controls[1]).Text = Server.HtmlEncode(l_conf.LngName);


                //delete btn col
                Button l_able = e.Item.Cells[5].FindControl("lnkbtnDel") as Button;

                if (l_conf.Checked)
                {
                    ((Label)e.Item.Cells[2].Controls[1]).Text = "Active";
                    l_able.Text = "Inactivate";
                    Panel l_pnl = e.Item.Cells[5].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to inactivate this Lookup Value [" + Server.HtmlEncode(l_conf.LngName) + "]?";
                }
                else
                {
                    ((Label)e.Item.Cells[2].Controls[1]).Text = "Inactive";
                    l_able.Text = "Activate";
                    Panel l_pnl = e.Item.Cells[5].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to activate this Lookup Value [" + Server.HtmlEncode(l_conf.LngName) + "]?";
                }
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                PubEntAdmin.BLL.Language l_conf = dt[e.Item.ItemIndex];
                String status = "";
                if (l_conf.Checked == true)
                { status = "Active"; }
                else
                { status = "Inactive"; }
                ((Label)e.Item.Cells[2].Controls[1]).Text = status;

                if (e.Item.Cells[3].Controls[2] is Button)
                {
                    Button l_btnCancel = ((Button)e.Item.Cells[3].Controls[2]);
                    l_btnCancel.ID = "gvResult_Cancel";

                    Panel l_panel = new Panel();
                    l_panel.ID = "l_panel";
                    l_panel.CssClass = "modalPopup";
                    l_panel.Style.Add("display", "none");
                    l_panel.Width = Unit.Pixel(233);

                    Label l_label = new Label();
                    l_label.Text = "Are you sure you want to continue?";

                    HtmlGenericControl l_div = new HtmlGenericControl();
                    Button l_ok = new Button();
                    Button l_cancel = new Button();
                    l_ok.ID = "l_ok";
                    l_ok.Text = "OK";
                    l_cancel.ID = "l_cancel";
                    l_cancel.Text = "Cancel";
                    l_div.Controls.Add(l_ok);
                    l_div.Controls.Add(new LiteralControl("&nbsp;"));
                    l_div.Controls.Add(l_cancel);
                    l_div.Attributes.Add("align", "center");

                    l_panel.Controls.Add(l_label);
                    l_panel.Controls.Add(new LiteralControl("<br>"));
                    l_panel.Controls.Add(new LiteralControl("<br>"));
                    l_panel.Controls.Add(l_div);

                    ModalPopupExtender l_mpe = new ModalPopupExtender();
                    l_mpe.ID = "l_mpe";
                    l_mpe.TargetControlID = l_btnCancel.ID;
                    l_mpe.PopupControlID = l_panel.ID;
                    l_mpe.BackgroundCssClass = "modalBackground";
                    l_mpe.DropShadow = true;
                    l_mpe.OkControlID = l_ok.ID;
                    l_mpe.CancelControlID = l_cancel.ID;

                    ConfirmButtonExtender l_cbe = new ConfirmButtonExtender();
                    l_cbe.TargetControlID = l_btnCancel.ID;
                    l_cbe.ConfirmText = "";
                    l_cbe.DisplayModalPopupID = l_mpe.ID;

                    e.Item.Cells[3].Controls.Add(l_panel);
                    e.Item.Cells[3].Controls.Add(l_mpe);
                    e.Item.Cells[3].Controls.Add(l_cbe);
                }

                //delete btn col
                Button l_able = e.Item.Cells[5].FindControl("lnkbtnDel") as Button;

                if (l_conf.Checked)
                {
                    ((Label)e.Item.Cells[2].Controls[1]).Text = "Active";
                    l_able.Text = "Inactivate";
                    Panel l_pnl = e.Item.Cells[5].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to inactivate this Lookup Value [" + Server.HtmlEncode(l_conf.LngName) + "]?";
                }
                else
                {
                    ((Label)e.Item.Cells[2].Controls[1]).Text = "Inactive";
                    l_able.Text = "Activate";
                    Panel l_pnl = e.Item.Cells[5].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to activate this Lookup Value [" + Server.HtmlEncode(l_conf.LngName) + "]?";
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Message.Visible = false;
            string lngdesc = this.txtLngName.Text.Trim();
            Boolean valid = false;
            Boolean validnum = false;
            Boolean validlen = false;

            if (lngdesc.Length != 0)
            {
                valid = PubEntAdminManager.OtherVal(lngdesc);
                validnum = PubEntAdminManager.SpecialVal2(lngdesc);
                validlen = PubEntAdminManager.LenVal(lngdesc, 50);
            }

            if ((valid == false) && (validnum == false) && (validlen == true))
            {
                AddLookup(lngdesc);
            }
            else
            {
                Response.Redirect("InvalidInput.aspx");
            }
 
        }

        protected void AddLookup(string lngdesc)
        {
            string lngcode = String.Empty;
            ArrayList status = new ArrayList();
            ArrayList toinsert = new ArrayList();
            int flag = 0;
            Boolean iValue = true;

            string[] split = null;
            string[] dbsplit = null;
            string sLngName = null;
            string delimStr = " ";
            char[] delimiter = delimStr.ToCharArray();

            sLngName = lngdesc.Trim();
            split = sLngName.Split(delimiter);

            DataSet ds = new DataSet();
            ds = LU_DAL.displayLanguage();

            string dbValue = "";
            //loop through dataset
            if (ds.Tables[0].Rows.Count == 0)
            {
                LU_DAL.AddLanguage(lngdesc, lngcode);
                Response.Redirect("~/LookupMgmt.aspx?sub=language");
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dataRow = ds.Tables[0].Rows[i];
                    dbValue = (string)dataRow["DESCRIPTION"];
                    dbsplit = dbValue.Split(delimiter);
                    status.Clear();

                    if (dbsplit.Length == split.Length)
                    {
                        //compare each word in the row to words being added
                        for (int j = 0; j < dbsplit.Length; j++)
                        {
                            Boolean s = Object.Equals(dbsplit[j], split[j]);
                            status.Add(s);
                        }
                        flag = 0;
                        //Parse through the status for each word, and if no match update the flag
                        for (int k = 0; k < status.Count; k++)
                        {
                            if (status[k].Equals(true))
                            {
                                flag = flag + 1;
                            }
                        }
                        //if the flag is equal to number of words, means that there is an exact match 
                        if (status.Count == flag)
                        {
                            //set iValue to false and display message
                            iValue = false;
                        }
                        else if (status.Count != flag)
                        {
                            iValue = true;
                        }
                    }
                    //populate all the iValues for the rows in the database in Arraylist
                    toinsert.Add(iValue);
                }

                Boolean iFinalinsert = false;
                int y = 0;
                int n = 0;
                //Parse the arrayList
                for (int m = 0; m < toinsert.Count; m++)
                {
                    if (toinsert[m].Equals(false))
                    {
                        //the Value already exits do not insert
                        y = y + 1;
                    }
                    else
                    {
                        n = n + 1;
                    }

                    if (n == toinsert.Count)
                    {
                        iFinalinsert = true;
                    }
                    else
                    {
                        iFinalinsert = false;

                    }
                    //Does not exist in database and therefore insert
                    if ((iFinalinsert == true) && (lngdesc.Length != 0))
                    {
                        LU_DAL.AddLanguage(lngdesc, lngcode);
                        Response.Redirect("~/LookupMgmt.aspx?sub=language");
                    }
                    else if (lngdesc.Length == 0)
                    {
                        string confirm = "Cannot add empty Lookup Value.";
                        Message.Text = confirm;
                        Message.Visible = true;
                    }
                    else
                    {
                        string confirm = "The Lookup value already exists.";
                        Message.Text = confirm;
                        Message.Visible = true;
                    }
                }
            }
        }


        protected void gvResult_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            LanguageCollection dt = ((LanguageCollection)this.gvResult.DataSource);

            PubEntAdmin.BLL.Language l_conf = dt[e.Item.ItemIndex];

            int Lngid = l_conf.LngID;
            string Lngname = l_conf.LngName;
            string Activestatus = ((Button)e.Item.Cells[4].Controls[7]).Text;

            if (Activestatus == "Inactive")
            {
                Boolean lngExist = LU_DAL.LngExist(Lngid);

                if (lngExist == false)
                {
                    LU_DAL.DeleteLanguageLU(Lngid);
                    Response.Redirect("~/LookupMgmt.aspx?sub=language");
                }
                else if (lngExist == true)
                {
                    string confirm = "Unable to Inactivate, value associated with Publication.";
                    ((Label)e.Item.Cells[5].Controls[1]).Text = confirm;
                }
            }
            if (Activestatus == "Active")
            {
                int Active = 1;
                LU_DAL.UpdateLanguageLU(Lngid, Lngname, Active);
                Response.Redirect("~/LookupMgmt.aspx?sub=language");
            }
        }

         #region Properties

        protected string ParentSpellCkrID1
        {
            get
            {
                return ((LookupMgmt)this.Page).ParentSpellCkr1;
            }
        }

        protected string ParentSpellCkrID2
        {
            get
            {
                return ((LookupMgmt)this.Page).ParentSpellCkr2;
            }
        }

        #endregion

        #region Methods

        public string GetSpellCheckParticipants()
        {
            string participant = this.txtLngName.ClientID;

            if (this.gvResult.EditItemIndex != -1)
                participant += "," + ((TextBox)this.gvResult.Items[this.gvResult.EditItemIndex].Cells[1].FindControl("txtLngName")).ClientID;

            return participant;
        }
        #endregion
        }

    }
