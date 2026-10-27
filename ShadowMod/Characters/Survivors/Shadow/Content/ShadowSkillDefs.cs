using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public class KillCooldownSkillDef : SkillDef
    {
        public static void Init()
        {
            On.RoR2.GenericSkill.CanApplyAmmoPack += NoBandolierOnKillCooldownSkillDef;
        }
        private static bool NoBandolierOnKillCooldownSkillDef(On.RoR2.GenericSkill.orig_CanApplyAmmoPack orig, GenericSkill self)
        {
            if (self.skillDef && self.skillDef is KillCooldownSkillDef)
            {
                return false;
            }
            else
            {
                return orig(self);
            }
        }
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = new InstanceData { skillSlot = skillSlot };
            GlobalEventManager.onDeathProcServer += instanceData.OnKilledOther;
            return instanceData;
        }
        public override void OnUnassigned([NotNull] GenericSkill skillSlot)
        {
            GlobalEventManager.onDeathProcServer -= ((InstanceData)skillSlot.skillInstanceData).OnKilledOther;
        }

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime)
        {
            // getting rid of normal cooldown
            // apparently, setting the recharge rate to 0 hides the cooldown in the UI, which is bad
        }
        private class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public GenericSkill skillSlot;
            public void OnKilledOther(DamageReport damageReport)
            {
                if (damageReport.attackerBody && damageReport.attackerBody == skillSlot.characterBody)
                {
                    RoR2.Orbs.OrbManager.instance.AddOrb(new KillCooldownOrb { origin = damageReport.victimBody.mainHurtBox.transform.position, target = damageReport.attackerBody.mainHurtBox });
                }
            }
        }
    }
    public class ChaosBoostSkillDef : KillCooldownSkillDef
    {
        public Sprite upgradeSkillIcon;
        public float upgradeRechargeInterval;
        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot)
        {
            if (upgradeSkillIcon && skillSlot.characterBody && skillSlot.characterBody.HasBuff(ShadowBuffs.chaosBoostBuff))
            {
                return upgradeSkillIcon;
            }
            else
            {
                return base.GetCurrentIcon(skillSlot);
            }
        }
        public override float GetRechargeInterval([NotNull] GenericSkill skillSlot)
        {
            if (skillSlot.characterBody && skillSlot.characterBody.HasBuff(ShadowBuffs.chaosBoostBuff))
            {
                return upgradeRechargeInterval;
            }
            else
            {
                return base.GetRechargeInterval(skillSlot);
            }
        }
    }
}
