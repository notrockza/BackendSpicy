using BackendSpicy.interfaces;

namespace BackendSpicy.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;
        public UploadFileService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {


            this.webHostEnvironment = webHostEnvironment;  //เข้าหา wwwroot
            this.configuration = configuration;  //เข้าหา appsettings.json  

        }



        public bool IsUpload(IFormFileCollection formFiles)
        {
            return formFiles != null && formFiles?.Count > 0;
        }

        public async Task<List<string>> UploadImages(IFormFileCollection formFiles)
        {
            var listFileName = new List<string>();
            var uploadPath = $"{webHostEnvironment.WebRootPath}/images/";




            //if (Directory.Exists(uploadPath))
            //{
            //    Directory.Delete(uploadPath);
            //}

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            //อันนี้เป็นการup โหลด ได้หลายๆ file ตัวอย่าง image/111111.ppg
            //เพราะ ชื่อ file ที่เรา up โหลด + กับ นามของfile
            foreach (var formFile in formFiles)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                string fullName = uploadPath + fileName;

                using (var stream = File.Create(fullName))
                {
                    await formFile.CopyToAsync(stream);

                }
                listFileName.Add(fileName);

            }
            return listFileName;
        }

        public string Validation(IFormFileCollection formFiles)
        {
            foreach (var file in formFiles)
            {
                //เเชกนามสกุล
                if (!ValidationExtension(file.FileName))
                {
                    return "Invalid file extension";
                }

                if (!ValidationSize(file.Length))
                {
                    return "The file is too large";
                }
            }
            return null;
        }

        public bool ValidationExtension(string filename)
        {
            string[] permittedExtensions = { ".jpg", ".png" };

            //ตรวดสอบนามสณุน
            string extension = Path.GetExtension(filename).ToLowerInvariant();

            //ตรวดสอบนามสณุนว่าเป็นค่า null แป็นป่าว 
            //ถ้าไม่จริงจะส่ง จริงออกไป ถ้าไม่จะส่ง เท็ดออกไป
            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))

            {
                return false;
            };

            return true;
        }

        public bool ValidationSize(long fileSize) => configuration.GetValue<long>("FileSizeLimit") > fileSize;

        public Task DeleteImage(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var uploadPath = $"{webHostEnvironment.WebRootPath}/images/";
                string fullName = uploadPath + fileName;

                if (File.Exists(fullName)) File.Delete(fullName);
            }
            return Task.CompletedTask;
        }
    }


}
