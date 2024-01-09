using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public abstract class GOTableViewController : MonoBehaviour
{
    // Cell을 생성하기 위한 Cell Prefab
    [SerializeField] protected GOTableViewCell cellPrefab;
    // Cell을 생성하게 될 Content 객체
    [SerializeField] protected Transform content;

    // Cell 재사용을 위한 Queue
    protected Queue<GOTableViewCell> reuseQueue = new Queue<GOTableViewCell>();

    // Cell간의 연결 고리를 관리하기 위한 List
    protected LinkedList<GOTableViewCell> cellLinkedList = new LinkedList<GOTableViewCell>();

    /// <summary>
    /// Cell의 Height 값과 content의 Height 값 필요
    /// </summary>
    private float cellHeight;

    // numberOfRows()로 받아오는 전체 Cell의 개수
    private int totalRows;

    // 자식 클래스에서 GOTableViewController가 표시해야할 전체 Data의 수를 반환하는 메서드
    protected abstract int numberOfRows();
    // 각 index의 Cell을 반환하는 메서드
    protected abstract GOTableViewCell cellForRowAtIndex(int index);

    /// <summary>
    /// virtual은 abstract와 비슷하지만
    /// abstract는 부모에서 메소드를 정의할 수 없지만,
    /// virtual은 부모 메소드에서 어느정도 정의 가능.
    /// </summary>
    protected virtual void Start()
    {
        totalRows = this.numberOfRows();
        // 스크린의 높이
        float screenHeight = UnityEngine.Screen.height;

        cellHeight = cellPrefab.GetComponent<RectTransform>().sizeDelta.y;

        // 표시해야 할 Cell의 수 계산
        int maxRows = (int)(screenHeight / cellHeight) + 3;
        // 표시할 Cell 보다 화면에 나타낼 수 있는 최대 Cell의 갯수가 더 많은 경우,
        // 표시할 Cell만 생성 / 그렇지 않으면 화면에 나타낼 수 있는 최대 Cell만 생성
        maxRows = (maxRows > totalRows) ? totalRows : maxRows;

        // Content 크기 조정
        RectTransform contentTransform = content.GetComponent<RectTransform>();
        // Content의 크기 = (x = content의 x), (y = numberOfRows()로 받아온 totalRows * 셀의 높이)
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, totalRows * cellHeight);

        // 초기 Cell을 생성
        for (int i = 0; i < maxRows; i++)
        {
            GOTableViewCell cell = cellForRowAtIndex(i);
            // Vertical layout group을 사용했으면 자동으로 배치가 됐을텐데
            // Vertical layout group을 지워서 수동으로 배치
            cell.gameObject.transform.localPosition = new Vector3(0, -i * cellHeight, 0);
            cellLinkedList.AddLast(cell);
        }
    }

    /// <summary>
    /// 재사용이 가능한 Cell을 반환해주는 메서드
    /// </summary>
    /// <returns>재사용 Cell 혹은 null</returns>
    protected GOTableViewCell deQueueReuseCell()
    {
        if (reuseQueue.Count > 0)
        {
            GOTableViewCell cell = reuseQueue.Dequeue();
            cell.gameObject.SetActive(true);
            return cell;
        }
        else
        {
            return null;
        }
    }

    public void OnValueChanged(Vector2 vector)
    {
        Debug.Log($"Content.localPosition.y: {content.localPosition.y}, total Cell Height: {cellLinkedList.Last.Value.Index * cellHeight}");

        
        if ((cellLinkedList.Last.Value.Index < totalRows - 1)
            && ((content.localPosition.y + UnityEngine.Screen.height) > cellLinkedList.Last.Value.Index * cellHeight + cellHeight))
        {
            // 하단에 새로운 Cell이 만들어지는 상황

            // 처음에 있던 Cell은 Reuse Queue 저장
            LinkedListNode<GOTableViewCell> firstCellNode = cellLinkedList.First;
            cellLinkedList.RemoveFirst();
            firstCellNode.Value.gameObject.SetActive(false);
            reuseQueue.Enqueue(firstCellNode.Value);

            // 하단에 새로운 Cell 생성
            LinkedListNode<GOTableViewCell> lastCellNode = cellLinkedList.Last;
            int currentIndex = lastCellNode.Value.Index;
            GOTableViewCell cell = cellForRowAtIndex(currentIndex + 1);
            cell.gameObject.transform.localPosition = new Vector3(0, -(currentIndex + 1) * cellHeight, 0);
            cellLinkedList.AddAfter(lastCellNode, cell);
            cell.gameObject.transform.SetAsLastSibling();
        }
        else if ((cellLinkedList.First.Value.Index > 0)
            && (content.localPosition.y < cellLinkedList.First.Value.Index * cellHeight))
        {
            // 상단에 새로운 Cell이 만들어지는 상황

            // 하단에 있던 Cell은 Reuse Queue 저장
            LinkedListNode<GOTableViewCell> lastCellNode = cellLinkedList.Last;
            cellLinkedList.RemoveLast();
            lastCellNode.Value.gameObject.SetActive(false);
            reuseQueue.Enqueue(lastCellNode.Value);

            // 상단에 새로운 Cell 생성
            LinkedListNode<GOTableViewCell> firstCellNode = cellLinkedList.First;
            int currentIndex = firstCellNode.Value.Index;
            GOTableViewCell cell = cellForRowAtIndex(currentIndex - 1);
            cell.gameObject.transform.localPosition = new Vector3(0, -(currentIndex - 1) * cellHeight, 0);
            cellLinkedList.AddBefore(firstCellNode, cell);
            cell.gameObject.transform.SetAsFirstSibling();
        }
        
    }
}
