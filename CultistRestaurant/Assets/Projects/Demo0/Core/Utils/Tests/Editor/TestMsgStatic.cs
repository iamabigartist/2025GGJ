using UnityEditor;
namespace Projects.Demo0.Core.Utils
{
public static class TestMsgStaticEditor
{
	[MenuItem("Tests/TestMsg")]
	public static void TestMsg()
	{
		TestMsgStatic.Invoke("TestMsgStatic");
	}
}
}