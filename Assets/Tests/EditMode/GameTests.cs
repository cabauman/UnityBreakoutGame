using NUnit.Framework;
using System.Collections.Generic;
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
            BallPresenter ballPresenter = CreateBall();
            PaddlePresenter paddlePresenter = CreatePaddle(ballPresenter.gameObject);

            // TODO: Set brick presenters
            var bricks = new[]
            {
                CreateBrick(1),
                CreateBrick(2),
                CreateBrick(3),
            };
            var gameView = new GameObject();
            var gameConfig = new GamePresenter.Config
            {
                _ballPresenter = ballPresenter,
                _paddlePresenter = paddlePresenter,
                _brickPresenters = new List<BrickPresenter>(),
                _defaultNumLives = 1,
                _ballPresenterPrefab = ballPresenter,
            };
            var sut = new Game(gameView, gameConfig);

            // Assert
            Assert.That(sut.NumLives.Value, Is.EqualTo(1));
            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(1));
            Assert.That(sut.BricksRemaining.Value, Is.EqualTo(3));
            Assert.That(sut.Ball, Is.EqualTo(ballPresenter.Ball));
            Assert.That(sut.Paddle, Is.EqualTo(paddlePresenter.Paddle));
            Assert.That(sut.Bricks, Is.EqualTo(bricks));
        }

        [Test]
        public void GameLostTest()
        {
            // Arrange
            BallPresenter ballPresenter = CreateBall();
            PaddlePresenter paddlePresenter = CreatePaddle(ballPresenter.gameObject);

            var bricks = new[]
            {
                CreateBrick(1),
                CreateBrick(2),
                CreateBrick(3),
            };
            var gameView = new GameObject();
            var gameConfig = new GamePresenter.Config
            {
                _ballPresenter = ballPresenter,
                _paddlePresenter = paddlePresenter,
                _brickPresenters = new List<BrickPresenter>(),
                _defaultNumLives = 1,
                _ballPresenterPrefab = ballPresenter,
            };
            var sut = new Game(gameView, gameConfig);

            var gameWonTriggered = false;
            var gameLostTriggered = false;
            sut.GameWon.Subscribe(_ => gameWonTriggered = true);
            sut.GameLost.Subscribe(_ => gameLostTriggered = true);

            // Act
            ballPresenter.Ball.Active.Value = false;

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
            BallPresenter ballPresenter = CreateBall();
            PaddlePresenter paddlePresenter = CreatePaddle(ballPresenter.gameObject);

            var bricks = new[] { CreateBrick(1) };
            var gameView = new GameObject();
            var gameConfig = new GamePresenter.Config
            {
                _ballPresenter = ballPresenter,
                _paddlePresenter = paddlePresenter,
                _brickPresenters = new List<BrickPresenter>(),
                _defaultNumLives = 1,
                _ballPresenterPrefab = ballPresenter,
            };
            var sut = new Game(gameView, gameConfig);

            var gameWonTriggered = false;
            var gameLostTriggered = false;
            sut.GameWon.Subscribe(_ => gameWonTriggered = true);
            sut.GameLost.Subscribe(_ => gameLostTriggered = true);

            // Act
            bricks[0].RespondToBallCollision.Execute(ballPresenter.Ball);

            // Assert
            Assert.That(sut.BricksRemaining.Value, Is.EqualTo(0));
            Assert.That(gameWonTriggered, Is.True);
            Assert.That(gameLostTriggered, Is.False);
        }

        [Test]
        public void ResetGameTest()
        {
            // Arrange
            BallPresenter ballPresenter = CreateBall();
            PaddlePresenter paddlePresenter = CreatePaddle(ballPresenter.gameObject);

            var bricks = new[] { CreateBrick(1) };
            var gameView = new GameObject();
            var gameConfig = new GamePresenter.Config
            {
                _ballPresenter = ballPresenter,
                _paddlePresenter = paddlePresenter,
                _brickPresenters = new List<BrickPresenter>(),
                _defaultNumLives = 1,
                _ballPresenterPrefab = ballPresenter,
            };
            var sut = new Game(gameView, gameConfig);

            ballPresenter.Ball.Active.Value = false;
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
            Assert.That(ballPresenter.Ball.Active.Value, Is.True);
        }

        [Test]
        public void CreateBonusBallTest()
        {
            // Arrange
            BallPresenter ballPresenter = CreateBall();
            PaddlePresenter paddlePresenter = CreatePaddle(ballPresenter.gameObject);

            var bricks = new[] { CreateBrick(1) };
            var gameView = new GameObject();
            var gameConfig = new GamePresenter.Config
            {
                _ballPresenter = ballPresenter,
                _paddlePresenter = paddlePresenter,
                _brickPresenters = new List<BrickPresenter>(),
                _defaultNumLives = 1,
                _ballPresenterPrefab = ballPresenter,
            };
            var sut = new Game(gameView, gameConfig);

            var bonusBallView = new GameObject();
            var bonusBallConfig = new BallPresenter.Config
            {
                _initialForce = 5,
                _power = 2,
                _initialAngle = 45f,
                _maxPaddleBounceAngle = 75f
            };
            var bonusBall = new Ball(bonusBallView, bonusBallConfig);

            // Act
            sut.CreateBonusBall.Execute(new Vector3(5, 5, 0));

            // Assert
            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(2));

            // Act - Deactivate the bonus ball
            bonusBall.Active.Value = false;

            // Assert
            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(1));
        }

        private static BallPresenter CreateBall()
        {
            var ballView = new GameObject();
            var ballConfig = new BallPresenter.Config
            {
                _initialForce = 5,
                _power = 2,
                _initialAngle = 45f,
                _maxPaddleBounceAngle = 75f
            };
            var ball = new Ball(ballView, ballConfig);
            var ballPresenter = ballView.AddComponent<BallPresenter>();
            ballPresenter.Ball = ball;
            return ballPresenter;
        }

        private static PaddlePresenter CreatePaddle(GameObject ballView)
        {
            var paddleView = new GameObject();
            var paddleConfig = new PaddlePresenter.Config
            {
                _ballPresenter = ballView,
                _graphicTrfm = paddleView.transform,
                _initialBallPosTrfm = paddleView.transform,
            };
            var paddle = new Paddle(paddleView, paddleConfig);
            var paddlePresenter = paddleView.AddComponent<PaddlePresenter>();
            paddlePresenter.Paddle = paddle;
            return paddlePresenter;
        }

        private static Brick CreateBrick(int initialHp)
        {
            var brickView = new GameObject();
            var brickConfig = new BrickPresenter.Config
            {
                _initialHp = initialHp,
                _powerUpSpawnOdds = 3,
                _powerUpPrefab = null,
            };
            return new Brick(brickView, brickConfig);
        }
    }
}



// Either yield return null or call InputSystem.Update() to update the state of the input system.


// InputSystem.QueueStateEvent(mouse, new MouseState() { position = Vector2.zero }.WithButton(MouseButton.Right, true));
// yield return null;
// InputSystem.QueueDeltaStateEvent(mouse.position, new Vector2(100f, 0f));
// yield return null;
// InputSystem.QueueStateEvent(mouse, new MouseState() { position = Vector2.right * 100f }.WithButton(MouseButton.Right, false));
// yield return new WaitForSeconds(3f);