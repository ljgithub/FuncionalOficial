namespace FarmaEnlace.Models
{
    public class ChangePasswordRequest
    {
        public string Identification { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
