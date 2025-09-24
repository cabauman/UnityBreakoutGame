using NUnit.Framework;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.TestTools;

namespace BreakoutGame
{
    public class BallTests
    {
        [TestCase(5, 2, 0)]
        public void InitTest(int initialForce, int power, int startPosX)
        {
            // Arrange
            var view = new GameObject();
            var config = new Ball.Config
            {
                _initialForce = initialForce,
                _power = power,
                _initialAngle = 45f,
                _maxPaddleBounceAngle = 75f
            };
            var sut = new BallPresenter(view, config);

            // Assert
            Assert.That(sut.InitialForce, Is.EqualTo(initialForce));
            Assert.That(sut.Power, Is.EqualTo(power));
            Assert.That(sut.Active.Value, Is.True);
        }

        //[TestCase(45f, 3.5355339059327378f, 3.5355339059327378f)]
        //[TestCase(-30f, -2.5f, 4.330127018922193f)]
        //[TestCase(30f, 2.5f, 4.330127018922193f)]
        //public void GetInitialForceTest(float angle, float expectedX, float expectedY)
        //{
        //    // Arrange
        //    var view = new GameObject();
        //    var config = new Ball.Config
        //    {
        //        _initialForce = 5,
        //        _power = 2,
        //        _initialAngle = 45f,
        //        _maxPaddleBounceAngle = 75f
        //    };
        //    var sut = new BallPresenter(view, config);

        //    // Act
        //    var force = sut.GetInitialForce(angle);

        //    // Assert
        //    Assert.That(force.x, Is.EqualTo(expectedX).Within(0.0001));
        //    Assert.That(force.y, Is.EqualTo(expectedY).Within(0.0001));
        //}

        [TestCase(-2f, -4.82962f, 1.29409f)] // Left edge
        [TestCase(0f, 0f, 5f)]              // Center
        [TestCase(2f, 4.82962f, 1.29409f)]  // Right edge
        public void CalculatePaddleBounceForceTest(float contactX, float expectedX, float expectedY)
        {
            // Arrange
            var view = new GameObject();
            var config = new Ball.Config
            {
                _initialForce = 5,
                _power = 2,
                _initialAngle = 45f,
                _maxPaddleBounceAngle = 75f
            };
            var sut = new BallPresenter(view, config);
            var paddleWidth = 4f;
            var maxBounceAngleRad = 75f * Mathf.Deg2Rad;

            // Act
            //var actual = sut.CalculatePaddleBounceForce(contactX, paddleWidth, maxBounceAngleRad);

            //// Assert
            //Assert.That(actual.x, Is.EqualTo(expectedX).Within(0.0001));
            //Assert.That(actual.y, Is.EqualTo(expectedY).Within(0.0001));
        }
    }
}