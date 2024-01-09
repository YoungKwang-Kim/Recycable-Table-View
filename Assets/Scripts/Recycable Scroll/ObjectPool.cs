using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject panelPrefab; // �г� ������
    [SerializeField] private RectTransform content;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private int itemCount;

    private List<Panel> panels = new List<Panel>(); // Ȱ��ȭ�� �г� ����Ʈ ����
    private Queue<Panel> itemPool = new Queue<Panel>(); // itemPool ����Ʈ ����

    private void Start()
    {
        InitializePanels();
    }

    private void Update()
    {
        UpdatePanelActivation();
    }

    private void InitializePanels()
    {
        for (int i = 0; i < itemCount; i++)
        {
            Panel panel = Instantiate(panelPrefab, content).GetComponent<Panel>();
            panel.InitPanel($"Item {i}");
            panels.Add(panel);
            itemPool.Enqueue(panel);
        }
    }

    private void UpdatePanelActivation()
    {
        foreach (var panel in panels)
        {
            // �г��� RectTransform ������Ʈ�� �����ɴϴ�.
            RectTransform panelRect = panel.transform as RectTransform;

            // IsVisible �Լ��� ȣ���Ͽ� �г��� viewport ���� �ִ��� Ȯ���մϴ�.
            bool visible = IsVisible(panelRect);

            // �г��� Ȱ��ȭ ���¸� visible ������ ���� ���� �����մϴ�.
            panel.gameObject.SetActive(visible);
        }
    }

    private bool IsVisible(RectTransform rectTransform)
    {
        Vector3[] worldCorners = new Vector3[4];
        Vector3[] viewportCorners = new Vector3[4];

        rectTransform.GetWorldCorners(worldCorners);
        scrollRect.viewport.GetWorldCorners(viewportCorners);

        Rect rect = new Rect(worldCorners[0], worldCorners[2] - worldCorners[0]);
        Rect viewportRect = new Rect(viewportCorners[0], viewportCorners[2] - viewportCorners[0]);

        bool overlaps = viewportRect.Overlaps(rect, true);
        /*
        Debug.Log($"Panel {rectTransform.name} world rect: {rect}");
        Debug.Log($"Viewport world rect: {viewportRect}");
        Debug.Log($"Panel {rectTransform.name} visibility: {overlaps}");
        */
        return overlaps;
    }

    public void ScrollUp()
    {
        float scrollSpeed = 10f;
        Vector2 newPosition = content.anchoredPosition + new Vector2(0, scrollSpeed);
        content.anchoredPosition = newPosition;
        Debug.Log($"New anchored position after scrolling up: {content.anchoredPosition}");
    }

    public void ScrollDown()
    {
        float scrollSpeed = 10f;
        Vector2 newPosition = content.anchoredPosition - new Vector2(0, scrollSpeed);
        content.anchoredPosition = newPosition;
        Debug.Log($"New anchored position after scrolling down: {content.anchoredPosition}");
    }
}