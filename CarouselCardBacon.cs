using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Sample.EchoBot
{
    public class CarouselCardBacon: IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsAttachments();

            await context.PostAsync(reply);

            context.Wait(this.MessageReceivedAsync);
        }

        public static IList<Attachment> GetCardsAttachments()
        {
            return new List<Attachment>()
            {
                GetHeroCard(
                    "Bacon",
                    "A Pound of Bacon - Cooked",
                    "$30",
                    new CardImage(url: "https://example.com/"),
                    new CardAction(ActionTypes.OpenUrl, "Order", value: "http://bing.com")),
                GetHeroCard(
                    "Bacon",
                    "A Pound of Bacon - Uncooked",
                    "$25",
                    new CardImage(url: "https://example.com/"),
                    new CardAction(ActionTypes.PostBack, "uncooked", value: "http://bing.com")),

            };
        }

        public static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction )
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment(); 
        }
    }
}