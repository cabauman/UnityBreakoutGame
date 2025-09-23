namespace BreakoutGame
{
    public sealed class PowerUpData
    {
        public PowerUpConfig Config;
        public PowerUpAction Action;

        public PowerUpData(PowerUpConfig config, PowerUpAction action)
        {
            Config = config;
            Action = action;
        }
    }
}