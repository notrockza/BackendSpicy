using BackendSpicy.DTOS.Cart;
using BackendSpicy.interfaces;
using BackendSpicy.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendSpicy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCartCustomerAll(int idAccount)
        {
            var data = (await cartService.GetAll(idAccount)).Select(CartResponse.FromCart);
            return Ok(data);
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCartCustomerByID(string id)
        {
            var data = CartResponse.FromCart(await cartService.GetByID(id));
            return Ok(data);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddCartCustomer([FromForm] CartRequest cartRequest)
        {
            var cartCustomer = cartRequest.Adapt<Cart>();
            await cartService.Create(cartCustomer);
            return Ok(new { msg = "OK" });
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCartCustomer([FromForm] CartRequest cartRequest)
        {
            var cartCustomer = await cartService.GetByID(cartRequest.ID);
            if (cartCustomer == null)
            {
                return Ok(new { msg = "ไม่พบสินค้า" });
            }
            var result = cartRequest.Adapt(cartCustomer);
            await cartService.Update(result);
            return Ok(new { msg = "OK" });
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteCartCustomer(string id)
        {
            var result = await cartService.GetByID(id);
            if (result == null)
            {
                return Ok(new { msg = "ไม่พบสินค้า" });
            }

            await cartService.Delete(result);
            return Ok(new { msg = "OK", data = result });
        }
    }
}
