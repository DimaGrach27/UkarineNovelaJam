using System;
using System.Threading.Tasks;

namespace ReflectionOfAmber.Scripts.Authenticator
{
    public interface IAuthenticator
    {
        public Task SignIn(Action<bool> callback);
    }
}