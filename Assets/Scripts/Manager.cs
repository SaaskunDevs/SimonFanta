using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class Manager : MonoBehaviour
{
    public Analytics analytics;
    public Gift[] gifts =  new Gift[5];
    private GiftValues[] giftValues = new GiftValues[5];

    private string fileName = "/data.json";
    // Start is called before the first frame update

    private string jsonDataSaved;
    public TextAsset initialJsonData;
    JSONObject json;

    int panelIndex;
    int closeIndex;
    public GameObject introPanel;
    //Camel
    public GameObject lostCamelPanel;
    public GameObject lostCamelPanelNoGift;
    public TextMeshProUGUI sequenceCamelTxt;
    public TextMeshProUGUI giftCamelTxt;
    //Winston
    public GameObject lostWinstonPanel;
    public GameObject lostWinstonPanelNoGift;
    public TextMeshProUGUI sequenceWinstonTxt;
    public TextMeshProUGUI giftWinstonTxt;


    //--config
    public GameObject configGO;
    public GameObject analyticsGO;
    private int configIndex;
    private int analyticsIndex;

    void Start()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            Debug.Log("El archivo existe " + Application.persistentDataPath + fileName);
            jsonDataSaved = File.ReadAllText(Application.persistentDataPath + fileName);
            json = new JSONObject(jsonDataSaved);
        }
        else
        {
            json = new JSONObject();
            Debug.Log("No existe " + Application.persistentDataPath + fileName);
            InitialSave();
        }


        Debug.Log(json);
        LoadData();
    }


    void InitialSave()
    {
        json = new JSONObject(initialJsonData.text);
        File.WriteAllText(Application.persistentDataPath + fileName, initialJsonData.text);
        Debug.Log("Guardando datos en " + Application.persistentDataPath + fileName);
    }

    void LoadData()
    {
        Debug.Log("Cargando data del json en la clase");
        for (int i = 0; i < gifts.Length; i++)
        {
            giftValues[i] = new GiftValues
            {
                index = (int)json[i]["Index"].n,
                name = json[i]["Name"].str,
                score = (int)json[i]["Score"].n,
                quantity = (int)json[i]["Quantity"].n
            };
            gifts[i].InitializeData(giftValues[i]);
        }
    }

    public void SaveChanges()
    {
        for (int i = 0; i < gifts.Length; i++)
        {
            giftValues[i] = gifts[i].GetValues();
        }

        SaveData();
    }

    public void PlayerFinish(int score)
    {
        

        int index = -1;
        for (int i = 0; i < giftValues.Length; i++)
        {
            if(giftValues[i].score <= score)
            {
                index = i;
            }
        }

        Debug.Log("Player finish " + score + " index " + index);



        CheckQuantityAndShowPrice(index, score);

        
    }


    void CheckQuantityAndShowPrice(int index, int score)
    {
        if (index >= 0 && giftValues[index].quantity <= 0)
        {
    //        Debug.Log("No tiene " + giftValues[index].name + ", cambiando a " + giftValues[index - 1].name);
            index--;

            CheckQuantityAndShowPrice(index, score);
            return;
        }

            

        if (panelIndex == 0)
        {
            lostCamelPanel.SetActive(true);
            sequenceCamelTxt.text = score.ToString();
            if (index >= 0)
            {
                lostCamelPanel.SetActive(true);
                giftCamelTxt.text = giftValues[index].name;
            }
            else
                lostCamelPanelNoGift.SetActive(true);
            panelIndex = 1;
        }
        else
        {
            
            sequenceWinstonTxt.text = score.ToString();
            if (index >= 0)
            {
                lostWinstonPanel.SetActive(true);
                giftWinstonTxt.text = giftValues[index].name;
            }
            else
                lostWinstonPanelNoGift.SetActive(true);
            panelIndex = 0;
        }

        if (index >= 0)
        {
            giftValues[index].quantity--;
            gifts[index].InitializeData(giftValues[index]);
            analytics.PlayerFinish(score, true);
        }
        else
            analytics.PlayerFinish(score, false);

        SaveData();
    }

    public void SaveData()
    {
        json = new JSONObject();

        for (int i = 0; i < giftValues.Length; i++)
        {
            JSONObject newdat = new JSONObject(JSONObject.Type.OBJECT);
            newdat.AddField("Index", giftValues[i].index);
            newdat.AddField("Name", giftValues[i].name.ToString());
            newdat.AddField("Score", giftValues[i].score);
            newdat.AddField("Quantity", giftValues[i].quantity);
            json.Add(newdat);
        }
       

        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + fileName, json.ToString());

        Debug.Log("Guardado Listo");
    }

    public void PanelClick()
    {
        closeIndex++;
        if(closeIndex >= 3)
        {
            closeIndex = 0;
            lostCamelPanel.SetActive(false);
            lostWinstonPanel.SetActive(false);
            lostCamelPanelNoGift.SetActive(false);
            lostWinstonPanelNoGift.SetActive(false);
            introPanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        configIndex = 0;
        analyticsIndex = 0;
    }

    public void ConfigClick()
    {
        configIndex++;
        if (configIndex >= 6)
        {
            configIndex = 0;
            configGO.SetActive(true);
        }
    }

    public void AnalyticsClick()
    {
        analyticsIndex++;
        if(analyticsIndex >= 10)
        {
            analyticsIndex = 0;
            analyticsGO.SetActive(true);
        }
    }
}

public class GiftValues
{
    public int index;
    public int score;
    public int quantity;
    public string name;
}
