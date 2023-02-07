using BackendSpicy.DTOS.OrderAccount;
using BackendSpicy.interfaces;
using BackendSpicy.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendSpicy.Services
{
    public class OrderAccountService : IOrderAccountService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IUploadFileService uploadFileService;
        public OrderAccountService(DatabaseContext databaseContext, IUploadFileService uploadFileService)
        {
            this.databaseContext = databaseContext;
            this.uploadFileService = uploadFileService;
        }

        public async Task AddOrder(OrderAccount orderAccount, ProductListOrderRequest productListOrderRequest)
        {
            if (string.IsNullOrEmpty(orderAccount.ID))
            {
                orderAccount.ID = await GenerateIdOrderCustomer();
            }
            var productList = new List<ProductList>();
            for (var i = 0; i < productListOrderRequest.ProductID.Length; i++)
            {
                var item = new ProductList()
                {
                    ID = await GenerateIdProductListr(),
                    OrderAccountID = orderAccount.ID,
                    ProductID = productListOrderRequest.ProductID[i],
                    ProductAmount = Convert.ToInt32(productListOrderRequest.ProductAmount[i]),
                    ProductPrice = productListOrderRequest.ProductPrice[i]
                };
                productList.Add(item);
            }
            //---------- AddRangeAsync เป็นการ Add ทั้งหมด ------
            await databaseContext.OrderAccount.AddRangeAsync(orderAccount);


            // ------------------ Add สินค้าทั้งหมดไว้ใน List ---------------
            await databaseContext.ProductList.AddRangeAsync(productList);
            //---------------------------------------------------------

            await RemoveCartProduct(productListOrderRequest);

            await RemoveStockProduct(productListOrderRequest);


            await databaseContext.SaveChangesAsync();
        }



        public async Task<IEnumerable<OrderAccount>> GetAll(int idAccount)
        {
            return await databaseContext.OrderAccount.Where(e => e.AccountID == idAccount).ToListAsync();
        }

        public async Task<OrderAccount> GetByID(string id)
        {
            var result = await databaseContext.OrderAccount.AsNoTracking().FirstOrDefaultAsync(e => e.ID == id);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task RemoveCartProduct(ProductListOrderRequest productListOrderRequest)
        {
            if (productListOrderRequest.ProductID.Length > 0 && productListOrderRequest != null)
            {
                for (var i = 0; i < productListOrderRequest.ProductID.Length; i++)
                {
                    var result = await databaseContext.Cart.AsNoTracking().FirstOrDefaultAsync(e => e.ID == productListOrderRequest.CartID[i]);
                    databaseContext.Remove(result);
                }
            }

        }

        public async Task RemoveStockProduct(ProductListOrderRequest productListOrderRequest)
        {
            if (productListOrderRequest.ProductID.Length > 0 && productListOrderRequest != null)
            {
                for (var i = 0; i < productListOrderRequest.ProductID.Length; i++)
                {
                    var result = await databaseContext.Product.AsNoTracking().FirstOrDefaultAsync(e => e.ID == productListOrderRequest.ProductID[i]);
                    result.Stock -= productListOrderRequest.ProductAmount[i];
                    databaseContext.Update(result);
                }
            }
        }

        public async Task<string> GenerateIdProductListr()
        {
            Random randomNumber = new Random();
            string IdProductListr = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                IdProductListr = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;


                var result = await databaseContext.ProductList.FindAsync(IdProductListr);

                if (result == null)
                {
                    break;
                };
            }
            return IdProductListr;
        }

        public async Task<string> GenerateIdOrderCustomer()
        {
            Random randomNumber = new Random();
            string IdProductListr = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                IdProductListr = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;


                var result = await databaseContext.OrderAccount.FindAsync(IdProductListr);

                if (result == null)
                {
                    break;
                };
            }
            return IdProductListr;
        }

        public async Task<IEnumerable<ProductList>> GetAllProductList(string idOrder)
        {
            return await databaseContext.ProductList.Include(e => e.Product).Where(e => e.OrderAccountID == idOrder).ToListAsync();

        }

        public async Task UpdateOrder(OrderAccount orderAccount)
        {
            databaseContext.Update(orderAccount);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<(string errorMessage, string imageName)> UploadImage(IFormFileCollection formFiles)
        {
            var errorMessage = string.Empty;
            //var imageName = new List<string>();
            var imageName = string.Empty;
            if (uploadFileService.IsUpload(formFiles))
            {
                errorMessage = uploadFileService.Validation(formFiles);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    imageName = (await uploadFileService.UploadImages(formFiles))[0];
                }
            }
            return (errorMessage, imageName);
        }

        public async Task DeleteImage(string fileName)
        {
            await uploadFileService.DeleteImage(fileName);
        }

        public async Task ConfirmOrder(List<OrderAccount> orderAccounts)
        {
            for (var i = 0; i < orderAccounts.Count(); i++)
            {
                orderAccounts[i].PaymentStatus = true;
                databaseContext.Update(orderAccounts[i]);
            }
            await databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderAccount>> GetConfirm()
        {
            var result = await databaseContext.OrderAccount.Include(e => e.Account).Where(e => e.ProofOfPayment != null).ToListAsync();
            if (result == null)
            {
                return null;
            }
            return result;
        }
    }
}
