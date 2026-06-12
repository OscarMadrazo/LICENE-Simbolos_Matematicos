using UnityEngine;
using DialogueEditor;

public class DialogueCursorController : MonoBehaviour
{
    [Header("Script de movimiento del jugador")]
    [SerializeField] private MonoBehaviour playerMovementScript;

    private void OnEnable()
    {
        ConversationManager.OnConversationStarted += OnConversationStarted;
        ConversationManager.OnConversationEnded += OnConversationEnded;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationStarted -= OnConversationStarted;
        ConversationManager.OnConversationEnded -= OnConversationEnded;
    }

    private void OnConversationStarted()
    {
        Debug.Log("Diálogo iniciado");

        // Mostrar cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Desactivar movimiento
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
    }

    private void OnConversationEnded()
    {
        Debug.Log("Diálogo terminado");

        // Ocultar cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Reactivar movimiento
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }
}