using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Simulation.Library.ViewModels
{
    public class vmSelectListItem
    {
        [Key]
        public int ValueMember { get; set; }
        public string DisplayMember { get; set; }
    }
}