public class GameInfo
{
    public int currentLevel = 0;
    public int playerHearts = 3;
    public int enemyKills = 0;
    public int currentScore = 0;

    public GameInfo()
    {
        currentLevel = 0;
        playerHearts = 3;
        enemyKills = 0;
        currentScore = 0;
    }

    public void NextLevel()
    {
        currentLevel++;
    }

    public void TakeLive()
    {
        playerHearts--;
    }

    public void AddKill()
    {
        enemyKills++;
    }

    public void UpdateScore()
    {
        currentScore = currentLevel * 100 + enemyKills * 10 + playerHearts * 10;
    }

}
