using System;
using System.Collections.Generic;
using UnityEngine;

namespace GutFeeling.Environment
{
    /// <summary>
    /// Dynamically toggles realtime shadows on registered scene lights based on squared distance
    /// from the active camera and a strict concurrent shadow-caster budget.
    /// Keeps <see cref="Light.enabled"/> untouched so gameplay systems (e.g. PowerOutageManager)
    /// retain sole ownership of light power states.
    /// </summary>
    public sealed class RoomLightShadowOptimizer : MonoBehaviour
    {
        [Serializable]
        public sealed class ManagedShadowLight
        {
            [Tooltip("Descriptive label for Inspector readability.")]
            public string label;

            [Tooltip("Target Light whose shadow casting is throttled by distance.")]
            public Light targetLight;

            [Tooltip("Optional reference transform for distance checks; defaults to targetLight.transform.")]
            public Transform distanceAnchor;

            [Tooltip("Maximum camera distance (in meters) at which this light is allowed to cast shadows.")]
            [Min(0.5f)]
            public float maxShadowDistance = 5.0f;

            [Tooltip("Shadow mode restored when within range and within the concurrent caster budget.")]
            public LightShadows activeShadowMode = LightShadows.Soft;

            [NonSerialized] public float normalizedSqrScore;
            [NonSerialized] public bool isEligible;
        }

        [Header("Camera Reference")]
        [Tooltip("Optional explicit camera reference. Leave empty to auto-resolve Camera.main or the active scene Camera.")]
        [SerializeField] private Camera targetCamera;

        [Header("Evaluation Timing")]
        [Tooltip("Seconds between distance/budget evaluations. Zero GC allocations per tick.")]
        [SerializeField, Range(0.05f, 1.0f)] private float checkInterval = 0.15f;

        [Header("Concurrent Shadow Budget")]
        [Tooltip("Maximum number of registered lights allowed to cast realtime shadows simultaneously.")]
        [SerializeField, Range(1, 4)] private int maxActiveShadowCasters = 2;

        [Header("Managed Shadow Lights")]
        [SerializeField] private List<ManagedShadowLight> managedLights = new List<ManagedShadowLight>();

        private Camera cachedCamera;
        private float evaluationTimer;
        private int[] selectedIndices = new int[4];

        /// <summary>
        /// Read-only view of registered shadow lights for tooling or diagnostics.
        /// </summary>
        public IReadOnlyList<ManagedShadowLight> ManagedLights => managedLights;

        private void Awake()
        {
            EnsureBudgetBufferCapacity();
            CacheInitialSettings();
        }

        private void OnEnable()
        {
            evaluationTimer = checkInterval;
            EvaluateShadows();
        }

        private void Update()
        {
            evaluationTimer += Time.unscaledDeltaTime;
            if (evaluationTimer < checkInterval)
                return;

            evaluationTimer = 0f;
            EvaluateShadows();
        }

        private void OnValidate()
        {
            checkInterval = Mathf.Clamp(checkInterval, 0.05f, 1.0f);
            maxActiveShadowCasters = Mathf.Clamp(maxActiveShadowCasters, 1, 4);

            if (managedLights == null)
                return;

            for (int i = 0; i < managedLights.Count; i++)
            {
                if (managedLights[i] != null && managedLights[i].maxShadowDistance < 0.5f)
                {
                    managedLights[i].maxShadowDistance = 0.5f;
                }
            }
        }

        /// <summary>
        /// Evaluates camera-to-light squared distances and assigns shadows to at most
        /// <see cref="maxActiveShadowCasters"/> eligible lights.
        /// </summary>
        public void EvaluateShadows()
        {
            if (managedLights == null || managedLights.Count == 0)
                return;

            Camera activeCam = ResolveActiveCamera();
            if (activeCam == null)
                return;

            EnsureBudgetBufferCapacity();

            Vector3 cameraPos = activeCam.transform.position;
            int selectedCount = 0;

            for (int i = 0; i < managedLights.Count; i++)
            {
                ManagedShadowLight entry = managedLights[i];
                if (entry == null || entry.targetLight == null)
                    continue;

                entry.isEligible = false;

                if (!entry.targetLight.gameObject.activeInHierarchy || !entry.targetLight.enabled)
                    continue;

                if (entry.activeShadowMode == LightShadows.None)
                    continue;

                Transform anchor = entry.distanceAnchor != null ? entry.distanceAnchor : entry.targetLight.transform;
                float sqrDist = (anchor.position - cameraPos).sqrMagnitude;
                float maxDist = Mathf.Max(0.5f, entry.maxShadowDistance);
                float sqrMaxDist = maxDist * maxDist;

                if (sqrDist > sqrMaxDist)
                    continue;

                entry.isEligible = true;
                entry.normalizedSqrScore = sqrDist / sqrMaxDist;

                selectedCount = InsertIntoBudgetBuffer(i, entry.normalizedSqrScore, selectedCount);
            }

            for (int i = 0; i < managedLights.Count; i++)
            {
                ManagedShadowLight entry = managedLights[i];
                if (entry == null || entry.targetLight == null)
                    continue;

                bool shouldCast = entry.isEligible && IsIndexSelected(i, selectedCount);
                LightShadows desired = shouldCast ? entry.activeShadowMode : LightShadows.None;

                if (entry.targetLight.shadows != desired)
                {
                    entry.targetLight.shadows = desired;
                }
            }
        }

        private Camera ResolveActiveCamera()
        {
            if (targetCamera != null && targetCamera.isActiveAndEnabled)
                return targetCamera;

            if (cachedCamera != null && cachedCamera.isActiveAndEnabled)
                return cachedCamera;

            cachedCamera = Camera.main;
            if (cachedCamera == null)
            {
                cachedCamera = FindAnyObjectByType<Camera>(FindObjectsInactive.Exclude);
            }

            return cachedCamera;
        }

        private void EnsureBudgetBufferCapacity()
        {
            if (selectedIndices == null || selectedIndices.Length < maxActiveShadowCasters)
            {
                selectedIndices = new int[maxActiveShadowCasters];
            }
        }

        private void CacheInitialSettings()
        {
            if (managedLights == null)
                return;

            for (int i = 0; i < managedLights.Count; i++)
            {
                ManagedShadowLight entry = managedLights[i];
                if (entry != null && entry.targetLight != null && entry.activeShadowMode == LightShadows.None)
                {
                    entry.activeShadowMode = entry.targetLight.shadows;
                }
            }
        }

        private int InsertIntoBudgetBuffer(int entryIndex, float score, int currentCount)
        {
            int insertPos = currentCount;
            while (insertPos > 0)
            {
                int prevEntryIndex = selectedIndices[insertPos - 1];
                if (managedLights[prevEntryIndex].normalizedSqrScore <= score)
                    break;

                if (insertPos < maxActiveShadowCasters)
                {
                    selectedIndices[insertPos] = prevEntryIndex;
                }
                insertPos--;
            }

            if (insertPos < maxActiveShadowCasters)
            {
                selectedIndices[insertPos] = entryIndex;
                if (currentCount < maxActiveShadowCasters)
                {
                    currentCount++;
                }
            }

            return currentCount;
        }

        private bool IsIndexSelected(int entryIndex, int selectedCount)
        {
            for (int i = 0; i < selectedCount; i++)
            {
                if (selectedIndices[i] == entryIndex)
                    return true;
            }
            return false;
        }
    }
}
