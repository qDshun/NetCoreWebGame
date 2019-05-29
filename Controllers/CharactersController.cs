using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gameModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace testcore
{
    [Authorize]
    public class CharactersController : Controller
    {
        private readonly gameModel.gameModel _context = new gameModel.gameModel();
        private readonly User _currentUser;
        private readonly HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        const int _mapTableSize = 11;
        const int _mapSize = 100;

        public CharactersController()
        {

            var query1 = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault();
            string login = query1.Value;
            _currentUser = _context.Users.FirstOrDefault(u => u.Login == login);
            _context = new gameModel.gameModel();
        }
        #region initializingInfo
        public IActionResult InitializeZone()
        {
            Random rnd = new Random();
            for  (int i=0; i<_mapSize; i++)
                for (int j=0; j<_mapSize; j++)
                {
                    var zone = new gameModel.Zone
                    {
                        XPos = i,
                        YPos = j,
                        Type = rnd.Next(0, 3),

                    };
                    _context.Zones.Add(zone);
                }
            _context.SaveChangesAsync();
            return View("Done!");
        }
        #endregion
        public IActionResult Map()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetPlayerPosition(int character_id)
        {
            var result = new
            {
               xpos = 1,
               ypos = 1
            };

            return Json(result);
        }
        [HttpGet]
        public IActionResult GetMapTable(int xpos, int ypos)
        {
            int first_XPos = xpos;
            int first_YPos = ypos;
            int halfTable = _mapTableSize / 2;
            if (xpos < halfTable)
                first_XPos = 0;
            if (ypos < halfTable)
                first_YPos = 0;
            if (xpos + halfTable*2+1 >= _mapSize)
                first_XPos = xpos - (_mapTableSize - (_mapSize - (xpos)));
            if (ypos + halfTable*2+1 >= _mapSize)
                first_YPos = ypos - (_mapTableSize - (_mapSize - (ypos)));
            var mapTableQuery = from z in _context.Zones
                                where (z.XPos >= first_XPos && z.YPos >= first_YPos
                                    && z.XPos < first_XPos + _mapTableSize && z.YPos < first_YPos + _mapTableSize)
                                select z;
            var mapTable = mapTableQuery.GroupBy(z => z.XPos).ToList();
            var ZoneList = new List<List<Zone>>();
            foreach (var row in mapTable)
            {
                ZoneList.Add(row.ToList());
            }
            return PartialView("MapTable", ZoneList);
        }
        //[HttpPost]
        public IActionResult Enter(int? id)
        {
            bool accessable = _context.Characters.FirstOrDefaultAsync(c => (c.CharacterId == id && c.UserId == _currentUser.UserId)).Result != null;
            if (!accessable)
            {
                return NotFound();
            }
            TempData["Character_id"] = id;
            return RedirectToAction("Index", "Game", id);
        }
        // GET: Characters
        public IActionResult Index(int ?id)
        {
            string username = HttpContext.User.Claims.FirstOrDefault().Value;
            id = _context.Users.FirstOrDefaultAsync(u => u.Login == username).Result.UserId;
            if (id==null)
            {
                return NotFound();
            }
            else
            {
                var gameModel = _context.Characters.Include(c => c.UserId == id);
                var charactersQuery = from character in _context.Characters
                                      where character.UserId == id
                                      select character;
                return View(charactersQuery);
                //return View();
            }
        }
        //[Authorize]
        // GET: Characters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            id = 1;
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CharacterId == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }
        public IActionResult Delete()
        {
            return View();
        }
        // GET: Characters/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Characters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CharacterId,Nickname,Birthdate,UserId")] Character character)
        {
            if (ModelState.IsValid)
            {
                _context.Add(character);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", character.UserId);
            return View(character);
        }

        // GET: Characters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", character.UserId);
            return View(character);
        }

        // POST: Characters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CharacterId,Nickname,Birthdate,UserId")] Character character)
        {
            if (id != character.CharacterId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(character);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterExists(character.CharacterId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", character.UserId);
            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.CharacterId == id);
        }
    }
}
