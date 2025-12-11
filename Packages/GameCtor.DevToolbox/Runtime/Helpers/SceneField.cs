using UnityEngine;

namespace GameCtor.DevToolbox
{
    // More feature-filled alternative: https://github.com/starikcetin/Eflatun.SceneReference
    [System.Serializable]
    public sealed class SceneField
    {
        [SerializeField]
        private Object _sceneAsset;

        [SerializeField]
        private string _sceneName = "";
        public string SceneName => _sceneName;

        public static implicit operator string(SceneField sceneField)
        {
            return sceneField.SceneName;
        }
    }
}
