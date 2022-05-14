using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace abc
{
    public class client
    {
        SQLiteConnection con;
        string wxid;//微信openid
        string name;//姓名
        string school;//学校
        string classid;//班级
        string stuid;//学号
        string idcard;//身份证
        string reqtime;//注册审核提交时间
        string type;//请求状态
        string userid;//用户id
        string signtime;//签到时间
        string signadd;//签到位置
        string signtype;//签到状态
        string setadd;//签到验证地址
        string settime;//签到验证最晚时间
        string setpeople;//签到设置人
        string sex;//性别
        string birth;//出生日期
        string regtime;//注册时间
        string lasttime;//上次签到时间
        string signnum;//正常签到次数
        string latenum;//迟到签到次数

        public SQLiteConnection Con { get => con; set => con = value; }
        public string Wxid { get => wxid; set => wxid = value; }
        public string Name { get => name; set => name = value; }
        public string School { get => school; set => school = value; }
        public string Classid { get => classid; set => classid = value; }
        public string Stuid { get => stuid; set => stuid = value; }
        public string Idcard { get => idcard; set => idcard = value; }
        public string Reqtime { get => reqtime; set => reqtime = value; }
        public string Type { get => type; set => type = value; }
        public string Userid { get => userid; set => userid = value; }
        public string Signtime { get => signtime; set => signtime = value; }
        public string Signadd { get => signadd; set => signadd = value; }
        public string Signtype { get => signtype; set => signtype = value; }
        public string Setadd { get => setadd; set => setadd = value; }
        public string Settime { get => settime; set => settime = value; }
        public string Setpeople { get => setpeople; set => setpeople = value; }
        public string Sex { get => sex; set => sex = value; }
        public string Birth { get => birth; set => birth = value; }
        public string Regtime { get => regtime; set => regtime = value; }
        public string Lasttime { get => lasttime; set => lasttime = value; }
        public string Signnum { get => signnum; set => signnum = value; }
        public string Latenum { get => latenum; set => latenum = value; }
        public DataRow  Audit()
        {
            SQLiteCommand b = new SQLiteCommand("select * from regaudit", Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = b;
            DataSet ds = new DataSet();
            sda.Fill(ds, "regrecord");
            return ds.Tables["regrecord"].Rows[0];
        }
        public DataRow Leave()
        {
            SQLiteCommand b = new SQLiteCommand("select * from leave where state='0'", Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = b;
            DataSet ds = new DataSet();
            sda.Fill(ds, "leave");
            return ds.Tables["leave"].Rows[0];
        }
        public string leaveok(string type,string userid,string time)
        {
            SQLiteCommand b = new SQLiteCommand("UPDATE leave set state='"+type+"' where userid='" + userid + "' and time='"+time+"'", Con);
            if(b.ExecuteNonQuery()>0)
            {
                if (type == "1")
                {
                    SQLiteCommand a = new SQLiteCommand("INSERT into signrecord(id,userid,signtime,signtype) VALUES(null,'" + userid + "','" + DateTime.Now.ToString() + "','leave')", Con);
                    if (a.ExecuteNonQuery() > 0)
                    {
                        SQLiteCommand c = new SQLiteCommand("UPDATE user set lastsigntime='" + DateTime.Now.ToLongDateString() + "',signnum=signnum+1 where userid='" + userid + "'", Con);
                        c.ExecuteNonQuery();
                        return "sucess";
                    }
                    else
                    {
                        return "fail";
                    }
                }
                else
                {
                    return "sucess";
                }
            }
            else
            {
                return "fail";
            }
            
        }
        public int Audcheck()
        {
            SQLiteCommand a = new SQLiteCommand("select count(*) from regrecord where type='1'", Con);
            return int.Parse(a.ExecuteScalar().ToString());
        }
        public int LeaCheck()
        {
            SQLiteCommand a = new SQLiteCommand("select count(*) from leave where state='0'", Con);
            return int.Parse(a.ExecuteScalar().ToString());
        }
        public int SignCheck()
        {
            SQLiteCommand a = new SQLiteCommand("select count(*) from user where lastsigntime='" + DateTime.Now.ToLongDateString() + "'", Con);
            return int.Parse(a.ExecuteScalar().ToString());
        }
        public int Userall()
        {
            SQLiteCommand a = new SQLiteCommand("select count(*) from user ", Con);
            return int.Parse(a.ExecuteScalar().ToString());
        }
        public string Adduser(string wxid,string name,string sex,string birth,string school,string classid,string stuid,string idcard,string regtime)
        {
            SQLiteCommand a = new SQLiteCommand("DELETE from regaudit where wxid='"+wxid+"'", Con);
            a.ExecuteNonQuery();
            SQLiteCommand b = new SQLiteCommand("UPDATE regrecord set type='2' where wxid='" + wxid + "'", Con);
            if (b.ExecuteNonQuery() > 0)
            {
                SQLiteCommand c = new SQLiteCommand("INSERT into user(userid,wxid,name,sex,birth,school,classid,stuid,idcard,regtime,lastsigntime,signnum,latenum) VALUES(null,'" + wxid + "','" + name + "','" + sex + "','"+birth+"','" + school + "','" + classid + "','" + stuid + "','" + idcard + "','" + regtime + "','0','0','0')", Con);
                if (c.ExecuteNonQuery() > 0)
                {
                    return "sucess";
                }
                else
                {
                    return "fail";
                }
            }
            else
            {
                return "fail";
            }
        }
        public string Notice(string head,string msg,string people)
        {
            SQLiteCommand c = new SQLiteCommand("INSERT into notice(id,head,msg,people,time) VALUES(null,'" + head + "','" + msg + "','" + people + "','" + DateTime.Now.ToString() + "')", Con);
            if (c.ExecuteNonQuery() > 0)
            {
                return "sucess";
            }
            else
            {
                return "fail";
            }
        }
        public string Signreturn(string wxid)
        {
            SQLiteCommand b = new SQLiteCommand("UPDATE regrecord set type='0' where wxid='" + wxid + "'", Con);
            if (b.ExecuteNonQuery() > 0)
            {
                return "sucess";
            }
            else
            {
                return "fail";
            }
        }
    }
    
}