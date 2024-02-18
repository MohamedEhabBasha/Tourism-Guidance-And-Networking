using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism_Guidance_And_Networking.Core.Consts
{
    public class PaymentStatus
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string DelayedPayment = "ApprovedForDelayedPayment";
        public const string Rejected = "Rejected";
        public const string Cancelled = "Cancelled";
        public const string Refunded = "Refunded";
    }
}
