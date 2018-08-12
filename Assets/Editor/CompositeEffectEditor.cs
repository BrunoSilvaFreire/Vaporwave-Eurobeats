using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shiroi.FX.Effects;
using UnityEditor;
using UnityEngine;
using UnityUtilities;
using UnityUtilities.Editor;
using UPM.Editor;

namespace UPM.Editor {
    [InitializeOnLoad]
    public static class UPMAssemblyUtil {


        private static readonly List<Assembly> KnownAssemblies = new List<Assembly>();

        private static void Reload() {
            KnownAssemblies.Clear();
        }

        static UPMAssemblyUtil() {
            LoadAssemblies();
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoaded;
        }

        private static void LoadAssemblies() {
            Reload();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                RegisterAssembly(assembly);
            }
        }

        private static void OnAssemblyLoaded(object sender, AssemblyLoadEventArgs args) {
            RegisterAssembly(args.LoadedAssembly);
        }

        private static void RegisterAssembly(Assembly assembly) {
            KnownAssemblies.Add(assembly);
        }

        public static IEnumerable<Type> GetAllTypesOf<T>() {
            var target = typeof(T);
            foreach (var assembly in KnownAssemblies) {
                foreach (var type in assembly.GetTypes()) {
                    if (target.IsAssignableFrom(type)) {
                        yield return type;
                    }
                }
            }
        }
    }
}
namespace Editor {
    [CustomEditor(typeof(CompositeEffect))]
    public class CompositeEffectEditor : UnityEditor.Editor {
        private CompositeEffect effect;
        private CompositeAddContent<WorldEffect> content;
        private CompositeAddContent<GameEffect> gameContent;

        private void OnEnable() {
            effect = (CompositeEffect) target;
            content = new CompositeAddContent<WorldEffect>(effect, worldEffect => effect.Effects.Add(worldEffect));
            gameContent = new CompositeAddContent<GameEffect>(effect, gameEffect => effect.GameEffects.Add(gameEffect));
        }

        public override void OnInspectorGUI() {
            DrawList(content, effect.Effects);
            DrawList(gameContent, effect.GameEffects);
        }

        private void DrawList<T>(PopupWindowContent compositeAddContent, List<T> effectList) where T : ScriptableObject {
            GUILayout.BeginHorizontal();
            var found = (T) EditorGUILayout.ObjectField("Add Existing Effect", null, typeof(T), false);
            if (found != null && effectList.Contains(found)) {
                effectList.Add(found);
            }

            if (GUILayout.Button("Add New Effect")) {
                PopupWindow.Show(new Rect(0, 0, 300, 0), compositeAddContent);
            }

            GUILayout.EndHorizontal();

            for (var i = 0; i < effectList.Count; i++) {
                var effectSubEffect = effectList[i];
                GUILayout.BeginHorizontal();
                effectSubEffect.name = GUILayout.TextField(effectSubEffect.name);
                if (GUILayout.Button("Edit")) {
                    Selection.SetActiveObjectWithContext(effectSubEffect, this);
                }

                if (GUILayout.Button("Delete")) {
                    effectList.RemoveAt(i);
                    DestroyImmediate(effectSubEffect, true);
                }

                GUILayout.EndHorizontal();
            }
        }
    }

    public class CompositeAddContent<T> : PopupWindowContent where T : ScriptableObject {
        private readonly IList<Type> subEffects = UPMAssemblyUtil.GetAllTypesOf<T>().Where(type => type != typeof(T)).ToList();

        private CompositeEffect effect;
        private Action<T> adder;

        public CompositeAddContent(CompositeEffect effect, Action<T> adder) {
            this.effect = effect;
            this.adder = adder;
        }

        public override Vector2 GetWindowSize() {
            return new Vector2(300, subEffects.Count() * EditorGUIUtility.singleLineHeight);
        }

        public override void OnGUI(Rect rect) {
            for (var i = 0; i < subEffects.Count; i++) {
                var subEffect = subEffects[i];
                if (!GUI.Button(rect.GetLine((uint) i), subEffect.Name)) {
                    continue;
                }

                var e = (T) ScriptableObject.CreateInstance(subEffect);
                e.name = subEffect.Name;
                adder(e);
                AssetDatabase.AddObjectToAsset(e, effect);
                AssetDatabase.SaveAssets();
            }
        }
    }

    public static class RectUtil {
        public static Rect GetLine(this Rect rect, uint collum, uint totalLines = 1, float yOffset = 0.0f) {
            return rect.GetLine(collum, EditorGUIUtility.singleLineHeight, totalLines, yOffset);
        }

        public static Rect GetLine(this Rect rect, uint collum, float collumHeight, uint totalLines = 1, float yOffset = 0.0f) {
            float height = (float) totalLines * collumHeight;
            return rect.GetLine(collum, height, collumHeight, yOffset);
        }

        private static Rect GetLine(this Rect rect, uint collum, float height, float collumHeight, float yOffset = 0.0f) {
            return rect.SubRect(rect.width, height, 0.0f, (float) collum * collumHeight + yOffset);
        }

        public static Rect SubRect(this Rect rect, float width, float height, float xOffset = 0.0f, float yOffset = 0.0f) {
            return new Rect(rect.x + xOffset, rect.y + yOffset, width, height);
        }

        public static void Split(this Rect rect, float splitPosition, out Rect a, out Rect b) {
            a = rect.SubRect(splitPosition, rect.height, 0.0f, 0.0f);
            b = rect.SubRect(rect.width - splitPosition, rect.height, splitPosition, 0.0f);
        }
    }
}