using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioSource audioSourceJugador;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    [Header("UI")]
    public GameObject panelPregunta;
    public TMP_Text textoPregunta;
    public TMP_Text textoResultado;
    public TMP_InputField inputRespuesta;

    [Header("Intercambio")]
    public GameObject panelIntercambio;

    private bool preguntaBonus = false;

    private int indiceActual;
    private string respuestaCorrecta;
    private GameObject fresaActual;

    private int preguntasRespondidas = 0;

    [Header("Preguntas")]
    public List<string> preguntas;
    public List<string> respuestas;

    void Start()
    {
        panelPregunta.SetActive(false);

        if (panelIntercambio != null)
        {
            panelIntercambio.SetActive(false);
        }
    }

    // =========================
    // MOSTRAR PREGUNTA NORMAL
    // =========================
    public void MostrarPregunta(GameObject fresa)
    {
        if (preguntas.Count == 0) return;

        preguntaBonus = false;

        fresaActual = fresa;

        int indice = Random.Range(0, preguntas.Count);

        indiceActual = indice;

        textoPregunta.text = preguntas[indice];
        respuestaCorrecta = respuestas[indice];

        inputRespuesta.text = "";
        textoResultado.text = "";

        panelPregunta.SetActive(true);

        Time.timeScale = 0f;
    }

    // =========================
    // PREGUNTA BONUS
    // =========================
    public void MostrarPreguntaBonus()
    {
        if (preguntas.Count == 0) return;

        preguntaBonus = true;

        int indice = Random.Range(0, preguntas.Count);

        indiceActual = indice;

        textoPregunta.text = preguntas[indice];
        respuestaCorrecta = respuestas[indice];

        inputRespuesta.text = "";
        textoResultado.text = "";

        panelPregunta.SetActive(true);

        Time.timeScale = 0f;
    }

    // =========================
    // VERIFICAR RESPUESTA
    // =========================
    public void VerificarRespuesta()
    {
        bool correcta =
            inputRespuesta.text.Trim().ToLower()
            == respuestaCorrecta.Trim().ToLower();

        // =========================
        // RESPUESTA CORRECTA
        // =========================
        if (correcta)
        {
            textoResultado.text = "Correcto";

            // SONIDO
            if (audioSourceJugador != null && sonidoCorrecto != null)
            {
                audioSourceJugador.PlayOneShot(sonidoCorrecto);
            }

            // BONUS
            if (preguntaBonus)
            {
                textoResultado.text =
                    "Correcto\nTransportando al siguiente nivel...";

                StartCoroutine(PasarDeNivel());
                return;
            }

            // ELIMINAR PREGUNTA USADA
            preguntas.RemoveAt(indiceActual);
            respuestas.RemoveAt(indiceActual);

            preguntasRespondidas++;

            GameManager.instance.PreguntaCorrecta();

            // DESTRUIR FRESA
            if (fresaActual != null)
            {
                Destroy(fresaActual);
            }

           
        }

        // =========================
        // RESPUESTA INCORRECTA
        // =========================
        else
        {
            textoResultado.text = "Incorrecto";

            // SONIDO
            if (audioSourceJugador != null && sonidoIncorrecto != null)
            {
                audioSourceJugador.PlayOneShot(sonidoIncorrecto);
            }

            // SI FALLA BONUS
            if (preguntaBonus)
            {
                textoResultado.text =
                    "Incorrecto\nReiniciando nivel...";

                StartCoroutine(ReiniciarNivel());
                return;
            }

            // ELIMINAR PREGUNTA FALLADA
            preguntas.RemoveAt(indiceActual);
            respuestas.RemoveAt(indiceActual);

            // DESTRUIR FRESA
            if (fresaActual != null)
            {
                Destroy(fresaActual);
            }
        }

        StartCoroutine(CerrarPregunta());
    }

    // =========================
    // PASAR NIVEL
    // =========================
    IEnumerator PasarDeNivel()
    {
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;

        int escenaActual = SceneManager.GetActiveScene().buildIndex;

        // NIVEL FINAL
        if (escenaActual == 5)
        {
            textoResultado.text =
                "¡Felicidades!\nSuperaste todo el juego";

            yield return new WaitForSecondsRealtime(3f);

            SceneManager.LoadScene("Menu");
        }
        else
        {
            SceneManager.LoadScene(escenaActual + 1);
        }
    }

    // =========================
    // REINICIAR NIVEL
    // =========================
    IEnumerator ReiniciarNivel()
    {
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;

        GameManager.instance.ReiniciarProgreso();

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    // =========================
    // CERRAR PANEL
    // =========================
    IEnumerator CerrarPregunta()
    {
        yield return new WaitForSecondsRealtime(2f);

        panelPregunta.SetActive(false);

        Time.timeScale = 1f;
    }
}
