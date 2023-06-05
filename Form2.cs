using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Data.SqlClient;

namespace botKMT
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cnStr = "Server=DESKTOP-2HC9Q42\\TEST01;Database=BTL;Trusted_Connection=True;TrustServerCertificate=True;";
            string kq;
            try
            {
                using (SqlConnection cn = new SqlConnection(cnStr))
                {
                    cn.Open();
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = "select ten from nv where manv=@id;";
                        cm.CommandType = CommandType.Text;
                        cm.Parameters.Add("@id", SqlDbType.Int).Value = 2;
                        object obj = cm.ExecuteScalar(); //lấy col1 of row1
                        if (obj != null)
                            kq = (string)obj;
                        else
                            kq = "không có dữ liệu";
                    }
                }
            }
            catch (Exception ex)
            {
                kq = $"Error: {ex.Message}";
            }
            textBox1.Text = kq;
        }
    }
}

