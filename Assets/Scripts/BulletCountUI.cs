using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletCountUI : MonoBehaviour
{
    TextMeshProUGUI countText;
    private int lastCount;

    void Awake()
    {
        countText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCount(int amount)
    {
        lastCount = amount;
        countText.text = amount.ToString();
    }

    public void IncreaseCount(int amount)
    {
        countText.text = (amount + lastCount).ToString();
    }
}
