public class Logs
{
    private string logsName;
    private string[] logsLevels;

    static public int LOG_LEVEL = 0;

    public Logs(string logsName)
    {
        this.logsName = logsName;
        logsLevels = new string[4];

        logsLevels[0] = "DEBUG";
        logsLevels[1] = "INFO";
        logsLevels[2] = "WARN";
        logsLevels[3] = "ERROR";
    }

    private void Message(string message, int level)
    {
        if (level >= LOG_LEVEL)
        {
            UnityEngine.Debug.Log($"[{logsLevels[level]}][{logsName}] {message}");
        }
    }

    public void Debug(string message)
    {
        int level = 0;
        Message(message, level);
    }

    public void Info(string message)
    {
        int level = 1;
        Message(message, level);
    }

    public void Warn(string message)
    {
        int level = 2;
        Message(message, level);
    }

    public void Error(string message)
    {
        int level = 3;
        Message(message, level);
    }
}
