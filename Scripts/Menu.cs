using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [Header("Screens")]
    public GameObject mainScreen;
    public GameObject createRoomScreen;
    public GameObject lobbyScreen;
    public GameObject lobbyBrowserScreen;
    public GameObject titleScreen;
    public GameObject characterScreen;

    [Header("Main Screen")]
    public Button createRoomButton;
    public Button findRoomButton;

    [Header("Lobby")]
    public TextMeshProUGUI playerListText;
    public TextMeshProUGUI roomInfoText;
    public Button startGameButton;

    [Header("Lobby Browser")]
    public RectTransform roomListContainer;
    public GameObject roomButtonPrefab;
    private List<GameObject> roomButtons = new List<GameObject>();
    private List<RoomInfo> roomList = new List<RoomInfo>();

    [Header("Character Custom")]
    public GameObject skin1;
    public GameObject skin2;
    public GameObject skin3;
    public GameObject skin4;
    public GameObject eye1;
    public GameObject eye2;
    public GameObject eye3;
    public GameObject hair1;
    public GameObject hair2;
    public GameObject hair3;
    public GameObject hair4;
    public GameObject hair5;
    public GameObject hair6;
    public GameObject hair7;


    // Start is called before the first frame update
    void Start()
    {
        // disable the menu buttons at the start
        createRoomButton.interactable = false;
        findRoomButton.interactable = false;

        // enable the cursor since we hide it when we play the game
        Cursor.lockState = CursorLockMode.None;

        // are we in a game?
        if (PhotonNetwork.InRoom)
        {
            // go to the lobby

            // make the room visible
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    // changes the currently visible screen
    void SetScreen(GameObject screen)
    {
        // disable all other screens
        mainScreen.SetActive(false);
        createRoomScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        lobbyBrowserScreen.SetActive(false);
        characterScreen.SetActive(false);
        titleScreen.SetActive(false);

        // activate the requested screen
        screen.SetActive(true);

        if (screen == lobbyBrowserScreen)
            UpdateLobbyBrowserUI();
    }

    public void OnPlayerNameValueChanged(TMP_InputField playerNameInput)
    {
        PhotonNetwork.NickName = playerNameInput.text;
    }

    public override void OnConnectedToMaster()
    {
        // enable the menu buttons once we connect to the server
        createRoomButton.interactable = true;
        findRoomButton.interactable = true;
    }

    // called when the "Create Room" button has been pressed.
    public void OnCreateRoomButton()
    {
        SetScreen(createRoomScreen);
    }

    public void OnStartTitleButton()
    {
        SetScreen(characterScreen);
    }

    public void OnDoneCharacterButton()
    {
        SetScreen(mainScreen);
    }

    public void click_skin_tone1_button()
    {
        PlayerController.skin_number = 1;
        skin1.SetActive(true);
        skin2.SetActive(false);
        skin3.SetActive(false);
        skin4.SetActive(false);
    }

    public void click_skin_tone2_button()
    {
        PlayerController.skin_number = 2;
        skin1.SetActive(false);
        skin2.SetActive(true);
        skin3.SetActive(false);
        skin4.SetActive(false);
    }

    public void click_skin_tone3_button()
    {
        PlayerController.skin_number = 3;
        skin1.SetActive(false);
        skin2.SetActive(false);
        skin3.SetActive(true);
        skin4.SetActive(false);
    }

    public void click_skin_tone4_button()
    {
        PlayerController.skin_number = 3;
        skin1.SetActive(false);
        skin2.SetActive(false);
        skin3.SetActive(false);
        skin4.SetActive(true);
    }

    public void click_eye1_button()
    {
        PlayerController.eye_number = 1;
        eye1.SetActive(true);
        eye2.SetActive(false);
        eye3.SetActive(false);
    }

    public void click_eye2_button()
    {
        PlayerController.eye_number = 2;
        eye1.SetActive(false);
        eye2.SetActive(true);
        eye3.SetActive(false);
    }

    public void click_eye3_button()
    {
        PlayerController.eye_number = 3;
        eye1.SetActive(false);
        eye2.SetActive(false);
        eye3.SetActive(true);
    }

    public void click_hair1_button()
    {
        PlayerController.hair_number = 1;
        hair1.SetActive(true);
        hair2.SetActive(false);
        hair3.SetActive(false);
        hair4.SetActive(false);
        hair5.SetActive(false);
        hair6.SetActive(false);
        hair7.SetActive(false);
    }

    public void click_hair2_button()
    {
        PlayerController.hair_number = 2;
        hair1.SetActive(false);
        hair2.SetActive(true);
        hair3.SetActive(false);
        hair4.SetActive(false);
        hair5.SetActive(false);
        hair6.SetActive(false);
        hair7.SetActive(false);
    }

    public void click_hair3_button()
    {
        PlayerController.hair_number = 3;
        hair1.SetActive(false);
        hair2.SetActive(false);
        hair3.SetActive(true);
        hair4.SetActive(false);
        hair5.SetActive(false);
        hair6.SetActive(false);
        hair7.SetActive(false);
    }

    public void click_hair4_button()
    {
        PlayerController.hair_number = 4;
        hair1.SetActive(false);
        hair2.SetActive(false);
        hair3.SetActive(false);
        hair4.SetActive(true);
        hair5.SetActive(false);
        hair6.SetActive(false);
        hair7.SetActive(false);
    }

    public void click_hair5_button()
    {
        PlayerController.hair_number = 5;
        hair1.SetActive(false);
        hair2.SetActive(false);
        hair3.SetActive(false);
        hair4.SetActive(false);
        hair5.SetActive(true);
        hair6.SetActive(false);
        hair7.SetActive(false);
    }

    public void click_hair6_button()
    {
        PlayerController.hair_number = 6;
        hair1.SetActive(false);
        hair2.SetActive(false);
        hair3.SetActive(false);
        hair4.SetActive(false);
        hair5.SetActive(false);
        hair6.SetActive(true);
        hair7.SetActive(false);
    }
    public void click_hair7_button()
    {
        PlayerController.hair_number = 7;
        hair1.SetActive(false);
        hair2.SetActive(false);
        hair3.SetActive(false);
        hair4.SetActive(false);
        hair5.SetActive(false);
        hair6.SetActive(false);
        hair7.SetActive(true);
    }
   
   



    // called when the "Find Room" button has been pressed
    public void OnFindRoomButton()
    {
        SetScreen(lobbyBrowserScreen);
    }

    // called when the "Back" button gets pressed
    public void OnBackButton()
    {
        SetScreen(mainScreen);
    }

    public void OnCreateButton(TMP_InputField roomNameInput)
    {
        NetworkManager.instance.CreateRoom(roomNameInput.text);
    }

    public void CharacterNameInput(TMP_InputField characterNameInput)
    {
        PlayerController.character_name = characterNameInput.text;
    }

    public override void OnJoinedRoom()
    {
        SetScreen(lobbyScreen);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    [PunRPC]
    void UpdateLobbyUI()
    {
        // enable or disable the start game button depending on if we're the host
        startGameButton.interactable = PhotonNetwork.IsMasterClient;

        // display all the players
        playerListText.text = "";
        foreach (Player player in PhotonNetwork.PlayerList)
            playerListText.text += player.NickName + "\n";
        
        // set the room info text
        roomInfoText.text = "<b>Room Name</b>\n" + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    public void OnStartGameButton()
    {
        // hide the room
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        // tell everyone to load the game scene
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }

    public void OnLeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }

    void UpdateLobbyBrowserUI()
    {
        // disable all room buttons
        foreach (GameObject button in roomButtons)
            button.SetActive(false);

        // display all current rooms in the master server
        for (int x = 0; x < roomList.Count; ++x)
        {
            // get or create the button object
            GameObject button = x >= roomButtons.Count ? CreateRoomButton() : roomButtons[x];
            button.SetActive(true);

            // set the room name and player count texts
            button.transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>().text = roomList[x].Name;
            button.transform.Find("PlayerCountText").GetComponent<TextMeshProUGUI>().text = roomList[x].PlayerCount + " / " + roomList[x].MaxPlayers;

            // set the button OnClick event
            Button buttonComp = button.transform.Find("Button").GetComponent<Button>();
            string roomName = roomList[x].Name;
            buttonComp.onClick.RemoveAllListeners();
            buttonComp.onClick.AddListener(() => { OnJoinRoomButton(roomName); });
        }
    }

    GameObject CreateRoomButton()
    {
        GameObject buttonObj = Instantiate(roomButtonPrefab, roomListContainer.transform);
        roomButtons.Add(buttonObj);
        return buttonObj;
    }

    public void OnJoinRoomButton(string roomName)
    {
        NetworkManager.instance.JoinRoom(roomName);
    }

    public void OnRefreshButton()
    {
        UpdateLobbyBrowserUI();
    }

    public override void OnRoomListUpdate(List<RoomInfo> allRooms)
    {
        roomList = allRooms;
    }
}
