using BackendSpicy.DTOS.Products;
using BackendSpicy.interfaces;
using BackendSpicy.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendSpicy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;
        private readonly IProductService productService;

        public ProductController(DatabaseContext databaseContext, IProductService productService)
        {
            this.databaseContext = databaseContext;
            this.productService = productService;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetProduct()
        {
            //var result = await productService.GetAll();
            var result = (await productService.GetAll()).Select(ProductResponse.FromProduct);
            return Ok(new { data = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByID(int id)
        {
            var result = await productService.GetByID(id);
            if (result == null) return Ok(new { msg = "ไม่พบสินค้า" });
            return Ok(new { msg = "OK", data = result });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductRequest productRequest)
        {


            (string errorMesage, string imageName) = await productService.UploadImage(productRequest.FormFiles);
            var result = await productService.GetByID(productRequest.ID);
            if (result != null)
            {
                return Ok(new { msg = "รหัสซ้ำ" });
            }

            var product = productRequest.Adapt<Product>();
            product.Image = imageName;
            await productService.Create(product);
            return Ok(new { msg = "OK", data = product });



        }

        [HttpPut( "[action]")]
        public async Task<ActionResult<Product>> UpdateProduct([FromForm] ProductRequest productRequest)
        {
            var result = await productService.GetByID(productRequest.ID);
            if (result == null)
            {
                return Ok(new { msg = "ไม่พบสินค้า" });
            }

            #region การจัดการรูปภาพ
            (string errorMessage, string imageName) = await productService.UploadImage(productRequest.FormFiles);
            if (!string.IsNullOrEmpty(errorMessage)) return BadRequest(errorMessage);
            //ตรวจสอบมีการส่งไฟล์เข้ามาแก้ไขหรือไม่

            if (!string.IsNullOrEmpty(imageName))
            {
                await productService.DeleteImage(result.Image);
                result.Image = imageName;
            }
            #endregion

             productRequest.Adapt(result);
            await productService.Update(result);
            return Ok(new { msg = "OK", data = result });
        }

        [HttpDelete("[action]")]
        // [FromQuery] int id ใส่เต็มยศ
        public async Task<ActionResult<Product>> DeleteProduct([FromQuery] int id)
        {
            var result = await productService.GetByID(id);
            if (result == null) return Ok(new { msg = "ไม่พบสินค้า" });
            await productService.Delete(result);
            await productService.DeleteImage(result.Image);
            return Ok(new { msg = "OK", data = result });
        }

    }
}
