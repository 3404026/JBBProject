using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
using JBBService.models;

namespace JBBService.controllers
{
    public class ValuesController : ApiController
    {
        //api/valuses/time
        [HttpGet]
        [HttpPost]
        [HttpBasicAuthenticationFilter]  //IE地址栏访问http://admin:api.admin@172.21.111.104:12821/api/Values/time
        public timeResult time()
        {
            return new timeResult()
            {
                id = DateTime.Now.Ticks,
                time = DateTime.Now,
                remark = DateTime.Now.ToString()
            };
        }


        [HttpGet]
        [HttpPost]
        public string getSleepx(string sleep)
        {
            return "getSleep：";
            //if (sleep < 1 || sleep > 10)
            //    sleep = 1;
            //sleep *= 1000;

            //var begionTime = DateTime.Now.ToString("HH:mm:ss");
            //System.Threading.Thread.Sleep(sleep);
            //var endTime = DateTime.Now.ToString("HH:mm:ss");
            //return new
            //{
            //    sleep = sleep,
            //    begionTime = begionTime,
            //    endTime = endTime
            //};
        }

        public string getxxx(string sleep)
        {
            return "getxxx：";
        }
       
        [HttpGet]
        [HttpPost]
        public string getWithPara(string id)
        {
            return "Para:," + id.ToString();
        }

        [HttpGet]
        [HttpPost]
        public string getNoPara()
        {
            return "no Para";
        }







        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "返回值是：" + id.ToString();
        }


        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}

