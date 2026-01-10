namespace OprawaObrazow.Modules.Base;

public class BaseFiltersDto
{
  public int Page { get; set; } = 1;
  public int PageSize { get; set; } = 20;

  public string Sort { get; set; } = "id";

  public string? Search { get; set; }
}