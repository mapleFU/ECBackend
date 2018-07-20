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
    public class TransactionController : ApiController
    {
        /// <summary>
        /// API 开始交易
        /// </summary>
        /// <returns></returns>
        [Route("api/begin")]
        public IHttpActionResult StartTransaction()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 确认交易, 只要手机扫描了这个就可以认证，不需要特殊的COOKIES
        /// </summary>
        /// <returns></returns>
        [Route("api/confirm")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ConfirmTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
