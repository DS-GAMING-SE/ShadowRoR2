using EnemiesReturns.Components;
using RoR2;
using RoR2.ContentManagement;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using static ShadowMod.Characters.Survivors.Shadow.Paths.ShadowPaths;

namespace ShadowMod.Characters.Survivors.Shadow.Paths
{
    public class BlackDoom : MonoBehaviour, IInteractable, IInspectable, IDisplayNameProvider
    {
        public static InteractableSpawnCard blackDoomSpawnCard;
        public static GameObject blackDoomInteractablePrefab;
        internal static void Init()
        {
            TeleporterInteraction.onTeleporterChargedGlobal += TrySpawnBlackDoom;

            blackDoomInteractablePrefab = AssetAsyncReferenceManager<GameObject>.LoadAsset(ShadowAssets.blackDoomInteractable).WaitForCompletion();

            blackDoomSpawnCard = ScriptableObject.CreateInstance<InteractableSpawnCard>();
            blackDoomSpawnCard.prefab = blackDoomInteractablePrefab;
            blackDoomSpawnCard.hullSize = HullClassification.Human;
            blackDoomSpawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            blackDoomSpawnCard.occupyPosition = true;

            blackDoomInteractablePrefab.AddComponent<BlackDoom>();
            blackDoomInteractablePrefab.AddComponent<NetworkIdentity>();
            blackDoomInteractablePrefab.AddComponent<Highlight>().targetRenderer = blackDoomInteractablePrefab.transform.Find("Model").GetComponent<Renderer>();
            blackDoomInteractablePrefab.transform.Find("Trigger").gameObject.AddComponent<EntityLocator>().entity = blackDoomInteractablePrefab;
            var inspect = ScriptableObject.CreateInstance<InspectDef>();
            var info = inspect.Info = new RoR2.UI.InspectInfo();

            info.Visual = AssetAsyncReferenceManager<Sprite>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_MiscIcons.texMysteryIcon_png).WaitForCompletion();
            info.DescriptionToken = ShadowSurvivor.SHADOW_PREFIX + "BLACK_DOOM_DESCRIPTION";
            info.TitleToken = ShadowSurvivor.SHADOW_PREFIX + "BLACK_DOOM_NAME";
            inspect.Info = info;

            blackDoomInteractablePrefab.gameObject.AddComponent<GenericInspectInfoProvider>().InspectInfo = inspect;
        }

        private static void TrySpawnBlackDoom(TeleporterInteraction teleporter)
        {
            if (!NetworkServer.active || !EligibleShadowExists()) return;

            teleporter.AttemptSpawnPortal(blackDoomSpawnCard, 0f, 30f, ShadowSurvivor.SHADOW_PREFIX + "BLACK_DOOM_SPAWN");
        }
        public static bool EligibleShadowExists()
        {
            foreach (var player in PlayerCharacterMasterController.instances)
            {
                if (player.master.backupBodyIndex == shadowBodyIndex || (player.body && player.body.bodyIndex == shadowBodyIndex))
                {
                    ShadowPaths.Path path = GetPath(player.master.inventory);
                    if (path == ShadowPaths.Path.Dark || path == ShadowPaths.Path.Neutral)
                    {
                        if (player.master.inventory.GetItemCountPermanent(Items.doomPowerItemDef.itemIndex) < ShadowStaticValues.doomPowersMax)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public IInspectInfoProvider inspectInfoProvider;
        public List<CharacterMaster> masterBlacklist = new List<CharacterMaster>();

        private void OnEnable()
        {
            InterfaceInstanceTracker<IInteractable>.Add(this);
        }

        private void OnDisable()
        {
            InterfaceInstanceTracker<IInteractable>.Remove(this);
        }

        private void Awake()
        {
            this.inspectInfoProvider = base.GetComponent<IInspectInfoProvider>();
        }

        public string GetDisplayName()
        {
            return Language.GetString(ShadowSurvivor.SHADOW_PREFIX + "BLACK_DOOM_NAME");
        }
        public string GetContextString(Interactor activator)
        {
            return Language.GetString(ShadowSurvivor.SHADOW_PREFIX + "BLACK_DOOM_CONTEXT");
        }

        public Interactability GetInteractability(Interactor activator)
        {
            if (enabled && activator.TryGetComponent(out CharacterBody characterBody) && characterBody.master && !masterBlacklist.Contains(characterBody.master))
            {
                return characterBody.bodyIndex == shadowBodyIndex ? Interactability.Available : Interactability.ConditionsNotMet;
            }
            return Interactability.Disabled;
        }
        public void OnInteractionBegin(Interactor activator)
        {
            if (enabled && activator.TryGetComponent(out CharacterBody characterBody) && characterBody.master)
            {
                DoomPowerOrb orb = new DoomPowerOrb();
                orb.target = characterBody.mainHurtBox;
                OrbManager.instance.AddOrb(orb);
                masterBlacklist.Add(characterBody.master);
            }
        }

        public bool ShouldIgnoreSpherecastForInteractibility(Interactor activator)
        {
            return false;
        }

        public bool ShouldProximityHighlight()
        {
            return true;
        }

        public bool ShouldShowOnScanner()
        {
            return true;
        }

        public IInspectInfoProvider GetInspectInfoProvider()
        {
            return this.inspectInfoProvider;
        }
    }
}
