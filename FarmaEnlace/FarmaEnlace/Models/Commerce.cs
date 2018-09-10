using SQLite;

namespace FarmaEnlace.Models
{
    public class Commerce
    {
        [PrimaryKey]
        public int CommerceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ScheduleMondayToFriday { get; set; }
        public string ScheduleSaturday { get; set; }
        public string ScheduleSunday { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public bool ShiftPharmacy { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Distance { get; set; }
        public string State { get; set; }
        public double Stock { get; set; }

        #region Methods
        public override int GetHashCode()
        {
            return CommerceId;
        }
        #endregion

       
    }
}
