using Discord;
using Discord.WebSocket;
using Discord.Commands;

using System;
using System.IO;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HZbot
{
    class Program
    {
        private readonly DiscordSocketClient _client;
        // Keep the CommandService and IServiceCollection around for use with commands.
        private readonly IServiceCollection _map = new ServiceCollection();
        private readonly CommandService _commands = new CommandService();
        private IServiceProvider _services;

        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private Program()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                // How much logging do you want to see?
                LogLevel = LogSeverity.Info,

                // If you or another service needs to do anything with messages
                // (eg. checking Reactions, checking the content of edited/deleted messages),
                // you must set the MessageCacheSize. You may adjust the number as needed.
                //MessageCacheSize = 50,

                // If your platform doesn't have native websockets,
                // add Discord.Net.Providers.WS4Net from NuGet,
                // add the `using` at the top, and uncomment this line:
                //WebSocketProvider = WS4NetProvider.Instance
            });
            // Subscribe the logging handler to both the client and the CommandService.
            _client.Log += Logger;
            _commands.Log += Logger;
        }

        private static Task Logger(LogMessage message)
        {
            var cc = Console.ForegroundColor;
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message}");
            Console.ForegroundColor = cc;

            // If you get an error saying 'CompletedTask' doesn't exist,
            // your project is targeting .NET 4.5.2 or lower. You'll need
            // to adjust your project's target framework to 4.6 or higher
            // (instructions for this are easily Googled).
            // If you *need* to run on .NET 4.5 for compat/other reasons,
            // the alternative is to 'return Task.Delay(0);' instead.
            return Task.CompletedTask;
        }

        private async Task MainAsync()
        {
            await InitCommands();

            InitDataBase();

            //Remmenber to keep this private
            string token = "MzQyODIyNzM5NzQyMjk0MDI2.DGVORA._v8Cu-Nd3H1gpOQDIloQz3GPFII";

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
          
            //Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private void InitDataBase()
        {
            try
            {
                DirectoryInfo outputDir = new DirectoryInfo(@"x:\DATABASE_HZ");
                if (!outputDir.Exists) throw new Exception("outputDir does not exist!");

                //PLAYER DATABASE
                Console.WriteLine("Running PlayerDatabase");
                string output = PLDatabase.Create(outputDir);
                Console.WriteLine("PlayerDatabase Online", output);   
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            Console.WriteLine();
            //Console.WriteLine("Press the return key to exit...");
            //Console.Read();
        }

        

        private async Task InitCommands()
        {
            // Repeat this for all the service classes
            // and other dependencies that your commands might need.
            _map.AddSingleton(new cmd_Roles());
            _map.AddSingleton(new cmd_Stats());
            _map.AddSingleton(new cmd_Inventory());
            _map.AddSingleton(new cmd_Trade());

            // When all your required services are in the collection, build the container.
            // Tip: There's an overload taking in a 'validateScopes' bool to make sure
            // you haven't made any mistakes in your dependency graph.
            _services = _map.BuildServiceProvider();

            // Either search the program and add all Module classes that can be found.
            // Module classes *must* be marked 'public' or they will be ignored.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            // Or add Modules manually if you prefer to be a little more explicit:
            //await _commands.AddModuleAsync<SomeModule>();

            // Subscribe a handler to see if a message invokes a command.
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            if (msg == null) return;

            // Create a number to track where the prefix ends and the command begins
            int pos = 0;
            if (msg.HasCharPrefix('!', ref pos) || msg.HasMentionPrefix(_client.CurrentUser, ref pos))
            {
                // Create a Command Context.
                var context = new SocketCommandContext(_client, msg);

                // Execute the command. (result does not indicate a return value, 
                // rather an object stating if the command executed succesfully).
                var result = await _commands.ExecuteAsync(context, pos, _services);

                // Uncomment the following lines if you want the bot
                // to send a message if it failed (not advised for most situations).
                //if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                //    await msg.Channel.SendMessageAsync(result.ErrorReason);
            }
        }     
    }
}
