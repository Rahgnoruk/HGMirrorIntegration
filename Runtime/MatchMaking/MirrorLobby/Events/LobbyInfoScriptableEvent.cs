using HyperGnosys.Core;
using UnityEngine;

namespace HyperGnosys.MirrorIntegration
{
    [CreateAssetMenu(fileName ="Lobby Info Scriptable Event", 
        menuName ="HGMirrorIntegration/Events/Lobby Info Scriptable Event")]
    public class LobbyInfoScriptableEvent : AScriptableEvent<LobbyInfo>
    {
    }
}