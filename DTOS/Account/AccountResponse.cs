namespace BackendSpicy.DTOS.Account
{
    public class AccountResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Tell { get; set; }
        public string Address { get; set; }
        public string? Image { get; set; }
        public string RoleName { get; set; }

        static public AccountResponse FromAccount(Models.Account account)
        {
            // return ตัวมันเอง
            return new AccountResponse
            {
                ID = account.ID,
                Name = account.Name,
                Image = !string.IsNullOrEmpty(account.Image) ? "https://localhost:7286/" + "images/" + account.Image : "",
                Email = account.Email,
                Tell = account.Tell,
                Address = account.Address,
                RoleName = account.Role.Name
            };
        }
    }
}
