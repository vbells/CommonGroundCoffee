using DataAccessLayer.Interfaces;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class PreferencesService : IPreferencesService
    {
        private readonly IPreferencesRepository _preferencesRepository;

        public PreferencesService(IPreferencesRepository preferencesRepository)
        {
            this._preferencesRepository = preferencesRepository;
        }

        public async Task<IEnumerable<Preferences>> GetPreferencesAsync()
        {
            return await _preferencesRepository.GetPreferencesAsync();
        }
    }
}
