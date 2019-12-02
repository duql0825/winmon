using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormDB07_DataSet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        MySqlConnection conn;
        MySqlDataAdapter dataAdapter;
        DataSet dataSet;
        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection("server=localhost;port=3306;database=pocketmon;uid=root;pwd=''");
            dataAdapter = new MySqlDataAdapter("SELECT * FROM monster", conn);
            dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "monster");
            dataGridView1.DataSource = dataSet.Tables["monster"];
            SetSearchComboBox();
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            string queryStr;
            string[] conditions = new string[5];
            conditions[0] = (comboBox1.Text != "") ? "name=@name" : null;
            conditions[1] = (comboBox2.Text != "") ? "personality=@personality" : null;
            conditions[2] = (comboBox3.Text != "") ? "type1=@type1" : null;
            conditions[3] = (textBox5.Text != "") ? "type2=@type2" : null;
            conditions[4] = (textBox6.Text != "") ? "weight=@weight" : null;



            if (conditions[0] != null || conditions[1] != null || conditions[2] != null || conditions[3] != null || conditions[4] != null)
            {
                queryStr = $"SELECT * FROM monster WHERE ";
                bool firstCondition = true;
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i] != null)
                        if (firstCondition)
                        {
                            queryStr += conditions[i];
                            firstCondition = false;
                        }
                        else
                        {
                            queryStr += " and " + conditions[i];
                        }
                }
            }
            else
            {
                queryStr = "SELECT * FROM monster";
            }
            dataAdapter.SelectCommand = new MySqlCommand(queryStr, conn);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@name", comboBox1.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@personality", comboBox2.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@type1", comboBox3.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@type2", textBox5.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@weight", textBox6.Text);

            try
            {
                conn.Open();
                dataSet.Clear();
                if (dataAdapter.Fill(dataSet, "monster") > 0)
                {
                    // 데이터가 있으면 화면에 표시
                    
                    if (comboBox1.Text != "")
                    {
                        label8.Text = comboBox1.Text;
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox1.Image = System.Drawing.Image.FromFile(@"C:\Users\USER\Desktop\image\" + comboBox1.Text + ".png");
                    }
                    
                    dataGridView1.DataSource = dataSet.Tables["monster"];
                }
                else
                    // 데이터 없을때
                    MessageBox.Show("검색된 데이터가 없습니다.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            #region InsertCommand를 이용한 처리
            //string sql = "INSERT INTO city (name,countrycode,district,population)" +
            //    "VALUES (@name,@countrycode,@district,@population)";
            //dataAdapter.InsertCommand = new MySqlCommand(sql, conn);
            //dataAdapter.InsertCommand.Parameters.AddWithValue("@name", textBox2.Text);
            //dataAdapter.InsertCommand.Parameters.AddWithValue("@countrycode", textBox3.Text);
            //dataAdapter.InsertCommand.Parameters.AddWithValue("@district", textBox4.Text);
            //dataAdapter.InsertCommand.Parameters.AddWithValue("@population", textBox5.Text);

            //try
            //{
            //    conn.Open();

            //    if (dataAdapter.InsertCommand.ExecuteNonQuery() > 0)
            //    {
            //        dataSet.Clear();
            //        dataAdapter.Fill(dataSet, "city");
            //        dataGridView1.DataSource = dataSet.Tables["city"];
            //    }
            //    else
            //        MessageBox.Show("???");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            #endregion

            #region Update()를 이용한 처리
            string sql = "INSERT INTO monster (name,personality,type1,type2,weight)" +
                "VALUES (@name,@personality,@type1,@type2,@weight)";
            dataAdapter.InsertCommand = new MySqlCommand(sql, conn);
            dataAdapter.InsertCommand.Parameters.AddWithValue("@name", comboBox1.Text);
            dataAdapter.InsertCommand.Parameters.AddWithValue("@personality", comboBox2.Text);
            dataAdapter.InsertCommand.Parameters.AddWithValue("@type1", comboBox3.Text);
            dataAdapter.InsertCommand.Parameters.AddWithValue("@type2", textBox5.Text);
            dataAdapter.InsertCommand.Parameters.AddWithValue("@weight", textBox6.Text);

            // 메모리상에 새로운 행(row) 생성
            DataRow newRow = dataSet.Tables["monster"].NewRow();
            newRow["name"] = comboBox1.Text;
            newRow["personality"] = comboBox2.Text;
            newRow["type1"] = comboBox3.Text;
            newRow["type2"] = textBox5.Text;
            newRow["weight"] = textBox6.Text;
            dataSet.Tables["monster"].Rows.Add(newRow);

            try
            {
                if (dataAdapter.Update(dataSet, "monster") > 0) // 메모리상에 수정된 내용을 실제 DB에 업데이트
                {
                    dataSet.Clear();
                    dataAdapter.Fill(dataSet, "monster");
                    dataGridView1.DataSource = dataSet.Tables["monster"];
                }
                else
                    MessageBox.Show("???");
                clr();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            string sql = "UPDATE monster SET name=@name,personality=@personality, type1=@type1,type2=@type2, weight= @weight WHERE name = @name";
            dataAdapter.UpdateCommand = new MySqlCommand(sql, conn);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@name", comboBox1.Text);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@personality", comboBox2.Text);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@type1", comboBox3.Text);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@type2", textBox5.Text);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@weight", textBox6.Text);

            #region Update Command를 이용한 처리
            try
            {
                conn.Open();
                dataAdapter.UpdateCommand.ExecuteNonQuery();

                dataSet.Clear();
                dataAdapter.Fill(dataSet, "monster");
                dataGridView1.DataSource = dataSet.Tables["monster"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                clr();
            }
            #endregion
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            string queryStr;
            string[] conditions = new string[5];
            conditions[0] = (comboBox1.Text != "") ? "name=@name" : null;
            conditions[1] = (comboBox2.Text != "") ? "personality=@personality" : null;
            conditions[2] = (comboBox3.Text != "") ? "type1=@type1" : null;
            conditions[3] = (textBox5.Text != "") ? "type2=@type2" : null;
            conditions[4] = (textBox6.Text != "") ? "weight=@weight" : null;

            if (conditions[0] != null || conditions[1] != null || conditions[2] != null || conditions[3] != null || conditions[4] != null)
            {
                queryStr = $"Delete From monster where ";
                bool firstCondition = true;
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i] != null)
                        if (firstCondition)
                        {
                            queryStr += conditions[i];
                            firstCondition = false;
                        }
                        else
                        {
                            queryStr += " and " + conditions[i];
                        }
                }
            }
            else
            {
                queryStr = "";
            }
            dataAdapter.DeleteCommand = new MySqlCommand(queryStr, conn);
            dataAdapter.DeleteCommand.Parameters.AddWithValue("@name", comboBox1.Text);
            dataAdapter.DeleteCommand.Parameters.AddWithValue("@personality", comboBox2.Text);
            dataAdapter.DeleteCommand.Parameters.AddWithValue("@type1", comboBox3.Text);
            dataAdapter.DeleteCommand.Parameters.AddWithValue("@type2", textBox5.Text);
            dataAdapter.DeleteCommand.Parameters.AddWithValue("@weight", textBox6.Text);


            try
            {
                conn.Open();
                dataAdapter.DeleteCommand.ExecuteNonQuery();

                dataSet.Clear();
                dataAdapter.Fill(dataSet, "monster");
                dataGridView1.DataSource = dataSet.Tables["monster"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                clr();
            }
        }

        private void clr()
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            textBox5.Clear();
            textBox6.Clear();
        }
       

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            string queryStr = "Select * from monster";
            dataAdapter.SelectCommand = new MySqlCommand(queryStr, conn);
            dataSet.Clear();
            dataAdapter.Fill(dataSet, "monster");
            label8.Text = "포켓몬 이름";
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = System.Drawing.Image.FromFile(@"C:\Users\USER\Desktop\image\white.png");
            clr();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Form4 newForm4 = new Form4();
            newForm4.Show();
        }
        private void SetSearchComboBox()
        {
           
            string sql = "SELECT distinct name FROM monster";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            try
            {
                // CountryCode 목록 표시
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())  // 다음 레코드가 있으면 true
                {
                    comboBox1.Items.Add(reader.GetString("name"));
                }
                reader.Close();

                // District 목록 표시
                sql = "SELECT distinct personality FROM monster";
                cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())  // 다음 레코드가 있으면 true
                {
                    comboBox2.Items.Add(reader.GetString("personality"));
                }
                reader.Close();

                sql = "SELECT distinct type1 FROM monster";
                cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())  // 다음 레코드가 있으면 true
                {
                    comboBox3.Items.Add(reader.GetString("type1"));
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT distinct name FROM monster";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", comboBox1.Text);
            
            try
            {
                // District 목록 표시
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())  // 다음 레코드가 있으면 true
                {
                    //comboBox1.Items.Add(reader.GetString("name"));
                }
               
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT distinct personality FROM monster";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@personality", comboBox2.Text);

            try
            {
                // District 목록 표시
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())  // 다음 레코드가 있으면 true
                {
                    //comboBox2.Items.Add(reader.GetString("personality"));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT distinct type1 FROM monster";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@type1", comboBox3.Text);

            try
            {
                // District 목록 표시
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())  // 다음 레코드가 있으면 true
                {
                    //comboBox3.Items.Add(reader.GetString("type1"));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

    }
}



