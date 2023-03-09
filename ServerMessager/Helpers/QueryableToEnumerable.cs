using ServerMessager.Models.Entitys;

namespace ServerMessager.Helpers
{
    public static class QueryableToEnumerable<T> where T : Entity
    {
        public static IEnumerable<T> Convert(IQueryable<T> values)
        {
            try
            {
                IEnumerable<T> result = new List<T>();
                foreach (var item in values)
                {
                    result = result.Append(item);
                }
                return result;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
