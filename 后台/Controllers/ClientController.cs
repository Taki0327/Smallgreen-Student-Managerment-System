using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace abc.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index()
        {
            return View();
        }
        public class Jsondata
        {
            string sex;
            string birth;
            string name;
            string school;
            string classid;
            string stuid;
            string idcard;
            string reqtime;
            string wxid;
            string msg;
            string people;
            string head;
            string regnum;
            string leamum;
            string signnum;
            string all;
            string userid;
            string time;
            public string Name { get => name; set => name = value; }
            public string School { get => school; set => school = value; }
            public string Classid { get => classid; set => classid = value; }
            public string Stuid { get => stuid; set => stuid = value; }
            public string Idcard { get => idcard; set => idcard = value; }
            public string Reqtime { get => reqtime; set => reqtime = value; }
            public string Wxid { get => wxid; set => wxid = value; }
            public string Sex { get => sex; set => sex = value; }
            public string Birth { get => birth; set => birth = value; }
            public string Head { get => head; set => head = value; }
            public string Msg { get => msg; set => msg = value; }
            public string People { get => people; set => people = value; }
            public string Regnum { get => regnum; set => regnum = value; }
            public string Leamum { get => leamum; set => leamum = value; }
            public string Signnum { get => signnum; set => signnum = value; }
            public string All { get => all; set => all = value; }
            public string Userid { get => userid; set => userid = value; }
            public string Time { get => time; set => time = value; }
        }
        client ct= new client();
        Jsondata jd = new Jsondata();
        public static SQLiteConnection createCon()//拼接连接字符串
        {
            SQLiteConnection con = new SQLiteConnection(ConfigurationManager.ConnectionStrings["DB"].ToString());
            return con;
        }
        public void sjk()
        {
            ct.Con = createCon();
            ct.Con.Open();
        }
        public void Audit()
        {
            sjk();
            if (HttpContext.Request.RequestType == "POST")
            {
                ct.Type = Request["type"].ToString();
                if (ct.Type == "audit")
                {
                    int type = ct.Audcheck();
                    if (type > 0)
                    {
                        DataRow dr = ct.Audit();
                        jd.Wxid= dr[0].ToString();
                        jd.Name = dr[1].ToString();
                        jd.School  = dr[2].ToString();
                        jd.Classid = dr[3].ToString();
                        jd.Stuid = dr[4].ToString();
                        jd.Idcard = dr[5].ToString();
                        jd.Reqtime = dr[6].ToString();
                        Response.Write(JsonConvert.SerializeObject(jd));
                    }
                    else
                    {
                        Response.Write("0");
                    }    
                }
                else if(ct.Type=="check")
                {
                    jd.Regnum = ct.Audcheck().ToString();
                    jd.Leamum=ct.LeaCheck().ToString();
                    jd.Signnum = ct.SignCheck().ToString();
                    jd.All = ct.Userall().ToString();
                    Response.Write(JsonConvert.SerializeObject(jd));
                }
                else if (ct.Type == "leave")
                {
                    int type = ct.LeaCheck();
                    if (type > 0)
                    {
                        DataRow dr = ct.Leave();
                        jd.Userid = dr[1].ToString();
                        jd.Name = dr[2].ToString();
                        jd.School = dr[3].ToString();
                        jd.Classid = dr[4].ToString();
                        jd.Msg = dr[5].ToString();
                        jd.Time= dr[6].ToString();
                        Response.Write(JsonConvert.SerializeObject(jd));
                    }
                    else
                    {
                        Response.Write("0");
                    }
                }
                else if (ct.Type == "leaveok")
                {
                    Response.Write(ct.leaveok("1",Request["userid"].ToString(), Request["time"].ToString()));
                }
                else if (ct.Type == "leavereturn")
                {
                    Response.Write(ct.leaveok("2",Request["userid"].ToString(), Request["time"].ToString()));
                }
                else if (ct.Type == "signreturn")
                {
                   Response.Write(ct.Signreturn(Request["wxid"].ToString()));
                }
                else if (ct.Type != null)
                {
                    jd = JsonConvert.DeserializeObject<Jsondata>(ct.Type);
                    if (jd.Msg == null)
                    {
                        ct.Type = ct.Adduser(jd.Wxid, jd.Name, jd.Sex, jd.Birth, jd.School, jd.Classid, jd.Stuid, jd.Idcard, DateTime.Now.ToString());
                    }
                    else
                    {
                        ct.Type = ct.Notice(jd.Head, jd.Msg, jd.People);
                    }
                    Response.Write(ct.Type);
                }
                Response.End();
            }
        }
    }
}