using BackendSpicy.interfaces;
using BackendSpicy.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendSpicy.Services
{
    public class ProductDescriptionService : IProductsDescriptionlService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IUploadFileService uploadFileService;

        public ProductDescriptionService(DatabaseContext databaseContext, IUploadFileService uploadFileService)
        {
            this.databaseContext = databaseContext;
            this.uploadFileService = uploadFileService;
        }
        public async Task Create(ProductDescription productDescription, List<string> imageName)
        {

            for (var i = 0; i < imageName.Count(); i++)
            {
                productDescription.ID = await GenerateIdProductDescription();
                productDescription.Image = imageName[i];
                await databaseContext.AddAsync(productDescription);
                await databaseContext.SaveChangesAsync();

            }   
           
        }

        public async Task<string> GenerateIdProductDescription()
        {
            Random randomNumber = new Random();
            string IdProductDescription = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                IdProductDescription = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;

                var result = await databaseContext.ProductDescription.FindAsync(IdProductDescription);

                if (result == null)
                {
                    break;
                };
            }
            return IdProductDescription;
        }

        public async Task DeleteImage(string fileName)
        {
            databaseContext.Remove(fileName);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDescription>> GetAll(int idProduct)
        {
            return await databaseContext.ProductDescription.Where(e => e.ProductID == idProduct).ToListAsync();
            //return await databaseContext.OrderAccount.Where(e => e.AccountID == idAccount).ToListAsync();
        }

        public async Task<ProductDescription> GetByID(string ID)
        {
            return await databaseContext.ProductDescription.Include(e => e.ProductID).AsNoTracking().FirstOrDefaultAsync(x => x.ID == ID);
        }

        public async Task<(string errorMessage, List<string> imageName)> UploadImage(IFormFileCollection formFiles)
        {
            var errorMessage = string.Empty;
            var imageName = new List<string>();
            //var imageName = string.Empty;
            if (uploadFileService.IsUpload(formFiles))
            {
                errorMessage = uploadFileService.Validation(formFiles);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    imageName = (await uploadFileService.UploadImages(formFiles));
                }
            }
            return (errorMessage, imageName);
        }



    }
}
