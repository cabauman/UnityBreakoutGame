namespace BreakoutGame
{
    [System.Serializable]
    public class PowerUpConfig
    {
        public PowerUpKind Kind;
        public PowerUp Prefab;
        public int Weight;
    }

    [System.Serializable]
    public class NonePowerUpConfig : PowerUpConfig
    {
    }

    [System.Serializable]
    public class ExtraLifePowerUpConfig : PowerUpConfig
    {
    }

    [System.Serializable]
    public class ExtraBallPowerUpConfig : PowerUpConfig
    {
    }

    [System.Serializable]
    public class EnlargePaddlePowerUpConfig : PowerUpConfig
    {
    }

    [System.Serializable]
    public class ShrinkPaddlePowerUpConfig : PowerUpConfig
    {
    }

    [System.Serializable]
    public class MagnetPowerUpConfig : PowerUpConfig
    {
    }

    [System.Serializable]
    public class ReverseBouncePowerUpConfig : PowerUpConfig
    {
    }
}