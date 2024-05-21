using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.Encodings.Web;
using Innerglow_App.Services;
using Innerglow_App.Areas.Identity.Data;
using Innerglow_App.Models;
using Innerglow_App.Repositories;

namespace Innerglow_App.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly ILogger _logger;
        public CartController(ICartRepository cartRepo, ILogger<CartRepository> logger) 
        { 
            _cartRepo = cartRepo;
            _logger = logger;
            _emailSender = emailSender;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> AddItem(int ProductId, int Qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(ProductId, Qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int ProductId)
        {
            var cartCount = await _cartRepo.RemoveItem(ProductId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItem()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }
        /*public async Task<IActionResult> Checkout(string address, string phoneNum)
        {
            bool isCheckedOut = await _cartRepo.DoCheckout();
            if (!isCheckedOut)
                throw new Exception("Something happen in server side");
            return RedirectToAction("Index", "Home");
        }*/
        public async Task<IActionResult> Checkout()
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";

            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";

            string orderInfo = "Innerglow payment";
            string returnUrl = "https://localhost:7163/Cart/Check";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test
            int price = await _cartRepo.GetTotalPrice();
            _logger.LogInformation(price.ToString());
            string amount = price.ToString();//Số tiền cần thanh toán
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString(); //Định danh mỗi yêu cầu
            string extraData = "";//Thông tin bổ sung cho order theo định dạng <key>=<value>;<key>=<value>mặc định là ""

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MomoSecurity crypto = new MomoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            _logger.LogInformation(jmessage.ToString());
            return Redirect(jmessage.GetValue("payUrl").ToString());
            /*return true;*/
        }

        public async Task<ActionResult> Check(Result result)
        {
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            string rErrorCode = result.errorCode; // = 0: thanh toán thành công
            bool succeed = false;
            //Huy: Bad
            _logger.LogInformation("abc" + rErrorCode);
            if (short.Parse(rErrorCode) == 0)
            {
                _logger.LogInformation(HttpContext.Session.GetString("Notes"));
                string address = HttpContext.Session.GetString("Address");
                string number = HttpContext.Session.GetString("Number");
                string notes = HttpContext.Session.GetString("Notes");
                _logger.LogInformation(notes + number + address);
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                await _emailSender.SendEmailAsync(user.Email, "Đơn hàng của bạn đã nhập đủ thông tin và thanh toán thành công",
                            $"Khi bạn nhận được email này nghĩa là đơn hàng của bạn đã được xác nhận vui lòng xem tiến trình giao hàng trên web" +
                            $" <br/> Cám ơn bạn đã tin tưởng Innerglow chúng tôi.");
                bool isCheckedOut = await _cartRepo.DoCheckout(address, number, notes);
                if (!isCheckedOut)
                    throw new Exception("Something happen in server side");
                _logger.LogInformation(address);
                succeed = true;
            }
            return View(succeed);
        }


        public async Task<ActionResult> ConfirmPaymentClient()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmPaymentClient(string address, string number, string notes)
        {
            //cập nhật dữ liệu vào db
            String a = "";
            bool isCheckedOut = await _cartRepo.DoCheckout(address, number, notes);
            if (!isCheckedOut)
                throw new Exception("Something happen in server side");
            _logger.LogInformation(address);
            return RedirectToAction("UserOrders", "UserOrder");
        }
    }
}
