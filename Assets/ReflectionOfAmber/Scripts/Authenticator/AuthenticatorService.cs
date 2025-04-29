using System;
using ReflectionOfAmber.Scripts.GlobalProject;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Authenticator
{
    public class AuthenticatorService : IInit
    {
        public event Action OnReady;

        private IAuthenticator m_Authenticator;
        
        public void Init()
        {
            Debug.Log("AuthenticatorService::Init");
#if STEAM_GAME
            m_Authenticator = new SteamAuthenticator();
#endif
            if (m_Authenticator == null)
            {
                Debug.LogError("AuthenticatorService::Init - Authenticator is null");
                OnReady?.Invoke();
                return;
            }
            SignIn();
        }

        private async void SignIn()
        {
            try
            {
                if (AuthenticationService.Instance.SessionTokenExists)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    OnSignIn(true);
                }
                else
                {
                    await m_Authenticator.SignIn(OnSignIn);
                }
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }
        
        private void OnSignIn(bool isSuccess)
        {
            if (isSuccess)
            {
                Debug.Log($"SignIn is successful. ID: {AuthenticationService.Instance.PlayerId}");
                OnReady?.Invoke();
                return;
            }

            Application.Quit();
        }
    }
}