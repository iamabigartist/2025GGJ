using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 在 Inspector 中设置需要加载的场景名称
    [SerializeField] private string sceneName;

    // 调用此方法时加载指定的场景
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is not set. Please assign a scene name in the Inspector.");
        }
    }
}
