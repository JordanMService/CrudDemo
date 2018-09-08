using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Granify.Models;
using Granify.Api.DataAccess;
using Microsoft.AspNetCore.Http;

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

        [HttpPost("[action]")]
        public async Task<IActionResult> Post([FromBody]Item itemToPost){
            try{
                await _itemRepo.PostItemAsync(itemToPost);
                return Ok();
            }
            catch(ArgumentException ex){
                return BadRequest(ex);
            }
            catch(Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError,ex);
            }
        }
    }
}
