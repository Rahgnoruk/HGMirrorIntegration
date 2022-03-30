using HyperGnosys.Core;
using Mirror;
using UnityEngine;

namespace HyperGnosys.InteractionModule
{
    public class RPCFloat : NetworkBehaviour
    {
        [SerializeField] private ExternalizableLabeledProperty<float> synchronizedFloat;
        private void Awake()
        {
            if (isServer)
            {
                synchronizedFloat.AddListener(RpcSendFloat);
            }
        }
        [ClientRpc]
        private void RpcSendFloat(float sentFloat)
        {
            if (isServer)
            {
                return;
            }
            synchronizedFloat.Value = sentFloat;
        }
    }
}