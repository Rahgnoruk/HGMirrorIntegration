using TMPro;
using UnityEngine;

namespace HyperGnosys.MirrorIntegration
{
    public class PlayerIcon : MonoBehaviour
    {
        [SerializeField] private TMP_Text captionText;
        [SerializeField] private TMP_Text readyText;

        public void SetCaption(string caption)
        {
            captionText.text = caption;
        }
        public void SetReadyText(string text)
        {
            readyText.text = text;
        }
    }
}