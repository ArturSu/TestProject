using System;
using System.Linq;
using MLAgents;
using TestProject.Controllers;
using TestProject.Model;
using UnityEngine;
using Zenject;

namespace TestProject
{
    public class SoldierAgent : Agent, IMoveInput, IAttackInput
    {
        private const float RightDirectionChoiceReward = 0.2f;
        private const float WrongDirectionChoiceReward = -1f;
        
        public event Action<Tuple<int, int>> TileSelected;
        public event Action<int> TargetSelected;

        [Inject] private BattleData _battleData;

        private int _currentSoldierId;
        private Tuple<int, int, MoveDirection>[] _tiles;

        public InputType InputType => InputType.AI;
       
        public override void CollectObservations()
        {
            float maxDistance = Math.Max(_battleData.GridHeight, _battleData.GridWidth);

            //General:
            AddVectorObs(_battleData.AttackDistance / maxDistance);

            //Current soldier:
            var soldier = _battleData.Soldiers.First(item => item.Id == _currentSoldierId);
            AddVectorObs(soldier.PositionX / maxDistance);
            AddVectorObs(soldier.PositionY / maxDistance);
            AddVectorObs((float) soldier.ArmyType);

            //Other soldiers:
            var opponent = _battleData.Soldiers.First(item => item.Id != _currentSoldierId);
            AddVectorObs((opponent.PositionX - soldier.PositionX) / maxDistance);
            AddVectorObs((opponent.PositionY - soldier.PositionY) / maxDistance);
            AddVectorObs((float) soldier.ArmyType);
            AddVectorObs(opponent.IsAlive ? 1f : 0f);
        }

        public override void AgentAction(float[] vectorAction, string textAction)
        {
            int action = Mathf.FloorToInt(vectorAction[0]) - 1;
            if (action >= 0)
            {
                var chosenDirection = (MoveDirection)action;
                var tile = _tiles.FirstOrDefault(item => item.Item3 == chosenDirection);
                if (tile != null)
                {
                    AddReward(RightDirectionChoiceReward);
                    OnTileSelected(new Tuple<int, int>(tile.Item1, tile.Item2));
                }
                else
                {
                    AddReward(WrongDirectionChoiceReward);
                    RequestAction();
                }
            }
            else
            {
                if (_tiles.Length == 0)
                {
                    AddReward(RightDirectionChoiceReward);
                }
            }
        }

        public void Activate(Tuple<int, int, MoveDirection>[] tiles, int soldierId)
        {
            _currentSoldierId = soldierId;
            _tiles = tiles;
            RequestAction();
        }

        public void Activate(int[] targetIds)
        {
            if (targetIds.Any())
            {
                AddReward(1f);
                OnTargetSelected(targetIds[0]);
            }
        }

        public void Deactivate()
        {
        }

        private void OnTargetSelected(int id)
        {
            TargetSelected?.Invoke(id);
        }

        private void OnTileSelected(Tuple<int, int> tile)
        {
            TileSelected?.Invoke(tile);
        }
    }
}