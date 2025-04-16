using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public enum JOBTYPE
    {
        NONE,
        WARRIOR,
        ARCHOR,
        MAGICIAN,
    }

    public enum ITEMTYPE
    {
        NONE,
        WEAPON,
        ARMOR,
        ACCESSORY,
        POTION,
        KEY,
        GOLD
    }

    public enum PAGE
    {
        NONE,
        VILLAGE,
        STATUS,
        INVENTORY,
        EQUIP,
        SHOP,
        SHOPMENU,
        SELLITEM,
        REST,
        DUNGEON,
        EXITGAME

    }
    public enum MONSTGERTYPE
    {
        NONE,
        SLIME,
        GOBLIN,
        ORC,
        DRAGON,
    }

    public enum DUNGEONTYPE
    {
        NONE,
        EASY,
        NORMAL,
        HARD
    }

}
