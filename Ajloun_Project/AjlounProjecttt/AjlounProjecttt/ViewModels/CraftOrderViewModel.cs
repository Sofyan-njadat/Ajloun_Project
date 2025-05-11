namespace AjlounProjecttt.ViewModels
{
    public class CraftOrderViewModel
    {
        public int OrderId { get; set; }
        public string CraftTitle { get; set; }
        public string UserName { get; set; }
        public int? Quantity { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Status { get; set; }
    }

}
