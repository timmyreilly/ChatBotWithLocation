﻿using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Microsoft.Bot.Sample.EchoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null)
            {
                switch (activity.GetActivityType())
                {
                    case ActivityTypes.Message:
                        if (activity.ChannelId.ToLower() == "emulator")
                        {
                            await Conversation.SendAsync(activity, () => new RootDialog(activity.ChannelId)); 
                        }
                        //await Conversation.SendAsync(activity, () => new DeliverDialog(activity.ChannelId));
                        // await Conversation.SendAsync(activity, () => new EchoDialog();
                        break;

                    case ActivityTypes.ContactRelationUpdate:
                    case ActivityTypes.Typing:
                    case ActivityTypes.DeleteUserData:
                    case ActivityTypes.Ping:
                    default:
                        Trace.TraceError($"Unknown activity type ignored: {activity.GetActivityType()}");
                        break;
                }

                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
        ///// <summary>
        ///// POST: api/Messages
        ///// receive a message from a user and send replies
        ///// </summary>
        ///// <param name="activity"></param>
        //[ResponseType(typeof(void))]
        //public virtual async Task<HttpResponseMessage> PostOld([FromBody] Activity activity)
        //{
        //    if (activity != null)
        //    {
        //        // one of these will have an interface and process it
        //        switch (activity.GetActivityType())
        //        {
        //            case ActivityTypes.Message:
        //                //await Conversation.SendAsync(activity, () => new EchoDialog());
        //                //await Conversation.SendAsync(activity, () => EchoCommandDialog.dialog);
        //                //await Conversation.SendAsync(activity, () => new EchoAttachmentDialog());
        //                //await Conversation.SendAsync(activity, () => EchoChainDialog.dialog);
        //                //await Conversation.SendAsync(activity, () => new EchoLocationDialog()); 
        //                await Conversation.SendAsync(activity, () => new DeliverDialog());
        //                break;

        //            case ActivityTypes.ConversationUpdate:
        //                IConversationUpdateActivity update = activity;
        //                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
        //                {
        //                    var client = scope.Resolve<IConnectorClient>();
        //                    if (update.MembersAdded.Any())
        //                    {
        //                        var reply = activity.CreateReply();
        //                        foreach (var newMember in update.MembersAdded)
        //                        {
        //                            if (newMember.Id != activity.Recipient.Id)
        //                            {
        //                                reply.Text = $"Welcome {newMember.Name}!";
        //                            }
        //                            else
        //                            {
        //                                reply.Text = $"Welcome {activity.From.Name}";
        //                            }
        //                            await client.Conversations.ReplyToActivityAsync(reply);
        //                        }
        //                    }
        //                }
        //                break;
        //            case ActivityTypes.ContactRelationUpdate:
        //            case ActivityTypes.Typing:
        //            case ActivityTypes.DeleteUserData:
        //            case ActivityTypes.Ping:
        //            default:
        //                Trace.TraceError($"Unknown activity type ignored: {activity.GetActivityType()}");
        //                break;
        //        }
        //    }
        //    return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        //}
    }
}
