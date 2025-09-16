using NUnit.Framework;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public class GameTests
    {
        [Test]
        public void InitTest()
        {
            // Arrange
            var ball = new Ball(5, 2, Vector3.zero);
            var paddle = new Paddle();
            var bricks = new[]
            {
            new Brick(1, 3),
            new Brick(2, 3),
            new Brick(3, 3)
        };
            var sut = new Game(ball, paddle, bricks, 1);

            // Assert
            Assert.That(sut.NumLives.Value, Is.EqualTo(1));
            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(1));
            Assert.That(sut.BricksRemaining.Value, Is.EqualTo(3));
            Assert.That(sut.Ball, Is.EqualTo(ball));
            Assert.That(sut.Paddle, Is.EqualTo(paddle));
            Assert.That(sut.Bricks, Is.EqualTo(bricks));
        }

        [Test]
        public void GameLostTest()
        {
            // Arrange
            var ball = new Ball(5, 2, Vector3.zero);
            var paddle = new Paddle();
            var bricks = new[]
            {
            new Brick(1, 3),
            new Brick(2, 3),
            new Brick(3, 3)
        };
            var sut = new Game(ball, paddle, bricks, 1);

            var gameWonTriggered = false;
            var gameLostTriggered = false;
            sut.GameWon.Subscribe(_ => gameWonTriggered = true);
            sut.GameLost.Subscribe(_ => gameLostTriggered = true);

            // Act
            ball.Active.Value = false;

            // Assert
            Assert.That(sut.NumLives.Value, Is.EqualTo(0));
            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(0));
            Assert.That(gameLostTriggered, Is.True);
            Assert.That(gameWonTriggered, Is.False);
        }

        [Test]
        public void GameWonTest()
        {
            // Arrange
            var ball = new Ball(5, 2, Vector3.zero);
            var paddle = new Paddle();
            var bricks = new[]
            {
            new Brick(1, 3),
        };
            var sut = new Game(ball, paddle, bricks, 1);

            var gameWonTriggered = false;
            var gameLostTriggered = false;
            sut.GameWon.Subscribe(_ => gameWonTriggered = true);
            sut.GameLost.Subscribe(_ => gameLostTriggered = true);

            // Act
            foreach (var brick in bricks)
            {
                brick.RespondToBallCollision.Execute(ball);
            }

            // Assert
            Assert.That(sut.BricksRemaining.Value, Is.EqualTo(0));
            Assert.That(gameWonTriggered, Is.True);
            Assert.That(gameLostTriggered, Is.False);
        }
    }
}