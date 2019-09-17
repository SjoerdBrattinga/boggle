using BoggleWebservice.Models;
using System;
using System.Collections.Generic;
using BoggleWebservice.Helpers;
using System.Linq;

namespace BoggleWebservice.Services
{
    public class BoggleService
    {
        private readonly WordValidator _wordValidator;

        public BoggleService()
        {
            _wordValidator = new WordValidator();
        }

        public BoggleBox CreateBoggleBox()
        {
            var boggleBox = new BoggleBox
            {
                BoggleBoxId = Guid.NewGuid(),
                Dice = GetDice()
            };

            CreateBoggleBoxInDb(boggleBox);

            return boggleBox;
        }

        public BoggleBox GetBoggleBox(Guid boggleBoxId)
        {
            var board = GetBoardById(boggleBoxId);

            var boggleBox = new BoggleBox
            {
                BoggleBoxId = board.BoggleBoxId,
                Dice = board.Letters.ConvertLettersToDice()
            };

            return boggleBox;
        }

        private List<List<Die>> GetDice()
        {
            var dice = new List<Die>();
            dice.Roll();
            dice.Shuffle();

            return dice.ConvertListToMatrix();
        }

        public void CreateGame(Game game)
        {            
            var boggleGame = new BoggleGame()
            {
                Name = game.Name,
                Score = game.Score,
                fk_BoggleBoxId = Guid.Parse(game.BoggleBoxId) 
            };
            using (var context = new BoggleDbContext())
            {
                context.BoggleGame.Add(boggleGame);
                context.SaveChanges();
            }
        }

        public List<PlayerScore> GetHighscores()
        {
            using (var context = new BoggleDbContext())
            {
                var playerScores = (from game in context.BoggleGame
                                    select new PlayerScore()
                                    {
                                        PlayerName = game.Name,
                                        Score = game.Score,
                                        BoggleBoxId = game.fk_BoggleBoxId
                                    }).ToList();

                var highscores = playerScores.OrderByDescending(p => p.Score).Take(15).ToList();                

                return highscores;               
            }
        }

        private Board GetBoardById(Guid boggleBoxId)
        {
            using (var context = new BoggleDbContext())
            {
                return (from board in context.Board
                        where board.BoggleBoxId == boggleBoxId
                        select board).FirstOrDefault();
            }
        }

        private void CreateBoggleBoxInDb(BoggleBox boggleBox)
        {
            var board = new Board()
            {
                BoggleBoxId = boggleBox.BoggleBoxId,
                Letters = boggleBox.Dice.GetLetters()
            };

            using (var context = new BoggleDbContext())
            {
                // Checks if board already exist
                if (context.Board.Count(b => b.BoggleBoxId == boggleBox.BoggleBoxId) != 0) return;

                try
                {
                    context.Board.Add(board);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public bool IsValidWord(Guid boggleBoxId, string word) 
        {
            var board = GetBoardById(boggleBoxId);
            //return _wordValidator.IsWordValid(word.ToLower());
            return _wordValidator.IsWordValid(word.ToLower()) && board.Letters.ContainsWord(word); 
        }
    }
}