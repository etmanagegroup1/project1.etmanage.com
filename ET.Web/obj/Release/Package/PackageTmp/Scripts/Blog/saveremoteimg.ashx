﻿<%@ WebHandler Language="C#" Class="saveremoteimg" %>

using System;
using System.Web;
using System.Net;
using System.IO;
public class saveremoteimg : IHttpHandler
{
    // saveremoteimg demo for aspx
    // @requires xhEditor
    // 
    // @author JEDIWOLF<jediwolf@gmail.com>
    // @site http://xheditor.com/
    // @licence LGPL(http://www.opensource.org/licenses/lgpl-license.php)
    // 
    // @Version: 0.9.1 (build 110703)
    //
    // 注：本程序仅为演示用，只实现了最简单的远程抓图及粘贴上传，如果要完善此功能，还需要自行开发以下功能：
    //		1，非图片扩展名的URL地址抓取
    //		2，大体积的图片转jpg格式，以及加水印等后续操作
    //		3，上传记录存入数据库以管理用户上传图片


    private string upExt = ",jpg,jpeg,gif,png,bmp,";  //上传扩展名
    private string attachDir = "UpLoad/Blog/EditorFiles/";        //上传文件保存路径，开头不要带/
    private int dirType = 1;                    // 1:按天存入目录 2:按月存入目录 3:按扩展名存目录  建议使用按天存
    private int maxAttachSize = 20971520;        // 最大上传大小，默认是20M
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";


        if (context.Request["urls"]==null)
            return;
        string[] arrUrl = context.Request["urls"].Split('|');
        for (int i = 0; i < arrUrl.Length; i++)
        {
            string localUrl = saveRemoteImg(arrUrl[i], context);
            if (localUrl != "") arrUrl[i] = localUrl;//有效图片替换
        }

        context.Response.Write(String.Join("|", arrUrl));
    }
    string saveRemoteImg(string sUrl, HttpContext context)
    {
        byte[] fileContent;
        string objStream;
        string sExt;
        string sFile;
        if (sUrl.StartsWith("data:image"))
        {
            // base64编码的图片，可能出现在firefox粘贴，或者某些网站上，例如google图片
            int pstart = sUrl.IndexOf('/') + 1;
            sExt = sUrl.Substring(pstart, sUrl.IndexOf(';') - pstart).ToLower();

            if (upExt.IndexOf("," + sExt + ",") == -1) return "";

            fileContent = Convert.FromBase64String(sUrl.Substring(sUrl.IndexOf("base64,") + 7));
        }
        else
        {
            // 图片网址
            sExt = sUrl.Substring(sUrl.LastIndexOf('.') + 1).ToLower();

            if (upExt.IndexOf("," + sExt + ",") == -1) return "";

            fileContent = getUrl(sUrl);
        }

        if (fileContent.Length > maxAttachSize) return "";//超过最大上传大小忽略

        //有效图片保存
        sFile = getLocalPath(sExt, context);
        File.WriteAllBytes(context.Server.MapPath(sFile), fileContent);

        return sFile;
    }

    string getLocalPath(string extension, HttpContext context)
    {
        string attach_dir, attach_subdir, filename, target, tmpfile;
        switch (dirType)
        {
            case 1:
                attach_subdir = "day_" + DateTime.Now.ToString("yyMMdd");
                break;
            case 2:
                attach_subdir = "month_" + DateTime.Now.ToString("yyMM");
                break;
            default:
                attach_subdir = "ext_" + extension;
                break;
        }
        attach_dir = attachDir + attach_subdir + "/";

        if (!Directory.Exists(context.Server.MapPath(SystemConfigConst.WebSiteDir + attach_dir)))
        {
            Directory.CreateDirectory(context.Server.MapPath(SystemConfigConst.WebSiteDir + attach_dir));
        }
        filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + extension;
        return ("/" + attach_dir + filename).ToLower();
    }

    byte[] getUrl(string sUrl)
    {
        WebClient wc = new WebClient();
        try
        {
            return wc.DownloadData(sUrl);
        }
        catch
        {
            return null;
        }
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}