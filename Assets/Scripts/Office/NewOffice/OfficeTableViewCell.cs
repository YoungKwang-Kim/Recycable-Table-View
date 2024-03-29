using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OfficeTableViewCell : GOTableViewCell
{
    public TMP_Text officeName;
    public TMP_Text businessType;
    public TMP_Text phoneNumber;

    private void Awake()
    {
        officeName.text = "";
        businessType.text = "";
        phoneNumber.text = "";
    }

    public void SetData(string officeName, string businessType, string phoneNumber)
    {
        this.officeName.text = officeName;
        this.businessType.text = businessType;
        this.phoneNumber.text = phoneNumber;
    }
}
