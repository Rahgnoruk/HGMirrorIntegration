using HyperGnosys.Core;
using Mirror;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace HyperGnosys.MirrorIntegration
{
    public class MirrorLobbyNetworkManager : NetworkRoomManager
    {
        [SerializeField] private bool debugging = false;
        [Header("Events")]
        [SerializeField] private UnityEvent<NetworkConnection> onServerReadied = new UnityEvent<NetworkConnection>();
        [SerializeField] private UnityEvent onServerStopped = new UnityEvent();
        [SerializeField] private BoolScriptableEvent onAllPlayersReady;

        public LobbyInfo LobbyInfo = new LobbyInfo();
        public void StartHost(LobbyInfo lobbyInfo)
        {
            this.LobbyInfo = lobbyInfo;
            networkAddress = lobbyInfo.HostAddress;
            StartHost();
        }
        public void StartClient(LobbyInfo lobbyInfo)
        {
            this.LobbyInfo = lobbyInfo;
            networkAddress = lobbyInfo.HostAddress;
            StartClient();
        }
        public override void OnStartServer()
        {
            spawnPrefabs = Resources.LoadAll<GameObject>("Spawnable").ToList();
            base.OnStartServer();
        }

        public override void OnStartClient()
        {
            ///The result is the same as OnStartServer
            GameObject[] spawnablePrefabs = Resources.LoadAll<GameObject>("Spawnable");
            foreach (GameObject prefab in spawnPrefabs)
            {
                NetworkClient.RegisterPrefab(prefab);
            }
            base.OnStartClient();
        }

        //Called on the server when the client asks to add a player with ClientScene.AddPlayer
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            HGDebug.Log($"NetworkRoomManager.OnServerAddPlayer playerPrefab:{roomPlayerPrefab.name}", this, debugging);
            base.OnServerAddPlayer(conn);
        }
        //Called in OnServerAddPlayer so the player object can be customised.
        public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
        {
            MirrorLobbyPlayer lobbyPlayer = Instantiate(roomPlayerPrefab as MirrorLobbyPlayer, Vector3.zero, Quaternion.identity);
            lobbyPlayer.lobbyInfo = this.LobbyInfo;
            return lobbyPlayer.gameObject;
        }
        public override void OnRoomServerPlayersReady()
        {
            onAllPlayersReady.Raise(allPlayersReady);
        }
        public override void OnRoomServerPlayersNotReady()
        {
            onAllPlayersReady.Raise(allPlayersReady);
        }
        public void StartGame()
        {
            ServerChangeScene(GameplayScene);
        }
        public UnityEvent<NetworkConnection> OnServerReadied { get => onServerReadied; set => onServerReadied = value; }
        public UnityEvent OnServerStopped { get => onServerStopped; set => onServerStopped = value; }
    }
}