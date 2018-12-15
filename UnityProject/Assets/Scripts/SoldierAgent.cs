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

        private BattleData _battleData;
        private Tuple<int, int, MoveDirection>[] _tiles;

        public int Id { get; private set; }

        public void Initialize(Brain agentBrain, BattleData battleData, int soldierId)
        {
            _battleData = battleData;
            brain = agentBrain;
            Id = soldierId;
            gameObject.name = $"Agent_{Id}";
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
            float maxDistance = Math.Max(_battleData.GridHeight, _battleData.GridWidth);

            //General:
            AddVectorObs(_battleData.AttackDistance / maxDistance);

            //Current soldier:
            var currentSoldier = _battleData.Soldiers.First(item => item.Id == Id);
            AddVectorObs(currentSoldier.PositionX / maxDistance);
            AddVectorObs(currentSoldier.PositionY / maxDistance);
            AddVectorObs((float) currentSoldier.ArmyType);

            //Other soldiers:
            var otherSoldiers = _battleData.Soldiers.Where(item => item.Id != Id).ToArray();
            foreach (var soldier in otherSoldiers)
            {
                AddVectorObs((soldier.PositionX - currentSoldier.PositionX) / maxDistance);
                AddVectorObs((soldier.PositionY - currentSoldier.PositionY) / maxDistance);
                AddVectorObs((float) soldier.ArmyType);
                AddVectorObs(soldier.IsAlive);
            }
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

        private void OnTileSelected(Tuple<int, int> tile)
        {
            TileSelected?.Invoke(tile);
        }
    }
}