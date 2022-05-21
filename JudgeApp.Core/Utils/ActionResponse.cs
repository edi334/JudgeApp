namespace JudgeApp.Core.Utils;

public class ActionResponse
{
    public List<string> Errors { get; set; }
    public string Action { get; set; }
    

    public ActionResponse()
    {
        Errors = new List<string>();
    }

    public ActionResponse(List<string> errors)
    {
        Errors = errors;
    }

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    public bool HasErrors()
    {
        return Errors.Count > 0;
    }

    public string GetErrors()
    {
        var errors = "";

        foreach (var e in Errors) errors += e;

        return errors;
    }
}

public class ActionResponse<T> : ActionResponse
{
    public T Item { get; set; }
    
    public ActionResponse()
    {
        
    }

    public ActionResponse(string action) : this()
    {
        Action = action;
    }

    public ActionResponse(T item) : this()
    {
        Item = item;
    }

    public ActionResponse(string action, T item) : this()
    {
        Action = action;
        Item = item;
    }
}