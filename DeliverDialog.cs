using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Location;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Microsoft.Bot.Sample.EchoBot.Models;

namespace Microsoft.Bot.Sample.EchoBot
{
    [Serializable]
    public class DeliverDialog: IDialog<object>
    {
        private readonly string channelId;
        private Order order;

        public DeliverDialog(string channelId, Order order)
        {
            this.channelId = channelId;
            this.order = order;
        }


        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }
        
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var apiKey = WebConfigurationManager.AppSettings["BingMapsApiKey"];
            var options = LocationOptions.UseNativeControl | LocationOptions.ReverseGeocode;

            var requiredFields = LocationRequiredFields.StreetAddress | LocationRequiredFields.Locality |
                                 LocationRequiredFields.Region | LocationRequiredFields.Country |
                                 LocationRequiredFields.PostalCode;

            var prompt = "Where should I ship your order?";

            var locationDialog = new LocationDialog(apiKey, this.channelId, prompt, options, requiredFields);

            context.Call(locationDialog, this.ResumeAfterLocationDialogAsync);
        }

        private async Task ResumeAfterLocationDialogAsync(IDialogContext context, IAwaitable<Place> result)
        {
            var place = await result;

            if (place != null)
            {
                var address = place.GetPostalAddress();
                this.order.Postal = place.GetPostalAddress(); 
                var formatteAddress = string.Join(", ", new[]
                {
                        address.StreetAddress,
                        address.Locality,
                        address.Region,
                        address.PostalCode,
                        address.Country
                    }.Where(x => !string.IsNullOrEmpty(x)));

                await context.PostAsync("Thanks, I will ship it to " + formatteAddress);
            }

            context.Done<Place>(place);
        }
    }
}

