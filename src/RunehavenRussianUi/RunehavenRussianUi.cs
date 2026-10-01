using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using Il2CppInterop.Runtime.Injection;
using TMPro;
using UnityEngine;
using UguiText = UnityEngine.UI.Text;

namespace RunehavenRussianUi;

[BepInPlugin("ru.runehaven.localization", "Runehaven Russian UI", "0.1.13")]
public sealed class Plugin : BasePlugin
{
    public override void Load()
    {
        ClassInjector.RegisterTypeInIl2Cpp<UiTranslator>();
        AddComponent<UiTranslator>();
        Log.LogInfo("Runehaven Russian UI loaded.");
    }
}

public sealed class UiTranslator : MonoBehaviour
{
    private static readonly Dictionary<string, string> Translations = new(StringComparer.Ordinal)
    {
        ["New Game"] = "Новая игра",
        ["New game"] = "Новая игра",
        ["Load game"] = "Загрузить игру",
        ["LOAD GAME"] = "ЗАГРУЗИТЬ ИГРУ",
        ["Tutorial"] = "Обучение",
        ["Credits"] = "Создатели",
        ["Modding"] = "Моды",
        ["Create"] = "Создать",
        ["Enter text..."] = "Введите текст...",
        ["Would you like to continue?"] = "Хотите продолжить?",
        ["There are no mods here... :("] = "Здесь пока нет модов... :(",
        ["FEEDBACK"] = "ОБРАТНАЯ СВЯЗЬ",
        ["WISHLIST"] = "В ЖЕЛАЕМОЕ",
        ["BACK"] = "НАЗАД",
        ["Examine"] = "Осмотреть",
        ["Level up (Q)"] = "Повысить уровень (Q)",
        ["Unidentified"] = "Неопознано",
        ["Runebinder"] = "Заклинатель рун",
        ["Archer"] = "Лучник",
        ["Warrior"] = "Воин",
        ["Pick a class:"] = "Выберите класс:",
        ["Bonus damage when using magic weapons\nMajor attributes: intellect and rejuvenation"] = "Бонус к урону магическим оружием\nГлавные характеристики: интеллект и восстановление",
        ["Bonus damage when using melee weapons\nMajor attributes: strength and vitality"] = "Бонус к урону оружием ближнего боя\nГлавные характеристики: сила и живучесть",
        ["Bonus damage when using ranged weapons\nMajor attributes: agility and dexterity"] = "Бонус к урону оружием дальнего боя\nГлавные характеристики: ловкость и сноровка",
        ["Find an identify scroll to identify this rune."] = "Найдите свиток опознания, чтобы опознать эту руну.",
        ["You can only draw runes carried in your inventory or runebook."] = "Можно рисовать только руны из инвентаря или книги рун.",
        ["You can only draw runes carried"] = "Рисовать можно только руны",
        ["in your inventory or runebook."] = "из инвентаря или книги рун.",
        ["Rune of missile"] = "Руна снаряда",
        ["Creates a magic missile."] = "Создаёт магический снаряд.",
        ["Rune of split"] = "Руна разделения",
        ["Causes the spell to split in two."] = "Разделяет заклинание надвое.",
        ["Rune of knockback"] = "Руна отбрасывания",
        ["Increases spell knockback."] = "Увеличивает отбрасывание заклинания.",
        ["Rune of ring"] = "Руна кольца",
        ["Creates a ring of spells."] = "Создаёт кольцо заклинаний.",
        ["Rune of permeability"] = "Руна проницаемости",
        ["Passes through terrain."] = "Проходит сквозь препятствия.",
        ["Rune of lifesteal"] = "Руна похищения жизни",
        ["Spell damage heals the caster."] = "Урон заклинанием лечит заклинателя.",
        ["Rune of attraction"] = "Руна притяжения",
        ["Attracts nearby objects."] = "Притягивает ближайшие объекты.",
        ["Rune of longevity"] = "Руна долговечности",
        ["Increases spell lifetime."] = "Увеличивает длительность заклинания.",
        ["Rune of chaos"] = "Руна хаоса",
        ["Changes the spell in a chaotic manner."] = "Хаотично изменяет заклинание.",
        ["Rune of manasteal"] = "Руна похищения маны",
        ["Spell damage increases the caster's mana."] = "Урон заклинанием восполняет ману заклинателя.",
        ["Rune of air"] = "Руна воздуха",
        ["Air element. Creates a wind."] = "Стихия воздуха. Создаёт ветер.",
        ["Rune of radial damage"] = "Руна радиального урона",
        ["Deals radial damage to mobs upon destruction."] = "При разрушении наносит врагам урон по области.",
        ["Rune of harm"] = "Руна урона",
        ["Increases spell damage."] = "Увеличивает урон заклинания.",
        ["Rune of ascension"] = "Руна вознесения",
        ["Ascends the caster."] = "Поднимает заклинателя в воздух.",
        ["Rune of scattering"] = "Руна рассеивания",
        ["Creates a scatter of spells."] = "Создаёт россыпь заклинаний.",
        ["Rune of impulse"] = "Руна импульса",
        ["Creates a shockwave impulse around the caster."] = "Создаёт ударную волну вокруг заклинателя.",
        ["Rune of rain"] = "Руна дождя",
        ["Useful for creating missile rain."] = "Полезна для создания дождя из снарядов.",
        ["Rune of light"] = "Руна света",
        ["Emits light around the caster for a short duration."] = "Ненадолго освещает пространство вокруг заклинателя.",
        ["Rune of bounce"] = "Руна отскока",
        ["Makes the spell bounce"] = "Заставляет заклинание отскакивать.",
        ["Rune of earth"] = "Руна земли",
        ["Earth element. Increases knockback."] = "Стихия земли. Увеличивает отбрасывание.",
        ["Rune of fragment"] = "Руна осколков",
        ["The spell scatters into multiple spells when destroyed."] = "При разрушении заклинание распадается на несколько.",
        ["Rune of gravity"] = "Руна гравитации",
        ["Increases the gravity of the spell."] = "Увеличивает гравитацию заклинания.",
        ["Rune of slowness"] = "Руна замедления",
        ["Slows down the spell."] = "Замедляет заклинание.",
        ["Rune of elemental"] = "Руна элементаля",
        ["Summons an elemental."] = "Призывает элементаля.",
        ["Rune of repulsion"] = "Руна отталкивания",
        ["Repels nearby objects."] = "Отталкивает ближайшие объекты.",
        ["Rune of fire"] = "Руна огня",
        ["Fire element. Small chance of igniting the target."] = "Стихия огня. Небольшой шанс поджечь цель.",
        ["Rune of return"] = "Руна возврата",
        ["Spell returns back towards the caster."] = "Заклинание возвращается к заклинателю.",
        ["Rune of speed"] = "Руна скорости",
        ["Increases the spell movement speed."] = "Увеличивает скорость заклинания.",
        ["Rune of water"] = "Руна воды",
        ["Water element."] = "Стихия воды.",
        ["Rune of luck"] = "Руна удачи",
        ["Increases critical chance."] = "Увеличивает шанс критического удара.",
        ["Rune of seeking"] = "Руна поиска",
        ["Seeks towards enemies."] = "Направляет заклинание к врагам.",
        ["Rune of explosion"] = "Руна взрыва",
        ["Explode when destroyed."] = "Взрывается при разрушении.",
        ["Rune of momentum"] = "Руна импульса",
        ["Applies momentum continously."] = "Постоянно придаёт импульс.",
        ["Increases movement speed and jump height."] = "Увеличивает скорость движения и высоту прыжка.",
        ["Hold down Ctrl while drawing runes"] = "Удерживайте Ctrl при рисовании рун",
        ["Hold down the <b>Craft Spell</b> key while drawing runes."] = "Удерживайте клавишу <b>Создать заклинание</b> при рисовании рун.",
        ["Release the <b>Craft Spell</b> key to create a spell."] = "Отпустите клавишу <b>Создать заклинание</b>, чтобы создать заклинание.",
        ["Release ctrl to craft the spell"] = "Отпустите Ctrl, чтобы создать заклинание",
        ["Tip:"] = "Совет:",
        ["Generating world..."] = "Создание мира...",
        ["Main menu"] = "Главное меню",
        ["Continue playing"] = "Продолжить игру",
        ["Saving game..."] = "Сохранение игры...",
        ["Loading game..."] = "Загрузка игры...",
        ["Saves:"] = "Сохранения:",
        ["Delete"] = "Удалить",
        ["Play"] = "Играть",
        ["Buy"] = "Купить",
        ["Disabled HUD (press F12 to show it again)"] = "Интерфейс скрыт (нажмите F12, чтобы показать его)",
        ["Remove marker"] = "Убрать метку",
        ["Create a character:"] = "Создание персонажа:",
        ["Name thyself:"] = "Имя:",
        ["Confirm"] = "Подтвердить",
        ["Pick a difficulty:"] = "Выберите сложность:",
        ["Easy"] = "Легко",
        ["Normal"] = "Норма",
        ["Hard"] = "Сложно",
        ["Nightmare"] = "Кошмар",
        ["Small"] = "Мал.",
        ["Large"] = "Больш.",
        ["Create a world:"] = "Создание мира:",
        ["Tip: Adjust the world size to make the run longer or shorter."] = "Совет: измените размер мира, чтобы забег был длиннее или короче.",
        ["Pick up"] = "Подобрать",
        ["No spell has been created..."] = "Заклинание не создано...",
        ["Crafted spell!"] = "Заклинание создано!",
        ["Hold Ctrl to craft spell"] = "Удерживайте Ctrl: создать заклинание",
        ["Release Ctrl to craft the spell"] = "Отпустите Ctrl: создать заклинание",
        ["Press RMB to switch weapon to enchant"] = "Нажмите ПКМ, чтобы выбрать оружие для зачарования",
        ["Rope"] = "Трос",
        ["Throw on walls and ceilings\nto create a climbable rope."] = "Бросьте в стену или потолок,\nчтобы создать верёвку для подъёма.",
        ["Moon crystal"] = "Лунный кристалл",
        ["Increases maximum rune draw capacity.\nUseful for combining runes."] = "Увеличивает вместимость рисования рун.\nПолезен для сочетания рун.",
        ["Amulet of sorcery"] = "Амулет колдовства",
        ["Ring of aqua"] = "Кольцо воды",
        ["Portal crystal"] = "Портальный кристалл",
        ["It glows energetically."] = "Он ярко светится энергией.",
        ["Amulet of Mithridates"] = "Амулет Митридата",
        ["Ring of winds"] = "Кольцо ветров",
        ["Ring of strength"] = "Кольцо силы",
        ["Leather pants"] = "Кожаные штаны",
        ["Ring of flames"] = "Кольцо пламени",
        ["Ring of life"] = "Кольцо жизни",
        ["Amulet of protection"] = "Амулет защиты",
        ["Merchant"] = "Торговец",
        ["Blacksmith"] = "Кузнец",
        ["Talk"] = "Поговорить",
        ["Greetings!\nHave a look at my wares, you might find something of interest.."] = "Приветствую!\nВзгляните на мои товары — вдруг найдётся что-нибудь интересное.",
        ["I am the blacksmith of this village...\nTake a look around."] = "Я кузнец этой деревни...\nОсмотритесь.",
        ["Greetings traveller!\nHave a look at my wares, you might find something that suits you.."] = "Приветствую, путник!\nВзгляните на мои товары — возможно, найдётся что-то подходящее.",
        ["Greetings traveller!\nLet me show you my wares."] = "Приветствую, путник!\nПозвольте показать вам мои товары.",
        ["Scroll of fire missile"] = "Свиток огненного снаряда",
        ["Scroll of magic missile"] = "Свиток магического снаряда",
        ["Scroll of air missile"] = "Свиток воздушного снаряда",
        ["Scroll of water missile"] = "Свиток водяного снаряда",
        ["Scroll of earth missile"] = "Свиток земляного снаряда",
        ["Longsword"] = "Длинный меч",
        ["Dragonroot"] = "Корень дракона",
        ["Increases strength for a short duration."] = "Ненадолго увеличивает силу.",
        ["Khopesh"] = "Хопеш",
        ["Arcuballista"] = "Аркбаллиста",
        ["Coins"] = "Монеты",
        ["Vial of sage"] = "Флакон шалфея",
        ["Immediately restores mana."] = "Мгновенно восстанавливает ману.",
        ["Longbow"] = "Длинный лук",
        ["Cikoria"] = "Цикория",
        ["Regenerates mana for a short duration."] = "Ненадолго восстанавливает ману.",
        ["Xiphos"] = "Ксифос",
        ["Bomb"] = "Бомба",
        ["Bolts"] = "Болты",
        ["Foxglove"] = "Наперстянка",
        ["Increases dexterity for a short duration."] = "Ненадолго увеличивает сноровку.",
        ["Dagger"] = "Кинжал",
        ["Pickaxe"] = "Кирка",
        ["Cheirosiphon"] = "Хейросифон",
        ["Torch"] = "Факел",
        ["Sunblade"] = "Солнечный клинок",
        ["Arrows"] = "Стрелы",
        ["Flask of arnica"] = "Фляга арники",
        ["Immediately restores health."] = "Мгновенно восстанавливает здоровье.",
        ["Aloe vera"] = "Алоэ вера",
        ["Regenerates health for a short duration."] = "Ненадолго восстанавливает здоровье.",
        ["Glue bomb"] = "Клеевая бомба",
        ["Shortbow"] = "Короткий лук",
        ["Trident"] = "Трезубец",
        ["Horseman's pick"] = "Конная кирка",
        ["Glaive"] = "Глефа",
        ["Partisan"] = "Партизан",
        ["Silver rod"] = "Серебряный жезл",
        ["Tip: Most mobs have weaknesses and resistances against different elements."] = "Совет: у большинства врагов есть слабости и сопротивления разным стихиям.",
        ["Tip: Ropes can be used to climb out of holes."] = "Совет: тросы помогают выбираться из ям.",
        ["Tip: Melee and ranged weapons can also be enchanted with runes."] = "Совет: оружие ближнего и дальнего боя можно зачаровывать рунами.",
        ["Tip: Add runes to the runebook to free up inventory space."] = "Совет: добавляйте руны в книгу рун, чтобы освободить место в инвентаре.",
        ["Right-click to add to runebook."] = "Нажмите ПКМ, чтобы добавить в книгу рун.",
        ["Equip a magic weapon and hold Ctrl to draw this rune."] = "Возьмите магическое оружие и удерживайте Ctrl, чтобы начертить эту руну.",
        ["Added to rune book"] = "Добавлено в книгу рун",
        ["A scroll for identifying items.\nUse it by right-clicking and then\nleft-clicking an unidentified item."] = "Свиток для опознания предметов.\nНажмите ПКМ, затем ЛКМ по\nнеопознанному предмету.",
        ["Read"] = "Прочитать",
        ["Save & exit"] = "Сохранить и выйти",
        ["Hold Ctrl while holding a weapon to "] = "Удерживайте Ctrl с оружием, чтобы ",
        ["craft spells or enchant weapons."] = "создавать заклинания или зачаровывать оружие.",
        ["Hold Ctrl while holding a weapon to craft spells or enchant weapons."] = "Удерживайте Ctrl с оружием, чтобы создавать заклинания или зачаровывать оружие.",
        ["Press K to open the runebook."] = "Нажмите K, чтобы открыть книгу рун.",
        ["Press К to open the runebook."] = "Нажмите К, чтобы открыть книгу рун.",
        ["The number of runes you can combine is"] = "Число сочетаемых рун",
        ["determined by your rune capacity."] = "определяет вместимость рун.",
        ["Runes can be combined to craft complex"] = "Сочетайте руны, чтобы создавать сложные",
        ["spells by drawing multiple runes in sequence."] = "заклинания, рисуя руны по порядку.",
        ["Interact"] = "Взаимодействовать",
        ["Saved game!"] = "Игра сохранена!",
        ["Rune draw capacity increased!"] = "Вместимость рун увеличена!",
        ["A strange flying creature left me severely wounded today.\nI do not know how I ended up here. All I know is that it has something to do with that machine...\nIf that be the case, perhaps it can also lead me out of this dreadful place somehow.\n\n\nW. Blakely"] = "Сегодня странное летающее существо тяжело ранило меня.\nНе знаю, как я здесь оказался. Знаю лишь, что это как-то связано с той машиной...\nЕсли это так, возможно, она сумеет вывести меня из этого ужасного места.\n\n\nУ. Блейкли",
        ["Increases damage with ranged weapons."] = "Увеличивает урон оружием дальнего боя.",
        ["Increases magic damage."] = "Увеличивает магический урон.",
        ["Increases mana regeneration speed."] = "Увеличивает скорость восстановления маны.",
        ["Increases damage dealt with melee weapons."] = "Увеличивает урон оружием ближнего боя.",
        ["Increases maximum health points."] = "Увеличивает максимальное здоровье.",
        ["Increases maximum mana points."] = "Увеличивает максимальный запас маны.",
        ["Wooden wand"] = "Деревянный жезл",
        ["Wooden staff"] = "Деревянный посох",
        ["Wooden club"] = "Деревянная дубинка",
        ["Rusty sword"] = "Ржавый меч",
        ["Composite bow"] = "Композитный лук",
        ["Metal bow"] = "Металлический лук",
        ["Flamewood bow"] = "Лук из огнедрева",
        ["Automatic arcuballista"] = "Автоматическая аркбаллиста",
        ["Steel arcuballista"] = "Стальная аркбаллиста",
        ["Golden scepter"] = "Золотой скипетр",
        ["Feathered amulet"] = "Перьевой амулет",
        ["Bronze key"] = "Бронзовый ключ",
        ["Silver key"] = "Серебряный ключ",
        ["Gold key"] = "Золотой ключ",
        ["Identify scroll"] = "Свиток опознания",
        ["air rune"] = "руна воздуха",
        ["earth rune"] = "руна земли",
        ["fire rune"] = "руна огня",
        ["water rune"] = "руна воды",
        ["Aetherstone boots"] = "Эфирокаменные сапоги",
        ["Aetherstone chestplate"] = "Эфирокаменная кираса",
        ["Aetherstone greaves"] = "Эфирокаменные поножи",
        ["Aetherstone helmet"] = "Эфирокаменный шлем",
        ["Aetherstone sword"] = "Эфирокаменный меч",
        ["Bronze boots"] = "Бронзовые сапоги",
        ["Bronze chestplate"] = "Бронзовая кираса",
        ["Bronze greaves"] = "Бронзовые поножи",
        ["Bronze helmet"] = "Бронзовый шлем",
        ["Iron boots"] = "Железные сапоги",
        ["Iron chestplate"] = "Железная кираса",
        ["Iron greaves"] = "Железные поножи",
        ["Iron helmet"] = "Железный шлем",
        ["Leather boots"] = "Кожаные сапоги",
        ["Leather chestplate"] = "Кожаная кираса",
        ["Leather helmet"] = "Кожаный шлем",
        ["Steel boots"] = "Стальные сапоги",
        ["Steel chestplate"] = "Стальная кираса",
        ["Steel greaves"] = "Стальные поножи",
        ["Steel helmet"] = "Стальной шлем",
        ["Aquamarine staff"] = "Аквамариновый посох",
        ["Azurite staff"] = "Азуритовый посох",
        ["Bone staff"] = "Костяной посох",
        ["Carnelian staff"] = "Сердоликовый посох",
        ["Crystal staff"] = "Хрустальный посох",
        ["Jade staff"] = "Нефритовый посох",
        ["Moon staff"] = "Лунный посох",
        ["Obsidian staff"] = "Обсидиановый посох",
        ["Quartz staff"] = "Кварцевый посох",
        ["Sun staff"] = "Солнечный посох",
        ["Sunstone staff"] = "Солнечнокаменный посох",
        ["Temple staff"] = "Храмовый посох",
        ["Continue"] = "Продолжить",
        ["Resume"] = "Продолжить",
        ["Options"] = "Настройки",
        ["Settings"] = "Настройки",
        ["Quit"] = "Выйти",
        ["Exit"] = "Выход",
        ["Back"] = "Назад",
        ["Apply"] = "Применить",
        ["Cancel"] = "Отмена",
        ["Save"] = "Сохранить",
        ["Load"] = "Загрузить",
        ["Inventory"] = "Инвентарь",
        ["Character"] = "Персонаж",
        ["Stats"] = "Характеристики",
        ["Skills"] = "Навыки",
        ["Controls"] = "Управление",
        ["Audio"] = "Звук",
        ["Graphics"] = "Графика",
        ["Display"] = "Экран",
        ["Language"] = "Язык",
        ["Fullscreen"] = "Полный экран",
        ["Resolution"] = "Разрешение",
        ["Volume"] = "Громкость",
        ["Music"] = "Музыка",
        ["Sound Effects"] = "Звуковые эффекты",
        ["Master Volume"] = "Общая громкость",
        ["Health"] = "Здоровье",
        ["Mana"] = "Мана",
        ["Level"] = "Уровень",
        ["Experience"] = "Опыт",
        ["Gold"] = "Золото",
        ["Runebook"] = "Книга рун",
        ["Equip"] = "Экипировать",
        ["Drop"] = "Выбросить",
        ["Use"] = "Использовать",
        ["Close"] = "Закрыть",
        ["Yes"] = "Да",
        ["No"] = "Нет",
        ["On"] = "Вкл.",
        ["Off"] = "Выкл.",
    };

    private static readonly KeyValuePair<string, string>[] Fragments =
    {
        new("Coins:", "Монеты:"),
        new(" coins", " мон."),
        new("Bought ", "Куплено: "),
        new("You need ", "Нужно ещё "),
        new(" more coins to buy that", " монет, чтобы купить это."),
        new("Critical chance:", "Шанс крит. удара:"),
        new("Critical damage:", "Критический урон:"),
        new("Fire damage:", "Урон огнём:"),
        new("Ice damage:", "Урон льдом:"),
        new("Poison damage:", "Урон ядом:"),
        new("Magic damage:", "Магический урон:"),
        new("Mana cost:", "Стоимость маны:"),
        new("Weapon swing mode:", "Режим взмаха:"),
        new("Seed:", "Сид:"),
        new("World:", "Мир:"),
        new("Size:", "Размер:"),
        new("Beartraps:", "Капканы:"),
        new("Ravines:", "Ущелья:"),
        new("Mob count:", "Врагов:"),
        new("Random mob respawns:", "Респавн врагов:"),
        new("Item durability:", "Прочность предметов:"),
        new("Ceiling collapses:", "Обвалы потолка:"),
        new("Randomize runes:", "Случайные руны:"),
        new("Show tutorial info:", "Подсказки обучения:"),
        new("Permadeath:", "Одна жизнь:"),
        new("Automatic", "Авто"),
        new("Medium", "Средн."),
        new("Normal", "Норма"),
        new("Yes", "Да"),
        new("No", "Нет"),
        new("Off", "Выкл."),
        new("Bolts (", "Болты ("),
        new("Arrows (", "Стрелы ("),
        new("Rope (", "Трос ("),
        new("Run completed in:", "Забег завершён за:"),
        new("minutes and", "мин. и"),
        new("Mobs defeated:", "Побеждено врагов:"),
        new("Props thrown:", "Брошено предметов:"),
        new("Damage taken:", "Получено урона:"),
        new("Spells crafted:", "Создано заклинаний:"),
        new("Secrets found:", "Найдено секретов:"),
        new("Rune capacity:", "Руны:"),
        new("Skill points:", "Очки:"),
        new("Refresh rate:", "Частота обновления:"),
        new("Display mode:", "Режим экрана:"),
        new("Anti aliasing:", "Сглаживание:"),
        new("Field of view:", "Поле зрения:"),
        new("Color depth:", "Глубина цвета:"),
        new("Vertical sync:", "Вертикальная синхронизация:"),
        new("Downscaling:", "Масштабирование:"),
        new("Resolution:", "Разрешение:"),
        new("Knockback:", "Отбрасывание:"),
        new("Damage:", "Урон:"),
        new("Health:", "Здор.:"),
        new("Mana:", "Мана:"),
        new("Armor:", "Броня:"),
        new("Level:", "Ур.:") ,
        new("Exp:", "Опыт:"),
        new("Agility:", "Ловк. :"),
        new("Dexterity:", "Снор. :"),
        new("Intellect:", "Инт. :"),
        new("Rejuvenation:", "Восст. :"),
        new("Strength:", "Сила:"),
        new("Vitality:", "Жизнь:"),
        new("Willpower:", "Воля:"),
        new("Gamma:", "Гамма:"),
        new("Bloom:", "Свечение:"),
        new("Particles:", "Частицы:"),
        new("Range:", "Дальность:"),
        new("Speed:", "Скорость:"),
        new("Condition:", "Прочность:"),
        new("Item level:", "Уровень предмета:"),
        new("Required level:", "Требуемый уровень:"),
        new("Attack:", "Атака:"),
        new("Craft spell:", "Создание заклинания:"),
        new("Interact:", "Взаимодействие:"),
        new("Parry:", "Парирование:"),
        new("Character stats:", "Характеристики:"),
        new("Inventory:", "Инвентарь:"),
        new("Dual-wield:", "Вторая рука:"),
        new("Use health potion:", "Зелье здоровья:"),
        new("Use mana potion:", "Зелье маны:"),
        new("Jump:", "Прыжок:"),
        new("Crouch:", "Присесть:"),
        new("Drop item:", "Выбросить предмет:"),
        new("Fullscreen", "Полный экран"),
    };

    // Store entries include prices or quantities, so their text does not exactly
    // match an item title in the dictionary above. These fragments cover every
    // item type sold by the game's merchants, including randomized stock.
    private static readonly KeyValuePair<string, string>[] MerchantItemFragments =
    {
        new("Glue bomb", "Клеевая бомба"),
        new("Dragonroot", "Корень дракона"),
        new("Identify scroll", "Свиток опознания"),
        new("Vial of sage", "Флакон шалфея"),
        new("Flask of arnica", "Фляга арники"),
        new("Aloe vera", "Алоэ вера"),
        new("Foxglove", "Наперстянка"),
        new("Iron chestplate", "Железная кираса"),
        new("Iron greaves", "Железные поножи"),
        new("Iron helmet", "Железный шлем"),
        new("Bronze chestplate", "Бронзовая кираса"),
        new("Ring of life", "Кольцо жизни"),
        new("Silver rod", "Серебряный жезл"),
        new("Horseman's pick", "Конная кирка"),
        new("Longsword", "Длинный меч"),
        new("Khopesh", "Хопеш"),
        new("Partisan", "Партизан"),
        new("Arcuballista", "Аркбаллиста"),
        new("Shortbow", "Короткий лук"),
        new("Longbow", "Длинный лук"),
        new("Cheirosiphon", "Хейросифон"),
        new("Pickaxe", "Кирка"),
        new("Dagger", "Кинжал"),
        new("Trident", "Трезубец"),
        new("Glaive", "Глефа"),
        new("Bomb", "Бомба"),
        new("Bolts", "Болты"),
        new("Arrows", "Стрелы"),
        new("Torch", "Факел"),
    };

    private readonly HashSet<string> _reported = new(StringComparer.Ordinal);
    private float _nextScanTime;
    private string _untranslatedFile;

    public UiTranslator(IntPtr pointer) : base(pointer) { }

    private void Start()
    {
        _untranslatedFile = Path.Combine(Paths.ConfigPath, "RunehavenRussian.untranslated.txt");
        File.WriteAllText(_untranslatedFile, "# Untranslated UI strings observed by Runehaven Russian UI\n");
    }

    private void Update()
    {
        if (Time.unscaledTime < _nextScanTime)
            return;

        _nextScanTime = Time.unscaledTime + 0.5f;
        var textComponents = Resources.FindObjectsOfTypeAll<TMP_Text>();
        foreach (var text in textComponents)
        {
            if (text == null || string.IsNullOrWhiteSpace(text.text))
                continue;

            var current = text.text;
            var translated = Translate(current);
            if (!string.Equals(current, translated, StringComparison.Ordinal))
            {
                text.text = translated;
                if (IsCompactPanelLabel(current))
                    text.fontSize = Math.Min(text.fontSize, 12f);
            }
            else if (!Translations.ContainsValue(current) && _reported.Add(current) && _untranslatedFile is not null)
            {
                File.AppendAllText(_untranslatedFile, current.Replace("\r", string.Empty).Replace("\n", "\\n") + Environment.NewLine);
            }
        }

        // Inventory, equipment values and world prompts use Unity's older UI.Text
        // component rather than TextMeshPro, so they need a separate pass.
        var legacyTextComponents = Resources.FindObjectsOfTypeAll<UguiText>();
        foreach (var text in legacyTextComponents)
        {
            if (text == null || string.IsNullOrWhiteSpace(text.text))
                continue;

            var current = text.text;
            var translated = Translate(current);
            if (!string.Equals(current, translated, StringComparison.Ordinal))
            {
                text.text = translated;
                if (IsClassSelectionText(current))
                {
                    text.horizontalOverflow = HorizontalWrapMode.Overflow;
                    text.verticalOverflow = VerticalWrapMode.Overflow;
                }
            }
            else if (!Translations.ContainsValue(current) && _reported.Add(current) && _untranslatedFile is not null)
            {
                File.AppendAllText(_untranslatedFile, current.Replace("\r", string.Empty).Replace("\n", "\\n") + Environment.NewLine);
            }
        }
    }

    private static string Translate(string source)
    {
        if (Translations.TryGetValue(source, out var whole))
            return whole;

        var result = source;

        // Merchant offers are generated from an item title plus a price, for
        // example "Iron helmet: 12 coins". Reuse the complete item dictionary
        // here, rather than maintaining a fragile list for each shop table.
        if (source.Contains("coins", StringComparison.OrdinalIgnoreCase) ||
            source.StartsWith("Bought ", StringComparison.OrdinalIgnoreCase))
        {
            foreach (var replacement in Translations)
                result = result.Replace(replacement.Key, replacement.Value, StringComparison.OrdinalIgnoreCase);
        }

        foreach (var replacement in Fragments)
            result = result.Replace(replacement.Key, replacement.Value, StringComparison.Ordinal);
        foreach (var replacement in MerchantItemFragments)
            result = result.Replace(replacement.Key, replacement.Value, StringComparison.OrdinalIgnoreCase);
        return result;
    }

    // The character panel places labels and values in independent fixed-width fields.
    // Cyrillic uses the fallback TMP font, so compact labels and a smaller size prevent overlap.
    private static bool IsCompactPanelLabel(string source) =>
        source is "Runebinder" or "Health:" or "Mana:" or "Rune capacity:" or "Level:" or "Exp:" or
        "Skill points:" or "Agility:" or "Dexterity:" or "Intellect:" or "Rejuvenation:" or
        "Strength:" or "Vitality:" or "Willpower:";

    private static bool IsWorldCreationText(string source) =>
        source is "Create a character:" or "Name thyself:" or "Confirm" or "Pick a difficulty:" or
        "Easy" or "Normal" or "Hard" or "Nightmare" or "Small" or "Medium" or "Large" or
        "Create a world:" or "Yes" or "No" or "Off" or "Automatic" ||
        source.StartsWith("Weapon swing mode:", StringComparison.Ordinal) ||
        source.StartsWith("Seed:", StringComparison.Ordinal) ||
        source.StartsWith("World:", StringComparison.Ordinal) ||
        source.StartsWith("Size:", StringComparison.Ordinal) ||
        source.StartsWith("Beartraps:", StringComparison.Ordinal) ||
        source.StartsWith("Ravines:", StringComparison.Ordinal) ||
        source.StartsWith("Mob count:", StringComparison.Ordinal) ||
        source.StartsWith("Random mob respawns:", StringComparison.Ordinal) ||
        source.StartsWith("Item durability:", StringComparison.Ordinal) ||
        source.StartsWith("Ceiling collapses:", StringComparison.Ordinal) ||
        source.StartsWith("Randomize runes:", StringComparison.Ordinal) ||
        source.StartsWith("Show tutorial info:", StringComparison.Ordinal) ||
        source.StartsWith("Permadeath:", StringComparison.Ordinal);

    private static bool IsClassSelectionText(string source) =>
        source is "Pick a class:" or "Warrior" or "Archer" or "Runebinder";
}
