using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ECBack.Controllers
{
    /// <summary>
    /// 用于付款的控制器
    /// https://stackoverflow.com/questions/20998695/qr-code-with-http-post
    /// 生成二维码 + TOKEN 当作付款认证
    /// </summary>
    public class BuyController : ApiController
    {
    }
}
