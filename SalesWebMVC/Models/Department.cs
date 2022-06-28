using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} required!")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public Department() { }

        public Department(string name)
        {
            Name = name;
        }
        public void AddSeller(Seller sl)
        {
            Sellers.Add(sl);
        }
        public void RemoveSeller(Seller sl)
        {
            Sellers.Remove(sl);
        }
        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sellers
                .Sum(sl => sl.TotalSales(initial, final));
        }
    }
}