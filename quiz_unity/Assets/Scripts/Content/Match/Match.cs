public class Match
{
    private int _quiz_id;         /* Referência; */
    private int _player_id;       /* Referência; */

    public int Id { get; set; }
    private int Score { get; set; }
    /* Stopwatch watch; */

    public Match(int _quiz_id, int _player_id)
    {
        this._quiz_id = _quiz_id;
        this._player_id = _player_id;
    }
}
