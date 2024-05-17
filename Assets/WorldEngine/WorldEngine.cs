// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

namespace FiveSQD.WebVerse.WorldEngine
{
    /// <summary>
    /// Class for the World Engine.
    /// </summary>
    public class WorldEngine : MonoBehaviour
    {
        /// <summary>
        /// Material to use for object highlighting.
        /// </summary>
        [Tooltip("Material to use for object highlighting.")]
        public Material highlightMaterial;

        /// <summary>
        /// Material to use for object previewing.
        /// </summary>
        [Tooltip("Material to use for object previewing.")]
        public Material previewMaterial;

        /// <summary>
        /// Material to use for the environment sky.
        /// </summary>
        [Tooltip("Material to use for the environment sky.")]
        public Material skyMaterial;

        /// <summary>
        /// Prefab for an input entity.
        /// </summary>
        [Tooltip("Prefab for an input entity.")]
        public GameObject inputEntityPrefab;

        /// <summary>
        /// Prefab for a webview.
        /// </summary>
        [Tooltip("Prefab for a webview.")]
        public GameObject webViewPrefab;

        /// <summary>
        /// Prefab for a canvas webview.
        /// </summary>
        [Tooltip("Prefab for a canvas webview.")]
        public GameObject canvasWebViewPrefab;

        /// <summary>
        /// Prefab for a character controller.
        /// </summary>
        [Tooltip("Prefab for a character controller.")]
        public GameObject characterControllerPrefab;

        /// <summary>
        /// Prefab for a voxel block.
        /// </summary>
        [Tooltip("Prefab for a voxel block.")]
        public GameObject voxelPrefab;

        /// <summary>
        /// Camera offset.
        /// </summary>
        [Tooltip("Camera offset.")]
        public GameObject cameraOffset;

        /// <summary>
        /// Whether or not world is in VR mode.
        /// </summary>
        [Tooltip("Whether or not world is in VR mode.")]
        public bool vr;

        /// <summary>
        /// The active world loaded by the world engine.
        /// </summary>
        public static World.World ActiveWorld
        {
            get
            {
                return instance.currentWorld;
            }
        }

        /// <summary>
        /// The instance of the world engine.
        /// </summary>
        private static WorldEngine instance;

        /// <summary>
        /// The current world in the world engine.
        /// </summary>
        private World.World currentWorld = null;

        /// <summary>
        /// The GameObject for the current world.
        /// </summary>
        private GameObject currentWorldGO;

        /// <summary>
        /// The URL query parameters for the current world.
        /// </summary>
        private Dictionary<string, string> queryParams;

        /// <summary>
        /// Load a world.
        /// </summary>
        /// <param name="worldName">Name for the world.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public static bool LoadWorld(string worldName, string queryParams = null)
        {
            if (instance.currentWorld != null)
            {
                Utilities.LogSystem.LogError("[WorldEngine->LoadWorld] Cannot load world. A world is loaded.");
                return false;
            }

            World.World.WorldInfo wInfo = new World.World.WorldInfo()
            {
                highlightMaterial = instance.highlightMaterial,
                previewMaterial = instance.previewMaterial,
                skyMaterial = instance.skyMaterial,
                inputEntityPrefab = instance.inputEntityPrefab,
                webViewPrefab = instance.webViewPrefab,
                canvasWebViewPrefab = instance.canvasWebViewPrefab,
                characterControllerPrefab = instance.characterControllerPrefab,
                voxelPrefab = instance.voxelPrefab,
                cameraOffset = instance.cameraOffset,
                vr = instance.vr,
                maxStorageEntries = 2048,
                maxEntryLength = 2048,
                maxKeyLength = 128
            };

            instance.currentWorldGO = new GameObject(worldName);
            instance.currentWorldGO.transform.parent = instance.transform;
            instance.currentWorld = instance.currentWorldGO.AddComponent<World.World>();
            instance.currentWorld.Initialize(wInfo);

            instance.queryParams = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(queryParams))
            {
                instance.LoadQueryParams(queryParams);
            }

            return true;
        }

        /// <summary>
        /// Unload the current world.
        /// </summary>
        public static void UnloadWorld()
        {
            if (instance.currentWorld != null)
            {
                instance.currentWorld.Unload();
            }
            instance.currentWorld = null;
        }

        /// <summary>
        /// Get a URL Query Parameter.
        /// </summary>
        /// <param name="key">Key of the Query Parameter.</param>
        /// <returns>The value of the Query Parameter, or null.</returns>
        public string GetParam(string key)
        {
            if (queryParams.ContainsKey(key))
            {
                return queryParams[key];
            }

            return null;
        }

        /// <summary>
        /// Load the URL Query Parameters.
        /// </summary>
        /// <param name="rawParams">Raw parameter string.</param>
        private void LoadQueryParams(string rawParams)
        {
            if (queryParams == null)
            {
                Utilities.LogSystem.LogError("[WorldEngine->LoadQueryParams] WorldEngine not initialized.");
                return;
            }

            string[] kvps = rawParams.Replace("%26", "&").Split("&");
            foreach (string kvp in kvps)
            {
                string[] param = kvp.Split("=");
                if (param.Length != 2)
                {
                    Utilities.LogSystem.LogWarning("[WorldEngine->LoadQueryParams] Invalid parameter " + param);
                    continue;
                }

                queryParams.Add(param[0], param[1]);
            }
        }

        /// <summary>
        /// Unity Awake method.
        /// </summary>
        void Awake()
        {
            instance = this;
        }
    }
}