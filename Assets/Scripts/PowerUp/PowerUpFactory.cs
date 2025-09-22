using BreakoutGame;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PowerUpFactory
{
    public abstract PowerUpPresenter Create(Vector3 position);
}

[System.Serializable]
public class PowerUpConfig
{
    public string Name;
    public PowerUpPresenter Prefab;
    public int Weight;
    public PowerUpFactory Factory;
}

public class PowerUpSpawner : ScriptableObject
{
    public List<PowerUpConfig> _powerUpTable;

    public void SpawnPowerUp()
    {
        PowerUpConfig config = GetRandomPowerUpConfig();
        if (config != null && config.Prefab != null)
        {
            //Instantiate(selectedLoot.Prefab, transform.position, Quaternion.identity);
            config.Factory.Create(Vector3.zero);
        }
    }

    PowerUpConfig GetRandomPowerUpConfig()
    {
        int totalWeight = _powerUpTable.Sum(item => item.Weight);
        int randomValue = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var item in _powerUpTable)
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
