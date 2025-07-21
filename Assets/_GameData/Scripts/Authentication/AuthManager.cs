using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class AuthManager : MonoBehaviour
{
    public static FirebaseAuth Auth;
    private FirebaseUser user;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Auth Initialized");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public void SignInWithGoogle(string idToken, string accessToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        Auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Sign-in failed: " + task.Exception);
                return;
            }

            user = task.Result;
            Debug.Log("Signed in successfully: " + user.DisplayName);
        });
    }
}
