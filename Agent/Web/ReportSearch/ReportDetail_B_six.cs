﻿namespace Agent.Web.ReportSearch
{
    using Agent.Web.WebBase;
    using LotterySystem.Model;
    using System;

    public class ReportDetail_B_six : MemberPageBase
    {
        protected agent_userinfo_session uModel;

        protected string GetReportOpenDate()
        {
            return CallBLL.cz_system_set_six_bll.GetSystemSet(100).get_ev_18();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.uModel = this.Session[this.Session["user_name"] + "lottery_session_user_info"] as agent_userinfo_session;
            base.Permission_Aspx_ZJ(this.uModel, "po_4_1");
            base.Permission_Aspx_DL(this.uModel, "po_7_1");
        }
    }
}

