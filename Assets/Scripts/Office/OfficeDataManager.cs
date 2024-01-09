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

    // 불러온 모든 Office 리스트
    public List<Office> officeLIst = new List<Office>();


    private void Start()
    {
        StartCoroutine(LoadData(0));
    }

    IEnumerator LoadData(int page, GameObject previousMoreButton = null)
    {
        // 서버 URL 설정
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
                    officeCellController.SetData(officeArray[i].사무소명, officeArray[i].영업구분, officeArray[i].전화번호, officeLIst.Count - 1);

                    officeCellController.cellDelegate = this;

                    officeCellController.transform.SetSiblingIndex(officeLIst.Count - 1);
                }

                // More Button 제거
                if (previousMoreButton != null) Destroy(previousMoreButton);

                // More Button 추가
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
    /// Cell을 클릭했을 때 동작하는 메서드
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
    /// + 버튼 눌렀을 때 호출되는 메서드
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
            officeCellController.SetData(office.사무소명,
                                        office.영업구분,
                                        office.전화번호,
                                        officeLIst.Count - 1);

            officeCellController.cellDelegate = this;
        officeCellController.transform.SetSiblingIndex(officeLIst.Count - 1);
        };
        */
        createCellController.createDataAction = (office) =>
        {
            officeLIst.Add(office);

            OfficeCellController officeCellController = Instantiate(officeCellPrefab, content);
            officeCellController.SetData(office.사무소명,
                                        office.영업구분,
                                        office.전화번호,
                                        officeLIst.Count - 1);

            officeCellController.cellDelegate = this;
            officeCellController.transform.SetSiblingIndex(officeLIst.Count - 1);

        };
    }

    public void saveData(Office office)
    {
        // Create Panel Controller에서 전달된 office 객체를 officeList에 추가
        officeLIst.Add(office);

        // 리스트 갱신
        reloadData();
    }

    private void reloadData()
    {
        // 모든 cell 지우기
        foreach (Transform transform in content.GetComponentInChildren<Transform>())
        {
            Destroy(transform.gameObject);
        }

        // 다시 officeList의 값을 cell로 생성
        for (int i = 0; i < officeLIst.Count; i++)
        {
            OfficeCellController officeCellController = Instantiate(officeCellPrefab, content);
            officeCellController.SetData(officeLIst[i].사무소명,
                                        officeLIst[i].영업구분,
                                        officeLIst[i].전화번호,
                                        officeLIst.Count - 1);

            officeCellController.cellDelegate = this;
        }
    }
}
