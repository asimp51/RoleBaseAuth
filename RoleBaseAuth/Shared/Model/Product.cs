using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleBaseAuth.Shared.Model
{
    public class Product
    {
        public int Id { get; set; }
        public  string Name { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } 
        public string Description { get; set; } = string.Empty;
    }
}
