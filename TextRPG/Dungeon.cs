using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class Dungeon
    {
        private int failValue = 20;
        private int defaultReward;
        public DUNGEONTYPE Type { get; set; }
        public int DefaultDefense { get; set; }


        public Dungeon(DUNGEONTYPE type)
        {
            Type = type;

            switch (type)
            {
                case DUNGEONTYPE.EASY:
                    DefaultDefense = 5;
                    defaultReward = 1000;
                    break;
                case DUNGEONTYPE.NORMAL:
                    DefaultDefense = 11;
                    defaultReward = 1700;
                    break;
                case DUNGEONTYPE.HARD:
                    DefaultDefense = 17;
                    defaultReward = 2500;
                    break;
                default:
                    break;
            }


        }

        public bool Enter(Player player)
        {
            double playerDefense = player.Status.Defense;


            //클리어 못했을 때
            if (IsDefenseFail(playerDefense))
            {
                player.Status.Health /= 2;
                return false;
            }

            //클리어 했을 때

            player.Status.DunClear();

            int damage = CalculateDamage(playerDefense);
            ApplyDamage(player, damage);

            int reward = CalculateReward((int)player.Status.Attack);
            player.Status.Gold += reward;

            return true;
        }

 
        private bool IsDefenseFail(double playerDefense)
        {
            return playerDefense < DefaultDefense && new Random().Next(0, 100) < failValue;
        }

        private int CalculateDamage(double playerDefense)
        {
            int baseDamage = new Random().Next(20, 36);
            int diff = Math.Abs((int)DefaultDefense - (int)playerDefense);

            int damage = playerDefense >= DefaultDefense ? baseDamage - diff : baseDamage + diff;
            return Math.Max(0, damage);

        }
        private void ApplyDamage(Player player, int damage)
        {
            player.Status.Health = Math.Max(1, player.Status.Health - damage);
        }

        private int CalculateReward(int attack)
        {
            Random rand = new Random();
            int bonusPercent = rand.Next(attack, attack * 2);
            return defaultReward + (defaultReward * bonusPercent / 100);
        }





    }
}
