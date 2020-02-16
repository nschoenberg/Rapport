namespace Rapport.Contracts
{
    public interface IJiraServiceFactory
    {
        IJiraService Create(string loginUsername, string loginPassword);
    }
}
