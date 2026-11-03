using HedgehogUtils.Forms;
using R2API.Networking.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using ShadowMod.Characters.Survivors.Shadow.Components;

namespace ShadowMod.Characters.Survivors.Shadow
{
    /*public class NetworkKillCooldown : INetMessage
    {
        private NetworkInstanceId netId;
        public NetworkKillCooldown()
        {

        }
        public NetworkKillCooldown(NetworkInstanceId netId)
        {
            this.netId = netId;
        }
        public void OnReceived()
        {
            CharacterBody body = RoR2.Util.FindNetworkCharacterBody(netId);
            if (body && body.hasAuthority && body.TryGetComponent<ChaosBoostComponent>(out var chaosBoost))
            {
                bool chargedAll = true;
                foreach (var skill in chaosBoost.killCooldownSkills)
                {
                    //skill.RunRecharge(1);
                    if (!skill.IsReady())
                    {
                        chargedAll = false;
                    }
                }
                if (chargedAll)
                {
                    BaseChaosBoostState.SetChaosBoostCharged(body.gameObject, true);
                }
            }
        }
        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
        }

        public void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
        }
    }*/
}
