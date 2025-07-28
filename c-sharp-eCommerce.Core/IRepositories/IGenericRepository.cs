namespace c_sharp_eCommerce.Core.IRepositories
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        public Task<(IEnumerable<TModel>, int count)> GetAll(int page,int limit, string[]? includedProperties = null);
        public Task<TModel?> GetById(int id);
        public Task Create(TModel model);
        public Task Update(object pk, Action<TModel> updateResource);
        public Task<bool> Delete(int id);
    }
}
