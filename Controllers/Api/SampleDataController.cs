using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Granify.Models;
using Granify.Api.DataAccess;

namespace Granify.Api.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly ItemRepo _itemRepo;
        public SampleDataController(ItemRepo itemRepo){
            _itemRepo = itemRepo;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Item>> Get()
        {
            return await _itemRepo.GetItemsAsync();
        }
    }
}
