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
        officeName.text = office.사무소명;
        roadAddress.text = office.도로명주소;
        shingoType.text = office.신고구분;
        yearNumber.text = office.연번.ToString();
        businessType.text = office.영업구분;
        phoneNumber.text = office.전화번호;
    }

    public void OnClickCloseButton()
    {
        Destroy(gameObject);
    }
}
