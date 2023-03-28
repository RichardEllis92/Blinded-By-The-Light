using System;

public class CheatSystemCommandBase
{
    public string CheatID { get; }
    public string CheatDescription { get; }
    public string CheatFormat { get; }

    protected CheatSystemCommandBase(string id, string description, string format)
    {
        CheatID = id;
        CheatDescription = description;
        CheatFormat = format;
    }
}

public class CheatSystemCommand : CheatSystemCommandBase
{
    private readonly Action _cheat;

    public CheatSystemCommand(string id, string description, string format, Action cheat) : base(id, description, format)
    {
        this._cheat = cheat;
    }

    public void Invoke()
    {
        _cheat.Invoke();
    }
}