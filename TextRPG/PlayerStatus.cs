using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class PlayerStatus
    {
        [JsonIgnore]
        public Player owner { get; private set; }

        public int Level { get; set; } = 1;

        public int ClearCount { get; private set; } = 0;
        public JOBTYPE job { get; set; }
        public string Name { get; set; }
        public int Health { get; set; } = 100;
        public int Gold { get; set; } = 1500;

        public double BaseAttack { get; set; } = 10;
        public double BaseDefense { get; set; } = 5;



        public double Attack => BaseAttack + owner.Inventory.GetEquippedItems()
         .Where(i => i.IsEquipped && i.Type == ITEMTYPE.WEAPON)
         .Sum(i => i.Power);

        public double Defense => BaseDefense + owner.Inventory.GetEquippedItems()
            .Where(i => i.IsEquipped && (i.Type == ITEMTYPE.ARMOR || i.Type == ITEMTYPE.ACCESSORY || i.Type ==  ITEMTYPE.WEAPON))
            .Sum(i => i.Defense);


        public PlayerStatus() { }
        public PlayerStatus(Player player)
        {
            owner = player;
        }

        public void SetOwner(Player player)
        {
            owner = player;
        }

        public void DunClear()
        {
            ClearCount++;
            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            if (ClearCount >= Level)
            {
                ClearCount -= Level;
                Level++;

                BaseAttack += 0.5;   
                BaseDefense += 1;    

            }
        }
    }
}
