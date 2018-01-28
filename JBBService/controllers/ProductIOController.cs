using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.Http;
using MySql.Data.MySqlClient;
using JBBService.models;

namespace JBBService.controllers
{
    public class ProductIOController : ApiController
    {
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();

        //1、ajax中传入数组 data: JSON.stringify(["a", "b", "c", "defa"]) 
        //   那么后端acion的参数也要定义为数组string[] ids， 即：
        //   public bool SaveData(string[] ids)
        //离柜rfid产品上传服务器，存入数据库
        public int SaveData(string[] ids)
        {
            try
            {
                string[] listRfid = ids;
                string strLeaveDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    DbTransaction trans = con.BeginTransaction();//开始事务  
                    for (int i = 0; i < listRfid.Count(); i++)
                    {
                        string sqlInsert = "insert into t_product_leave(rfid_no,leave_dt) values(@rfid_no,@leave_dt)";
                        MySqlCommand cmd2 = new MySqlCommand(sqlInsert, con);
                        cmd2.Parameters.Add(new MySqlParameter("@rfid_no", MySqlDbType.String));
                        cmd2.Parameters.Add(new MySqlParameter("@leave_dt", MySqlDbType.String));
                        cmd2.Parameters["@rfid_no"].Value = listRfid[i];
                        cmd2.Parameters["@leave_dt"].Value = strLeaveDt;
                        cmd2.ExecuteNonQuery();
                    }
                    trans.Commit();//提交事务    
                    return listRfid.Count();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        //1、ajax中传入数组 data: JSON.stringify(["a", "b", "c", "defa"]) 
        //   那么后端acion的参数也要定义为数组string[] ids， 即：
        //   public bool SaveData(string[] ids)
        //在柜rfid上传服务器，存入数据库
        public int SaveInventory(string[] ids)
        {
            try
            {
                string[] listRfid = ids;
                string strCrtDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                string strDt = DateTime.Now.ToString("yyyy-MM-dd");
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    DbTransaction trans = con.BeginTransaction();//开始事务
                    //删除当天上一次的盘点数据
                    MySqlCommand cmdDel = new MySqlCommand("delete from t_inventory where date_format(create_date,'%Y-%m-%d')='" + strDt+"';", con);
                    cmdDel.ExecuteNonQuery();

                    for (int i = 0; i < listRfid.Count(); i++)
                    {
                        string sqlInsert = "insert into t_inventory(rfid_no,create_date) values(@rfid_no,@create_date)";
                        MySqlCommand cmd2 = new MySqlCommand(sqlInsert, con);
                        cmd2.Parameters.Add(new MySqlParameter("@rfid_no", MySqlDbType.String));
                        cmd2.Parameters.Add(new MySqlParameter("@create_date", MySqlDbType.String));
                        cmd2.Parameters["@rfid_no"].Value = listRfid[i];
                        cmd2.Parameters["@create_date"].Value = strCrtDt;
                        cmd2.ExecuteNonQuery();
                    }
                    trans.Commit();//提交事务    
                    return listRfid.Count();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        //获取所有未售产品
        [HttpPost]
        [HttpGet]
        public List<Product> GetList()
        {
            List<Product> list1 = new List<Product>();
            string sqlSelect = @"select rfid_no,  prod_name,  price,  prod_code,  my_code, remark , status, prod_id ,my_code_name  from  t_product where status=0 and  rfid_no<>'';";
            try
            {
                DataSet ds = MySqlHelper.GetDataSet(connectionString, CommandType.Text, sqlSelect);
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    //list1.Add(new Chart() { name = row.ItemArray[0].ToString(), value = Convert.ToInt32(row.ItemArray[1]), pcode = row.ItemArray[2].ToString() });
                    list1.Add(new Product
                    {
                        rfidNo = row.ItemArray[0].ToString(),
                        prodName = row.ItemArray[1].ToString(),
                        Price = Convert.ToInt32(row.ItemArray[2]),
                        prodCode = row.ItemArray[3].ToString(),
                        MyCode = row.ItemArray[4].ToString(),
                        remark = row.ItemArray[5].ToString(),
                        Status = Convert.ToInt32(row.ItemArray[6]),
                        prodId = Convert.ToInt32(row.ItemArray[7]),
                        MyCodeName = row.ItemArray[8].ToString()
                    });
                }
                return list1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
