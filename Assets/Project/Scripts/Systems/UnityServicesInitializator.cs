using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Systems
{
    public class UnityServicesInitializator
    {
        public async UniTask SetupAndSignIn()
        {
            await UnityServices.InitializeAsync();
            Debug.Log("UnityCloudSaveRepository::SetupAndSignIn");
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("UnityCloudSaveRepository::SetupAndSignIn");
        }
    }
}