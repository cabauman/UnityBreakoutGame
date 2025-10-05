using System.Collections;
using NSubstitute;
using NUnit.Framework;
using R3;
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
            var config = new Brick.Config { _initialHp = 1 };
            var powerUpTable = ScriptableObject.CreateInstance<PowerUpTable>();
            var powerUpSpawner = Substitute.For<IPowerUpSpawner>();
            var sut = new BrickPresenter(view, config, powerUpTable, powerUpSpawner);

            // Assert
            Assert.That(sut.Hp.Value, Is.EqualTo(1));
            Assert.That(sut.Active.CurrentValue, Is.True);
        }

        [TestCase(3, 1, 2)]
        [TestCase(3, 3, 0)]
        [TestCase(2, 5, -3)]
        public void HpReducedByBallPowerAmount(int initialHp, int ballPower, int expectedRemainingHp)
        {
            // Arrange
            var view = new GameObject();
            var config = new Brick.Config { _initialHp = initialHp };
            var powerUpTable = ScriptableObject.CreateInstance<PowerUpTable>();
            var powerUpSpawner = Substitute.For<IPowerUpSpawner>();
            var sut = new BrickPresenter(view, config, powerUpTable, powerUpSpawner);

            var ballObj = new GameObject();
            var ball = ballObj.AddComponent<Ball>();
            var ballConfig = new Ball.Config
            {
                _power = ballPower,
            };
            var testableBall = (ITestable<Ball.Config>)ball;
            testableBall.SetConfig(ballConfig);
            InvokeLifecycleFunction(ball, "Awake");

            // Act
            sut.OnCollisionEnter2D(ballObj);

            // Assert
            Assert.That(sut.Hp.Value, Is.EqualTo(expectedRemainingHp));
        }

        [Test]
        public void ActiveSetToFalseWhenHpReaches0()
        {
            // Arrange
            var view = new GameObject();
            var config = new Brick.Config
            {
                _initialHp = 2,
            };
            var powerUpTable = ScriptableObject.CreateInstance<PowerUpTable>();
            var powerUpSpawner = Substitute.For<IPowerUpSpawner>();
            var sut = new BrickPresenter(view, config, powerUpTable, powerUpSpawner);

            var ballObj = new GameObject();
            var ball = ballObj.AddComponent<Ball>();
            var ballConfig = new Ball.Config
            {
                _power = 2,
            };
            var testableBall = (ITestable<Ball.Config>)ball;
            testableBall.SetConfig(ballConfig);
            InvokeLifecycleFunction(ball, "Awake");
            //var ballPresenter = new BallPresenter(ballObj, ballConfig);

            // Act
            sut.OnCollisionEnter2D(ballObj);

            // Assert
            Assert.That(sut.Active.CurrentValue, Is.False);
        }

        [Test]
        public void PowerUpSpawnerNotInvoked_When_HpGreaterThan0()
        {
            // Arrange
            var view = new GameObject();
            view.transform.position = new Vector3(1, 2, 3);
            var config = new Brick.Config
            {
                _initialHp = 2,
            };
            var powerUpTable = ScriptableObject.CreateInstance<PowerUpTable>();
            var powerUpSpawner = Substitute.For<IPowerUpSpawner>();
            var sut = new BrickPresenter(view, config, powerUpTable, powerUpSpawner);

            var ballObj = new GameObject();
            var ball = ballObj.AddComponent<Ball>();
            var ballConfig = new Ball.Config
            {
                _power = 1,
            };
            var testableBall = (ITestable<Ball.Config>)ball;
            testableBall.SetConfig(ballConfig);
            InvokeLifecycleFunction(ball, "Awake");

            // Act
            sut.OnCollisionEnter2D(ballObj);

            // Assert
            //powerUpSpawner.DidNotReceive().SpawnPowerUp(Arg.Any<Vector3>());
        }

        [Test]
        public void PowerUpSpawnerInvoked_When_HpReaches0()
        {
            // Arrange
            var view = new GameObject();
            view.transform.position = new Vector3(1, 2, 3);
            var config = new Brick.Config
            {
                _initialHp = 2,
            };
            var powerUpTable = ScriptableObject.CreateInstance<PowerUpTable>();
            var powerUpSpawner = Substitute.For<IPowerUpSpawner>();
            var sut = new BrickPresenter(view, config, powerUpTable, powerUpSpawner);

            var ballObj = new GameObject();
            var ball = ballObj.AddComponent<Ball>();
            var ballConfig = new Ball.Config
            {
                _power = 2,
            };
            var testableBall = (ITestable<Ball.Config>)ball;
            testableBall.SetConfig(ballConfig);
            InvokeLifecycleFunction(ball, "Awake");

            // Act
            sut.OnCollisionEnter2D(ballObj);

            // Assert
            //powerUpSpawner.Received(1).SpawnPowerUp(new(1, 2, 3));
        }

        [Test]
        public void ResetHpTest()
        {
            // Arrange
            var view = new GameObject();
            var config = new Brick.Config
            {
                _initialHp = 2,
            };
            var powerUpTable = ScriptableObject.CreateInstance<PowerUpTable>();
            var powerUpSpawner = Substitute.For<IPowerUpSpawner>();
            var sut = new BrickPresenter(view, config, powerUpTable, powerUpSpawner);

            var ballObj = new GameObject();
            var ball = ballObj.AddComponent<Ball>();
            var ballConfig = new Ball.Config
            {
                _power = 2,
            };
            var testableBall = (ITestable<Ball.Config>)ball;
            testableBall.SetConfig(ballConfig);
            InvokeLifecycleFunction(ball, "Awake");
            //var ballPresenter = new BallPresenter(ballObj, ballConfig);
            //sut.OnCollisionEnter2D(ballObj);

            sut.Hp.Value = 0;
            Assert.That(sut.Active.CurrentValue, Is.False);

            // Act
            sut.ResetHp.Execute(Unit.Default);

            // Assert
            Assert.That(sut.Hp.Value, Is.EqualTo(2));
            Assert.That(sut.Active.CurrentValue, Is.True);
        }

        public static void InvokeLifecycleFunction<T>(T mono, string name)
        {
            var method = typeof(T).GetMethod(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(mono, null);
        }
    }
}