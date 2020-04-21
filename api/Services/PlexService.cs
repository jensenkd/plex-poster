using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plex.Api;
using Plex.Api.Models.Status;

namespace Dibbler.Poster.Api.Services
{
    public class PlexService
    {
        private readonly IPlexClient _plexClient;
      
        public PlexService(IPlexClient plexClient)
        {
            _plexClient = plexClient;
        }

        public async Task<Session> GetActiveSession(string authKey, string plexServerHost, string playerMachineId)
        {
            List<Session> sessions = await _plexClient.GetSessions(authKey, plexServerHost);
            if (sessions == null || sessions.Count == 0)
            {
                return null;
            }

            return sessions.FirstOrDefault(c => c.Player.MachineIdentifier == playerMachineId);
        }
        
    }
}