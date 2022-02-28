namespace DutchTreat.Interfaces
{
    public interface IMailService
    {
        void SendMessage(string to, string from, string subject, string body);
    }
}