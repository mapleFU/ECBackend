using ECBack.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        private QRCodeGenerator qrGenerator;
        private OracleDbContext db;
        public TransactionController(): base()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            db = new OracleDbContext();
        }
        
        /// <summary>
        /// API 开始交易
        /// </summary>
        /// <returns></returns>
        [Route("api/Orderform/{OrderID:int}/Begin")]
        public async Task<IHttpActionResult> StartTransaction(int OrderID)
        {

            var odf = await db.Orderforms.FindAsync(OrderID);
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 确认交易, 只要手机扫描了这个就可以认证，不需要特殊的COOKIES
        /// </summary>
        /// <returns></returns>
        [Route("api/confirm")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ConfirmTransaction([FromUri] string token)
        {
            throw new NotImplementedException();
        }
    }
}
