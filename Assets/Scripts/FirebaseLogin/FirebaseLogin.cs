using Google;
using Firebase.Auth;
using Unity;
using UnityEngine;
using System.Threading.Tasks;
public class FirebaseLogin : MonoBehaviour
{	// Auth �� instance
    FirebaseAuth auth = null;

    // ����� ����
    FirebaseUser user = null;

    // ��� ������ �Ǿ� �ִ� �������� üũ�Ѵ�.
    private bool signedIn = false;

    private void Awake()
    {

    }
    private void Start()
    {
        try
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            LoginLink(LoginState.GOOGLE_ACCOUNT);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
    
    }
    public void Inint()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        user.TokenAsync(true).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("TokenAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("TokenAsync encountered an error: " + task.Exception);
                return;
            }

            string idToken = task.Result;

            // Send token to your backend via HTTPS
            // ...
        });

    }

    public void LoginLink(LoginState _t)
    {
        switch (_t)
        {
            case LoginState.GOOGLE_ACCOUNT:
                GooleLogin();
                break;
            case LoginState.APPLE_ACCOUNT:
                break;
            case LoginState.EMAIL_ACCOUNT:
                break;
            default:
                break;
        }
    }

    public void GooleLogin()
    {
        if (GoogleSignIn.Configuration == null)
        {
            // ����
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                RequestEmail = true,
                // Copy this value from the google-service.json file.
                // oauth_client with type == 3
                WebClientId = "494831558708-2dq0fqt5ut11d37l24139nad54it8h04.apps.googleusercontent.com",
                RequestAuthCode = true,
                ForceTokenRefresh = true,
            };
        }

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();

        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Google Login task.IsCanceled");
            }
            else if (task.IsFaulted)
            {
                Debug.Log("Google Login task.IsFaulted");
            }
            else
            {
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                {
                    Debug.Log(authTask.Result);
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                        Debug.Log("Google Login authTask.IsCanceled");
                        return;
                    }
                    if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                        Debug.Log("Google Login authTask.IsFaulted");
                        return;
                    }

                    user = authTask.Result;
                    Debug.LogFormat("Google User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
                    return;
                });
            }
        });
    }    
    // ���� ����
    public void SignOut()
    {
        if (auth.CurrentUser != null)
            auth.SignOut();
    }

    // ���� ���� ����
    public void UserDelete()
    {
        if (auth.CurrentUser != null)
            auth.CurrentUser.DeleteAsync();
    }
}
