public interface IWinCondition
{
    bool CheckWinCondition();
    bool CheckLoseCondition();
    event System.Action OnWin;
    event System.Action OnLose;
}