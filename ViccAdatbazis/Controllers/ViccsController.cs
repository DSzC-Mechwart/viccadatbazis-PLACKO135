using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;

namespace ViccAdatbazis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViccController : ControllerBase
    {
        //Adatbázis kapcsolat
        private readonly ViccDbContext _context;

        public ViccController(ViccDbContext context)
        {
            _context = context;
        }

        //Összes vicc lekérdezése
        [HttpGet]
      
        //Aszinkron módon
        public async Task<ActionResult<List<Vicc>>> GetViccek()
        {
            return await _context.Viccek.Where(x => x.Aktiv == true).ToListAsync();
        }

        //Egy vicc lekérdezése
        [HttpGet("{id}")]
        public async Task<ActionResult<Vicc>> GetVicc(int id)
        {
            var vicc = await _context.Viccek.FindAsync(id);
            if (vicc == null)
            {
                return NotFound();
            }
            return vicc;
        }

        //Vicc feltöltése
        [HttpPost]
        public async Task<ActionResult<Vicc>> PostVicc(Vicc vicc)
        {
            _context.Viccek.Add(vicc);
            await _context.SaveChangesAsync();

   

            //A válasz maga a vicc
            return CreatedAtAction("GetVicc", new { id = vicc.Id }, vicc);
        }

        //Vicc módosítása
        [HttpPut("{id}")]
        public async Task<ActionResult> PutVicc(int id, Vicc vicc)
        {
            if (id != vicc.Id)
            {
                return BadRequest();
            }
            _context.Entry(vicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Vicc törlése
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVicc(int id)
        {
            var vicc = await _context.Viccek.FindAsync(id);
            if (vicc == null)
            {
                return NotFound();
            }
            if (vicc.Aktiv == true)
            {
                vicc.Aktiv = false;
                _context.Entry(vicc).State = EntityState.Modified;
            }
            else
            {
                _context.Viccek.Remove(vicc);
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Lájkolás
        [Route("{id}/like")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<string>> Tetszik(int id)
        {
            var vicc = _context.Viccek.Find(id);
            if (vicc == null) { return NotFound(); }
            vicc.Tetszik++;
            _context.Entry(vicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var valasz = new
            {
                tdb = vicc.Tetszik
            };
            return Ok(JsonSerializer.Serialize(valasz));
        }

        //DisLike
        [Route("{id}/dislike")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<string>> NemTetszik(int id)
        {
            var vicc = _context.Viccek.Find(id);
            if (vicc == null) { return NotFound(); }
            vicc.NemTetszik++;
            _context.Entry(vicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var valasz = new
            {
                tdb = vicc.NemTetszik
            };
            return Ok(JsonSerializer.Serialize(valasz));
        }
    }
}