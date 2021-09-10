using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIDTest
{
    public class SampleModule : Nancy.NancyModule
    {
        public SampleModule()
        {
            Get["/"] = r =>
            {
                String name = Request.Query.name;//获取get方式提交的参数值
                                                 //Request.Form 用于获取request的请求
                                                 //Request.Body 二进制的请求可以这么获取
                return "hello world," + name;
            };
        }
    }
}
