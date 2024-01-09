using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemText;

    public void InitPanel(string text)
    {
        itemText.text = text;
    }
}
