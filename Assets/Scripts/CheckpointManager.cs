using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private Vector3 currentCheckpointPosition;
    private bool hasCheckpoint = false;

    void Awake()
    {
        Debug.Log("CheckpointManager Awake llamado");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("CheckpointManager instanciado correctamente");

            // Buscar jugador para checkpoint inicial
            SetInitialCheckpoint();
        }
        else
        {
            Debug.Log("CheckpointManager duplicado - destruyendo");
            Destroy(gameObject);
        }
    }

    private void SetInitialCheckpoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SetCheckpoint(player.transform.position);
            Debug.Log("Checkpoint inicial establecido en posición del jugador: " + player.transform.position);
        }
        else
        {
            // Si no hay jugador, establecer en (0,0,0) o donde prefieras
            SetCheckpoint(Vector3.zero);
            Debug.Log("Checkpoint inicial establecido en Vector3.zero");
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        currentCheckpointPosition = position;
        hasCheckpoint = true;
        Debug.Log("Nuevo checkpoint establecido en: " + position);
    }

    public Vector3 GetCurrentCheckpoint()
    {
        return currentCheckpointPosition;
    }

    public bool HasCheckpoint()
    {
        return hasCheckpoint;
    }
}
