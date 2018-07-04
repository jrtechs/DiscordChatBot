using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

using AIMLbot;

using System;

/**
 * Driving code for the discord chat bot. This
 * listens for messages on the server and responds
 * with a response from the AIML bot.
 *
 * @author Jeffery Russell 2016
 *
 */
namespace DiscordBot
{
    public class Program
    {
        private DiscordSocketClient _client;

        private static Bot myBot = new Bot();
        static AIMLbot.User myUser = new AIMLbot.User("consoleUser", myBot);

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            myBot.loadSettings();


            myBot.loadAIMLFromFiles();

            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            string token = "42"; // Remember to keep this private!
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {

            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }

            Console.WriteLine(message.Content);
            if (message.Channel.Name.Equals("therapy"))
            {
                if (!message.Author.IsBot)
                {
                    Request r = new Request(message.Content, myUser, myBot);
                    Result res = myBot.Chat(r);
                    Console.WriteLine("Bot: " + res.Output);

                    await message.Channel.SendMessageAsync(res.Output);
                }
            }
            else
            {
                Console.WriteLine(message.Channel.Name);
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return null;
        }
    }
}
