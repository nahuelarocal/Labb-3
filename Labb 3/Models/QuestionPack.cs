namespace Labb_3.Models
{
    public class QuestionPack
    {
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public int SecondsPerQuestion { get; set; }

        public QuestionPack(string name, string difficulty, int secondsPerQuestion)
        {
            Name = name;
            Difficulty = difficulty;
            SecondsPerQuestion = secondsPerQuestion;
        }
    }
}
