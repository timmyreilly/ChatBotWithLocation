using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.Bot.Sample.EchoBot
{
    public class RootDialog : IDialog<Object>
    {
        private readonly string channelId;
        private Models.Order order;

        public RootDialog(string channelId)
        {
            this.channelId = channelId;
            this.order = null; 
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text.ToLower().Contains("help"))
            {
                await context.PostAsync("Say bacon to start your order"); 
            }
            else if(message.Text.ToLower().Contains("bacon"))
            {
                //await context.Call(new DeliverDialog(), this.ResumeAfterLocationCapture, message, CancellationToken.None);
                this.order = new Models.Order(); 
                var addressDialog = new DeliverDialog(channelId, this.order); 
                context.Call(addressDialog, this.AfterDeliveryAddress());
                //await context.Wait(new DeliverDialog(), new ContinueOrderDialog(), message, CancellationToken.None); 
             
            }
            else {
                this.ShowOptions(context);
            }
        }

        private async Task<ResumeAfter<object>> AfterDeliveryAddress(IDialogContext context, IAwaitable<Place> result)
        {
            try
            {
                var msg = await result;
                order.Postal = msg.GetPostalAddress();
                await context.PostAsync("Got your order"); 
            }
            catch (TooManyAttemptsException)
            {
                await WelcomeMessageAsync(context);
            }

            context.Done<Place>(null);
        }

        //private async Task AfterDeliveryAddress(IDialogContext context, IAwaitable<Place> result)
        //{
        //    try
        //    {
        //        var msg = await result;
        //        order.Postal = msg.GetPostalAddress();
        //    }
        //    catch (TooManyAttemptsException)
        //    {
        //        await WelcomeMessageAsync(context);
        //    }

        //    context.Done<Place>(null);
        //}

        private async Task WelcomeMessageAsync(IDialogContext context)
        {
            //var reply = context.MakeMessage();

            await context.PostAsync("Welcome to bacon!"); 
        }

        private void ShowOptions(IDialogContext context)
        {
            //PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { FlightsOption, HotelsOption }, "Are you looking for a flight or a hotel?", "Not a valid option", 3);
            context.PostAsync($"You're confused? Just enter 'bacon' to start your order, or 'help' to find more info");
        }

        
    }
}