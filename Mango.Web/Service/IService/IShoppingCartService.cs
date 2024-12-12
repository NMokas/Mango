using Mango.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Service.IService
{
    public interface IShoppingCartService
    {
        Task<ResponseDto> GetCartAsync(string userId);
        Task<ResponseDto> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto> RemoveCartAsync(int cartDetailsId);
        Task<ResponseDto> ApplyCouponAsync(CartDto cartDto);
        Task<ResponseDto> EmailCartAsync(CartDto cartDto);
        Task<ResponseDto> RemoveCouponAsync(CartDto cartDto);

    }
}
