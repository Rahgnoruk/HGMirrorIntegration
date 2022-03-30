using HyperGnosys.Core;
using Mirror;
using UnityEngine;

namespace HyperGnosys.MirrorIntegration
{
    public class RPCInt : NetworkBehaviour
    {
        [SerializeField] private ExternalizableLabeledProperty<int> synchronizedInt;
        private void Awake()
        {
            if (isServer)
            {
                synchronizedInt.AddListener(RpcSendInt);
            }
        }
        [ClientRpc]
        private void RpcSendInt(int sentInt)
        {
            if (isServer)
            {
                return;
            }
            synchronizedInt.Value = sentInt;
        }
    }
}