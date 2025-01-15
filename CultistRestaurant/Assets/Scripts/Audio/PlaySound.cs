using UnityEngine;

namespace Audio
{
    public class PlaySound : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.AddComponent<AkAudioListener>();

            var result = AkSoundEngine.LoadBank("Main.bnk", out var bankId);
            if (result != AKRESULT.AK_Success)
            {
                Debug.LogError(
                    $"WwiseUnity: Failed load bnk with result: {result}, id: {bankId}, path is: {AkBasePathGetter.Get().SoundBankBasePath}");
            }
        }

        private void OnEnable()
        {
            uint eventId = AkSoundEngine.GetIDFromString("Play_TestSpeech");
            AkSoundEngine.PostEvent(eventId, gameObject);
        }
    }
}