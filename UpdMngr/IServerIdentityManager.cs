
namespace UpdMngr
{
    public interface IServerIdentityManager
    {
        IReadOnlyAuthorizationCookie AuthorizationCookie { get; }
        string ServerId { get; }
        string ServerName { get; }

        void RegisterAuthorizationCookieData(string pluginId, byte[] data);
    }
}
