using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shared;
using Shared.Attributes;
using Shared.Models.Base;
using Shared.Models.Templates;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PidTorClient;

public class PiTorClient : BaseOnlineController
{
    private static readonly HashSet<string> AllVoices = new()
    {
        "Movie Dubbing", "Bravo Records", "Ozz", "Laci", "Kerob", "LE-Production",  "Parovoz Production", "Paradox", "Omskbird", "LostFilm", "Причудики", "BaibaKo", "NewStudio", "AlexFilm", "FocusStudio", "Gears Media", "Jaskier", "ViruseProject",
        "Кубик в Кубе", "IdeaFilm", "Sunshine Studio", "Ozz.tv", "Hamster Studio", "Сербин", "To4ka", "Кравец", "Victory-Films", "SNK-TV", "GladiolusTV", "Jetvis Studio", "ApofysTeam", "ColdFilm",
        "Agatha Studdio", "KinoView", "Jimmy J.", "Shadow Dub Project", "Amedia", "Red Media", "Selena International", "Гоблин", "Universal Russia", "Kiitos", "Paramount Comedy", "Кураж-Бамбей",
        "Студия Пиратского Дубляжа", "Чадов", "Карповский", "RecentFilms", "Первый канал", "Alternative Production", "NEON Studio", "Колобок", "Дольский", "Синема УС", "Гаврилов", "Живов", "SDI Media",
        "Алексеев", "GreenРай Studio", "Михалев", "Есарев", "Визгунов", "Либергал", "Кузнецов", "Санаев", "ДТВ", "Дохалов", "Sunshine Studio", "Горчаков", "LevshaFilm", "CasStudio", "Володарский",
        "ColdFilm", "Шварко", "Карцев", "ETV+", "ВГТРК", "Gravi-TV", "1001cinema", "Zone Vision Studio", "Хихикающий доктор", "Murzilka", "turok1990", "FOX", "STEPonee", "Elrom", "Колобок", "HighHopes",
        "SoftBox", "GreenРай Studio", "NovaFilm", "Четыре в квадрате", "Greb&Creative", "MUZOBOZ", "ZM-Show", "RecentFilms", "Kerems13", "Hamster Studio", "New Dream Media", "Игмар", "Котов", "DeadLine Studio",
        "Jetvis Studio", "РенТВ", "Андрей Питерский", "Fox Life", "Рыбин", "Trdlo.studio", "Studio Victory Аsia", "Ozeon", "НТВ", "CP Digital", "AniLibria", "STEPonee", "Levelin", "FanStudio", "Cmert",
        "Интерфильм", "SunshineStudio", "Kulzvuk Studio", "Кашкин", "Вартан Дохалов", "Немахов", "Sedorelli", "СТС", "Яроцкий", "ICG", "ТВЦ", "Штейн", "AzOnFilm", "SorzTeam", "Гаевский", "Мудров",
        "Воробьев Сергей", "Студия Райдо", "DeeAFilm Studio", "zamez", "ViruseProject", "Иванов", "STEPonee", "РенТВ", "СВ-Дубль", "BadBajo", "Комедия ТВ", "Мастер Тэйп", "5-й канал СПб", "SDI Media",
        "Гланц", "Ох! Студия", "СВ-Кадр", "2x2", "Котова", "Позитив", "RusFilm", "Назаров", "XDUB Dorama", "Реальный перевод", "Kansai", "Sound-Group", "Николай Дроздов", "ZEE TV", "Ozz.tv", "MTV",
        "Сыендук", "GoldTeam", "Белов", "Dream Records", "Яковлев", "Vano", "SilverSnow", "Lord32x", "Filiza Studio", "Sony Sci-Fi", "Flux-Team", "NewStation", "XDUB Dorama", "Hamster Studio", "Dream Records",
        "DexterTV", "ColdFilm", "Good People", "RusFilm", "Levelin", "AniDUB", "SHIZA Project", "AniLibria.TV", "StudioBand", "AniMedia", "Kansai", "Onibaku", "JWA Project", "MC Entertainment", "Oni", "Jade",
        "Ancord", "ANIvoice", "Nika Lenina", "Bars MacAdams", "JAM", "Anika", "Berial", "Kobayashi", "Cuba77", "RiZZ_fisher", "OSLIKt", "Lupin", "Ryc99", "Nazel & Freya", "Trina_D", "JeFerSon", "Vulpes Vulpes",
        "Hamster", "KinoGolos", "Fox Crime", "Денис Шадинский", "AniFilm", "Rain Death", "LostFilm", "New Records", "Ancord", "Первый ТВЧ", "RG.Paravozik", "Profix Media", "Tycoon", "RealFake",
        "HDrezka", "Jimmy J.", "AlexFilm", "Discovery", "Viasat History", "AniMedia", "JAM", "HiWayGrope", "Ancord", "СВ-Дубль", "Tycoon", "SHIZA Project", "GREEN TEA", "STEPonee", "AlphaProject",
        "AnimeReactor", "Animegroup", "Shachiburi", "Persona99", "3df voice", "CactusTeam", "AniMaunt", "AniMedia", "AnimeReactor", "ShinkaDan", "Jaskier", "ShowJet", "RAIM", "RusFilm", "Victory-Films",
        "АрхиТеатр", "Project Web Mania", "ko136", "КураСгречей", "AMS", "СВ-Студия", "Храм Дорам ТВ", "TurkStar", "Медведев", "Рябов", "BukeDub", "FilmGate", "FilmsClub", "Sony Turbo", "ТВЦ", "AXN Sci-Fi",
        "NovaFilm", "DIVA Universal", "Курдов", "Неоклассика", "fiendover", "SomeWax", "Логинофф", "Cartoon Network", "Sony Turbo", "Loginoff", "CrezaStudio", "Воротилин", "LakeFilms", "Andy", "CP Digital",
        "XDUB Dorama + Колобок", "SDI Media", "KosharaSerials", "Екатеринбург Арт", "Julia Prosenuk", "АРК-ТВ Studio", "Т.О Друзей", "Anifilm", "Animedub", "AlphaProject", "Paramount Channel", "Кириллица",
        "AniPLague", "Видеосервис", "JoyStudio", "HighHopes", "TVShows", "AniFilm", "GostFilm", "West Video", "Формат AB", "Film Prestige", "West Video", "Екатеринбург Арт", "SovetRomantica", "РуФилмс",
        "AveBrasil", "Greb&Creative", "BTI Studios", "Пифагор", "Eurochannel", "NewStudio", "Кармен Видео", "Кошкин", "Кравец", "Rainbow World", "Воротилин", "Варус-Видео", "ClubFATE", "HiWay Grope",
        "Banyan Studio", "Mallorn Studio", "Asian Miracle Group", "Эй Би Видео", "AniStar", "Korean Craze", "LakeFilms", "Невафильм", "Hallmark", "Netflix", "Mallorn Studio", "Sony Channel", "East Dream",
        "Bonsai Studio", "Lucky Production", "Octopus", "TUMBLER Studio", "CrazyCatStudio", "Amber", "Train Studio", "Анастасия Гайдаржи", "Мадлен Дюваль", "Fox Life", "Sound Film", "Cowabunga Studio", "Фильмэкспорт",
        "VO-Production", "Sound Film", "Nickelodeon", "MixFilm", "GreenРай Studio", "Sound-Group", "Back Board Cinema", "Кирилл Сагач", "Bonsai Studio", "Stevie", "OnisFilms", "MaxMeister", "Syfy Universal",
        "TUMBLER Studio", "NewStation", "Neo-Sound", "Муравский", "IdeaFilm", "Рутилов", "Тимофеев", "Лагута", "Дьяконов", "Zone Vision Studio", "Onibaku", "AniMaunt", "Voice Project", "AniStar", "Пифагор",
        "VoicePower", "StudioFilms", "Elysium", "AniStar", "BeniAffet", "Selena International", "Paul Bunyan", "CoralMedia", "Кондор", "Игмар", "ViP Premiere", "FireDub", "AveTurk", "Sony Sci-Fi", "Янкелевич",
        "Киреев", "Багичев", "2x2", "Лексикон", "Нота", "Arisu", "Superbit", "AveDorama", "VideoBIZ", "Киномания", "DDV", "Alternative Production", "WestFilm", "Анастасия Гайдаржи + Андрей Юрченко", "Киномания",
        "Agatha Studdio", "GreenРай Studio", "VSI Moscow", "Horizon Studio", "Flarrow Films", "Amazing Dubbing", "Asian Miracle Group", "Видеопродакшн", "VGM Studio", "FocusX", "CBS Drama", "NovaFilm", "Novamedia",
        "East Dream", "Дасевич", "Анатолий Гусев", "Twister", "Морозов", "NewComers", "kubik&ko", "DeMon", "Анатолий Ашмарин", "Inter Video", "Пронин", "AMC", "Велес", "Volume-6 Studio", "Хоррор Мэйкер",
        "Ghostface", "Sephiroth", "Акира", "Деваль Видео", "RussianGuy27", "neko64", "Shaman", "Franek Monk", "Ворон", "Andre1288", "Selena International", "GalVid", "Другое кино", "Студия NLS", "Sam2007",
        "HaseRiLLoPaW", "Севастьянов", "D.I.M.", "Марченко", "Журавлев", "Н-Кино", "Lazer Video", "SesDizi", "Red Media", "Рудой", "Товбин", "Сергей Дидок", "Хуан Рохас", "binjak", "Карусель", "Lizard Cinema",
        "Варус-Видео", "Акцент", "RG.Paravozik", "Max Nabokov", "Barin101", "Васька Куролесов", "Фортуна-Фильм", "Amalgama", "AnyFilm", "Студия Райдо", "Козлов", "Zoomvision Studio", "Пифагор", "Urasiko",
        "VIP Serial HD", "НСТ", "Кинолюкс", "Project Web Mania", "Завгородний", "AB-Video", "Twister", "Universal Channel", "Wakanim", "SnowRecords", "С.Р.И", "Старый Бильбо", "Ozz.tv", "Mystery Film", "РенТВ",
        "Латышев", "Ващенко", "Лайко", "Сонотек", "Psychotronic", "DIVA Universal", "Gremlin Creative Studio", "Нева-1", "Максим Жолобов", "Good People", "Мобильное телевидение", "Lazer Video",
        "IVI", "DoubleRec", "Milvus", "RedDiamond Studio", "Astana TV", "Никитин", "КТК", "D2Lab", "НСТ", "DoubleRec", "Black Street Records", "Останкино", "TatamiFilm", "Видеобаза", "Crunchyroll", "Novamedia",
        "RedRussian1337", "КонтентикOFF", "Creative Sound", "HelloMickey Production", "Пирамида", "CLS Media", "Сонькин", "Мастер Тэйп", "Garsu Pasaulis", "DDV", "IdeaFilm", "Gold Cinema", "Че!", "Нарышкин",
        "Intra Communications", "OnisFilms", "XDUB Dorama", "Кипарис", "Королёв", "visanti-vasaer", "Готлиб", "Paramount Channel", "СТС", "диктор CDV", "Pazl Voice", "Прямостанов", "Zerzia", "НТВ", "MGM",
        "Дьяков", "Вольга", "АРК-ТВ Studio", "Дубровин", "МИР", "Netflix", "Jetix", "Кипарис", "RUSCICO", "Seoul Bay", "Филонов", "Махонько", "Строев", "Саня Белый", "Говинда Рага", "Ошурков", "Horror Maker",
        "Хлопушка", "Хрусталев", "Антонов Николай", "Золотухин", "АрхиАзия", "Попов", "Ultradox", "Мост-Видео", "Альтера Парс", "Огородников", "Твин", "Хабар", "AimaksaLTV", "ТНТ", "FDV", "3df voice",
        "The Kitchen Russia", "Ульпаней Эльром", "Видеоимпульс", "GoodTime Media", "Alezan", "True Dubbing Studio", "FDV", "Карусель", "Интер", "Contentica", "Мельница", "RealFake", "ИДДК", "Инфо-фильм",
        "Мьюзик-трейд", "Кирдин | Stalk", "ДиоНиК", "Стасюк", "TV1000", "Hallmark", "Тоникс Медиа", "Бессонов", "Gears Media", "Бахурани", "NewDub", "Cinema Prestige", "Набиев", "New Dream Media", "ТВ3",
        "Малиновский Сергей", "Superbit", "Кенс Матвей", "LE-Production", "Voiz", "Светла", "Cinema Prestige", "JAM", "LDV", "Videogram", "Индия ТВ", "RedDiamond Studio", "Герусов", "Элегия фильм", "Nastia",
        "Семыкина Юлия", "Электричка", "Штамп Дмитрий", "Пятница", "Oneinchnales", "Gravi-TV", "D2Lab", "Кинопремьера", "Бусов Глеб", "LE-Production", "1001cinema", "Amazing Dubbing", "Emslie",
        "1+1", "100 ТВ", "1001 cinema", "2+2", "2х2", "3df voice", "4u2ges", "5 канал", "A. Lazarchuk", "AAA-Sound", "AB-Video", "AdiSound", "ALEKS KV", "AlexFilm", "AlphaProject", "Alternative Production",
        "Amalgam", "AMC", "Amedia", "AMS", "Andy", "AniLibria", "AniMedia", "Animegroup", "Animereactor", "AnimeSpace Team", "Anistar", "AniUA", "AniWayt", "Anything-group", "AOS",
        "Arasi project", "ARRU Workshop", "AuraFilm", "AvePremier", "AveTurk", "AXN Sci-Fi", "Azazel", "AzOnFilm", "BadBajo", "BadCatStudio", "BBC Saint-Petersburg", "BD CEE", "Black Street Records",
        "Bonsai Studio", "Boльгa", "Brain Production", "BraveSound", "BTI Studios", "Bubble Dubbing Company", "Byako Records", "Cactus Team", "Cartoon Network", "CBS Drama", "CDV", "Cinema Prestige",
        "CinemaSET GROUP", "CinemaTone", "ColdFilm", "Contentica", "CP Digital", "CPIG", "Crunchyroll", "Cuba77", "D1", "D2lab", "datynet", "DDV", "DeadLine", "DeadSno", "DeMon", "den904", "Description",
        "DexterTV", "Dice", "Discovery", "DniproFilm", "DoubleRec", "DreamRecords", "DVD Classic", "East Dream", "Eladiel", "Elegia", "ELEKTRI4KA", "Elrom", "ELYSIUM", "Epic Team", "eraserhead", "erogg",
        "Eurochannel", "Extrabit", "F-TRAIN", "Family Fan Edition", "FDV", "FiliZa Studio", "Film Prestige", "FilmGate", "FilmsClub", "FireDub", "Flarrow Films", "Flux-Team", "FocusStudio", "FOX", "Fox Crime",
        "Fox Russia", "FoxLife", "Foxlight", "Franek Monk", "Gala Voices", "Garsu Pasaulis", "Gears Media", "Gemini", "General Film", "GetSmart", "Gezell Studio", "Gits", "GladiolusTV", "GoldTeam", "Good People",
        "Goodtime Media", "GoodVideo", "GostFilm", "Gramalant", "Gravi-TV", "GREEN TEA", "GreenРай Studio", "Gremlin Creative Studio", "Hallmark", "HamsterStudio", "HiWay Grope", "Horizon Studio", "hungry_inri",
        "ICG", "ICTV", "IdeaFilm", "IgVin &amp; Solncekleshka", "ImageArt", "INTERFILM", "Ivnet Cinema", "IНТЕР", "Jakob Bellmann", "JAM", "Janetta", "Jaskier", "JeFerSon", "jept", "JetiX", "Jetvis", "JimmyJ",
        "KANSAI", "KIHO", "kiitos", "KinoGolos", "Kinomania", "KosharaSerials", "Kолобок", "L0cDoG", "LakeFilms", "LDV", "LE-Production", "LeDoyen", "LevshaFilm", "LeXiKC", "Liga HQ", "Line", "Lisitz",
        "Lizard Cinema Trade", "Lord32x", "lord666", "LostFilm", "Lucky Production", "Macross", "madrid", "Mallorn Studio", "Marclail", "Max Nabokov", "MC Entertainment", "MCA", "McElroy", "Mega-Anime",
        "Melodic Voice Studio", "metalrus", "MGM", "MifSnaiper", "Mikail", "Milirina", "MiraiDub", "MOYGOLOS", "MrRose", "MTV", "Murzilka", "MUZOBOZ", "National Geographic", "NemFilm", "Neoclassica", "NEON Studio",
        "New Dream Media", "NewComers", "NewStation", "NewStudio", "Nice-Media", "Nickelodeon", "No-Future", "NovaFilm", "Novamedia", "Octopus", "Oghra-Brown", "OMSKBIRD", "Onibaku", "OnisFilms", "OpenDub",
        "OSLIKt", "Ozz TV", "PaDet", "Paramount Comedy", "Paramount Pictures", "Parovoz Production", "PashaUp", "Paul Bunyan", "Pazl Voice", "PCB Translate", "Persona99", "PiratVoice", "Postmodern", "Profix Media",
        "Project Web Mania", "Prolix", "QTV", "R5", "Radamant", "RainDeath", "RATTLEBOX", "RealFake", "Reanimedia", "Rebel Voice", "RecentFilms", "Red Media", "RedDiamond Studio", "RedDog", "RedRussian1337",
        "Renegade Team", "RG Paravozik", "RinGo", "RoxMarty", "Rumble", "RUSCICO", "RusFilm", "RussianGuy27", "Saint Sound", "SakuraNight", "Satkur", "Sawyer888", "Sci-Fi Russia", "SDI Media", "Selena", "seqw0",
        "SesDizi", "SGEV", "Shachiburi", "SHIZA", "ShowJet", "Sky Voices", "SkyeFilmTV", "SmallFilm", "SmallFilm", "SNK-TV", "SnowRecords", "SOFTBOX", "SOLDLUCK2", "Solod", "SomeWax", "Sony Channel", "Sony Turbo",
        "Sound Film", "SpaceDust", "ssvss", "st.Elrom", "STEPonee", "SunshineStudio", "Superbit", "Suzaku", "sweet couple", "TatamiFilm", "TB5", "TF-AniGroup", "The Kitchen Russia", "The Mike Rec.", "Timecraft",
        "To4kaTV", "Tori", "Total DVD", "TrainStudio", "Troy", "True Dubbing Studio", "TUMBLER Studio", "turok1990", "TV 1000", "TVShows", "Twister", "Twix", "Tycoon", "Ultradox", "Universal Russia", "VashMax2",
        "VendettA", "VHS", "VicTeam", "VictoryFilms", "Video-BIZ", "Videogram", "ViruseProject", "visanti-vasaer", "VIZ Media", "VO-production", "Voice Project Studio", "VoicePower", "VSI Moscow", "VulpesVulpes",
        "Wakanim", "Wayland team", "WestFilm", "WiaDUB", "WVoice", "XL Media", "XvidClub Studio", "zamez", "ZEE TV", "Zendos", "ZM-SHOW", "Zone Studio", "Zone Vision", "Агапов", "Акопян", "Алексеев", "Артемьев",
        "Багичев", "Бессонов", "Васильев", "Васильцев", "Гаврилов", "Герусов", "Готлиб", "Григорьев", "Дасевич", "Дольский", "Карповский", "Кашкин", "Киреев", "Клюквин", "Костюкевич", "Матвеев", "Михалев", "Мишин",
        "Мудров", "Пронин", "Савченко", "Смирнов", "Тимофеев", "Толстобров", "Чуев", "Шуваев", "Яковлев", "ААА-sound", "АБыГДе", "Акалит", "Акира", "Альянс", "Амальгама", "АМС", "АнВад", "Анубис", "Anubis", "Арк-ТВ",
        "АРК-ТВ Studio", "Б. Федоров", "Бибиков", "Бигыч", "Бойков", "Абдулов", "Белов", "Вихров", "Воронцов", "Горчаков", "Данилов", "Дохалов", "Котов", "Кошкин", "Назаров", "Попов", "Рукин", "Рутилов",
        "Варус Видео", "Васька Куролесов", "Ващенко С.", "Векшин", "Велес", "Весельчак", "Видеоимпульс", "Витя «говорун»", "Войсовер", "Вольга", "Ворон", "Воротилин", "Г. Либергал", "Г. Румянцев", "Гей Кино Гид",
        "ГКГ", "Глуховский", "Гризли", "Гундос", "Деньщиков", "Есарев", "Нурмухаметов", "Пучков", "Стасюк", "Шадинский", "Штамп", "sf@irat", "Держиморда", "Домашний", "ДТВ", "Дьяконов", "Е. Гаевский", "Е. Гранкин",
        "Е. Лурье", "Е. Рудой", "Е. Хрусталёв", "ЕА Синема", "Екатеринбург Арт", "Живаго", "Жучков", "З Ранку До Ночі", "Завгородний", "Зебуро", "Зереницын", "И. Еремеев", "И. Клушин", "И. Сафронов", "И. Степанов",
        "ИГМ", "Игмар", "ИДДК", "Имидж-Арт", "Инис", "Ирэн", "Ист-Вест", "К. Поздняков", "К. Филонов", "К9", "Карапетян", "Кармен Видео", "Карусель", "Квадрат Малевича", "Килька",  "Кипарис", "Королев", "Котова",
        "Кравец", "Кубик в Кубе", "Кураж-Бамбей", "Л. Володарский", "Лазер Видео", "ЛанселаП", "Лапшин", "Лексикон", "Ленфильм", "Леша Прапорщик", "Лизард", "Люсьена", "Заугаров", "Иванов", "Иванова и П. Пашут",
        "Латышев", "Ошурков", "Чадов", "Яроцкий", "Максим Логинофф", "Малиновский", "Марченко", "Мастер Тэйп", "Махонько", "Машинский", "Медиа-Комплекс", "Мельница", "Мика Бондарик", "Миняев", "Мительман",
        "Мост Видео", "Мосфильм", "Муравский", "Мьюзик-трейд", "Н-Кино", "Н. Антонов", "Н. Дроздов", "Н. Золотухин", "Н.Севастьянов seva1988", "Набиев", "Наталья Гурзо", "НЕВА 1", "Невафильм", "НеЗупиняйПродакшн",
        "Неоклассика", "Несмертельное оружие", "НЛО-TV", "Новий", "Новый диск", "Новый Дубляж", "Новый Канал", "Нота", "НСТ", "НТВ", "НТН", "Оверлорд", "Огородников", "Омикрон", "Гланц", "Карцев", "Морозов",
        "Прямостанов", "Санаев", "Парадиз", "Пепелац", "Первый канал ОРТ", "Переводман", "Перец", "Петербургский дубляж", "Петербуржец", "Пирамида", "Пифагор", "Позитив-Мультимедиа", "Прайд Продакшн", "Премьер Видео",
        "Премьер Мультимедиа", "Причудики", "Р. Янкелевич", "Райдо", "Ракурс", "РенТВ", "Россия", "РТР", "Русский дубляж", "Русский Репортаж", "РуФилмс", "Рыжий пес", "С. Визгунов", "С. Дьяков", "С. Казаков",
        "С. Кузнецов", "С. Кузьмичёв", "С. Лебедев", "С. Макашов", "С. Рябов", "С. Щегольков", "С.Р.И.", "Сolumbia Service", "Самарский", "СВ Студия", "СВ-Дубль", "Светла", "Селена Интернешнл", "Синема Трейд",
        "Синема УС", "Синта Рурони", "Синхрон", "Советский", "Сокуров", "Солодухин", "Сонотек", "Сонькин", "Союз Видео", "Союзмультфильм", "СПД - Сладкая парочка", "Строев", "СТС", "Студии Суверенного Лепрозория",
        "Студия «Стартрек»", "KOleso", "Студия Горького", "Студия Колобок", "Студия Пиратского Дубляжа", "Студия Райдо", "Студия Трёх", "Гуртом", "Супербит", "Сыендук", "Так Треба Продакшн", "ТВ XXI век", "ТВ СПб",
        "ТВ-3", "ТВ6", "ТВИН", "ТВЦ", "ТВЧ 1", "ТНТ", "ТО Друзей", "Толмачев", "Точка Zрения", "Трамвай-фильм", "ТРК", "Уолт Дисней Компани", "Хихидок", "Хлопушка", "Цікава Ідея", "Четыре в квадрате", "Швецов",
        "Штамп", "Штейн", "Ю. Живов", "Ю. Немахов", "Ю. Сербин", "Ю. Товбин", "Я. Беллманн", "Украинский"
    };

    [HttpGet, Staticache(manually: true)]
    [Route("lite/pidtorclient")]
    async public Task<ActionResult> Index(string title, string original_title, short year, 
        string original_language, byte serial, short s = -1, bool rjson = false, 
        string pidtor = null)
    {
        try
        {
            var init = ModInit.conf;
            if (!init.enable)
                return StatusCode(403);

            if (NoAccessGroup(init, out string error_msg))
                return Json(new { accsdb = true, msg = error_msg });

            const string PluginMarker = "1";
            if (pidtor != PluginMarker)
            {
                return WarningTpl(title, original_title, serial, s,
                    init.msg_no_plugin,
                    init.msg_no_plugin_desc,
                    "pidtorclient://install");
            }

            var cache = await InvokeCacheResult<List<Torrent>>(
                $"pidtorclient:{title}:{original_title}:{year}:{original_language}:{serial}", 
                20, textJson: true, onget: async e =>
            {
                string uri = $"{init.redapi}/api/v2.0/indexers/all/results?title={HttpUtility.UrlEncode(title)}&title_original={HttpUtility.UrlEncode(original_title)}&year={year}&is_serial={(original_language == "ja" ? 5 : (serial + 1))}&apikey={init.apikey}";

                var root = await Http.Get<RootObject>(uri, timeoutSeconds: 8, textJson: true);
                if (root?.Results == null || root.Results.Length == 0)
                    return e.Success(new List<Torrent>());

                var results = root.Results;
                var torrents = new List<Torrent>(results.Length);

                foreach (var torrent in results)
                {
                    string magnet = torrent.MagnetUri;
                    string name = torrent.Title;

                    if (string.IsNullOrEmpty(magnet) || string.IsNullOrEmpty(name))
                        continue;

                    if (torrent.Tracker == "selezen")
                        continue;

                    if (serial == 1 && init.max_serial_size > 0 && torrent.Size > init.max_serial_size)
                        continue;
                    if (serial != 1 && init.max_size > 0 && torrent.Size > init.max_size)
                        continue;

                    string lowerName = name.ToLower();

                    if (init.forceAll || Regex.IsMatch(lowerName, "(4k|uhd)( |\\]|,|$)") || 
                        name.Contains("2160p") || name.Contains("1080p") || name.Contains("720p"))
                    {
                        int sid = torrent.Seeders;
                        if (sid < init.min_sid) continue;

                        string mediainfo = torrent.info?.sizeName ?? "";
                        if (!string.IsNullOrEmpty(mediainfo))
                            mediainfo += " / ";

                        string voicename = string.Empty;

                        var voices = torrent.info.voices;
                        if (voices != null && voices.Length > 0)
                        {
                            var displayVoices = voices.Take(3);
                            voicename = string.Join(", ", displayVoices);
                            if (voices.Length > 3)
                                voicename += $" +{voices.Length - 3}";
                        }

                        if (string.IsNullOrWhiteSpace(voicename))
                        {
                            if (Regex.IsMatch(lowerName, "( дб| d|дубляж)", RegexOptions.IgnoreCase))
                                voicename += "Дубляж, ";
                            if (Regex.IsMatch(lowerName, "( ст| пм)", RegexOptions.IgnoreCase))
                                voicename += "Многоголосый, ";
                            if (torrent.Tracker.ToLower() == "lostfilm")
                                voicename += "LostFilm, ";
                            else if (torrent.Tracker.ToLower() == "toloka")
                                voicename += "Украинский, ";
                            else
                            {
                                foreach (string v in AllVoices)
                                {
                                    if (v.Length > 4 && lowerName.Contains(v.ToLower()))
                                        voicename += $"{v}, ";
                                }
                            }
                            voicename = Regex.Replace(voicename, ", +$", "");
                        }

                        if (init.emptyVoice == false && string.IsNullOrEmpty(voicename))
                            continue;

                        if (Regex.IsMatch(name, "HDR10", RegexOptions.IgnoreCase) || Regex.IsMatch(name, "10-?bit", RegexOptions.IgnoreCase))
                            mediainfo += " HDR10 ";
                        else if (Regex.IsMatch(name, "HDR", RegexOptions.IgnoreCase))
                            mediainfo += " HDR ";
                        else
                            mediainfo += " SDR ";

                        if (Regex.IsMatch(name, "HEVC", RegexOptions.IgnoreCase) || Regex.IsMatch(name, "H.265", RegexOptions.IgnoreCase))
                            mediainfo += " / H.265 ";
                        if (Regex.IsMatch(name, "Dolby Vision", RegexOptions.IgnoreCase))
                            mediainfo += " / Dolby Vision ";

                        string tr = "";
                        var match = Regex.Match(magnet, "(&|\\?)tr=([^&\\?]+)");
                        while (match.Success)
                        {
                            string t = match.Groups[2].Value.ToLowerAndTrim();
                            if (!string.IsNullOrEmpty(t))
                                tr += t.Contains("/") || t.Contains(":") ? $"&tr={HttpUtility.UrlEncode(t)}" : $"&tr={t}";
                            match = match.NextMatch();
                        }
                        if (!string.IsNullOrEmpty(tr)) tr = tr.Remove(0, 1);

                        if (!string.IsNullOrEmpty(init.filter) && !Regex.IsMatch($"{name}:{voicename}", init.filter, RegexOptions.IgnoreCase))
                            continue;
                        if (!string.IsNullOrEmpty(init.filter_ignore) && Regex.IsMatch($"{name}:{voicename}", init.filter_ignore, RegexOptions.IgnoreCase))
                            continue;

                        torrents.Add(new Torrent(name, voicename, magnet, sid, tr,
                            name.Contains("2160p") ? "2160p" : name.Contains("1080p") ? "1080p" : "720p",
                            torrent.Size ?? 0, mediainfo, torrent));
                    }
                }

                var movies = torrents
                    .OrderByDescending(i => i.voice.Contains("Дубляж"))
                    .ThenByDescending(i => !string.IsNullOrEmpty(i.voice))
                    .ThenByDescending(i => i.magnet.Contains("&tr="));

                movies = init.sort == "size" ? movies.ThenByDescending(i => i.size) :
                         init.sort == "sid" ? movies.ThenByDescending(i => i.sid) :
                         movies.ThenByDescending(i => i.torrent.PublishDate);

                return e.Success(movies.ToList());
            });

            return ContentTpl(cache, () =>
            {
                var torrents = cache.Value;
                string en_title = HttpUtility.UrlEncode(title);
                string en_original_title = HttpUtility.UrlEncode(original_title);

                if (serial == 1)
                {
                    if (s == -1)
                    {
                        var seasons = new HashSet<short>();
                        string quality = torrents.FirstOrDefault(i => 
                            Regex.IsMatch(i.name, "(4k|uhd)( |\\]|,|$)", RegexOptions.IgnoreCase) || 
                            i.name.Contains("2160p"))?.name != null ? "2160p" : 
                            torrents.FirstOrDefault(i => i.name.Contains("1080p"))?.name != null ? "1080p" : "720p";
                        
                        var tpl = new SeasonTpl(quality: quality);
                        foreach (var t in torrents)
                        {
                            if (t.torrent?.info?.seasons == null || t.torrent.info.seasons.Length == 0)
                                continue;
                            foreach (var item in t.torrent.info.seasons)
                                seasons.Add(item);
                        }
                        foreach (int season in seasons.OrderBy(i => i))
                        {
                            tpl.Append($"{season} сезон",
                                $"{host}/lite/pidtorclient?rjson={rjson}&title={en_title}&original_title={en_original_title}&year={year}&original_language={original_language}&serial=1&s={season}",
                                season);
                        }
                        return tpl;
                    }
                    else
                    {
                        var mtpl = new MovieTpl(title, original_title);
                        foreach (var torrent in torrents)
                        {
                            if (torrent?.torrent?.info?.seasons == null || torrent.torrent.info.seasons.Length == 0)
                                continue;
                            if (!torrent.torrent.info.seasons.Contains(s))
                                continue;

                            string encodedMagnet = Convert.ToBase64String(Encoding.UTF8.GetBytes(torrent.magnet));
                            var seasonList = torrent.torrent.info.seasons.OrderBy(i => i);
                            string seasonStr = "Сезон " + string.Join(",", seasonList);
                            string title_line = $"{seasonStr}";
                            if (!string.IsNullOrEmpty(torrent.voice))
                                title_line += $" | {torrent.voice}";
                            title_line += $" | {torrent.name}";

                            string info = "";
                            if (torrent.torrent.PublishDate != default)
                                info += $"{torrent.torrent.PublishDate:dd.MM.yyyy} | ";
                            info += $"{torrent.quality} | {torrent.mediainfo} | S:{torrent.sid}";

                            mtpl.Append(title_line, $"pidtor://{encodedMagnet}?s={s}", voice_name: info, quality: "0");
                        }
                        return mtpl;
                    }
                }
                else
                {
                    var mtpl = new MovieTpl(title, original_title);
                    foreach (var torrent in torrents)
                    {
                        string encodedMagnet = Convert.ToBase64String(Encoding.UTF8.GetBytes(torrent.magnet));
                        mtpl.Append(torrent.voice, $"pidtor://{encodedMagnet}",
                            voice_name: $"{torrent.quality} / {torrent.mediainfo} / {torrent.sid}",
                            quality: torrent.quality.Replace("p", ""));
                    }
                    return mtpl;
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PIDTOR FATAL: {ex}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Staticache(cacheMinutes: 10, always: true, setHeadersNoCache: true)]
    [Route("pidtor-client.js")]
    public ActionResult Plugin()
    {
        string js = @"(function init() {
    'use strict';
    
    var originalPlay = null;
    var isPidTorPlaying = false;
    
    if (typeof Lampa !== 'undefined' && Lampa.Listener) {
        Lampa.Listener.follow('request_before', function(e) {
            if (!e.params || typeof e.params.url !== 'string') return;
            if (e.params.url.indexOf('/lite/pidtorclient') === -1) return;
            e.params.url = Lampa.Utils.addUrlComponent(e.params.url, 'pidtor=1');
        });
    }
    
    if (typeof Lampa !== 'undefined' && Lampa.Player && Lampa.Player.play && Lampa.Torserver) {
        if (Lampa.Player.play.toString().indexOf('pidtor://') === -1) {
            originalPlay = Lampa.Player.play;
            
            Lampa.Player.play = function(data) {
                var url = data && data.url;
                
                if (url && url.indexOf('pidtor://') === 0) {
                    if (url === 'pidtorclient://install') {
                        Lampa.Noty.show('Установите плагин PidTorClient и настройте TorrServer');
                        return;
                    }
                    
                    if (url === 'pidtorclient://settings') {
                        Lampa.Noty.show('Проверьте настройки TorrServer в приложении');
                        return;
                    }
                    
                    if (isPidTorPlaying) return;
                    
                    var useLink = Lampa.Storage.get('torrserver_use_link', 'one');
                    var tsUrl = '';
                    
                    if (useLink === 'two') {
                        tsUrl = Lampa.Storage.get('torrserver_url_two', '');
                        if (!tsUrl) {
                            Lampa.Noty.show('Дополнительный TorrServer не настроен');
                            return;
                        }
                    } else {
                        tsUrl = Lampa.Storage.get('torrserver_url', '');
                        if (!tsUrl) {
                            Lampa.Noty.show('TorrServer не настроен');
                            return;
                        }
                    }
                    
                    if (tsUrl.indexOf('http') !== 0) {
                        tsUrl = window.location.protocol + '//' + tsUrl;
                    }
                    
                    isPidTorPlaying = true;
                    
                    Lampa.Loading.start(function() {
                        isPidTorPlaying = false;
                        Lampa.Loading.stop();
                    }, 'Загрузка торрента...');
                    
                    try {
                        var parts = url.replace('pidtor://', '').split('?s=');
                        var magnet = atob(parts[0]);
                        var season = parts[1] ? parseInt(parts[1]) : null;
                        
                        var savedTimeline = data.timeline;
                        var savedCard = data.card;
                        var savedTitle = data.title;
                        
                        var tsAuth = Lampa.Storage.get('torrserver_auth', 'false');
                        var tsLogin = Lampa.Storage.get('torrserver_login', '');
                        var tsPassword = Lampa.Storage.get('torrserver_password', '');
                        
                        var headers = {};
                        if ((tsAuth === 'true' || tsAuth === true) && tsLogin && tsPassword) {
                            headers['Authorization'] = 'Basic ' + btoa(tsLogin + ':' + tsPassword);
                        }
                        
                        Lampa.Loading.setText('Добавление торрента...');
                        
                        fetch(tsUrl + '/torrents', {
                            method: 'POST',
                            headers: headers,
                            body: JSON.stringify({
                                action: 'add',
                                link: magnet,
                                title: '[LAMPA] ' + (savedTitle || ''),
                                poster: '',
                                save_to_db: false
                            })
                        })
                        .then(function(r) { return r.json(); })
                        .then(function(json) {
                            if (!json || !json.hash) {
                                Lampa.Loading.stop();
                                Lampa.Noty.show('Ошибка добавления торрента');
                                isPidTorPlaying = false;
                                return;
                            }
                            
                            if (season) {
                                Lampa.Loading.stop();
                                Lampa.Torrent.open(json.hash, savedCard || {
                                    title: savedTitle,
                                    name: savedTitle
                                });
                                setTimeout(function() {
                                    isPidTorPlaying = false;
                                }, 1000);
                            } else {
                                Lampa.Loading.setText('Индексация торрента...');
                                
                                var retry = 0;
                                function tryGetFiles() {
                                    fetch(tsUrl + '/torrents', {
                                        method: 'POST',
                                        headers: headers,
                                        body: JSON.stringify({action: 'get', hash: json.hash})
                                    })
                                    .then(function(r) { return r.json(); })
                                    .then(function(f) {
                                        if (f.file_stats && f.file_stats.length > 0) {
                                            var ve = ['mkv','mp4','avi','m2ts','mov','wmv','flv','webm'];
                                            var vf = f.file_stats.filter(function(x) {
                                                return ve.indexOf(x.path.split('.').pop().toLowerCase()) >= 0;
                                            });
                                            
                                            if (vf.length === 0) vf = [f.file_stats[0]];
                                            var videoFile = vf.sort(function(a,b) { return b.length - a.length; })[0];
                                            
                                            Lampa.Loading.stop();
                                            
                                            originalPlay.call(Lampa.Player, {
                                                url: Lampa.Torserver.stream(videoFile.path, json.hash, videoFile.id),
                                                title: savedTitle || videoFile.path,
                                                timeline: savedTimeline,
                                                card: savedCard,
                                                torrent_hash: json.hash
                                            });
                                            
                                            setTimeout(function() {
                                                isPidTorPlaying = false;
                                            }, 1000);
                                        } else if (retry < 15) {
                                            retry++;
                                            Lampa.Loading.setText('Индексация... (' + retry + '/15)');
                                            setTimeout(tryGetFiles, 1000);
                                        } else {
                                            Lampa.Loading.stop();
                                            Lampa.Noty.show('Не удалось получить файлы торрента');
                                            isPidTorPlaying = false;
                                        }
                                    })
                                    .catch(function() {
                                        if (retry < 15) {
                                            retry++;
                                            setTimeout(tryGetFiles, 1000);
                                        } else {
                                            Lampa.Loading.stop();
                                            Lampa.Noty.show('TorrServer не отвечает');
                                            isPidTorPlaying = false;
                                        }
                                    });
                                }
                                setTimeout(tryGetFiles, 500);
                            }
                        })
                        .catch(function() {
                            Lampa.Loading.stop();
                            Lampa.Noty.show('TorrServer недоступен');
                            isPidTorPlaying = false;
                        });
                    } catch(e) {
                        Lampa.Loading.stop();
                        Lampa.Noty.show('Ошибка: ' + e.message);
                        isPidTorPlaying = false;
                    }
                    return;
                }
                
                return originalPlay.call(this, data);
            };
        }
    } else {
        setTimeout(init, 50);
    }
})();";

        return Content(js, "application/javascript; charset=utf-8");
    }

    private ActionResult WarningTpl(string title, string original_title, byte serial, short s, 
        string warning_title, string warning_text, string action_url)
    {
        var mtpl = new MovieTpl(title, original_title);
        mtpl.Append(warning_title, action_url, voice_name: warning_text, quality: "0");
        return ContentTpl(mtpl);
    }
}