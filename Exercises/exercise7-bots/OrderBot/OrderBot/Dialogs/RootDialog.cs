using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using OrderBot.Data;

namespace OrderBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.Contains("order"))
            {
                context.Call(new StockItemDialog(), OnStockItemReceivedAsync);
            }
        }

        private async Task OnStockItemReceivedAsync(IDialogContext context, IAwaitable<Stockitem> result)
        {
            await context.PostAsync("Thank you!");

            context.Wait(MessageReceivedAsync);
        }
    }
}