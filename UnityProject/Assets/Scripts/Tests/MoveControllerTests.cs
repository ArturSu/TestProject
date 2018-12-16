using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestProject.Controllers;
using TestProject.Model;
using Coordinates = System.Tuple<int, int>;

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

            var mockMoveInput = new MoveInputMock(new[] {new Coordinates(1, 2), new Coordinates(2, 1)});
            var controller = new MoveController(new List<IMoveInput> {mockMoveInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(new Tuple<SoldierData, InputType>(playerSoldier, InputType.Player));
            controller.Start();
        }
        
        [Test]
        public void LeftUpCornerPosition_TwoDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 5};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier}};

            var mockMoveInput = new MoveInputMock(new[] {new Coordinates(1, 4), new Coordinates(2, 5)});
            var controller = new MoveController(new List<IMoveInput> {mockMoveInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(new Tuple<SoldierData, InputType>(playerSoldier, InputType.Player));
            controller.Start();
        }

        [Test]
        public void RightUpCornerPosition_TwoDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 10, PositionY = 5};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier}};

         
            var mockMoveInput = new MoveInputMock(new[] {new Coordinates(10, 4), new Coordinates(9, 5)});
            var controller = new MoveController(new List<IMoveInput> {mockMoveInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(new Tuple<SoldierData, InputType>(playerSoldier, InputType.Player));
            controller.Start();
        }
        
        [Test]
        public void RightDownCornerPosition_TwoDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 10, PositionY = 1};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier}};
            
            var mockMoveInput = new MoveInputMock(new[] {new Coordinates(10, 2), new Coordinates(9, 1)});
            var controller = new MoveController(new List<IMoveInput> {mockMoveInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(new Tuple<SoldierData, InputType>(playerSoldier, InputType.Player));
            controller.Start();
        }

        [Test]
        public void BoardCenterPosition_FourDirectionsAvailable()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 5, PositionY = 2};
            var battleData = new BattleData
                {AttackDistance = 5, GridHeight = 5, GridWidth = 10, Soldiers = new[] {playerSoldier}};
            
            var mockMoveInput = new MoveInputMock(new[] {new Coordinates(5, 1), new Coordinates(4, 2), new Coordinates(6, 2), new Coordinates(5, 3)});
            var controller = new MoveController(new List<IMoveInput> {mockMoveInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(new Tuple<SoldierData, InputType>(playerSoldier, InputType.Player));
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
            
            var mockMoveInput = new MoveInputMock(new[] {new Coordinates(5, 1), new Coordinates(4, 2), new Coordinates(6, 2)});
            var controller = new MoveController(new List<IMoveInput> {mockMoveInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(new Tuple<SoldierData, InputType>(playerSoldier, InputType.Player));
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

            var mockMoveInput = new MoveInputMock(new[] {new Coordinates(5, 1), new Coordinates(4, 2), new Coordinates(6, 2), new Coordinates(5, 3)});
            var controller = new MoveController(new List<IMoveInput> {mockMoveInput}, new BattleViewStub(), battleData, null);
            controller.Initialize(new Tuple<SoldierData, InputType>(playerSoldier, InputType.Player));
            controller.Start();
        }

        private class MoveInputMock : IMoveInput
        {
            private readonly Coordinates[] _expectedTiles;

            public event Action<Coordinates> TileSelected;
            
            public InputType InputType => InputType.Player;
            
            public MoveInputMock(Coordinates[] expectedTiles)
            {
                _expectedTiles = expectedTiles;
            }

            public void Activate(Tuple<int, int, MoveDirection>[] tiles, int soldierId)
            {
                Assert.IsTrue(tiles.Length == _expectedTiles.Length);
                foreach (var tile in tiles)
                {
                    Assert.IsTrue(_expectedTiles.Any(item => item.Item1 == tile.Item1 && item.Item2 == tile.Item2));
                }
            }

            public void Deactivate()
            {
            }
        }
    }
}