using System;

public class CheatSystemCommandBase
{
    private string _cheatID;
    private string _cheatDescription;
    private string _cheatFormat;

    public string cheatID { get { return _cheatID; } }
    public string cheatDescription { get { return _cheatDescription; } }
    public string cheatFormat { get { return _cheatFormat; } }

    public CheatSystemCommandBase(string id, string description, string format)
    {
        _cheatID = id;
        _cheatDescription = description;
        _cheatFormat = format;
    }
}

public class CheatSystemCommand : CheatSystemCommandBase
{
    private Action cheat;

    public CheatSystemCommand(string id, string description, string format, Action cheat) : base(id, description, format)
    {
        this.cheat = cheat;
    }

    public void Invoke()
    {
        cheat.Invoke();
    }
}