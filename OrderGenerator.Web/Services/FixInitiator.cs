using OrderGenerator.Web.Services.Interfaces;
using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;
using QuickFix.Logger;
using QuickFix.Store;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace OrderGenerator.Web.Services
{
    public class FixInitiator : MessageCracker, IApplication, IFixInitiator
    {
        private Session? _session;
      

        public void OnCreate(SessionID sessionId)
        {
            {
                _session = Session.LookupSession(sessionId);
                if (_session is null)
                    throw new ApplicationException("Somehow session is not found");
            }
        }
            
        public void OnLogon(SessionID sessionID) => Console.WriteLine($"[Initiator] Conectado: {sessionID}");
        public void OnLogout(SessionID sessionID) => Console.WriteLine($"[Initiator] Desconectado: {sessionID}");
        public void ToAdmin(QuickFix.Message message, SessionID sessionID) { }
        public void FromAdmin(QuickFix.Message message, SessionID sessionID) { }
        public void ToApp(QuickFix.Message message, SessionID sessionID) => Console.WriteLine($"[Initiator] Enviado: {message}");
        public void FromApp(QuickFix.Message message, SessionID sessionID) => Crack(message, sessionID);

        public void SendNewOrder(string symbol, string side, int quantity, decimal price)
        {
                if (_session == null)
                {
                    Console.WriteLine("Sessão FIX não está ativa. Ordem não enviada.");
                    return;
                }
               
            var order = new NewOrderSingle(
                new ClOrdID(Guid.NewGuid().ToString()),
                new Symbol(symbol),
                new Side(side == "Compra" ? Side.BUY : Side.SELL),
                new TransactTime(DateTime.UtcNow),
                new OrdType(OrdType.LIMIT));

            order.SetField(new OrderQty(quantity));
            order.SetField(new Price(price));

            if (_session == null)
{
                Console.WriteLine("Sessão FIX não está ativa. Ordem não enviada.");
                return;
}

            _session.Send(order);
        }
    }
}
