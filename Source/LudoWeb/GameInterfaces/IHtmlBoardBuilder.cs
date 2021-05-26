using LudoWeb.ViewModel;

namespace LudoWeb.GameInterfaces
{
    public interface IHtmlBoardBuilder
    {
        int XCount { get; }
        int YCount { get; }
        GameSquareViewModel GetGameSquare(int x, int y);
    }
}