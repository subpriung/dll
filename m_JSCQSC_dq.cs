﻿using System;
using User.Web.WebBase;

public class m_JSCQSC_dq : MemberPageBase_Mobile
{
    protected string lottery_type = "";
    protected string player_type = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        base.Response.Expires = 0;
        base.IsUserLoginByMobile();
        base.UsePageCache();
        this.lottery_type = base.qq("lottery_type");
        if (string.IsNullOrEmpty(this.lottery_type))
        {
            this.lottery_type = 13.ToString();
        }
        this.player_type = base.qq("player_type");
        if (string.IsNullOrEmpty(this.player_type))
        {
            this.player_type = "dq";
        }
    }
}

