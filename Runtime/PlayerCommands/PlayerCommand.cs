using HyperGnosys.Core;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace HyperGnosys.MirrorIntegration
{
    [System.Serializable]
    public class PlayerCommand<T>
    {
        [SerializeField] AObservablePropertyComponent<T> property;
        [SerializeField] private UnityEvent<T> responses = new UnityEvent<T>();

        [Command]
        public void CmdMove(T sentMoveVector)
        {
            responses.Invoke(sentMoveVector);
        }
    }
}