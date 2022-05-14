using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace 教师端
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string HttpPost(string msg)
        {
            try
            {
                byte[] postD = Encoding.UTF8.GetBytes(msg);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://39.99.195.210/abc/Client/Audit");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postD, 0, postD.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                return sr.ReadToEnd();
            }
            catch
            {
                MessageBox.Show("服务器连接失败");
                return "";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 1;
            if (label11.Text != "0")
            {
                Reg();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fresh();
            check();
            if (label11.Text == "0")
            {
                MessageBox.Show("当前没有待审核对象！");
            }
            else
            {
                Reg();
            }

        }
        public void Reg()
        {
            Class1 rb = JsonConvert.DeserializeObject<Class1>(HttpPost("type=audit"));
            textBox1.Text = rb.Name;
            comboBox1.Text = rb.School;
            comboBox2.Text = rb.Classid;
            textBox4.Text = rb.Stuid;
            textBox6.Text = rb.Idcard;
            textBox5.Text = rb.Idcard.Substring(6, 8);
            wxid = rb.Wxid;
            label9.Text = rb.Reqtime;
            if (int.Parse(rb.Idcard.Substring(16, 1)) % 2 == 0)
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox1.Checked = true;
            }
        }
        string wxid;
        private void button4_Click(object sender, EventArgs e)
        {
            if (label11.Text != "0")
            {
                Class1 rb = new Class1();
                rb.Wxid = wxid;
                rb.Name = textBox1.Text;
                rb.School = comboBox1.Text;
                rb.Classid = comboBox2.Text;
                rb.Stuid = textBox4.Text;
                rb.Idcard = textBox6.Text;
                if (checkBox1.Checked == true)
                {
                    rb.Sex = "男";
                }
                else
                {
                    rb.Sex = "女";
                }
                rb.Birth = textBox5.Text;
                rb.Reqtime = label9.Text;
                rb.People = "最高管理员";
                string type = JsonConvert.SerializeObject(rb);
                type = HttpPost("type=" + type);
                if (type == "sucess")
                {
                    MessageBox.Show("提交成功!");
                    fresh();
                }
                else
                {
                    MessageBox.Show("提交失败！");
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true)
            {
                checkBox2.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string type = HttpPost("type=signreturn&&wxid="+wxid+"");
            if (type == "sucess")
            {
                MessageBox.Show("退回成功");
                fresh();
            }
            else
            {
                MessageBox.Show("退回失败");
            }
            
        }
        int error;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                check();
            }
            catch
            {
                if(error==3)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("服务器故障 请稍后再试");
                    error = 0;
                }
            }
            
        }
        public void check()
        {
            Class1 rb = JsonConvert.DeserializeObject<Class1>(HttpPost("type=check"));
            label11.Text = rb.Regnum;
            label13.Text = rb.Leamum;
            label15.Text = rb.Signnum;
            label17.Text = rb.All;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
        public void fresh()
        {
            textBox1.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            label9.Text="";
            label22.Text = "";
            label27.Text = "";
            label28.Text = "";
            label29.Text = "";
            textBox8.Text = "";
            userid = "";
        }
        private void label11_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 1;
            if (label11.Text != "0")
            {
                Reg();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(textBox2.Text!=null&& textBox3.Text != null && textBox7.Text != null)
            {
                Class1 rb = new Class1();
                rb.Head = textBox3.Text;
                rb.Msg = textBox2.Text;
                rb.People = textBox7.Text;
                string type = JsonConvert.SerializeObject(rb);
                type = HttpPost("type=" + type);
                if (type == "sucess")
                {
                    MessageBox.Show("发送成功!");
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox7.Text = "";
                }
                else
                {
                    MessageBox.Show("发送失败！");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 2;
            if (label13.Text != "0")
            {
                Leave();
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }


        private void button8_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 3;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(userid!="")
            {
                string type = HttpPost("type=leaveok&&userid=" + userid + "&&time="+label29.Text+"");
                if (type == "sucess")
                {
                    MessageBox.Show("成功");
                    fresh();
                }
                else
                {
                    MessageBox.Show("失败");
                }
            }
        }
        string userid;
        public void Leave()
        {
            Class1 rb = JsonConvert.DeserializeObject<Class1>(HttpPost("type=leave"));
            label22.Text = rb.Name;
            label27.Text = rb.School;
            label28.Text = rb.Classid;
            label29.Text = rb.Time;
            textBox8.Text = rb.Msg;
            userid = rb.Userid;
        }
        private void button11_Click(object sender, EventArgs e)
        {
            string type = HttpPost("type=leavereturn&&userid=" + userid + "&&time=" + label29.Text + "");
            if (type == "sucess")
            {
                MessageBox.Show("退回成功");
                fresh();
            }
            else
            {
                MessageBox.Show("退回失败");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            fresh();
            check();
            if (label13.Text == "0")
            {
                MessageBox.Show("当前没有请假需要审批！");
            }
            else
            {
                Leave();
            }
        }
    }
}
