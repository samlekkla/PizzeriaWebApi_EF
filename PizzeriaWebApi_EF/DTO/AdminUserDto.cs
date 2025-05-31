namespace PizzeriaWebApi_EF.DTO
{
    public class AdminUserDto
    {

        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }


    }
}
