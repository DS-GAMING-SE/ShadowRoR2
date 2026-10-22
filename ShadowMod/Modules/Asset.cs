using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.UI;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using Path = System.IO.Path;

namespace ShadowMod.Modules
{
    internal static class Asset
    {
        public static string AddressablesDirectory { get; private set; }
        internal static void LoadAddressables()
        {
            AddressablesDirectory = Path.Combine(Path.GetDirectoryName(ShadowPlugin.instance.Info.Location), "Addressables");
            Addressables.LoadContentCatalogAsync(Path.Combine(AddressablesDirectory, "catalog.json")).WaitForCompletion();
        }

        internal static GameObject CloneTracer(string originalTracerName, string newTracerName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) 
                return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();
            
            newTracer.GetComponent<Tracer>().speed = 250f;
            newTracer.GetComponent<Tracer>().length = 50f;

            Modules.Content.CreateAndAddEffectDef(newTracer);

            return newTracer;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (MeshRenderer i in objectToConvert.GetComponentsInChildren<MeshRenderer>())
            {
                if (i)
                {
                    if (i.sharedMaterial)
                    {
                        i.sharedMaterial.ConvertDefaultShaderToHopoo();
                    }
                }
            }

            foreach (SkinnedMeshRenderer i in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (i)
                {
                    if (i.sharedMaterial)
                    {
                        i.sharedMaterial.ConvertDefaultShaderToHopoo();
                    }
                }
            }
        }

        internal static GameObject LoadCrosshair(string crosshairName)
        {
            GameObject loadedCrosshair = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
            if (loadedCrosshair == null)
            {
                Log.Error($"could not load crosshair with the name {crosshairName}. defaulting to Standard");

                return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            }

            return loadedCrosshair;
        }
        internal static GameObject CreateEffect(GameObject prefab){return CreateEffect(prefab, 10f, false, "", out _, out _);}
        internal static GameObject CreateEffect(GameObject prefab, float destroyOnTimer) { return CreateEffect(prefab, destroyOnTimer, false, "", out _, out _); }
        internal static GameObject CreateEffect(GameObject prefab, float destroyOnTimer, bool parentToTransform, string soundName) { return CreateEffect(prefab, destroyOnTimer, parentToTransform, soundName, out _, out _); }

        internal static GameObject CreateEffect(GameObject prefab, float destroyOnTimer, bool parentToTransform, string soundName, out VFXAttributes vfx, out EffectComponent effect)
        {
            if (!prefab) { Log.Error("Effect failed to load"); }
            if (destroyOnTimer > 0) { prefab.AddComponent<DestroyOnTimer>().duration = destroyOnTimer; }
            prefab.AddComponent<NetworkIdentity>();
            vfx = prefab.AddComponent<VFXAttributes>();
            vfx.vfxPriority = VFXAttributes.VFXPriority.Always;
            effect = prefab.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            Modules.Content.CreateAndAddEffectDef(prefab);

            return prefab;
        }

        internal static GameObject CreateProjectileGhostPrefab(GameObject ghostPrefab)
        {
            if (ghostPrefab == null)
            {
                Log.Error($"Failed to load ghost prefab");
            }
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Asset.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        internal static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}