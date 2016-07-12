
namespace UpdMngr.Api
{
    public interface IReadOnlyAuthorizationCookie
    {
        byte[] CookieData { get; }
        string PlugInId { get; }
    }
}
