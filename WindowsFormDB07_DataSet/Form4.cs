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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        MySqlConnection conn;
        MySqlDataAdapter dataAdapter;
        DataSet dataSet;
        private void Form4_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection("server=localhost;port=3306;database=pocketmon;uid=root;pwd=''");
            dataAdapter = new MySqlDataAdapter("SELECT ball FROM backpack", conn);
            dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "backpack");
            dataGridView1.DataSource = dataSet.Tables["backpack"];
        }
    }
}
