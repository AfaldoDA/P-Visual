namespace GuestRegistrationApp.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? InvitationCode { get; set; }
        public System.DateTime RegistrationDate { get; set; }
    }
}
