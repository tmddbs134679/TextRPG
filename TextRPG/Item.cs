using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class Item
    {
        public string Name { get; set; }
        public ITEMTYPE Type { get; set; }
        public int Power { get; set; }

        public int Defense { get; set; }
        public int PurchasePrice  { get; set; }
        public int SellPrice { get; set; }
        public string Description { get; set; }
        public bool IsEquipped { get; set; } = false;

        public bool IsEquipable => Type == ITEMTYPE.WEAPON || Type == ITEMTYPE.ARMOR;
        public bool IsConsumable => Type == ITEMTYPE.POTION;

        public Item(string name, ITEMTYPE type, int power, int defense, int purchaseprice, string description)
        {
            Name = name;
            Type = type;
            Power = power;
            Defense = defense;
            PurchasePrice = purchaseprice;
            SellPrice = (int)((int)PurchasePrice * 0.7);
            Description = description;
        }
    }
}
