using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace abc
{
    public class data
    {
        string wxid;//微信openid
        string name;//姓名
        string school;//学校
        string classnum;//班级
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

        public string Wxid { get => wxid; set => wxid = value; }
        public string Name { get => name; set => name = value; }
        public string School { get => school; set => school = value; }
        public string Classnum { get => classnum; set => classnum = value; }
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
    }
}