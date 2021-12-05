using InventoryServiceExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace InventoryServiceExample.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly InventoryContext _context;
        public ProductsController(InventoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(bool? intstok,int? skip,int? take)
        {
            var products=_context.Products.AsQueryable();
            if (intstok !=null)//Kullanılabilirliği kontrol etme koşulu ekler
            {
                products = _context.Products.Where(i => i.AvailableQuantity > 0);
            }
            if (skip!=null)
            {
                products=products.Skip((int)skip);
            }
            if (take!=null)
            {
                products = products.Take((int)take);
            }
            return await _context.Products.ToListAsync();
        }
        //id gore veri getirme
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
            //return produtcs
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id,Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProducts", new { id = product.ProductId }, product);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var products=await _context.Products.FindAsync(id);
            if (products==null)
            {
                return NotFound();
            }
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();

            return products;
        }
        //gonderilen id ile product id ayni degil ise yukarda notfond olarak calisacak
        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e=>e.ProductId == id);
        }
    }
}
