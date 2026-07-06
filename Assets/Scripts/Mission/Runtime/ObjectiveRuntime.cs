using System;

public abstract class ObjectiveRuntime
{
    public ObjectiveDataSO Data { get; }
    public bool IsCompleted { get; private set; }
    public int CurrentProgress { get; protected set; }
    public int TargetProgress { get; protected set; }

    public event EventHandler<ObjectiveRuntime> Completed;
    public event EventHandler<ObjectiveRuntime> ProgressChanged;

    public ObjectiveRuntime(ObjectiveDataSO data)
    {
        Data = data;
    }

    protected void Complete()
    {
        if (IsCompleted)
            return;

        IsCompleted = true;

        Completed?.Invoke(this, this);
    }

    protected void SetProgress(int value)
    {
        CurrentProgress = value;
        ProgressChanged?.Invoke(this, this);
    }

    public abstract void Register();

    public abstract void Unregister();
}