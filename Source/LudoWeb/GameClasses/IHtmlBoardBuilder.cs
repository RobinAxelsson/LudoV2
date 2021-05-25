using LudoWeb.ViewModel;

namespace LudoWeb.GameClasses
{
    public interface IHtmlBoardBuilder
    {
        int XCount { get; }
        int YCount { get; }
        GameSquareViewModel GetGameSquare(int x, int y);
    }
}