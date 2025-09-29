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
            //// Arrange
            //var config = new Paddle.Config
            //{
            //    _spriteRenderer = new GameObject().GetComponent<SpriteRenderer>(),
            //    _initialBallPosTrfm = new GameObject().transform,
            //    _ballObj = new GameObject(),
            //};
            //var view = new GameObject();
            //var sut = new PaddlePresenter(view, null, config);

            //var mouse = InputSystem.AddDevice<Mouse>();
            //Set(mouse.position, new Vector2(Screen.width * 0.4f, Screen.height * 0.4f));
            ////Move(mouse.position, new Vector2(1000f, 100f));

            //// Act
            //sut.Tick(0.1f);

            //// Assert
            ////Assert.That(view.transform.position.x > 0);
        }
    }
}