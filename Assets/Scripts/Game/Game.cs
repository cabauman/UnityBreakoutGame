using System;
using System.Linq;
using R3;
using UnityEngine;
using System.Collections.Generic;

namespace BreakoutGame
{
    public interface IPlayerInputProvider
    {
        Vector2 GetHorizontalInput();
        bool IsLaunchPressed();
    }

    public sealed class ScoreKeeper
    {
        private int _score = 0;
        public int Score => _score;

        public void AddPoints(int points)
        {
            if (points < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(points), "Points to add cannot be negative.");
            }
            _score += points;
        }

        public void ResetScore()
        {
            _score = 0;
        }
    }

    public abstract class MonoCommand : MonoBehaviour
    {
        public abstract void Execute();
    }



    public readonly struct NumLivesChangedEvent
    {
        public uint NewNumLives { get; init; }
    }
    public readonly struct NewGameRequest { }
    public readonly struct GameOverEvent { }
    public interface IPublisher<T>
    {
        void Publish(T eventData);
    }
    public interface ISubscriber<T>
    {
        void Subscribe(Action<T> onEvent);
        void Unsubscribe(Action<T> onEvent);
    }

    public class Signal<T> : IPublisher<T>, ISubscriber<T>
    {
        public List<Action<T>> _subscribers = new();
        public void Publish(T eventData)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Invoke(eventData);
            }
        }
        public void Subscribe(Action<T> onEvent)
        {
            _subscribers.Add(onEvent);
        }
        public void Unsubscribe(Action<T> onEvent)
        {
            _subscribers.Remove(onEvent);
        }
    }



    public interface IGame
    {
    }
    public sealed class Game : IGame
    {
        private readonly BrickManager _brickManager;
        private readonly BallManager _ballManager;
        private readonly Paddle _paddle;
        private readonly uint _defaultNumLives;
        private bool _gameOver = false;

        public Game(BrickManager brickManager, BallManager ballManager, Paddle paddle)
        {
            Debug.Log("Game Constructor");
            _brickManager = brickManager;
            _ballManager = ballManager;
            _paddle = paddle;
            _defaultNumLives = 2;

            NumLives = new ReactiveProperty<uint>(_defaultNumLives);

            ResetGameCmd = new ReactiveCommand();
            ResetGameCmd.Subscribe(_ => ResetGame());
            //GameCtor.DevToolbox.StartupLifecycle.AddPostInjectListener(PostInject);

            var noBallsInPlay = _ballManager.NumBallsInPlay
                .Where(count => count == 0)
                .Do(_ => NumLives.Value -= 1)
                .Do(_ => Debug.Log($"NumLives is now {NumLives.Value}"))
                .Publish()
                .RefCount();

            noBallsInPlay
                .Where(_ => NumLives.Value > 0)
                .Delay(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => UseExtraLife());

            GameLost = noBallsInPlay
                .Where(_ => NumLives.Value == 0)
                .Select(static _ => Unit.Default);

            GameWon = _brickManager.BricksRemaining
                .Where(count => count == 0)
                .Do(_ => _gameOver = true)
                .Select(static _ => Unit.Default);

            // Just making sure brick manager dependencies are injected by the time this constructor runs
            Debug.Log(_brickManager.Random);

            //Observable
            //    .EveryUpdate()
            //    .Where(_ =>
            //        UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            //    .Subscribe(_ => _ballManager.Ball.Presenter.AddInitialForce());

            //GameLost
            //    .Merge(GameWon)
            //    .Subscribe(_ => Debug.Log(_gameOver ? "Game Won!" : "Game Lost!"));
        }

        public Observable<Unit> GameWon { get; private set; }

        public Observable<Unit> GameLost { get; private set; }

        public ReactiveProperty<uint> NumLives { get; }

        public ReactiveCommand ResetGameCmd { get; }

        private void ResetGame()
        {
            NumLives.Value = _defaultNumLives;
            _paddle.Presenter.ResetBallPos.Execute(Unit.Default);
            _brickManager.ResetGame();
            _ballManager.ResetGame();
            _gameOver = false;
        }

        private void UseExtraLife()
        {
            _ballManager.UseExtraLife();
            _paddle.Presenter.ResetBallPos.Execute(Unit.Default);
        }
    }
}