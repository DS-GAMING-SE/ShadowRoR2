using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ChaosSnapPummel : StickToTargetState, ISkillState
    {
        protected float attackTimer;
        protected DamageInfo attack = new DamageInfo();
        protected virtual Type nextStateType { get { return typeof(ChaosSnapFinal); } }
        private ICharacterGravityParameterProvider enemyGrav;
        public override void OnEnter()
        {
            base.OnEnter();
            attackTimer = ShadowStaticValues.chaosSnapPummelInterval / characterBody.attackSpeed;
            if (activatorSkillSlot) activatorSkillSlot.SetBlockedCooldownSkillState(true);
            if (target && NetworkServer.active) target.healthComponent.body.AddBuff(HedgehogUtils.Buffs.suppressDeathBuff);
            if (target && target.healthComponent.TryGetComponent<ICharacterGravityParameterProvider>(out enemyGrav))
            {
                var param = enemyGrav.gravityParameters;
                param.channeledAntiGravityGranterCount++;
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            attackTimer -= Time.fixedDeltaTime;
            while (attackTimer <= 0 && target && target.healthComponent && !target.healthComponent.body.HasBuff(RoR2Content.Buffs.Intangible))
            {
                attackTimer += ShadowStaticValues.chaosSnapPummelInterval / characterBody.attackSpeed;
                if (NetworkServer.active)
                {
                    CreateAttack();
                    ModifyAttack(attack);
                    FireAttack();
                }
            }
            if (isAuthority && (!target || !target.healthComponent || target.healthComponent.body.HasBuff(RoR2Content.Buffs.Intangible) || !inputBank.skill1.down || fixedAge > ShadowStaticValues.chaosSnapPummelMaxDuration))
            {
                SetNextState(); // final hit
            }
        }
        protected virtual void SetNextState()
        {
            EntityState state = EntityStateCatalog.InstantiateState(nextStateType);
            if (state is ChaosSnapFinal final)
            {
                final.target = target;
                final.activatorSkillSlot = activatorSkillSlot;
                outer.SetNextState(final);
                return;
            }
            outer.SetNextState(state);
        }
        protected void CreateAttack()
        {
            attack.attacker = gameObject;
            attack.inflictor = gameObject;
            attack.crit = RollCrit();
            attack.procCoefficient = ShadowStaticValues.chaosSnapPummelProcCoefficient;
            attack.damage = ShadowStaticValues.chaosSnapPummelDamageCoefficient * characterBody.damage;
            attack.damageType = new DamageTypeCombo(DamageType.Stun1s, DamageTypeExtended.Generic, DamageSource.Primary);
            attack.inflictedHurtbox = target;
            attack.position = target.collider.ClosestPoint(transform.position);
        }
        protected virtual void ModifyAttack(DamageInfo damage)
        {

        }
        protected void FireAttack()
        {
            if (target.healthComponent)
            {
                target.healthComponent.TakeDamage(attack);
                GlobalEventManager.instance.OnHitEnemy(attack, target.healthComponent.gameObject);
                GlobalEventManager.instance.OnHitAll(attack, target.healthComponent.gameObject);
            }
        }
        public override void OnExit()
        {
            if (target && NetworkServer.active) target.healthComponent.body.RemoveBuff(HedgehogUtils.Buffs.suppressDeathBuff);
            if (activatorSkillSlot) activatorSkillSlot.SetBlockedCooldownSkillState(false);
            if (enemyGrav != null)
            {
                var param = enemyGrav.gravityParameters;
                param.channeledAntiGravityGranterCount--;
            }
            base.OnExit();
        }
    }
}
