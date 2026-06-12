using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text statusText;

    [Header("Canvas")]
    public GameObject loginCanvas;

    [Header("Jugador")]
    public GameObject playerController;

    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // Mostrar cursor mientras el login está activo
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Desactivar jugador temporalmente
        if (playerController != null)
            playerController.SetActive(false);
    }

    public void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        statusText.text = "Iniciando sesión...";

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError("Error login: " + task.Exception);

                    statusText.text = "Correo o contraseña incorrectos";
                    return;
                }

                FirebaseUser user = task.Result.User;

                Debug.Log("Login exitoso");
                Debug.Log("UID: " + user.UserId);
                Debug.Log("Correo: " + user.Email);

                statusText.text = "Bienvenido " + user.Email;

                // Guardar datos localmente
                PlayerPrefs.SetString("USER_UID", user.UserId);
                PlayerPrefs.SetString("USER_EMAIL", user.Email);

                PlayerPrefs.Save();

                Debug.Log("Ocultando Canvas Login...");

                // Destruir canvas completamente
                if (loginCanvas != null)
                {
                    Destroy(loginCanvas);
                }

                // Reactivar jugador
                if (playerController != null)
                {
                    playerController.SetActive(true);
                }

                // Restaurar cursor del juego
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            });
    }
}