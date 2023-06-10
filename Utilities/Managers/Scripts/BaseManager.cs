﻿// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using UnityEngine;

namespace FiveSQD.WebVerse.Utilities
{
    /// <summary>
    /// Base class for a manager.
    /// </summary>
    public class BaseManager : MonoBehaviour
    {
        /// <summary>
        /// Initialize the manager.
        /// </summary>
        public virtual void Initialize()
        {
            WorldEngine.Utilities.LogSystem.Log("[" + GetType().Name + "] Initialized.");
        }

        /// <summary>
        /// Terminate the manager.
        /// </summary>
        public virtual void Terminate()
        {
            WorldEngine.Utilities.LogSystem.Log("[" + GetType().Name + "] Terminated.");
        }
    }
}