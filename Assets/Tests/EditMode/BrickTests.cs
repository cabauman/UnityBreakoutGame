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
            var sut = new Brick(1, 3);

            // Assert
            Assert.That(sut.Hp.Value, Is.EqualTo(1));
            Assert.That(sut.Active.Value, Is.True);
        }

        [TestCase(3, 1, 2)]
        [TestCase(3, 3, 0)]
        [TestCase(2, 5, -3)]
        public void RespondToBallCollisionTest(int initialHp, int ballPower, int expectedRemainingHp)
        {
            // Arrange
            var sut = new Brick(initialHp, 3);
            var ball = new Ball(5, ballPower, Vector3.one);

            // Act
            sut.RespondToBallCollision.Execute(ball);

            // Assert
            Assert.That(sut.Hp.Value, Is.EqualTo(expectedRemainingHp));
        }

        [Test]
        public void ActiveTest()
        {
            // Arrange
            var sut = new Brick(2, 3);
            var ball = new Ball(5, 2, Vector3.one);

            // Act
            sut.RespondToBallCollision.Execute(ball);

            // Assert
            Assert.That(sut.Active.Value, Is.False);
        }

        [Test]
        public void ResetHpTest()
        {
            // Arrange
            var sut = new Brick(2, 3);
            var ball = new Ball(5, 2, Vector3.one);
            //sut.RespondToBallCollision.Execute(ball);

            sut.Hp.Value = 0;
            Assert.That(sut.Active.Value, Is.False);

            // Act
            sut.ResetHp.Execute(Unit.Default);

            // Assert
            Assert.That(sut.Hp.Value, Is.EqualTo(2));
            Assert.That(sut.Active.Value, Is.True);
        }

        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}