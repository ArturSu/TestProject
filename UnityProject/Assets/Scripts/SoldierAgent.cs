using System;
using System.Linq;
using MLAgents;
using TestProject.Controllers;
using TestProject.Model;
using UnityEngine;

namespace TestProject
{
    public class SoldierAgent : Agent
    {
        private const float RightDirectionChoiceReward = 0.05f;
        private const float WrongDirectionChoiceReward = -1f;
        private const float StepPenalty = -0.1f;
        
        private const int MaxSoldiersCount = 8;

        public event Action<Tuple<int, int>> TileSelected;

        private Tuple<int, int, MoveDirection>[] _tiles;
        private float _maxDistance;
        private SoldierData _currentSoldier;
        private SoldierData[] _sameArmySoldiers;
        private SoldierData[] _otherArmySoldiers;
        private float _attackDistance;

        public int Id => _currentSoldier.Id;

        public void Initialize(Brain agentBrain, BattleData battleData, int soldierId)
        {
            brain = agentBrain;
            _currentSoldier = battleData.Soldiers.First(item => item.Id == soldierId);
            _sameArmySoldiers = battleData.Soldiers
                .Where(item => item.Id != Id && item.ArmyType == _currentSoldier.ArmyType).ToArray();
            _otherArmySoldiers = battleData.Soldiers.Where(item => item.ArmyType != _currentSoldier.ArmyType).ToArray();
            _maxDistance = Math.Max(battleData.GridHeight, battleData.GridWidth);
            _attackDistance = battleData.AttackDistance / _maxDistance;
            gameObject.name = $"Agent_{Id}";

            //Do it for correct brain init.
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void Done(ArmyType winner)
        {
            var reward = _currentSoldier.ArmyType == winner ? 1f : -1f;
            AddReward(reward);
            Done();
        }

        public void RequestAction(Tuple<int, int, MoveDirection>[] tiles)
        {
            _tiles = tiles;
            RequestDecision();
        }

        public override void CollectObservations()
        {
            //General:
            AddVectorObs(_attackDistance);

            //Current soldier:

            AddVectorObs(_currentSoldier.PositionX / _maxDistance);
            AddVectorObs(_currentSoldier.PositionY / _maxDistance);

            //Other soldiers:
            CollectSoldiersData(_sameArmySoldiers);
            CollectFakeSoldiersData(MaxSoldiersCount - _sameArmySoldiers.Length - 1, true);
            CollectSoldiersData(_otherArmySoldiers);
            CollectFakeSoldiersData(MaxSoldiersCount - _otherArmySoldiers.Length, false);
        }

        public override void AgentAction(float[] vectorAction, string textAction)
        {
            AddReward(StepPenalty);
            int action = Mathf.FloorToInt(vectorAction[0]) - 1;
            if (action >= 0)
            {
                var chosenDirection = (MoveDirection) action;
                var tile = _tiles.FirstOrDefault(item => item.Item3 == chosenDirection);
                if (tile != null)
                {
                    AddReward(RightDirectionChoiceReward);
                    OnTileSelected(new Tuple<int, int>(tile.Item1, tile.Item2));
                }
                else
                {
                    AddReward(WrongDirectionChoiceReward);
                    RequestDecision();
                }
            }
            else
            {
                if (_tiles.Length == 0)
                {
                    AddReward(RightDirectionChoiceReward);
                }
                else
                {
                    RequestDecision();
                }
            }
        }

        private void CollectSoldiersData(SoldierData[] soldiers)
        {
            foreach (var soldier in soldiers)
            {
                AddVectorObs((soldier.PositionX - _currentSoldier.PositionX) / _maxDistance);
                AddVectorObs((soldier.PositionY - _currentSoldier.PositionY) / _maxDistance);
                AddVectorObs(soldier.ArmyType == _currentSoldier.ArmyType);
                AddVectorObs(soldier.IsAlive);
            }
        }

        private void CollectFakeSoldiersData(int soldiersCount, bool isSameArmy)
        {
            for (var i = 0; i < soldiersCount; i++)
            {
                AddVectorObs(0);
                AddVectorObs(0);
                AddVectorObs(isSameArmy);
                AddVectorObs(false);
            }
        }

        private void OnTileSelected(Tuple<int, int> tile)
        {
            TileSelected?.Invoke(tile);
        }
    }
}