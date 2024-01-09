using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OfficeDataManager : MonoBehaviour, ICellDelegate
{
    [SerializeField] private string serviceKey;
    [SerializeField] private OfficeCellController officeCellPrefab;
    [SerializeField] private PopupCellController popupCellPrefab;
    [SerializeField] private CreateCellController createCellPrefab;
    [SerializeField] private UpdateCellController UpdateCellPrefab;
    [SerializeField] private MoreButtonCellController moreButtonPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private Transform canvas;

    // �ҷ��� ��� Office ����Ʈ
    public List<Office> officeLIst = new List<Office>();


    private void Start()
    {
        StartCoroutine(LoadData(0));
    }

    IEnumerator LoadData(int page, GameObject previousMoreButton = null)
    {
        // ���� URL ����
        string url = string.Format("{0}?page={1}&perPage={2}&serviceKey={3}",
            Constants.url, page, Constants.perPage, serviceKey);

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
                    officeLIst.Add(officeArray[i]);

                    OfficeCellController officeCellController = Instantiate(officeCellPrefab, content);
                    officeCellController.SetData(officeArray[i].�繫�Ҹ�, officeArray[i].��������, officeArray[i].��ȭ��ȣ, officeLIst.Count - 1);

                    officeCellController.cellDelegate = this;

                    officeCellController.transform.SetSiblingIndex(officeLIst.Count - 1);
                }

                // More Button ����
                if (previousMoreButton != null) Destroy(previousMoreButton);

                // More Button �߰�
                int currentTotalCount = officeData.perPage * (officeData.page - 1) + officeData.currentCount;

                if (currentTotalCount < officeData.totalCount)
                {
                    MoreButtonCellController moreButtonCellController = Instantiate(moreButtonPrefab, content);
                    moreButtonCellController.loadMoreData = () =>
                    {
                        StartCoroutine(LoadData(officeData.page + 1, moreButtonCellController.gameObject));
                    };
                }
            }
        }
    }

    /// <summary>
    /// Cell�� Ŭ������ �� �����ϴ� �޼���
    /// </summary>
    /// <param name="index"></param>
    public void OnclickCell(int index)
    {
        // PopupCellController popupCellController = Instantiate(popupCellPrefab, canvas);
        // popupCellController.SetPopupData(officeLIst[index]);

        UpdateCellController updateCellController = Instantiate(UpdateCellPrefab, canvas);
        updateCellController.SetPopupData(officeLIst[index], index);

        updateCellController.updateData = (office, updateIndex) =>
        {
            officeLIst[updateIndex] = office;
            reloadData();
        };


    }

    /// <summary>
    /// + ��ư ������ �� ȣ��Ǵ� �޼���
    /// </summary>
    public void OnPlusButton()
    {
        CreateCellController createCellController = Instantiate(createCellPrefab, canvas);
        // createCellController.createData = new CreateData(saveData);

        /*
        createCellController.createData = (office) =>
        {
            officeLIst.Add(office);

            OfficeCellController officeCellController = Instantiate(officeCellPrefab, content);
            officeCellController.SetData(office.�繫�Ҹ�,
                                        office.��������,
                                        office.��ȭ��ȣ,
                                        officeLIst.Count - 1);

            officeCellController.cellDelegate = this;
        officeCellController.transform.SetSiblingIndex(officeLIst.Count - 1);
        };
        */
        createCellController.createDataAction = (office) =>
        {
            officeLIst.Add(office);

            OfficeCellController officeCellController = Instantiate(officeCellPrefab, content);
            officeCellController.SetData(office.�繫�Ҹ�,
                                        office.��������,
                                        office.��ȭ��ȣ,
                                        officeLIst.Count - 1);

            officeCellController.cellDelegate = this;
            officeCellController.transform.SetSiblingIndex(officeLIst.Count - 1);

        };
    }

    public void saveData(Office office)
    {
        // Create Panel Controller���� ���޵� office ��ü�� officeList�� �߰�
        officeLIst.Add(office);

        // ����Ʈ ����
        reloadData();
    }

    private void reloadData()
    {
        // ��� cell �����
        foreach (Transform transform in content.GetComponentInChildren<Transform>())
        {
            Destroy(transform.gameObject);
        }

        // �ٽ� officeList�� ���� cell�� ����
        for (int i = 0; i < officeLIst.Count; i++)
        {
            OfficeCellController officeCellController = Instantiate(officeCellPrefab, content);
            officeCellController.SetData(officeLIst[i].�繫�Ҹ�,
                                        officeLIst[i].��������,
                                        officeLIst[i].��ȭ��ȣ,
                                        officeLIst.Count - 1);

            officeCellController.cellDelegate = this;
        }
    }
}
