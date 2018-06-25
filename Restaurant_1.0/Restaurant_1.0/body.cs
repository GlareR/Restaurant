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
    public partial class body : Form
    {
        public body()
        {
            InitializeComponent();
        }
        //自己设计窗口
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

        SqlConnection sqlcon = new SqlConnection("server=.;uid=java;pwd=123456;DataBase=Restaurant");
        private void body_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“restaurantDataSet.Bill”中。您可以根据需要移动或删除它。
            this.billTableAdapter.Fill(this.restaurantDataSet.Bill);
            // TODO: 这行代码将数据加载到表“restaurantDataSet.Food”中。您可以根据需要移动或删除它。
            this.foodTableAdapter.Fill(this.restaurantDataSet.Food);
            // TODO: 这行代码将数据加载到表“restaurantDataSet.Room”中。您可以根据需要移动或删除它。
            this.roomTableAdapter.Fill(this.restaurantDataSet.Room);
            // TODO: 这行代码将数据加载到表“restaurantDataSet.Food”中。您可以根据需要移动或删除它。
            this.foodTableAdapter.Fill(this.restaurantDataSet.Food);
            // TODO: 这行代码将数据加载到表“restaurantDataSet.Bill”中。您可以根据需要移动或删除它。
            this.billTableAdapter.Fill(this.restaurantDataSet.Bill);

        }

        int groundtimes = 58;   //大厅位置计数器
        int roomno;             //房间号
        string getl = Form1.dfgtel;     //从登陆窗口传过来的是客人手机号或者散客
        private void button1_Click(object sender, EventArgs e)  //选择桌子
        {
            /* string info;
             int num = 0;
             if (textBox1.Text == "") //未输入桌号
             {
                 info = Convert.ToString(dataGridView3.SelectedRows[0].Cells[0].Value);
                 if (info == "")   //鼠标未选择
                     MessageBox.Show("尚未选择桌号", "提示");
                 else  //未输入桌号但是选择了
                 {
                     roomno = Convert.ToInt32(info);
                     MessageBox.Show(info, "提示");
                 }

             }*/
            //选桌。并且插入GR表
            
            if(Convert.ToString(dataGridView1.SelectedRows[0].Cells[2].Value).Trim()!="占用")
            {
                string UpdateRoom;
                roomno = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                if (roomno == 100)
                {
                    UpdateRoom = "Update Room set Rseats=Rseats-1";
                    groundtimes--;
                    if (groundtimes == 0)
                    {
                        UpdateRoom = "Update Room set Rstate='大厅满员' where Rno = '" + roomno + "'";
                        MessageBox.Show("大厅满员", "警告");
                    }
                }
                else
                {
                    UpdateRoom = "Update Room set Rstate='占用' where Rno = '" + roomno + "'";
                }
                string GRoption = "Insert into GR(Rno,Gtel,vip) values ('" + roomno + "','" + getl + "','1')";

                /*if (getl != "default_guest")//如果是会员
                {
                    GRoption = "Insert into GR(Rno,Gtel,vip) values ('" + roomno + "','" + getl + "','1')";
                }
                else//如果不是会员  用default_guest
                {
                    GRoption = "Update GR set Rno='" + roomno + "'  where Gtel = 'default_guest'";
                }*/

                sqlcon.Open();

                SqlCommand update = new SqlCommand(UpdateRoom, sqlcon);
                SqlCommand insert = new SqlCommand(GRoption, sqlcon);
                update.ExecuteNonQuery();
                insert.ExecuteNonQuery();
                sqlcon.Close();

                //sqlselect("select * from Room");
                string select = "select * from Room";
                DataTable dt = new DataTable();

                try
                {
                    sqlcon.Open();//打开
                    SqlDataAdapter da = new SqlDataAdapter(select, sqlcon);
                    da.Fill(dt);//进行填充
                    DataView dv = new DataView(dt);
                    dt = dv.ToTable(true);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    sqlcon.Close();//关闭连接，释放资源
                }

                MessageBox.Show("" + roomno + "房间选择成功", "提示");

                tabControl1.SelectedTab = tabPage2;


            }
            else
            {
                MessageBox.Show("房间被占用！", "提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)  //点菜
        {
            int FNO = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
            float OnePrice = Convert.ToSingle(dataGridView2.SelectedRows[0].Cells[3].Value);
            string count = numericUpDown1.Value.ToString();
            string InsertBill = "insert into Bill(Rno,Fno,Fcount,Fprice) values ('"+ roomno+"','" + FNO + "','" + count + "','" + OnePrice + "')";


            sqlcon.Open();          //操作数据库
            /*SqlDataAdapter da = new SqlDataAdapter(InsertBill, sqlcon);
            da.Fill(dt);*/

            string norepeat = "select count(*) from Bill where fno = '"+ FNO + "'";
            SqlCommand repeat = new SqlCommand(norepeat, sqlcon);
            int amount = Convert.ToInt32(repeat.ExecuteScalar());

            if (amount > 0)
            {
                MessageBox.Show("已经点过！", "提示");
            }
            else
            {
                SqlCommand insert = new SqlCommand(InsertBill, sqlcon);
                insert.ExecuteNonQuery();
                MessageBox.Show("点餐成功", "提示");
            }
            /*SqlCommand insert = new SqlCommand(InsertBill, sqlcon);
            insert.ExecuteNonQuery();*/
            sqlcon.Close();

            //显示在界面上
            string select = "select * from Bill where Rno = '"+ roomno +"'";
            DataTable dt = new DataTable();

            try
            {
                sqlcon.Open();//打开
                SqlDataAdapter da = new SqlDataAdapter(select, sqlcon);
                da.Fill(dt);//进行填充
                DataView dv = new DataView(dt);
                dt = dv.ToTable(true);
                dataGridView3.DataSource = dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                sqlcon.Close();//关闭连接，释放资源
            }

            //tabControl1.SelectedTab = tabPage3;
        }

        private void button3_Click(object sender, EventArgs e)  //结账
        {
            float price = 0;
            float pays = 0;
            int row = dataGridView3.Rows.Count;

            int rno = Convert.ToInt32(dataGridView3.Rows[1].Cells[0].Value);
            for (int i = 0; i < row; i++)
            {
                if (rno == roomno)
                    price += (Convert.ToSingle(dataGridView3.Rows[i].Cells[3].Value)) * (Convert.ToSingle(dataGridView3.Rows[i].Cells[2].Value));
            }

            if (getl != "default_guest")
            {
                pays = price * Convert.ToSingle(0.8);
                MessageBox.Show("总价为" + price + "，您是尊贵的会员享受八折优惠，应付" + pays + "", "总价");
            }
            else
            {
                MessageBox.Show("总价为" + price + "", "总价");
            }
            

            //清空bill
            string DeleteBill = "delete from Bill";
            sqlcon.Open();
            SqlCommand deletebill = new SqlCommand(DeleteBill, sqlcon);
            deletebill.ExecuteNonQuery();
            sqlcon.Close();

               /*
            //删除GR中的记录  但是预留default_guest

            string DeleteGR = "delete from GR";
            sqlcon.Open();
            SqlCommand deletegr = new SqlCommand(DeleteGR, sqlcon);
            deletegr.ExecuteNonQuery();
            sqlcon.Close();

            //把Room里面的占用清空

            string updateRoom = "update Room set Rstate='空闲' where Rno = '" + roomno + "'";
            sqlcon.Open();
            SqlCommand updateroom = new SqlCommand(updateRoom, sqlcon);
            updateroom.ExecuteNonQuery();
            sqlcon.Close();

            */

            tabControl1.SelectedTab = tabPage3;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            int fno = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells[2].Value);

            string DeleteBill = "delete from Bill where Fno = fno";

            sqlcon.Open();

            SqlCommand delete = new SqlCommand(DeleteBill, sqlcon);
            delete.ExecuteNonQuery();

            //SqlDataAdapter da = new SqlDataAdapter(DeleteBill, sqlcon);//全删完了///////////////////////////
            //da.Fill(dt); 
            sqlcon.Close();

            string select = "select * from Bill where Rno = '" + roomno + "'";
            DataTable dt = new DataTable();

            try
            {
                sqlcon.Open();//打开
                SqlDataAdapter da = new SqlDataAdapter(select, sqlcon);
                da.Fill(dt);//进行填充
                DataView dv = new DataView(dt);
                dt = dv.ToTable(true);
                dataGridView3.DataSource = dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                sqlcon.Close();//关闭连接，释放资源
            }
        }   //清空

        private void tabPage3_Click(object sender, EventArgs e)
        {
        }   //退出

        private void pictureBox5_Click(object sender, EventArgs e)
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

        private void pictureBox6_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Minimized; 

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //删除GR中的记录  但是预留default_guest

            string DeleteGR = "delete from GR";
            sqlcon.Open();
            SqlCommand deletegr = new SqlCommand(DeleteGR, sqlcon);
            deletegr.ExecuteNonQuery();
            sqlcon.Close();

            //把Room里面的占用清空

            string updateRoom = "update Room set Rstate='空闲' where Rno = '" + roomno + "'";
            sqlcon.Open();
            SqlCommand updateroom = new SqlCommand(updateRoom, sqlcon);
            updateroom.ExecuteNonQuery();
            sqlcon.Close();

            MessageBox.Show("感谢光临！\n欢迎下次再来！", "提醒");

            timer1.Interval = 2000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
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
    }
}
