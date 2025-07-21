using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Tab
{
    public string tabName;         // "Blush", "Eyeshadow", "Lipstick"
    public Button button;        
    public Sprite defaultSprite;   
    public Sprite activeSprite;    
    public GameObject pagePanel;  
}

public class TabManager : MonoBehaviour
{
    public Tab[] tabs;

    public void SwitchTab(string tabName)
    {
        foreach (var tab in tabs)
        {
            bool isActive = tab.tabName == tabName;
            tab.pagePanel.SetActive(isActive);
            Image img = tab.button.GetComponent<Image>();
            if (img != null)
                img.sprite = isActive ? tab.activeSprite : tab.defaultSprite;
        }
    }
}
