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
        private const float RightDirectionChoiceReward = 0.2f;
        private const float WrongDirectionChoiceReward = -1f;
        
        public event Action<Tuple<int, int>> TileSelected;

        private Tuple<int, int, MoveDirection>[] _tiles;
        private float _maxDistance;
        private SoldierData _soldier;
        private SoldierData[] _sameArmySoldiers;
        private SoldierData[] _otherArmySoldiers;
        private float _attackDistance;

        public int Id => _soldier.Id;

        public void Initialize(Brain agentBrain, BattleData battleData, int soldierId)
        {
            brain = agentBrain;
            _soldier = battleData.Soldiers.First(item => item.Id == soldierId);
            _sameArmySoldiers = battleData.Soldiers.Where(item => item.Id != Id && item.ArmyType == _soldier.ArmyType).ToArray();
            _otherArmySoldiers = battleData.Soldiers.Where(item => item.ArmyType != _soldier.ArmyType).ToArray();
            _maxDistance = Math.Max(battleData.GridHeight, battleData.GridWidth);
            _attackDistance = battleData.AttackDistance / _maxDistance;
            gameObject.name = $"Agent_{Id}";
           
            //Do it for correct brain init.
            gameObject.SetActive(false);
            gameObject.SetActive(true);
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
         
            AddVectorObs(_soldier.PositionX / _maxDistance);
            AddVectorObs(_soldier.PositionY / _maxDistance);
            AddVectorObs((float) _soldier.ArmyType);

            //Other soldiers:
            CollectSoldiersData(_sameArmySoldiers);
            CollectSoldiersData(_otherArmySoldiers);
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

        private void CollectSoldiersData(SoldierData[] otherSoldiers)
        {
            foreach (var soldier in otherSoldiers)
            {
                AddVectorObs((soldier.PositionX - _soldier.PositionX) / _maxDistance);
                AddVectorObs((soldier.PositionY - _soldier.PositionY) / _maxDistance);
                AddVectorObs((float) soldier.ArmyType);
                AddVectorObs(soldier.IsAlive);
            }
        }
        
        private void OnTileSelected(Tuple<int, int> tile)
        {
            TileSelected?.Invoke(tile);
        }
    }
}