using Microsoft.EntityFrameworkCore;

namespace Fiorello.ViewModels;
public class PaginationVM<T> : List<T>
{
    public PaginationVM(List<T> items, int currentPage, int totalPage)
    {
        CurrentPage = currentPage;
        TotalPage = totalPage;
        this.AddRange(items);

        int range = 5;
        int halfRange = range / 2;

        int start = currentPage - halfRange;
        int end = currentPage + halfRange;

        if (start <= 0)
        {
            end -= (start - 1);
            start = 1;
        }

        if (end > TotalPage)
        {
            end = TotalPage;
            if (end - range + 1 > 0)
            {
                start = end - range + 1;
            }
            else
            {
                start = 1;
            }
        }

        Start = start;
        End = end;
    }

    public int CurrentPage { get; }
    public int TotalPage { get; }
    public bool HasNext => CurrentPage < TotalPage;
    public bool HasPrev => CurrentPage > 1;
    public int Start { get; }
    public int End { get; }

    public static async Task<PaginationVM<T>> CreateVM(IQueryable<T> query, int stage, int take)
    {
        if (stage <= 0)
        {
            stage = 1;
        }
        var totalItemCount = await query.CountAsync();
        var data = await query
            .Skip((stage - 1) * take)
            .Take(take)
            .ToListAsync();
        var totalPage = (int)Math.Ceiling((decimal)totalItemCount / take);
        if (stage > totalPage)
        {
            stage = totalPage;
        }
        return new PaginationVM<T>(data, stage, totalPage);
    }
}

//pagination
