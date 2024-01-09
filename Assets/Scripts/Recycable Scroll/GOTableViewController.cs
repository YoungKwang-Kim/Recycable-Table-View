using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public abstract class GOTableViewController : MonoBehaviour
{
    // Cell�� �����ϱ� ���� Cell Prefab
    [SerializeField] protected GOTableViewCell cellPrefab;
    // Cell�� �����ϰ� �� Content ��ü
    [SerializeField] protected Transform content;

    // Cell ������ ���� Queue
    protected Queue<GOTableViewCell> reuseQueue = new Queue<GOTableViewCell>();

    // Cell���� ���� ���� �����ϱ� ���� List
    protected LinkedList<GOTableViewCell> cellLinkedList = new LinkedList<GOTableViewCell>();

    /// <summary>
    /// Cell�� Height ���� content�� Height �� �ʿ�
    /// </summary>
    private float cellHeight;

    // numberOfRows()�� �޾ƿ��� ��ü Cell�� ����
    private int totalRows;

    // �ڽ� Ŭ�������� GOTableViewController�� ǥ���ؾ��� ��ü Data�� ���� ��ȯ�ϴ� �޼���
    protected abstract int numberOfRows();
    // �� index�� Cell�� ��ȯ�ϴ� �޼���
    protected abstract GOTableViewCell cellForRowAtIndex(int index);

    /// <summary>
    /// virtual�� abstract�� ���������
    /// abstract�� �θ𿡼� �޼ҵ带 ������ �� ������,
    /// virtual�� �θ� �޼ҵ忡�� ������� ���� ����.
    /// </summary>
    protected virtual void Start()
    {
        totalRows = this.numberOfRows();
        // ��ũ���� ����
        float screenHeight = UnityEngine.Screen.height;

        cellHeight = cellPrefab.GetComponent<RectTransform>().sizeDelta.y;

        // ǥ���ؾ� �� Cell�� �� ���
        int maxRows = (int)(screenHeight / cellHeight) + 3;
        // ǥ���� Cell ���� ȭ�鿡 ��Ÿ�� �� �ִ� �ִ� Cell�� ������ �� ���� ���,
        // ǥ���� Cell�� ���� / �׷��� ������ ȭ�鿡 ��Ÿ�� �� �ִ� �ִ� Cell�� ����
        maxRows = (maxRows > totalRows) ? totalRows : maxRows;

        // Content ũ�� ����
        RectTransform contentTransform = content.GetComponent<RectTransform>();
        // Content�� ũ�� = (x = content�� x), (y = numberOfRows()�� �޾ƿ� totalRows * ���� ����)
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, totalRows * cellHeight);

        // �ʱ� Cell�� ����
        for (int i = 0; i < maxRows; i++)
        {
            GOTableViewCell cell = cellForRowAtIndex(i);
            // Vertical layout group�� ��������� �ڵ����� ��ġ�� �����ٵ�
            // Vertical layout group�� ������ �������� ��ġ
            cell.gameObject.transform.localPosition = new Vector3(0, -i * cellHeight, 0);
            cellLinkedList.AddLast(cell);
        }
    }

    /// <summary>
    /// ������ ������ Cell�� ��ȯ���ִ� �޼���
    /// </summary>
    /// <returns>���� Cell Ȥ�� null</returns>
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
            // �ϴܿ� ���ο� Cell�� ��������� ��Ȳ

            // ó���� �ִ� Cell�� Reuse Queue ����
            LinkedListNode<GOTableViewCell> firstCellNode = cellLinkedList.First;
            cellLinkedList.RemoveFirst();
            firstCellNode.Value.gameObject.SetActive(false);
            reuseQueue.Enqueue(firstCellNode.Value);

            // �ϴܿ� ���ο� Cell ����
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
            // ��ܿ� ���ο� Cell�� ��������� ��Ȳ

            // �ϴܿ� �ִ� Cell�� Reuse Queue ����
            LinkedListNode<GOTableViewCell> lastCellNode = cellLinkedList.Last;
            cellLinkedList.RemoveLast();
            lastCellNode.Value.gameObject.SetActive(false);
            reuseQueue.Enqueue(lastCellNode.Value);

            // ��ܿ� ���ο� Cell ����
            LinkedListNode<GOTableViewCell> firstCellNode = cellLinkedList.First;
            int currentIndex = firstCellNode.Value.Index;
            GOTableViewCell cell = cellForRowAtIndex(currentIndex - 1);
            cell.gameObject.transform.localPosition = new Vector3(0, -(currentIndex - 1) * cellHeight, 0);
            cellLinkedList.AddBefore(firstCellNode, cell);
            cell.gameObject.transform.SetAsFirstSibling();
        }
        
    }
}
