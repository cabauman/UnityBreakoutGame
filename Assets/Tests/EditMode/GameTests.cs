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
            var paddle = new Paddle(null, null);
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
            var paddle = new Paddle(null, null);
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
            var paddle = new Paddle(null, null);
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
            bricks[0].RespondToBallCollision.Execute(ball);

            // Assert
            Assert.That(sut.BricksRemaining.Value, Is.EqualTo(0));
            Assert.That(gameWonTriggered, Is.True);
            Assert.That(gameLostTriggered, Is.False);
        }

        [Test]
        public void ResetGameTest()
        {
            // Arrange
            var ball = new Ball(5, 2, Vector3.zero);
            var paddle = new Paddle(null, null);
            var bricks = new[]
            {
                new Brick(1, 3),
            };

            var sut = new Game(ball, paddle, bricks, 1);

            ball.Active.Value = false;
            Assert.That(sut.NumLives.Value, Is.EqualTo(0));
            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(0));

            // Act
            sut.ResetGameCmd.Execute();

            // Assert
            Assert.That(sut.NumLives.Value, Is.EqualTo(1));
            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(1));
            Assert.That(sut.BricksRemaining.Value, Is.EqualTo(1));

            Assert.That(bricks[0].Hp.Value, Is.EqualTo(1));
            Assert.That(bricks[0].Active.Value, Is.True);
            Assert.That(ball.Active.Value, Is.True);
        }

        [Test]
        public void CreateBonusBallTest()
        {
            // Arrange
            var ball = new Ball(5, 1, Vector3.zero);
            var paddle = new Paddle(null, null);
            var bricks = new[] { new Brick(1, 3) };
            var sut = new Game(ball, paddle, bricks, 1);

            var bonusBall = new Ball(5, 1, Vector3.zero);

            // Act
            sut.CreateBonusBall.Execute(bonusBall);

            // Assert
            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(2));

            // Act - Deactivate the bonus ball
            bonusBall.Active.Value = false;

            // Assert
            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(1));
        }
    }
}



// Either yield return null or call InputSystem.Update() to update the state of the input system.


            InputSystem.QueueStateEvent(mouse, new MouseState() { position = Vector2.zero }.WithButton(MouseButton.Right, true));
            yield return null;
            InputSystem.QueueDeltaStateEvent(mouse.position, new Vector2(100f, 0f));
            yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState() { position = Vector2.right * 100f }.WithButton(MouseButton.Right, false));
            yield return new WaitForSeconds(3f);