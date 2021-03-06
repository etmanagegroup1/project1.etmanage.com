﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ET.Constant.DBConst
{
    public static class ViewNames
    {
        #region 系统框架模块

        /// <summary>
        /// 用户权限信息
        /// </summary>
        public const string V_USER_ACTION_INFO = "V_USER_ACTION_INFO";

                /// <summary>
        /// 角色权限信息
        /// </summary>
        public const string V_ROLE_ACTION_INFO = "V_ROLE_ACTION_INFO";
        

        /// <summary>
        /// 用户模块信息
        /// </summary>
        public const string V_USER_MODULE_INFO = "V_USER_MODULE_INFO";
        
        /// <summary>
        /// 用户角色
        /// </summary>
        public const string V_USER_ROLE_INFO = "V_USER_ROLE_INFO";

       
        /// <summary>
        /// 用户信息，仅仅显示未冻结用户
        /// </summary>
        public const string V_AVAILABLE_USER_INFO = "V_AVAILABLE_USER_INFO";

              /// <summary>
        /// 所有用户信息，包括冻结和未冻结的
        /// </summary>
        public const string V_USER_FULL_INFO = "V_USER_FULL_INFO";

        
       

        #endregion


        
    }
}
