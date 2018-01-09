using Abilities;
using GameModes;
using Movement;
using SubPhases;
using System;

namespace Ship
{
    namespace StarViper
    {
        public class DalanOberos : StarViper
        {
            public DalanOberos() : base()
            {
                PilotName = "Dalan Oberos";
                PilotSkill = 6;
                Cost = 30;

                IsUnique = true;

                PrintedUpgradeIcons.Add(Upgrade.UpgradeType.Elite);
                PilotAbilities.Add(new DalanOberosAbility());
            }
        }
    }
}

namespace Abilities
{
    public class DalanOberosAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnManeuverIsRevealed += RegisterDalanAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnManeuverIsRevealed -= RegisterDalanAbility;
        }

        private void RegisterDalanAbility(Ship.GenericShip ship)
        {
            RegisterAbilityTrigger(TriggerTypes.OnManeuverIsRevealed, AskGuriAbility);

        }

        private void AskGuriAbility(object sender, EventArgs e)
        {
            if (HostShip.HasToken(typeof(Tokens.StressToken)))
            {
                Triggers.FinishTrigger();
                return;
            }

            GenericMovement shipMovement = HostShip.AssignedManeuver;
            if (shipMovement is TurnMovement ||
                shipMovement is BankMovement ||
                shipMovement is SegnorsLoopMovement)
            {
                DecisionSubPhase decisionSubPhase = (DecisionSubPhase)Phases.StartTemporarySubPhaseNew(
                    Name,
                    typeof(DecisionSubPhase),
                    null);

                decisionSubPhase.InfoText = "Would you like to perform a Red Tallon Roll of the same maneuver?";

                decisionSubPhase.AddDecision("Yes", delegate
                {
                    PerformTallonRoll(shipMovement);
                    DecisionSubPhase.ConfirmDecisionNoCallback();
                    Triggers.FinishTrigger();
                });

                decisionSubPhase.AddDecision("No", delegate 
                {
                    DecisionSubPhase.ConfirmDecisionNoCallback();
                    Triggers.FinishTrigger();
                });

                decisionSubPhase.ShowSkipButton = true;

                decisionSubPhase.Start();               
            }
        }

        private void PerformTallonRoll(GenericMovement shipMovement)
        {
            GenericMovement movement = HostShip.AssignedManeuver;
            if (movement is BankMovement || movement is SegnorsLoopMovement)
            {
                HostShip.SetAssignedManeuver(new TallonRollBankMovement(shipMovement.Speed, shipMovement.Direction, shipMovement.Bearing, Movement.ManeuverColor.Red));
            }
            else
            {
                HostShip.SetAssignedManeuver(new TallonRollMovement(shipMovement.Speed, shipMovement.Direction, shipMovement.Bearing, Movement.ManeuverColor.Red));
            }            
        }
    }
}

