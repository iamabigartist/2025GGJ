using System;
using UnityEngine;

namespace Audio
{
    public class TestPlaySound : MonoBehaviour
    {
        private void OnEnable()
        {
            AudioManager.Instance.PostEvent("Play_TestSpeech", gameObject);
        }
    }
}