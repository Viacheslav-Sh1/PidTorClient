using System;

namespace PidTorClient;

public class PidTorClientSettings : Shared.Models.Base.Igroup
{
    public bool enable { get; set; } = true;
    public string displayname { get; set; } = "PidTor Client";
    public int displayindex { get; set; } = 552;
    public int group { get; set; }
    public bool group_hide { get; set; }

    public string redapi { get; set; } = "https://jac.red";
    public string apikey { get; set; }

    public int min_sid { get; set; } = 20;
    public long max_size { get; set; }
    public long max_serial_size { get; set; }
    public bool forceAll { get; set; }
    public bool emptyVoice { get; set; } = true;
    public string sort { get; set; } = "sid";
    public string filter { get; set; }
    public string filter_ignore { get; set; }
	
	public string msg_no_plugin { get; set; } = "⚠️ Плагин не установлен";
	public string msg_no_plugin_desc { get; set; } = "Установите плагин PidTorClient и настройте TorrServer";
	public string msg_no_torrserver { get; set; } = "⚠️ TorrServer недоступен";
	public string msg_no_torrserver_desc { get; set; } = "Проверьте настройки TorrServer в приложении";
}

public class Torrent
{
    public Torrent() { }

    public Torrent(string name, string voice, string magnet, int sid, string tr, string quality, long size, string mediainfo, Result torrent)
    {
        this.name = name;
        this.voice = voice;
        this.magnet = magnet;
        this.sid = sid;
        this.tr = tr;
        this.quality = quality;
        this.size = size;
        this.mediainfo = mediainfo;
        this.torrent = torrent;
    }

    public string name { get; set; }
    public string voice { get; set; }
    public string magnet { get; set; }
    public int sid { get; set; }
    public string tr { get; set; }
    public string quality { get; set; }
    public long size { get; set; }
    public string mediainfo { get; set; }
    public Result torrent { get; set; }
}

public class Info
{
    public string[] voices { get; set; }
    public string sizeName { get; set; }
    public short[] seasons { get; set; }
}

public class Result
{
    public string Tracker { get; set; }
    public string Title { get; set; }
    public long? Size { get; set; }
    public int Seeders { get; set; }
    public string MagnetUri { get; set; }
    public Info info { get; set; }
    public DateTime PublishDate { get; set; }
}

public class RootObject
{
    public Result[] Results { get; set; }
}