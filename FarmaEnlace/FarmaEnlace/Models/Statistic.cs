using SQLite;

namespace FarmaEnlace.Models
{
    public class StatisticGeneral
    {
        #region Properties        
        [PrimaryKey, AutoIncrement]
        public int StatisticGeneralId { get; set; }
        public int BrandId { get; set; }
        public string DeviceSO { get; set; }
        public int SumProducts { get; set; }
        public int SumPharmacys { get; set; }
        public int SumSalesCode { get; set; }
        #endregion
    }
    
    public class StatisticCategory
    {
        #region Properties
        [PrimaryKey]
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int FarmaEnlaceId { get; set; }
        public int SumCategory { get; set; }
        #endregion
    }

    public class StatisticProduct
    {
        #region Properties
        [PrimaryKey]
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int SumProduct { get; set; }
        #endregion
    }
}
