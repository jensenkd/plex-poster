using System;
using System.Threading.Tasks;
using Plex.Api.Clients.Interfaces;
using Plex.Api.Factories;
using Plex.Api.PlexModels.Media;

namespace PlexPoster.Api.Services
{
    public class PlexService
    {
        private readonly IPlexAccountClient _plexAccountClient;
        private readonly IPlexServerClient _plexServerClient;
        private readonly IPlexLibraryClient _plexLibraryClient;
        private readonly IPlexFactory _plexFactory;
        public PlexService(IPlexFactory plexFactory, IPlexAccountClient accountClient,
            IPlexServerClient serverClient,IPlexLibraryClient libraryClient)
        {
            _plexFactory = plexFactory;
            _plexAccountClient = accountClient;
            _plexServerClient = serverClient;
            _plexLibraryClient = libraryClient;
        }

        public async Task<Metadata> GetRandomMovie(string authKey, string plexServerHost, string[] movieLibraries)
        {
            throw new NotImplementedException();
            //var movies = _plexLibraryClient.LibraryItems(authKey, plexServerHost, movieLibraries);

            // var directories = libraries.MediaContainer.Directory.
            //     Where(c => movieLibraries.Contains(c.Title, StringComparer.OrdinalIgnoreCase));
            //
            // List<Metadata> movies = new List<Metadata>();
            // foreach (var directory in directories)
            // {
            //     var items = await _plexClient.GetLibrary(authKey, plexServerHost, directory.Key);
            //     movies.AddRange(items.MediaContainer.Metadata);
            // }
            //
            // Random rnd = new Random();
            // return movies[rnd.Next(movies.Count)];
        }

        // public async Task<Session> GetActiveSession(string authKey, string plexServerHost, string playerMachineId)
        // {
        //     List<Session> sessions = await _plexClient.GetSessions(authKey, plexServerHost);
        //     if (sessions == null || sessions.Count == 0)
        //     {
        //         return null;
        //     }
        //
        //     return sessions.FirstOrDefault(c => c.Player.MachineIdentifier == playerMachineId);
        // }
    }
}