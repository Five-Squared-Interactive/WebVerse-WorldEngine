// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.WorldEngine.Camera;
using FiveSQD.WebVerse.WorldEngine.Entity;
using FiveSQD.WebVerse.WorldEngine.Materials;
using FiveSQD.WebVerse.WorldEngine.WorldStorage;
using FiveSQD.WebVerse.WorldEngine.Utilities;
using UnityEngine;

namespace FiveSQD.WebVerse.WorldEngine.World
{
    /// <summary>
    /// Class for a World.
    /// </summary>
    public class World : MonoBehaviour
    {
        /// <summary>
        /// Class for World information.
        /// </summary>
        public class WorldInfo
        {
            /// <summary>
            /// Entity highlight material.
            /// </summary>
            public Material highlightMaterial;

            /// <summary>
            /// Input entity prefab.
            /// </summary>
            public GameObject inputEntityPrefab;

            /// <summary>
            /// Character controller prefab.
            /// </summary>
            public GameObject characterControllerPrefab;

            /// <summary>
            /// Voxel prefab.
            /// </summary>
            public GameObject voxelPrefab;

            /// <summary>
            /// Maximum number of storage entries.
            /// </summary>
            [Range(0, int.MaxValue)]
            public int maxStorageEntries;

            /// <summary>
            /// Maximum length of a storage entry.
            /// </summary>
            [Range(0, int.MaxValue)]
            public int maxEntryLength;

            /// <summary>
            /// Maximum length of a storage key.
            /// </summary>
            [Range(0, int.MaxValue)]
            public int maxKeyLength;
        }

        /// <summary>
        /// The mesh manager for the world.
        /// </summary>
        public MeshManager.MeshManager meshManager { get; private set; }

        /// <summary>
        /// The entity manager for the world.
        /// </summary>
        public EntityManager entityManager { get; private set; }

        /// <summary>
        /// The storage manager for the world.
        /// </summary>
        public WorldStorageManager storageManager { get; private set; }

        /// <summary>
        /// The camera manager for the world.
        /// </summary>
        public CameraManager cameraManager { get; private set; }

        /// <summary>
        /// The material manager for the world.
        /// </summary>
        public MaterialManager materialManager { get; private set; }

        /// <summary>
        /// The GameObject for the mesh manager.
        /// </summary>
        private GameObject meshManagerGO;

        /// <summary>
        /// The GameObject for the entity manager.
        /// </summary>
        private GameObject entityManagerGO;

        /// <summary>
        /// The GameObject for the storage manager.
        /// </summary>
        private GameObject storageManagerGO;

        /// <summary>
        /// The GameObject for the camera manager.
        /// </summary>
        private GameObject cameraManagerGO;

        /// <summary>
        /// The GameObject for the material manager.
        /// </summary>
        private GameObject materialManagerGO;

        /// <summary>
        /// Initialize the World.
        /// </summary>
        /// <param name="worldInfo">World information to use.</param>
        public void Initialize(WorldInfo worldInfo)
        {
            if (meshManager != null)
            {
                LogSystem.LogError("[World->Initialize] Mesh manager already initialized.");
                return;
            }
            meshManagerGO = new GameObject("MeshManager");
            meshManagerGO.transform.parent = transform;
            meshManager = meshManagerGO.AddComponent<MeshManager.MeshManager>();
            meshManager.Initialize();

            if (entityManager != null)
            {
                LogSystem.LogError("[World->Initialize] Entity manager already initialized.");
                return;
            }
            entityManagerGO = new GameObject("EntityManager");
            entityManagerGO.transform.parent = transform;
            entityManager = entityManagerGO.AddComponent<EntityManager>();
            entityManager.Initialize();
            entityManager.inputEntityPrefab = worldInfo.inputEntityPrefab;
            entityManager.characterControllerPrefab = worldInfo.characterControllerPrefab;
            entityManager.voxelPrefab = worldInfo.voxelPrefab;

            if (storageManager != null)
            {
                LogSystem.LogError("[World->Initialize] Storage manager already initialized.");
                return;
            }
            storageManagerGO = new GameObject("StorageManager");
            storageManagerGO.transform.parent = transform;
            storageManager = storageManagerGO.AddComponent<WorldStorageManager>();
            storageManager.Initialize(worldInfo.maxStorageEntries, worldInfo.maxEntryLength, worldInfo.maxKeyLength);

            if (cameraManager != null)
            {
                LogSystem.LogError("[World->Initialize] Camera manager already initialized.");
                return;
            }
            cameraManagerGO = new GameObject("CameraManager");
            cameraManagerGO.transform.parent = transform;
            cameraManager = cameraManagerGO.AddComponent<CameraManager>();
            cameraManager.Initialize(UnityEngine.Camera.main, entityManagerGO);

            if (materialManager != null)
            {
                LogSystem.LogError("[World->Initialize] Material manager already initialized.");
                return;
            }
            materialManagerGO = new GameObject("MaterialManager");
            materialManagerGO.transform.parent = transform;
            materialManager = materialManagerGO.AddComponent<MaterialManager>();
            materialManager.Initialize(worldInfo.highlightMaterial);
        }

        /// <summary>
        /// Unload the world.
        /// </summary>
        public void Unload()
        {
            if (entityManager == null)
            {
                LogSystem.LogError("[World->Unload] No entity manager.");
            }
            else
            {
                entityManager.Unload();
                Destroy(entityManagerGO);
            }

            if (meshManager == null)
            {
                LogSystem.LogError("[World->Unload] No mesh manager.");
            }
            else
            {
                meshManager.Terminate();
                Destroy(meshManagerGO);
            }

            if (storageManager == null)
            {
                LogSystem.LogError("[World->Unload] No storage manager.");
            }
            else
            {
                storageManager.Terminate();
                Destroy(storageManagerGO);
            }

            if (cameraManager == null)
            {
                LogSystem.LogError("[World->Unload] No camera manager.");
            }
            else
            {
                cameraManager.Terminate();
                Destroy(cameraManagerGO);
            }

            if (materialManager == null)
            {
                LogSystem.LogError("[World->Unload] No material manager.");
            }
            else
            {
                materialManager.Terminate();
                Destroy(materialManagerGO);
            }
        }
    }
}