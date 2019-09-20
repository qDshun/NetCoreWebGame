using gameModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreWebGame.Controllers
{
    public class GameController : Controller
    {
        private readonly gameModel.gameModel _context = new gameModel.gameModel();
        private readonly User _currentUser;
        private Character _currentCharacter;
        private CharacterInfo _currentCharacterInfo;
        private CharacterState _currentCharacterState;
        private readonly HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        const int _mapTableSize = 11;
        const int _mapSize = 40;
        const int _maxMonstersInTile = 4;
        public List<string> BattleLog = new List<string>();

        public IQueryable<Monster> ForestMonsterQuery;
        public IQueryable<Monster> MeadowMonsterQuery;
        public IQueryable<Monster> DesertMonsterQuery;
        public IQueryable<Monster> TownMonsterQuery;
        public IQueryable<Monster> MountainMonsterQuery;
        public class BattleObject
        {
            public BattleObject(Character character, ExistingMonster monster)
            {
                this.monster = monster;
                this.character = character;
            }
            public Character character;
            public ExistingMonster monster;
        }

        public class CharacterInfoObject
        {
            public CharacterInfoObject(Character character, List<ExistingItem> equipedItems)
            {
                this.character = character;
                this.equipedItems = equipedItems;
            }
            public Character character;
            public List<ExistingItem> equipedItems;

        }

        public class TownInfoObject
        {
            public TownInfoObject(List<Merchant> merchants, Zone zone, int subLocationId)
            {
                this.merchants = merchants;
                Enum.TryParse(subLocationId.ToString(), out subLocation);
                this.zone = zone;
            }
            public Zone zone;
            public List<Merchant> merchants;
            public SubLocation subLocation;
            public enum SubLocation
            {
                Center = 0,
                Market = 1,
                Tavern = 2

            }

        }
        public GameController()
        {
            _context = new gameModel.gameModel();
            string login = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
            _currentUser = _context.Users.FirstOrDefault(u => u.Login == login);
            ForestMonsterQuery = _context.Monsters.Where(m => m.InForest == true);
            TownMonsterQuery   = _context.Monsters.Where(m => m.InTown   == true);
            MeadowMonsterQuery = _context.Monsters.Where(m => m.InMeadow == true);
            DesertMonsterQuery = _context.Monsters.Where(m => m.InDesert == true);
            MountainMonsterQuery = _context.Monsters.Where(m=> false);
        }

        public IActionResult Index(int? id)
        {
            _currentCharacter = _context.Characters.FirstOrDefault(c => c.CharacterId == id);
            _currentCharacterInfo = _context.CharacterInfos.FirstOrDefault(c => c.CharacterId == id);
            _currentCharacterState = _context.CharacterStates.FirstOrDefault(c => c.CharacterId == id);
            InitCharacter(id);
            ViewBag.currentCharacterId = id;
            return View("Map", id);
        }
        public bool InitCharacter(int? id)
        {
            var curent_character_info = _context.CharacterInfos.FirstOrDefault(i => i.CharacterId == id);
            var curent_character_state = _context.CharacterStates.FirstOrDefault(i => i.CharacterId == id);
            if (curent_character_info == null)
            {
                CharacterInfo newInfo = new CharacterInfo
                {
                    CharacterId = (int)id,
                    Cha = 1,
                    Con = 1,
                    Str = 1,
                    Dex = 1,
                    Int = 1,
                    Exp = 0,
                    Level = 1,
                    Gold = 0,
                    StatPoints = 0,
                    Race = "Human",
                    Sex = "male"
                };
                _context.CharacterInfos.AddAsync(newInfo);
                _context.SaveChangesAsync();
            }
            if (curent_character_state == null)
            {
                curent_character_info = _context.CharacterInfos.FirstOrDefault(i => i.CharacterId == id);
                CharacterState newState = new CharacterState
                {
                    CharacterId = (int)id,
                    MaxHp = curent_character_info.Con * 10,
                    MaxMp = curent_character_info.Int * 10,
                    MaxStamina = curent_character_info.Dex * 5 + curent_character_info.Str * 5 + curent_character_info.Con * 5,
                    XPos = 5,
                    YPos = 5
                };
                newState.Hp = newState.MaxHp;
                newState.Mp = newState.MaxMp;
                newState.Stamina = newState.MaxStamina;
                int existing_item_id;
                if (_context.ExistingItems.Count() != 0)
                    existing_item_id = _context.ExistingItems.Max(i => i.ExistingItemId) + 1;
                else
                    existing_item_id = 1;
                var baseWeapon = new ExistingItem
                {

                    ExistingItemId = existing_item_id,
                    CharacterId = id,
                    ItemId = 2,
                    ItemTypeId = 2
                };
                _context.ExistingItems.AddAsync(baseWeapon);
                _context.CharacterStates.AddAsync(newState);
                _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public IActionResult Map()
        {
            return View();
        }

        public void ClickMapCellOnMapRedactor(int xpos, int ypos)
        {

            var currentZone = _context.Zones.FirstOrDefault(z => z.XPos == xpos && z.YPos == ypos);
            if (currentZone.Type < 4)
                currentZone.Type++;
            else
                currentZone.Type = 0;
            _context.SaveChanges();
        }
        [HttpGet]
        public JsonResult GetPlayerPosition(int id)
        {
            var result = new int[] { _context.CharacterStates.FirstOrDefault(c => c.CharacterId == id).XPos, _context.CharacterStates.FirstOrDefault(c => c.CharacterId == id).YPos }.ToList();
            return Json(result);
        }

        public JsonResult ChangePlayerPosition(int XPos, int YPos, int id)
        {
            _context.CharacterStates.FirstOrDefault(c => c.CharacterId == id).XPos = XPos;
            _context.CharacterStates.FirstOrDefault(c => c.CharacterId == id).YPos = YPos;
            if (_context.HasChanges())
            {
                _context.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        [HttpGet]
        public IActionResult GetMapTable(int xpos, int ypos)
        {
            int first_XPos = xpos;
            int first_YPos = ypos;
            int halfTable = _mapTableSize / 2;
            if (xpos < 0)
                first_XPos = 0;
            if (ypos < 0)
                first_YPos = 0;
            if (xpos + halfTable * 2 + 1 >= _mapSize)
                first_XPos = xpos - (_mapTableSize - (_mapSize - (xpos)));
            if (ypos + halfTable * 2 + 1 >= _mapSize)
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

        public IActionResult GetZoneInfo(int xpos, int ypos, int? id)
        {
            var currentZone = _context.Zones.FirstOrDefault(z => z.XPos == xpos && z.YPos == ypos);
            ViewBag.CharacterID = id;
            return PartialView("ZoneInfo", currentZone);
        }

        public IActionResult GetCharacterInfo(int? id, bool? full)
        {
            /*if (_context.ExistingMonsters.Count() == 0)
                RefillMapWithMonsters();*/
            var character = _context.Characters.Include(c => c.CharacterInfo).Include(c => c.CharacterState).Include(c=> c.ExistingItems).FirstOrDefault(c => c.CharacterId == id);
            var item = _context.ExistingItems.Include(c => c.Item).Include(c => c.ItemType);
            if (full != null)
                ViewBag.infoIsFull = full;
            ViewBag.StatPoints = character.CharacterInfo.StatPoints;
            return PartialView("CharacterStats", character);
        }

        public IActionResult SpendStatPoint (int id, int stat_id)
        {
            var character = _context.Characters.Include(c => c.CharacterInfo).Include(c => c.CharacterState).FirstOrDefault(c => c.CharacterId == id);

            character.CharacterInfo.StatPoints--;
            switch (stat_id)
            {
                case (1):
                    character.CharacterInfo.Str++;
                    break;
                case (2):
                    character.CharacterInfo.Dex++;
                    break;
                case (3):
                    character.CharacterInfo.Int++;
                    break;
                case (4):
                    character.CharacterInfo.Con++;
                    break;
                case (5):
                    character.CharacterInfo.Cha++;
                    break;
            }
            _context.SaveChangesAsync();
            return GetCharacterInfo(id, true);
        }

        public void HealCharacter(int id, int percentage)
        {
            _context.CharacterStates.FirstOrDefault(c => c.CharacterId == id).Hp += _context.CharacterStates.FirstOrDefault(c => c.CharacterId == id).MaxHp * percentage / 100;
            if (_context.CharacterStates.FirstOrDefault(c => c.CharacterId == id).Hp > _context.CharacterStates.FirstOrDefault(c => c.CharacterId == id).MaxHp)
                _context.CharacterStates.FirstOrDefault(c => c.CharacterId == id).Hp = _context.CharacterStates.FirstOrDefault(c => c.CharacterId == id).MaxHp;
            _context.SaveChangesAsync();
        }
        //start of batlle logic
        #region battle 



        #region Damage And Armor
        private int CalculateItemDamage(ExistingItem item)
        {
            int damage = 0;
            if (item != null)
            {
                damage += (int)(_context.Items.FirstOrDefault(i => i.ItemId == item.ItemId).Damage +
                _context.ItemTypes.FirstOrDefault(i => i.ItemTypeId == item.ItemTypeId).DamageModifier); //weapon dmg
            }
            return damage;
        }

        private int CalculateItemArmor(ExistingItem item)
        {
            int armor = 0;
            if (item != null)
            {
                armor += (int)(_context.Items.FirstOrDefault(i => i.ItemId == item.ItemId).Armor +
                _context.ItemTypes.FirstOrDefault(i => i.ItemTypeId == item.ItemTypeId).ArmorModifier);  //weapon armor
            }
            return armor;
        }
        private List<int> CalculateItemStats(ExistingItem item)
        {
            int STR = 0, DEX = 0, INT = 0, CON = 0, CHA = 0;
            if (item != null)
            {
                STR += (int)(_context.Items.FirstOrDefault(i => i.ItemId == item.ItemId).StrBonus +
                _context.ItemTypes.FirstOrDefault(i => i.ItemTypeId == item.ItemTypeId).StrModifier);  

                DEX += (int)(_context.Items.FirstOrDefault(i => i.ItemId == item.ItemId).DexBonus +
                _context.ItemTypes.FirstOrDefault(i => i.ItemTypeId == item.ItemTypeId).DexModifier);

                INT += (int)(_context.Items.FirstOrDefault(i => i.ItemId == item.ItemId).IntBonus +
                _context.ItemTypes.FirstOrDefault(i => i.ItemTypeId == item.ItemTypeId).IntModifier);

                CON += (int)(_context.Items.FirstOrDefault(i => i.ItemId == item.ItemId).ConBonus +
                _context.ItemTypes.FirstOrDefault(i => i.ItemTypeId == item.ItemTypeId).ConModifier);

                CHA += (int)(_context.Items.FirstOrDefault(i => i.ItemId == item.ItemId).ChaBonus +
                _context.ItemTypes.FirstOrDefault(i => i.ItemTypeId == item.ItemTypeId).ChaModifier);
            }
            List<int> stats = new List<int>
            {
                STR,
                DEX,
                INT,
                CON,
                CHA
            };
            return stats;
        }
        public List<int> CalculateDamageAndArmor(int id)
        {
            var character = _context.Characters.Include(c => c.CharacterInfo).Include(c => c.CharacterState).Include(c => c.ExistingItems).FirstOrDefault(c => c.CharacterId == id);
            int STR = 0;
            int DEX = 0;
            int INT = 0;
            int CON = 0;
            int CHA = 0;
            int damage = 0;
            int armor = 0;

            List<int> stats = new List<int> //initiate list for stats
            {
                STR,
                DEX,
                INT,
                CON,
                CHA
            };

            List<ExistingItem> equipedItems = new List<ExistingItem> //and for equiped items, ok if null
            {
                character.CharacterState.ExistingItem_EquipedWeaponId,
                character.CharacterState.ExistingItem_EquipedOffhandId,
                character.CharacterState.ExistingItem_EquipedArmorId,
                character.CharacterState.ExistingItem_EquipedAmuletId
            };

            foreach (var item in equipedItems)      //calculate stats
            {
                damage += CalculateItemDamage(item);
                armor += CalculateItemArmor(item);
                var itemStats = CalculateItemStats(item);
                for (int i=0; i<stats.Count; i++)
                {
                    stats[i] += itemStats[i];
                }
            }
            stats[0] += character.CharacterInfo.Str;
            stats[1] += character.CharacterInfo.Dex;
            stats[2] += character.CharacterInfo.Int;
            stats[3] += character.CharacterInfo.Con;
            stats[4] += character.CharacterInfo.Cha;
            List<int> result = new List<int>
            {
                damage,
                armor
            };
            foreach (int stat in stats)         
                result.Add(stat);
            return result;
        }
        public JsonResult GetCalculatedDmgAndArmor(int id)
        {
            return Json(CalculateDamageAndArmor(id));
        }
        #endregion



        public IActionResult StartBattle(int id, int monster_id)
        {
            var character = _context.Characters.Include(c => c.CharacterState).FirstOrDefault(c => c.CharacterId == id);
            var monster = _context.ExistingMonsters.Include(m => m.Monster).FirstOrDefault(m => m.ExistingMonsterId == monster_id);
            BattleObject battleObject = new BattleObject(character, monster);
            return View("Battle", battleObject);
        }

        public JsonResult isMonsterDead(int id, int monster_id)
        {
            if (_context.ExistingMonsters.Include(m => m.Monster).FirstOrDefault(m => m.ExistingMonsterId == monster_id).Monster.Hp < 
                _context.ExistingMonsters.Include(m => m.Monster).FirstOrDefault(m => m.ExistingMonsterId == monster_id).Damaged)
            {
                _context.Remove(_context.ExistingMonsters.FirstOrDefault(m => m.ExistingMonsterId == monster_id));
                return Json(true);
            }
            return Json(false);
        }
        public JsonResult WinBattle(int id, int monster_id)
        {
            var monsterid = _context.ExistingMonsters.FirstOrDefault(m => m.ExistingMonsterId == monster_id).MonsterId;
            var character = _context.Characters.Include(c => c.CharacterInfo).Include(c => c.CharacterState).FirstOrDefault(c => c.CharacterId == id);
            var exp = GainExp(id, monster_id);
            var loot = GetLoot(id, exp);
            string itemName = null;
            if (loot != -1)
            {
                ExistingItem lootItem = _context.ExistingItems.FirstOrDefault(i => i.ExistingItemId == loot);
                itemName = lootItem.ItemType.ItemTypeName + ' ' + lootItem.Item.Name;

            }
            return Json(new {exp, itemName}); 
        }

        public int GainExp(int id, int monster_id)
        {

            var monsterid = _context.ExistingMonsters.FirstOrDefault(m => m.ExistingMonsterId == monster_id).MonsterId;
            var character = _context.Characters.Include(c => c.CharacterInfo).Include(c => c.CharacterState).FirstOrDefault(c => c.CharacterId == id);
            var monster = _context.Monsters.FirstOrDefault(m => m.MonsterId == monsterid);
            int gainedExp = (monster.Int + monster.Str + monster.Dex + monster.Con + monster.Cha) * 5
                + (monster.Damage + monster.Armor) * 10 + monster.Hp;
            character.CharacterInfo.Exp += gainedExp;
            while (character.CharacterInfo.Exp >= character.CharacterInfo.Level * 100)
            {
                character.CharacterInfo.Exp -= (int)character.CharacterInfo.Level * 100;
                character.CharacterInfo.Level++;
                character.CharacterInfo.StatPoints++;
            }
            _context.SaveChangesAsync();
            return gainedExp;
        }

        public int GetLoot(int id, int exp)
        {
            var character = _context.Characters.Include(c => c.CharacterInfo).Include(c => c.CharacterState).FirstOrDefault(c => c.CharacterId == id);
            int result = -1; //loot id, -1 if not exists
            // from exp%100 - 1 to exp%100 + 1
            // generation of loot start
            int lower = exp / 100 - 1;
            int upper = exp / 100 + 1;
            Random random = new Random();
            int rarityLevel = random.Next(lower, upper + 1);

            if (_context.Items.Count() < rarityLevel)
                rarityLevel = _context.Items.Max(i => i.RarityLevel);
            var niceItems = (from item in _context.Items
                             where item.RarityLevel == rarityLevel
                             select item).ToList();
            if (rarityLevel > 0)
            {
                var lootItem = niceItems[(random.Next(0, niceItems.Count()))];
                bool nextIter = true;
                int itemTypeLevel = 1;
                while (nextIter) //modifier generation
                {
                    int rnd = random.Next(0, 2);
                    if (rnd == 0)
                        itemTypeLevel++;
                    else
                        break;
                }
                if (itemTypeLevel > _context.ItemTypes.Max(i => i.TypeRarityLevel))
                {
                    itemTypeLevel = _context.ItemTypes.Max(i => i.TypeRarityLevel);
                }
                var niceTypes = (from type in _context.ItemTypes
                                 where type.TypeRarityLevel == itemTypeLevel
                                 select type).ToList();
                var lootType = niceTypes[random.Next(0, niceTypes.Count)];
                var loot = new gameModel.ExistingItem
                {
                    ExistingItemId = _context.ExistingItems.Count() + 1,
                    ItemId = lootItem.ItemId,
                    ItemTypeId = lootType.ItemTypeId,
                    CharacterId = id
                };
                //generation of loot end
                _context.ExistingItems.Add(loot);
                if (loot != null)
                    result = loot.ExistingItemId;
            }
            _context.SaveChangesAsync();
            return result;
        }

        public void RefillMapWithMonsters()
        {
            _context.Database.ExecuteSqlCommandAsync("TRUNCATE `game`.`existing_monster`;");
            _context.SaveChangesAsync();
            int counter;
            if (_context.ExistingMonsters.Count() != 0)
                counter = _context.ExistingMonsters.Max(m => m.ExistingMonsterId) + 1;
            else
                counter = 1;
            foreach (var zone in _context.Zones)
            {
                TryGenerateMobInZone(zone, ref counter);
            }
            _context.SaveChangesAsync();
        }

        public void TryGenerateMobInZone(Zone zone, ref int counter)
        {
            IQueryable<Monster> niceMonstersQuery = null;
            switch (zone.Type)
            {
                case (0):
                    niceMonstersQuery = MeadowMonsterQuery;
                    break;
                case (1):
                    niceMonstersQuery = ForestMonsterQuery;
                    break;
                case (2):
                    niceMonstersQuery = DesertMonsterQuery;
                    break;
                case (3):
                    niceMonstersQuery = MountainMonsterQuery;
                    break;
                case (4):
                    niceMonstersQuery = TownMonsterQuery;
                    break;
            }
            var niceMonsters = niceMonstersQuery.ToList();
            Random random = new Random();
            for (int i=0; i<_maxMonstersInTile; i++)
            {
                int rarityLevel = 1;
                int rnd;
                while (true) //modifier generation
                {
                    rnd = random.Next(0, 2);
                    if (rnd == 0)
                        rarityLevel++;
                    else
                        break;
                }
                if (rarityLevel > _context.Monsters.Max(m => m.RarityLevel))
                {
                    rarityLevel = _context.Monsters.Max(m => m.RarityLevel);
                }
                int chance = (int)Math.Pow(2, i)+1;
                rnd = random.Next(0, chance);
                if (rnd == 0 && (niceMonsters.Count() != 0))
                {
                    int monsterPosInList = random.Next(0, niceMonsters.Count());
                    int existingMonsterId;
                    if (_context.ExistingMonsters.Count() == 0)
                        existingMonsterId = 0;
                    else
                        existingMonsterId = _context.ExistingMonsters.Max(m => m.ExistingMonsterId) + 1;
                    ExistingMonster existingMonster = new ExistingMonster
                    {
                        Damaged = 0,
                        XPos = zone.XPos,
                        YPos = zone.YPos,
                        MonsterId = niceMonsters[monsterPosInList].MonsterId,
                        ExistingMonsterId = counter
                    };
                    _context.ExistingMonsters.Add(existingMonster);
                    counter++;
                }

            }
        }
        public JsonResult Turn(int id, int existingMontersId)
        {
            DealDamage(id, existingMontersId);
            MonsterTurn(id, existingMontersId);
            var result = BattleLog;
            return Json(result);
        }

        public bool MonsterTurn(int id, int existingMontersId)
        {
            var stats = CalculateDamageAndArmor(id);
            int damage = stats[0];
            int armor = stats[1];
            var dealtDamageToPlayer = (_context.ExistingMonsters.Include(m => m.Monster).FirstOrDefault(m => m.ExistingMonsterId == existingMontersId).Monster.Damage - armor);
            if (dealtDamageToPlayer < 0)
                dealtDamageToPlayer = 0;
            _context.CharacterStates.FirstOrDefault(s => s.CharacterId == id).Hp -= dealtDamageToPlayer;
            _context.SaveChangesAsync();
            BattleLog.Add(_context.ExistingMonsters.Include(m => m.Monster).FirstOrDefault(m => m.ExistingMonsterId == existingMontersId).Monster.MonsterName 
                + " dealt " + "<b>"+dealtDamageToPlayer + "</b>" + " damage to you." + "\n");
            if (_context.CharacterStates.FirstOrDefault(s => s.CharacterId == id).Hp < 0)
                return true;
            return false;
        }

        #region Turns
        public bool DealDamage(int id, int existingMontersId)
        {
            var stats = CalculateDamageAndArmor(id);
            int damage = stats[0];
            int armor = stats[1];
            var dealtDamageToMonster = (damage - _context.ExistingMonsters.Include(m => m.Monster).FirstOrDefault(m => m.ExistingMonsterId == existingMontersId).Monster.Armor);
            if (dealtDamageToMonster < 0)
                dealtDamageToMonster = 0;
            _context.ExistingMonsters.FirstOrDefault(m => m.ExistingMonsterId == existingMontersId).Damaged += dealtDamageToMonster;
            BattleLog.Add( "You damaged " + _context.ExistingMonsters.Include(m => m.Monster).FirstOrDefault(m => m.ExistingMonsterId == existingMontersId).Monster.MonsterName +
                " for " + "<b>"+ dealtDamageToMonster + "</b>" + " hp" +"\n");
            _context.SaveChangesAsync();
            if (_context.ExistingMonsters.FirstOrDefault(m => m.ExistingMonsterId == existingMontersId).Damaged >
                _context.ExistingMonsters.FirstOrDefault(m => m.ExistingMonsterId == existingMontersId).Monster.Hp)
            {
                return true;
            }
            return false;
        }
        #endregion
        #endregion
        //end of battle logic
        public IActionResult GetInventory(int id)
        {
            var inventory = from i in _context.ExistingItems
                            where (i.CharacterId == id)
                            select i;
            var inventoryList = inventory.Include(i => i.Item).Include(i => i.ItemType).OrderBy(i =>
            (i.Item.Worth + i.ItemType.AdditionalWorth) * i.ItemType.WorthModifier).ToList();
            return PartialView("Inventory", inventoryList);
        }

        public IActionResult GetItemInfo(int id, int itemId)
        {
            var inventory = from i in _context.ExistingItems
                            where (i.CharacterId == id)
                            select i;
            bool equiped = false;
            var item = inventory.Include(i => i.Item).Include(i => i.ItemType).FirstOrDefault(i => i.ExistingItemId == itemId);
            if (item != null)
            {
                if (_context.CharacterStates.FirstOrDefault(s => s.CharacterId == id).EquipedWeaponId == item.ExistingItemId ||
                _context.CharacterStates.FirstOrDefault(s => s.CharacterId == id).EquipedOffhandId == item.ExistingItemId ||
                _context.CharacterStates.FirstOrDefault(s => s.CharacterId == id).EquipedArmorId == item.ExistingItemId ||
                _context.CharacterStates.FirstOrDefault(s => s.CharacterId == id).EquipedAmuletId == item.ExistingItemId)
                    equiped = true;
                if (equiped)
                    ViewBag.EquipedItemId = item.ExistingItemId;
            }
            return PartialView("ItemInfo", item);
        }

        public IActionResult GetMonsterInfo(int existing_monster_id)
        {
            var existing_monster = _context.ExistingMonsters.FirstOrDefault(m => m.ExistingMonsterId == existing_monster_id);
            var monster = _context.Monsters.FirstOrDefault(m => m.MonsterId == existing_monster.MonsterId);
            ViewBag.CurrentHP = monster.Hp - existing_monster.Damaged;
            return PartialView("MonsterInfo", monster);
        }
        
        public IActionResult EnterTown(int XPos, int YPos)
        {
            return PartialView("Town");
        }
        public IActionResult EnterTavern()
        {
            return PartialView("Tavern");
        }

        public IActionResult SleepNight()
        {
            ViewBag.TavernType = "sleep";
            return PartialView("Tavern");
        }
        public bool SpendMoney(int id, int amount)
        {
            var character = _context.Characters.Include(c => c.CharacterInfo).Include(c => c.CharacterState).FirstOrDefault(c => c.CharacterId == id);
            if (character.CharacterInfo.Gold < amount)
                return false;
            character.CharacterInfo.Gold -= amount;
            _context.SaveChangesAsync();
            return true;
        }
        public IActionResult GetTownEvents(int XPos, int YPos, int type)
        {

            List<Merchant> merchantsInTown = _context.Merchants.Where(m => m.XPos == XPos && m.YPos == YPos).ToList();
            Zone zone = _context.Zones.FirstOrDefault(m => m.XPos == XPos && m.YPos == YPos);
            TownInfoObject townInfo = new TownInfoObject(merchantsInTown, zone, type);
            return PartialView("TownEvents", townInfo);
        }

        public JsonResult Equip (int id, int existingItemId, string type)
        {
            switch (type)
            {
                case "weapon":
                    _context.CharacterStates.FirstOrDefaultAsync(c => c.CharacterId == id).Result.EquipedWeaponId = existingItemId;
                    break;
                case "offhand":
                    _context.CharacterStates.FirstOrDefaultAsync(c => c.CharacterId == id).Result.EquipedOffhandId = existingItemId;
                    break;
                case "armor":
                    _context.CharacterStates.FirstOrDefaultAsync(c => c.CharacterId == id).Result.EquipedArmorId = existingItemId;
                    break;
                case "amulet":
                    _context.CharacterStates.FirstOrDefaultAsync(c => c.CharacterId == id).Result.EquipedAmuletId = existingItemId;
                    break;
                default:
                    break;
            }
            _context.SaveChangesAsync();
            return Json(true);
        }
        public JsonResult UnEquip(int id, string type)
        {
            switch (type)
            {
                case "weapon":
                    _context.CharacterStates.FirstOrDefaultAsync(c => c.CharacterId == id).Result.EquipedWeaponId = null;
                    break;
                case "offhand":
                    _context.CharacterStates.FirstOrDefaultAsync(c => c.CharacterId == id).Result.EquipedOffhandId = null;
                    break;
                case "armor":
                    _context.CharacterStates.FirstOrDefaultAsync(c => c.CharacterId == id).Result.EquipedArmorId = null;
                    break;
                case "amulet":
                    _context.CharacterStates.FirstOrDefaultAsync(c => c.CharacterId == id).Result.EquipedAmuletId = null;
                    break;
                default:
                    break;
            }
            _context.SaveChangesAsync();
            return Json(true);
        }

        //TODO: Нормальные расчеты атаки и защиты
        //TODO: Разные типы оружия/брони
        //TODO: Луки/стрелы
        //TODO: Боевая система
        //TODO: Действие мобов
        //TODO: Дизайн
            //TODO: Битву в отдельную страницу, пофиксит гору багов
        //TODO: Прокачка (награды за левелап)
        //TODO: Генерацию мобов
        //TODO: Инициализация/создание персонажей
        //TODO: Сделать скейл оружий и брони от характеристик
        //TODO: Таблицы умений и заклинаний
        //TODO: Другие ивенты на карте (сундуки, свитки, ловушки...)
        //TODO: Таблица расходников и их использование
        //TODO: Расход и регенрация стамины/маны/хп
        //TODO: Подумать насчет городов
        //Таверна
            //Ночевка (перегенерация мобов/лечение/бафф)
            //Восставновление стата без ночи
            //Покупка еды/зельев
            //Квесты
        //TODO: Торговля
        //TODO: Харизма (зачем она?)
        //TODO: Заполнить базы не тестовыми данными
        //TODO: Возможно, типы урона
        //TODO: Scaling from attr
        
    }
}