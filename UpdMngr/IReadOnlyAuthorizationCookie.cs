
namespace UpdMngr
{
    public interface IReadOnlyAuthorizationCookie
    {
        byte[] CookieData { get; }
        string PlugInId { get; }
    }
}
