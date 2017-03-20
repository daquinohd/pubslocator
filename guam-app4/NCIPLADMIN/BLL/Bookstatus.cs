﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace PubEntAdmin.BLL
{
    public class Bookstatus : MultiSelectListBoxItem
    {
        #region Fields
        //private int intAudId;
        //private string strAudName;
        private bool blnChecked;
        #endregion

        #region Constructors
        public Bookstatus(int audId, string audName, bool _checked)
            : base(audId, audName)
        {
            this.blnChecked = _checked;
        }

        #endregion

        #region Properties
        public int BookstatusID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string BookstatusName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public bool Checked
        {
            get { return this.blnChecked; }
            set { this.blnChecked = value; }
        }
        #endregion

    }
}
