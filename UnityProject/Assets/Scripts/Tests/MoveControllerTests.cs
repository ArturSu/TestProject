using System;
using System.Linq;
using NUnit.Framework;
using TestProject.Controllers;
using TestProject.Model;

namespace Tests
{
    public class MoveControllerTests
    {
        [Test]
        public void LeftDownCornerPosition_TwoDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 1};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier}};

            var mockMoveInput = new MoveInputMock(new[] {MoveDirection.Up, MoveDirection.Right});
            var controller = new MoveController(mockMoveInput, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }
        
        [Test]
        public void LeftUpCornerPosition_TwoDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 5};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier}};

            var mockMoveInput = new MoveInputMock(new[] {MoveDirection.Down, MoveDirection.Right});
            var controller = new MoveController(mockMoveInput, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }

        [Test]
        public void RightUpCornerPosition_TwoDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 10, PositionY = 5};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier}};

            var mockMoveInput = new MoveInputMock(new[] {MoveDirection.Down, MoveDirection.Left});
            var controller = new MoveController(mockMoveInput, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }
        
        [Test]
        public void RightDownCornerPosition_TwoDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 10, PositionY = 1};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier}};

            var mockMoveInput = new MoveInputMock(new[] {MoveDirection.Up, MoveDirection.Left});
            var controller = new MoveController(mockMoveInput, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }

        [Test]
        public void BoardCenterPosition_FourDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 5, PositionY = 2};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier}};

            var mockMoveInput = new MoveInputMock(new[] {MoveDirection.Down, MoveDirection.Left, MoveDirection.Right, MoveDirection.Up});
            var controller = new MoveController(mockMoveInput, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }
        
        [Test]
        public void BoardCenterPositionWithOtherSoldierBeside_ThreeDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 5, PositionY = 2};
            var opponentSoldier = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 2, IsAlive = true, PositionX = 5, PositionY = 3};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier, opponentSoldier}};

            var mockMoveInput = new MoveInputMock(new[] {MoveDirection.Down, MoveDirection.Left, MoveDirection.Right});
            var controller = new MoveController(mockMoveInput, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }
        
        [Test]
        public void BoardCenterPositionWithKilledSoldierBeside_ThreeDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 5, PositionY = 2};
            var opponentSoldier = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 2, IsAlive = false, PositionX = 5, PositionY = 3};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier, opponentSoldier}};

            var mockMoveInput = new MoveInputMock(new[] {MoveDirection.Down, MoveDirection.Left, MoveDirection.Right, MoveDirection.Up});
            var controller = new MoveController(mockMoveInput, new BattleViewStub(), battleData, null);
            controller.Initialize(playerSoldier);
            controller.Start();
        }

        private class MoveInputMock : IMoveInput
        {
            private readonly MoveDirection[] _expectedDirections;
            
            public event Action<MoveDirection> DirectionSelected;

            public MoveInputMock(MoveDirection[] expectedDirections)
            {
                _expectedDirections = expectedDirections;
            }
            
            public void Activate(MoveDirection[] directions)
            {
                Assert.IsTrue(directions.Length == _expectedDirections.Length);
                foreach (var direction in directions)
                {
                    Assert.IsTrue(_expectedDirections.Contains(direction));
                }
            }

            public void Deactivate()
            {
            }
        }
    }
}