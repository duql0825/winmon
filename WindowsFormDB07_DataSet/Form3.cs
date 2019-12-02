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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        MySqlConnection conn;
        MySqlDataAdapter dataAdapter;
        DataSet dataSet;

        private void Form3_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection("server=localhost;port=3306;database=pocketmon;uid=root;pwd=''");
            dataAdapter = new MySqlDataAdapter("SELECT * FROM user", conn);
            dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "user");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "INSERT INTO user (id,password,name,hint)" +
                "VALUES (@id,@password,@name,@hint)";
            dataAdapter.InsertCommand = new MySqlCommand(sql, conn);
            dataAdapter.InsertCommand.Parameters.AddWithValue("@id", textBox2.Text);
            dataAdapter.InsertCommand.Parameters.AddWithValue("@password", textBox3.Text);
            dataAdapter.InsertCommand.Parameters.AddWithValue("@name", textBox4.Text);
            dataAdapter.InsertCommand.Parameters.AddWithValue("@hint", textBox5.Text);

            // 메모리상에 새로운 행(row) 생성
            DataRow newRow = dataSet.Tables["user"].NewRow();
            newRow["id"] = textBox2.Text;
            newRow["password"] = textBox3.Text;
            newRow["name"] = textBox4.Text;
            newRow["hint"] = textBox5.Text;
            dataSet.Tables["user"].Rows.Add(newRow);

            try
            {
                if (dataAdapter.Update(dataSet, "user") > 0) // 메모리상에 수정된 내용을 실제 DB에 업데이트
                {
                    dataSet.Clear();
                    dataAdapter.Fill(dataSet, "user");
                    MessageBox.Show("회원가입이 완료 되었습니다.");
                    this.Close();
                }
                else
                    MessageBox.Show("???");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
