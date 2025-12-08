using System;
using System.Linq;
using R3;
using UnityEngine;
using System.Collections.Generic;

namespace BreakoutGame
{
    public sealed class LevelLoader
    {
        /*
         * NOTES
         * - Load from json file
         */
    }
    [Serializable]
    public sealed class BrickData
    {
        public string Type { get; set; }
        // Prefab ID
        public string PrefabId { get; set; }
        public Vector2 Position { get; set; }
    }
    [Serializable]
    public sealed class LevelData
    {
        public string LevelName { get; set; }
        public List<BrickData> Bricks { get; set; }
    }

    public sealed class GameManager
    {
        /*
         * NOTES
         * - load first level on game start
         * - listen to Level.GameWon event: level transition UI; load next level
         * - listen to Level.GameLost event: game over screen; restart from first level
         */
    }

    public sealed class GameStopwatch
    {
        /*
         * NOTES
         * - Measure the time taken to complete each level
         */
    }

    public sealed class BestTimesManager
    {
        /*
         * NOTES
         * - Manage and store the best times for each level
         */
    }

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

    public sealed class LifeTracker
    {
        public readonly ReactiveProperty<int> Lives;

        public LifeTracker(
            Observable<BallCountChangedEvent> ballCountChanged,
            Observer<ExtraLifeUsedEvent> extraLifeUsed)
        {
            Lives = new ReactiveProperty<int>(3);
            ballCountChanged
                .Where(static evt => evt.Count == 0)
                .Select(_ => Lives.Value -= 1)
                .Where(static lives => lives > 0)
                .Delay(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => extraLifeUsed.OnNext(new ExtraLifeUsedEvent()));
        }
    }

    public abstract class MonoCommand : MonoBehaviour
    {
        public abstract void Execute();
    }


    public readonly struct BallCountChangedEvent
    {
        public int Count { get; init; }
    }
    public readonly struct NumLivesChangedEvent
    {
        public uint NewNumLives { get; init; }
    }
    public readonly struct AllBricksDestroyedEvent { }
    public readonly struct ExtraLifeUsedEvent { }
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
        private bool _gameOver = false;

        public Game(
            Observable<AllBricksDestroyedEvent> allBricksDestroyed,
            LifeTracker lifeTracker,
            // TODO: Consider removing gameOver in favor of the properties GameWon and GameLost
            Observer<GameOverEvent> gameOver)
        {
            Debug.Log("Game Constructor");

            ResetGameCmd = new ReactiveCommand();
            ResetGameCmd.Subscribe(_ => ResetGame());

            GameLost = lifeTracker.Lives
                .Where(lives => lives == 0)
                .AsUnitObservable();

            GameWon = allBricksDestroyed
                .AsUnitObservable();

            GameLost
               .Merge(GameWon)
               .Subscribe(_ => gameOver.OnNext(new GameOverEvent()));
        }

        public Observable<Unit> GameWon { get; private set; }

        public Observable<Unit> GameLost { get; private set; }

        public ReactiveCommand ResetGameCmd { get; }

        private void ResetGame()
        {
            //NumLives.Value = _defaultNumLives;
            //_paddle.Presenter.ResetBallPos.Execute(Unit.Default);
            //_brickManager.ResetGame();
            //_ballManager.ResetGame();
            _gameOver = false;
        }

        private void UseExtraLife()
        {
            //_ballManager.UseExtraLife();
            //_paddle.Presenter.ResetBallPos.Execute(Unit.Default);
        }
    }
}