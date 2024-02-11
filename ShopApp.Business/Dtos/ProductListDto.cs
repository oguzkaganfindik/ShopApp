namespace ShopApp.Business.Dtos
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? UnitPrice { get; set; }
        public int UnitInStock { get; set; }
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }
    }
}
