﻿/* 
 * OnlineUserInsertCmd.cs @Microsoft Visual Studio 2008 <.NET Framework 2.0 (or Higher)>
 * AfritXia
 * 2008-03-06
 * 
 * Copyright(c) http://www.AfritXia.NET/
 * 
 */

using System;

namespace ET.Sys_Base.OnlineUser
{
    /// <summary>
    /// 插入命令
    /// </summary>
    internal class OnlineUserInsertCmd : OnlineUserCmdBase
    {
        #region 类构造器
        /// <summary>
        /// 类默认构造器
        /// </summary>
        public OnlineUserInsertCmd()
            : base()
        {
        }

        /// <summary>
        /// 类参数构造器
        /// </summary>
        /// <param name="db">在线用户数据库</param>
        /// <param name="currUser">当前插入的新用户</param>
        public OnlineUserInsertCmd(OnlineUserDB db, OnlineUser currUser)
            : base(db, currUser)
        {
        }
        #endregion

        /// <summary>
        /// 执行命令
        /// </summary>
        public override void Execute()
        {
            this.OnlineUserDB.Insert(this.CurrentUser);
        }
    }
}