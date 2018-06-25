using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace Restaurant_1._0
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        SqlConnection sqlcon = new SqlConnection("server=.;uid=java;pwd=123456;DataBase=Restaurant");

        //密码管理开发中。。。

        //自己设置窗口
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // 窗体上鼠标按下时
        protected override void OnMouseDown(MouseEventArgs e)   //拖动
        {
            if (e.Button == MouseButtons.Left & this.WindowState == FormWindowState.Normal)
            {
                // 移动窗体
                this.Capture = false;
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string vip = "select count(*) from Guest where Gvip = '" + textBox1.Text.Trim() + "'";
                sqlcon.Open();
                SqlCommand vipno = new SqlCommand(vip, sqlcon);
                //SqlDataReader password = login.ExecuteReader();
                int vipnum = Convert.ToInt32(vipno.ExecuteScalar());
                sqlcon.Close();
                if (vipnum == 0)
                {
                    if (textBox2.Text != "")
                    {
                        if (textBox3.Text != "")
                        {
                            string telphone = "select count(*) from Guest where Gtel = '" + textBox3.Text.Trim() + "'";
                            sqlcon.Open();
                            SqlCommand telnoo = new SqlCommand(telphone, sqlcon);
                            //SqlDataReader password = login.ExecuteReader();
                            int telnum = Convert.ToInt32(telnoo.ExecuteScalar());
                            sqlcon.Close();
                            if(telnum == 0)
                            {
                                if (textBox4.TextLength < 6 || textBox4.TextLength > 16)
                                {
                                    MessageBox.Show("密码不符合标准！", "提示");
                                }
                                else if (textBox4.Text != textBox5.Text)
                                {
                                    MessageBox.Show("两次密码输入不一致，请重新输入！", "提示");
                                }
                                else
                                {
                                    string insertGuest = "insert into Guest(Gvip,Gname,Gtel,Gpwd) values ('" + textBox1.Text.Trim() + "','" + textBox2.Text.Trim() + "','" + textBox3.Text.Trim() + "','" + textBox4.Text.Trim() + "')";
                                    sqlcon.Open();

                                    SqlCommand reg = new SqlCommand(insertGuest, sqlcon);
                                    reg.ExecuteNonQuery();
                                    sqlcon.Close();


                                    MessageBox.Show("尊敬的VIP客户，您已注册成功。\n以后将采用手机号作为登录账号。\n祝您用餐愉快！", "OK");
                                    this.Close();
                                }
                            }
                            else
                            {
                                MessageBox.Show("手机号已存在，\n是您的手机号吗？", "提示");
                            }
                        }
                        else
                        {
                            MessageBox.Show("手机号不能为空！", "提示");
                        }
                    }
                    else
                    {
                        MessageBox.Show("称呼不能为空！", "提示");
                    }
                }
                else
                {
                    MessageBox.Show("VIP号已存在！", "提示");
                }
            }
            else
            {
                MessageBox.Show("VIP号不允许为空！", "提示");
            }
        }       //注册

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            for (int iNum = 10; iNum >= 0; iNum--)
            {
                //变更窗体的不透明度
                this.Opacity = 0.1 * iNum;
                //暂停
                System.Threading.Thread.Sleep(30);
            }
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
