using System.Linq;
using musika.Entities;

public class PlaylistService
{
    private readonly MusicAppContext _context;

    public PlaylistService(MusicAppContext context)
    {
        _context = context;
    }

    public void CreatePlaylist(string name, string category)
    {
        var playlist = new Playlist { Name = name, Category = category };
        _context.Playlists.Add(playlist);
        _context.SaveChanges();
    }

    public void AddTrackToPlaylist(int playlistId, int trackId)
    {
        var playlist = _context.Playlists.Find(playlistId);
        var track = _context.Tracks.Find(trackId);

        if (playlist != null && track != null)
        {
            playlist.Tracks.Add(track);
            _context.SaveChanges();
        }
    }
}
