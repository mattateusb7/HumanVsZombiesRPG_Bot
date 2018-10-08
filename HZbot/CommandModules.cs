using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace HZbot
{
//await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num,2)}");
    [Group("help"), Summary("help commands")]
    public class cmd_help : ModuleBase
    {
        [Command(""), Summary("help commands")]
        public async Task Help()
        {
            var userinfo = Context.Message.Author;
            var eb = new EmbedBuilder();
            eb.Title = "HELP commands!";
            eb.AddInlineField("UserCommands", "!help\n");
            await userinfo.SendMessageAsync("", false, eb);
        }
    }

    public class cmd_Roles : ModuleBase
    {
        //public IEnumerable<SocketRole> Roles = (IEnumerable<SocketRole>)Context.Guild.Roles;
        [Command("myrole"), Summary("Chose roll: ZOMBIE / HUMAN")]
        public async Task InitRole([Summary("Role choice")] IRole RoleChoice)
        {
            //Get User calling Command
            //var userinfo = Context.Message.Author;
            var userinfoIGU = (Context.User as SocketGuildUser) as IGuildUser;
            
            //Get newRole
            string strRoleChoice = RoleChoice.ToString().ToUpper();
           
            //Get Guild Role List
            var RolesList = userinfoIGU.Guild.Roles;
                //await ReplyAsync((userinfo as IGuildUser).Guild.Roles.First(x => x.Name == "LostSoul").Name);
            
            //GET ID OF ROLE
            var Id_LostSoul = userinfoIGU.Guild.Roles.First(x => x.Name == "LostSoul").Id;
            if (userinfoIGU.RoleIds.Contains(Id_LostSoul))
            {
                switch (strRoleChoice)
                {
                    case "ZOMBIE":
                    case "HUMAN":
                        //set Roles 
                        await userinfoIGU.RemoveRoleAsync(RolesList.First(x => x.Name == "LostSoul"));
                        await userinfoIGU.AddRoleAsync(RoleChoice);
                        await ReplyAsync($"{Context.User.Mention} your role has been set to: {strRoleChoice}\n");
                        PLDatabase.addPlayer(Context.Message.Author.Username,strRoleChoice, (Context.Message.Author.Id).ToString());
                        break;
                    default:
                        await ReplyAsync($"\"{strRoleChoice}\" is not a valid Choice\nTry HUMAN or ZOMBIE instead.");
                        break;
                }
            }
            else
            {
                await ReplyAsync("To Change Faction please talk with a GameMaster(GM)");
            }
            try { await Context.Message.DeleteAsync(); } catch (Exception e) { } //DELETE USER REQUEST MSG
        }
    }

    public class cmd_Stats : ModuleBase
    {
        [Command("Stats"), Summary("ViewStats")]
        public async Task Stats()
        {
            var userinfo = Context.Message.Author;
            string[] PlStats = PLDatabase.getStatsOf(userinfo.Id.ToString());
            var eb = new EmbedBuilder();
            eb.Color = Color.Gold;
            eb.Title = "**Stats Menu:**";
            eb.Description = userinfo.Username;
            int j = 1;
            foreach(string s in PlStats)
            {
                switch (j)
                {
                    case 1:
                        eb.AddField($"{j.ToString()} :MONEY:", $"Shlinks => {decimal.Round(Convert.ToDecimal(s), 0)}");
                        break;
                    case 2:
                        eb.AddInlineField($"{j.ToString()} :HP:", $"Health => {decimal.Round(Convert.ToDecimal(s), 0)}");
                        break;
                    case 3:
                        eb.AddInlineField($"{j.ToString()} :STR:", $"Strength => {decimal.Round(Convert.ToDecimal(s), 0)}");
                        break;
                    case 4:
                        eb.AddInlineField($"{j.ToString()} :DEF:", $"Defense => {decimal.Round(Convert.ToDecimal(s), 0)}");
                        break;
                }

                j++;
            }
            await userinfo.SendMessageAsync("", false, eb);
            try { await Context.Message.DeleteAsync(); } catch (Exception e) { } //DELETE USER REQUEST MSG
        }

        [Command("Level"), Summary("UpdatesEXP LVLS")]
        public async Task EXPLevel()
        {
            bool isinguild = false;
            try
            {
                isinguild = (Context.Guild.Id == 342823780760027147);
            }
            catch(Exception e)
            {
                //Console.WriteLine($"{Context.User.Id} wrongly uses '!level'");
            }

            if (isinguild)
            {
                //LVLnum,PL_LVL,playerEXP,nextEXP,%
                var eb = new EmbedBuilder();
                eb.Title = $"LEVEL MENU";
                eb.Description = $"{Context.Message.Author.Username}";
                eb.Color = Color.Gold;

                string[] INFO = PLDatabase.updateEXPtoPlayerByID((Context.Message.Author.Id).ToString());
                string lvlbar = $"[{INFO[4]}%]>";
                int barnum = Convert.ToInt32(INFO[4][0].ToString());
                //DEBUG//Console.WriteLine(barnum);
                for (int i = 0; i < 10; i++)
                {
                    if (i < barnum && INFO[4].Length == 2)
                    {
                        lvlbar += "»";
                    }
                    else
                    {
                        lvlbar += "..";
                    }
                }
                //EXP&LVL
                eb.AddField($"LVL: **__{INFO[1]}__**", $"EXP: {decimal.Round(Convert.ToDecimal(INFO[2]), 2)} / {decimal.Round(Convert.ToDecimal(INFO[3]), 2)}");
                eb.AddField("LVL%:", $"{lvlbar}<[100 %]");
                await ReplyAsync("", false, eb);

                //LVL UP
                if (Convert.ToUInt32(INFO[0]) == 1)
                {
                    await ReplyAsync($"{Context.User.Mention} LEVEL UP !\n");
                }
                else if (Convert.ToUInt32(INFO[0]) > 1)
                {
                    await ReplyAsync($"{Context.User.Mention} LEVEL UP {INFO[0]} TIMES!\n");
                }
                //ROLE
                if (INFO[5].Length > 0)
                {
                    await (Context.Message.Author as IGuildUser).AddRoleAsync(Context.Guild.Roles.First(x => x.Name == INFO[5]));
                    await ReplyAsync($"{Context.User.Mention} You have become a {INFO[5]}!\n");
                }          
            }
            else
            {
                await Context.Message.Author.SendMessageAsync("Please use the \"!LEVL\" command exclusively in the server channels.");
            }
            try{await Context.Message.DeleteAsync();}catch (Exception e){ } //DELETE USER REQUEST MSG
        }
    }

    [Group("inventory")]
    public class cmd_Inventory : ModuleBase
    {

        [Command("Open"), Summary("Shows Inventory")]
        public async Task PrintInventory()
        {
            var m = Context.Message;
            var userinfo = Context.Message.Author;
            string[] Inv = PLDatabase.getInventoryOf(userinfo.Id.ToString());
            var eb = new EmbedBuilder();
            eb.Title = $"**Inventory Menu**";
            eb.Description = $":: {userinfo.Username} ::";
            eb.Color = Color.Gold;

            //Console.WriteLine("AAA");
            //Add Indexes
            int j = 1;
            //string invprint = "";
            foreach (string s in Inv)
            {   
                if(s.Split('#')[3] != "0") //NOT EMPTY
                {
                    if (s.Split('#')[5] != "0")//Armor
                    {
                        eb.AddInlineField($"{j.ToString()}: {s.Split('#')[1]} LVL: {s.Split('#')[3]}", $"Description:\n{s.Split('#')[2]}\nDefense = {s.Split('#')[5]} | SpecialAttributes = {s.Split('#')[4]}");
                    }else if(s.Split('#')[6] != "0")//weapon
                    {
                        eb.AddInlineField($"{j.ToString()}: {s.Split('#')[1]} LVL: {s.Split('#')[3]}", $"Description:\n{s.Split('#')[2]}\nDamage = {s.Split('#')[6]} | Accuracy = {s.Split('#')[6]} | SpecialAttributes = {s.Split('#')[4]}");
                    }
                }
                else //EMPTY SPACE
                {
                    eb.AddInlineField($"{j.ToString()}: {s.Split('#')[1]}", "Empty Space");
                }

                //HA#BareFist#ITEM_DESC#1#0#0#10*1D2#30##
                j++;
            }
            await userinfo.SendMessageAsync("", false, eb);
            try{await Context.Message.DeleteAsync();}catch (Exception e){ } //DELETE USER REQUEST MSG
        }

        [Command("Equipment"), Summary("Shows Equiment")]
        public async Task PrintEquipment()
        {
            var userinfo = Context.Message.Author;
            string[] equip = PLDatabase.getEquipmentOf(userinfo.Id.ToString());
            var eb = new EmbedBuilder();
            eb.Color = Color.Gold;
            //Console.WriteLine("AAA");
            //Add Indexes
            int j = 1;
            foreach (string s in equip)
            {          
                switch (j)
                {
                    case 1:
                        eb.AddInlineField($"{j.ToString()} :HEAD: {s}", "STATS");
                        break;
                    case 2:
                        eb.AddInlineField($"{j.ToString()} :R-HAND: {s}", "STATS");
                        break;
                    case 3:
                        eb.AddInlineField($"{j.ToString()} :L-HAND: {s}", "STATS");
                        break;
                    case 4:
                        eb.AddInlineField($"{j.ToString()} :CHEST: {s}", "STATS");
                        break;
                    case 5:
                        eb.AddInlineField($"{j.ToString()} :LEGS: {s}", "STATS");
                        break;
                    case 6:
                        eb.AddInlineField($"{j.ToString()} :BOOTS: {s}", "STATS");
                        break;
                    case 7:
                        eb.AddInlineField($"{j.ToString()} :ACCESSORY: {s}", "STATS");
                        break;
                }
                
                j++;
            }
            eb.Title = $"**Equipment Menu**";
            eb.Description = userinfo.Username;
            await userinfo.SendMessageAsync("", false, eb);
            try{await Context.Message.DeleteAsync();}catch (Exception e){ } //DELETE USER REQUEST MSG
        }

        [Command("EquipItem"), Summary("Equips item from Inventory")]
        public async Task EquipItem([Summary("Inventory Item index")] int num)
        {
            var userinfo = Context.Message.Author;
            if (num > 20 || num < 1)
            {
                await userinfo.SendMessageAsync($"**he Chosen Item is not Equipable.**");
                return;
            }
            string[] Inv = PLDatabase.getInventoryOf(userinfo.Id.ToString());
            if (PLDatabase.EquipItemXOf(num, Context.Message.Author.Id.ToString()))
            {
                await userinfo.SendMessageAsync($"**Item __{Inv[num - 1]}__ Equiped.**");
            }
            else
            {
                await userinfo.SendMessageAsync($"**The Chosen Item is not Equipable __OR__ Your hands are Full.**");
            }
            try{await Context.Message.DeleteAsync();}catch (Exception e){ } //DELETE USER REQUEST MSG
        }

        [Command("UnEquipItem"), Summary("UnEquips item from Inventory")]
        public async Task UnEquipItem([Summary("Equipment Item index")] int num)
        {
            var userinfo = Context.Message.Author;
            if (num > 5 || num < 1)
            {
                await userinfo.SendMessageAsync($"**There isn't any Item to Unequip in the chosen slot __OR__ Your inventory is Full.**");
                return;
            }
            string[] Equips = PLDatabase.getEquipmentOf(userinfo.Id.ToString());
            if (PLDatabase.UnEquipItemXOf(num, Context.Message.Author.Id.ToString()) && Equips[num-1] != "_empty_")
            {
                await userinfo.SendMessageAsync($"**Item __{Equips[num - 1]}__ Unequiped.**");
            }
            else
            {
                await userinfo.SendMessageAsync($"**There isn't any Item to Unequip in the chosen slot __OR__ Your inventory is Full.**");
            }
            try{await Context.Message.DeleteAsync();}catch (Exception e){ } //DELETE USER REQUEST MSG
        }

        [Command("useItem"), Summary("Use an Usable Item")]
        public async Task UseItem([Summary("Item index")] string num)
        {
            /*var ItemDesc = //get Item from database
                if (ItemDesc != null)
                await ReplyAsync($"{ItemDesc}:\n");*/
        }

        [Command("destroyItem"), Summary("")]
        public async Task DestroyItem([Summary("Item to be destroyed. This action cannot be undone.")] int  num)
        {
            var userinfo = Context.Message.Author;
            if (num > 20 || num < 1)
            {
                await userinfo.SendMessageAsync($"**There isn't any Item to Destroy in the chosen slot.**");
                return;
            }
            string[] Inv = PLDatabase.getInventoryOf(userinfo.Id.ToString()); 
            await userinfo.SendMessageAsync($"**Item __{Inv[num-1]}__ has been destroyed. This action cannot be undone :(**");
            PLDatabase.DestroyItemXOf(num, userinfo.Id.ToString());
            try{await Context.Message.DeleteAsync();}catch (Exception e){ } //DELETE USER REQUEST MSG
        }
    }

    [Group("Trade")]
    public class cmd_Trade : ModuleBase
    {
        [Command("invite"), Summary("")]
        public async Task InviteXToTrade([Summary("PLID of other")] IUser otherPL_ID)
        {
            var userinfo = Context.Message.Author;
            var otherinfo = Context.Guild.GetUsersAsync().Result.First(x => x == otherPL_ID); //GET USER BY ID
            if(otherinfo.Status == UserStatus.Online)
            {
                if (otherPL_ID.ToString() != userinfo.ToString())
                {
                    switch (PLDatabase.InviteXToTrade(userinfo.Id.ToString(), otherinfo.Id.ToString()))
                    {
                        case 1:
                            await otherinfo.SendMessageAsync($"**__{userinfo.Username}__** has sent you a TRADE REQUEST. *!Trade accept* / *!Trade decline*.");
                            break;
                        case 2:
                            Console.WriteLine("DDD");
                            await userinfo.SendMessageAsync($"You're now trading with **__{otherinfo.Username}__** ! use __!help trade__ if you need help with the commands.");
                            await otherinfo.SendMessageAsync($"You're now trading with **__{userinfo.Username}__** ! use __!help trade__ if you need help with the commands.");
                            break;
                        case 0:
                            await userinfo.SendMessageAsync($"**__{otherinfo.Username}__** is occupied Trading with someone else.");
                            break;
                    }
                }
                else
                {
                    await userinfo.SendMessageAsync("You can't Trade with yourself...");
                }
            }
            else
            {
                await userinfo.SendMessageAsync($"You can only Trade with Online Users... **__{otherinfo.Username}__** is {otherinfo.Status}.");

            }
            try{await Context.Message.DeleteAsync();}catch (Exception e){ } //DELETE USER REQUEST MSG
        }

        [Command("accept"), Summary("Accept Trade Request")]
        public async Task TradeAccept()
        {
            //Accept fails when trading with = N/A
            var userinfo = Context.Message.Author;
            string otherId = PLDatabase.AcceptTradeRequest(userinfo.Id.ToString());
            var otherinfo = Context.Guild.GetUsersAsync().Result.First(x => Convert.ToString(x.Id) == otherId); //GET USER BY ID
            Console.WriteLine("DDD");
            await userinfo.SendMessageAsync($"You're now trading with **__{otherinfo.Username}__** ! use __!help trade__ if you need help with the commands.");
            await otherinfo.SendMessageAsync($"You're now trading with **__{userinfo.Username}__** ! use __!help trade__ if you need help with the commands.");
            try{await Context.Message.DeleteAsync();}catch (Exception e){ } //DELETE USER REQUEST MSG
        }

        [Command("decline"), Summary("Decline Trade Request")]
        public async Task TradeDecline()
        {
            //Decline fails when trading with = N/A
            var userinfo = Context.Message.Author;
            string otherId = PLDatabase.DeclineTradeRequest(userinfo.Id.ToString());
            var otherinfo = Context.Guild.GetUsersAsync().Result.First(x => Convert.ToString(x.Id) == otherId); //GET USER BY ID
            Console.WriteLine("CCC");
            await userinfo.SendMessageAsync($"You've declined the Trading request from **__{otherinfo.Username}__**.");
            await otherinfo.SendMessageAsync($"You're no longer trading with **__{userinfo.Username}__**.");
            try{await Context.Message.DeleteAsync();}catch (Exception e){ } //DELETE USER REQUEST MSG
        }

        [Command("check"), Summary("Decline Trade Request")]
        public async Task TradeCheck()
        {
            var userinfo = Context.Message.Author;
            string otherId = PLDatabase.DeclineTradeRequest(userinfo.Id.ToString());
            var otherinfo = Context.Guild.GetUsersAsync().Result.First(x => Convert.ToString(x.Id) == otherId); //GET USER BY ID
            Console.WriteLine("CCC");
            await userinfo.SendMessageAsync($"You've declined the Trading request from **__{otherinfo.Username}__**.");
            await otherinfo.SendMessageAsync($"You're no longer trading with **__{userinfo.Username}__**.");
            try { await Context.Message.DeleteAsync(); } catch (Exception e) { } //DELETE USER REQUEST MSG
        }
    }
}



