using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                var status = task.Result;

                if (status == DependencyStatus.Available)
                {
                    Debug.Log("Firebase conectado correctamente");
                }
                else
                {
                    Debug.LogError("Firebase ERROR: " + status);
                }
            });
    }
}