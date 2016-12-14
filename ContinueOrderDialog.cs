using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Sample.EchoBot
{
    [Serializable]
    public class ContinueOrderDialog : IDialog<object>
    {
        
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Let's get you some bacon!");

            
        }
    }
}