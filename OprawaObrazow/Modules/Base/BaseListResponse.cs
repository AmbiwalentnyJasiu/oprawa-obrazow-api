namespace OprawaObrazow.Modules.Base;

public class BaseListResponse<TListModel>
where TListModel : class
{
    public int Count { get; set; }
    public IEnumerable<TListModel> Items { get; set; } = null!;
}