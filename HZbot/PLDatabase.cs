using OfficeOpenXml;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZbot
{
    class PLDatabase
    {
        private static FileInfo databaseFile;
        private static Math_statsFunc HP = new Math_statsFunc();
        private static Math_statsFunc STR = new Math_statsFunc();
        private static Math_statsFunc DEF = new Math_statsFunc();
        private static Math_EXPFunc EXP = new Math_EXPFunc();

        public static String Create(DirectoryInfo outputDir)
        {
            //Create EXSL file
            databaseFile = new FileInfo(outputDir.FullName + @"\PLDB.xlsx");
            if (!databaseFile.Exists)
            {
                databaseFile.Delete();
                databaseFile = new FileInfo(outputDir.FullName + @"\PLDB.xlsx");
            }
            //Edit EXSL file
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                //add Worksheet
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets.Add("PlayerDatabase");

                //Add the headers
                worksheet1.Cells[1, 1].Value = "PL_ID";
                worksheet1.Cells[1, 2].Value = "PL_NAME";
                worksheet1.Cells[1, 3].Value = "PL_ROLE";
                worksheet1.Cells[1, 4].Value = "EXP";
                worksheet1.Cells[1, 5].Value = "LVL";
                worksheet1.Cells[1, 6].Value = "MONEY";
                worksheet1.Cells[1, 7].Value = "HP";
                worksheet1.Cells[1, 8].Value = "STR";
                worksheet1.Cells[1, 9].Value = "DEF";
                worksheet1.Cells[1, 10].Value = "HEAD";
                worksheet1.Cells[1, 11].Value = "RHAND";
                worksheet1.Cells[1, 12].Value = "LHAND";
                worksheet1.Cells[1, 13].Value = "CHEST";
                worksheet1.Cells[1, 14].Value = "LEGS";
                worksheet1.Cells[1, 15].Value = "BOOTS";
                worksheet1.Cells[1, 16].Value = "ACCESS";
                //INVENTORY 1-5
                worksheet1.Cells[1, 17].Value = "INV1";
                worksheet1.Cells[1, 18].Value = "INV2";
                worksheet1.Cells[1, 19].Value = "INV3";
                worksheet1.Cells[1, 20].Value = "INV4";
                worksheet1.Cells[1, 21].Value = "INV5";
                //INVENTORY 6-10
                worksheet1.Cells[1, 22].Value = "INV6";
                worksheet1.Cells[1, 23].Value = "INV7";
                worksheet1.Cells[1, 24].Value = "INV8";
                worksheet1.Cells[1, 25].Value = "INV9";
                worksheet1.Cells[1, 26].Value = "INV10";
                //INVENTORY 11-15
                worksheet1.Cells[1, 27].Value = "INV11";
                worksheet1.Cells[1, 28].Value = "INV12";
                worksheet1.Cells[1, 29].Value = "INV13";
                worksheet1.Cells[1, 30].Value = "INV14";
                worksheet1.Cells[1, 31].Value = "INV15";
                //INVENTORY 16-20
                worksheet1.Cells[1, 32].Value = "INV16";
                worksheet1.Cells[1, 33].Value = "INV17";
                worksheet1.Cells[1, 34].Value = "INV18";
                worksheet1.Cells[1, 35].Value = "INV19";
                worksheet1.Cells[1, 36].Value = "INV20";

                //TRADE
                worksheet1.Cells[1, 37].Value = "TRADING_WITH";

                // set some document properties
                package.Workbook.Properties.Title = "PlayerDatabase";
                package.Workbook.Properties.Author = "MateusBranco";
                package.Workbook.Properties.Comments = "HZ-RPG Player's Database";

                //add Worksheet
                ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add("ItemDatabase");

                //Add the headers
                worksheet2.Cells[1, 1].Value = "ITEM_ID"; //ID
                worksheet2.Cells[1, 2].Value = "ITEM_DESC"; //Desctiption
                worksheet2.Cells[1, 3].Value = "ITEM_LVL"; //REQUIRED PLAYER LEVEL
                worksheet2.Cells[1, 4].Value = "ITEM_ATB"; //special atributes|
                worksheet2.Cells[1, 5].Value = "ITEM_DEF"; //DEFENSE| x~y -> math.random(x,y) {requires Parsing} //DEF = 10~15    
                worksheet2.Cells[1, 6].Value = "ITEM_DMG"; //DAMAGE| x~y -> math.random(x,y) [+/-] z*math.random(1,w) {requires Parsing} //DMG = 10~15#+-1D3
                worksheet2.Cells[1, 7].Value = "ITEM_ACC"; //ACCURACY| x~y -> math.random(x,y) {requires Parsing} || if 1D100 < ACC dealt dmg //ACC = 1~100

                //ITEMS DATA
                lastItemRow ItemRow = new lastItemRow();
                ItemRow.last = 2;
                {

                    //Hand
                    {

                        //Fist
                        worksheet2.Cells[ItemRow.last, 1].Value = "HA#BareFist"; //ITEM_ID
                        worksheet2.Cells[ItemRow.last, 2].Value = "Your Fist is like heyy that's pretty gud";
                        worksheet2.Cells[ItemRow.last, 3].Value = "1"; //ITEM_LVL
                        worksheet2.Cells[ItemRow.last, 4].Value = "0";
                        worksheet2.Cells[ItemRow.last, 5].Value = "0";
                        worksheet2.Cells[ItemRow.last, 6].Value = "10~10~.1D2"; //ITEM_DMG
                        worksheet2.Cells[ItemRow.last, 7].Value = "30~30"; //ITEM_ACC

                        ItemRow.add1last();

                        //Stone
                        worksheet2.Cells[ItemRow.last, 1].Value = "HA#Stone"; //ITEM_ID
                        worksheet2.Cells[ItemRow.last, 2].Value = "ITEM_DESC";
                        worksheet2.Cells[ItemRow.last, 3].Value = "1"; //ITEM_LVL
                        worksheet2.Cells[ItemRow.last, 4].Value = "0";
                        worksheet2.Cells[ItemRow.last, 5].Value = "0";
                        worksheet2.Cells[ItemRow.last, 6].Value = "15~50~+1D3"; //ITEM_DMG
                        worksheet2.Cells[ItemRow.last, 7].Value = "45~50"; //ITEM_ACC

                        ItemRow.add1last();

                        //Sick
                        worksheet2.Cells[ItemRow.last, 1].Value = "HA#Stick"; //ITEM_ID
                        worksheet2.Cells[ItemRow.last, 2].Value = "ITEM_DESC";
                        worksheet2.Cells[ItemRow.last, 3].Value = "1"; //ITEM_LVL
                        worksheet2.Cells[ItemRow.last, 4].Value = "0";
                        worksheet2.Cells[ItemRow.last, 5].Value = "0";
                        worksheet2.Cells[ItemRow.last, 6].Value = "1~5~+1D3"; //ITEM_DMG 
                        worksheet2.Cells[ItemRow.last, 7].Value = "30~60"; //ITEM_ACC
                        
                        ItemRow.add1last();

                    }
                    //Head
                    {
                        //Bucket
                        worksheet2.Cells[ItemRow.last, 1].Value = "HE#Bucket"; //ITEM_ID
                        worksheet2.Cells[ItemRow.last, 2].Value = "ITEM_DESC";
                        worksheet2.Cells[ItemRow.last, 3].Value = "1"; //ITEM_LVL
                        worksheet2.Cells[ItemRow.last, 4].Value = "0";
                        worksheet2.Cells[ItemRow.last, 5].Value = "10~15"; //ITEM_DEF
                        worksheet2.Cells[ItemRow.last, 6].Value = "0";
                        worksheet2.Cells[ItemRow.last, 7].Value = "0";

                        ItemRow.add1last();

                    }
                    //Chest
                    {
                        //Bucket
                        /*

                        ItemRow.add1last();*/

                    }
                    //Legs
                    {
                        //LEAF
                        worksheet2.Cells[ItemRow.last, 1].Value = "LE#Leaf"; //ITEM_ID
                        worksheet2.Cells[ItemRow.last, 2].Value = "ITEM_DESC";
                        worksheet2.Cells[ItemRow.last, 3].Value = "1"; //ITEM_LVL
                        worksheet2.Cells[ItemRow.last, 4].Value = "0";
                        worksheet2.Cells[ItemRow.last, 5].Value = "5~5"; //ITEM_DEF
                        worksheet2.Cells[ItemRow.last, 6].Value = "0";
                        worksheet2.Cells[ItemRow.last, 7].Value = "0";

                        ItemRow.add1last();

                    }
                    //Boots
                    {
                        //Bucket
                        /*

                        ItemRow.add1last();*/

                    }
                    //Accesories
                    {
                        //Bucket
                        /*

                        ItemRow.add1last();*/

                    }
                }

                // set some document properties
                package.Workbook.Properties.Title = "ItemDatabase";
                package.Workbook.Properties.Author = "MateusBranco";
                package.Workbook.Properties.Comments = "HZ-RPG Item's Database";

                /*ExcelWorksheet worksheet3 = package.Workbook.Worksheets.Add("ItemInstances");

                //Add the headers
                worksheet3.Cells[1, 1].Value = "INSTANCE_ID";
                worksheet3.Cells[1, 2].Value = "INSTANCE_NAME";
                // Adjective based of based of DMG && DEF && DUR && ACC stats 
                // "ACC|DMG|DUR|DEF|"
                // |Small,Normal,Big,Giant,Massive,Monolithic|Dull,Polished,Edgy,Sharp,Exquisite|Brittle,Fragile,Week,Firm,Sturdy,Solid,Unbreakable|Light,Heavy|
                worksheet3.Cells[1, 3].Value = "INSTANCE_ADJ";
                worksheet3.Cells[1, 4].Value = "INSTANCE_TITLE"; //FAME TITLE
                worksheet2.Cells[1, 5].Value = "ITEM_DESC";
                worksheet3.Cells[1, 6].Value = "INSTANCE_LVL"; //REQUIRED PLAYER LEVEL
                worksheet3.Cells[1, 7].Value = "INSTANCE_ATB"; //special atributes|
                worksheet3.Cells[1, 8].Value = "INSTANCE_DEF"; //DEFENSE| x~y -> math.random(x,y) {requires Parsing} //DEF = 10~15    
                worksheet3.Cells[1, 9].Value = "INSTANCE_DMG"; //DAMAGE| x~y -> math.random(x,y) [+/-] z*math.random(1,w) {requires Parsing} //DMG = 10~15#+-1D3
                worksheet3.Cells[1, 10].Value = "INSTANCE_ACC"; //ACCURACY| x~y -> math.random(x,y) {requires Parsing} || if 1D100 < ACC dealt dmg //ACC = 1~100
                worksheet3.Cells[1, 11].Value = "INSTANCE_OWNER_ID";

                // set some document properties
                package.Workbook.Properties.Title = "ItemInstances";
                package.Workbook.Properties.Author = "MateusBranco";
                package.Workbook.Properties.Comments = "HZ-RPG ItemInstances's Database";*/

                package.Save();
            }
            return databaseFile.FullName;
        }

        //Static LastRow
        struct lastItemRow { public int last; public int add1last() { last++; return last; } };

        static public void addPlayer(string PL_NAME, string PL_ROLE, string PL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                //SEARCH LAST INDEX NOT NULL
                int maxrow = 1;
                while (worksheet1.Cells[maxrow, 1].Value != null)
                {
                    maxrow++;
                }

                //string PL_IDparse = maxrow.ToString() + "#" + PL_ID;

                //Initial Values
                worksheet1.Cells[maxrow, 1].Value = PL_ID;
                worksheet1.Cells[maxrow, 2].Value = PL_NAME;
                worksheet1.Cells[maxrow, 3].Value = PL_ROLE;
                worksheet1.Cells[maxrow, 4].Value = 1000;//"EXP";
                worksheet1.Cells[maxrow, 5].Value = 0;//"LVL";
                worksheet1.Cells[maxrow, 6].Value = 500;//"MONEY";
                package.Save();

                //Init Inventory & Equipment
                for(int i = 10; i<=36; i++)
                {
                    worksheet1.Cells[maxrow, i].Value = "_e#_empty_#0#0#0#0#0#0##";
                }

                //Trading
                worksheet1.Cells[maxrow, 37].Value = "N/A";

                switch (PL_ROLE)
                {
                    case "HUMAN":
                        //init Functions
                        HP.y(0, 900, 5);
                        STR.y(0, 50, 5);
                        DEF.y(0, 70, 5);

                        worksheet1.Cells[maxrow, 7].Value = HP.val;//"HP";
                        worksheet1.Cells[maxrow, 8].Value = STR.val;//"STR";
                        worksheet1.Cells[maxrow, 9].Value = DEF.val;//"DEF";

                        break;
                    case "ZOMBIE":
                        //init Functions
                        HP.y(0, 1200, 5);
                        STR.y(0, 40, 5);
                        DEF.y(0, 50, 5);

                        worksheet1.Cells[maxrow, 7].Value = HP.val;//"HP";
                        worksheet1.Cells[maxrow, 8].Value = STR.val;//"STR";
                        worksheet1.Cells[maxrow, 9].Value = DEF.val;//"DEF";

                        break;
                } 
                package.Save();
                InstantiateItemXtoPL_ID("LE#Leaf", PL_ID);
                InstantiateItemXtoPL_ID("HA#BareFist", PL_ID);
                InstantiateItemXtoPL_ID("HA#BareFist", PL_ID);
                Console.WriteLine($"{PL_ID} {worksheet1.Cells[maxrow, 2].Value} Added with Success to PLAYER.DATABASE!");
            }
        }

        static public int getItemRowByID(string Item_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet2 = package.Workbook.Worksheets[2];
                int IdRow = 1;
                while (worksheet2.Cells[IdRow, 1].Value.ToString() != Item_ID)
                {
                    IdRow++;
                }
                return IdRow;
            }
        }

        static public int getPLRowByID(string PL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                int IdRow = 1;
                while (worksheet1.Cells[IdRow, 1].Value.ToString() != PL_ID)
                {
                    IdRow++;
                }
                return IdRow;
            }
        }

        static public string[] getStatsOf(string PL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                //SEARCH PL
                int IdRow = getPLRowByID(PL_ID);
                string[] PlStats = new string[4];
                int j = 0;
                for (int i = 6; i <= 9; i++)
                {
                    PlStats[j] = worksheet1.Cells[IdRow, i].Value.ToString();
                    j++;
                }
                return PlStats;
            }
        }


        static public string[] getInventoryOf(string PL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                //SEARCH PL
                int IdRow = getPLRowByID(PL_ID);
                string[] inventory = new string[20];
                int j = 0;
                for (int i = 17; i <= 36; i++)
                { 
                    inventory[j] = worksheet1.Cells[IdRow, i].Value.ToString();
                    j++;
                }
                return inventory;
            }
        }

        static public string[] getEquipmentOf(string PL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                //SEARCH PL
                int IdRow = getPLRowByID(PL_ID);
                string[] equip = new string[7];
                int j = 0;
                for (int i = 10; i <= 16; i++)
                {
                    equip[j] = worksheet1.Cells[IdRow, i].Value.ToString().Split('#')[1];
                    j++;
                }
                return equip;
            }
        }

        static public string GenerateItem(string ITEM_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet2 = package.Workbook.Worksheets[2];
                int ItemIdRow = getItemRowByID(ITEM_ID);
                string GeneratedItem = "";            
                GeneratedItem += worksheet2.Cells[ItemIdRow, 1].Value.ToString() + '#';//Type[0]&NAME[1]
                GeneratedItem += worksheet2.Cells[ItemIdRow, 2].Value.ToString() + '#';//Description
                GeneratedItem += worksheet2.Cells[ItemIdRow, 3].Value.ToString() + '#';//LVL
                GeneratedItem += worksheet2.Cells[ItemIdRow, 4].Value.ToString() + '#';//SpecialATRIBUTES
                GeneratedItem += worksheet2.Cells[ItemIdRow, 5].Value.ToString() + '#';//DEF
                GeneratedItem += worksheet2.Cells[ItemIdRow, 6].Value.ToString() + '#';//DMG
                GeneratedItem += worksheet2.Cells[ItemIdRow, 7].Value.ToString() + '#';//ACCuracy
                //Console.WriteLine(GeneratedItem); //LE#Leaf#ITEM_DESC#1#0#5~5#0#0#

                //PARSING VALUES
                string[] AuxItem = GeneratedItem.Split('#');
                Random Rand = new Random();
                if (AuxItem[5].Length > 1)
                {
                    //Console.WriteLine(AuxItem[5]);
                    int minDEF = int.Parse(AuxItem[5].Split('~')[0]);
                    int maxDEF = int.Parse(AuxItem[5].Split('~')[1]);
                    AuxItem[5] = Rand.Next(minDEF, maxDEF).ToString();//DEF 
                }
                if (AuxItem[6].Length > 1)
                {
                    //Console.WriteLine(AuxItem[6]);
                    int minDMG = int.Parse(AuxItem[6].Split('~')[0]);
                    int maxDMG = int.Parse(AuxItem[6].Split('~')[1]);
                    AuxItem[6] = Rand.Next(minDMG, maxDMG).ToString() + AuxItem[6].Split('~')[2];//DMG
                }
                if (AuxItem[7].Length > 1)
                {
                    //Console.WriteLine(AuxItem[7]);
                    int minACC = int.Parse(AuxItem[7].Split('~')[0]);
                    int maxACC = int.Parse(AuxItem[7].Split('~')[1]);
                    AuxItem[7] = Rand.Next(minACC, maxACC).ToString();//ACC
                }
                //NAME
                string auxname = AuxItem[1];

                string FinalItem = "";
                foreach(string i in AuxItem)
                {
                    FinalItem += i + "#";
                }
                Console.WriteLine(FinalItem);
                return FinalItem;
            }
        }

        static public bool InstantiateItemXtoPL_ID(string ITEM_ID, string PL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                int PLIdRow = getPLRowByID(PL_ID);
                string GeneratedItem = GenerateItem(ITEM_ID);
                ///Console.WriteLine("AAA");
                //SEARCH LAST Inventory INDEX NOT NULL
                int emptyCol = 17;
                while (worksheet1.Cells[PLIdRow, emptyCol].Value.ToString() != "_e#_empty_#0#0#0#0#0#0##" && emptyCol <= 37)
                {
                    emptyCol++;
                }
                if (emptyCol == 37) return false;
                worksheet1.Cells[PLIdRow, emptyCol].Value = GeneratedItem;
                package.Save();
            }
            return true;
        }

        static public bool EquipItemXOf(int num, string PL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                int IdRow = getPLRowByID(PL_ID);
                string Item = worksheet1.Cells[IdRow, 16 + num].Value.ToString();
                string ItemType = Item.Substring(0,2);
                worksheet1.Cells[IdRow, 16 + num].Value = "_e#_empty_#0#0#0#0#0#0##";
                switch (ItemType)
                {
                    case "_e"://EMPTY
                        return false;
                    case "HE"://HEAD
                        if (worksheet1.Cells[IdRow, 9 + 1].Value.ToString() != "_e#_empty_#0#0#0#0#0#0##")
                        {
                            worksheet1.Cells[IdRow, 16 + num].Value = worksheet1.Cells[IdRow, 9 + 1].Value;
                        }
                        else
                        {
                            worksheet1.Cells[IdRow, 9 + 1].Value = Item;
                        }
                        break;
                    case "HA"://HAND
                        if (worksheet1.Cells[IdRow, 9 + 2].Value.ToString() != "_e#_empty_#0#0#0#0#0#0##")//Item In RightHand
                        {
                            if(worksheet1.Cells[IdRow, 9+ 3].Value.ToString() != "_e#_empty_#0#0#0#0#0#0##")//Item In LeftHand
                            {
                                //worksheet1.Cells[IdRow, 16 + num].Value = worksheet1.Cells[IdRow, 9 + 2].Value;
                                return false; //Hands FULL
                            }
                            else//NoItem In LeftHand
                            {
                                worksheet1.Cells[IdRow, 9 + 3].Value = Item;
                            }
                        }
                        else//NoItem In RightHand
                        {
                            worksheet1.Cells[IdRow, 9 + 2].Value = Item;
                        } 
                        break;
                    case "CH"://CHEST
                        if (worksheet1.Cells[IdRow, 9 + 4].Value.ToString() != "_e#_empty_#0#0#0#0#0#0##")
                        {
                            worksheet1.Cells[IdRow, 16 + num].Value = worksheet1.Cells[IdRow, 9 + 4].Value;
                        }
                        else
                        {
                            worksheet1.Cells[IdRow, 9 + 4].Value = Item;
                        }
                        break;
                    case "LE"://LEGS
                        if (worksheet1.Cells[IdRow, 9 + 5].Value.ToString() != "_e#_empty_#0#0#0#0#0#0##")
                        {
                            worksheet1.Cells[IdRow, 16 + num].Value = worksheet1.Cells[IdRow, 9 + 5].Value;
                        }
                        else
                        {
                            worksheet1.Cells[IdRow, 9 + 5].Value = Item;
                        }
                        break;
                    case "BO"://BOOTS
                        if (worksheet1.Cells[IdRow, 9 + 6].Value.ToString() != "_e#_empty_#0#0#0#0#0#0##")
                        {
                            worksheet1.Cells[IdRow, 16 + num].Value = worksheet1.Cells[IdRow, 9 + 6].Value;
                        }
                        else
                        {
                            worksheet1.Cells[IdRow, 9 + 6].Value = Item;
                        }
                        
                        break;
                    case "AC"://ACCESSORIES
                        if (worksheet1.Cells[IdRow, 9 + 7].Value.ToString() != "_e#_empty_#0#0#0#0#0#0##")
                        {
                            worksheet1.Cells[IdRow, 16 + num].Value = worksheet1.Cells[IdRow, 9 + 7].Value;
                        }
                        else
                        {
                            worksheet1.Cells[IdRow, 9 + 7].Value = Item;
                        }
                        break;
                    default:
                        return false;
                }
                package.Save();
            }
            return true;
        }

        static public bool UnEquipItemXOf(int num, string PL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                int IdRow = getPLRowByID(PL_ID);
                int emptyCol = 17;
                while (worksheet1.Cells[IdRow, emptyCol].Value.ToString() != "_e#_empty_#0#0#0#0#0#0##" && emptyCol < 37)
                {
                    emptyCol++;
                    if (emptyCol == 37) return false;
                }
                worksheet1.Cells[IdRow, emptyCol].Value = worksheet1.Cells[IdRow, 9 + num].Value;
                worksheet1.Cells[IdRow,9 + num].Value = "_e#_empty_#0#0#0#0#0#0##";
                package.Save();
            }
            return true;
        }

        static public bool DestroyItemXOf(int num, string PL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                int IdRow = getPLRowByID(PL_ID);
                worksheet1.Cells[IdRow, 16 + num].Value = "_e#_empty_#0#0#0#0#0#0##";
                package.Save();
            }
            return true;
        }

        static public string[] updateEXPtoPlayerByID(string PL_ID, float exp = 0)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                ///LVLnum,PL_LVL,playerEXP,nextEXP,%
                string[] INFO = {"","","","","",""};
                int LevelUpNum = 0;
                int IdRow = getPLRowByID(PL_ID);
                double newexp = (double)worksheet1.Cells[IdRow, 4].Value + exp;
                worksheet1.Cells[IdRow, 4].Value = newexp;

                //InitEXPFunc
                EXP.y(Convert.ToInt32(worksheet1.Cells[IdRow, 5].Value), 100, 8f, 80f, 1.1f);
                while ((double)worksheet1.Cells[IdRow, 4].Value > EXP.val)
                {
                    //LEVEL UP !
                    LevelUpNum++;
                    //Console.WriteLine("LEVEL UP! " + EXP.val);
                    //LevelUp_ByID(PL_ID,IdRow); 
                    newexp -= EXP.y(Convert.ToInt32(worksheet1.Cells[IdRow, 5].Value));
                    //EXP
                    worksheet1.Cells[IdRow, 4].Value = newexp;
                    //LVL
                    worksheet1.Cells[IdRow, 5].Value = Convert.ToInt32(worksheet1.Cells[IdRow, 5].Value) + 1;
                    //ROLE
                    switch (worksheet1.Cells[IdRow, 5].Value)
                    {
                        case 2:
                            if (worksheet1.Cells[IdRow, 3].Value.ToString().Contains("HUMAN")) {
                                INFO[5] = "BIGHUMAN";
                                //worksheet1.Cells[IdRow, 3].Value =  worksheet1.Cells[IdRow, 3].Value.ToString() + ",BIGHUMAN";
                            }
                            else if(worksheet1.Cells[IdRow, 3].Value.ToString().Contains("ZOMBIE")){
                                //worksheet1.Cells[IdRow, 3].Value = worksheet1.Cells[IdRow, 3].Value.ToString() + ",BIGZOMBU";
                                INFO[5] = "BIGZOMBU";
                            }
                            break;
                        default:
                            break;
                    }
                    //STATS
                    worksheet1.Cells[IdRow, 7].Value = HP.y(Convert.ToInt32(worksheet1.Cells[IdRow, 5].Value));//"HP";
                    worksheet1.Cells[IdRow, 8].Value = STR.y(Convert.ToInt32(worksheet1.Cells[IdRow, 5].Value));//"STR";
                    worksheet1.Cells[IdRow, 9].Value = DEF.y(Convert.ToInt32(worksheet1.Cells[IdRow, 5].Value));//"DEF";

                    package.Save();
                }
                ///BUG! level up slow update on EXP && too much float points
                INFO[0] = LevelUpNum.ToString();
                INFO[1] = worksheet1.Cells[IdRow, 5].Value.ToString();
                INFO[2] = worksheet1.Cells[IdRow, 4].Value.ToString();
                INFO[3] = EXP.val.ToString();
                INFO[4] = Convert.ToInt32(Convert.ToDouble(worksheet1.Cells[IdRow, 4].Value) * 100 /(EXP.y(Convert.ToInt32(worksheet1.Cells[IdRow, 5].Value)))).ToString();
                return INFO;
            }
        }

        //TRADING --------------------------------

        static public int InviteXToTrade(string MyPL_ID, string OtherPL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                int MyPLIdRow = getPLRowByID(MyPL_ID);
                int OtherPLIdRow = getPLRowByID(OtherPL_ID);
                Console.WriteLine(worksheet1.Cells[OtherPLIdRow, 37].Value.ToString()[0]);
                if(worksheet1.Cells[OtherPLIdRow, 37].Value.ToString()[0] != 'T' && worksheet1.Cells[OtherPLIdRow, 37].Value.ToString()[0] != 'I')// Other NOT TRADING NOR ANSWERING INVITE
                {
                    worksheet1.Cells[MyPLIdRow, 37].Value = "T" + OtherPL_ID;
                    worksheet1.Cells[OtherPLIdRow, 37].Value = "I" + MyPL_ID;
                    Console.WriteLine("Normal");
                    package.Save();
                    return 1;
                }
                else if(worksheet1.Cells[OtherPLIdRow, 37].Value.ToString() == "T" + MyPL_ID) //IF OTHER WANTS TO TRADE WITH MYSELF => ACCEPT
                {
                    Console.WriteLine("2sided invite");
                    worksheet1.Cells[MyPLIdRow, 37].Value = "T" + OtherPL_ID;
                    Console.WriteLine("D");
                    package.Save();
                    return 2;
                }
                else
                {
                    Console.WriteLine("awaiting other");
                    return 0;
                }
            } 
        }

        static public string AcceptTradeRequest(string MyPL_ID)
        {
            
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                int MyPLIdRow = getPLRowByID(MyPL_ID);
                string otherId = worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Substring(1, worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Length - 1);
                if (worksheet1.Cells[MyPLIdRow, 37].Value.ToString() == "I" + otherId) //IF
                {
                    Console.WriteLine("BBB");
                    worksheet1.Cells[MyPLIdRow, 37].Value = "T" + otherId;//342 822 739 742 294 026 = 18 worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Length
                    Console.WriteLine("CCC");
                    package.Save(); 
                }
                return otherId;
            }
        }

        static public string DeclineTradeRequest(string MyPL_ID)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                int MyPLIdRow = getPLRowByID(MyPL_ID);
                string otherId = worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Substring(1, worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Length - 1);
                int OtherPLIdRow = getPLRowByID(otherId); //GET TRADER ID
                if (worksheet1.Cells[OtherPLIdRow, 37].Value.ToString() != "T" + MyPL_ID) //IF OTHER NOT TRADING WITH ME
                {
                    //DECLINE INVITE
                    worksheet1.Cells[MyPLIdRow, 37].Value = "N/A";
                    package.Save();
                }
                else
                {
                    //BREAK TRADING
                    //remove self from Trade
                    worksheet1.Cells[MyPLIdRow, 37].Value = "N/A";
                    //remove other from Trade
                    worksheet1.Cells[OtherPLIdRow, 37].Value = "N/A"; //OTHER RESET TRADE PARAM
                    package.Save();
                    
                }
                return otherId;
            }
        }

        static public string[] CheckCurrentTrading(string MyPL_ID, string moneyAmmount)
        {   
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                int MyPLIdRow = getPLRowByID(MyPL_ID);
                int OtherPLIdRow = getPLRowByID(worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Substring(1, worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Length - 1)); //GET TRADER ID
                string[] TradeBag = {worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Substring(1, worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Length - 1) , "", ""}; //|OtherID|MYbag|OTHERbag
                if (worksheet1.Cells[OtherPLIdRow, 37].Value.ToString() == "T" + MyPL_ID) //IF OTHER TRADING WITH MYSELF
                {
                    
                }
                package.Save();
                return TradeBag;
            }
        }

        static public void ConcludeGoodTrading(string MyPL_ID, string moneyAmmount)
        {
            using (ExcelPackage package = new ExcelPackage(databaseFile))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                int MyPLIdRow = getPLRowByID(MyPL_ID);
                int OtherPLIdRow = getPLRowByID(worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Substring(1, worksheet1.Cells[MyPLIdRow, 37].Value.ToString().Length-1)); //GET TRADER ID 
                if (worksheet1.Cells[OtherPLIdRow, 37].Value.ToString() == "T" + MyPL_ID) //IF OTHER TRADING WITH SELF
                {
                    worksheet1.Cells[MyPLIdRow, 37].Value = "N/A"; //SELF RESET TRADE PARAM
                    worksheet1.Cells[OtherPLIdRow, 37].Value = "N/A"; //OTHER RESET TRADE PARAM
                }
                package.Save();
            }
        }

        public struct Math_statsFunc
        {
            float i;
            float v;
            public float val;
            /// <summary>
            /// Contruct Stats Values Fucntion by Level
            /// </summary>
            /// <param name="x">Level</param>
            /// <param name="i">MinVal(40)</param>
            /// <param name="v">GrowVelocity(5)</param>
            /// <returns></returns>
            public float y(int x,float i, float v)
            {
                this.i = i;
                this.v = v;
                val = i + ((float)Math.Pow(x,2)/v);
                return val;
            }

            /// <summary>
            /// Use SavedParams
            /// </summary>
            /// <param name="x">Level</param>
            /// <returns></returns>
            public float y(int x)
            {
                val = this.i + ((float)Math.Pow(x, 2) / this.v);
                return val;
            }
        }

        public struct Math_EXPFunc
        {
            float m;
            float p;
            float v;
            float f;
            public double val;
            /// <summary>
            /// Contruct EXP Values Fucntion for Level
            /// </summary>
            /// <param name="x">Level</param>
            /// <param name="m">MaxLevel(100)</param>
            /// <param name="p">VerticalityPow(8)</param>
            /// <param name="v">GrowingVelocity(80)</param>
            /// <param name="f">EndGameTweek(1.1)</param>
            /// <returns></returns>
            public double y(int x, int m,float p, float v, float f)
            {
                this.m = m;
                this.p = p;
                this.v = v;
                this.f = f;
                this.val = -((m * (float)Math.Pow(v,p)) / ((float)Math.Pow(x,p) + (float)Math.Pow(v, p))) + ((float)Math.Sqrt(x)/f) + m;
                val *= 1000;
                return val;
            }

            /// <summary>
            /// Use SavedParams
            /// </summary>
            /// <param name="x">Level</param>
            /// <returns></returns>
            public double y(int x)
            {
                this.val = -((this.m * (float)Math.Pow(this.v, this.p)) / ((float)Math.Pow(x, this.p) + (float)Math.Pow(this.v, this.p))) + ((float)Math.Sqrt(x) / this.f) + this.m;
                val *= 1000;
                return val;
            }
        }
    }
}
