using BackendSpicy.interfaces;
using BackendSpicy.Models;
using BackendSpicy.Settings;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BackendSpicy.Services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext databaseContext;
        private readonly JwtSetting jwtSetting;
        private readonly IUploadFileService uploadFileService;

        public AccountService(DatabaseContext databaseContext, JwtSetting jwtSetting, IUploadFileService uploadFileService)
        {
            this.databaseContext = databaseContext;
            this.jwtSetting = jwtSetting;
            this.uploadFileService = uploadFileService;

        }

        public async Task<Account> Login(string email, string password)
        {
            var result = await databaseContext.Account.Include(x => x.Role).SingleOrDefaultAsync(p => p.Email == email);

            if (result != null && VerifyPassword(result.Password, password))
            {
                return result;
            }
            return null;


        }



        public async Task<object> Register(Account account)
        {
            if (account.RoleID == 0)
            {
                account.RoleID = 1;
            }

            var result = await databaseContext.Account.SingleOrDefaultAsync(x => x.Email == account.Email);
            if (result != null) return new { msg = "อีเมลซ้ำ" };


            account.Password = CreateHashPassword(account.Password);

            await databaseContext.Account.AddAsync(account);
            await databaseContext.SaveChangesAsync();
            return null;
        }




        private string CreateHashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            var hashed = HashPassword(password, salt);

            var hpw = $"{Convert.ToBase64String(salt)}.{hashed}";
            return hpw;
        }


        private bool VerifyPassword(string saltAndHashFromDB, string password)
        {
            //ตังอย่าง PA5f4kwKm/nHFWnwPtjnig==.u8DSuxHRbTfcEWnXv5cUEgJvzNP9WKkwd4UVPPzCqBc=
            // . จะเป็นจุดตัด จะได้ password มา 2 ท้อน //ถ้ามีมากกว่า 2 เเสดงว่าผิด
            var parts = saltAndHashFromDB.Split('.', 2);
            if (parts.Length != 2) return false;
            //เเปลงค่า
            var salt = Convert.FromBase64String(parts[0]);
            var passwordHash = parts[1];

            //ให้ทำการhas ใหม่ ถ้าทำการhas เเล้วเหมือนกับ รหัสเก่า = ถูก
            var hashed = HashPassword(password, salt);

            return hashed == passwordHash;
        }

        private string HashPassword(string password, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public string GenerateToken(Account account)
        {
            //payload หรือ claim ข้อมูลที่ต้องการเก็บ ใส่อะไรก็ได้//
            //Claim("Sub", account.Username) ใส่ค่าที่ที่ไม่ซ้ำเช่น Username
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,account.Email),
                new Claim("role",account.Role.Name),
                new Claim("additonal","TestSomething"),
                new Claim("todo day","10/10/99"),

            };

            return BuildToken(claims);

        }

        private string BuildToken(Claim[] claims)
        {
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSetting.Expire)); //ดึงข้อหมดอายุมา เเล้ว + วันที่
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key)); //ให้ทำการเข้ารหัสอีกครั้ง 1
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //สร้าง Token ของเเท้
            var token = new JwtSecurityToken(
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            //เขียน Token ออกมา
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Account GetInfo(string accessToken)
        {
            //แปลงค่า Token (ถอดรหัส)
            var token = new JwtSecurityTokenHandler().ReadToken(accessToken) as JwtSecurityToken;

            //ค้นหาค่า key ขึ้นมา
            var email = token.Claims.First(claim => claim.Type == "sub").Value;
            var role = token.Claims.First(claim => claim.Type == "role").Value;

            var account = new Account
            {
                Email = email,
                Role = new Role
                {
                    Name = role
                }
            };

            return account;
        }

        public async Task<Account> GetByID(int id)
        {
            var result = await databaseContext.Account.Include(e => e.Role).AsNoTracking().FirstOrDefaultAsync(e => e.ID == id);
            if (result == null) return null;
            return result;
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

        public async Task<IEnumerable<Account>> GetAll()
        {
            return await databaseContext.Account.Include(e => e.Role).ToListAsync();
        }

        public  async Task UpdateAccount(Account account)
        {
            databaseContext.Update(account);
            await databaseContext.SaveChangesAsync();
        }
    }
}
