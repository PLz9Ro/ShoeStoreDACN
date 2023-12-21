using ShoeStore.Models;

namespace ShoeStore.ModelViews
{
    public class CartItem
    {
        public Product product { get; set; }
        public int amount {  get; set; }
        public string? size {  get; set; }
        public string? thumb { get; set; }
        public string? title { get; set; }
        public string? productName { get; set; }

        public double TotalMoney => amount * product.Price.Value;
    }
}
