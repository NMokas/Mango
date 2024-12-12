using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();

            ResponseDto? response = await _couponService.GetAllCouponAsync();

            if(response != null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> CouponCreate(CouponDto couponDto)
		{
            if (ModelState.IsValid)
            {
			    ResponseDto response = await _couponService.CreateCouponAsync(couponDto);

                if(response.IsSuccess)
                {
                    return RedirectToAction("CouponIndex"); 
                }
            }
			return View(couponDto);
		}

   
        public async Task<IActionResult> CouponDelete(int CouponId)
        {
            
            ResponseDto response = await _couponService.DeleteCouponAsync(CouponId);
            if (response.IsSuccess)
            {
                return RedirectToAction("CouponIndex");
                //add toastr
            }
			
			return RedirectToAction("CouponIndex");

		}
	}
}
