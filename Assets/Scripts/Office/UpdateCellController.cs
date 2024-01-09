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
    /// �����͸� �ҷ����� �޼���
    /// </summary>
    /// <param name="office">������</param>
    /// <param name="index">�������� �ε���</param>
    public void SetPopupData(Office office, int index)
    {
        roadAddressInputField.text = office.���θ��ּ�;
        officeNameInputField.text = office.�繫�Ҹ�;
        shingoTypeInputField.text = office.�Ű���;
        yearNumberInputField.text = office.����.ToString();
        businessTypeInputField.text = office.��������;
        phoneNumberInputField.text = office.��ȭ��ȣ;
    }

    /// <summary>
    /// ���� ��ư Ŭ���� ������ �޼���
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
    /// �ݱ� ��ư Ŭ���� ������ �޼���
    /// </summary>
    public void OnClickCloseButton()
    {
        Destroy(gameObject);
    }
}
