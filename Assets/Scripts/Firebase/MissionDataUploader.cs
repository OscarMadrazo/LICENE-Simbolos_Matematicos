using UnityEngine;
using Firebase.Firestore;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections.Generic;

public class MissionDataUploader : MonoBehaviour
{
    private FirebaseFirestore db;
    private FirebaseAuth auth;

    [Header("Datos de la misión")]
    public string missionName = "mission_1";

    public int xp = 500;
    public int tiempo = 120;
    public int aciertos = 8;
    public int errores = 2;

    public string mundo = "Simbolos_Matematicos";

    void Start()
    {
        // Inicializar Firebase
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        Debug.Log("Firebase Firestore inicializado");

        // PRUEBA AUTOMÁTICA
        // Después puedes llamar SaveMissionData()
        // cuando realmente termine la misión
        SaveMissionData();
    }

    public void SaveMissionData()
    {
        Debug.Log("Intentando guardar datos de misión...");

        // Verificar login
        if (auth.CurrentUser == null)
        {
            Debug.LogError("No hay usuario autenticado");
            return;
        }

        // UID REAL DEL USUARIO
        string uid = auth.CurrentUser.UserId;

        // Correo del usuario
        string email = auth.CurrentUser.Email;

        Debug.Log("Usuario autenticado:");
        Debug.Log("Correo: " + email);
        Debug.Log("UID: " + uid);

        // ==================================================
        // REFERENCIA MISIÓN
        // users/UID/games/Simbolos_Matematicos/missions/mission_1
        // ==================================================

        DocumentReference missionRef =
            db.Collection("users")
              .Document(uid)
              .Collection("games")
              .Document("Simbolos_Matematicos")
              .Collection("missions")
              .Document(missionName);

        // DATOS DE LA MISIÓN
        Dictionary<string, object> missionData =
            new Dictionary<string, object>()
        {
            { "missionName", missionName },
            { "xp", xp },
            { "tiempo", tiempo },
            { "aciertos", aciertos },
            { "errores", errores },
            { "misionCompletada", true },
            { "mundo", mundo },
            { "correo", email },
            { "fecha", Timestamp.GetCurrentTimestamp() }
        };

        // GUARDAR MISIÓN
        missionRef.SetAsync(missionData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Misión guardada correctamente");
                }
                else
                {
                    Debug.LogError("Error guardando misión:");
                    Debug.LogError(task.Exception);
                }
            });

        // ==================================================
        // REFERENCIA STATS
        // users/UID/games/Simbolos_Matematicos/stats/general
        // ==================================================

        DocumentReference statsRef =
            db.Collection("users")
              .Document(uid)
              .Collection("games")
              .Document("Simbolos_Matematicos")
              .Collection("stats")
              .Document("general");

        // ACTUALIZAR STATS GLOBALES
        Dictionary<string, object> statsUpdates =
            new Dictionary<string, object>()
        {
            { "xpTotal", FieldValue.Increment(xp) },
            { "tiempoTotal", FieldValue.Increment(tiempo) },
            { "erroresTotales", FieldValue.Increment(errores) },
            { "aciertosTotales", FieldValue.Increment(aciertos) },
            { "misionesCompletadas", FieldValue.Increment(1) },
            { "ultimoJuego", mundo },
            { "ultimaActualizacion", Timestamp.GetCurrentTimestamp() }
        };

        // CREAR O ACTUALIZAR STATS
        statsRef.SetAsync(statsUpdates, SetOptions.MergeAll)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Stats actualizadas correctamente");
                }
                else
                {
                    Debug.LogError("Error actualizando stats:");
                    Debug.LogError(task.Exception);
                }
            });
    }
}