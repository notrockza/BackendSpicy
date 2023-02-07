using BackendSpicy.DTOS.ProductDescription;
using BackendSpicy.interfaces;
using BackendSpicy.Models;
using BackendSpicy.Services;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendSpicy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductDescriptionController : ControllerBase
    {
        private readonly IProductsDescriptionlService productDescriptionService;

        public ProductDescriptionController(IProductsDescriptionlService productDescriptionService)
        {
            this.productDescriptionService = productDescriptionService;
        }
        [HttpPost("[action]")]
        // [FromBody] คือ Json 
        public async Task<IActionResult> AddProductDescription([FromForm] ProductDescriptionRequest productDescriptionRequest)
        {
            #region จัดการรูปภาพ
            (string erorrMesage, List<string> imageName) = await productDescriptionService.UploadImage(productDescriptionRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            #endregion

            var productDescription = productDescriptionRequest.Adapt<ProductDescription>();
            await productDescriptionService.Create(productDescription, imageName);

            return Ok(new { msg = "OK" });
        }

        [HttpGet("[action]/{idProduct}")]
        public async Task<IActionResult> GetDetailAll(int idProduct)
        {
            var result = (await productDescriptionService.GetAll(idProduct)).Select(ProductDescriptionResponse.FromDescription);
            //return Ok(await orderAccountService.GetAll(idAccount));

            //return Ok((await productDescriptionService.GetAll(idProduct)).Select(ProductDescriptionResponse.FromDescription));

            if (result.Count() == 0) return Ok(new { msg = "ไม่พบรูป" });
            return Ok(new { msg = "OK", data = result });
        }
    }
}
