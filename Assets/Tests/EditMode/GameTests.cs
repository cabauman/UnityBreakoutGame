//using NUnit.Framework;
//using System.Collections.Generic;
//using UniRx;
//using UnityEngine;

//namespace BreakoutGame
//{
//    public class GameTests
//    {
//        [Test]
//        public void InitTest()
//        {
//            // Arrange
//            Ball ballPresenter = CreateBall();
//            Paddle paddlePresenter = CreatePaddle(ballPresenter.gameObject);

//            // TODO: Set brick presenters
//            var bricks = new[]
//            {
//                CreateBrick(1),
//                CreateBrick(2),
//                CreateBrick(3),
//            };
//            var gameView = new GameObject();
//            var gameConfig = new GamePresenter.Config
//            {
//                _ballPresenter = ballPresenter,
//                _paddlePresenter = paddlePresenter,
//                _brickPresenters = new List<Brick>(),
//                _defaultNumLives = 1,
//                _ballPresenterPrefab = ballPresenter,
//            };
//            var sut = new GameDraft(gameView, gameConfig);

//            // Assert
//            Assert.That(sut.NumLives.Value, Is.EqualTo(1));
//            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(1));
//            Assert.That(sut.BricksRemaining.Value, Is.EqualTo(3));
//            Assert.That(sut.Ball, Is.EqualTo(ballPresenter.Presenter));
//            Assert.That(sut.Paddle, Is.EqualTo(paddlePresenter.Presenter));
//            Assert.That(sut.Bricks, Is.EqualTo(bricks));
//        }

//        [Test]
//        public void GameLostTest()
//        {
//            // Arrange
//            Ball ballPresenter = CreateBall();
//            Paddle paddlePresenter = CreatePaddle(ballPresenter.gameObject);

//            var bricks = new[]
//            {
//                CreateBrick(1),
//                CreateBrick(2),
//                CreateBrick(3),
//            };
//            var gameView = new GameObject();
//            var gameConfig = new GamePresenter.Config
//            {
//                _ballPresenter = ballPresenter,
//                _paddlePresenter = paddlePresenter,
//                _brickPresenters = new List<Brick>(),
//                _defaultNumLives = 1,
//                _ballPresenterPrefab = ballPresenter,
//            };
//            var sut = new GameDraft(gameView, gameConfig);

//            var gameWonTriggered = false;
//            var gameLostTriggered = false;
//            sut.GameWon.Subscribe(_ => gameWonTriggered = true);
//            sut.GameLost.Subscribe(_ => gameLostTriggered = true);

//            // Act
//            ballPresenter.Presenter.Active.Value = false;

//            // Assert
//            Assert.That(sut.NumLives.Value, Is.EqualTo(0));
//            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(0));
//            Assert.That(gameLostTriggered, Is.True);
//            Assert.That(gameWonTriggered, Is.False);
//        }

//        [Test]
//        public void GameWonTest()
//        {
//            // Arrange
//            Ball ballPresenter = CreateBall();
//            Paddle paddlePresenter = CreatePaddle(ballPresenter.gameObject);

//            var bricks = new[] { CreateBrick(1) };
//            var gameView = new GameObject();
//            var gameConfig = new GamePresenter.Config
//            {
//                _ballPresenter = ballPresenter,
//                _paddlePresenter = paddlePresenter,
//                _brickPresenters = new List<Brick>(),
//                _defaultNumLives = 1,
//                _ballPresenterPrefab = ballPresenter,
//            };
//            var sut = new GameDraft(gameView, gameConfig);

//            var gameWonTriggered = false;
//            var gameLostTriggered = false;
//            sut.GameWon.Subscribe(_ => gameWonTriggered = true);
//            sut.GameLost.Subscribe(_ => gameLostTriggered = true);

//            // Act
//            bricks[0].RespondToBallCollision.Execute(ballPresenter.Presenter);

//            // Assert
//            Assert.That(sut.BricksRemaining.Value, Is.EqualTo(0));
//            Assert.That(gameWonTriggered, Is.True);
//            Assert.That(gameLostTriggered, Is.False);
//        }

//        [Test]
//        public void ResetGameTest()
//        {
//            // Arrange
//            Ball ballPresenter = CreateBall();
//            Paddle paddlePresenter = CreatePaddle(ballPresenter.gameObject);

//            var bricks = new[] { CreateBrick(1) };
//            var gameView = new GameObject();
//            var gameConfig = new GamePresenter.Config
//            {
//                _ballPresenter = ballPresenter,
//                _paddlePresenter = paddlePresenter,
//                _brickPresenters = new List<Brick>(),
//                _defaultNumLives = 1,
//                _ballPresenterPrefab = ballPresenter,
//            };
//            var sut = new GameDraft(gameView, gameConfig);

//            ballPresenter.Presenter.Active.Value = false;
//            Assert.That(sut.NumLives.Value, Is.EqualTo(0));
//            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(0));

//            // Act
//            sut.ResetGameCmd.Execute();

//            // Assert
//            Assert.That(sut.NumLives.Value, Is.EqualTo(1));
//            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(1));
//            Assert.That(sut.BricksRemaining.Value, Is.EqualTo(1));

//            Assert.That(bricks[0].Hp.Value, Is.EqualTo(1));
//            Assert.That(bricks[0].Active.Value, Is.True);
//            Assert.That(ballPresenter.Presenter.Active.Value, Is.True);
//        }

//        [Test]
//        public void CreateBonusBallTest()
//        {
//            // Arrange
//            Ball ballPresenter = CreateBall();
//            Paddle paddlePresenter = CreatePaddle(ballPresenter.gameObject);

//            var bricks = new[] { CreateBrick(1) };
//            var gameView = new GameObject();
//            var gameConfig = new GamePresenter.Config
//            {
//                _ballPresenter = ballPresenter,
//                _paddlePresenter = paddlePresenter,
//                _brickPresenters = new List<Brick>(),
//                _defaultNumLives = 1,
//                _ballPresenterPrefab = ballPresenter,
//            };
//            var sut = new GameDraft(gameView, gameConfig);

//            var bonusBallView = new GameObject();
//            var bonusBallConfig = new Ball.Config
//            {
//                _initialForce = 5,
//                _power = 2,
//                _initialAngle = 45f,
//                _maxPaddleBounceAngle = 75f,
//            };
//            var bonusBall = new BallPresenter(bonusBallView, bonusBallConfig);

//            // Act
//            sut.CreateBonusBall.Execute(new Vector3(5, 5, 0));

//            // Assert
//            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(2));

//            // Act - Deactivate the bonus ball
//            bonusBall.Active.Value = false;

//            // Assert
//            Assert.That(sut.NumBallsInPlay.Value, Is.EqualTo(1));
//        }

//        private static Ball CreateBall()
//        {
//            var ballView = new GameObject();
//            var ballConfig = new Ball.Config
//            {
//                _initialForce = 5,
//                _power = 2,
//                _initialAngle = 45f,
//                _maxPaddleBounceAngle = 75f,
//            };
//            var ball = new BallPresenter(ballView, ballConfig);
//            var ballPresenter = ballView.AddComponent<Ball>();
//            ballPresenter.Presenter = ball;
//            return ballPresenter;
//        }

//        private static Paddle CreatePaddle(GameObject ballView)
//        {
//            var paddleView = new GameObject();
//            var paddleConfig = new Paddle.Config
//            {
//                _ballObj = ballView,
//                _graphicTrfm = paddleView.transform,
//                _initialBallPosTrfm = paddleView.transform,
//            };
//            var paddle = new PaddlePresenter(paddleView, paddleConfig);
//            var paddlePresenter = paddleView.AddComponent<Paddle>();
//            paddlePresenter.Presenter = paddle;
//            return paddlePresenter;
//        }

//        private static BrickPresenter CreateBrick(int initialHp)
//        {
//            var brickView = new GameObject();
//            var brickConfig = new Brick.Config
//            {
//                _initialHp = initialHp,
//                _powerUpSpawnOdds = 3,
//                _powerUpPrefab = null,
//            };
//            return new BrickPresenter(brickView, brickConfig);
//        }
//    }
//}