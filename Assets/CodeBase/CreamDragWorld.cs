using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreamDragWorld : MonoBehaviour
{
    [Header("UI References")]
    public Button creamButton;      // кнопка-крем
    public GameObject creamPrefab;    // префаб крема для нанесения 
    public Camera mainCamera;       

    [Header("Face Zone")]
    public LayerMask faceZoneLayer; // слой, где лежит FaceZone

    public GameObject acneObject;    

    private GameObject currentCream;     

    bool isDragging = false;

   

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        EventTrigger trigger = creamButton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null) trigger = creamButton.gameObject.AddComponent<EventTrigger>();
        
        trigger.triggers = new List<EventTrigger.Entry>();
        
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnCreamButtonClick(); });
        trigger.triggers.Add(entry);
    }

   
    public void OnCreamButtonClick()
    {
       
        creamButton.gameObject.SetActive(false);
     
        Vector3 spawnPos = creamButton.transform.position;
        currentCream = Instantiate(creamPrefab, spawnPos, Quaternion.identity);
        isDragging = true;
    }

    void Update()
    {
        if (!isDragging || currentCream == null) return;

        
        if (Input.GetMouseButton(0))
        {
            Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z);
            Vector3 worldPoint = mainCamera.ScreenToWorldPoint(screenPoint);
            currentCream.transform.position = worldPoint;
        }
    
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            
            Vector2 checkPos = new Vector2(currentCream.transform.position.x, currentCream.transform.position.y);
            Collider2D hit = Physics2D.OverlapPoint(checkPos, faceZoneLayer);
            if (hit != null && hit.CompareTag("FaceZone"))
            {
                acneObject.SetActive(false);
            }
         
            Destroy(currentCream);
            currentCream = null;
           
            creamButton.gameObject.SetActive(true);
        }
    }
}