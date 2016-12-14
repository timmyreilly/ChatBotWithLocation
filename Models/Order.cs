using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Location;


namespace Microsoft.Bot.Sample.EchoBot.Models
{
    [Serializable]
    public class Order
    {

        public string OrderID { get; set; }
        public string DeliveryAddress { get; set; }
        public PostalAddress Postal { get; set; }   
    }
}