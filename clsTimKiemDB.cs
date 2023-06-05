using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//ta đang dùng .NET 6.0 (chứ ko phải .Net FrameWork xx)
//nuget GUI: cài Microsoft.Data.SqlClient vào
using Microsoft.Data.SqlClient;

namespace botKMT
{
    public class clsTimKiemDB
    {
        static string cnStr = "Server=DESKTOP-2HC9Q42\\TEST01;Database=BTL;Trusted_Connection=True;TrustServerCertificate=True;";

        public static string TimKiem(string q)
        {
            string kq = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cnStr))
                {
                    cn.Open();
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = "SP_Search_NV";
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.Parameters.Add("@q", SqlDbType.NVarChar, 50).Value = "%" + q.Replace(' ','%') + "%";
                        object obj = cm.ExecuteScalar(); //lấy col1 of row1
                        if (obj != null)
                            kq = (string)obj;
                        else
                            kq = "không có dữ liệu";
                    }
                }
            }catch (Exception ex)
            {
                kq += $"Error: {ex.Message}";
            }
            return kq;
        }
    }
}
