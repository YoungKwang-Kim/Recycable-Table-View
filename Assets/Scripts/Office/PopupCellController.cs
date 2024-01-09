using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupCellController : MonoBehaviour
{
    [SerializeField] UpdateCellController updateCellPrefab;

    [SerializeField] TextMeshProUGUI officeName;
    [SerializeField] TextMeshProUGUI roadAddress;
    [SerializeField] TextMeshProUGUI shingoType;
    [SerializeField] TextMeshProUGUI yearNumber;
    [SerializeField] TextMeshProUGUI businessType;
    [SerializeField] TextMeshProUGUI phoneNumber;

    void Awake()
    {
        officeName.text = "";
        roadAddress.text = "";
        shingoType.text = "";
        yearNumber.text = "";
        businessType.text = "";
        phoneNumber.text = "";
    }

    public void SetPopupData(Office office)
    {
        officeName.text = office.�繫�Ҹ�;
        roadAddress.text = office.���θ��ּ�;
        shingoType.text = office.�Ű���;
        yearNumber.text = office.����.ToString();
        businessType.text = office.��������;
        phoneNumber.text = office.��ȭ��ȣ;
    }

    public void OnClickCloseButton()
    {
        Destroy(gameObject);
    }
}
