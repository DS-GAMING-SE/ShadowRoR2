using EntityStates;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using ShadowMod.Characters.Survivors.Shadow.SkillStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public class ChaosSnapSkillDef : SkillDef
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = new InstanceData { tracker = skillSlot.GetComponent<ITargetTracker>(), skillSlot = skillSlot };
            GlobalEventManager.onDeathProcServer += instanceData.OnKilledOther;
            return instanceData;
        }
        public override void OnUnassigned([NotNull] GenericSkill skillSlot)
        {
            GlobalEventManager.onDeathProcServer -= ((InstanceData)skillSlot.skillInstanceData).OnKilledOther;
        }
        public override void OnExecute([NotNull] GenericSkill skillSlot)
        {
            EntityState state = this.InstantiateNextState(skillSlot);
            skillSlot.stateMachine.SetInterruptState(state, this.interruptPriority);
            if (state is not ChaosSnapIn chaosSnap || chaosSnap.rechargeFromChaosMark)
            {
                skillSlot.stock -= this.stockToConsume;
            }
            if (this.cancelSprintingOnActivation)
            {
                skillSlot.characterBody.isSprinting = false;
            }
            if (this.resetCooldownTimerOnUse)
            {
                skillSlot.rechargeStopwatch = 0f;
            }
            if (skillSlot.characterBody)
            {
                skillSlot.characterBody.OnSkillActivated(skillSlot);
            }
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            HurtBox target = GetTarget(skillSlot);
            return target && (skillSlot.stock >= 1 || TargetHasChaosMark(target));
        }
        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState state = base.InstantiateNextState(skillSlot);
            if (state is ChaosSnapIn chaosSnapIn)
            {
                HurtBox target = GetTarget(skillSlot);
                chaosSnapIn.rechargeFromChaosMark = skillSlot.stock >= 1;
                chaosSnapIn.target = target;
                return chaosSnapIn;
            }
            return state;
        }
        private static HurtBox GetTarget([NotNull] GenericSkill skillSlot)
        {
            ITargetTracker tracker = ((InstanceData)skillSlot.skillInstanceData).tracker;
            return (tracker != null) ? tracker.GetTrackingTarget() : null;
        }
        private static bool TargetHasChaosMark(HurtBox target)
        {
            return target && target.healthComponent && target.healthComponent.body.HasBuff(ShadowBuffs.chaosMarkDebuff);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public ITargetTracker tracker;
            public GenericSkill skillSlot;
            public void OnKilledOther(DamageReport damageReport)
            {
                if (damageReport.attackerBody && damageReport.attackerBody == skillSlot.characterBody && damageReport.victimBody && damageReport.victimBody.HasBuff(ShadowBuffs.chaosMarkDebuff) &&
                    damageReport.attackerBody.hasAuthority && skillSlot.CanApplyAmmoPack())
                {
                    skillSlot.ApplyAmmoPack();
                }
            }
        }
    }

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
                    RoR2.Orbs.OrbManager.instance.AddOrb(new KillCooldownOrb { origin = damageReport.damageInfo.position, target = damageReport.attackerBody.mainHurtBox });
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
