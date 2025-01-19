using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio
{
    public class TestPlaySound : MonoBehaviour
    {
        public string eventName;
        [ReadOnly] public uint uuid;
        private void OnEnable()
        {
            uuid = AudioManager.Instance.PostEvent(eventName, gameObject);
        }
        
        private void OnDisable()
        {
            AudioManager.Instance.StopPlayingID(uuid);
        }
    }
}