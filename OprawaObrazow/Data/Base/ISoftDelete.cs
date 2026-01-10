namespace OprawaObrazow.Data.Base;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
    }
    
    public void UndoDelete()
    {
        IsDeleted = false;
    }
}