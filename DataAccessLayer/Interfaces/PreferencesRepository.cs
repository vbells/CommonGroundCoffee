using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public class PreferencesRepository : IPreferencesRepository
    {
        private readonly ApplicationDbContext _context;

        public PreferencesRepository(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }

        public async Task<IEnumerable<Preferences>> GetPreferencesAsync()
        {
            return await _context.Preferences.ToListAsync();
        }

    }
}
