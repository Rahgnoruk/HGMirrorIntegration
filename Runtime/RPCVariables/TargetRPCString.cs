using HyperGnosys.Core;
using Mirror;
using UnityEngine;

namespace HyperGnosys.MirrorIntegration
{
    public class TargetRPCString : NetworkBehaviour
    {
        [SerializeField] private NetworkIdentity targetNetworkIdentity;
        [SerializeField] private ExternalizableLabeledProperty<string> synchronizedString;
        private void Awake()
        {
            if (isServer)
            {
                synchronizedString.AddListener(SendString);
            }
        }
        private void SendString(string sentString)
        {
            TargetSendString(targetNetworkIdentity.connectionToClient, synchronizedString.Value);
        }
        [TargetRpc]
        private void TargetSendString(NetworkConnection targetConnection, string sentString)
        {
            if (isServer)
            {
                return;
            }
            synchronizedString.Value = sentString;
        }
    }
}