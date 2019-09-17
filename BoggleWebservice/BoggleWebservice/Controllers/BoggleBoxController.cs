using System;
using BoggleWebservice.Models;
using BoggleWebservice.Services;
using BoggleWebservice.Helpers;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Http.Cors;

namespace BoggleWebservice.Controllers
{   
    [EnableCors(origins: "http://localhost:4200,http://localhost:8080", headers: "*", methods: "*")]
    public class BoggleBoxController : ApiController
    {
        private readonly BoggleService _boggleService;
        private readonly WordValidator _wordValidator;

        public BoggleBoxController()
        {
            _boggleService = new BoggleService();
            _wordValidator = new WordValidator();
        }

        [HttpGet]
        public BoggleBox GetBoggleBox()
        {
            return _boggleService.CreateBoggleBox();
        }

        [HttpGet]
        [Route("api/BoggleBox/GetBoggleBox/{boggleBoxId}")]
        public BoggleBox GetBoggleBox(string boggleBoxId)
        {
            return _boggleService.GetBoggleBox(Guid.Parse(boggleBoxId)); 
        }

        [HttpGet]
        [Route("api/BoggleBox/IsValidWord/{boggleBoxId}/{word}")]
        public bool IsValidWord(string boggleBoxId, string word)
        {            
            return _boggleService.IsValidWord(Guid.Parse(boggleBoxId), word);
        }

        [HttpGet]
        [Route("api/BoggleBox/ScoreWord/{word}")]
        public int ScoreWord(string word)
        {
            return _wordValidator.GetPoints(word);
        }

        [HttpPost]
        //[Route("api/BoggleBox/CreateBoggleGame")]
        public Game CreateBoggleGame(Game game)
        {           
            _boggleService.CreateGame(game);

            return game;
        }

        [HttpGet]
        public List<PlayerScore> GetHighscores()
        {
            return _boggleService.GetHighscores();
        }






        [HttpGet]
        public bool[] TestIsWordValid()
        {
            var letters = "abcdefghijklmnop";

            var list = letters.ConvertLettersToDieList();

            var matrix = list.ConvertListToMatrix();

            var words = new[] { "hoi", "doei", "middag", "skrr", "huisx", "laptop", "Xylofoon", "csharp" };
            var results = new bool[words.Length];
            for (var i = 0; i < words.Length; i++)
            {
                if (_wordValidator.IsWordValid(words[i]))
                    results[i] = true;
                else
                    results[i] = false;
            }

            return results;
        }
        
        [HttpGet]
        public bool TestContainsWord()
        {
            var testBoardLetters = "TSKUITIROEYCTCSC";
            var result = testBoardLetters.ContainsWord("TIRCYS");
            var testWords = new[] { "SKITT", "TSKURU", "TSKUR", "TIYET", "URS", "STES" };
            //true, false,  true    ,  true,   false , true 
            var results = new bool[testWords.Length];

            for (var i = 0; i < testWords.Length; i++)
            {
                var word = testWords[i];
                results[i] = testBoardLetters.ContainsWord(word);
            }

            return result;
        }

        [HttpGet]
        public int[] TestScoreWord()
        {
            var words = new[] { "a", "aa", "aaa", "aaaa", "aaaaa", "aaaaaa", "aaaaaaa", "aaaaaaaa", "aaaaaaaaa", "aaaaaaaaaa" };
            var results = new int[words.Length];
            for (var i = 0; i < words.Length; i++)
            {
                results[i] = _wordValidator.GetPoints(words[i]);
            }

            return results;
        }
    }
}
