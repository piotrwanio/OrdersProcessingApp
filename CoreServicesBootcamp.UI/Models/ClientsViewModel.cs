using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreServicesBootcamp.UI.Models
{
    public class ClientsViewModel
    {
        public int SelectedId { get; set; }
        public List<SelectListItem> ClientList { get; set; }
    }
}
