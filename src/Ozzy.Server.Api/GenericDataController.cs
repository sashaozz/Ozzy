using Microsoft.AspNetCore.Mvc;
using Ozzy.DomainModel;
using System.Collections.Generic;
using System.Linq;

namespace Ozzy.Server.Api
{
    [Route("api/[controller]")]
    public class GenericDataController<TItem, TId> : Controller where TItem : class, IEntity<TId>
    {
        private IDataRepository<TItem, TId> _repository;

        public GenericDataController(IDataRepository<TItem, TId> repository)
        {
            _repository = repository;
        }

        // Get all items
        [HttpGet]
        public IEnumerable<TItem> Get()
        {
            var flags = _repository.Query().ToList();
            return flags;
        }

        // Get item by id
        [HttpGet("{id}")]
        public TItem Get(TId id)
        {
            return _repository.Query().Single(i => i.Id.Equals(id));
        }

        // Create item
        [HttpPost]
        public void Post([FromBody]TItem item)
        {
            _repository.Create(item);
        }

        // Update item
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]TItem item)
        {
            _repository.Update(item);
        }

        // Delete item
        [HttpDelete("{id}")]
        public void Delete(TId id)
        {
            _repository.Remove(id);
        }

    }
}
