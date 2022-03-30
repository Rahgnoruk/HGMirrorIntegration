using System;
namespace HyperGnosys.MirrorIntegration
{
    [Serializable]
    public class LobbyInfo
    {
        private string hostAddress;
        private string localPlayerDisplayName;
        private string lobbyName;
        private int connectedPlayers;
        private int maxPlayers;
        private string description;

        public LobbyInfo() { }
        public LobbyInfo(string hostAddress, string playerDisplayName, string lobbyName, 
            int connectedPlayers, int maxPlayers, string description)
        {
            this.hostAddress = hostAddress;
            this.localPlayerDisplayName = playerDisplayName;
            this.lobbyName = lobbyName;
            this.connectedPlayers = connectedPlayers;
            this.maxPlayers = maxPlayers;
            this.description = description;
        }
        public string HostAddress { get => hostAddress; protected set => hostAddress = value; }
        public string LocalPlayerDisplayName { get => localPlayerDisplayName; protected set => localPlayerDisplayName = value; }
        public string LobbyName { get => lobbyName; protected set => lobbyName = value; }
        public int ConnectedPlayers { get => connectedPlayers; protected set => connectedPlayers = value; }
        public int MaxPlayers { get => maxPlayers; protected set => maxPlayers = value; }
        public string Description { get => description; protected set => description = value; }
    }
}