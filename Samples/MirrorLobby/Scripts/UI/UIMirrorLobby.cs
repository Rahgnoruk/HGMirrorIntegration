using HyperGnosys.Core;
using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperGnosys.MirrorIntegration
{
    public class UIMirrorLobby : MonoBehaviour
    {
        [SerializeField] private bool debugging = false;
        [Header("Event Received")]
        [SerializeField] private MirrorLobbyPlayerScriptableEvent OnLocalPlayerRaised;
        [SerializeField] private BoolScriptableEvent OnAllPlayersReady;
        [SerializeField] private LobbyInfoScriptableEvent OnLobbyChanged;
        [Header("Lobby Elements")]
        [SerializeField] private TMP_Text lobbyNameText;
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button startGameButton;
        [SerializeField] private List<Selectable> deactivableElements = new List<Selectable>();
        [Header("Lobby Slots")]
        [SerializeField] private Transform container;
        [SerializeField] private PlayerIcon playerIconPrefab;
        private const string DisplayNamePrefsKey = "DisplayName";

        private List<PlayerIcon> playerIcons = new List<PlayerIcon>();
        private MirrorLobbyPlayer localPlayer;
        void Awake()
        {
            OnLocalPlayerRaised.onEventRaised.AddListener(SetUpLobby);
            OnAllPlayersReady.onEventRaised.AddListener(ActivateStartButton);
            startGameButton.gameObject.SetActive(false);
            OnLobbyChanged.onEventRaised.AddListener(UpdateDisplay);
        }
        private void SetUpLobby(MirrorLobbyPlayer localPlayer)
        {
            HGDebug.Log("Setting up lobby", this, debugging);
            this.localPlayer = localPlayer;
            SetUpInputField();
        }
        public void ActivateStartButton(bool allPlayersReady)
        {
            startGameButton.interactable = true;
        }
        public void StartGame()
        {
            if (localPlayer.IsLeader && Room.allPlayersReady)
            {
                Room.StartGame();
            }
        }
        public void LeaveLobby()
        {
            localPlayer.LeaveLobby();
        }
        public void ToggleReadyState()
        {
            localPlayer.ToggleReadyStatus();

            TMP_Text text = readyButton.GetComponentInChildren<TMP_Text>();
            if (text != null)
            {
                if (localPlayer.isReady)
                {

                    text.text = "Not Ready";
                }
                else
                {
                    text.text = "Ready";
                }
            }
        }
        public void UpdateDisplay(LobbyInfo lobby)
        {
            if (localPlayer.IsLeader)
            {
                startGameButton.gameObject.SetActive(true);
            }
            if (!Room.allPlayersReady)
            {
                startGameButton.interactable = false;
            }
            else
            {
                startGameButton.interactable = true;
            }
            HGDebug.Log($"Updating lobby with {playerIcons.Count} icons and {Room.roomSlots.Count} players", this, debugging);
            while(playerIcons.Count < Room.roomSlots.Count)
            {
                PlayerIcon playerIconInstance = Instantiate(playerIconPrefab, container);
                playerIcons.Add(playerIconInstance);
            }
            while(playerIcons.Count > Room.roomSlots.Count)
            {
                PlayerIcon excessIcon = playerIcons[playerIcons.Count - 1];
                playerIcons.Remove(excessIcon);
                Destroy(excessIcon.gameObject);
            }
            ///Cada versión del script de MirrorLobbyPlayers se suscribe
            ///en Start a los roomSlots. Esta definido en NetworkRoomPlayers
            for (int i = 0; i < playerIcons.Count; i++)
            {
                MirrorLobbyPlayer roomPlayer = Room.roomSlots[i] as MirrorLobbyPlayer;
                LobbyInfo lobbyInfo = roomPlayer.lobbyInfo;
                playerIcons[i].SetCaption(lobbyInfo.LocalPlayerDisplayName);
                if (roomPlayer.readyToBegin)
                {
                    playerIcons[i].SetReadyText("<color=green>Ready</color>");
                }
                else
                {
                    playerIcons[i].SetReadyText("<color=red>Not Ready</color>");
                }
            }
        }
        private void SetUpInputField()
        {
            if (!PlayerPrefs.HasKey(DisplayNamePrefsKey))
            {
                ValidateUI();
                return;
            }
            string defaultName = PlayerPrefs.GetString(DisplayNamePrefsKey);
            playerNameInputField.text = defaultName;
            SetDisplayName(defaultName);
        }
        public void SetDisplayName(string displayName)
        {
            localPlayer.UpdateDisplayName(displayName);
            ValidateUI();
            SavePlayerName();
        }
        private void SavePlayerName()
        {
            PlayerPrefs.SetString(DisplayNamePrefsKey, playerNameInputField.text);
        }

        private void ValidateUI()
        {
            if (string.IsNullOrEmpty(playerNameInputField.text) || playerNameInputField.text.Length < 2)
            {
                DeactivateUI();
            }
            else
            {
                ActivateUI();
            }
        }
        private void DeactivateUI()
        {
            foreach(Selectable element in deactivableElements)
            {
                element.interactable = false;
            }
        }
        private void ActivateUI()
        {
            foreach (Selectable element in deactivableElements)
            {
                element.interactable = true;
            }
        }
        private MirrorLobbyNetworkManager Room
        {
            get
            {
                return NetworkManager.singleton as MirrorLobbyNetworkManager;
            }
        }
    }
}