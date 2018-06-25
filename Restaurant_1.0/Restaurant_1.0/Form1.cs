using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Data;
using System.Data.SqlClient;

namespace Restaurant_1._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


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


        public static string dfgtel = "default_guest"; 

        SqlConnection sqlcon = new SqlConnection("server=.;uid=java;pwd=123456;DataBase=Restaurant");
        private void button2_Click(object sender, EventArgs e)  //注册
        {
            Register reg = new Register();
            reg.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sqlcon.Open();
            //string telno = textBox1.Text.Trim();
            string pwd = textBox2.Text.Trim();
            string cmd = "select count(*) from Guest where Gtel = '" + textBox1.Text.Trim() + "' and Gpwd = '"+ textBox2.Text.Trim()+"'";
            try
            {
                if (textBox1.Text != "")                                                    //输入用户名
                {
                    dfgtel = textBox1.Text;
                    if (pwd != "")                                                   //输入密码
                    {

                        SqlCommand login = new SqlCommand(cmd, sqlcon);
                        //SqlDataReader password = login.ExecuteReader();
                        int ok = Convert.ToInt32(login.ExecuteScalar());
                        if (ok > 0)
                        {

                            MessageBox.Show("登录成功", "提示", MessageBoxButtons.OK);
                            this.Hide();
                            body body = new body();
                            body.Show();
                        }
                        else
                        {
                            MessageBox.Show("用户名或密码错误", "警告", MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("密码不能为空", "提示", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("账号不能为空", "提示", MessageBoxButtons.OK);
                }
                sqlcon.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }  //登录

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            body body = new body();
            body.Show();
        }   //散客跳转到主页面

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            for (int iNum = 10; iNum >= 0; iNum--)
            {
                //变更窗体的不透明度
                this.Opacity = 0.1 * iNum;
                //暂停
                System.Threading.Thread.Sleep(30);
            }
            Environment.Exit(0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
