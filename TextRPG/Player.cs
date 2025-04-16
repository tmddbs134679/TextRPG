using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TextRPG
{
    
    internal class Player
    {
        public PlayerStatus Status { get; set; }
        public Inventory Inventory { get; set; }
      
        public Player()
        {
            Status = new PlayerStatus(this);
            Inventory = new Inventory();
        }
    }
}
