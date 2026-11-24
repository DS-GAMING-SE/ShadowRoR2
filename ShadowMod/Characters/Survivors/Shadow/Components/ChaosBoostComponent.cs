using RoR2;
using RoR2.Skills;
using System;
using System.Collections;
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
        private Coroutine playChargedEffectAfterAFrame;
        public Action<bool> onKillCooldownChargedChanged;

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
        public void KillCooldownChargedChanged(GenericSkill skillSlot, bool newCharged)
        {
            if (playChargedEffectAfterAFrame != null) return;

            playChargedEffectAfterAFrame = StartCoroutine(VerifyKillCooldownChargedChanged(skillSlot, newCharged));
        }
        private IEnumerator VerifyKillCooldownChargedChanged(GenericSkill skillSlot, bool newCharged)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (skillSlot && (skillSlot.stock > 0 == newCharged))
            {
                onKillCooldownChargedChanged?.Invoke(newCharged);
                EffectManager.SpawnEffect(ShadowAssets.killCooldownChargedEffect, new EffectData { origin = transform.position, rootObject = gameObject }, false);
            }
            playChargedEffectAfterAFrame = null;
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