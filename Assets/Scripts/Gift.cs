using UnityEngine;
using TMPro;
using System.Collections;

public class Gift : MonoBehaviour
{
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI cantidadTxt;
    public TMP_InputField nombreInput;
    private int index;
    private string productName;
    private int scoreToObtain;
    private int availableQuanity;


    private GiftValues giftValues;

    public void InitializeData(GiftValues giftVal)
    {
        this.index = giftVal.index;
        productName = giftVal.name;
        scoreToObtain = giftVal.score;
        availableQuanity = giftVal.quantity;
        giftValues = giftVal;
        SetData();
    }

    void SetData()
    {
        nombreInput.text = productName;
        scoreTxt.text = scoreToObtain.ToString();
        cantidadTxt.text = availableQuanity.ToString();

        giftValues.name = productName;
        giftValues.score = scoreToObtain;
        giftValues.quantity = availableQuanity;
    }

    public void ChangeName()
    {
        productName = nombreInput.text.ToUpper();
        SetData();
    }

    public void MoreScore()
    {
        scoreToObtain++;
        SetData();
    }

    public void LessScore()
    {
        scoreToObtain--;
        SetData();
    }

    public void MoreQuantity()
    {
        availableQuanity++;
        SetData();
    }

    public void LessQuantity()
    {
        availableQuanity--;
        SetData();
    }

    public GiftValues GetValues()
    {
        return giftValues;
    }
}
