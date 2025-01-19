using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class PlaySoundOnButtonClick : MonoBehaviour
    {
        public string eventName;
        [ReadOnly] public uint uuid;
        private Button _button;
        
        void Start()
        {
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(PlaySound);
        }
        
        void PlaySound()
        {
            // uuid = AudioManager.Instance.PostEvent(eventName, AudioManager.Instance.globalInitializer);
        }
    }
}