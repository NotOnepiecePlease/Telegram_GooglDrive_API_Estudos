using Colorful;
using System.Drawing;
using Telegram.Bot.Types;
using Console = Colorful.Console;

namespace TelegramBOT.Mensagens
{
    internal class LogsConsole
    {
        public static void LogComandoEnviadoPeloUsuario(Update update)
        {
            StyleSheet corComando = new StyleSheet(Color.White);
            corComando.AddStyle("/[a-z]*", Color.MediumSlateBlue);

            StyleSheet corUser = new StyleSheet(Color.White);
            corUser.AddStyle($"{update.Message.From.FirstName}", Color.DarkGoldenrod);

            Console.WriteLine($"--------------------------------", Color.FromArgb(225, 255, 250));
            //Console.WriteLine($"User: {update.Message.From.FirstName}", Color.FromArgb(255, 255, 255));
            Console.WriteStyled($"Comando digitado: {update.Message.Text}", Color.FromArgb(255, 255, 255), corComando);
            Console.WriteStyled($" -- (User: {update.Message.From.FirstName})", Color.FromArgb(255, 255, 255), corUser);
            System.Console.WriteLine();
            //Console.WriteLine($"Comando digitado: {update.Message.Text} -- (User: {update.Message.From.FirstName})", Color.FromArgb(255, 255, 255));
        }

        public static void LogQuerySolicitada(Update update)
        {
            StyleSheet corUser = new StyleSheet(Color.White);
            corUser.AddStyle($"{update.CallbackQuery.Message.Chat.FirstName}", Color.DarkGoldenrod);

            Console.Write($"Query solicitada: {update.CallbackQuery.Data}", Color.FromArgb(255, 255, 255));
            Console.WriteStyled($" -- (User: {update.CallbackQuery.Message.Chat.FirstName})", Color.FromArgb(255, 255, 255), corUser);
            System.Console.WriteLine();
        }

        public static void LogRetornoQuery(Update _update, string _retorno)
        {
            StyleSheet corRetorno = new StyleSheet(Color.White);
            corRetorno.AddStyle($"{_retorno}", Color.HotPink);

            StyleSheet corBotName = new StyleSheet(Color.White);
            corBotName.AddStyle($"{_update.CallbackQuery.Message.From.FirstName}", Color.YellowGreen);

            StyleSheet corUser = new StyleSheet(Color.White);
            corUser.AddStyle($"{_update.CallbackQuery.Message.Chat.FirstName}", Color.DarkGoldenrod);

            Console.WriteStyled($"Retorno da query: {_retorno}", Color.FromArgb(255, 255, 255), corRetorno);
            Console.WriteStyled($" -- (User: {_update.CallbackQuery.Message.From.FirstName}", Color.FromArgb(255, 255, 255), corBotName);
            Console.WriteStyled($" - {_update.CallbackQuery.Message.Chat.FirstName})", Color.FromArgb(255, 255, 255), corUser);
            System.Console.WriteLine();
        }
    }
}