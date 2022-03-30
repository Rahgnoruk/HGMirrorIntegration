using HyperGnosys.Core;
using Mirror;
using UnityEngine;

namespace HyperGnosys.MirrorIntegration
{
    public class RPCString : NetworkBehaviour
    {
        [SerializeField] private ExternalizableLabeledProperty<string> synchronizedString;
        private void Awake()
        {
            if (isServer)
            {
                synchronizedString.AddListener(RpcSendString);
            }
        }
        [ClientRpc]
        private void RpcSendString(string sentString)
        {
            if (isServer)
            {
                return;
            }
            synchronizedString.Value = sentString;
        }
    }
}