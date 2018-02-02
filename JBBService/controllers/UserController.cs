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


        ////[HttpPost, HttpGet]
        //public string PostGetInfo()
        //{
        //    return "API测试地址";
        //}

        //[HttpGet]
        //public string  categorycnt()
        //{
             
        //    List<Chart> list1 = new  List<Chart>();
        //    //Hashtable hs = new Hashtable();
        //    list1.Add(new Chart(){name="A",value=111,pcode="jd-101"});
        //    list1.Add(new Chart(){name="B",value=222,pcode="jd-102"});
        //    list1.Add(new Chart(){name="C",value=333,pcode="jd-103"});

        //    return JsonConvert.SerializeObject(list1);

        //}

        //[HttpGet]
        //public string categorycnt2()
        //{

        //    List<Chart> list1 = new List<Chart>();
        //    //Hashtable hs = new Hashtable();
        //    list1.Add(new Chart() { name = "D", value = 444, pcode = "jd-104" });
        //    list1.Add(new Chart() { name = "E", value = 555, pcode = "jd-105" });
        //    list1.Add(new Chart() { name = "F", value = 666, pcode = "jd-106" });

        //    return JsonConvert.SerializeObject(list1);

        //}

        



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
                sqlSelect = @"select DISTINCT y.my_code_name name , x.cnt value, x.mycode pcode 
                              from 
                                (
                                  select   substr(a.my_code,1,3) mycode , count(*) cnt  from t_product a , t_product_leave b  where   a.rfid_no = b.rfid_no   
                                  group by substr(a.my_code,1,3)
                                ) x , t_product y
                              where x.mycode = y.my_code";

            else
                sqlSelect = @"select DISTINCT y.my_code_name name , x.cnt value, x.mycode pcode 
                              from 
                                (
                                select   a.my_code mycode, count(*) cnt  from t_product a , t_product_leave b  where   a.rfid_no = b.rfid_no  and a.my_code like '" + pcode.ToString() +"-%' " +
                             @" group by a.my_code
                                ) x , t_product y
                              where x.mycode = y.my_code ";
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

            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            DbTransaction trans = con.BeginTransaction();//开始事务  

            string sqldel = "delete from t_inventory_stay where create_date in (select max(create_date) from t_inventory)";
            MySqlCommand cmdDel = new MySqlCommand(sqldel, con);
            cmdDel.ExecuteNonQuery();

            string sqlIns = "insert into t_inventory_stay  select *  from t_inventory where create_date in (select max(create_date) from t_inventory)";
            MySqlCommand cmdIns = new MySqlCommand(sqlIns, con);
            cmdDel.ExecuteNonQuery();


            string sqlSelect = @"select 
	                            a.cname   name ,
	                            (select count(*) from t_inventory_stay x ,t_products y  where x.rfid_no = y.rfid_no and y.caizhi_code=a.para_code and x.create_date in (select max(create_date) from t_inventory_stay) )  value,
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
                          @" select  peidai_name , count(distinct rfid_no) jianshu  ,count(rfid_no) cishu ,  round(sum(timestampdiff(MINUTE,out_dttm,in_dttm))/60,1) sichang
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
                                                    timeCount = Convert.ToDecimal(row.ItemArray[3]) });
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
            public decimal timeCount { get; set; }
        }


    }
}




