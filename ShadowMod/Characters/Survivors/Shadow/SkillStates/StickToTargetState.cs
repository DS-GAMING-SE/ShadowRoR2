using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public abstract class StickToTargetState : BaseSkillState
    {
        public HurtBox target;
        public Vector3 offset;

        public override void OnEnter()
        {
            base.OnEnter();
            var grav = characterMotor.gravityParameters;
            grav.channeledAntiGravityGranterCount++;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                if (characterMotor) characterMotor.velocity = Vector3.zero;
                if (characterDirection) characterDirection.forward = -offset;
            }
        }
        public override void Update()
        {
            base.Update();
            if (isAuthority && target && characterMotor)
            {
                characterMotor.Motor.MoveCharacter(target.transform.position + offset);
            }
        }
        public static Vector3 GetStickOffset(HurtBox target, Transform user)
        {
            Vector3 offset = target.collider.ClosestPoint(user.transform.position);
            offset += ((user.position - target.transform.position).normalized * 1.5f);
            return offset - target.transform.position;
        }
        public override void OnExit()
        {
            var grav = characterMotor.gravityParameters;
            grav.channeledAntiGravityGranterCount--;
            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(HurtBoxReference.FromHurtBox(target));
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            target = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}
