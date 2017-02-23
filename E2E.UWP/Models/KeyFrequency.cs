using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.Models
{
    public class KeyFrequency
    {
        public int Id { get; set; }
        public int Frequency { get; set; }

        [MaxLength(20)]
        public string Word { get; set; }
    }
}
