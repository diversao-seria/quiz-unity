public class Match
{
    private int _quiz_id;         /* Referência; */
    private int _player_id;       /* Referência; */
    private int correctAnswers;
    /* Stopwatch watch; */

    public Match(int _quiz_id, int _player_id)
    {
        this._quiz_id = _quiz_id;
        this._player_id = _player_id;
    }
}
