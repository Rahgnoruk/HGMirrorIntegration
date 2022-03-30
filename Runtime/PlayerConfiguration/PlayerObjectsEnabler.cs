using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace HyperGnosys.MirrorIntegration
{
    public class PlayerObjectsEnabler : NetworkBehaviour
    {
        [SerializeField] private UnityEvent<bool> localOnlyObjects;
        [SerializeField] private UnityEvent<bool> remoteOnlyObjects;
        [SerializeField] private UnityEvent<bool> sharedObjects;
        [SerializeField] private UnityEvent<bool> serverObjects;

        private void Start()
        {   
            SetupPlayer();
            if (isServer)
            {
                ServerSetup();
            }
        }

        private void ServerSetup()
        {
            if (!isClient)
            {
                localOnlyObjects.Invoke(false);
                remoteOnlyObjects.Invoke(false);
                sharedObjects.Invoke(false);
            }
            serverObjects.Invoke(true);
        }

        private void SetupPlayer()
        {
            serverObjects.Invoke(false);
            if (isLocalPlayer)
            {
                remoteOnlyObjects.Invoke(false);
            }
            else
            {
                localOnlyObjects.Invoke(false);
            }
            EnablePlayer();
        }

        private void EnablePlayer()
        {
            sharedObjects.Invoke(true);

            if (isLocalPlayer)
            {
                localOnlyObjects.Invoke(true);
            }
            else
            {
                remoteOnlyObjects.Invoke(true);
            }
        }

        private void DisablePlayer()
        {
            sharedObjects.Invoke(false);

            if (isLocalPlayer)
            {
                localOnlyObjects.Invoke(false);
            }
            else
            {
                remoteOnlyObjects.Invoke(false);
            }
        }
    }
}