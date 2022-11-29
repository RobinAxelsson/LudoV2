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

                if (chr == '/') break;
                
                int x = charCoord.X;
                int y = charCoord.Y;

                var newSquare = MapGameSquare(chr, x, y);
                
                squares.Add(newSquare);
            }

            return squares;
        }

        private static GameSquare MapGameSquare(char chr, int x, int y)
        {
            return chr switch
            {
                '0' => new SquareStandard(x, y, GameEnum.BoardDirection.Up),
                '1' => new SquareStandard(x, y, GameEnum.BoardDirection.Right),
                '2' => new SquareStandard(x, y, GameEnum.BoardDirection.Down),
                '3' => new SquareStandard(x, y, GameEnum.BoardDirection.Left),

                'a' => new SquareSafeZone(x, y, GameEnum.TeamColor.Red, GameEnum.BoardDirection.Down),
                'b' => new SquareSafeZone(x, y, GameEnum.TeamColor.Blue, GameEnum.BoardDirection.Right),
                'c' => new SquareSafeZone(x, y, GameEnum.TeamColor.Yellow, GameEnum.BoardDirection.Up),
                'd' => new SquareSafeZone(x, y, GameEnum.TeamColor.Green, GameEnum.BoardDirection.Left),

                'e' => new SquareExit(x, y, GameEnum.TeamColor.Red, GameEnum.BoardDirection.Right),
                'f' => new SquareExit(x, y, GameEnum.TeamColor.Blue, GameEnum.BoardDirection.Up),
                'g' => new SquareExit(x, y, GameEnum.TeamColor.Yellow, GameEnum.BoardDirection.Left),
                'h' => new SquareExit(x, y, GameEnum.TeamColor.Green, GameEnum.BoardDirection.Down),

                'r' => new SquareStart(x, y, GameEnum.TeamColor.Red, GameEnum.BoardDirection.Down),
                'l' => new SquareStart(x, y, GameEnum.TeamColor.Blue, GameEnum.BoardDirection.Right),
                'y' => new SquareStart(x, y, GameEnum.TeamColor.Yellow, GameEnum.BoardDirection.Up),
                'n' => new SquareStart(x, y, GameEnum.TeamColor.Green, GameEnum.BoardDirection.Left),

                '4' => new SquareTeamBase(x, y, GameEnum.TeamColor.Red, GameEnum.BoardDirection.Left),
                '5' => new SquareTeamBase(x, y, GameEnum.TeamColor.Blue, GameEnum.BoardDirection.Down),
                '6' => new SquareTeamBase(x, y, GameEnum.TeamColor.Yellow, GameEnum.BoardDirection.Right),
                '7' => new SquareTeamBase(x, y, GameEnum.TeamColor.Green, GameEnum.BoardDirection.Up),

                's' => new SquareGoal(x, y),
                'w' => new SquareWinner(x,y),

                _ => throw new NullReferenceException()
            };
        }

        //The file is embedded so all projects can use it with the library
        private string BoardReadEmbedded()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LudoGame.GameEngine.Data.BoardMap.txt";

            Stream stream = null;
            StreamReader streamReader = null;
            string fileContent = null;
            try
            {
                stream = assembly.GetManifestResourceStream(resourceName) ??
                         throw new ArgumentNullException("File path did not get resource stream");
                streamReader = new StreamReader(stream);
                fileContent = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                streamReader?.Dispose();
                stream?.Dispose();
            }

            return fileContent;
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
