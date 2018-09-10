using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FarmaEnlace.Models
{
    public class StockProduct
    {
        [PrimaryKey, AutoIncrement]
        public int StockId { get; set; }
        [ForeignKey(typeof(Brand))]
        public int BrandId { get; set; }
        public int ProductId { get; set; }
        public string CodeProduct { get; set; }
        [ForeignKey(typeof(Commerce))]
        public int CommerceId { get; set; }
        public double Stock { get; set; }
        [ManyToOne]
        public Commerce Commerce { get; set; }
        [OneToMany]
        public Brand Brand { get; set; }
        public override int GetHashCode()
        {
            return StockId;
        }
    }
}
