using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net.Http.Formatting;
using Owin;
using Microsoft.Owin;
using System.Web.Http;
using System.Net;


namespace JBBService
{
    class Startup
    {

        private static string _siteDir = System.Configuration.ConfigurationManager.AppSettings.Get("SiteDir");
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder app)
        {
            // web api 接口
            HttpConfiguration config = InitWebApiConfig();
            app.UseWebApi(config);

            //静态文件托管
            app.Use((context, fun) =>
            {
                return myhandle(context, fun);
            });
        }
        /// <summary>
        /// 路由初始化
        /// </summary>
        public HttpConfiguration InitWebApiConfig()
        {
            
            
            HttpConfiguration config = new HttpConfiguration();

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //1.默认路由
            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ////2.自定义路由一：匹配到action
            //config.Routes.MapHttpRoute(
            //    name: "ActionApi",
            //    routeTemplate: "actionapi/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            ////3.自定义路由二
            //config.Routes.MapHttpRoute(
            //                name: "GetMessage",
            //                routeTemplate: "api1/{controller}/{action}/{pcode}",
            //                defaults: new { pcode = RouteParameter.Optional }
            //            );



            //config.Routes.MapHttpRoute(
            //    name: "Default",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

             config.Formatters
               .XmlFormatter.SupportedMediaTypes.Clear();
            //默认返回 json
            config.Formatters
                .JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "json", "application/json"));
            //返回格式选择
            config.Formatters
                .XmlFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "xml", "application/xml"));
            //json 序列化设置
            config.Formatters
                .JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                };
            return config;
        }


        public Task myhandle(IOwinContext context, Func<Task> next)
        {
            //获取物理文件路径
            var path = GetFilePath(context.Request.Path.Value);

            //验证路径是否存在
            if (File.Exists(path))
            {
                return SetResponse(context, path);
            }

            //不存在返回下一个请求
            return next();
        }
        public static string GetFilePath(string relPath)
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory
                , _siteDir
                , relPath.TrimStart('/').Replace('/', '\\'));
        }

        public Task SetResponse(IOwinContext context, string path)
        {
            var perfix = Path.GetExtension(path);
            if (perfix == ".html")
                context.Response.ContentType = "text/html; charset=utf-8";
            else if (perfix == ".js")
                context.Response.ContentType = "application/x-javascript";
            else if (perfix == ".js")
                context.Response.ContentType = "atext/css";
            return context.Response.WriteAsync(File.ReadAllText(path));
        }


    }
}





