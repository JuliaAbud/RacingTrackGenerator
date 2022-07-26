using UnityEngine;

public class RunsCounter : MonoBehaviour
{
    [Header("Keeping track of the accepted ratio")]


    [SerializeField] private int _runs = 0;
    [SerializeField] private int _goodRuns = 0;
    [SerializeField] private int _badRuns = 0;


    public int Runs 
    {
        get => _runs;
        set => _runs = value;
    }
    public int GoodRuns
    {
        get => _goodRuns;
        set => _goodRuns = value;
    }
    public int BadRuns
    {
        get => _badRuns;
        set => _badRuns = value;
    }

    public void AddOneRun()
    {
        Runs += 1;
    }

    public void AddOneGoodRun()
    {
        GoodRuns += 1;
    }

    public void AddOneBadRun()
    {
        BadRuns += 1;
    }

    private void Start()
    {
        Runs = 0;
        GoodRuns = 0;
        BadRuns = 0;
    }
}
