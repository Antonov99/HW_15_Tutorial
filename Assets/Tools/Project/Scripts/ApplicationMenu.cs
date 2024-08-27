#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Game.Development
{
    public sealed class ApplicationMenu
    {
        [MenuItem("Tools/Play Game")]
        private static void Play()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                var name = SceneManager.GetActiveScene().name;
                if (name != "LoadingScene")
                {
                    EditorSceneManager.OpenScene(SceneMenu.LOADING_SCENE_PATH);
                }

                EditorApplication.isPlaying = true;
            }
        }
    }
}
#endif