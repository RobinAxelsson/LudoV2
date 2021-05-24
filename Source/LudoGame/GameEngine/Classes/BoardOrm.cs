using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LudoGame.GameEngine.GameSquares;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class BoardOrm : IBoardOrm
    {
        public List<GameSquare> Map()
        {
            var boardLines = BoardReadEmbedded().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return map(boardLines);
        }
        public List<GameSquare> Map(string filePath)
        {
            var boardLines = File.ReadAllLines(filePath);
            return map(boardLines);
        }
        private List<GameSquare> map(string[] boardLines)
        {
            var squares = new List<GameSquare>();
            var charCoords = ReadCharCoords(boardLines);

            foreach (var charCoord in charCoords)
            {
                var chr = charCoord.chr;
                int x = charCoord.X;
                int y = charCoord.Y;

                GameSquare newSquare =
                    chr == '0' ? new StandardSquare(x, y, GameEnum.BoardDirection.Up) :
                    chr == '1' ? new StandardSquare(x, y, GameEnum.BoardDirection.Right) :
                    chr == '2' ? new StandardSquare(x, y, GameEnum.BoardDirection.Down) :
                    chr == '3' ? new StandardSquare(x, y, GameEnum.BoardDirection.Left) :

                    chr == 'a' ? new SafezoneSquare(x, y, GameEnum.TeamColor.Red, GameEnum.BoardDirection.Down) :
                    chr == 'b' ? new SafezoneSquare(x, y, GameEnum.TeamColor.Blue, GameEnum.BoardDirection.Right) :
                    chr == 'c' ? new SafezoneSquare(x, y, GameEnum.TeamColor.Yellow, GameEnum.BoardDirection.Up) :
                    chr == 'd' ? new SafezoneSquare(x, y, GameEnum.TeamColor.Green, GameEnum.BoardDirection.Left) :

                    chr == 'e' ? new ExitSquare(x, y, GameEnum.TeamColor.Red, GameEnum.BoardDirection.Right) :
                    chr == 'f' ? new ExitSquare(x, y, GameEnum.TeamColor.Blue, GameEnum.BoardDirection.Up) :
                    chr == 'g' ? new ExitSquare(x, y, GameEnum.TeamColor.Yellow, GameEnum.BoardDirection.Left) :
                    chr == 'h' ? new ExitSquare(x, y, GameEnum.TeamColor.Green, GameEnum.BoardDirection.Down) :

                    chr == 'r' ? new StartSquare(x, y, GameEnum.TeamColor.Red, GameEnum.BoardDirection.Down) :
                    chr == 'l' ? new StartSquare(x, y, GameEnum.TeamColor.Blue, GameEnum.BoardDirection.Right) :
                    chr == 'y' ? new StartSquare(x, y, GameEnum.TeamColor.Yellow, GameEnum.BoardDirection.Up) :
                    chr == 'n' ? new StartSquare(x, y, GameEnum.TeamColor.Green, GameEnum.BoardDirection.Left) :

                    chr == '4' ? new BaseSquare(x, y, GameEnum.TeamColor.Red, GameEnum.BoardDirection.Left) :
                    chr == '5' ? new BaseSquare(x, y, GameEnum.TeamColor.Blue, GameEnum.BoardDirection.Down) :
                    chr == '6' ? new BaseSquare(x, y, GameEnum.TeamColor.Yellow, GameEnum.BoardDirection.Right) :
                    chr == '7' ? new BaseSquare(x, y, GameEnum.TeamColor.Green, GameEnum.BoardDirection.Up) :

                    chr == 'w' ? new WinnerSquare(x, y) :
                    chr == 's' ? new GoalSquare(x, y) : throw new NullReferenceException();

                squares.Add(newSquare);
            }

            return squares;
        }

        //The file is embedded so all projects can use it with the library
        private string BoardReadEmbedded()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LudoGame.GameEngine.Data.BoardMap.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        private List<(char chr, int X, int Y)> ReadCharCoords(string[] lines)
        {

            var charCoord = new List<(char chr, int X, int Y)>();

            int x = 0;
            int y = 0;
            foreach (var line in lines)
            {
                if (line[0] == '/') break;
                foreach (char chr in line)
                {
                    if (chr != ' ')
                        charCoord.Add((chr, x, y));
                    x++;
                }
                y++;
                x = 0;
            }
            return charCoord;
        }
        
    }
}
