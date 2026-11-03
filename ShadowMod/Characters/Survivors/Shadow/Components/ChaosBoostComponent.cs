using RoR2;
using ShadowMod.Characters.Survivors.Shadow.Paths;
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
                    characterBody.inventory.GetItemCountEffective(Items.killCooldownChargeItemDef) > ShadowStaticValues.chaosBoostKillsToCharge; 
            } 
        }

        private bool subscribed;
        public CharacterBody characterBody;
        public readonly List<GenericSkill> killCooldownSkills = new List<GenericSkill>();

        private void Awake()
        {
            characterBody = GetComponent<CharacterBody>();
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