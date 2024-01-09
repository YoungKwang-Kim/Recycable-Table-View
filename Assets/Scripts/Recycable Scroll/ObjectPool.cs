using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject panelPrefab; // 패널 프리팹
    [SerializeField] private RectTransform content;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private int itemCount;

    private List<Panel> panels = new List<Panel>(); // 활성화된 패널 리스트 저장
    private Queue<Panel> itemPool = new Queue<Panel>(); // itemPool 리스트 저장

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
            // 패널의 RectTransform 컴포넌트를 가져옵니다.
            RectTransform panelRect = panel.transform as RectTransform;

            // IsVisible 함수를 호출하여 패널이 viewport 내에 있는지 확인합니다.
            bool visible = IsVisible(panelRect);

            // 패널의 활성화 상태를 visible 변수의 값에 따라 설정합니다.
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