namespace SEP.WebShop.Web.RabbitMQ
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message, string queue, string port);
    }
}
