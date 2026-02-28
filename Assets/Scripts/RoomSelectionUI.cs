using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RoomSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject roomPanel;
    public Transform roomListContainer;
    public GameObject roomButtonPrefab;
    public TextMeshProUGUI currentRoomText;
    public TextMeshProUGUI roomDescriptionText;
    public TextMeshProUGUI creditsText;
    
    [Header("Room Details")]
    public GameObject roomDetailPanel;
    public TextMeshProUGUI detailName;
    public TextMeshProUGUI detailDescription;
    public TextMeshProUGUI detailCost;
    public TextMeshProUGUI detailRequirements;
    public Button unlockButton;
    public Button teleportButton;
    public Button backButton;
    
    [Header("Colors")]
    public Color unlockedColor = Color.green;
    public Color lockedColor = Color.red;
    public Color currentColor = Color.yellow;
    
    private HomeRoomSystem roomSystem;
    private HouseUpgradeSystem upgradeSystem;
    private HomeRoomSystem.RoomData selectedRoom;
    private List<GameObject> roomButtons = new List<GameObject>();
    
    void Start()
    {
        roomSystem = HomeRoomSystem.Instance;
        upgradeSystem = FindObjectOfType<HouseUpgradeSystem>();
        
        if (backButton != null)
            backButton.onClick.AddListener(HideDetails);
        
        RefreshUI();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRoomPanel();
        }
        
        if (creditsText != null && upgradeSystem != null)
        {
            creditsText.text = $"${upgradeSystem.playerCredits}";
        }
    }
    
    public void ToggleRoomPanel()
    {
        roomPanel?.SetActive(!roomPanel.activeSelf);
        if (roomPanel?.activeSelf == true)
        {
            RefreshUI();
        }
    }
    
    void RefreshUI()
    {
        if (roomSystem == null) return;
        
        ClearRoomButtons();
        
        if (currentRoomText != null)
        {
            currentRoomText.text = $"Current: {roomSystem.currentRoom?.roomName ?? "Unknown"}";
        }
        
        foreach (var room in roomSystem.allRooms)
        {
            CreateRoomButton(room);
        }
    }
    
    void ClearRoomButtons()
    {
        foreach (var btn in roomButtons)
        {
            if (btn != null) Destroy(btn);
        }
        roomButtons.Clear();
    }
    
    void CreateRoomButton(HomeRoomSystem.RoomData room)
    {
        if (roomButtonPrefab == null || roomListContainer == null) return;
        
        GameObject btnObj = Instantiate(roomButtonPrefab, roomListContainer);
        var btn = btnObj.GetComponent<Button>();
        var txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();
        
        bool isUnlocked = roomSystem.IsRoomUnlocked(room.roomId);
        bool isCurrent = roomSystem.currentRoom?.roomId == room.roomId;
        
        if (txt != null)
        {
            txt.text = isUnlocked ? room.roomName : $"??? (${room.cost})";
        }
        
        // Set color
        var img = btnObj.GetComponent<Image>();
        if (img != null)
        {
            if (isCurrent) img.color = currentColor;
            else if (isUnlocked) img.color = unlockedColor;
            else img.color = lockedColor;
        }
        
        btn.onClick.AddListener(() => SelectRoom(room));
        roomButtons.Add(btnObj);
    }
    
    void SelectRoom(HomeRoomSystem.RoomData room)
    {
        selectedRoom = room;
        ShowDetails(room);
    }
    
    void ShowDetails(HomeRoomSystem.RoomData room)
    {
        if (roomDetailPanel != null)
            roomDetailPanel.SetActive(true);
        
        if (detailName != null)
            detailName.text = room.roomName;
        
        if (detailDescription != null)
            detailDescription.text = room.description;
        
        bool isUnlocked = roomSystem.IsRoomUnlocked(room.roomId);
        bool isCurrent = roomSystem.currentRoom?.roomId == room.roomId;
        
        if (detailCost != null)
            detailCost.text = isUnlocked ? "OWNED" : $"${room.cost}";
        
        if (detailRequirements != null)
            detailRequirements.text = room.unlockRequirements ?? "None";
        
        // Configure buttons
        if (unlockButton != null)
        {
            unlockButton.gameObject.SetActive(!isUnlocked);
            unlockButton.interactable = upgradeSystem?.playerCredits >= room.cost;
            unlockButton.onClick.RemoveAllListeners();
            unlockButton.onClick.AddListener(() => TryUnlock(room));
        }
        
        if (teleportButton != null)
        {
            teleportButton.gameObject.SetActive(isUnlocked && !isCurrent);
            teleportButton.onClick.RemoveAllListeners();
            teleportButton.onClick.AddListener(() => Teleport(room));
        }
    }
    
    void HideDetails()
    {
        if (roomDetailPanel != null)
            roomDetailPanel.SetActive(false);
        selectedRoom = null;
    }
    
    void TryUnlock(HomeRoomSystem.RoomData room)
    {
        if (roomSystem.UnlockRoom(room.roomId))
        {
            RefreshUI();
            ShowDetails(room);
        }
    }
    
    void Teleport(HomeRoomSystem.RoomData room)
    {
        roomSystem.SwitchRoom(room.roomId);
        RefreshUI();
        HideDetails();
    }
}
