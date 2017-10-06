using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using OrderBot.Data;
using System.Linq;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace OrderBot.Dialogs
{
    [Serializable]
    public class StockItemSelectionDialog : IDialog<Stockitem>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var dialog = new PromptDialog.PromptString("What do you want to order?", "Please tell me the name of the product.", 3);

            context.Call(dialog, OnStockitemNameReceivedAsync);
        }

        private async Task OnStockitemNameReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            var stockitemName = await result;

            // send typing message
            var typingMessage = context.MakeMessage();
            typingMessage.Type = ActivityTypes.Typing;

            await context.PostAsync(typingMessage);

            await Task.Delay(3000);

            // get results
            var dataManager = new DataManager();
            var stockItems = await dataManager.GetStockitemsAsync(stockitemName);

            if (stockItems.Count == 0)
            {
                var dialog = new PromptDialog.PromptString("Sorry, we didn't find this article. What is its name?", "Sorry. What product did you mean?", 3);

                context.Call(dialog, OnStockitemNameReceivedAsync);
            }
            else if (stockItems.Count == 1)
            {
                await SendOrderConfirmationAsync(context, stockItems.First());
            }
            else
            {
                var dialog = new PromptDialog.PromptChoice<Stockitem>(stockItems, "Please select your product.", "Please select.", 3);

                context.Call(dialog, OnStockItemReceivedAsync);
            }
        }

        private async Task OnStockItemReceivedAsync(IDialogContext context, IAwaitable<Stockitem> result)
        {
            var stockitem = await result;

            await SendOrderConfirmationAsync(context, stockitem);
        }

        private async Task SendOrderConfirmationAsync(IDialogContext context, Stockitem stockitem)
        {
            var card = new HeroCard()
            {
                Title = stockitem.StockItemName,
                Subtitle = stockitem.Size,
                Text = $"Buy now for {stockitem.RecommendedRetailPrice} $!",
                Images = new List<CardImage>()
                {
                    new CardImage()
                    {
                        Url = "https://sec.ch9.ms/ch9/56e2/7d81df89-2160-453d-b300-255ef17756e2/Behindscenes_960.jpg",
                        Tap = new CardAction(ActionTypes.OpenUrl, "Open", null, "https://docs.microsoft.com/en-us/sql/sample/world-wide-importers")
                    }
                }
            };

            var responseMessage = context.MakeMessage();
            responseMessage.Attachments.Add(card.ToAttachment());

            await context.PostAsync(responseMessage);

            context.Done(stockitem);
        }
    }
}