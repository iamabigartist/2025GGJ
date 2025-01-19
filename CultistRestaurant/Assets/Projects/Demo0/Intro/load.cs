using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public PPTController ppt;

    // 调用此方法时加载指定的场景
    public void LoadScene()
    {
        ppt.LoadScene();
    }
}
