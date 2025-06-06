namespace finrv.ApiService.Application;

public class Pagination<TMetadata, TList>
{
    public uint Page { get; set; }
    public uint PageSize { get; set; }
    public uint TotalPages { get; set; }
    public uint TotalRecords { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
    public PaginationData<TMetadata?, TList> Data { get; set; }

    public Pagination(List<TList> records, uint page, uint pageSize, uint totalRecords, TMetadata? metadata)
    {
        Page = page;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (uint)Math.Ceiling(totalRecords / (double)pageSize);
        HasNext = page < TotalPages;
        HasPrevious = page > 1;

        Data = new PaginationData<TMetadata?, TList>(metadata, records);
    }
}

public record PaginationData<TMetadata, TList>(TMetadata? Metadata, List<TList> Records);