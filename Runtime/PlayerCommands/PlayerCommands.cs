using HyperGnosys.Core;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace HyperGnosys.MirrorIntegration
{
    public class PlayerCommands : NetworkBehaviour
    {
        [SerializeField] private bool debugging = false;
        [SerializeField] private UnityEvent<Vector3> onMove = new UnityEvent<Vector3>();
        [SerializeField] private UnityEvent<bool> onAttack = new UnityEvent<bool>();
        [SerializeField] private UnityEvent<bool> onActivate = new UnityEvent<bool>();
        [SerializeField] private UnityEvent<bool> onJump = new UnityEvent<bool>();
        [SerializeField] private UnityEvent<Vector2> onLook = new UnityEvent<Vector2>();
        [SerializeField] private UnityEvent<int> onRun = new UnityEvent<int>();
        [Command] 
        public void CmdMove(Vector3 sentMoveVector)
        {
            onMove.Invoke(sentMoveVector);
        }
        [Command]
        public void CmdAttack(bool sentAttackInput)
        {
            onAttack.Invoke(sentAttackInput);
        }
        [Command]
        public void CmdActivate(bool sentActivateInput)
        {
            onActivate.Invoke(sentActivateInput);
        }
        [Command]
        public void CmdJump(bool sentJumpInput)
        {
            onJump.Invoke(sentJumpInput);
        }
        [Command]
        public void CmdLook(Vector2 sentLookVector)
        {
            onLook.Invoke(sentLookVector);
        }
        [Command]
        public void CmdRun(int sentRunInput)
        {
            onRun.Invoke(sentRunInput);
        }
    }
}