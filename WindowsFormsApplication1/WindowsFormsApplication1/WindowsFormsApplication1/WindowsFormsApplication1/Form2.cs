using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        
        public static bool bmap=false,b5=false,b2=false,left=false,right=false,down=false,check=false;
        public static Point last;
        
        public Form2()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            button5_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn sẽ điều khiển bạch tuộc đỏ di chuyển bằng chuột hoặc bàn phím\nNếu sử dụng chuột, bạn click vào 4 nút\n- Up: Di chuyển lên\n- Down: Di chuyển xuống\n- Left: Di chuyển sang trái\n- Right: Di chuyển sang phải\nNếu sử dụng bàn phím, bạn sẽ di chuyển bằng 4 phím\n- w/W: Di chuyển lên\n- s/S: Di chuyển sang xuống\n- a/A: Di chuyển sang trái\n- d/D: Di chuyển sang phải\nBạn sẽ thắng cuộc nếu sau 10s, bạch tuộc cam không thể bắt được bạch tuộc đỏ\nChọn Map phù hợp bằng cách click chuột vào nút Map và click chuột vào nút Play để bắt đầu trò chơi \nCả bạn và máy nếu chạy vào lỗ đen đều sẽ bị dịch chuyển đến 1 vị trí ngẫu nhiên ", "Luật chơi");
        }

        public static int time = 10;

        private void button5_Click(object sender, EventArgs e)
        {
           
            if (time == 0)
            {
               
                label1.Text = "10";
                time = 10;
            }
            map.InitMap();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1_Tick(sender,e);
            last = map.Oct1Position;
            map.Oct1Position = last;
            map.ShowPath();
            b2 = true;
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "10";
            timer1.Enabled = true;
            time--;
            label1.Text = time.ToString();
            if (check) timer1.Stop();
            if (time == 0)
            {
                timer1.Stop();
                check = true;
                MessageBox.Show("MISSION PASSED", "Congratulations");
                //timer1.Enabled = false;
                time = 10;
                label1.Text = "10";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bmap = true;
            
            last=map.Oct1Position;
            map.InitMap();
            map.Oct1Position = last; // lưu lại vị trí của bạch tuộc 1 khi ấn nút 
            label1.Text = "10";
            time = 10;
            check = false;
            timer1.Enabled = false;
            if (b2 == true)
            {
                map._path.Clear();
                b2 = false;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            last = map.Oct1Position;// lưu lại vị trí của bạch tuộc 1 khi ấn nút 
            
            b5 = true; // đánh dấu đã ấn nút này 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            last = map.Oct1Position;

            left = true; // đánh dấu đã ấn nút này để di chuyển qua trái 
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            last = map.Oct1Position;
            right = true;// đánh dấu đã ấn nút này để di chuyển qua phải
        }

        private void button8_Click(object sender, EventArgs e)
        {
            last = map.Oct1Position;
            down = true;// đánh dấu đã ấn nút này để di chuyển xuống dưới
        }


        private void button2_KeyPress(object sender, KeyPressEventArgs e) // Nhận sự kiện đầu vào là các phím awsd hoặc AWSD trong trường hợp người dùng ấn nút play 
        {

            if (e.KeyChar == 115 || e.KeyChar == 83)
            {
                last = map.Oct1Position; // lưu lại vị trí 
                down = true; // đánh dấu là sẽ di chuyển xuống 
            };
            if (e.KeyChar == 119 || e.KeyChar == 87)
            {
                last = map.Oct1Position; // tương tự
                b5 = true; // lên trên
            };
            if (e.KeyChar == 97 || e.KeyChar == 65)
            {
                last = map.Oct1Position;
                left = true;
            };
            if (e.KeyChar == 100 || e.KeyChar == 64)
            {
                last = map.Oct1Position;
                right = true;
            }
        }

        private void map_KeyPress(object sender, KeyPressEventArgs e)// Nhận sự kiện đầu vào là các phím awsd hoặc AWSD trong trường hợp người dùng ấn nút play rồi vô tình ấn qua map , tương tự hàm trên
        {

            if (e.KeyChar == 115 || e.KeyChar == 83)
            {
                last = map.Oct1Position;
                down = true;
            };
            if (e.KeyChar == 119 || e.KeyChar == 87)
            {
                last = map.Oct1Position;
                b5 = true;
            };
            if (e.KeyChar == 97 || e.KeyChar == 65)
            {
                last = map.Oct1Position;
                left = true;
            };
            if (e.KeyChar == 100 || e.KeyChar == 64)
            {
                last = map.Oct1Position;
                right = true;
            }
        }

        
    }
}
