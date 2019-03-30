using CoreServicesBootcamp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreServicesBootcamp.DAL.Entities
{
    //represents all request rows in client's request
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Request")]
        public int OrderId { get; set; }

        public int ClientId { get; set; }
        public long RequestId { get; set; }
        public double Amount { get; set; }
        
        public virtual List<Request>  Requests { get; set; }
    }
}
