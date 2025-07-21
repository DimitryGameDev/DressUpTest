using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ShadowDragWorld : MonoBehaviour
{
    [System.Serializable]
    public class MakeupOption
    {
        public Button button;        // кнопки в UI
        public GameObject makeupObject; // Мейкапы на кукле (определенный вид)
        public GameObject brushPrefab;  // чем наносим мейк
        public bool hideButtonOnDrag;  // если надо во время нанесения спрятать кнопку
    }

    [Header("UI References")]
    public List<MakeupOption> makeupOptions; // список мейкапов
    public Camera mainCamera;                

    private GameObject currentBrush;
    private MakeupOption currentOption;
    private bool isDragging = false;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Set up PointerDown triggers on each button
        foreach (var option in makeupOptions)
        {
            var trigger = option.button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null) trigger = option.button.gameObject.AddComponent<EventTrigger>();
            trigger.triggers = new List<EventTrigger.Entry>();
            var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            var captured = option;
            entry.callback.AddListener((data) => StartBrushDrag(captured));
            trigger.triggers.Add(entry);
        }
    }

    void StartBrushDrag(MakeupOption option)
    {
        currentOption = option;
        
        if (option.hideButtonOnDrag)
            option.button.gameObject.SetActive(false);
  
        Vector3 spawnPos = option.button.transform.position;
        currentBrush = Instantiate(option.brushPrefab, spawnPos, Quaternion.identity);
        isDragging = true;
    }

    void Update()
    {
        if (!isDragging || currentBrush == null)
            return;

 
        if (Input.GetMouseButton(0))
        {
            Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z);
            Vector3 worldPoint = mainCamera.ScreenToWorldPoint(screenPoint);
            currentBrush.transform.position = worldPoint;
        }
    
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            // Чек зоны лица
            Vector2 checkPos = currentBrush.transform.position;
            Collider2D hit = Physics2D.OverlapPoint(checkPos);
            if (hit != null && hit.CompareTag("FaceZone"))
            {
                currentOption.makeupObject.SetActive(true);
            }
            
            Destroy(currentBrush);
            currentBrush = null;
            // восстанавливаем кнопку
            if (currentOption.hideButtonOnDrag)
                currentOption.button.gameObject.SetActive(true);
            currentOption = null;
        }
    }
}
