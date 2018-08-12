using System.IO;
using Scripts.FX;
using Shiroi.FX.Effects;
using UnityEditor;
using UnityEngine;

namespace Shiroi.FX.Editor {
    public static class ShiroiFXEditor {
        [MenuItem("Shiroi/FX/Create Composite Effect", false, 5)]
        public static void CreateCompositeEffect() {
            CreateEffect<CompositeEffect>();
        }

        [MenuItem("Shiroi/FX/Create Particle Effect", false, 5)]
        public static void CreateParticleEffect() {
            CreateEffect<ParticleEffect>();
        }

        [MenuItem("Shiroi/FX/Create Audio Effect", false, 5)]
        public static void CreateAudioEffect() {
            CreateEffect<AudioEffect>();
        }

        public static void CreateEffect<T>() where T : ScriptableObject {
            var effect = ScriptableObject.CreateInstance<T>();
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "") {
                path = "Assets";
            } else if (Path.GetExtension(path) != "") {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/New{typeof(T).Name}.asset");
            AssetDatabase.CreateAsset(effect, assetPathAndName);
            AssetDatabase.SaveAssets();
        }
    }
}