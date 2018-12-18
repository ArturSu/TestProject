using System;
using System.Collections.Generic;
using System.Linq;
using MLAgents;
using TestProject.Controllers;
using TestProject.Model;
using UnityEngine;

namespace TestProject.Views
{
    public class AiInput : MonoBehaviour, IAiInput, IMoveInput, IAttackInput
    {
        private readonly List<SoldierAgent> _agents = new List<SoldierAgent>();

        public event Action<Tuple<int, int>> TileSelected;
        public event Action<int> TargetSelected;

        [SerializeField] private SoldierAgent _agentPrefab;
        [SerializeField] private Brain _brain;

        private SoldierAgent _currentAgent;
        private BattleData _battleData;

        public InputType InputType => InputType.AI;
        public bool IsTrainRun => _brain.brainType == BrainType.External;

        public void Initialize(BattleData battleData)
        {
            _battleData = battleData;
            InitializeAgents();
        }
        
        public void Done(ArmyType winner)
        {
            foreach (var agent in _agents)
            {
                agent.Done(winner);
            }
        }

        public void Activate(Tuple<int, int, MoveDirection>[] tiles, int soldierId)
        {
            _currentAgent = GetAgent(soldierId);
            _currentAgent.RequestAction(tiles);
        }

        public void Activate(int[] targetIds)
        {
            if (targetIds.Any())
            {
                _currentAgent.AddReward(0.8f);
                var chosenId = targetIds[0];
                var agent = _agents.FirstOrDefault(item => item.Id == chosenId);
                if (agent != null)
                {
                    agent.AddReward(-0.6f);
                }
                
                OnTargetSelected(chosenId);
            }
        }

        public void Deactivate()
        {
        }
        
        private void InitializeAgents()
        {
            foreach (var agent in _agents)
            {
                if (_battleData.Soldiers.Any(item => item.Id == agent.Id))
                {
                    agent.Initialize(_brain, _battleData, agent.Id);
                }
            }
        }

        private SoldierAgent GetAgent(int soldierId)
        {
            var agent = _agents.FirstOrDefault(item => item.Id == soldierId);
            if (agent == null)
            {
                agent = Instantiate(_agentPrefab, transform);
                agent.Initialize(_brain, _battleData, soldierId);
                agent.TileSelected += Agent_TileSelected;
                _agents.Add(agent);
            }

            return agent;
        }

        private void Agent_TileSelected(Tuple<int, int> tile)
        {
            OnTileSelected(tile);
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