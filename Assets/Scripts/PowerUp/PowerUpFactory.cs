using BreakoutGame;
using GameCtor.DevToolbox;
using System.Linq;
using UnityEngine;

public class PowerUpFactory
{
    public virtual PowerUpPresenter Create(PowerUpConfig config, Vector3 position)
    {
        var instance = Object.Instantiate(config.Prefab, position, Quaternion.identity);
        instance.Init(config);
        return instance;
    }
}

public enum PowerUpKind
{
    None,
    ExtraLife,
    ExtraBall,
}

public class PowerUpConfigWrapper
{
    public PowerUpConfig Config;
    public PowerUpAction Action;

    public PowerUpConfigWrapper(PowerUpConfig config, PowerUpAction action)
    {
        Config = config;
        Action = action;
    }
}

public abstract class PowerUpAction
{
    public abstract void ApplyEffect();
}
public class ExtraLifePowerUpAction : PowerUpAction
{
    private ServiceA _serviceA;
    public ExtraLifePowerUpAction(ServiceA serviceA)
    {
        _serviceA = serviceA;
    }
    public override void ApplyEffect()
    {
        Debug.Log($"Applied ExtraLifePowerUpAction: {_serviceA} extra lives granted");
    }
}

[System.Serializable]
public abstract class PowerUpConfig
{
    public string Name;
    public PowerUpKind Kind;
    public PowerUpPresenter Prefab;
    public int Weight;

    public abstract void ApplyEffect();
}

[System.Serializable]
public class ExtraLifePowerUpConfig : PowerUpConfig
{
    public int ExtraLives;
    [Inject] private ServiceA _serviceA;
    public void Inject(ServiceA serviceA)
    {
        _serviceA = serviceA;
    }
    public override void ApplyEffect()
    {
        // Implement the effect of extra lives here
        Debug.Log($"Applied ExtraLifePowerUp: {ExtraLives} extra lives granted: {_serviceA}");
    }
}

[System.Serializable]
public class ExtraBallPowerUpConfig : PowerUpConfig
{
    public int ExtraBalls;
    [Inject] private ServiceA _serviceA;
    public void Inject(ServiceA serviceA)
    {
        _serviceA = serviceA;
    }
    public override void ApplyEffect()
    {
        // Implement the effect of extra balls here
        Debug.Log($"Applied ExtraBallPowerUp: {ExtraBalls} extra balls granted: {_serviceA}");
    }
}

public interface IPowerUpSpawner
{
    void SpawnPowerUp(Vector3 position);
}

public interface IRandom
{
    int Next(int minValue, int maxValue);
}

public sealed class UnityRandom : IRandom
{
    /// <summary>
    /// Return a random int within [minInclusive..maxExclusive).
    /// </summary>
    public int Next(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }
}

public class PowerUpSpawner : IPowerUpSpawner
{
    private readonly PowerUpTable _powerUpTable;
    private readonly PowerUpFactory _factory;
    private readonly IRandom _random;

    public PowerUpSpawner(PowerUpTable powerUpTable, PowerUpFactory factory, IRandom random)
    {
        _powerUpTable = powerUpTable;
        _factory = factory;
        _random = random;
    }

    public void SpawnPowerUp(Vector3 position)
    {
        PowerUpConfig config = GetRandomPowerUpConfig();
        if (config != null)
        {
            _factory.Create(config, position);
        }
    }

    private PowerUpConfig GetRandomPowerUpConfig()
    {
        int totalWeight = _powerUpTable.Configs.Sum(item => item.Weight);
        int randomValue = _random.Next(0, totalWeight);
        int cumulative = 0;

        foreach (var item in _powerUpTable.Configs)
        {
            cumulative += item.Weight;
            if (randomValue < cumulative)
            {
                return item.Name == "None" ? null : item;
            }
        }

        return null;
    }
}

public sealed class ExtraLifePowerUpFactory : PowerUpFactory
{
    private readonly Game2 _game;

    public ExtraLifePowerUpFactory(Game2 game)
    {
        _game = game;
    }

    public PowerUpPresenter Create(ExtraLifePowerUp prefab, Vector3 position)
    {
        var instance = Object.Instantiate(prefab, position, Quaternion.identity);
        instance.Inject(_game);
        return instance;
    }
}

public sealed class ExtraBallPowerUpFactory : PowerUpFactory
{
    private readonly BallManager _ballManager;

    public ExtraBallPowerUpFactory(BallManager ballManager)
    {
        _ballManager = ballManager;
    }

    public PowerUpPresenter Create(ExtraBallPowerUp prefab, Vector3 position)
    {
        var instance = Object.Instantiate(prefab, position, Quaternion.identity);
        instance.Inject(_ballManager);
        return instance;
    }
}
