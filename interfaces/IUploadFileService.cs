namespace BackendSpicy.interfaces
{
    public interface IUploadFileService
    {
        //ตรวจสอบมีการอัพโหลดไฟล์เข้ามาหรือไม่
        //IFormFileCollection มันคือ [] ชนิดหนึ่งที่ใช้ upโหลดfile
        bool IsUpload(IFormFileCollection formFiles);

        //ตรวจสอบนามสกุลไฟล์หรือรูปแบบที่่ต้องการ
        string Validation(IFormFileCollection formFiles);

        //อัพโหลดและส่งรายชื่อไฟล์ออกมา
        Task<List<string>> UploadImages(IFormFileCollection formFiles);

        Task DeleteImage(string fileName);
    }
}
