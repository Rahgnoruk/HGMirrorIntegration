using HyperGnosys.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperGnosys.MirrorIntegration
{
    public class UILobby : MonoBehaviour
    {
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private TMP_InputField matchIDInputFIeld;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;

        [SerializeField] private VoidScriptableEvent hostEvent;
        [SerializeField] private StringScriptableEvent joinEvent;
        [SerializeField] private VoidScriptableEvent hostRoomFailed;
        [SerializeField] private VoidScriptableEvent joinRoomFailed;
        public string DisplayName { get; private set; }
        private const string DisplayNamePrefsKey = "DisplayName";

        private void Awake()
        {
            hostEvent.onEventRaised.AddListener(Host);
            hostRoomFailed.onEventRaised.AddListener(OnHostFailed);
            joinRoomFailed.onEventRaised.AddListener(OnJoinFailed);
            SetUpInputField();
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
            DisplayName = displayName;
            ValidateUI();
            SavePlayerName();
        }

        private void SavePlayerName()
        {
            PlayerPrefs.SetString(DisplayNamePrefsKey, DisplayName);
        }

        private void ValidateUI()
        {
            if (string.IsNullOrEmpty(DisplayName))
            {
                matchIDInputFIeld.interactable = false;
                hostButton.interactable = false;
                joinButton.interactable = false;
            }
            else
            {
                matchIDInputFIeld.interactable = true;
                hostButton.interactable = true;
                joinButton.interactable = true;
            }
        }
        private void Host(Void noParameter)
        {
            DeactivateUI();
        }
        public void Join()
        {
            DeactivateUI();
            joinEvent.Raise(matchIDInputFIeld.text);
        }

        private void OnHostFailed(Void noParameter)
        {
            ActivateUI();
        }

        private void OnJoinFailed(Void noParameter)
        {
            ActivateUI();
        }

        private void DeactivateUI()
        {
            playerNameInputField.interactable = false;
            matchIDInputFIeld.interactable = false;
            hostButton.interactable = false;
            joinButton.interactable = false;
        }
        private void ActivateUI()
        {
            playerNameInputField.interactable = true;
            matchIDInputFIeld.interactable = true;
            hostButton.interactable = true;
            joinButton.interactable = true;
        }
    }
}