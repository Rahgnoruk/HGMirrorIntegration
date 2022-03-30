using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperGnosys.MirrorIntegration
{
    public class UILobbySingleton : MonoBehaviour
    {
        public static UILobbySingleton Instance { get; private set; }

        [Header("Host Join")]
        [SerializeField] TMP_InputField joinMatchInput;
        [SerializeField] List<Selectable> activableElements = new List<Selectable>();
        [SerializeField] GameObject lobbyCanvas;
        [SerializeField] GameObject roomCanvas;
        [SerializeField] GameObject searchCanvas;
        bool searching = false; 

        [Header("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;
        [SerializeField] TMP_Text matchIDText;
        [SerializeField] GameObject beginGameButton;

        GameObject localPlayerLobbyUI;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void HostPublic()
        {
            DeactivateUI();

            NetworkedPlayerSingleton.localPlayer.HostGame(true);
        }

        public void HostPrivate()
        {
            DeactivateUI();

            NetworkedPlayerSingleton.localPlayer.HostGame(false);
        }

        public void HostSuccess(bool success, string matchID)
        {
            if (success)
            {
                ActivateRoomCanvas();

                if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab(NetworkedPlayerSingleton.localPlayer);
                matchIDText.text = matchID;
                beginGameButton.SetActive(true);
            }
            else
            {
                ActivateUI();
            }
        }

        public void Join()
        {
            DeactivateUI();
            NetworkedPlayerSingleton.localPlayer.JoinGame(joinMatchInput.text.ToUpper());
        }

        public void JoinSuccess(bool success, string matchID)
        {
            if (success)
            {
                ActivateRoomCanvas();

                if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab(NetworkedPlayerSingleton.localPlayer);
                matchIDText.text = matchID;
            }
            else
            {
                ActivateUI();
            }
        }

        public void DisconnectGame()
        {
            if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
            NetworkedPlayerSingleton.localPlayer.DisconnectGame();

            ActivateLobbyCanvas();
            ActivateUI();
            beginGameButton.SetActive(false);
        }

        public GameObject SpawnPlayerUIPrefab(NetworkedPlayerSingleton player)
        {
            GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
            PlayerIcon playerIcon = newUIPlayer.GetComponent<PlayerIcon>();
            if (playerIcon != null) { 
                playerIcon.SetCaption("Player " + player.PlayerIndex);
            }
            newUIPlayer.transform.SetSiblingIndex(player.PlayerIndex - 1);
            return newUIPlayer;
        }

        public void BeginGame()
        {
            NetworkedPlayerSingleton.localPlayer.BeginGame();
        }

        public void SearchGame()
        {
            StartCoroutine(Searching());
        }

        public void CancelSearchGame()
        {
            searching = false;
        }

        public void SearchGameSuccess(bool success, string matchID)
        {
            if (success)
            {
                searchCanvas.SetActive(false);
                searching = false;
                JoinSuccess(success, matchID);
            }
        }

        IEnumerator Searching()
        {
            searchCanvas.SetActive(true);
            searching = true;

            float searchInterval = 1;
            float currentTime = 1;

            while (searching)
            {
                if (currentTime > 0)
                {
                    currentTime -= Time.deltaTime;
                }
                else
                {
                    currentTime = searchInterval;
                    NetworkedPlayerSingleton.localPlayer.SearchGame();
                }
                yield return null;
            }
            searchCanvas.SetActive(false);
        }
        private void DeactivateUI()
        {
            activableElements.ForEach(x => x.interactable = false);
        }
        private void ActivateUI()
        {
            activableElements.ForEach(x => x.interactable = true);
        }
        private void ActivateRoomCanvas()
        {
            roomCanvas.SetActive(true);
            lobbyCanvas.SetActive(false);
        }
        private void ActivateLobbyCanvas()
        {
            lobbyCanvas.SetActive(true);
            roomCanvas.SetActive(false);
        }
    }
}