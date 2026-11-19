using EnemiesReturns.Components;
using HedgehogUtils.Forms;
using RoR2;
using RoR2.Stats;
using ShadowMod.Characters.Survivors.Shadow.Components;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.Paths
{
    public class ShadowPathsComponent : MonoBehaviour
    {
        public CharacterBody characterBody;
        public FormComponent formComponent;
        public HomingTracker homingTracker;
        public int lastDoomPowerCount;
        private void Awake()
        {
            characterBody = base.GetComponent<CharacterBody>();
            formComponent = GetComponent<FormComponent>();
            homingTracker = GetComponent<HomingTracker>();
        }
        private void OnEnable()
        {
            characterBody.onInventoryChanged += UpdateDoomPowerCount;
            formComponent.OnFormChanged += SetPathOnTransform;
        }
        private void OnDisable()
        {
            characterBody.onInventoryChanged -= UpdateDoomPowerCount;
            formComponent.OnFormChanged -= SetPathOnTransform;
        }

        private void Start()
        {
            if (NetworkServer.active)
            {
                this.TrySetStartingPathToNeutral();
            }
            UpdateDoomPowerCount();
        }
        private void TrySetStartingPathToNeutral()
        {
            if (RunArtifactManager.instance.IsArtifactEnabled(HedgehogUtils.Forms.SuperForm.Artifact.chaosEmeraldArtifactDef) && 
                characterBody.master && characterBody.master.playerStatsComponent && 
                characterBody.master.playerStatsComponent.currentStats.GetStatValueDouble(PerBodyStatDef.totalTimeAlive, BodyCatalog.GetBodyName(characterBody.bodyIndex)) == 0.0)
            {
                ShadowPaths.SetPath(characterBody.master.inventory, ShadowPaths.Path.Neutral);
            }
        }
        public void UpdateDoomPowerCount()
        {
            int doomPowerCount = characterBody.inventory.GetItemCountPermanent(Items.doomPowerItemDef.itemIndex);

            if (doomPowerCount == lastDoomPowerCount) return;

            if (NetworkServer.active && doomPowerCount >= ShadowStaticValues.doomPowersMax) RoR2.Util.KarmaEventTrigger("ChaosEmeraldKarma");

            for (int i = 0; i < DoomPowerDef.doomPowerDefs.Length; i++)
            {
                if (Math.Max(doomPowerCount, lastDoomPowerCount) >= DoomPowerDef.doomPowerDefs[i].requiredDoomPower)
                {
                    if (characterBody.skillLocator.GetSkill(DoomPowerDef.doomPowerDefs[i].skillSlot).baseSkill == DoomPowerDef.doomPowerDefs[i].skillToOverride)
                    {
                        if (doomPowerCount >= DoomPowerDef.doomPowerDefs[i].requiredDoomPower && lastDoomPowerCount < DoomPowerDef.doomPowerDefs[i].requiredDoomPower)
                        {
                            characterBody.skillLocator.GetSkill(DoomPowerDef.doomPowerDefs[i].skillSlot).SetSkillOverride(this, DoomPowerDef.doomPowerDefs[i].skillDef, GenericSkill.SkillOverridePriority.Upgrade);
                            if (DoomPowerDef.doomPowerDefs[i] == DoomPowerDef.doomSpearDef && homingTracker) homingTracker.maxTargets = 5;
                        }
                        else if (doomPowerCount < DoomPowerDef.doomPowerDefs[i].requiredDoomPower && lastDoomPowerCount >= DoomPowerDef.doomPowerDefs[i].requiredDoomPower)
                        {
                            characterBody.skillLocator.GetSkill(DoomPowerDef.doomPowerDefs[i].skillSlot).UnsetSkillOverride(this, DoomPowerDef.doomPowerDefs[i].skillDef, GenericSkill.SkillOverridePriority.Upgrade);
                            if (DoomPowerDef.doomPowerDefs[i] == DoomPowerDef.doomSpearDef && homingTracker) homingTracker.maxTargets = 1;
                        }
                    }
                }
                else break;
            }
            lastDoomPowerCount = doomPowerCount;
        }
        private void SetPathOnTransform(FormDef previous, FormDef current)
        {
            if (current == HedgehogUtils.Forms.SuperForm.SuperFormDef.superFormDef && ShadowPaths.GetPath(characterBody.inventory) == ShadowPaths.Path.Neutral)
            {
                ShadowPaths.SetPath(characterBody.inventory, ShadowPaths.Path.Hero);
            }
        }
    }
}