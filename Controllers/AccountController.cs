using BackendSpicy.DTOS.Account;
using BackendSpicy.interfaces;
using BackendSpicy.Models;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendSpicy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAccountAll()
        {
            return Ok((await accountService.GetAll()).Select(AccountResponse.FromAccount));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest registerRequest)
        {
            #region จัดการรูปภาพ
            (string erorrMesage, string imageName) = await accountService.UploadImage(registerRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            #endregion

            var account = registerRequest.Adapt<Account>();
            account.Image = imageName;
           
            //var token = accountService.GenerateToken(account);
            var data = await accountService.Register(account);

            if (data != null) return Ok(data);
            return Ok(new { msg = "OK", data = account });
            //return Ok(token);
            //if (account == null) return Ok(new { msg = "ไม่พบอีเเมว"});
            // return Ok(new { msg = "OK ", data = account  });

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest)
        {
            var account = await accountService.Login(loginRequest.Email, loginRequest.Password);
            if (account == null)
            {
                return Ok(new { msg = "เข้าสู่ระบบไม่สำเร็จ" });
            }
            //var token = accountService.GenerateToken(account);

            //return Ok(token);
            return Ok(new { msg = "OK", data = account });


        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetAccountByID(int id)
        {
            var result = await accountService.GetByID(id);
            if (result == null)
            {
                return Ok(new { msg = "ไม่มีผู้ใช้งานนี้" });
            }
            return Ok(AccountResponse.FromAccount(result));
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateAccount([FromForm] AccountRequest accountRequest)
        {
            var result = await accountService.GetByID(accountRequest.ID);
            if (result != null)
            {
                if (accountRequest.Name != null)
                {
                    result.Name = accountRequest.Name;
                }
                if (accountRequest.Email != null)
                {
                    result.Email = accountRequest.Email;
                }
                if (accountRequest.Password != null)
                {
                    result.Password = accountRequest.Password;
                }
                if (accountRequest.Tell != null)
                {
                    result.Tell = accountRequest.Tell;
                }
                if (accountRequest.Address != null)
                {
                    result.Address = accountRequest.Address;
                }

            }
            await accountService.UpdateAccount(result);
            return Ok(new { msg = "OK" });
        }




        //[HttpGet("[action]")]
        //public async Task<ActionResult> Info()
        //{
        //    //อ่านค่า Token (คล้ายๆ การอ่าน session)
        //    var accessToken = await HttpContext.GetTokenAsync("access_token");
        //    if (accessToken == null) return Unauthorized();

        //    var account = accountService.GetInfo(accessToken);
        //    //ได้ account เเล้ว username and role คือ ใคร
        //    return Ok(new
        //    {
        //        Email = account.Email,
        //        role = account.Role.Name
        //    });
        //}
    }
}
