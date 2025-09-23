using System.Collections;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.TestTools;

namespace BreakoutGame
{
    public class BrickTests
    {
        [Test]
        public void InitTest()
        {
            // Arrange
            var view = new GameObject();
            var config = new Brick.Config
            {
                _initialHp = 1,
                _powerUpSpawnOdds = 3,
            };
            var sut = new BrickPresenter(view, config);

            // Assert
            Assert.That(sut.Hp.Value, Is.EqualTo(1));
            Assert.That(sut.Active.Value, Is.True);
        }

        // TODO: Mock RandomUtil to make this test deterministic
        [TestCase(3, 1, 2)]
        [TestCase(3, 3, 0)]
        [TestCase(2, 5, -3)]
        public void RespondToBallCollisionTest(int initialHp, int ballPower, int expectedRemainingHp)
        {
            // Arrange
            var powerUpPrefab = new GameObject().AddComponent<PowerUp>();
            var view = new GameObject();
            var config = new Brick.Config
            {
                _initialHp = initialHp,
                _powerUpSpawnOdds = 3,
                _powerUpPrefab = powerUpPrefab,
            };
            var sut = new BrickPresenter(view, config);

            var ballView = new GameObject();
            var ballConfig = new Ball.Config
            {
                _initialForce = 5,
                _initialAngle = 45,
                _power = ballPower,
                _maxPaddleBounceAngle = 75,
            };
            var ball = new BallPresenter(ballView, ballConfig);

            // Act
            sut.RespondToBallCollision.Execute(ball);

            // Assert
            Assert.That(sut.Hp.Value, Is.EqualTo(expectedRemainingHp));
        }

        // TODO: Mock RandomUtil to make this test deterministic
        [Test]
        public void ActiveTest()
        {
            // Arrange
            var powerUpPrefab = new GameObject().AddComponent<PowerUp>();
            var view = new GameObject();
            var config = new Brick.Config
            {
                _initialHp = 2,
                _powerUpSpawnOdds = 3,
                _powerUpPrefab = powerUpPrefab,
            };
            var sut = new BrickPresenter(view, config);

            var ballView = new GameObject();
            var ballConfig = new Ball.Config
            {
                _initialForce = 5,
                _initialAngle = 45,
                _power = 2,
                _maxPaddleBounceAngle = 75,
            };
            var ball = new BallPresenter(ballView, ballConfig);

            // Act
            sut.RespondToBallCollision.Execute(ball);

            // Assert
            Assert.That(sut.Active.Value, Is.False);
        }

        [Test]
        public void ResetHpTest()
        {
            // Arrange
            var view = new GameObject();
            var config = new Brick.Config
            {
                _initialHp = 2,
                _powerUpSpawnOdds = 3,
            };
            var sut = new BrickPresenter(view, config);

            var ballView = new GameObject();
            var ballConfig = new Ball.Config
            {
                _initialForce = 5,
                _initialAngle = 45,
                _power = 2,
                _maxPaddleBounceAngle = 75,
            };
            var ball = new BallPresenter(ballView, ballConfig);
            //sut.RespondToBallCollision.Execute(ball);

            sut.Hp.Value = 0;
            Assert.That(sut.Active.Value, Is.False);

            // Act
            sut.ResetHp.Execute(Unit.Default);

            // Assert
            Assert.That(sut.Hp.Value, Is.EqualTo(2));
            Assert.That(sut.Active.Value, Is.True);
        }
    }
}