using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // �� Inspector ��������Ҫ���صĳ�������
    [SerializeField] private string sceneName;

    // ���ô˷���ʱ����ָ���ĳ���
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
