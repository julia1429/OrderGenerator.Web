namespace OrderGenerator.Web.Services.Interfaces
{
    public interface IFixInitiator
    {
        public void SendNewOrder(string symbol, string side, int quantity, decimal price);
    }
}
