
using Abilities;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    namespace StarViper
    {
        public class PrinceXizor : StarViper
        {
            public PrinceXizor() : base()
            {
                PilotName = "Prince Xizor";
                PilotSkill = 7;
                Cost = 31;

                IsUnique = true;

                PrintedUpgradeIcons.Add(Upgrade.UpgradeType.Elite);
                PilotAbilities.Add(new PrinceXizorAbility());
            }
        }
    }
}

namespace Abilities
{
    public class PrinceXizorAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnAttackHitAsDefender += RegisterPrinceXizorAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackHitAsDefender -= RegisterPrinceXizorAbility;
        }

        private void RegisterPrinceXizorAbility()
        {
            RegisterAbilityTrigger(TriggerTypes.OnAttackHit, AskShedDamage);
        }

        private void AskShedDamage(object sender, EventArgs e)
        {
            List<GenericShip> nearbyFriendlyShips = Board.BoardManager.GetShipsAtRange(HostShip, new Vector2(1, 1), Team.Type.Friendly);
            if (nearbyFriendlyShips.Count > 1)
            {
                SelectTargetForAbility(
                    ShedDamage,
                    new List<TargetTypes> { TargetTypes.OtherFriendly },
                    new Vector2(1, 1));
            }
        }

        private void ShedDamage()
        {
            int successes = Combat.DiceRollAttack.Successes;
            int criticalSuccesses = Combat.DiceRollAttack.CriticalSuccesses;

            DecisionSubPhase decisionSubPhase = (DecisionSubPhase)Phases.StartTemporarySubPhaseNew(
                Name,
                typeof(DecisionSubPhase),
                DecisionSubPhase.ConfirmDecision);

            decisionSubPhase.InfoText = string.Format("Which type of damage would you like to apply to {0}?", TargetShip);

            if (successes > 0)
            {
                decisionSubPhase.AddDecision("Apply 1 Success?", delegate { MitigateDamage(DiceKind.Attack); });

            }
            if (criticalSuccesses > 0)
            {
                decisionSubPhase.AddDecision("Apply 1 Critical Success?", delegate { MitigateDamage(DiceKind.Attack); });
            }

            decisionSubPhase.ShowSkipButton = true;

            decisionSubPhase.Start();
        }

        private void MitigateDamage(DiceKind die)
        {

        }
    }
}

