using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.DTOs.BreakWordDto
{
        public class BreakWordObject
        {
            public Candidate[] candidates { get; set; }
        }

        public class Candidate
        {
            public string words { get; set; }
            public float probability { get; set; }
        }
}
