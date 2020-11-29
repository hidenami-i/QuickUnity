using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QuickUnity.Editor.Misc
{
    /// <summary>
    /// Set the raycastTarget of UI other than button to false by default.
    /// </summary>
    public static class GraphicDefaultValueWriter
    {
        private static readonly HashSet<Graphic> SceneGraphics = new HashSet<Graphic>();
        private static string scenesHash = string.Empty;
        private static readonly List<GameObject> TempRoots = new List<GameObject>();
        private static readonly List<Graphic> TempGraphics = new List<Graphic>();
        private static readonly HashAlgorithm HashAlgorithm = new SHA512Managed();

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            BuildInitialSceneGraphics(ComputeScenesHash());
        }

        private static void OnHierarchyChanged()
        {
            if (Application.isPlaying)
            {
                scenesHash = string.Empty;
                return;
            }

            var hash = ComputeScenesHash();
            if (hash != scenesHash)
            {
                // New scene, rebuild
                BuildInitialSceneGraphics(hash);
            }
            else
            {
                // Each new graphics, set default value
                foreach (var graphic in GetNewGraphics())
                {
                    graphic.raycastTarget = graphic.GetComponent<IEventSystemHandler>() != null;
                }
            }
        }

        private static void BuildInitialSceneGraphics(string hash)
        {
            SceneGraphics.Clear();
            scenesHash = hash;

            foreach (var graphic in GetSceneGraphics())
            {
                SceneGraphics.Add(graphic);
            }
        }

        private static string ComputeScenesHash()
        {
            var sb = new StringBuilder();
            for (var ss = 0; ss < SceneManager.sceneCount; ss++)
            {
                var scene = SceneManager.GetSceneAt(ss);
                if (scene.isLoaded)
                {
                    sb.Append(scene.path);
                }
            }

            var hash = HashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            var hashSb = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
            {
                hashSb.AppendFormat("{0:X2}", b);
            }

            return hashSb.ToString();
        }

        private static IEnumerable<Scene> GetLoadedScenes()
        {
            for (var ss = 0; ss < SceneManager.sceneCount; ss++)
            {
                var scene = SceneManager.GetSceneAt(ss);
                if (scene.isLoaded)
                {
                    yield return scene;
                }
            }
        }

        private static IEnumerable<Graphic> GetSceneGraphics()
        {
            foreach (var scene in GetLoadedScenes())
            {
                TempRoots.Clear();
                scene.GetRootGameObjects(TempRoots);
                foreach (var root in TempRoots)
                {
                    TempGraphics.Clear();
                    root.GetComponentsInChildren(TempGraphics);
                    foreach (var graphic in TempGraphics)
                    {
                        yield return graphic;
                    }
                }
            }
        }

        private static IEnumerable<Graphic> GetNewGraphics()
        {
            foreach (var graphic in GetSceneGraphics())
            {
                if (!SceneGraphics.Contains(graphic))
                {
                    SceneGraphics.Add(graphic);
                    yield return graphic;
                }
            }
        }
    }
}
