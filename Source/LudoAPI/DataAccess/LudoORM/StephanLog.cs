using System;
using System.IO;
using System.Linq;
using LudoAPI.GameEngine.Interfaces;

namespace LudoAPI.DataAccess.LudoORM
{
    public class StephanLog : ILog
    {
        private StreamWriter Logger;
        public StephanLog(ModelEnum.TeamColor color)
        {
            if (!Directory.Exists(Environment.CurrentDirectory + @"\StephanLogs")) Directory.CreateDirectory(Environment.CurrentDirectory + @"\StephanLogs");
            var number = new DirectoryInfo(Environment.CurrentDirectory + @"\StephanLogs").GetFiles().Count(finf => finf.Name.StartsWith($"stephan_{color.ToString()}") && finf.Extension == ".log");
            Logger = new StreamWriter($@"{Environment.CurrentDirectory}\StephanLogs\stephan_{color.ToString()}{number.ToString()}.log");
        }
        public void Log(string input)
        {
            Logger.Write(input);
            Logger.WriteLine("");
            Logger.Flush();
        }
    }
}