using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using TestProject.Controllers;
using TestProject.Model;

namespace Tests
{
    public class AttackControllerTests
    {
        [Test]
        public void HorizontalReach_TargetFound()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 2};
            var opponentSoldier = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 2, IsAlive = true, PositionX = 5, PositionY = 2};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier, opponentSoldier}};

            var mockAttackInput = new AttackInputMock(new[] {2});
            var controller = new AttackController(new List<IAttackInput> {mockAttackInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }

        [Test]
        public void HorizontalReach_TargetNonReachable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 2};
            var opponentSoldier = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 2, IsAlive = true, PositionX = 10, PositionY = 2};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier, opponentSoldier}};

            var mockAttackInput = new AttackInputMock(new int[] { });
            var controller = new AttackController(new List<IAttackInput> {mockAttackInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }

        [Test]
        public void HorizontalReach_TargetPlayersTeam()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 2};
            var playerSoldier2 = new SoldierData
                {ArmyType = ArmyType.Player, Id = 2, IsAlive = true, PositionX = 5, PositionY = 2};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier, playerSoldier2}};

            var mockAttackInput = new AttackInputMock(new int[] { });
            var controller = new AttackController(new List<IAttackInput> {mockAttackInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }

        [Test]
        public void HorizontalReach_TargetNotAlive()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 2};
            var opponentSoldier = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 2, IsAlive = false, PositionX = 5, PositionY = 2};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier, opponentSoldier}};

            var mockAttackInput = new AttackInputMock(new int[] { });
            var controller = new AttackController(new List<IAttackInput> {mockAttackInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }

        [Test]
        public void VerticalReach_TargetFound()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 2};
            var opponentSoldier = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 2, IsAlive = true, PositionX = 1, PositionY = 5};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier, opponentSoldier}};

            var mockAttackInput = new AttackInputMock(new[] {2});
            var controller = new AttackController(new List<IAttackInput> {mockAttackInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }

        [Test]
        public void VerticalReach_BothTargetsFound()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 3};
            var opponentSoldier1 = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 2, IsAlive = true, PositionX = 1, PositionY = 5};
            var opponentSoldier2 = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 3, IsAlive = true, PositionX = 1, PositionY = 1};
            var battleData = new BattleData
            {
                AttackDistance = 5, GridHeight = 5, GridWidth = 10,
                Soldiers = new[] {playerSoldier, opponentSoldier1, opponentSoldier2}
            };

            var mockAttackInput = new AttackInputMock(new[] {2, 3});
            var controller = new AttackController(new List<IAttackInput> {mockAttackInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }

        [Test]
        public void DiagonalReach_TargetNotFound()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 2};
            var opponentSoldier = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 2, IsAlive = true, PositionX = 2, PositionY = 3};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier, opponentSoldier}};

            var mockAttackInput = new AttackInputMock(new int[] { });
            var controller = new AttackController(new List<IAttackInput> {mockAttackInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }

        private class AttackInputMock : IAttackInput
        {
            private readonly int[] _expectedIds;
            public event Action<int> TargetSelected;

            public InputType InputType => InputType.Player;

            public AttackInputMock(int[] expectedIds)
            {
                _expectedIds = expectedIds;
            }

            public void Activate(int[] targetIds)
            {
                Assert.IsTrue(targetIds.Length == _expectedIds.Length);
                foreach (var id in targetIds)
                {
                    Assert.IsTrue(_expectedIds.Contains(id));
                }
            }

            public void Deactivate()
            {
            }
        }
    }
}