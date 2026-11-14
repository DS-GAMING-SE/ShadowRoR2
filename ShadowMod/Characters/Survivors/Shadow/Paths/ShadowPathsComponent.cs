using EnemiesReturns.Components;
using RoR2;
using RoR2.Stats;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.Paths
{
    public class ShadowPathsComponent : MonoBehaviour
    {
        public CharacterBody characterBody;
        public int lastDoomPowerCount;
        private void Awake()
        {
            characterBody = base.GetComponent<CharacterBody>();
        }
        private void OnEnable()
        {
            characterBody.onInventoryChanged += UpdateDoomPowerCount;
        }
        private void OnDisable()
        {
            characterBody.onInventoryChanged -= UpdateDoomPowerCount;
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

            for (int i = 0; i < DoomPowerDef.doomPowerDefs.Length; i++)
            {
                if (Math.Max(doomPowerCount, lastDoomPowerCount) >= DoomPowerDef.doomPowerDefs[i].requiredDoomPower)
                {
                    if (characterBody.skillLocator.GetSkill(DoomPowerDef.doomPowerDefs[i].skillSlot).baseSkill == DoomPowerDef.doomPowerDefs[i].skillToOverride)
                    {
                        if (doomPowerCount >= DoomPowerDef.doomPowerDefs[i].requiredDoomPower && lastDoomPowerCount < DoomPowerDef.doomPowerDefs[i].requiredDoomPower)
                        {
                            characterBody.skillLocator.GetSkill(DoomPowerDef.doomPowerDefs[i].skillSlot).SetSkillOverride(this, DoomPowerDef.doomPowerDefs[i].skillDef, GenericSkill.SkillOverridePriority.Upgrade);
                        }
                        else if (doomPowerCount < DoomPowerDef.doomPowerDefs[i].requiredDoomPower && lastDoomPowerCount >= DoomPowerDef.doomPowerDefs[i].requiredDoomPower)
                        {
                            characterBody.skillLocator.GetSkill(DoomPowerDef.doomPowerDefs[i].skillSlot).UnsetSkillOverride(this, DoomPowerDef.doomPowerDefs[i].skillDef, GenericSkill.SkillOverridePriority.Upgrade);
                        }
                    }
                }
                else break;
            }
            lastDoomPowerCount = doomPowerCount;
        }
    }
}