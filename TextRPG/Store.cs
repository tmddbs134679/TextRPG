using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class Store
    {
        private List<Item> items;
        public int count => items.Count;

        public Store()
        {
            items = new List<Item>();

            var sword = new Item("강철검", ITEMTYPE.WEAPON, 10, 0, 100, "기본 철검. 공격력 +10");
            var potion = new Item("회복 포션", ITEMTYPE.POTION, 0, 0, 30, "체력을 50 회복합니다");

            var bow = new Item("양궁 활", ITEMTYPE.WEAPON, 10, 10, 150, "양궁 선수들이 사용하는 활. 공격력 +10, 방어력 +10");
            var wand = new Item("나무 완드 ", ITEMTYPE.WEAPON, 10, 10, 120, "기본 완드. 공격력 +18, 방어력 +5");
            var epSword = new Item("다이아검", ITEMTYPE.WEAPON, 3500, 300, 3000, "다이아로 만든 검. 공격력 +3500, 방어력 +300");

            items.Add(sword);
            items.Add(potion);
            items.Add(bow);
            items.Add(wand);
            items.Add(epSword);

        }


        public List<Item> GetItems()
        {
            return items;
        }

        public void BuyItem(Item item)
        {
            items.Remove(item);
        }
    }
}
