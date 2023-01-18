using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBOT.Mensagens
{
    public static class Mensagem
    {
        public static async Task EnviarMensagemTelegramComBotoes(ITelegramBotClient botClient, CancellationToken cancellationToken, long chatId, InlineKeyboardMarkup inlineKeyboard, string _mensagem)
        {
            _ = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: _mensagem,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Metodo que envia uma mensagem para o telegram sem nenhum retorno, apenas uma mensagem pura
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="chatId"></param>
        /// <param name="_mensagem"></param>
        /// <returns></returns>
        public static async Task EnviarMensagemTelegram(ITelegramBotClient botClient, CancellationToken cancellationToken, long chatId, string _mensagem)
        {
            _ = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: _mensagem,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Metodo que envia uma mensagem para o telegram com o formato HTML, voce pode personaliza a mensagem usando TAGs HTML.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="chatId"></param>
        /// <param name="_mensagem"></param>
        /// <returns></returns>
        public static async Task EnviarMensagemTelegramHtml(ITelegramBotClient botClient, CancellationToken cancellationToken, long chatId, string _mensagem)
        {
            _ = await botClient.SendTextMessageAsync(
                chatId: chatId,
                parseMode: ParseMode.Html,
                text: _mensagem,
                cancellationToken: cancellationToken);
        }
    }
}