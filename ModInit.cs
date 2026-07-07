using Microsoft.AspNetCore.Http;
using Shared;
using Shared.Models.Base;
using Shared.Models.Events;
using Shared.Models.Module;
using Shared.Models.Module.Interfaces;
using Shared.Services;
using System.Collections.Generic;

namespace PidTorClient;

public class ModInit : IModuleLoaded, IModuleOnline
{
    public static string modpath;
    public static PidTorClientSettings conf;

    public List<ModuleOnlineItem> Invoke(HttpContext httpContext, RequestModel requestInfo, 
        string host, OnlineEventsModel args)
    {
        if (!conf.enable)
            return null;

        var md = new BaseSettings()
        {
            plugin = "PidTorClient",
            enable = conf.enable,
            enabled = conf.enable,
            displayname = conf.displayname,
            displayindex = conf.displayindex,
            group = conf.group,
            group_hide = conf.group_hide
        };

        return new List<ModuleOnlineItem>()
        {
            new(md, "pidtorclient", "PidTor Client")
        };
    }

    public void Loaded(InitspaceModel baseconf)
    {
        modpath = baseconf.path;
        CoreInit.conf.online.with_search.Add("pidtorclient");
        updateConf();
        EventListener.UpdateInitFile += updateConf;
        EventListener.OnlineApiQuality += onlineApiQuality;
    }

    public void Dispose()
    {
        EventListener.UpdateInitFile -= updateConf;
        EventListener.OnlineApiQuality -= onlineApiQuality;
    }

void updateConf()
{
    conf = ModuleInvoke.Init("PidTorClient", new PidTorClientSettings()
    {
        enable = true,
        displayindex = 552,
        min_sid = 20,
        emptyVoice = true,
        redapi = "https://jac.red",
        apikey = "",
        msg_no_plugin = "⚠️ Плагин не установлен",
        msg_no_plugin_desc = "Установите плагин PidTorClient и настройте TorrServer",
        msg_no_torrserver = "⚠️ TorrServer недоступен",
        msg_no_torrserver_desc = "Проверьте настройки TorrServer в приложении"
    });
}

    string onlineApiQuality(EventOnlineApiQuality e)
    {
        return e.balanser == "pidtorclient" ? " ~ 2160p" : null;
    }
}