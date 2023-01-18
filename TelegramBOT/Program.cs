using Colorful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBOT.Dados;
using TelegramBOT.Mensagens;
using Console = Colorful.Console;

namespace TelegramBOT
{
    internal class Program
    {
        private static BuscarDados buscarDados = new BuscarDados();

        private static async Task Main(string[] args)
        {
            WindowUtility.MoveWindowToCenter();
            var botClient = new TelegramBotClient("5981284492:AAErr_sfCmKrBvniMmynjpSIXa5K-Foh-_A");

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            //FigletFont font = FigletFont.Load("doh.flf");
            //Console.WriteAscii("ROBINHO", font, Color.FromArgb(255, 0, 113));

            //List<char> chars = new List<char>()
            //{
            //    'T','e','l','e','g','r','a','m',' ',' ',' ','B','O','T'
            //};
            //Console.WriteWithGradient(chars, Color.Yellow, Color.Fuchsia, 14);
            //ColorAlternator calt = new FrequencyBasedColorAlternator(1, Color.FromArgb(117, 255, 196), Color.BlueViolet);
            ColorAlternator corAlternante = new FrequencyBasedColorAlternator(1, Color.DarkSlateBlue, Color.BlueViolet);
            FigletFont figletFont = FigletFont.Load("doom.flf");
            //Console.WriteAscii("Telegram   BOT", font2, Color.FromArgb(117, 255, 196));
            Console.WriteAsciiAlternating("Telegram   BOT", figletFont, corAlternante);

            #region Tipos de fonte no menu

            //FigletFont font3 = FigletFont.Load("epic.flf");
            //Console.WriteAscii("ROBINHO", font3, Color.FromArgb(255, 0, 113));

            //FigletFont font4 = FigletFont.Load("jazmine.flf");
            //Console.WriteAscii("ROBINHO", font4, Color.FromArgb(255, 0, 113));

            //FigletFont font5 = FigletFont.Load("larry3d.flf");
            //Console.WriteAscii("ROBINHO", font5, Color.FromArgb(255, 0, 113));

            //FigletFont font6 = FigletFont.Load("speed.flf");
            //Console.WriteAscii("ROBINHO", font6, Color.FromArgb(255, 0, 113));

            //List<char> chars = new List<char>()
            //{
            //    'R','O','B','I','N','H','O'
            //};
            //Console.WriteWithGradient(chars, Color.Yellow, Color.Fuchsia, 14);

            #endregion Tipos de fonte no menu

            System.Console.WriteLine();
            System.Console.WriteLine();
            //Console.WriteLine($"--- ESTUDOS Bot Telegram ---", Color.Chartreuse);

            int r = 225;
            int g = 255;
            int b = 250;

            List<string> textos = new List<string>()
            {
                $"|-----------------------------------------------------------------------------|",
               // $"Nome do bot: {me.Username}",
                $"|                   Nome do bot: StandBy Assistencia TEC_BOT                  |",
                //$"Id do Bot: {me.Id}\n",
                $"|                            Id do Bot: 554548713                             |",
                $"|-----------------------------------------------------------------------------|",
                $"                       Digite /quit para finalizar o BOT                       "
            };

            for (int i = 0; i < textos.Count; i++)
            {
                Console.WriteLine(textos[i], Color.FromArgb(r, g, b));
                r -= 18;
                b -= 9;
            }

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            Console.WriteAscii("   INICIO", FigletFont.Default, Color.FloralWhite);

            string input;
            do
            {
                input = Console.ReadLine();
            } while (input != "/quit");

            // Send cancellation request to stop bot
            cts.Cancel();

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                #region Explicação

                //update.Message NAO É DO TIPO Message? entao armazena o valor na variavel "message"
                //e essa variavel fica visivel pra o restante do escopo.
                //is not { } é equivalente a is not Message  <--- sendo o "Message" sendo o tipo da variavel a esquerda

                #endregion Explicação

                //Tipo = Message
                //Sao todos os inputs digitados pelo usuario, diferente de Call back que são as devoluções
                //do telegram quando um usuario clica em algum botao no chat.
                if (update.Type == UpdateType.Message)
                {
                    long chatId = 0;
                    if (update.Message.Text == "/comandos")
                    {
                        LogsConsole.LogComandoEnviadoPeloUsuario(update);
                        InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Buscar Clientes", callbackData: "buscar_clientes"),
                                InlineKeyboardButton.WithCallbackData(text: "Buscar Serviços", callbackData: "buscar_servicos"),
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Buscar Lucro (Mês)", callbackData: "buscar_lucro_mes"),
                                InlineKeyboardButton.WithCallbackData(text: "Inserir Cliente", callbackData: "inserir_cliente"),
                            },
                        });

                        chatId = update.Message.Chat.Id;
                        await Mensagem.EnviarMensagemTelegramComBotoes(botClient, cancellationToken, chatId, inlineKeyboard, "Escolha uma opção abaixo");
                    }
                }

                //Todas Callback
                else if (update.Type == UpdateType.CallbackQuery)
                {
                    long chatId = 0;
                    var querySolicitada = update.CallbackQuery.Data;

                    if (querySolicitada.Contains("buscar_lucro_mes"))
                    {
                        if (querySolicitada.Equals("buscar_lucro_mes"))
                        {
                            InlineKeyboardMarkup inlineKeyboard = new(new[]
                            {
                                // first row
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData(text: "Janeiro", callbackData: "buscar_lucro_mes/1"),
                                    InlineKeyboardButton.WithCallbackData(text: "Fevereiro", callbackData: "buscar_lucro_mes/2"),
                                    InlineKeyboardButton.WithCallbackData(text: "Março", callbackData: "buscar_lucro_mes/3"),
                                    InlineKeyboardButton.WithCallbackData(text: "Abril", callbackData: "buscar_lucro_mes/4"),
                                },
                                // second row
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData(text: "Maio", callbackData: "buscar_lucro_mes/5"),
                                    InlineKeyboardButton.WithCallbackData(text: "Junho", callbackData: "buscar_lucro_mes/6"),
                                    InlineKeyboardButton.WithCallbackData(text: "Julho", callbackData: "buscar_lucro_mes/7"),
                                    InlineKeyboardButton.WithCallbackData(text: "Agosto", callbackData: "buscar_lucro_mes/8"),
                                },
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData(text: "Setembro", callbackData: "buscar_lucro_mes/9"),
                                    InlineKeyboardButton.WithCallbackData(text: "Outubro", callbackData: "buscar_lucro_mes/10"),
                                    InlineKeyboardButton.WithCallbackData(text: "Novembro", callbackData: "buscar_lucro_mes/11"),
                                    InlineKeyboardButton.WithCallbackData(text: "Dezembro", callbackData: "buscar_lucro_mes/12"),
                                },
                            });

                            chatId = update.CallbackQuery.Message.Chat.Id;
                            await Mensagem.EnviarMensagemTelegramComBotoes(botClient, cancellationToken, chatId, inlineKeyboard, "Qual mes gostaria de ver os lucros?");
                        }
                        else if (querySolicitada.Contains("buscar_lucro_mes/"))
                        {
                            chatId = update.CallbackQuery.Message.Chat.Id;
                            LogsConsole.LogQuerySolicitada(update);

                            //Processando a informacao no banco
                            await Mensagem.EnviarMensagemTelegram(botClient, cancellationToken, chatId, $"Estamos processando a sua solicitação, aguarde...'\n");
                            string mesAbreviado = PegarTresPrimeirasLetrasMes(Convert.ToInt32(querySolicitada.Split('/')[1]));
                            var retorno = buscarDados.LucrosMes(mesAbreviado);

                            //Log do retorno
                            LogsConsole.LogRetornoQuery(update, retorno.ToString());

                            //Enviando msg pra o usuario informando retorno
                            await Mensagem.EnviarMensagemTelegramHtml(botClient, cancellationToken, chatId, $"Lucro total do mes 11/22 foi de: <b>{retorno}</b>");
                            //Console.WriteLine($"--------------------------------", Color.FromArgb(225, 255, 250));
                        }
                    }
                    else if (querySolicitada.Contains("buscar_servicos"))
                    {
                        chatId = update.CallbackQuery.Message.Chat.Id;
                        if (querySolicitada.Equals("buscar_servicos"))
                        {
                            #region Legenda do vetor

                            //0 = id
                            //1 = sv_cl_idcliente
                            //2 = sv_data
                            //3 = cl_nome
                            //4 = sv_aparelho
                            //5 = sv_defeito
                            //6 = sv_situacao
                            //7 = sv_senha
                            //8 = sv_valorservico
                            //9 = sv_valorpeca
                            //10 = sv_lucro
                            //11 = sv_servico
                            //12 = sv_previsao_entrega
                            //13 = sv_existe_um_prazo
                            //14 = sv_acessorios
                            //15 = sv_cor_tempo
                            //16 = sv_tempo_para_entregar

                            #endregion Legenda do vetor

                            var servicosAtivos = buscarDados.BuscarServicosEmAndamento();
                            InlineKeyboardMarkup inlineKeyboard = null;
                            List<InlineKeyboardButton[]> botoes = new List<InlineKeyboardButton[]>();

                            foreach (DataRow servico in servicosAtivos.Rows)
                            {
                                botoes.Add(
                                    new[]
                                        {
                                        InlineKeyboardButton.WithCallbackData(text: $"{servico[3]} - {servico[4]}" , callbackData: $"buscar_servicos/{servico[0]}"),
                                        }
                                );
                            }

                            inlineKeyboard = new(botoes.ToArray());

                            await Mensagem.EnviarMensagemTelegramComBotoes(botClient, cancellationToken, chatId, inlineKeyboard, "Lista com os serviços em ativos no momento");
                            //var c = (DataRow)v[2];
                        }
                        else if (querySolicitada.Contains("buscar_servicos/"))
                        {
                            LogsConsole.LogQuerySolicitada(update);
                            string id = "";
                            try
                            {
                                id = querySolicitada.Substring(16);
                            }
                            catch (Exception e)
                            {
                                System.Console.WriteLine(e);
                                await Mensagem.EnviarMensagemTelegramHtml(botClient, cancellationToken, chatId, "<b>ERRO - Contate o desenvolvedor.</b>");
                                throw;
                            }

                            var servico =
                                buscarDados.BuscarServicoPorID(Convert.ToInt32(id));

                            StringBuilder builder = new StringBuilder();

                            builder.Append($"<b>Cliente</b>: {servico[3]}\n");
                            builder.Append($"<b>Aparelho</b>: {servico[4]}\n");
                            builder.Append($"<b>Defeito</b>: {servico[5]}\n");
                            builder.Append($"<b>Situação</b>: \n{servico[6]}");

                            LogsConsole.LogRetornoQuery(update, "Dados do usuario solicitado");
                            await Mensagem.EnviarMensagemTelegramHtml(botClient, cancellationToken, chatId, builder.ToString());
                        }
                    }
                }

                #region Pedir pra o user compartilhar o contato dele ou localização (botao em baixo do chat)

                //ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                //{
                //    KeyboardButton.WithRequestLocation("Share Location"),
                //    KeyboardButton.WithRequestContact("Share Contact"),
                //});

                //Message sentMessage = await botClient.SendTextMessageAsync(
                //    chatId: chatId,
                //    text: "Who or Where are you?",
                //    replyMarkup: replyKeyboardMarkup,
                //    cancellationToken: cancellationToken);

                #endregion Pedir pra o user compartilhar o contato dele ou localização (botao em baixo do chat)

                #region Pedir para o usuario selecionar uma opção (botao em baixo do chat)

                //ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                //{
                //    new KeyboardButton[] { "Help me", "Call me ☎️" },
                //})
                //{
                //    ResizeKeyboard = true
                //};

                //Message sentMessage = await botClient.SendTextMessageAsync(
                //    chatId: chatId,
                //    text: "Choose a response",
                //    replyMarkup: replyKeyboardMarkup,
                //    cancellationToken: cancellationToken);

                #endregion Pedir para o usuario selecionar uma opção (botao em baixo do chat)

                #region Remover o menu de opcoes da tela do user

                //Message sentMessage1 = await botClient.SendTextMessageAsync(
                //    chatId: chatId,
                //    text: "Removing keyboard",
                //    replyMarkup: new ReplyKeyboardRemove(),
                //    cancellationToken: cancellationToken);

                #endregion Remover o menu de opcoes da tela do user

                #region Enviar uma marcação pra algum contato ou marcar no proprio chat (?)

                //InlineKeyboardMarkup inlineKeyboard = new(new[]
                //{
                //    InlineKeyboardButton.WithSwitchInlineQuery(
                //        text: "switch_inline_query"),
                //    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(
                //        text: "switch_inline_query_current_chat"),
                //});

                //Message sentMessage = await botClient.SendTextMessageAsync(
                //    chatId: chatId,
                //    text: "A message with an inline keyboard markup",
                //    replyMarkup: inlineKeyboard,
                //    cancellationToken: cancellationToken);

                #endregion Enviar uma marcação pra algum contato ou marcar no proprio chat (?)
            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }

        public static string PegarTresPrimeirasLetrasMes(int _numeroMes)
        {
            DateTime date = new DateTime(2020, _numeroMes, 1);
            var mesAbreviado = date.ToString("MMM");

            return mesAbreviado;
        }
    }
}