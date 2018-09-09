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
            return (await _itemRepo.GetRowsAsync()).Select(r => r.Item);
        }

        /// <summary>
        /// Get item by its 10 character Id
        /// </summary>
        /// <param name="id">10 character assigned Id</param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(string id){
            try{
                var row = await _itemRepo.GetRowById(id) ;
                return Ok(row.Item);
            }
            catch(KeyNotFoundException ex){
                return NotFound();
            }
            catch(Exception ex){
                  return StatusCode(StatusCodes.Status500InternalServerError,ex);
            }
        }

        /// <summary>
        /// Post new items to server
        /// </summary>
        /// <param name="itemToPost"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete Item
        /// </summary>
        /// <param name="id">10 character assigned Id</param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Delete(string id){
            try{
                await _itemRepo.DeleteItemAsync(id);
                return Ok();
            }
            catch(KeyNotFoundException ex){
                return NotFound();
            }
            catch(Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError,ex);
            }
        }
    }
}
