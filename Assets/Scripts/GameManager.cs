
using UnityEngine;
using TMPro; // Esto es para que reconozca el texto

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Configuración del Juego")]
    public int monedas = 0;
    public int preguntasCorrectas = 0;
    public int preguntasNecesarias = 4; // Lo cambié a 5 para que coincida con tus preguntas

    [Header("Referencias de UI")]
    public TMP_Text textoMonedas; // AQUÍ ARRASTRARÁS EL OBJETO "00"

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Quitamos el DontDestroyOnLoad para evitar problemas de duplicados entre niveles por ahora
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ActualizarInterfaz();
    }

    public void SumarMoneda(int cantidad)
    {
        monedas += cantidad;
        ActualizarInterfaz();
    }

    public void PreguntaCorrecta()
    {
        preguntasCorrectas++;
    }

    // Nueva función para que el número cambie en la pantalla
    void ActualizarInterfaz()
    {
        if (textoMonedas != null)
        {
            textoMonedas.text = monedas.ToString("00"); // Muestra 01, 02, etc.
        }
    }

    public bool PuedePasar() => preguntasCorrectas >= preguntasNecesarias;
    public bool PuedeIntercambiar() => monedas >= 10;
    public void Intercambiar() { monedas -= 10; ActualizarInterfaz(); }
    public void ReiniciarProgreso() { monedas = 0; preguntasCorrectas = 0; ActualizarInterfaz(); }
}
