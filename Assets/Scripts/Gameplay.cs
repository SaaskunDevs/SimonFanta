using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public Analytics analytics;
    public Manager manager;
    public TouchBehavour touchBehavour;
    public Toucher[] toucher; // Botones que brillan alv
    private int touchers;
    private int dificulty = 3;
    private int index; //referencia
    private int checkIndex; //index para revisar si esta bien
    private int level = 1; // nivel alcanzado
    private int[] memory = new int[50]; //Memoria del simon dice

    private float actualTime; //Tiempo actual para cambio de toucher
    private float toucherChangeTime = 0.8f; // tiempo para cambiar toucher
    public float minToucherTime = 0.25f;


    private bool start;
    private bool canPlay;

    /// <summary>
    /// Inicializa los touchers y la memoria
    /// </summary>
    void Start()
    {
        touchers = toucher.Length;
        for (int i = 0; i < touchers; i++)
        {
            toucher[i].toucherIndex = i;
        }
        

        SetNewMemory();
    }

    /// <summary>
    /// Timing para el sig toucher
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            StartGame();

        if (start)
            ToucherTiming();
    }

    void SetNewMemory()
    {
        memory = new int[50];
        for (int i = 0; i < memory.Length; i++)
        {
            int ran = Random.Range(0, touchers);
            memory[i] = ran;
       //     Debug.Log(memory[i]);
        }
        
    }

    void ToucherTiming()
    {
        actualTime += Time.deltaTime;
        if (actualTime >= toucherChangeTime)
            NextTouch();
    }

    void NextTouch()
    {
        actualTime = 0;
        Debug.Log("NExt touch, index " + index + " value " + memory[index]);
        toucher[memory[index]].Bip();
        index++;
        
        if (index >= dificulty)
        {
            Debug.Log("Ya puede jugar, dificultad " + dificulty);
            start = false;
            canPlay = true;
            touchBehavour.canTouch = true;
        }
    }

    public void CheckTouch(int index)
    {
        if (!canPlay)
        {
            Debug.Log("no puede jugar aun");
            return;
        }
        Debug.Log("Index " + index + " memoria " + checkIndex + " : " + memory[checkIndex]);

        if (memory[checkIndex] == index)
        {
            checkIndex++;
            Good();
        }
        else
            Bad();

        
    }


    void Good()
    {
        Debug.Log("Bien " + checkIndex);
        if (checkIndex >= dificulty)
        {
            Win();
        }
    }

    void Bad()
    {
        Debug.Log("Mal " + checkIndex);
        Lost();
    }

    void Win()
    {
        Debug.Log("WIN , next dificulty");
        NextDificulty();
    }

    void Lost()
    {
        manager.PlayerFinish(dificulty - 1);
        Debug.Log("Lost");
        Debug.Log("LLEgo a nivel " + level);
        ResetParams();
    }

    void NextDificulty()
    {
        level++;
        dificulty++;
        index = 0;
        canPlay = false;
        touchBehavour.canTouch = false;
        start = false;
        actualTime = 0;
        checkIndex = 0;
        toucherChangeTime -= 0.2f;
        if (toucherChangeTime <= minToucherTime)
            toucherChangeTime = minToucherTime;

        StartCoroutine(WaitForNextDif());
    }

    IEnumerator WaitForNextDif()
    {
        yield return new WaitForSeconds(0.5f);
        start = true;
    }

    void Restart()
    {
        canPlay = false;
        touchBehavour.canTouch = false;
        start = false;
        SetNewMemory();
    }

    public void StartGame()
    {
        start = true;
        analytics.StartGame();
        Debug.Log("Iniciando el juego");
    }

    void ResetParams()
    {
        level = 1;
        dificulty = 3;
        index = 0;
        canPlay = false;
        touchBehavour.canTouch = false;
        start = false;
        actualTime = 0;
        checkIndex = 0;
        toucherChangeTime = 1.2f;
        SetNewMemory();
    }
}
