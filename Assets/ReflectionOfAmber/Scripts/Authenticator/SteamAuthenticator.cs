using System;
using System.Threading.Tasks;
using Steamworks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Authenticator
{
    public class SteamAuthenticator : IAuthenticator
    {
        // private const string IDENTITY = "unity";
        private const string IDENTITY = "unityauthenticationservice";

        public async Task SignIn(Action<bool> callback)
        {
            var task = await GetToken();

            try
            {
                Debug.Log("Start init to authenticate Steam");
                
                await AuthenticationService.Instance.SignInWithSteamAsync(task, IDENTITY);
                Debug.Log("SignIn in Steam is successful.");

                callback?.Invoke(true);
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
        
        private async Task<string> GetToken()
        {
            Debug.Log("GetToken authenticate Steam");
            // var ticket = await SteamUser.GetAuthSessionTicketAsync(NetIdentity.LocalHost);
            var ticket = await SteamUser.GetAuthTicketForWebApiAsync(IDENTITY);
            
            Debug.Log(ticket.Handle);
            string token = BitConverter.ToString(ticket.Data);
            token = token.Replace("-", string.Empty);
            Debug.Log(token);

            return token;
        }
    }
}