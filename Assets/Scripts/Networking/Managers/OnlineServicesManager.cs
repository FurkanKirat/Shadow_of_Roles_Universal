using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Networking.Managers
{
    public static class OnlineServicesManager
    {
        public static async Task InitializeAndSignInAsync()
        {
            try
            {
                await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Unity Services init/sign-in failed: " + e);
            }
        }
    }
}