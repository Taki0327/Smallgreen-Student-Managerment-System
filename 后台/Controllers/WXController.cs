using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SQLite;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace abc.Controllers
{
    public class WXController : Controller
    {
        // GET: WX
        public ActionResult Index()
        {
            return View();
        }
        public static SQLiteConnection createCon()//拼接连接字符串
        {
            SQLiteConnection con = new SQLiteConnection(ConfigurationManager.ConnectionStrings["DB"].ToString());
            return con;
        }
        public void sjk()
        {
            db.Con = createCon();

            db.Con.Open();

        }
        data wx = new data();
        opeart db = new opeart();
        public class Jsondata
        {
            string name;
            string type;
            string signtime;
            string school;
            string classid;
            string settime;
            string setadd;
            string date;
            string week;
            string userid;
            string sex;
            string stuid;
            string idcard;
            string signnum;
            string latenum;
            string birth;
            public string Name { get => name; set => name = value; }
            public string Type { get => type; set => type = value; }
            public string Signtime { get => signtime; set => signtime = value; }
            public string School { get => school; set => school = value; }
            public string Classid { get => classid; set => classid = value; }
            public string Settime { get => settime; set => settime = value; }
            public string Setadd { get => setadd; set => setadd = value; }
            public string Date { get => date; set => date = value; }
            public string Week { get => week; set => week = value; }
            public string Userid { get => userid; set => userid = value; }
            public string Sex { get => sex; set => sex = value; }
            public string Stuid { get => stuid; set => stuid = value; }
            public string Idcard { get => idcard; set => idcard = value; }
            public string Signnum { get => signnum; set => signnum = value; }
            public string Latenum { get => latenum; set => latenum = value; }
            public string Birth { get => birth; set => birth = value; }
        }
        public class NoticeMsg
        {
            string id;
            string head;
            string msg;
            string people;
            string time;

            public string Id { get => id; set => id = value; }
            public string Head { get => head; set => head = value; }
            public string Msg { get => msg; set => msg = value; }
            public string People { get => people; set => people = value; }
            public string Time { get => time; set => time = value; }
        }
        public class ABC
        {
            string a;
            string b;

            public string A { get => a; set => a = value; }
            public string B { get => b; set => b = value; }
        }
        /// <summary>
        /// 验证用户
        /// </summary>
        public void Check()
        {
            sjk();
            if (HttpContext.Request.RequestType == "POST")
            {
                wx.Type = Request["type"].ToString();
                if (wx.Type == "check")
                {
                    wx.Wxid = Request["wxid"].ToString();
                    string type = db.Check(wx.Wxid);
                    if (type == "1")
                    {
                        DataRow dr = db.DR(wx.Wxid);
                        if (dr[1].ToString() == "2")
                        {
                            type = db.Signcheck(wx.Wxid);
                        }
                        else if (dr[1].ToString() == "0")
                        {
                            type = dr[1].ToString();
                        }
                        Jsondata jd = new Jsondata
                        {
                            Name = dr[0].ToString(),
                            Type = type
                        };
                        type = JsonConvert.SerializeObject(jd);
                    }  
                    Response.Write(type);
                    Response.End();

                }
                
            }
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        public void Reg()
        {
            sjk();
            if (HttpContext.Request.RequestType == "POST")
            {
                wx.Type = Request["type"].ToString();
                if (wx.Type == "reg")
                {
                    wx.Wxid = Request["wxid"].ToString();
                    wx.Name = Request["name"].ToString();
                    wx.School = Request["school"].ToString(); 
                    wx.Classnum = Request["classid"].ToString(); 
                    wx.Stuid = Request["stuid"].ToString(); 
                    wx.Idcard = Request["idcard"].ToString();
                    if (wx.Name == "测试")
                    {
                        Response.Write(db.Test(wx.Wxid));
                    }
                    else if (wx.Name == "0" && wx.School == "0" && wx.Classnum == "0" && wx.Stuid == "0" && wx.Idcard == "0")
                    {
                        Jsondata jd = new Jsondata();
                        DataRow ds = db.Regreturn(wx.Wxid);
                        jd.Name = ds[1].ToString();
                        jd.School = ds[2].ToString();
                        jd.Classid = ds[3].ToString();
                        jd.Stuid = ds[4].ToString();
                        jd.Idcard = ds[5].ToString();
                        Response.Write(JsonConvert.SerializeObject(jd));
                    }
                    else
                    {
                        if(db.Check(wx.Wxid)=="1")
                        {
                            Response.Write(db.Regreg(wx.Wxid, wx.Name, wx.School, wx.Classnum, wx.Stuid, wx.Idcard));
                        }
                        else
                        {
                            Response.Write(db.Reg(wx.Wxid, wx.Name, wx.School, wx.Classnum, wx.Stuid, wx.Idcard));
                        }
                    }
                    Response.End();
                }
            }

        }
        /// <summary>
        /// 用户签到
        /// </summary>
        public void Sign()
        {
            sjk();
            if (HttpContext.Request.RequestType == "POST")
            {
                wx.Type = Request["type"].ToString();
                if (wx.Type == "sign")
                {
                    wx.Wxid = Request["wxid"].ToString();
                    string type= Request["sign"].ToString();
                    Jsondata jd = new Jsondata();
                    DataRow ds = db.School(wx.Wxid);
                    jd.School = ds[1].ToString();
                    jd.Classid = ds[2].ToString();
                    if (type == "0")
                    {
                        DataRow dss = db.Schoolset(jd.School, jd.Classid);
                        jd.Settime=dss[0].ToString();
                        jd.Setadd=dss[1].ToString();
                    }
                    else
                    {
                        jd.Signtime = db.Sign(ds[0].ToString());
                    }
                    jd.Date = DateTime.Now.ToLongDateString();
                    jd.Week = DateTime.Now.ToString("dddd");
                    jd.Type = db.LeaveCheck(ds[0].ToString(), jd.Date);
                    type = JsonConvert.SerializeObject(jd);
                    Response.Write(type);
                    Response.End();
                }
                else if (wx.Type == "ok")
                {
                    wx.Wxid = Request["wxid"].ToString();
                    string type = Request["sign"].ToString();
                    if (type=="0")
                    {
                        Jsondata jd = new Jsondata();
                        DataRow ds = db.School(wx.Wxid);
                        jd.School = ds[1].ToString();
                        jd.Classid = ds[2].ToString();
                        DataRow dss = db.Schoolset(jd.School, jd.Classid);
                        if (int.Parse(dss[0].ToString())<= int.Parse(DateTime.Now.Hour.ToString ()))
                        {
                            type = "late";
                        }
                        else
                        {
                            type = "normal";
                        }
                        type = db.SignSign(ds[0].ToString(), type);
                        Response.Write(type);
                        Response.End();
                    }
                }
                else if(wx.Type == "leave")
                {
                    wx.Wxid = Request["wxid"].ToString();
                    DataRow ds = db.Leave(wx.Wxid);
                    if(db.LeaveCheck(ds[0].ToString(), DateTime.Now.ToLongDateString())=="2")
                    {
                        Response.Write(db.Leaveupdate(ds[0].ToString(), Request["sign"].ToString()));
                    }
                    else
                    {
                        Response.Write(db.LeaveLeave(ds[0].ToString(), ds[1].ToString(), ds[2].ToString(), ds[3].ToString(), Request["sign"].ToString()));
                    }
                    Response.End(); 
                }
                else if (wx.Type == "leaveleave")
                {
                    wx.Wxid = Request["wxid"].ToString();
                    if( Request["sign"].ToString()=="0")
                    {
                        DataRow ds = db.Leave(wx.Wxid);
                        Response.Write(db.Leavereturn(ds[0].ToString()));
                    }
                    Response.End();
                }
            }
        }
        /// <summary>
        /// 用户页面
        /// </summary>
        public void About()
        {
            sjk();
            if (HttpContext.Request.RequestType == "POST")
            {
                wx.Type = Request["type"].ToString();
                if (wx.Type == "my")
                {
                    wx.Wxid = Request["wxid"].ToString();
                    DataRow ds = db.My(wx.Wxid);
                    Jsondata jd = new Jsondata();
                    jd.Userid= ds[0].ToString();
                    jd.Sex = ds[1].ToString();
                    jd.Birth=ds[2].ToString();
                    jd.School= ds[3].ToString();
                    jd.Classid= ds[4].ToString();
                    jd.Stuid=ds[5].ToString();
                    jd.Idcard= ds[6].ToString();
                    jd.Signnum= ds[7].ToString();
                    jd.Latenum= ds[8].ToString();
                    Response.Write(JsonConvert.SerializeObject(jd));
                    Response.End();
                }
            }
        }
        /// <summary>
        /// 通知
        /// </summary>
        public void Notice()
        {
            sjk();
            if (HttpContext.Request.RequestType == "POST")
            {
                wx.Type = Request["type"].ToString();
                if (wx.Type == "notice")
                {
                    string type = db.Noticecount();
                    DataTable a = db.Notice(type);
                    string msg="";
                    NoticeMsg ms = new NoticeMsg();
                    DataRow b;
                    for (int i=0;i<int.Parse(type);i++)
                    {
                        b = a.Rows[i];
                        ms.Id = b[0].ToString();
                        ms.Head = b[1].ToString();
                        ms.Msg = b[2].ToString();
                        ms.People = b[3].ToString();
                        ms.Time = b[4].ToString();
                        msg = msg+JsonConvert.SerializeObject(ms);
                        //msg = msg + ;
                        if(i< int.Parse(type)-1)
                        {
                            msg = msg + ",";
                        }
                    }
                    msg = "[" + msg + "]";
                    msg= JsonConvert.SerializeObject(msg);
                    Response.Write(msg);
                    Response.End();
                }
            }
        }
    }
}