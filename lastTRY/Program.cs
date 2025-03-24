using System;
using System.Linq;
using musika.Entities;

namespace musika.Entities
{
    class Program
    {
        static void Main()
        {
            using (var context = new MusicAppContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var playlistService = new PlaylistService(context);

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("🎵 Музичний додаток 🎵");
                    Console.WriteLine("1. Додати артиста");
                    Console.WriteLine("2. Додати альбом");
                    Console.WriteLine("3. Додати трек");
                    Console.WriteLine("4. Створити плейлист");
                    Console.WriteLine("5. Додати трек у плейлист");
                    Console.WriteLine("6. Переглянути всі плейлисти");
                    Console.WriteLine("7. Відображення треків певного альбому");
                    Console.WriteLine("8. Відображення ТОП-3 треків та альбомів артиста");
                    Console.WriteLine("9. Пошук трека");
                    Console.WriteLine("0. Вийти");
                    Console.Write("Виберіть опцію: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddArtist(context);
                            break;
                        case "2":
                            AddAlbum(context);
                            break;
                        case "3":
                            AddTrack(context);
                            break;
                        case "4":
                            CreatePlaylist(playlistService);
                            break;
                        case "5":
                            AddTrackToPlaylist(playlistService, context);
                            break;
                        case "6":
                            ShowPlaylists(context);
                            break;
                        case "7":
                            ShowTracksAboveAverage(context);
                            break;
                        case "8":
                            ShowTop3TracksAndAlbumsByArtist(context);
                            break;
                        case "9":
                            SearchTrack(context);
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("❌ Невірний вибір! Спробуйте ще раз.");
                            break;
                    }
                    Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
                    Console.ReadKey();
                }
            }
        }

        static void ShowTracksAboveAverage(MusicAppContext context)
        {
            Console.Write("Введіть ID альбому: ");
            int albumId = int.Parse(Console.ReadLine());

            var album = context.Albums
                .Where(a => a.Id == albumId)
                .FirstOrDefault();

            if (album == null)
            {
                Console.WriteLine("❌ Альбом не знайдено!");
                return;
            }

            var avgPlays = context.Tracks
                .Where(t => t.AlbumId == albumId)
                .Average(t => t.PlayCount);

            var tracks = context.Tracks
                .Where(t => t.AlbumId == albumId && t.PlayCount > avgPlays)
                .ToList();

            Console.WriteLine($"Треки з альбому {album.Name}, що мають більше за середнє значення прослуховувань:");
            foreach (var track in tracks)
            {
                Console.WriteLine($"- {track.Name} ({track.PlayCount} прослуховувань)");
            }
        }

        static void ShowTop3TracksAndAlbumsByArtist(MusicAppContext context)
        {
            Console.Write("Введіть ID артиста: ");
            int artistId = int.Parse(Console.ReadLine());

            var artist = context.Artists
                .Where(a => a.Id == artistId)
                .FirstOrDefault();

            if (artist == null)
            {
                Console.WriteLine("❌ Артист не знайдений!");
                return;
            }

            var topTracks = context.Tracks
                .Where(t => t.Album.ArtistId == artistId)
                .OrderByDescending(t => t.Rating)
                .Take(3)
                .ToList();

            Console.WriteLine($"ТОП-3 треки артиста {artist.FirstName} {artist.LastName}:");
            foreach (var track in topTracks)
            {
                Console.WriteLine($"- {track.Name} ({track.Rating} рейтинг)");
            }

            var topAlbums = context.Albums
                .Where(a => a.ArtistId == artistId)
                .OrderByDescending(a => a.Rating)
                .Take(3)
                .ToList();

            Console.WriteLine($"\nТОП-3 альбоми артиста {artist.FirstName} {artist.LastName}:");
            foreach (var album in topAlbums)
            {
                Console.WriteLine($"- {album.Name} ({album.Rating} рейтинг)");
            }
        }

        static void SearchTrack(MusicAppContext context)
        {
            Console.Write("Введіть частину назви треку або уривок тексту: ");
            string searchTerm = Console.ReadLine().ToLower();

            var tracks = context.Tracks
                .Where(t => t.Name.ToLower().Contains(searchTerm) || t.Lyrics.ToLower().Contains(searchTerm))
                .ToList();

            if (!tracks.Any())
            {
                Console.WriteLine("❌ Трек не знайдено.");
                return;
            }

            Console.WriteLine("Знайдені треки:");
            foreach (var track in tracks)
            {
                Console.WriteLine($"- {track.Name} ({track.Lyrics})");
            }
        }


        static void AddArtist(MusicAppContext context)
        {
            Console.Write("Введіть ім'я артиста: ");
            string firstName = Console.ReadLine();
            Console.Write("Введіть прізвище артиста: ");
            string lastName = Console.ReadLine();
            Console.Write("Введіть країну артиста: ");
            string country = Console.ReadLine();

            var artist = new Artist { FirstName = firstName, LastName = lastName, Country = country };
            context.Artists.Add(artist);
            context.SaveChanges();
            Console.WriteLine("✅ Артист доданий!");
        }

        static void AddAlbum(MusicAppContext context)
        {
            var artists = context.Artists.ToList();
            if (!artists.Any())
            {
                Console.WriteLine("❌ Немає жодного артиста! Спочатку додайте артиста.");
                return;
            }

            Console.Write("Введіть назву альбому: ");
            string name = Console.ReadLine();
            Console.Write("Введіть рік випуску: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Введіть жанр: ");
            string genre = Console.ReadLine();

            Console.WriteLine("Виберіть артиста:");
            for (int i = 0; i < artists.Count; i++)
                Console.WriteLine($"{i + 1}. {artists[i].FirstName} {artists[i].LastName}");

            int artistIndex = int.Parse(Console.ReadLine()) - 1;
            var album = new Album { Name = name, Year = year, Genre = genre, ArtistId = artists[artistIndex].Id };

            context.Albums.Add(album);
            context.SaveChanges();
            Console.WriteLine("✅ Альбом доданий!");
        }

        static void AddTrack(MusicAppContext context)
        {
            var albums = context.Albums.ToList();
            if (!albums.Any())
            {
                Console.WriteLine("❌ Немає жодного альбому! Спочатку додайте альбом.");
                return;
            }

            Console.Write("Введіть назву треку: ");
            string name = Console.ReadLine();
            Console.Write("Введіть тривалість (хвилини.секунди): ");
            TimeSpan duration = TimeSpan.Parse(Console.ReadLine());

            Console.WriteLine("Виберіть альбом:");
            for (int i = 0; i < albums.Count; i++)
                Console.WriteLine($"{i + 1}. {albums[i].Name}");

            int albumIndex = int.Parse(Console.ReadLine()) - 1;
            var track = new Track { Name = name, Duration = duration, AlbumId = albums[albumIndex].Id };

            context.Tracks.Add(track);
            context.SaveChanges();
            Console.WriteLine("✅ Трек доданий!");
        }

        static void CreatePlaylist(PlaylistService playlistService)
        {
            Console.Write("Введіть назву плейлиста: ");
            string name = Console.ReadLine();
            Console.Write("Введіть категорію: ");
            string category = Console.ReadLine();

            playlistService.CreatePlaylist(name, category);
        }

        static void AddTrackToPlaylist(PlaylistService playlistService, MusicAppContext context)
        {
            var playlists = context.Playlists.ToList();
            if (!playlists.Any())
            {
                Console.WriteLine("❌ Немає жодного плейлиста! Спочатку створіть плейлист.");
                return;
            }

            var tracks = context.Tracks.ToList();
            if (!tracks.Any())
            {
                Console.WriteLine("❌ Немає жодного треку! Спочатку додайте трек.");
                return;
            }

            Console.WriteLine("Виберіть плейлист:");
            for (int i = 0; i < playlists.Count; i++)
                Console.WriteLine($"{i + 1}. {playlists[i].Name}");

            int playlistIndex = int.Parse(Console.ReadLine()) - 1;

            Console.WriteLine("Виберіть трек:");
            for (int i = 0; i < tracks.Count; i++)
                Console.WriteLine($"{i + 1}. {tracks[i].Name}");

            int trackIndex = int.Parse(Console.ReadLine()) - 1;

            playlistService.AddTrackToPlaylist(playlists[playlistIndex].Id, tracks[trackIndex].Id);
        }

        static void ShowPlaylists(MusicAppContext context)
        {
            var playlists = context.Playlists.ToList();
            if (!playlists.Any())
            {
                Console.WriteLine("❌ Немає жодного плейлиста!");
                return;
            }

            Console.WriteLine("\n📜 Список плейлистів:");
            foreach (var playlist in playlists)
            {
                Console.WriteLine($"🎵 {playlist.Name} ({playlist.Category})");
                var tracks = context.Tracks
                    .Where(t => t.Playlists.Any(p => p.Id == playlist.Id))
                    .ToList();

                if (tracks.Any())
                {
                    Console.WriteLine("   📌 Треки:");
                    foreach (var track in tracks)
                        Console.WriteLine($"   - {track.Name} ({track.Duration})");
                }
                else
                {
                    Console.WriteLine("   ❌ Немає треків.");
                }
            }
        }
    }
}

