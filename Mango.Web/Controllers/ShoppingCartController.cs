using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        public ShoppingCartController(IShoppingCartService shoppingCart, IOrderService orderService)
        {
            _shoppingCartService = shoppingCart;
            _orderService = orderService;

        }
        [Authorize]
        public async Task<IActionResult> ShoppingCartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUserAsync());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUserAsync());
        }
        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {

            CartDto cart = await LoadCartDtoBasedOnLoggedInUserAsync();
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.FirstName = cartDto.CartHeader.FirstName;

            var response = await _orderService.CreateOrderAsync(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            if (response != null && response.IsSuccess)
            {
                //get stripe session and redirect to stripe to place order
                //
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";

                StripeRequestDto stripeRequestDto = new()
                {
                    ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                    CancelUrl = domain + "cart/checkout",
                    OrderHeader = orderHeaderDto
                };

                var stripeResponse = await _orderService.CreateStripeSessionAsync(stripeRequestDto);
                StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>
                                            (Convert.ToString(stripeResponse.Result));
                Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                return new StatusCodeResult(303);



            }
            return View(cartDto);
        }
        public async Task<IActionResult> Confirmation(int orderId)
        {
            return View(orderId);
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int detailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value; 
            ResponseDto? response =await _shoppingCartService.RemoveCartAsync(detailsId);

            return RedirectToAction(nameof(ShoppingCartIndex));
        }
        [Authorize]
        public async Task<IActionResult> ApplyCouponCart(CartDto cartDto)
        {
            ResponseDto response = await _shoppingCartService.ApplyCouponAsync(cartDto);

            if ((bool)response.Result) 
            {
                //toaster message success
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            //toaster message invalid
            return RedirectToAction(nameof(ShoppingCartIndex));
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {

            CartDto cart=await LoadCartDtoBasedOnLoggedInUserAsync();
            cart.CartHeader.Email=User.Claims.Where(u=>u.Type==JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
            ResponseDto response = await _shoppingCartService.EmailCartAsync(cart);

            if ((bool)response.Result)
            {
                //toaster message success
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            //toaster message invalid
            return RedirectToAction(nameof(ShoppingCartIndex));
        }
        [Authorize]
        public async Task<IActionResult> RemoveCouponCart(CartDto cartDto)
        {
            ResponseDto response = await _shoppingCartService.RemoveCouponAsync(cartDto);

            if ((bool)response.Result)
            {
                //toaster message success
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            //toaster message invalid
            return RedirectToAction(nameof(ShoppingCartIndex));
        }



        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUserAsync()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response =await _shoppingCartService.GetCartAsync(userId);
            if(response != null && response.IsSuccess) 
            {
                CartDto cartDto=JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }
    }
}
