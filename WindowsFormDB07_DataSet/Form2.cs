using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormDB07_DataSet
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        MySqlConnection conn;

        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection("server=localhost;port=3306;database=pocketmon;uid=root;pwd=''");

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    label1.Text = "포켓몬 도감";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlDataAdapter sda = new MySqlDataAdapter("Select * from user where id='"+ textBox1.Text + "'And password = '" + textBox2.Text + "'", conn);

            DataTable newTable = new DataTable();

            sda.Fill(newTable);

            try
            {
                if (newTable.Rows[0][0].ToString() == "0") { }
                MessageBox.Show("로그인 성공");
                Form1 newForm1 = new Form1();
                newForm1.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("아이디 패스워드 틀렷어요");
            }
        }

        public void SetData(String Data)
        {
            textBox1.Text = Data;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 newForm3 = new Form3();
            newForm3.Show();
        }
    }
}
