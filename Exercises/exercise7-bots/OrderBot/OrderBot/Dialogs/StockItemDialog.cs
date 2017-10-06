using Microsoft.Bot.Builder.Dialogs;
using OrderBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace OrderBot.Dialogs
{
    [Serializable]
    public class StockItemDialog : IDialog<Stockitem>
    {
        public Task StartAsync(IDialogContext context)
        {
            var dialog = new PromptDialog.PromptString("What do you want to order?", "Please tell me the name of the product.", 3);

            context.Call(dialog, OnStockitemNameReceivedAsync);

            return Task.CompletedTask;
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
                Text = $"Buy now for {stockitem.RecommendedRetailPrice} $!",
                Images = new List<CardImage>()
                {
                    new CardImage()
                    {
                        Url = stockitem.ImageUrl,
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