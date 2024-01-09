using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 1. delegate
public delegate void UpdateData(Office office, int index);

public class UpdateCellController : MonoBehaviour
{
    [SerializeField] private TMP_InputField officeNameInputField;
    [SerializeField] private TMP_InputField roadAddressInputField;
    [SerializeField] private TMP_InputField shingoTypeInputField;
    [SerializeField] private TMP_InputField yearNumberInputField;
    [SerializeField] private TMP_InputField businessTypeInputField;
    [SerializeField] private TMP_InputField phoneNumberInputField;

    // 1. delegate
    public UpdateData updateData;

    private int dataIndex;


    private void Awake()
    {
        roadAddressInputField.text = "";
        officeNameInputField.text = "";
        shingoTypeInputField.text = "";
        yearNumberInputField.text = "";
        businessTypeInputField.text = "";
        phoneNumberInputField.text = "";
    }

    /// <summary>
    /// 데이터를 불러오는 메서드
    /// </summary>
    /// <param name="office">데이터</param>
    /// <param name="index">데이터의 인덱스</param>
    public void SetPopupData(Office office, int index)
    {
        roadAddressInputField.text = office.도로명주소;
        officeNameInputField.text = office.사무소명;
        shingoTypeInputField.text = office.신고구분;
        yearNumberInputField.text = office.연번.ToString();
        businessTypeInputField.text = office.영업구분;
        phoneNumberInputField.text = office.전화번호;
    }

    /// <summary>
    /// 저장 버튼 클릭시 실행할 메서드
    /// </summary>
    public void OnclickUpdateButton()
    {
        Office office = new Office(roadAddressInputField.text, 
                officeNameInputField.text,
                shingoTypeInputField.text,
                int.Parse(yearNumberInputField.text),
                businessTypeInputField.text,
                phoneNumberInputField.text);

        // 1. delegate
        if (this.updateData != null)
        {
            this.updateData(office, dataIndex);
        }
    }

    /// <summary>
    /// 닫기 버튼 클릭시 실행할 메서드
    /// </summary>
    public void OnClickCloseButton()
    {
        Destroy(gameObject);
    }
}
