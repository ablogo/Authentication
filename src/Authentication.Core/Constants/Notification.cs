namespace Authentication.Core.Constants
{
    public class Notification
    {
        public enum Types
        {
            EmailWelcome = 1,
            EmailConfirmationAccount,
            EmailRecoverPassword,
            SmsConfirmation,
            SubscriptionCreate,
            SubscriptionUpdate,
            SubscriptionPause,
            SubscriptionReactivate,
            SubscriptionCancel,
            ChangePaymentMethod,
            OrderCreate,
            OrderUpdate,
            OrderUpdateAddress,
            OrderCancel,
            OrderRefunded,
            OrderPaymentStatus,
            OrderDelivery,
            OrderPendingPayment
        }

    }
}
