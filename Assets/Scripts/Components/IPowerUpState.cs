namespace BreakoutGame
{
    public interface IPowerUpState
    {
        string Name { get; }
        void Enter();
        void Exit();
    }
}
