﻿namespace Agent.Web.Account
{
    using Agent.Web.WebBase;
    using LotterySystem.Common;
    using LotterySystem.Model;
    using System;
    using System.Data;

    public class child_add : MemberPageBase
    {
        protected DataRow[] dlDataRow1;
        protected DataRow[] dlDataRow2;
        protected DataRow[] dlDataRow3;
        protected agent_userinfo_session userModel;
        protected string utypeTxt = "";
        protected DataRow[] zjDataRow1;
        protected DataRow[] zjDataRow2;
        protected DataRow[] zjDataRow3;
        protected DataRow[] zjDataRow4;

        private void AddUser()
        {
            string str = LSRequest.qq("userName").ToLower().Trim();
            string str2 = LSRequest.qq("userPassword");
            string str3 = LSRequest.qq("userNicker");
            string str4 = LSRequest.qq("qx");
            string message = "";
            if (!base.ValidParamByUserAdd("child", ref message, null, null, null))
            {
                base.Response.Write(base.ShowDialogBox(message, null, 400));
                base.Response.End();
            }
            if (!Regexlib.IsValidPassword(str2.Trim(), base.get_GetPasswordLU()))
            {
                if (base.get_GetPasswordLU().Equals("1"))
                {
                    base.Response.Write(base.ShowDialogBox("密碼要8-20位,且必需包含大寫字母、小寫字母和数字！", null, 400));
                }
                else
                {
                    base.Response.Write(base.ShowDialogBox("密碼要8-20位,且必需包含字母、和数字！", null, 400));
                }
                base.Response.End();
            }
            cz_users_child model = new cz_users_child();
            model.set_u_id(Guid.NewGuid().ToString().ToUpper());
            model.set_u_name(str.Trim());
            model.set_u_nicker(str3.Trim());
            model.set_u_skin(base.GetUserSkin("agent"));
            string ramSalt = Utils.GetRamSalt(6);
            model.set_u_psw(DESEncrypt.EncryptString(str2, ramSalt));
            model.set_salt(ramSalt);
            model.set_parent_u_name(this.Session["user_name"].ToString());
            model.set_add_date(new DateTime?(DateTime.Now));
            model.set_status(0);
            model.set_permissions_name(str4.Trim());
            if (CallBLL.cz_users_child_bll.AddUser(model))
            {
                base.user_add_children_log(model);
                base.Response.Write(base.ShowDialogBox("添加子帳號成功！", base.UserReturnBackUrl, 0));
                base.Response.End();
            }
            else
            {
                base.Response.Write(base.ShowDialogBox("添加子帳號失敗！", base.UserReturnBackUrl, 400));
                base.Response.End();
            }
        }

        private void InitPermissions()
        {
            DataTable table = CallBLL.cz_permissions_bll.GetListByUType((this.userModel.get_u_type().Equals("zj") != null) ? "zj" : "dl").Tables[0];
            if (this.userModel.get_u_type().Equals("zj"))
            {
                this.zjDataRow1 = table.Select(string.Format(" group_id={0} ", 1));
                this.zjDataRow2 = table.Select(string.Format(" group_id={0} ", 2));
                this.zjDataRow3 = table.Select(string.Format(" group_id={0} ", 3));
                this.zjDataRow4 = table.Select(string.Format(" group_id={0} ", 4));
            }
            else
            {
                if (this.userModel.get_u_type().Equals("fgs"))
                {
                    if (this.userModel.get_six_op_odds().Equals(1) || this.userModel.get_kc_op_odds().Equals(1))
                    {
                        this.dlDataRow1 = table.Select(string.Format(" group_id={0} ", 5));
                    }
                    else
                    {
                        this.dlDataRow1 = table.Select(string.Format(" group_id={0} and name<>'{1}' ", 5, "po_5_3"));
                    }
                }
                else
                {
                    this.dlDataRow1 = table.Select(string.Format(" group_id={0} and name<>'{1}' ", 5, "po_5_3"));
                }
                this.dlDataRow2 = table.Select(string.Format(" group_id={0} ", 6));
                this.dlDataRow3 = table.Select(string.Format(" group_id={0} ", 7));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = this.Session["user_name"].ToString();
            this.userModel = this.Session[str + "lottery_session_user_info"] as agent_userinfo_session;
            if (((!this.userModel.get_u_type().Trim().Equals("zj") && !this.userModel.get_u_type().Trim().Equals("fgs")) && (!this.userModel.get_u_type().Trim().Equals("gd") && !this.userModel.get_u_type().Trim().Equals("zd"))) && !this.userModel.get_u_type().Trim().Equals("dl"))
            {
                base.Response.Redirect("../MessagePage.aspx?code=u100035&url=&issuccess=1&isback=0&isopen=1");
                base.Response.End();
            }
            if (this.Session["child_user_name"] != null)
            {
                base.Response.Redirect("../MessagePage.aspx?code=u100014&url=&issuccess=1&isback=0&isopen=1");
                base.Response.End();
            }
            string str3 = this.userModel.get_u_type().ToLower();
            if (str3 != null)
            {
                if (!(str3 == "zj"))
                {
                    if (str3 == "fgs")
                    {
                        this.utypeTxt = "分公司";
                    }
                    else if (str3 == "gd")
                    {
                        this.utypeTxt = "股東";
                    }
                    else if (str3 == "zd")
                    {
                        this.utypeTxt = "總代";
                    }
                    else if (str3 == "dl")
                    {
                        this.utypeTxt = "代理";
                    }
                }
                else
                {
                    this.utypeTxt = "總監";
                }
            }
            this.InitPermissions();
            if (LSRequest.qq("hdnadd").Equals("hdnadd"))
            {
                this.AddUser();
            }
        }
    }
}

