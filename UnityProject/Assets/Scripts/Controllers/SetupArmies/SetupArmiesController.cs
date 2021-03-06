using System;
using System.Collections.Generic;
using Controllers.Core;
using TestProject.Model;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace TestProject.Controllers.SetupArmies
{
    public class SetupArmiesController : ControllerWithResultBase
    {
        private readonly BattleData _battleData;
        private readonly IArmySetupView _armySetupView;
        private bool _isPvE;

        public SetupArmiesController(BattleData battleData, IArmySetupView armySetupView,
            ControllerFactory controllerFactory) : base(controllerFactory)
        {
            _battleData = battleData;
            _armySetupView = armySetupView;
        }

        protected override void SetArg(object arg)
        {
            Assert.IsTrue(arg is bool);
            _isPvE = (bool) arg;
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (_isPvE)
            {
                _armySetupView.Show(2, 4, 4, 8);
            }
            else
            {
                FillBattleData(Random.Range(1, 9), Random.Range(1, 9));
            }
        }

        protected override void OnStop()
        {
            _armySetupView.Hide();
        }

        protected override void AddEventHandlers()
        {
            _armySetupView.SetupCompleted += ArmySetupView_SetupCompleted;
        }

        protected override void RemoveEventHandlers()
        {
            _armySetupView.SetupCompleted -= ArmySetupView_SetupCompleted;
        }

        private void FillBattleData(int playerSoldiersCount, int opponentSoldiersCount)
        {
            _battleData.GridWidth = 10;
            _battleData.GridHeight = 8;
            _battleData.AttackDistance = 5;
            _battleData.Soldiers = CreateSoldiers(playerSoldiersCount, opponentSoldiersCount);

            Complete(new ControllerResultBase(this));
        }

        private SoldierData[] CreateSoldiers(int playerSoldiersCount, int opponentSoldiersCount)
        {
            var maxArmySize = Math.Max(playerSoldiersCount, opponentSoldiersCount);
            var createdPlayerSoldiers = 0;
            var createdOpponentSoldiers = 0;
            var soldiers = new List<SoldierData>();
            for (int i = 1; i <= maxArmySize; i++)
            {
                var yPosition = GetYPosition(i);
                if (createdPlayerSoldiers < playerSoldiersCount)
                {
                    var playerSoldier = new SoldierData
                        {ArmyType = ArmyType.Player, Id = i, IsAlive = true, PositionX = 1, PositionY = yPosition};

                    soldiers.Add(playerSoldier);
                    createdPlayerSoldiers++;
                }

                if (createdOpponentSoldiers < opponentSoldiersCount)
                {
                    var opponentSoldier = new SoldierData
                    {
                        ArmyType = ArmyType.Opponent, Id = playerSoldiersCount + i, IsAlive = true,
                        PositionX = _battleData.GridWidth,
                        PositionY = yPosition
                    };

                    soldiers.Add(opponentSoldier);
                    createdOpponentSoldiers++;
                }
            }

            return soldiers.ToArray();
        }

        private int GetYPosition(int index)
        {
            var displacement = (int) Math.Pow(-1, index - 1) * Mathf.CeilToInt(index * 0.5f - 0.5f);
            return _battleData.GridHeight / 2 - displacement;
        }

        private void ArmySetupView_SetupCompleted(int playerSoldiersCount, int opponentSoldiersCount)
        {
            FillBattleData(playerSoldiersCount, opponentSoldiersCount);
        }
    }
}