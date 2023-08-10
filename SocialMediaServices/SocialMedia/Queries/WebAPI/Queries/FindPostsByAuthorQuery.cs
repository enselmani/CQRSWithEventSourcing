using SharedKernel.Queries;

namespace WebAPI.Queries
{
    public class FindPostsByAuthorQuery : BaseQuery
    {
        public string Author { get; set; }
    }
}