using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.Http;
using MySql.Data.MySqlClient;
using JBBService.models;
using System.Data.SqlClient;

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
                string strCurrDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    DbTransaction trans = con.BeginTransaction();//开始事务  
                    for (int i = 0; i < listRfid.Count(); i++)
                    {
                        //回柜  更新
                        string sqlSelect = "select id from  t_product_io where in_dttm is null and  rfid_no = @rfid_no ";
                        MySqlCommand cmdSelect = new MySqlCommand(sqlSelect, con);
                        cmdSelect.Parameters.Add(new MySqlParameter("@rfid_no", MySqlDbType.String));
                        cmdSelect.Parameters["@rfid_no"].Value = listRfid[i];
                        if (cmdSelect.ExecuteScalar()==null)
                        //MySqlDataReader rd = cmdSelect.ExecuteReader();
                        //if (rd.HasRows==false)
                        {
                            //离柜，插入
                            string sqlInsert = "insert into t_product_io(rfid_no,out_dttm) values(@rfid_no,@out_dttm)";
                            MySqlCommand cmd2 = new MySqlCommand(sqlInsert, con);
                            cmd2.Parameters.Add(new MySqlParameter("@rfid_no", MySqlDbType.String));
                            cmd2.Parameters.Add(new MySqlParameter("@out_dttm", MySqlDbType.String));
                            cmd2.Parameters["@rfid_no"].Value = listRfid[i];
                            cmd2.Parameters["@out_dttm"].Value = strCurrDt;
                            cmd2.ExecuteNonQuery();
                        }
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



        public int UpdateProdIOinDttm(string[] ids)
        {
            try
            {
                string[] listRfid = ids;
                string strCurrDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    DbTransaction trans = con.BeginTransaction();//开始事务  
                    for (int i = 0; i < listRfid.Count(); i++)
                    {
                        //回柜  更新
                        string sqlUpdate = "update t_product_io set in_dttm = @in_dttm where in_dttm is null and  rfid_no = @rfid_no ";
                        MySqlCommand cmdUpdate = new MySqlCommand(sqlUpdate, con);
                        cmdUpdate.Parameters.Add(new MySqlParameter("@rfid_no", MySqlDbType.String));
                        cmdUpdate.Parameters.Add(new MySqlParameter("@in_dttm", MySqlDbType.String));
                        cmdUpdate.Parameters["@rfid_no"].Value = listRfid[i];
                        cmdUpdate.Parameters["@in_dttm"].Value = strCurrDt;
                        cmdUpdate.ExecuteNonQuery();
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
                string strCrtDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string strDt = DateTime.Now.ToString("yyyy-MM-dd");
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    DbTransaction trans = con.BeginTransaction();//开始事务
                    //删除当天上一次的盘点数据
                    MySqlCommand cmdDel = new MySqlCommand("delete from t_inventory where date_format(create_date,'%Y-%m-%d')='" + strDt + "';", con);
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
            string sqlSelect = @"
                        select 
	                        rfid_no,  
	                        prod_name,  
	                        price,  
	                        company_code  
	                        prod_code,  
	                        CONCAT((select  a.para_code  from t_param  a where a.para_code = p.caizhi_code ) , (select  b.para_code  from t_param  b where b.para_code = p.peidai_code ))    my_code,
	                        remark , 
	                        status, 
	                        prod_id ,
	                        CONCAT((select  a.cname  from t_param  a where a.para_code = p.caizhi_code ) , (select  b.cname  from t_param  b where b.para_code = p.peidai_code ))    my_code_name  
                        from  t_products  p where status=0 and  rfid_no<>''";

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
