using System.ComponentModel.DataAnnotations;

namespace PizzeriaWebApi_EF.Identity
{
    public class RegularUser: ApplicationUser
    {
        [Range(0, int.MaxValue)]
        public int BonusPoints { get; set; } = 0;
    }
}
