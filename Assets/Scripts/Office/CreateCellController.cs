using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// 1. delegate
public delegate void CreateData(Office office);

public class CreateCellController : MonoBehaviour
{
    [SerializeField] OfficeCellController officeCellPrefab;

    [SerializeField] private TMP_InputField officeNameInputField;
    [SerializeField] private TMP_InputField roadAddressInputField;
    [SerializeField] private TMP_InputField shingoTypeInputField;
    [SerializeField] private TMP_InputField yearNumberInputField;
    [SerializeField] private TMP_InputField businessTypeInputField;
    [SerializeField] private TMP_InputField phoneNumberInputField;

    // 1. delegate
    public CreateData createData;

    // 2. Action
    public Action<Office> createDataAction;

    private void Awake()
    {
        officeNameInputField.text = "";
        roadAddressInputField.text = "";
        shingoTypeInputField.text = "";
        yearNumberInputField.text = "";
        businessTypeInputField.text = "";
        phoneNumberInputField.text = "";
    }

    /// <summary>
    /// 저장 버튼 클릭시 실행할 메서드
    /// </summary>
    public void OnclickCreateButton()
    {
        Office office = new Office(officeNameInputField.text,
                roadAddressInputField.text,
                shingoTypeInputField.text,
                int.Parse(yearNumberInputField.text),
                businessTypeInputField.text,
                phoneNumberInputField.text);

        // 1. delegate
        if (this.createData != null)
        {
            this.createData(office);
        }

        // 2. Action
        if (this.createDataAction != null)
        {
            this.createDataAction(office);
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
