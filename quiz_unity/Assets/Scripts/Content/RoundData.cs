[System.Serializable]
public class RoundData
{
	public string FilePath { get; set; }
	public string FolderPath { get; set; }
	public string QuizCode { get; set; }
	public int CurrentPoints { get; set; }

	public QuestionData[] Questions { get; set; }
}