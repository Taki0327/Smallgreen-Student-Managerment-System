using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using abc.Controllers;
using System.Data;

namespace abc
{
    public class opeart
    {
        SQLiteConnection con;

        public SQLiteConnection Con { get => con; set => con = value; }

        public DataRow DR(string wxid)
        {
            SQLiteCommand b = new SQLiteCommand("select name,type from regrecord where wxid='" + wxid + "'", Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = b;
            DataSet ds = new DataSet();
            sda.Fill(ds, "regrecord");
            return ds.Tables["regrecord"].Rows[0];
        }
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public string Check(string wxid)
        {
            SQLiteCommand a = new SQLiteCommand("select count(*) from regrecord where wxid='" + wxid + "'", Con);
            if (int.Parse(a.ExecuteScalar().ToString()) > 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public int Reg(string wxid,string name,string school,string classid,string stuid,string idcard)
        {
            SQLiteCommand a = new SQLiteCommand("INSERT into regrecord(id,wxid,name,type) VALUES(null,'"+wxid+"','"+name+"',1)", Con);
            if(a.ExecuteNonQuery() > 0)
            {
                SQLiteCommand b = new SQLiteCommand("INSERT into regaudit(wxid,name,school,classid,stuid,idcard,reqtime) VALUES('" + wxid + "','"+name+"','"+school+"','"+classid+"','"+stuid+"','"+idcard+"','" + DateTime.Now.ToString() + "')", Con) ;
                if (b.ExecuteNonQuery() > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public DataRow Regreturn(string wxid)
        {
            SQLiteCommand s = new SQLiteCommand("select * from regaudit where wxid='"+wxid+"'", Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = s;
            DataSet ds = new DataSet();
            sda.Fill(ds, "user");
            return ds.Tables["user"].Rows[0];
        }
        public int Test(string wxid)
        {
            SQLiteCommand b = new SQLiteCommand("UPDATE regrecord set wxid='" + wxid + "' where name='测试'", Con);
            if (b.ExecuteNonQuery() > 0)
            {
                SQLiteCommand c = new SQLiteCommand("UPDATE user set wxid='" + wxid + "' where name='测试'", Con);
                if (c.ExecuteNonQuery() > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public int Regreg(string wxid, string name, string school, string classid, string stuid, string idcard)
        {
            SQLiteCommand b = new SQLiteCommand("UPDATE regrecord set type='1',name='" + name + "' where wxid='" + wxid + "'", Con);
            if (b.ExecuteNonQuery() > 0)
            {
                SQLiteCommand c = new SQLiteCommand("UPDATE regaudit set name='" + name + "',school='" + school + "',classid='" + classid + "',stuid='" + stuid + "',idcard='" + idcard + "',reqtime='" + DateTime.Now.ToString() + "' where wxid='" + wxid + "'", Con);
                c.ExecuteNonQuery();
                if (c.ExecuteNonQuery() > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
        /// <summary>
        /// 是否签到
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public string Signcheck(string wxid)
        {
            SQLiteCommand s = new SQLiteCommand("select lastsigntime from user where wxid='" + wxid + "'", Con);
            if (s.ExecuteScalar().ToString()== DateTime.Now.ToLongDateString())
            {
                return "3";
            }
            else
            {
                return "2";
            }
        }
        /// <summary>
        /// 用户签到
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public string Sign(string userid)
        {
            SQLiteCommand s = new SQLiteCommand("select signtime from signrecord where userid='" + userid + "' ORDER BY id desc limit 1", Con);
            return s.ExecuteScalar().ToString();
        }
        public string SignSign(string userid, string type)
        {
            SQLiteCommand a = new SQLiteCommand("INSERT into signrecord(id,userid,signtime,signtype) VALUES(null,'" + userid + "','" + DateTime.Now.ToString() + "','" + type + "')", Con);
            if (a.ExecuteNonQuery() > 0)
            {
                if (type == "normal")
                {
                    SQLiteCommand b = new SQLiteCommand("UPDATE user set lastsigntime='" + DateTime.Now.ToLongDateString() + "',signnum=signnum+1 where userid='" + userid + "'", Con);
                    b.ExecuteNonQuery();
                    return "sucess";
                }
                else if (type == "late")
                {
                    SQLiteCommand b = new SQLiteCommand("UPDATE user set lastsigntime='" + DateTime.Now.ToLongDateString() + "',latenum=latenum+1 where userid='" + userid + "'", Con);
                    b.ExecuteNonQuery();
                    return "fail";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 查询用户学校
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public DataRow School(string wxid)
        {
            SQLiteCommand s = new SQLiteCommand("select userid,school,classid from user where wxid='" + wxid + "'", Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = s;
            DataSet ds = new DataSet();
            sda.Fill(ds, "user");
            return ds.Tables["user"].Rows[0];
        }
        /// <summary>
        /// 查询用户学校签到设置
        /// </summary>
        /// <param name="school"></param>
        /// <param name="classid"></param>
        /// <returns></returns>
        public DataRow Schoolset(string school,string classid)
        {
            SQLiteCommand s = new SQLiteCommand("select time,setadd from signset where school='" + school + "' and classid='" + classid + "'", Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = s;
            DataSet ds = new DataSet();
            sda.Fill(ds, "signset");
            return ds.Tables["signset"].Rows[0];
        }
        /// <summary>
        /// 用户请假
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public DataRow Leave(string wxid)
        {
            SQLiteCommand s = new SQLiteCommand("select userid,name,school,classid from user where wxid='" + wxid + "'", Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = s;
            DataSet ds = new DataSet();
            sda.Fill(ds, "user");
            return ds.Tables["user"].Rows[0];
        }
        public string LeaveLeave(string userid, string name, string school, string classid, string content)
        {
            SQLiteCommand a = new SQLiteCommand("INSERT into leave(id,userid,name,school,classid,content,time,state) VALUES(null,'" + userid + "','" + name + "','" + school + "','" + classid + "','" + content + "','" + DateTime.Now.ToLongDateString() + "','0')", Con);
            if (a.ExecuteNonQuery() > 0)
            {
                return "sucess";
            }
            else
            {
                return "fail";
            }

        }
        public string LeaveCheck(string userid,string time)
        {
            SQLiteCommand a = new SQLiteCommand("select count(*) from leave where userid='"+userid+"' and time='"+time+"'", Con);
            if (int.Parse(a.ExecuteScalar().ToString()) > 0)
            {
                SQLiteCommand b = new SQLiteCommand("select state from leave where userid='" + userid + "' and time='" + time + "'", Con);
                return b.ExecuteScalar().ToString();
            }
            else
            {
                return "null";
            }
        }
        public string Leavereturn(string userid)
        {
            SQLiteCommand b = new SQLiteCommand("select content from leave where userid='"+userid+"' and time='"+ DateTime.Now.ToLongDateString() + "' and state='2'", Con);
            return b.ExecuteScalar().ToString();
        }
        public string Leaveupdate(string userid,string msg)
        {
            SQLiteCommand b = new SQLiteCommand("UPDATE leave set state='0',content='"+msg+"' where userid='" + userid + "' and time='" + DateTime.Now.ToLongDateString() + "'", Con);
            if (b.ExecuteNonQuery() > 0)
            {
                return "sucess";
            }
            else
            {
                return "fail";
            }
        }
        /// <summary>
        /// 查询个人资料
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public DataRow My(string wxid)
        {
            SQLiteCommand s = new SQLiteCommand("select userid,sex,birth,school,classid,stuid,idcard,signnum,latenum from user where wxid='" + wxid + "'", Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = s;
            DataSet ds = new DataSet();
            sda.Fill(ds, "user");
            return ds.Tables["user"].Rows[0];
        }
        public string Noticecount()
        {
            SQLiteCommand s = new SQLiteCommand("select count(*) from notice", Con);
            return s.ExecuteScalar().ToString();
        }

        public DataTable Notice(string count)
        {
            SQLiteCommand s = new SQLiteCommand("select * from notice order by id desc LIMIT '" + count+"'", Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = s;
            DataSet ds = new DataSet();
            sda.Fill(ds, "notice");
            return ds.Tables["notice"];
        }
    }
}