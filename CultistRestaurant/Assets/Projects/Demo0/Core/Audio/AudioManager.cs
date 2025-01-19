using System;
using Sirenix.OdinInspector;
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
                    Debug.LogWarning("AudioManager Instance Not Found!");
                }

                return _instance;
            }
        }

        public GameObject globalInitializer;
        public AkWwiseInitializationSettings wwiseSetting;
        [ReadOnly] public uint uuidMusic;

        // constants
        public static class StateConstants
        {
            public const string GameLevelGrp = "GameLevel";
            public static class GameLevelVal
            {
                public const string StartView = "StartView";
                public const string IntroView = "IntroView";
                public const string FinishView = "FinishView";
                public const string Level01 = "Level01";
                public const string Level02 = "Level02";
                public const string Level03 = "Level03";
                public const string BlackTransition = "BlackTransition";
            }
        }
        
        public static class RtpcConstants
        {
            public const string GlobalPollutionLevel = "GlobalPollutionLevel";
        }


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
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
            
            // Post music event
            uuidMusic = PostEvent("Play_Music", globalInitializer);
            
            // Set init game syncs
            SetStateValue(StateConstants.GameLevelGrp, StateConstants.GameLevelVal.StartView);
            SetGlobalRtpcValue(RtpcConstants.GlobalPollutionLevel, 0);
        }

        public uint PostEvent(string eventName, GameObject go)
        {
            uint eventId = AkSoundEngine.GetIDFromString(eventName);
            uint uuid = AkSoundEngine.PostEvent(eventId, go);
            Debug.Log($"Audio Manager, Post event complete, event name is: {eventName}, uuid is: {uuid}");
            return uuid;
        }
        
        public void StopPlayingID(uint uuid, int fadeoutTime = 100, AkCurveInterpolation fadeoutCurve = AkCurveInterpolation.AkCurveInterpolation_Linear)
        {
            AkSoundEngine.ExecuteActionOnPlayingID(AkActionOnEventType.AkActionOnEventType_Stop, uuid, fadeoutTime, fadeoutCurve);
        }

        public void SetStateValue(string group, string val)
        {
            AkSoundEngine.SetState(group, val);
            Debug.Log($"Audio Manager, SetStateValue, group name is: {group}, val is: {val}");
        }

        public void SetGlobalRtpcValue(string key, float val)
        {
            AkSoundEngine.SetRTPCValue(key, val);
        }
    }
}