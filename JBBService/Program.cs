
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBBService.controllers;


using Microsoft.Owin.Hosting;
//using System.Configuration;  


namespace JBBService
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //}

        static void Main(string[] args)
        {
            //监听地址  
            string domain = System.Configuration.ConfigurationManager.AppSettings["Domain"];
            string port = System.Configuration.ConfigurationManager.AppSettings["Port"];
            string sslport = System.Configuration.ConfigurationManager.AppSettings["sslPort"];
            
            string baseAddress = string.Format("http://{0}:{1}", domain, port);
            string baseAddress_ssl = string.Format("https://{0}:{1}", domain, sslport);

            StartOptions sop = new StartOptions();
            sop.Urls.Add(baseAddress);
            sop.Urls.Add(baseAddress_ssl); //监控2个端口


            //启动监听  
            //using (WebApp.Start<Startup>(url: baseAddress))
            using (WebApp.Start<Startup>(sop))
            {
                Console.WriteLine("host 已启动：{0}", DateTime.Now);
                Console.WriteLine("访问：{0}/pages/index.html", baseAddress);
                Console.WriteLine("访问：{0}/pages/index.html", baseAddress_ssl);

                //ProductIOController prodio = new ProductIOController();
                //List<string> list = new List<string>();
                //list.Add(DateTime.Now.ToString());
                //list.Add(DateTime.Now.ToString());
                //int x = prodio.Add(list);
                
                Console.ReadLine();


            }
        }  


    }
}
