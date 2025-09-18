using System.Collections;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace BreakoutGame
{
    public class PaddleTests : InputTestFixture
    {
        [Test]
        public void InitTest()
        {
            // Arrange
            var config = new PaddlePresenter.Config
            {
                _graphicTrfm = new GameObject().transform,
                _initialBallPosTrfm = new GameObject().transform,
                _ballPresenter = new GameObject().AddComponent<BallPresenter>()
            };
            var view = new GameObject().AddComponent<PaddlePresenter>();
            var sut = new Paddle(view, config);

            var mouse = InputSystem.AddDevice<Mouse>();
            Set(mouse.position, new Vector2(Screen.width * 0.4f, Screen.height * 0.4f));
            //Move(mouse.position, new Vector2(1000f, 100f));

            // Act
            sut.Tick(0.1f);

            // Assert
            //Assert.That(view.transform.position.x > 0);
        }
    }
}