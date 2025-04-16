using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class Inventory
    {
        private List<Item> items;
        public int Count => items.Count;
        public bool IsEmpty => !items.Any();
         
        public Inventory()
        {
            items = new List<Item>();
        }


        public List<Item> GetEquippedItems()
        {
            return items.Where(i => i.IsEquipped).ToList();
        }

        public List<Item> GetItems()
        {
            return items;
        }

        public void AddItem(Item item, ITEMTYPE type)
        {
            item.Type = type;
            items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }

        public void EquipItem(Item itemEquip)
        {
            // 이미 장착 중인 아이템을 다시 선택한 경우 → 해제
            if (itemEquip.IsEquipped)
            {
                itemEquip.IsEquipped = false;
                return;
            }  

            foreach (var item in items)
            {
                if(item.Type == itemEquip.Type && item.IsEquipped)
                {
                    item.IsEquipped = false;
                    break;
                }
            }

            itemEquip.IsEquipped = true;

        }

    }
}
