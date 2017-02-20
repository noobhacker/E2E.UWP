using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.Objects
{
    public class LookingDirectionObject
    {
        public double XPercent { get; set; }
        public double YPercent { get; set; }
        public bool IsLookingTop { get; set; }
        public bool IsLookingBottom { get; set; }
        public bool IsLookingLeft { get; set; }
        public bool IsLookingRight { get; set; }
        public bool IsLookingCenter { get; set; }

        public LookingDirectionObject()
        {
            IsLookingBottom = false;
            IsLookingTop = false;
            IsLookingLeft = false;
            IsLookingRight = false;
            IsLookingCenter = false;
        }
    }
}
