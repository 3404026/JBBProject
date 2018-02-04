using MySql.Data.MySqlClient;
using System.Data.Common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using JBBService.models;
using System.Data;
using System.Data;

namespace JBBService.controllers
{
    public class UserController : ApiController
    {

        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();

        //浏览器测试 webaip
        //https://172.21.111.104:12321/api/User/getCategoryCnt?Pcode=0
        //http://172.21.111.104:1232/api/User/getCategoryCnt?Pcode=0
        //参考资料https://www.cnblogs.com/landeanfen/p/5337072.html#_label1_0
        [HttpPost]
        [HttpGet]
        public string getCategoryCnt(string pcode)
        {

            List<Chart> list1 = new List<Chart>();
            string sqlSelect="";

            if (pcode.ToString() == "0")
                sqlSelect = @"select DISTINCT y.cname   name , x.cnt value, x.mycode pcode 
                              from 
                                (
                                  select   a.caizhi_code mycode, count(*) cnt  from t_products a , t_product_io b  where   a.rfid_no = b.rfid_no   
                                  group by a.caizhi_code
                                ) x , t_param y
                              where x.mycode = y.para_code";

            else
                sqlSelect = @"select DISTINCT y.cname   name , x.cnt value, x.mycode pcode 
                              from 
                                (
                                  select   a.peidai_code mycode, count(*) cnt  from t_products a , t_product_io b  where   a.rfid_no = b.rfid_no  and a.caizhi_code ='" + pcode.ToString() +"' " +
                                  @" group by a.caizhi_code
                                ) x , t_param y
                              where x.mycode = y.para_code";
            try
            {
                DataSet  ds =  MySqlHelper.GetDataSet(connectionString, CommandType.Text, sqlSelect);
                foreach (DataRow row in ds.Tables[0].Rows)  
                {
                    list1.Add(new Chart() { name = row.ItemArray[0].ToString(), value = Convert.ToInt32(row.ItemArray[1]), pcode = row.ItemArray[2].ToString() });
                }
                return JsonConvert.SerializeObject(list1);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //浏览器测试 webaip
        //https://172.21.111.104:12321/api/User/getCategoryCnt?Pcode=0
        //http://172.21.111.104:1232/api/User/getCategoryCnt?Pcode=0
        //参考资料https://www.cnblogs.com/landeanfen/p/5337072.html#_label1_0
        [HttpPost]
        [HttpGet]
        public string getInventoryByCaizhi()
        {

            List<Chart> list1 = new List<Chart>();
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            DbTransaction trans = con.BeginTransaction();//开始事务  

            //string sqldel = "delete from t_inventory_stay where create_date in (select max(create_date) from t_inventory)";
            //MySqlCommand cmdDel = new MySqlCommand(sqldel, con);
            //int intDel = cmdDel.ExecuteNonQuery();


            //string sqlIns = "insert into t_inventory_stay  select *  from t_inventory where create_date in (select max(create_date) from t_inventory)";
            //MySqlCommand cmdIns = new MySqlCommand(sqlIns, con);
            //cmdIns.ExecuteNonQuery();


            //string sqlSelect = @"select 
            //                 a.cname   name ,
            //                 (select count(*) from t_inventory_stay x ,t_products y  where x.rfid_no = y.rfid_no and y.caizhi_code=a.para_code and x.create_date in (select max(create_date) from t_inventory_stay) )  value,
            //                    a.para_code pcode
            //                from  t_param  a 
            //                where a.parent_para_id=10";

            string sqlSelect = @"select 
                             a.cname   name ,
                             (select count(*) from t_inventory x ,t_products y  where x.rfid_no = y.rfid_no and y.caizhi_code=a.para_code and x.create_date in (select max(create_date) from t_inventory) )  value,
                                a.para_code pcode
                            from  t_param  a 
                            where a.parent_para_id=10";

            try
            {
                ds = MySqlHelper.GetDataSet(connectionString, CommandType.Text, sqlSelect);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (Convert.ToInt32(row.ItemArray[1])>0)
                        list1.Add(new Chart() { name = row.ItemArray[0].ToString(), value = Convert.ToInt32(row.ItemArray[1]), pcode = row.ItemArray[2].ToString() });
                }
                return JsonConvert.SerializeObject(list1);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //浏览器测试 webaip
        //https://172.21.111.104:12321/api/User/getCategoryCnt?Pcode=0
        //http://172.21.111.104:1232/api/User/getCategoryCnt?Pcode=0
        //参考资料https://www.cnblogs.com/landeanfen/p/5337072.html#_label1_0
        [HttpPost]
        [HttpGet]
        public string getInventoryByPeidai(string pcode)
        {

            List<inventory> list1 = new List<inventory>();
            string sqlSelect =
                          @" select  peidai_name , count(distinct rfid_no) jianshu  ,count(rfid_no) cishu ,  sum(timestampdiff(minute,out_dttm,in_dttm)) sichang
                             from v_inventory_io 
                             where caizhi_code = '" + pcode +"'" +
                           " group by peidai_name";

            try
            {
                DataSet ds = MySqlHelper.GetDataSet(connectionString, CommandType.Text, sqlSelect);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (Convert.ToInt32(row.ItemArray[1]) > 0)
                        list1.Add(new inventory() { peidaiName = row.ItemArray[0].ToString(),   
                                                    jianShu = Convert.ToInt32(row.ItemArray[1]),
                                                    ciShu   = Convert.ToInt32(row.ItemArray[2]),
                                                    timeCount = Convert.ToDouble(row.ItemArray[3]) });
                }
                return JsonConvert.SerializeObject(list1);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        class inventory {
            public string peidaiName { get; set; }
            public int jianShu { get; set; }
            public int ciShu { get; set; }
            public double timeCount { get; set; }
        }

        #region 秒转换小时 SecondToHour  
        /// <summary>  
        /// 秒转换小时  
        /// </summary>  
        /// <param name="time"></param>  
        /// <returns></returns>  
        public static string SecondToHour(double time)
        {
            string str = "";
            int hour = 0;
            int minute = 0;
            int second = 0;
            second = Convert.ToInt32(time);

            if (second > 60)
            {
                minute = second / 60;
                second = second % 60;
            }
            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }
            return (hour + ":" + minute + ":"
                + second );
        }
        #endregion

    }
}




