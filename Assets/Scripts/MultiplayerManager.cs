using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public static MultiplayerManager Instance { get; private set; }
    
    [Header("Photon")]
    public string gameVersion = "v1.0";
    public string roomNamePrefix = "FlyingHouse_";
    public int maxPlayers = 8;
    public byte maxPlayersPerRoom = 8;
    
    [Header("Player")]
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    
    [Header("UI")]
    public TMPro.TextMeshProUGUI connectionStatus;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    
    private bool isConnecting = false;
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    public void Connect()
    {
        if (!isConnecting)
        {
            isConnecting = true;
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        if (connectionStatus != null)
            connectionStatus.text = "Connected to Server";
        
        PhotonNetwork.JoinLobby();
    }
    
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Photon Lobby");
        if (connectionStatus != null)
            connectionStatus.text = "In Lobby";
    }
    
    public void CreateRoom(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayersPerRoom;
        options.PublishUserId = true;
        
        PhotonNetwork.CreateRoom(roomNamePrefix + roomName, options);
    }
    
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomNamePrefix + roomName);
    }
    
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        
        if (connectionStatus != null)
            connectionStatus.text = "In Room: " + PhotonNetwork.CurrentRoom.Name;
        
        if (lobbyPanel != null) lobbyPanel.SetActive(false);
        if (roomPanel != null) roomPanel.SetActive(true);
        
        // Spawn player
        SpawnPlayer();
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join random failed, creating new room");
        CreateRoom("Room" + Random.Range(1000, 9999));
    }
    
    void SpawnPlayer()
    {
        int index = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        Transform spawn = spawnPoints != null && spawnPoints.Length > index 
            ? spawnPoints[index] 
            : transform;
        
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawn.position, spawn.rotation);
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player joined: " + newPlayer.NickName);
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left: " + otherPlayer.NickName);
    }
    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnLeftRoom()
    {
        if (lobbyPanel != null) lobbyPanel.SetActive(true);
        if (roomPanel != null) roomPanel.SetActive(false);
    }
    
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
    
    public bool IsConnected => PhotonNetwork.IsConnected;
    public bool IsInRoom => PhotonNetwork.InRoom;
}
