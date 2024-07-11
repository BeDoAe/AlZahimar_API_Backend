using System.ComponentModel.DataAnnotations.Schema;

namespace ZahimarProject.Models
{
    public class RelativePayment
    {
        public RelativePayment()
        {
            Quantity = 1;
            OederDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Quantity * Price;
        public DateTime OederDate { get; set; }
        [ForeignKey("Relative")]
        public int RelativeID { get; set; }
        public Relative Relative { get; set; }
    }
}
