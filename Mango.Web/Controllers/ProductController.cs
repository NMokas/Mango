using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new();

            ResponseDto? response = await _productService.GetAllProductAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _productService.CreateProductAsync(productDto);

                if (response.IsSuccess)
                {
                    return RedirectToAction("ProductIndex");
                }
            }
            return View(productDto);
        }


        public async Task<IActionResult> ProductDelete(int ProductId)
        {

            ResponseDto response = await _productService.DeleteProductAsync(ProductId);
            if (response.IsSuccess)
            {
                return RedirectToAction("ProductIndex");
                //add toastr
            }

            return RedirectToAction("ProductIndex");

        }
    }
}
