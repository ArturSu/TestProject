using System;
using System.Collections.Generic;
using System.Linq;
using MLAgents;
using TestProject.Controllers;
using TestProject.Model;
using UnityEngine;
using Zenject;

namespace TestProject.Views
{
    public class AiInput : MonoBehaviour, IMoveInput, IAttackInput
    {
        private readonly List<SoldierAgent> _agents = new List<SoldierAgent>();

        public event Action<Tuple<int, int>> TileSelected;
        public event Action<int> TargetSelected;

        [SerializeField] private SoldierAgent _agentPrefab;
        [SerializeField] private Brain _brain;

        private SoldierAgent _currentAgent;
        private BattleData _battleData;

        public InputType InputType => InputType.AI;

        public void Initialize(BattleData battleData)
        {
            _battleData = battleData;
        }
        
        public void Done()
        {
            foreach (var agent in _agents)
            {
                agent.Done();
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
                _currentAgent.AddReward(1f);
                OnTargetSelected(targetIds[0]);
            }
        }

        public void Deactivate()
        {
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