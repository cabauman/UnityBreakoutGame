using System;
using System.Linq;
using R3;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    public interface IPlayerInputProvider
    {
        float GetHorizontalInput();
        bool IsLaunchPressed();
    }
    public sealed class PlayerInputProvider : MonoBehaviour, IPlayerInputProvider
    {
        [SerializeField] public InputActionReference _moveAction;
        [SerializeField] public InputActionReference _launchAction;

        public float GetHorizontalInput()
        {
            return _moveAction.action.ReadValue<Vector2>().x;
        }

        public bool IsLaunchPressed()
        {
            return _launchAction.action.triggered;
        }
    }

    public sealed class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHp = 1;
        private int _currentHp;

        public int CurrentHp => _currentHp;
        public int MaxHp => _maxHp;

        public event Action<int> OnHpChanged;
        public event Action OnDestroyed;

        private void Awake()
        {
            _currentHp = _maxHp;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative.");
            }
            _currentHp -= damage;
            if (_currentHp < 0) _currentHp = 0;
            OnHpChanged?.Invoke(_currentHp);
            if (_currentHp == 0)
            {
                OnDestroyed?.Invoke();
                Destroy(gameObject);
            }
        }

        public void Heal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Heal amount cannot be negative.");
            }
            _currentHp += amount;
            if (_currentHp > _maxHp) _currentHp = _maxHp;
            OnHpChanged?.Invoke(_currentHp);
        }

        public void ResetHealth()
        {
            _currentHp = _maxHp;
            OnHpChanged?.Invoke(_currentHp);
        }
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
    public sealed partial class IncrementScoreCommand : MonoCommand
    {
        [SerializeField] private int _pointsToAdd = 100;
        [UniDig.Inject] private ScoreKeeper _scoreKeeper;

        public override void Execute()
        {
            _scoreKeeper.AddPoints(_pointsToAdd);
        }
    }
    public sealed partial class SpawnPowerUpCommand : MonoCommand
    {
        [SerializeField] private PowerUpTable _powerUpTable;
        [UniDig.Inject] private IPowerUpSpawner _powerUpSpawner;

        public override void Execute()
        {
            _powerUpSpawner.SpawnPowerUp(_powerUpTable, transform.position);
        }
    }
    public sealed class PlayAudioClipCommand : MonoCommand
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] [Range(0f, 1f)] private float _volume = 1f;
        [SerializeField] private AudioSource _audioSource;

        public override void Execute()
        {
            _audioSource.PlayOneShot(_clip, _volume);
        }
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