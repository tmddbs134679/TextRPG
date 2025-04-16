

using System.Numerics;
using TextRPG;

internal class Program
{
    static PAGE page = PAGE.NONE;
    static Store store = new Store();

    static void Main(string[] args)
    {
        ShowBanner();

        Player player = GameStart(); 
        ShowVillageMenu(player);
    }

   

    private static Player GameStart()
    {
        if (SaveManager.Exists())
        {
            return SaveManager.Load();
        }
        else
        {
            return Init();
        }
    }

    private static Player Init()
    {
        Player player = new Player();
        Console.WriteLine("스파르타 던전에 오신걸 환영합니다");
        Console.WriteLine("원하시는 이름을 설정해주세요");
        Console.Write("이름 : ");

        player.Status.Name = Console.ReadLine();

        int jobInput = 0;

        while (true)
        {

            Console.WriteLine("직업을 선택해주세요(1 ~ 3)");
            Console.WriteLine("1 : 전사");
            Console.WriteLine("2 : 궁수");
            Console.WriteLine("3 : 마법사");
            Console.Write("선택: ");

           
            if (int.TryParse(Console.ReadLine(), out jobInput) && jobInput >= 1 && jobInput <= 3)
            {
                break; 
            }

            Console.WriteLine("잘못된 입력입니다. 1~3 중에서 선택해주세요.");
        }

        player.Status.job = (JOBTYPE)jobInput;

        return player;
    }

    private static void ShowVillageMenu(Player player)
    {
        page = PAGE.VILLAGE;

        bool inVillage = true;

        while (true)
        {
            Console.Clear();

            string input = Choise();

            switch (input)
            {
                case "1":
                    page = PAGE.STATUS;
                    ShowStatus(player);
                    break;
                case "2":
                    page = PAGE.INVENTORY;
                    ShowInventory(player);
                    break;
                case "3":
                    page = PAGE.SHOP;
                    ShowShop(player);
                    break;
                case "4":
                    page = PAGE.DUNGEON;
                    EnterDungeon(player);
                    break;
                case "5":
                    page = PAGE.REST;
                    ShowRest(player);
                    break;
                case "0":
                    page = PAGE.EXITGAME;
                    ShowExit(player);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 아무키나 눌러주세요.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void EnterDungeon(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 잇습니다.");

            string input = Choise();

            if (!int.TryParse(input, out int check) || check < 0 || check > 3)
            {
                Console.WriteLine("잘못된 입력입니다. 숫자를 정확히 입력해주세요.");
                Console.ReadKey(); 
                continue;
            }


            if (check >= (int)DUNGEONTYPE.EASY && check <= (int)DUNGEONTYPE.HARD)
            {
                page = PAGE.NONE;

                int prevPlayerHealth = player.Status.Health;
                int prevPlayerGold = player.Status.Gold;

                DUNGEONTYPE selectedType = (DUNGEONTYPE)check;
                Dungeon dungeon = new Dungeon(selectedType);
                bool isClear = dungeon.Enter(player);

                ShowGameResult(isClear, player, prevPlayerHealth, prevPlayerGold, (DUNGEONTYPE)check);
                

            }
            else if(check == 0)
            {
                page = PAGE.VILLAGE;
                return;
            }
  
        }


    }

    private static void ShowGameResult(bool isClear, Player player,int prevHealth,int prevGold, DUNGEONTYPE type)
    {
        while(true)
        {
            if (isClear)
            {
                Console.Clear();
                Console.WriteLine("축하합니다!!");
                Console.WriteLine($"{type} 던전을 클리어 하였습니다.");
                Console.WriteLine();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("이런 ㅠㅠ");
                Console.WriteLine($"{type} 던전 클리어 하지 못하셨습니다.");
                Console.WriteLine();
            }


            Console.WriteLine("탐험 결과");
            Console.WriteLine($"체력 {prevHealth} -> {player.Status.Health}");
            Console.WriteLine($"Gold {prevGold} -> {player.Status.Gold}");

            Console.ReadKey();
            page = PAGE.DUNGEON;
            break;

        }
  
    }

    private static void ShowRest(Player player)
    {
        const int restPrice = 500;

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"500 G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Status.Gold} G)");
            string input = Choise();

            if (input == "1")
            {
                if (player.Status.Gold < restPrice)
                {
                    Console.WriteLine("골드가 부족합니다!");
                }
                else
                {
                    player.Status.Gold -= restPrice;
                    player.Status.Health = 100;

                    Console.Clear(); 
                    Console.WriteLine($"500 G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Status.Gold} G)");
                    Console.WriteLine();
                    Console.WriteLine("체력이 모두 회복되었습니다!");
                }
                Console.WriteLine("계속하려면 아무 키나 누르세요.");
                Console.ReadKey();

                page = PAGE.VILLAGE;
                break;
            }
            else if (input == "0")
            {
                page = PAGE.VILLAGE;
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
            }
        }

    }

    private static void ShowShop(Player player)
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine("===상점===");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Status.Gold}");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");


            foreach (var item in store.GetItems())
            {
                Console.WriteLine($" {item.Name} [{item.Type}] [구매가격 : {item.PurchasePrice} ] [판매가격 : {item.SellPrice} ] -> {item.Description}");
            }
            string check = Choise();

            if (check == "1")
            {
                page = PAGE.SHOPMENU;
                ShowShopMenu(player);
            }
            else if (check == "2")
            {
                page = PAGE.SELLITEM;
                ShowSellItem(player);
            }
            else if (check == "0")
            {
                page = PAGE.VILLAGE;
                break;
            }
        }
       


    }

    private static void ShowSellItem(Player player)
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine("===상점===");
            Console.WriteLine("아이템들을 팔 수 있습니다.");
            Console.WriteLine();

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Status.Gold}");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");

            int idx = 1;


            List<Item> sellableItems = player.Inventory.GetItems();

            //판매 가능 아이템이 없을 때
            if (sellableItems.Count == 0)
            {
                Console.WriteLine("판매 가능한 아이템이 없습니다.");
                Console.ReadKey();
                page = PAGE.SHOP;
                break;
            }
            else
            {
                //판매 가능 아이템이 있을 때
                foreach (var item in sellableItems)
                {
                    Console.WriteLine($" {idx++} / {item.Name} [{item.Type}] [구매가격 : {item.PurchasePrice} ] [판매가격 : {item.SellPrice} ] -> {item.Description}");
                }

                string input = Choise();

                if(!int.TryParse(input, out int check) || check < 0 || check > sellableItems.Count)
                {
                    Console.WriteLine("잘못된 번호입니다. 판매를 취소합니다.");
                    Console.WriteLine("아무키나 누르세요.");
                    Console.ReadKey();
                    page = PAGE.SHOP;
                    break;
                }
                else if(input == "0")
                {
                    page = PAGE.SHOP;
                    break;
                }

                Item selectedIterm = sellableItems[check - 1];
                player.Inventory.RemoveItem(selectedIterm);
                player.Status.Gold += selectedIterm.SellPrice;
            }
        }
    }

    private static void ShowShopMenu(Player player)
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine("===상점===");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Status.Gold}");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");

            //아이템 구매 누를때만 idx나오게
            int idx = 1;

            foreach (var item in store.GetItems())
            {
                Console.WriteLine($"{idx++} / {item.Name} [{item.Type}] [구매가격 : {item.PurchasePrice} ] [판매가격 : {item.SellPrice} ] -> {item.Description}");
            }


            //구매할 아이템 번호 선택
            string check = Choise();

            //종료
            if (check == "0")
            {
                page = PAGE.SHOP;
                break;
            }
               

            if (!int.TryParse(check, out int itemNum) || itemNum < 1 || itemNum > store.GetItems().Count)
            {
                Console.WriteLine("잘못된 입력입니다. 숫자를 정확히 입력해주세요.");
                Console.ReadKey(); // 입력 대기 (선택)
                continue; 
            }

            Item choiceItem = store.GetItems()[itemNum - 1];

            if (player.Status.Gold < choiceItem.PurchasePrice)
            {
                //골드가 부족함.
                Console.WriteLine("골드가 부족합니다. 아무키나 누르세요.");
                Console.ReadKey();
            }
            else if (player.Inventory.GetItems().Any(item => item.Name == choiceItem.Name))
            {
                //이미 구매한 아이템
                Console.WriteLine("이미 구매한 아이템입니다. 아무키나 누르세요. ");
                Console.ReadKey();
            }
            else
            {
                player.Status.Gold -= choiceItem.PurchasePrice;
                player.Inventory.AddItem(choiceItem, choiceItem.Type);

            }
        }
       
    }

    private static void ShowInventory(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===인벤토리===");

            if (player.Inventory.Count == 0)
            {
                Console.WriteLine("인벤토리에 아이템이 없습니다.");
            }
            else
            {
                int idx = 1;

                foreach (var item in player.Inventory.GetItems())
                {
                    Console.WriteLine($"{idx++} / {item.Name} [{item.Type}] [구매가격 : {item.PurchasePrice} ] [판매가격 : {item.SellPrice} ] -> {item.Description}");
                }
            }

            string input = Choise();

            if (input == "0")
            {
                page = PAGE.VILLAGE;
                break;
            }
            else if (input == "1")
            {
                page = PAGE.EQUIP;
                ShowEquipMenu(player);
            }
        }

    }

    private static void ShowEquipMenu(Player player)
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine("===인벤토리===");

            if (player.Inventory.Count == 0)
            {
                Console.WriteLine("인벤토리에 아이템이 없습니다.");
            }
            else
            {
                int idx = 1;

                foreach (var item in player.Inventory.GetItems())
                {
                    string equipMark = item.IsEquipped ? "[E] " : "";
                    Console.WriteLine($"{idx++} / {equipMark}{item.Name} [{item.Type}] [구매가격 : {item.PurchasePrice} ] [판매가격 : {item.SellPrice} ] -> {item.Description}");
                }
            }

            string check = Choise();


            if (check == "0")
            {
                page = PAGE.INVENTORY;
                break;
            }
            else
            {

                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    string item = (i + 1).ToString();

                    //입력 Index Check
                    if (item == check)
                    {
                        //낄수있는 장비인지 Check
                        if (player.Inventory.GetItems()[i].IsEquipable)
                        {
                            player.Inventory.EquipItem(player.Inventory.GetItems()[i]);
                            break;
                        }

                    }

                }
            }
        }

    }

    private static void ShowStatus(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"이름 : {player.Status.Name}");
            Console.WriteLine($"직업 : {player.Status.job}");
            Console.WriteLine($"레벨 : {player.Status.Level}");
            Console.WriteLine($"공격력 : {player.Status.Attack}");
            Console.WriteLine($"방어력 : {player.Status.Defense}");
            Console.WriteLine($"체력 : {player.Status.Health}");
            Console.WriteLine($"Gold : {player.Status.Gold}");

            string input = Choise();

            if (input == "0")
            {
                page = PAGE.VILLAGE;
                break;
            }
        }
    }

    private static void ShowExit(Player player)
    {
        Console.Clear();
        string input = Choise();

        if (input == "1")
        {
            Environment.Exit(0);
        }
        else if (input == "2")
        {
            SaveManager.Save(player);
            ShowVillageMenu(player);
        }
        else if (input == "3")
        {
            //데이터 초기화
            SaveManager.Reset();

            Console.Clear();
            ShowVillageMenu(Init());
        }
        else if (input == "0")
        {
            ShowVillageMenu(player);
        }
        
    }
    private static string Choise()
    {
      
        Console.WriteLine();

        if(page == PAGE.VILLAGE)
        {
            Console.Clear();
            Console.WriteLine("=======================================");
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("0. 게임 종료, 저장하기");
            Console.WriteLine("=======================================");
            Console.Write(">> ");

            return Console.ReadLine();

        }
        else if (page == PAGE.INVENTORY)
        {

            Console.WriteLine();
            Console.WriteLine("1 : 장착관리");
        }
        else if (page == PAGE.EQUIP)
        {
            Console.WriteLine("장착 되어있으면 [E] 표시, 장착 불가 아이템은 작동 안됨");
            Console.WriteLine("Index를 입력하여 장착 및 해제하세요.");
            Console.WriteLine();
        }
        else if (page == PAGE.SHOP)
        {
            Console.WriteLine("1 : 아이템 구매하기");
            Console.WriteLine("2 : 아이템 판매하기");
        }
        else if(page == PAGE.SHOPMENU)
        {
            Console.WriteLine("Index를 입력하여 Item을 구매하세요.");
        }
        else if (page == PAGE.SELLITEM)
        {
            Console.WriteLine("Index를 입력하여 아이템을 판매하세요.");
        }
        else if (page == PAGE.DUNGEON)
        {
            Console.WriteLine("1. 쉬운 던전   |   방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전   |   방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전   |   방어력 17 이상 권장");
            Console.WriteLine();
        }
        else if(page ==PAGE.REST)
        {
            Console.WriteLine("1. 휴식하기");
        }
        else if(page == PAGE.EXITGAME)
        {
            Console.WriteLine("1. 게임 종료하기");
            Console.WriteLine("2. 게임 저장하기");
            Console.WriteLine("3. 데이터 초기화");
        }
  
        
        Console.WriteLine("0 : 뒤로가기");

       

        Console.Write("다음 행동을 입력하세요: ");
 

        return Console.ReadLine();
    }

    private static void ShowBanner()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;


        string ascii =
@"##################################################
##################################################
######:~~*#$~~~;##--=##;~~~~~$;~~~~~:#:~##########
####*.=##~$= =#* $$-*###,##-.~~=$.!#-#;.:#########
####=.!=###= =#! $$, ###,##= :=#$ !###---$########
#####!-.:=#=.~:~~#;=,###,$;,;=##$ *##*;$.*########
######$=;,#=.!==$@.,.,##,#; ####$.*##;,,.!########
########*,$= =###,##~.##,##~ $##$ !#=*##!!########
####*:!!:-#! ;##=.*#*.!!.*#;-,=#! :#;~$#:-!#######
####$****$#**!##=!*#****!*##$!=#!!!#!!$#!**#######
###,*=-@#~.###,@: ##~.#;~!:.#.:!!~#@~@-#*.$##~####
### @##.!~.###.#:.=#-#=*###:# ;#$;$~##==#~ =#-####
###.@## ~~.###.#::,*-#*!#$!!# -;!#~$#!.;*;,.*-####
###.@##.~~,###.#~$!,.#*!##;.# :==$-#::=-~*=.,.####
###.###,$:,###,#:$#!.#*-$#!.# ;#$;$.-##.#!#=. ####
##=.*!~;##~,!:#$-!##~##!.*:,# ~#,:-=!!::$:=#*.####
##*!!!$####!!$#$!!##=###*!**#!!:,*$#!!$#$!=##*####
##############################!.$#################
###########################$=.!###################
##########################*,:*####################
#########################$,;######################
###################!*###,,##,#####################
####################;#;-!#$-~:$###################
####################-#:=#$~:#*;###################
##################,!$-####=###$###################
################$-=:##~##@=:.!~=##################
###############!:$$~*###!@!*.;~=!#################
###############!##=.*###,#!*.**~.~=###############
###############*!#=..=.!# :##~- ##################
###################*=#. !$~- .;##=,###############
######################- *##!  ;##=,:##############
######################~~*##~= ;##=, ##############
###################### ;$##~$.;##=,$##############
######################.!###- *=##=,$$#############";

        Console.WriteLine(ascii);
        Console.WriteLine("\n아무키나 누르세요.");
        Console.ReadKey();

    }
}
