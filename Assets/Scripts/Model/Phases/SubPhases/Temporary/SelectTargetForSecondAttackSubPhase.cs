﻿using Ship;
using System;
using UnityEngine;

namespace SubPhases
{
    public class SelectTargetForSecondAttackSubPhase : SelectShipSubPhase
    {
        public override void Prepare()
        {
            targetsAllowed.Add(TargetTypes.Enemy);
            finishAction = FinishActon;

            UI.ShowSkipButton();

            Selection.ThisShip.IsAttackPerformed = false;
            Combat.IsAttackAlreadyCalled = false;

            Selection.ThisShip.Owner.StartExtraAttack();
        }

        private void FinishActon()
        {
            Selection.AnotherShip = TargetShip;
            Combat.ChosenWeapon = Selection.ThisShip.PrimaryWeapon;
            Combat.ShotInfo = new Board.ShipShotDistanceInformation(Selection.ThisShip, TargetShip, Combat.ChosenWeapon);
            MovementTemplates.ShowFiringArcRange(Combat.ShotInfo);
            ExtraAttackTargetSelected();
        }

        private void ExtraAttackTargetSelected()
        {
            Phases.StartTemporarySubPhaseNew(
                "Extra Attack",
                typeof(ExtraAttackSubPhase),
                delegate {
                    Phases.FinishSubPhase(typeof(ExtraAttackSubPhase));
                    Phases.FinishSubPhase(typeof(SelectTargetForSecondAttackSubPhase));
                    CallBack();
                }
            );
            Combat.DeclareIntentToAttack(Selection.ThisShip.ShipId, Selection.AnotherShip.ShipId);
        }

        public override void RevertSubPhase() { }

        public override void SkipButton()
        {
            UI.HideSkipButton();
            Phases.FinishSubPhase(typeof(SelectTargetForSecondAttackSubPhase));
            CallBack();
        }

        public override void Next()
        {
            Combat.ExtraAttackFilter = null;
            Phases.CurrentSubPhase = PreviousSubPhase;
        }

        public override void Resume()
        {
            UpdateHelpInfo();
            UI.ShowSkipButton();
        }

    }
}