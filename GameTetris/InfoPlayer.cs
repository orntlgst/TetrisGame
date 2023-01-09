using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace GameTetris
{
    public class InfoPlayer
    {
        public string name { get; set; }
        public string score { get; set; }

        public InfoPlayer(string name, string score)
        {
            this.name = name;
            this.score = score;
        }
    }
}
