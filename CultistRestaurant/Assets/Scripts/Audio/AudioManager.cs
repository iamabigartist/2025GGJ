using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        public static AudioManager Instance
        {
            get
            {
                _instance = FindAnyObjectByType<AudioManager>();
                if (_instance == null)
                {
                    Debug.LogError("AudioManager Instance Not Found!");
                }
                return _instance;
            }
        }

        public GameObject globalInitializer;
        public AkWwiseInitializationSettings wwiseSetting;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            Init();
        }


        private void Init()
        {
            // AkInitializer
            globalInitializer.SetActive(false);
            AkInitializer akInitializer = globalInitializer.AddComponent<AkInitializer>();
            akInitializer.InitializationSettings = wwiseSetting;
            globalInitializer.SetActive(true);

            // AkAudioListener
            AkAudioListener listener = globalInitializer.AddComponent<AkAudioListener>();
            listener.SetIsDefaultListener(true);
            listener.listenerId = 0;

            // Load Main.bnk
            var result = AkSoundEngine.LoadBank("Main.bnk", out var bankId);
            if (result != AKRESULT.AK_Success)
            {
                Debug.LogError(
                    $"WwiseUnity: Failed load bnk with result: {result}, id: {bankId}, path is: {AkBasePathGetter.Get().SoundBankBasePath}");
            }
        }

        public uint PostEvent(string eventName, GameObject go)
        {
            uint eventId = AkSoundEngine.GetIDFromString(eventName);
            uint uuid = AkSoundEngine.PostEvent(eventId, go);
            Debug.Log($"Post event complete, event name is: {eventName}, uuid is: {uuid}");
            return uuid;
        }
    }
}