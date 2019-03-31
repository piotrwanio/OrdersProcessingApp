using CoreServicesBootcamp.DAL.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreServicesBootcamp.DAL.Entities
{
    public class Request
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(6)]
        public int ClientId { get; set; }
        public long RequestId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
    
}
