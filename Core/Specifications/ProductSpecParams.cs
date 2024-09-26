namespace Core.Specifications;

public class ProductSpecParams
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;

    private int pageSize = 6;

    public int PageSize
    {
        get { return pageSize; }
        set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
    }


    private List<string> _types = [];

    public List<string> Types
    {
        get { return _types; }
        set { 
            _types = value.SelectMany(x => x.Split(',' ,
                StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }
    private List<string> _brand = [];

    public List<string> Brand
    {
        get { return _brand; }
        set
        {
            _brand = value.SelectMany(x => x.Split(',',
                StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public string? Sort { get; set; }
}
