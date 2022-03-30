using HyperGnosys.Core;
using Mirror;
using UnityEngine;

namespace HyperGnosys.MirrorIntegration
{
    public class PlayerRPCs : NetworkBehaviour
    {
        [SerializeField] private ExternalizableLabeledProperty<float> health;
        [SerializeField] private ExternalizableLabeledProperty<float> stamina;
        [SerializeField] private ExternalizableLabeledProperty<string> currentActivableDescription;
        [SerializeField] private ExternalizableLabeledProperty<float> xVelocity;
        [SerializeField] private ExternalizableLabeledProperty<float> yVelocity;
        private void Start()
        {
            if (!isServer) { return; }
            health.AddListener(RpcSyncHealth);
            stamina.AddListener(RpcSyncStamina);
            currentActivableDescription.AddListener(RpcSyncActivableDescription);
            xVelocity.AddListener(RpcSyncXVelocity);
            yVelocity.AddListener(RpcSyncYVelocity);
        }
        [ClientRpc]
        public void RpcSyncHealth(float sentHealth)
        {
            if (isServer)
            {
                return;
            }
            health.Value = sentHealth;
        }
        [ClientRpc]
        public void RpcSyncStamina(float sentStamina)
        {
            if (isServer)
            {
                return;
            }
            stamina.Value = sentStamina;
        }
        [ClientRpc]
        private void RpcSyncActivableDescription(string sentDescription)
        {
            if (isServer)
            {
                return;
            }
            currentActivableDescription.Value = sentDescription;
        }
        [ClientRpc]
        public void RpcSyncXVelocity(float sentVelocity)
        {
            if (isServer)
            {
                return;
            }
            xVelocity.Value = sentVelocity;
        }
        [ClientRpc]
        public void RpcSyncYVelocity(float sentVelocity)
        {
            if (isServer)
            {
                return;
            }
            yVelocity.Value = sentVelocity;
        }
    }
}