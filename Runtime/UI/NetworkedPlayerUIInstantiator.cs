using Mirror;
using UnityEngine;

namespace HyperGnosys.MirrorIntegration
{
    public class NetworkedPlayerUIInstantiator : MonoBehaviour
    {
        [SerializeField] private Canvas playerCanvas;
        [SerializeField] private NetworkIdentity playerNetId;
        private void Awake()
        {
            playerCanvas.gameObject.SetActive(false);
            if (playerNetId.isLocalPlayer)
            {
                playerCanvas.gameObject.SetActive(true);
            }
        }
    }
}