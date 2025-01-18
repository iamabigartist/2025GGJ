using Sirenix.OdinInspector;
namespace Projects.Demo0.Core.Utils
{
public class MonoSingleton<T> : SerializedMonoBehaviour where T : MonoSingleton<T>
{
	public static T Instance { get; private set; }
	protected virtual void Awake() => Instance = this as T;
}

}