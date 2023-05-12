using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using customer.Data;
using customer.Models;

namespace customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OffersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Offers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offer>>> GetProducts()
        {
          if (_context.offer == null)
          {
              return NotFound();
          }
            return await _context.offer.ToListAsync();
        }

        // GET: api/Offers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Offer>> GetOffer(int id)
        {
          if (_context.offer == null)
          {
              return NotFound();
          }
            var offer = await _context.offer.FindAsync(id);

            if (offer == null)
            {
                return NotFound();
            }

            return offer;
        }
       
    }
}
