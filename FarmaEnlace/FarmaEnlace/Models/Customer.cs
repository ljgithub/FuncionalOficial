using SQLite;


namespace FarmaEnlace.Models
{
    public class Customer
    {
        [PrimaryKey]
        public int CustomerId { get; set; }
        public string Identification { get; set; }
        public string IdentificationGroup { get; set; }
        public string TypeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Imei { get; set; }
        public string TokenPush { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int? Year { get; set; }
        public bool AuthorizationPromotions { get; set; }


        public override int GetHashCode()
        {
            return CustomerId;
        }

    }
}
