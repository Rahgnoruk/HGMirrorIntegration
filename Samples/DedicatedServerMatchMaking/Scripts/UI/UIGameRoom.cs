using TMPro;
using UnityEngine;

namespace HyperGnosys.MirrorIntegration
{
    public class UIGameRoom : MonoBehaviour
    {
        [SerializeField] private GameObject playerIconPrefab;
        [SerializeField] private Transform playersScrollViewContainer;
        [SerializeField] private TMP_Text matchIDLabel;

        public void JoinRoom(JoinRoomInfo roomInfo)
        {
            GameObject playerIconInstance = Instantiate(playerIconPrefab, playersScrollViewContainer);
            PlayerIcon playerIcon = playerIconInstance.GetComponent<PlayerIcon>();
            if (roomInfo != null)
            {
                matchIDLabel.text = roomInfo.MatchID;
                playerIcon.SetCaption($"Player {roomInfo.PlayerNumber}");
            }
        }
    }
}