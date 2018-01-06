using Abilities;
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
            HostShip.OnActivationPhaseStart += RegisterDalanAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActivationPhaseStart -= RegisterDalanAbility;
        }

        private void RegisterDalanAbility(Ship.GenericShip ship)
        {
            Triggers.RegisterTrigger(new Trigger
            {
                Name = Name,
                TriggerType = TriggerTypes.OnActivationPhaseStart,
                TriggerOwner = HostShip.Owner.PlayerNo,
                EventHandler = AskGuriAbility
            });
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
                    Phases.CurrentPhase.StartPhase();
                });

                decisionSubPhase.AddDecision("No", delegate { Triggers.FinishTrigger(); });

                decisionSubPhase.ShowSkipButton = true;

                decisionSubPhase.Start();
                Triggers.FinishTrigger();

            }
        }

        private void PerformTallonRoll(GenericMovement shipMovement)
        {
            HostShip.SetAssignedManeuver(new TallonRollMovement(shipMovement.Speed, shipMovement.Direction, shipMovement.Bearing, Movement.ManeuverColor.Red));
        }
    }
}

