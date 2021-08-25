using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VergilMercenary.EntityStates
{
    public class MainState : GenericCharacterMain
    {
        private bool isVergil;

        public override void OnEnter()
        {
            base.OnEnter();
            this.isVergil = true;
            if (this.characterBody.skinIndex > 1U)
                return;
            this.isVergil = false;
        }

        public override void Update()
        {
            base.Update();
            if (!this.isVergil || !this.isAuthority || !this.characterMotor.isGrounded)
                return;
            if (Input.GetKeyDown(VergilMerc.tauntKey.Value))
            {
                this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(Taunt))), InterruptPriority.Any);
            }
            else
            {
                if (!Input.GetKeyDown(VergilMerc.altTauntKey.Value))
                    return;
                this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(AltTaunt))), InterruptPriority.Any);
            }
        }

        public override void OnExit() => base.OnExit();

        public override void ProcessJump() => base.ProcessJump();
    }
}
