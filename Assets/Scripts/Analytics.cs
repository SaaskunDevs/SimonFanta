using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Analytics : MonoBehaviour
{
    private float tiempoDeJuego;
    private int vecesJugadas;
    private int nivelMaximo;
    private int premiosDados;
    public Text tiempoDeJuegoTxt;
    public Text vecesJugadasTxt;
    public Text nivelMaximoTxt;
    public Text premiosDadosTxt;

    private bool isPlaying;
    // Start is called before the first frame update
    void Start()
    {
        tiempoDeJuego = PlayerPrefs.GetFloat("_PLAYTIME");
        vecesJugadas = PlayerPrefs.GetInt("_TOTALPLAY");
        nivelMaximo = PlayerPrefs.GetInt("_MAXLVL");
        premiosDados = PlayerPrefs.GetInt("_GIFTS");
        ShowData();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
            tiempoDeJuego += Time.deltaTime;
    }

    public void StartGame()
    {
        isPlaying = true;
    }

    public void PlayerFinish(int score, bool ganoPremio)
    {
        isPlaying = false;
        vecesJugadas++;
        if (ganoPremio)
            premiosDados++;
        if (score > nivelMaximo)
            nivelMaximo = score;
        PlayerPrefs.SetInt("_TOTALPLAY", vecesJugadas);
        PlayerPrefs.SetInt("_MAXLVL", nivelMaximo);
        PlayerPrefs.SetInt("_GIFTS", premiosDados);
        PlayerPrefs.SetFloat("_PLAYTIME", tiempoDeJuego);

        ShowData();
    }

    void ShowData()
    {
        tiempoDeJuegoTxt.text = "Tiempo de juego: " + tiempoDeJuego.ToString("F2") + " segundos";
        vecesJugadasTxt.text = "Veces jugadas: " + vecesJugadas;
        nivelMaximoTxt.text = "Nivel máximo: " + nivelMaximo;
        premiosDadosTxt.text = "Premios entregados: " + premiosDados;
    }
}
