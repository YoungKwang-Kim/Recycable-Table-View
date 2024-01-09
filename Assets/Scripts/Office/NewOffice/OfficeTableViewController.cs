using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class OfficeTableViewController : GOTableViewController
{
    [SerializeField] private string serviceKey;

    private List<Office> officeList = new List<Office>();

    protected override void Start()
    {
        StartCoroutine(LoadData());
    }

    // �ڽ� Ŭ�������� GOTableViewController�� ǥ���ؾ��� ��ü Data�� ���� ��ȯ�ϴ� �޼���
    protected override int numberOfRows()
    {
        return officeList.Count;
    }

    // �� index�� Cell�� ��ȯ�ϴ� �޼���
    protected override GOTableViewCell cellForRowAtIndex(int index)
    {
        // Cell ��ü ����� ��ȯ
        OfficeTableViewCell cell = deQueueReuseCell() as OfficeTableViewCell;

        if (cell == null) 
        {
            cell = Instantiate(cellPrefab, content) as OfficeTableViewCell;
        }

        cell.Index = index;
        cell.officeName.text = officeList[index].�繫�Ҹ�;
        cell.businessType.text = officeList[index].��������;
        cell.phoneNumber.text = officeList[index].��ȭ��ȣ;

        return cell;
    }
    IEnumerator LoadData()
    {

        /*
        // ���� URL ����
        string url = string.Format("{0}?page={1}&perPage={2}&serviceKey={3}",
            Constants.url, page, Constants.perPage, serviceKey);
        */
        string url = string.Format("{0}?page={1}&perPage={2}&serviceKey={3}",
            Constants.url, 0, 100, serviceKey);

        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                string result = request.downloadHandler.text;

                OfficeData officeData = JsonUtility.FromJson<OfficeData>(result);
                Office[] officeArray = officeData.data;

                for (int i = 0; i < officeArray.Length; i++)
                {
                    officeList.Add(officeArray[i]);
                }
            }

            // GOTableViewController�� Start() ȣ��
            base.Start();
        }
    }
}
