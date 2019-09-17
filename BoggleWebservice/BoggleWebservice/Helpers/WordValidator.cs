using System.IO;
using System.Web;

namespace BoggleWebservice.Helpers
{
    public class WordValidator
    {
        private string FilePath = HttpContext.Current.Server.MapPath("~/App_Data/lower(1).lst");        

        public bool IsWordValid(string word)
        {
            return word.Length >= 3 && IsDutchWord(word);
        }        

        private bool IsDutchWord(string word)
        {
            using (var sr = new StreamReader(FilePath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line == word)
                        return true;
                }

                return false;
            }
        }
        
        public int GetPoints(string word)
        {            
            var points = 0;

            if (word.Length < 3) return points;

            switch (word.Length)
            {
                case 3:
                    points = 1;
                    break;
                case 4:
                    points = 1;
                    break;
                case 5:
                    points = 2;
                    break;
                case 6:
                    points = 3;
                    break;
                case 7:
                    points = 5;
                    break;
                case 8:
                    points = 11;
                    break;                
                default: // word.Length > 8
                    points = 11;
                    break;
            }

            return points;
        }       
    }
}