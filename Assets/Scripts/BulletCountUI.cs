using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletCountUI : MonoBehaviour
{
    TextMeshProUGUI countText;

    void Awake()
    {
        countText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCount(int amount)
    {
        countText.text = amount.ToString();
    }
}
