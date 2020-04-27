using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plex.Api;
using Plex.Api.Models;
using Plex.Api.Models.Status;
using PlexPoster.Api.Data;
using PlexPoster.Api.Entities;
using PlexPoster.Api.ResourceModels;

namespace PlexPoster.Api.Services
{
    public class PlexService
    {
        private readonly IPlexClient _plexClient;
      
        public PlexService(IPlexClient plexClient)
        {
            _plexClient = plexClient;
        }

        public async Task<Config> GetConfig()
        {
            await using (var db = new ConfigContext())
            {
                Config config = db.Configuration;
                return config;
            }
        }

        public async Task<Metadata> GetRandomMovie(string authKey, string plexServerHost, string[] movieLibraries)
        {
            PlexMediaContainer libraries = await _plexClient.GetLibraries(authKey, plexServerHost);

            var directories = libraries.MediaContainer.Directory.
                Where(c => movieLibraries.Contains(c.Title, StringComparer.OrdinalIgnoreCase));

            List<Metadata> movies = new List<Metadata>();
            foreach (var directory in directories)
            {
                var items = await _plexClient.GetLibrary(authKey, plexServerHost, directory.Key);
                movies.AddRange(items.MediaContainer.Metadata);
            }
            
            Random rnd = new Random();
            return movies[rnd.Next(movies.Count)];
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

        public async void UpdateConfig(ConfigModel configModel)
        {
            await using (var db = new ConfigContext())
            {
                Config config = db.Configuration;
                config.ComingSoonText = configModel.ComingSoonText;
                config.NowPlayingText = configModel.NowPlayingText;
                await db.SaveChangesAsync();
            }
        }
    }
}