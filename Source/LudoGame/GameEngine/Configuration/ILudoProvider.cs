namespace LudoGame.GameEngine.Configuration
{
    public interface ILudoProvider
    {
        T GetGameService<T>();
    }
}