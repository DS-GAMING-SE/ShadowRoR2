using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowMod.Characters.Survivors.Shadow.Components
{
    public class ChaosBoostComponent : MonoBehaviour
    {
        public bool charged 
        { 
            get 
            { 
                return characterBody && characterBody.inventory && 
                    characterBody.inventory.GetItemCountEffective(KillCooldownSkillDef.killCooldownChargeItemDef) > ShadowStaticValues.chaosBoostKillsToCharge; 
            } 
        }

        private bool subscribed;
        public CharacterBody characterBody;
        public readonly List<GenericSkill> killCooldownSkills = new List<GenericSkill>();
        private SkillDef skillOverride;
        private bool skillOverrideActive;
        private int chaosBoostOverrideCount;

        private void Awake()
        {
            characterBody = GetComponent<CharacterBody>();
        }
        public void ResetChaosBoostOverrideCount()
        {
            chaosBoostOverrideCount = 0;
        }
        public void AddChaosBoostOverrideCount(GenericSkill skillSlot, SkillDef skillOverride)
        {
            if (++chaosBoostOverrideCount >= 3)
            {
                chaosBoostOverrideCount = 0;
                ActivateSkillOverride(skillSlot, skillOverride);
            }
        }
        public void ActivateSkillOverride(GenericSkill skillSlot, SkillDef skillOverride)
        {
            skillSlot.SetSkillOverride(this, skillOverride, GenericSkill.SkillOverridePriority.Contextual);
            this.skillOverride = skillOverride;
            skillOverrideActive = true;
        }
        public void DeactivateSkillOverride(GenericSkill skillSlot)
        {
            if (!skillOverrideActive) return;

            skillSlot.UnsetSkillOverride(this, skillOverride, GenericSkill.SkillOverridePriority.Contextual);
            skillOverrideActive = false;
        }
        public void AddKillCooldownSkill(GenericSkill skill)
        {
            killCooldownSkills.Add(skill);
            if (killCooldownSkills.Count > 0 && !subscribed)
            {
                GlobalEventManager.onDeathProcServer += OnKilledOther;
                subscribed = true;
            }
        }
        public void RemoveKillCooldownSkill(GenericSkill skill)
        {
            killCooldownSkills.Remove(skill);
            if (killCooldownSkills.Count == 0 && subscribed)
            {
                GlobalEventManager.onDeathProcServer -= OnKilledOther;
                subscribed = false;
            }
        }
        private void OnDestroy()
        {
            if (subscribed)
            {
                GlobalEventManager.onDeathProcServer -= OnKilledOther;
            }
        }
        public void OnKilledOther(DamageReport damageReport)
        {
            if (!charged && damageReport.attackerBody && damageReport.attackerBody == characterBody)
            {
                RoR2.Orbs.OrbManager.instance.AddOrb(new KillCooldownOrb { origin = damageReport.damageInfo.position, target = damageReport.attackerBody.mainHurtBox });
            }
        }
    }
}