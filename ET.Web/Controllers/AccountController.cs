using ET.Constant.DBConst;
using ET.Sys_DEF;
using ET.ToolKit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class AccountController : WebControllerBase
    {
        //
        // GET: /Account/

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            ET.Sys_Base.Login_Ajax loginAjax = new ET.Sys_Base.Login_Ajax();
            loginAjax.Logout();
            return View();
        }

 
        public ActionResult ValidateCode()
        {
            int width = 100;
            int height = 40;
            int fontsize = 20;
            string code = string.Empty;
            byte[] bytes = ET.ToolKit.ToolKit.Drawing.ValidateCodeHelper.CreateValidateGraphic(out code, 4, width, height, fontsize);
            ET.ToolKit.ToolKit.Common.SessionHelper.Add(SystemConfigConst.SessioncheckValiCode, code);
            return File(bytes, @"image/jpeg");
        }


        #region Ajax处理
        [HttpPost]
        public ActionResult AjaxQQlogin(string u, string p, string name, string sex, string pic)
        {
            try
            {
                string strReturn = "";
                if (!string.IsNullOrEmpty(u) && !string.IsNullOrEmpty(p))
                {
                    UserBaseInfo baseinfo = new ET.Sys_BLL.OrganizationBLL().Get_UserBaseInfo(" AND UserName='" + u + "'");
                    if (baseinfo == null)
                    {
                        User_Full_Info info = new User_Full_Info();
                        baseinfo = new UserBaseInfo();
                        UserPropertyInfo stuinfo = new UserPropertyInfo();
                        baseinfo.UserName = u;
                        baseinfo.UserStatus = 1;
                        baseinfo.UserPwd = ET.ToolKit.Encrypt.EncrypeHelper.EncryptMD5(p);

                        stuinfo.Nickname = u;
                        if (!string.IsNullOrEmpty(name))
                            stuinfo.CNName = name;
                        if (!string.IsNullOrEmpty(sex))
                            stuinfo.Sex = sex;
                        stuinfo.QQ = u;
                        if (!string.IsNullOrEmpty(pic))
                            stuinfo.Photo = pic;
                        stuinfo.Source = "QQ登录";
                        stuinfo.CreateTime = DateTime.Now;

                        info.userbaseinfo = baseinfo;
                        info.userstuinfo = stuinfo;
                        info.userrole = new List<UserRoleLink>();
                        UserRoleLink role = new UserRoleLink();
                        info.userrole.Add(role);
                        if (new ET.Sys_BLL.OrganizationBLL().Operate_User_Info(info, true))
                        {
                        }
                    }

                    if (baseinfo.UserStatus == 1 || (baseinfo.UserStatus == 0 && baseinfo.UserStartTime < DateTime.Now && baseinfo.UserEndTime >= DateTime.Now))
                    {
                        ET.Sys_Base.Login_Ajax loginAjax = new ET.Sys_Base.Login_Ajax();
                        try
                        {
                            if (loginAjax.SinglePointState() == "1")
                            {
                                strReturn = loginAjax.WebLoginSinglePoint(u, p, false);

                            }
                            else
                            {
                                strReturn = loginAjax.WebLoginUser(u, p, false);

                            }
                        }
                        catch (Exception ex)
                        {
                            strReturn = ex.Message;
                        }
                    }
                    else
                        strReturn = "非常抱歉！该帐号无权登录！请联系管理员";
                }
                return Content(strReturn);
            }
            catch (Exception ex)
            {
                //throw ex;
                return Content(ex.Message);
            }


        }



        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult AjaxLogin(FormCollection collection, string l)
        {
            string strReturn = "";
            if (ModelState.IsValid)
            {
                ET.Sys_Base.Login_Ajax loginAjax = new ET.Sys_Base.Login_Ajax();
                try
                {
                    if (loginAjax.SinglePointState() == "1")
                    {
                        strReturn = loginAjax.WebLoginSinglePoint(collection["username"], collection["password"], false);

                    }
                    else
                    {
                        strReturn = loginAjax.WebLoginUser(collection["username"], collection["password"], false);

                    }
                }
                catch (Exception ex)
                {
                    strReturn = ex.Message;
                }
            }
            //if (strReturn == "true")
            //{
            //    Response.Redirect(!string.IsNullOrEmpty(l) ? l : "/blog/index/");

            //}
            return Content(strReturn);
        }

        [HttpGet]
        public ActionResult AjaxCheckUser(string uid)
        {
            uid = ET.ToolKit.Common.StringHelper.ClearSqlDangerous(uid);
            UserBaseInfo info = new ET.Sys_BLL.OrganizationBLL().Get_UserBaseInfo(" AND UserName='" + uid + "'");
            if (info != null)
                return Content("用户名已经存在！");
            else
                return Content("true");

        }
        [HttpGet]
        public ActionResult AjaxCheckValidatecode(string code)
        {
            string resultMsg = "true";
            string pCode = code;
            string sCode = ET.ToolKit.ToolKit.Common.SessionHelper.Get(SystemConfigConst.SessioncheckValiCode).ToString();
            if (string.IsNullOrEmpty(pCode))
            {
                resultMsg = "请输入验证码";
            }
            else if (string.IsNullOrEmpty(sCode))
            {
                resultMsg = "验证码过期";
            }
            else if (pCode.ToLower() != sCode.ToLower())
            {
                resultMsg = "验证码不正确";
            }

            return Content(resultMsg);

        }
        [HttpPost]
        public ActionResult AjaxRegister(FormCollection collection, string l)
        {



            User_Full_Info info = new User_Full_Info();
            UserBaseInfo baseinfo = new UserBaseInfo();
            UserPropertyInfo stuinfo = new UserPropertyInfo();



            baseinfo.UserName = collection["user"];
            baseinfo.UserPwd = ET.ToolKit.Encrypt.EncrypeHelper.EncryptMD5(collection["passwd"]);
            baseinfo.UserStatus = 1;

            stuinfo.Nickname = collection["user"];
            stuinfo.CNName = collection["user"];
            stuinfo.ENName = collection["user"];
            stuinfo.Sex = "保密";
            stuinfo.QQ = collection["qq"];
            stuinfo.CreateTime = DateTime.Now;
            info.userbaseinfo = baseinfo;
            info.userstuinfo = stuinfo;

            info.userrole = new List<UserRoleLink>();
            UserRoleLink role = new UserRoleLink();
            info.userrole.Add(role);
            if (new ET.Sys_BLL.OrganizationBLL().Operate_User_Info(info, true))
                return Content("true");
            else
                return Content("error");
        }
        [HttpGet]
        public ActionResult AjaxLogout()
        {

            return RedirectToAction("login");
        }
        #endregion


    }
}
