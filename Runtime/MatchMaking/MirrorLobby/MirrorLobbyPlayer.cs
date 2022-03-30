using HyperGnosys.Core;
using Mirror;
using UnityEngine;

namespace HyperGnosys.MirrorIntegration
{
    public class MirrorLobbyPlayer : NetworkRoomPlayer
    {
        [SerializeField] private bool debugging = false;

        [Header("Events")]
        [SerializeField] private MirrorLobbyPlayerScriptableEvent OnAuthorityStarted;
        [SerializeField] private LobbyInfoScriptableEvent OnLobbyChanged;

        [SyncVar(hook = nameof(HandleLobbyChanged))]
        public LobbyInfo lobbyInfo;
        private bool isLeader = false;
        public bool isReady = false;

        private new void Start()
        {
            base.Start();
            if (Room.roomSlots.Count == 1)
            {
                isLeader = true;
            }
            RaiseLobbyChanged();
        }
        public override void OnDisable()
        {
            base.OnDisable();
            RaiseLobbyChanged();
        }

        [Server]
        private void GetLobbyInfo()
        {
            lobbyInfo = Room.LobbyInfo;
        }
        public override void OnStartAuthority()
        {
            HGDebug.Log($"Starting Lobby Player Authority as player number {Room.roomSlots.Count}", this, debugging);
            if (isServer)
            {
                GetLobbyInfo();
            }
            HGDebug.Log("Starting Lobby Player", this, debugging);
            OnAuthorityStarted.Raise(this);
        }

        public void UpdateDisplayName(string displayName)
        {
            CmdSetDisplayName(displayName);
        }

        [Command]
        private void CmdSetDisplayName(string displayName)
        {
            HGDebug.Log("Commanding DisplayName change", this, debugging);
            this.lobbyInfo = new LobbyInfo(this.lobbyInfo.HostAddress, displayName, this.lobbyInfo.LobbyName,
                this.lobbyInfo.ConnectedPlayers, this.lobbyInfo.MaxPlayers, this.lobbyInfo.Description);
        }
        public void ToggleReadyStatus()
        {
            isReady = !isReady;
            CmdChangeReadyState(isReady);
        }
        public void RaiseLobbyChanged()
        {
            HGDebug.Log("Update Display Called", this, debugging);
            if (!hasAuthority)
            {
                foreach (MirrorLobbyPlayer player in Room.roomSlots)
                {
                    if (player.hasAuthority)
                    {
                        player.RaiseLobbyChanged();
                        break;
                    }
                }
                return;
            }
            OnLobbyChanged.Raise(lobbyInfo);
        }
        public void LeaveLobby()
        {
            if (isServer)
            {
                Room.StopServer();
            }
            else
            {
                Room.StopClient();
            }
        }
        public void HandleLobbyChanged(LobbyInfo oldValue, LobbyInfo newValue) => RaiseLobbyChanged();
        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState) => RaiseLobbyChanged();
        public bool IsLeader
        {
            get => isLeader;
            set
            {
                isLeader = value;
                //mirrorLobbyPlayerUI.StartGameButton.gameObject.SetActive(true);
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