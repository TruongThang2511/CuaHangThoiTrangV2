using CuaHangThoiTrangV2.Extensions;
using CuaHangThoiTrangV2.Helper;
using CuaHangThoiTrangV2.Helper.Momo;
using CuaHangThoiTrangV2.Helper.VNPay;
using CuaHangThoiTrangV2.Models;
using CuaHangThoiTrangV2.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Web;
using System;

namespace CuaHangThoiTrangV2.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly dbCHTTContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CheckoutController(dbCHTTContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if(gh == default(List<CartItem>))
                { 
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        public IActionResult Index()
        {
            ViewBag.PaymentID = new SelectList(_context.Phuongthucthanhtoans, "MaPt", "TenPt", 1);
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("MaNd");
            MuaHangViewModel model = new MuaHangViewModel();
            if (taikhoanID != null)
            {
                var khachhang = _context.Nguoidungs.AsNoTracking().SingleOrDefault(x => x.MaNd == Convert.ToInt32(taikhoanID));
                model.MaNd = khachhang.MaNd;
                model.TenNd = khachhang.TenNd;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Sdt;
                model.Diachi = khachhang.Diachi;
                model.Note = ""; 
            }
            else
            {
                return RedirectToAction("Login", "Acounts");
            }
            ViewBag.GioHang = cart;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MuaHangViewModel model)
        {
            ViewBag.PaymentID = new SelectList(_context.Phuongthucthanhtoans, "MaPt", "TenPt", 1);

            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("MaNd");
            if (taikhoanID != null)
            {
                var khachhang = _context.Nguoidungs.AsNoTracking().SingleOrDefault(x => x.MaNd == Convert.ToInt32(taikhoanID));
                model.MaNd = khachhang.MaNd;
                model.TenNd = khachhang.TenNd;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Sdt;
                model.Diachi = khachhang.Diachi;
                ViewBag.PaymentID = _context.Phuongthucthanhtoans.ToList();

                khachhang.Diachi = model.Diachi;
                //Console.WriteLine(donhangForm);
                _context.Update(khachhang);
                _context.SaveChanges();
            }
            try
            {
                if (ModelState.IsValid)
                {

                   
                    //donhang.MaPtNavigation.MaPt = model.PaymentID;

                    Donhang donhang = new Donhang();
                    donhang.MaNd = model.MaNd;
                    donhang.HotenKh = model.TenNd;
                    donhang.Diachi = model.Diachi;
                    donhang.Email = model.Email;
                    string s = model.PaymentID.ToString();
                    if (int.TryParse(s, out int ptthanhtoan))
                    {
                        donhang.MaPt = ptthanhtoan;
                    }
                    donhang.Tonggiatri = Convert.ToInt32(cart.Sum(x => x.tongTien));
                    donhang.NgayDat = DateTime.Now;
                    donhang.TrangThai = "Chưa xác nhận";
                    //donhang.Ghichu = model.Note.ToString();
                    donhang.Ghichu = model.Note ?? "Không có ghi chú";
                    _context.Add(donhang);
                    _context.SaveChanges();
                    foreach (var item in cart)
                    {
                        Chitietdonhang orderDetail = new Chitietdonhang();
                        orderDetail.MaDh = donhang.MaDh;
                        orderDetail.MaSp = item.sanpham.MaSp;
                        orderDetail.Soluong = item.amount;
                        orderDetail.Tonggiatri = donhang.Tonggiatri;
                        orderDetail.Gia = item.sanpham.Gia;
                        _context.Add(orderDetail);
                    }

                    _context.SaveChanges();
                    if (ptthanhtoan == 2)
                    {
                        return RedirectToAction("PaymentMomo", "Checkout");
                    }
                    if(ptthanhtoan == 3)
                    {
                        return RedirectToAction("PaymentVNPay", "Checkout");
                    }
                    HttpContext.Session.Remove("GioHang");
                    return RedirectToAction("Success");
                }
                else
                {
                    // Hiển thị thông báo lỗi
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        // Log hoặc hiển thị thông báo lỗi theo cách phù hợp với ứng dụng của bạn
                        Console.WriteLine(error.ErrorMessage);
                    }

                    // Trả về view với mô hình hiện tại để người dùng có thể xem lỗi
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.GioHang = cart;
                return View(model);
            }
        }

        public IActionResult Success()
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("MaNd");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Accounts", new { returnUrl = "/Checkout/Success" });
                }
                var khachhang = _context.Nguoidungs.AsNoTracking().SingleOrDefault(x => x.MaNd == Convert.ToInt32(taikhoanID));
                var donhang = _context.Donhangs.Where(x => x.MaNd == Convert.ToInt32(taikhoanID)).OrderByDescending(x => x.MaDh).FirstOrDefault();
                MuaHangSuccessViewModel successVM = new MuaHangSuccessViewModel();
                successVM.tenNd = khachhang.TenNd;
                successVM.madon = donhang.MaDh;
                successVM.Phone = khachhang.Sdt;  
                successVM.Address = khachhang.Diachi;
                return View(successVM);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult PaymentMomo()
        {

            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            double total = 0;
            foreach (var item in cart)
            {
                total += item.tongTien;
            }
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán đồ thời trang";
            string returnUrl = "https://localhost:7704/Checkout/ConfirmPaymentClient";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Checkout/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = total.ToString();
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

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

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        public ActionResult ConfirmPaymentClient(Result result)
        {

            var taikhoanID = HttpContext.Session.GetString("MaNd");
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            var khachhang = _context.Nguoidungs.AsNoTracking().SingleOrDefault(x => x.MaNd == Convert.ToInt32(taikhoanID));
            var donhang = _context.Donhangs.Where(x => x.MaNd == Convert.ToInt32(taikhoanID)).OrderByDescending(x => x.MaDh).FirstOrDefault();
            MuaHangSuccessViewModel successVM = new MuaHangSuccessViewModel();
            successVM.tenNd = khachhang.TenNd;
            successVM.madon = donhang.MaDh;
            successVM.Phone = khachhang.Sdt;
            successVM.Address = khachhang.Diachi;
            HttpContext.Session.Remove("GioHang");
            return View(successVM);
        }
        [HttpPost]
        public void SavePayment()
        {
            //cập nhật dữ liệu vào db
        }

        public ActionResult PaymentVNPay()
        {
            string url = _configuration["AppSettings:Url"];
            string returnUrl = _configuration["AppSettings:ReturnUrl"];
            string tmnCode = _configuration["AppSettings:TmnCode"];
            string hashSecret = _configuration["AppSettings:HashSecret"];
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            double total = 0;
            foreach (var item in cart)
            {
                total += item.tongTien;
            }
            total = total * 100;

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", total.ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress(_httpContextAccessor)); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toán đồ thời trang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "fashion"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }
        public ActionResult PaymentConfirm()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNd");
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            var khachhang = _context.Nguoidungs.AsNoTracking().SingleOrDefault(x => x.MaNd == Convert.ToInt32(taikhoanID));
            var donhang = _context.Donhangs.Where(x => x.MaNd == Convert.ToInt32(taikhoanID)).OrderByDescending(x => x.MaDh).FirstOrDefault();
            MuaHangSuccessViewModel successVM = new MuaHangSuccessViewModel();
            successVM.tenNd = khachhang.TenNd;
            successVM.madon = donhang.MaDh;
            successVM.Phone = khachhang.Sdt;
            successVM.Address = khachhang.Diachi;
            if (Request.Query.Count > 0)
            {
                string hashSecret = _configuration["AppSettings:HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.Query;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData.Keys)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        string value = vnpayData[s].ToString();
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.Query["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }
            HttpContext.Session.Remove("GioHang");
            return View(successVM);
        }
    }
}
