using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace KeyBV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TB_Path.Text = System.Windows.Forms.Application.StartupPath;
            CB_Mode.SelectedIndex = 0;
            CB_TeamSelect.SelectedIndex = 0;
            CB_TeamSelect2.SelectedIndex = 0;
            RTB.Text = "KeyBV - By Kaphotics and OmegaDonut\r\n\r\nhttp://projectpokemon.org/\r\n\r\nContact @ Forums or (preferably) IRC.";
        }

        public byte[] breakstream = new Byte[260 * 6];
        public string location, modestring, result;

        // Encryption Workings
        private static uint LCRNG(uint seed)
        {
            uint a = 0x41C64E6D;
            uint c = 0x00006073;

            seed = (seed * a + c) & 0xFFFFFFFF;
            return seed;
        }
        private uint getchecksum(byte[] pkx)
        {
            uint chk = 0;
            for (int i = 8; i < 232; i += 2) // Loop through the entire PKX
            {
                chk += (uint)(pkx[i] + pkx[i + 1] * 0x100);
            }
            return chk & 0xFFFF;
        }

        // Array Manipulation
        private byte[] unshufflearray(byte[] pkx, uint sv)
        {
            byte[] ekx = new Byte[260];
            for (int i = 0; i < 8; i++)
            {
                ekx[i] = pkx[i];
            }

            // Now to shuffle the blocks

            // Define Shuffle Order Structure
            var aloc = new byte[] { 0, 0, 0, 0, 0, 0, 1, 1, 2, 3, 2, 3, 1, 1, 2, 3, 2, 3, 1, 1, 2, 3, 2, 3 };
            var bloc = new byte[] { 1, 1, 2, 3, 2, 3, 0, 0, 0, 0, 0, 0, 2, 3, 1, 1, 3, 2, 2, 3, 1, 1, 3, 2 };
            var cloc = new byte[] { 2, 3, 1, 1, 3, 2, 2, 3, 1, 1, 3, 2, 0, 0, 0, 0, 0, 0, 3, 2, 3, 2, 1, 1 };
            var dloc = new byte[] { 3, 2, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 0, 0, 0, 0, 0, 0 };

            // Get Shuffle Order
            var shlog = new byte[] { aloc[sv], bloc[sv], cloc[sv], dloc[sv] };

            // UnShuffle Away!
            for (int b = 0; b < 4; b++)
            {
                for (int i = 0; i < 56; i++)
                {
                    ekx[8 + 56 * b + i] = pkx[8 + 56 * shlog[b] + i];
                }
            }

            // Fill the Battle Stats back
            if (pkx.Length > 232)
            {
                for (int i = 232; i < 260; i++)
                {
                    ekx[i] = pkx[i];
                }
            }
            return ekx;
        }
        private byte[] shufflearray(byte[] pkx, uint sv)
        {
            byte[] ekx = new Byte[260];
            for (int i = 0; i < 8; i++)
            {
                ekx[i] = pkx[i];
            }

            // Now to shuffle the blocks

            // Define Shuffle Order Structure
            var aloc = new byte[] { 0, 0, 0, 0, 0, 0, 1, 1, 2, 3, 2, 3, 1, 1, 2, 3, 2, 3, 1, 1, 2, 3, 2, 3 };
            var bloc = new byte[] { 1, 1, 2, 3, 2, 3, 0, 0, 0, 0, 0, 0, 2, 3, 1, 1, 3, 2, 2, 3, 1, 1, 3, 2 };
            var cloc = new byte[] { 2, 3, 1, 1, 3, 2, 2, 3, 1, 1, 3, 2, 0, 0, 0, 0, 0, 0, 3, 2, 3, 2, 1, 1 };
            var dloc = new byte[] { 3, 2, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 0, 0, 0, 0, 0, 0 };

            // Get Shuffle Order
            var shlog = new byte[] { aloc[sv], bloc[sv], cloc[sv], dloc[sv] };

            // Shuffle Away!
            for (int b = 0; b < 4; b++)
            {
                for (int i = 0; i < 56; i++)
                {
                    ekx[8 + 56 * shlog[b] + i] = pkx[8 + 56 * b + i];
                }
            }

            // Fill the Battle Stats back
            if (pkx.Length > 232)
            {
                for (int i = 232; i < 260; i++)
                {
                    ekx[i] = pkx[i];
                }
            }
            return ekx;
        }
        private byte[] decryptarray(byte[] ekx)
        {
            byte[] pkx = ekx;
            uint pv = (uint)ekx[0] + (uint)((ekx[1] << 8)) + (uint)((ekx[2]) << 16) + (uint)((ekx[3]) << 24);
            uint sv = (((pv & 0x3E000) >> 0xD) % 24);

            uint seed = pv;
            // Decrypt Blocks with RNG Seed
            for (int i = 8; i < 232; i += 2)
            {
                int pre = pkx[i] + ((pkx[i + 1]) << 8);
                seed = LCRNG(seed);
                int seedxor = (int)((seed) >> 16);
                int post = (pre ^ seedxor);
                pkx[i] = (byte)((post) & 0xFF);
                pkx[i + 1] = (byte)(((post) >> 8) & 0xFF);
            }
            // Deshuffle
            pkx = unshufflearray(pkx, sv);

            // Decrypt the Party Stats
            seed = pv;
            for (int i = 232; i < 260; i += 2)
            {
                int pre = pkx[i] + ((pkx[i + 1]) << 8);
                seed = LCRNG(seed);
                int seedxor = (int)((seed) >> 16);
                int post = (pre ^ seedxor);
                pkx[i] = (byte)((post) & 0xFF);
                pkx[i + 1] = (byte)(((post) >> 8) & 0xFF);
            }

            return pkx;
        }
        private byte[] encryptarray(byte[] pkx)
        {
            // Shuffle
            uint pv = (uint)pkx[0] + (uint)((pkx[1] << 8)) + (uint)((pkx[2]) << 16) + (uint)((pkx[3]) << 24);
            uint sv = (((pv & 0x3E000) >> 0xD) % 24);

            var encrypt_sv = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 12, 18, 13, 19, 8, 10, 14, 20, 16, 22, 9, 11, 15, 21, 17, 23 };

            sv = encrypt_sv[sv];

            byte[] ekx = shufflearray(pkx, sv);

            uint seed = pv;
            // Encrypt Blocks with RNG Seed
            for (int i = 8; i < 232; i += 2)
            {
                int pre = ekx[i] + ((ekx[i + 1]) << 8);
                seed = LCRNG(seed);
                int seedxor = (int)((seed) >> 16);
                int post = (pre ^ seedxor);
                ekx[i] = (byte)((post) & 0xFF);
                ekx[i + 1] = (byte)(((post) >> 8) & 0xFF);
            }

            // Encrypt the Party Stats
            seed = pv;
            for (int i = 232; i < 260; i += 2)
            {
                int pre = ekx[i] + ((ekx[i + 1]) << 8);
                seed = LCRNG(seed);
                int seedxor = (int)((seed) >> 16);
                int post = (pre ^ seedxor);
                ekx[i] = (byte)((post) & 0xFF);
                ekx[i + 1] = (byte)(((post) >> 8) & 0xFF);
            }

            // Done
            return ekx;
        }

        // Data Retrieval
        private string getspecies(int species)
        {
            string[] spectable = new string[] { "None", "Bulbasaur", "Ivysaur", "Venusaur", "Charmander", "Charmeleon", "Charizard", "Squirtle", "Wartortle", "Blastoise", "Caterpie", "Metapod", "Butterfree", "Weedle", "Kakuna", "Beedrill", "Pidgey", "Pidgeotto", "Pidgeot", "Rattata", "Raticate", "Spearow", "Fearow", "Ekans", "Arbok", "Pikachu", "Raichu", "Sandshrew", "Sandslash", "Nidoran♀", "Nidorina", "Nidoqueen", "Nidoran♂", "Nidorino", "Nidoking", "Clefairy", "Clefable", "Vulpix", "Ninetales", "Jigglypuff", "Wigglytuff", "Zubat", "Golbat", "Oddish", "Gloom", "Vileplume", "Paras", "Parasect", "Venonat", "Venomoth", "Diglett", "Dugtrio", "Meowth", "Persian", "Psyduck", "Golduck", "Mankey", "Primeape", "Growlithe", "Arcanine", "Poliwag", "Poliwhirl", "Poliwrath", "Abra", "Kadabra", "Alakazam", "Machop", "Machoke", "Machamp", "Bellsprout", "Weepinbell", "Victreebel", "Tentacool", "Tentacruel", "Geodude", "Graveler", "Golem", "Ponyta", "Rapidash", "Slowpoke", "Slowbro", "Magnemite", "Magneton", "Farfetchd", "Doduo", "Dodrio", "Seel", "Dewgong", "Grimer", "Muk", "Shellder", "Cloyster", "Gastly", "Haunter", "Gengar", "Onix", "Drowzee", "Hypno", "Krabby", "Kingler", "Voltorb", "Electrode", "Exeggcute", "Exeggutor", "Cubone", "Marowak", "Hitmonlee", "Hitmonchan", "Lickitung", "Koffing", "Weezing", "Rhyhorn", "Rhydon", "Chansey", "Tangela", "Kangaskhan", "Horsea", "Seadra", "Goldeen", "Seaking", "Staryu", "Starmie", "Mr. Mime", "Scyther", "Jynx", "Electabuzz", "Magmar", "Pinsir", "Tauros", "Magikarp", "Gyarados", "Lapras", "Ditto", "Eevee", "Vaporeon", "Jolteon", "Flareon", "Porygon", "Omanyte", "Omastar", "Kabuto", "Kabutops", "Aerodactyl", "Snorlax", "Articuno", "Zapdos", "Moltres", "Dratini", "Dragonair", "Dragonite", "Mewtwo", "Mew", "Chikorita", "Bayleef", "Meganium", "Cyndaquil", "Quilava", "Typhlosion", "Totodile", "Croconaw", "Feraligatr", "Sentret", "Furret", "Hoothoot", "Noctowl", "Ledyba", "Ledian", "Spinarak", "Ariados", "Crobat", "Chinchou", "Lanturn", "Pichu", "Cleffa", "Igglybuff", "Togepi", "Togetic", "Natu", "Xatu", "Mareep", "Flaaffy", "Ampharos", "Bellossom", "Marill", "Azumarill", "Sudowoodo", "Politoed", "Hoppip", "Skiploom", "Jumpluff", "Aipom", "Sunkern", "Sunflora", "Yanma", "Wooper", "Quagsire", "Espeon", "Umbreon", "Murkrow", "Slowking", "Misdreavus", "Unown", "Wobbuffet", "Girafarig", "Pineco", "Forretress", "Dunsparce", "Gligar", "Steelix", "Snubbull", "Granbull", "Qwilfish", "Scizor", "Shuckle", "Heracross", "Sneasel", "Teddiursa", "Ursaring", "Slugma", "Magcargo", "Swinub", "Piloswine", "Corsola", "Remoraid", "Octillery", "Delibird", "Mantine", "Skarmory", "Houndour", "Houndoom", "Kingdra", "Phanpy", "Donphan", "Porygon2", "Stantler", "Smeargle", "Tyrogue", "Hitmontop", "Smoochum", "Elekid", "Magby", "Miltank", "Blissey", "Raikou", "Entei", "Suicune", "Larvitar", "Pupitar", "Tyranitar", "Lugia", "Ho-Oh", "Celebi", "Treecko", "Grovyle", "Sceptile", "Torchic", "Combusken", "Blaziken", "Mudkip", "Marshtomp", "Swampert", "Poochyena", "Mightyena", "Zigzagoon", "Linoone", "Wurmple", "Silcoon", "Beautifly", "Cascoon", "Dustox", "Lotad", "Lombre", "Ludicolo", "Seedot", "Nuzleaf", "Shiftry", "Taillow", "Swellow", "Wingull", "Pelipper", "Ralts", "Kirlia", "Gardevoir", "Surskit", "Masquerain", "Shroomish", "Breloom", "Slakoth", "Vigoroth", "Slaking", "Nincada", "Ninjask", "Shedinja", "Whismur", "Loudred", "Exploud", "Makuhita", "Hariyama", "Azurill", "Nosepass", "Skitty", "Delcatty", "Sableye", "Mawile", "Aron", "Lairon", "Aggron", "Meditite", "Medicham", "Electrike", "Manectric", "Plusle", "Minun", "Volbeat", "Illumise", "Roselia", "Gulpin", "Swalot", "Carvanha", "Sharpedo", "Wailmer", "Wailord", "Numel", "Camerupt", "Torkoal", "Spoink", "Grumpig", "Spinda", "Trapinch", "Vibrava", "Flygon", "Cacnea", "Cacturne", "Swablu", "Altaria", "Zangoose", "Seviper", "Lunatone", "Solrock", "Barboach", "Whiscash", "Corphish", "Crawdaunt", "Baltoy", "Claydol", "Lileep", "Cradily", "Anorith", "Armaldo", "Feebas", "Milotic", "Castform", "Kecleon", "Shuppet", "Banette", "Duskull", "Dusclops", "Tropius", "Chimecho", "Absol", "Wynaut", "Snorunt", "Glalie", "Spheal", "Sealeo", "Walrein", "Clamperl", "Huntail", "Gorebyss", "Relicanth", "Luvdisc", "Bagon", "Shelgon", "Salamence", "Beldum", "Metang", "Metagross", "Regirock", "Regice", "Registeel", "Latias", "Latios", "Kyogre", "Groudon", "Rayquaza", "Jirachi", "Deoxys", "Turtwig", "Grotle", "Torterra", "Chimchar", "Monferno", "Infernape", "Piplup", "Prinplup", "Empoleon", "Starly", "Staravia", "Staraptor", "Bidoof", "Bibarel", "Kricketot", "Kricketune", "Shinx", "Luxio", "Luxray", "Budew", "Roserade", "Cranidos", "Rampardos", "Shieldon", "Bastiodon", "Burmy", "Wormadam", "Mothim", "Combee", "Vespiquen", "Pachirisu", "Buizel", "Floatzel", "Cherubi", "Cherrim", "Shellos", "Gastrodon", "Ambipom", "Drifloon", "Drifblim", "Buneary", "Lopunny", "Mismagius", "Honchkrow", "Glameow", "Purugly", "Chingling", "Stunky", "Skuntank", "Bronzor", "Bronzong", "Bonsly", "Mime Jr.", "Happiny", "Chatot", "Spiritomb", "Gible", "Gabite", "Garchomp", "Munchlax", "Riolu", "Lucario", "Hippopotas", "Hippowdon", "Skorupi", "Drapion", "Croagunk", "Toxicroak", "Carnivine", "Finneon", "Lumineon", "Mantyke", "Snover", "Abomasnow", "Weavile", "Magnezone", "Lickilicky", "Rhyperior", "Tangrowth", "Electivire", "Magmortar", "Togekiss", "Yanmega", "Leafeon", "Glaceon", "Gliscor", "Mamoswine", "Porygon-Z", "Gallade", "Probopass", "Dusknoir", "Froslass", "Rotom", "Uxie", "Mesprit", "Azelf", "Dialga", "Palkia", "Heatran", "Regigigas", "Giratina", "Cresselia", "Phione", "Manaphy", "Darkrai", "Shaymin", "Arceus", "Victini", "Snivy", "Servine", "Serperior", "Tepig", "Pignite", "Emboar", "Oshawott", "Dewott", "Samurott", "Patrat", "Watchog", "Lillipup", "Herdier", "Stoutland", "Purrloin", "Liepard", "Pansage", "Simisage", "Pansear", "Simisear", "Panpour", "Simipour", "Munna", "Musharna", "Pidove", "Tranquill", "Unfezant", "Blitzle", "Zebstrika", "Roggenrola", "Boldore", "Gigalith", "Woobat", "Swoobat", "Drilbur", "Excadrill", "Audino", "Timburr", "Gurdurr", "Conkeldurr", "Tympole", "Palpitoad", "Seismitoad", "Throh", "Sawk", "Sewaddle", "Swadloon", "Leavanny", "Venipede", "Whirlipede", "Scolipede", "Cottonee", "Whimsicott", "Petilil", "Lilligant", "Basculin", "Sandile", "Krokorok", "Krookodile", "Darumaka", "Darmanitan", "Maractus", "Dwebble", "Crustle", "Scraggy", "Scrafty", "Sigilyph", "Yamask", "Cofagrigus", "Tirtouga", "Carracosta", "Archen", "Archeops", "Trubbish", "Garbodor", "Zorua", "Zoroark", "Minccino", "Cinccino", "Gothita", "Gothorita", "Gothitelle", "Solosis", "Duosion", "Reuniclus", "Ducklett", "Swanna", "Vanillite", "Vanillish", "Vanilluxe", "Deerling", "Sawsbuck", "Emolga", "Karrablast", "Escavalier", "Foongus", "Amoonguss", "Frillish", "Jellicent", "Alomomola", "Joltik", "Galvantula", "Ferroseed", "Ferrothorn", "Klink", "Klang", "Klinklang", "Tynamo", "Eelektrik", "Eelektross", "Elgyem", "Beheeyem", "Litwick", "Lampent", "Chandelure", "Axew", "Fraxure", "Haxorus", "Cubchoo", "Beartic", "Cryogonal", "Shelmet", "Accelgor", "Stunfisk", "Mienfoo", "Mienshao", "Druddigon", "Golett", "Golurk", "Pawniard", "Bisharp", "Bouffalant", "Rufflet", "Braviary", "Vullaby", "Mandibuzz", "Heatmor", "Durant", "Deino", "Zweilous", "Hydreigon", "Larvesta", "Volcarona", "Cobalion", "Terrakion", "Virizion", "Tornadus", "Thundurus", "Reshiram", "Zekrom", "Landorus", "Kyurem", "Keldeo", "Meloetta", "Genesect", "Chespin", "Quilladin", "Chesnaught", "Fennekin", "Braixen", "Delphox", "Froakie", "Frogadier", "Greninja", "Bunnelby", "Diggersby", "Fletchling", "Fletchinder", "Talonflame", "Scatterbug", "Spewpa", "Vivillon", "Litleo", "Pyroar", "Flabébé", "Floette", "Florges", "Skiddo", "Gogoat", "Pancham", "Pangoro", "Furfrou", "Espurr", "Meowstic", "Honedge", "Doublade", "Aegislash", "Spritzee", "Aromatisse", "Swirlix", "Slurpuff", "Inkay", "Malamar", "Binacle", "Barbaracle", "Skrelp", "Dragalge", "Clauncher", "Clawitzer", "Helioptile", "Heliolisk", "Tyrunt", "Tyrantrum", "Amaura", "Aurorus", "Sylveon", "Hawlucha", "Dedenne", "Carbink", "Goomy", "Sliggoo", "Goodra", "Klefki", "Phantump", "Trevenant", "Pumpkaboo", "Gourgeist", "Bergmite", "Avalugg", "Noibat", "Noivern", "Xerneas", "Yveltal", "Zygarde", "Diancie", "Hoopa", "Volcanion" };
            try
            {
                return spectable[species];
            }
            catch { return "Error"; }
        }
        private string getmove(byte[] buff, int offset)
        {
            string[] movetable = new string[] { "", "Pound", "Karate Chop", "Double Slap", "Comet Punch", "Mega Punch", "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch", "Scratch", "Vice Grip", "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust", "Wing Attack", "Whirlwind", "Fly", "Bind", "Slam", "Vine Whip", "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick", "Sand Attack", "Headbutt", "Horn Attack", "Fury Attack", "Horn Drill", "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash", "Double-Edge", "Tail Whip", "Poison Sting", "Twineedle", "Pin Missile", "Leer", "Bite", "Growl", "Roar", "Sing", "Supersonic", "Sonic Boom", "Disable", "Acid", "Ember", "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf", "Ice Beam", "Blizzard", "Psybeam", "Bubble Beam", "Aurora Beam", "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick", "Counter", "Seismic Toss", "Strength", "Absorb", "Mega Drain", "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder", "Stun Spore", "Sleep Powder", "Petal Dance", "String Shot", "Dragon Rage", "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder", "Rock Throw", "Earthquake", "Fissure", "Dig", "Toxic", "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility", "Quick Attack", "Rage", "Teleport", "Night Shade", "Mimic", "Screech", "Double Team", "Recover", "Harden", "Minimize", "Smokescreen", "Confuse Ray", "Withdraw", "Defense Curl", "Barrier", "Light Screen", "Haze", "Reflect", "Focus Energy", "Bide", "Metronome", "Mirror Move", "Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge", "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift", "Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis", "Soft-Boiled", "High Jump Kick", "Glare", "Dream Eater", "Poison Gas", "Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform", "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave", "Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen", "Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute", "Struggle", "Sketch", "Triple Kick", "Thief", "Spider Web", "Mind Reader", "Nightmare", "Flame Wheel", "Snore", "Curse", "Flail", "Conversion 2", "Aeroblast", "Cotton Spore", "Reversal", "Spite", "Powder Snow", "Protect", "Mach Punch", "Scary Face", "Feint Attack", "Sweet Kiss", "Belly Drum", "Sludge Bomb", "Mud-Slap", "Octazooka", "Spikes", "Zap Cannon", "Foresight", "Destiny Bond", "Perish Song", "Icy Wind", "Detect", "Bone Rush", "Lock-On", "Outrage", "Sandstorm", "Giga Drain", "Endure", "Charm", "Rollout", "False Swipe", "Swagger", "Milk Drink", "Spark", "Fury Cutter", "Steel Wing", "Mean Look", "Attract", "Sleep Talk", "Heal Bell", "Return", "Present", "Frustration", "Safeguard", "Pain Split", "Sacred Fire", "Magnitude", "Dynamic Punch", "Megahorn", "Dragon Breath", "Baton Pass", "Encore", "Pursuit", "Rapid Spin", "Sweet Scent", "Iron Tail", "Metal Claw", "Vital Throw", "Morning Sun", "Synthesis", "Moonlight", "Hidden Power", "Cross Chop", "Twister", "Rain Dance", "Sunny Day", "Crunch", "Mirror Coat", "Psych Up", "Extreme Speed", "Ancient Power", "Shadow Ball", "Future Sight", "Rock Smash", "Whirlpool", "Beat Up", "Fake Out", "Uproar", "Stockpile", "Spit Up", "Swallow", "Heat Wave", "Hail", "Torment", "Flatter", "Will-O-Wisp", "Memento", "Facade", "Focus Punch", "Smelling Salts", "Follow Me", "Nature Power", "Charge", "Taunt", "Helping Hand", "Trick", "Role Play", "Wish", "Assist", "Ingrain", "Superpower", "Magic Coat", "Recycle", "Revenge", "Brick Break", "Yawn", "Knock Off", "Endeavor", "Eruption", "Skill Swap", "Imprison", "Refresh", "Grudge", "Snatch", "Secret Power", "Dive", "Arm Thrust", "Camouflage", "Tail Glow", "Luster Purge", "Mist Ball", "Feather Dance", "Teeter Dance", "Blaze Kick", "Mud Sport", "Ice Ball", "Needle Arm", "Slack Off", "Hyper Voice", "Poison Fang", "Crush Claw", "Blast Burn", "Hydro Cannon", "Meteor Mash", "Astonish", "Weather Ball", "Aromatherapy", "Fake Tears", "Air Cutter", "Overheat", "Odor Sleuth", "Rock Tomb", "Silver Wind", "Metal Sound", "Grass Whistle", "Tickle", "Cosmic Power", "Water Spout", "Signal Beam", "Shadow Punch", "Extrasensory", "Sky Uppercut", "Sand Tomb", "Sheer Cold", "Muddy Water", "Bullet Seed", "Aerial Ace", "Icicle Spear", "Iron Defense", "Block", "Howl", "Dragon Claw", "Frenzy Plant", "Bulk Up", "Bounce", "Mud Shot", "Poison Tail", "Covet", "Volt Tackle", "Magical Leaf", "Water Sport", "Calm Mind", "Leaf Blade", "Dragon Dance", "Rock Blast", "Shock Wave", "Water Pulse", "Doom Desire", "Psycho Boost", "Roost", "Gravity", "Miracle Eye", "Wake-Up Slap", "Hammer Arm", "Gyro Ball", "Healing Wish", "Brine", "Natural Gift", "Feint", "Pluck", "Tailwind", "Acupressure", "Metal Burst", "U-turn", "Close Combat", "Payback", "Assurance", "Embargo", "Fling", "Psycho Shift", "Trump Card", "Heal Block", "Wring Out", "Power Trick", "Gastro Acid", "Lucky Chant", "Me First", "Copycat", "Power Swap", "Guard Swap", "Punishment", "Last Resort", "Worry Seed", "Sucker Punch", "Toxic Spikes", "Heart Swap", "Aqua Ring", "Magnet Rise", "Flare Blitz", "Force Palm", "Aura Sphere", "Rock Polish", "Poison Jab", "Dark Pulse", "Night Slash", "Aqua Tail", "Seed Bomb", "Air Slash", "X-Scissor", "Bug Buzz", "Dragon Pulse", "Dragon Rush", "Power Gem", "Drain Punch", "Vacuum Wave", "Focus Blast", "Energy Ball", "Brave Bird", "Earth Power", "Switcheroo", "Giga Impact", "Nasty Plot", "Bullet Punch", "Avalanche", "Ice Shard", "Shadow Claw", "Thunder Fang", "Ice Fang", "Fire Fang", "Shadow Sneak", "Mud Bomb", "Psycho Cut", "Zen Headbutt", "Mirror Shot", "Flash Cannon", "Rock Climb", "Defog", "Trick Room", "Draco Meteor", "Discharge", "Lava Plume", "Leaf Storm", "Power Whip", "Rock Wrecker", "Cross Poison", "Gunk Shot", "Iron Head", "Magnet Bomb", "Stone Edge", "Captivate", "Stealth Rock", "Grass Knot", "Chatter", "Judgment", "Bug Bite", "Charge Beam", "Wood Hammer", "Aqua Jet", "Attack Order", "Defend Order", "Heal Order", "Head Smash", "Double Hit", "Roar of Time", "Spacial Rend", "Lunar Dance", "Crush Grip", "Magma Storm", "Dark Void", "Seed Flare", "Ominous Wind", "Shadow Force", "Hone Claws", "Wide Guard", "Guard Split", "Power Split", "Wonder Room", "Psyshock", "Venoshock", "Autotomize", "Rage Powder", "Telekinesis", "Magic Room", "Smack Down", "Storm Throw", "Flame Burst", "Sludge Wave", "Quiver Dance", "Heavy Slam", "Synchronoise", "Electro Ball", "Soak", "Flame Charge", "Coil", "Low Sweep", "Acid Spray", "Foul Play", "Simple Beam", "Entrainment", "After You", "Round", "Echoed Voice", "Chip Away", "Clear Smog", "Stored Power", "Quick Guard", "Ally Switch", "Scald", "Shell Smash", "Heal Pulse", "Hex", "Sky Drop", "Shift Gear", "Circle Throw", "Incinerate", "Quash", "Acrobatics", "Reflect Type", "Retaliate", "Final Gambit", "Bestow", "Inferno", "Water Pledge", "Fire Pledge", "Grass Pledge", "Volt Switch", "Struggle Bug", "Bulldoze", "Frost Breath", "Dragon Tail", "Work Up", "Electroweb", "Wild Charge", "Drill Run", "Dual Chop", "Heart Stamp", "Horn Leech", "Sacred Sword", "Razor Shell", "Heat Crash", "Leaf Tornado", "Steamroller", "Cotton Guard", "Night Daze", "Psystrike", "Tail Slap", "Hurricane", "Head Charge", "Gear Grind", "Searing Shot", "Techno Blast", "Relic Song", "Secret Sword", "Glaciate", "Bolt Strike", "Blue Flare", "Fiery Dance", "Freeze Shock", "Ice Burn", "Snarl", "Icicle Crash", "V-create", "Fusion Flare", "Fusion Bolt", "Flying Press", "Mat Block", "Belch", "Rototiller", "Sticky Web", "Fell Stinger", "Phantom Force", "Trick-or-Treat", "Noble Roar", "Ion Deluge", "Parabolic Charge", "Forest's Curse", "Petal Blizzard", "Freeze-Dry", "Disarming Voice", "Parting Shot", "Topsy-Turvy", "Draining Kiss", "Crafty Shield", "Flower Shield", "Grassy Terrain", "Misty Terrain", "Electrify", "Play Rough", "Fairy Wind", "Moonblast", "Boomburst", "Fairy Lock", "King's Shield", "Play Nice", "Confide", "'-591-", "'-592-", "'-593-", "Water Shuriken", "Mystical Fire", "Spiky Shield", "Aromatic Mist", "Eerie Impulse", "Venom Drench", "Powder", "Geomancy", "Magnetic Flux", "Happy Hour", "Electric Terrain", "Dazzling Gleam", "Celebrate", "'-607-", "Baby-Doll Eyes", "Nuzzle", "'-610-", "Infestation", "Power-Up Punch", "Oblivion Wing", "'-614-", "'-615-", "Land's Wrath", "'-617", "'-618-", "'-619-", "'-620-", };
            try
            {
                int move = buff[offset] + buff[offset + 1] * 0x100;
                return movetable[move];
            }
            catch { return "Error"; }
        }
        private string getivs(byte[] buff, uint sv)
        {
            int IV32 = buff[0x77] * 0x1000000 + buff[0x76] * 0x10000 + buff[0x75] * 0x100 + buff[0x74];
            int HP_IV = IV32 & 0x1F;
            int ATK_IV = (IV32 >> 5) & 0x1F;
            int DEF_IV = (IV32 >> 10) & 0x1F;
            int SPE_IV = (IV32 >> 15) & 0x1F;
            int SPA_IV = (IV32 >> 20) & 0x1F;
            int SPD_IV = (IV32 >> 25) & 0x1F;

            string ivs = "";
            ivs += HP_IV.ToString("00") + ".";
            ivs += ATK_IV.ToString("00") + ".";
            ivs += DEF_IV.ToString("00") + ".";
            ivs += SPA_IV.ToString("00") + ".";
            ivs += SPD_IV.ToString("00") + ".";
            ivs += SPE_IV.ToString("00");

            int isegg = (IV32 >> 30) & 1;
            if (true)
            {
                ivs += " {" + sv.ToString("0000") + "}";
            }
            return ivs;
        }
        private string getivs2(byte[] buff, uint sv)
        {
            int IV32 = buff[0x77] * 0x1000000 + buff[0x76] * 0x10000 + buff[0x75] * 0x100 + buff[0x74];
            int HP_IV = IV32 & 0x1F;
            int ATK_IV = (IV32 >> 5) & 0x1F;
            int DEF_IV = (IV32 >> 10) & 0x1F;
            int SPE_IV = (IV32 >> 15) & 0x1F;
            int SPA_IV = (IV32 >> 20) & 0x1F;
            int SPD_IV = (IV32 >> 25) & 0x1F;

            string ivs = "";
            ivs += HP_IV.ToString("00") + ".";
            ivs += ATK_IV.ToString("00") + ".";
            ivs += DEF_IV.ToString("00") + ".";
            ivs += SPA_IV.ToString("00") + ".";
            ivs += SPD_IV.ToString("00") + ".";
            ivs += SPE_IV.ToString("00");

            int isegg = (IV32 >> 30) & 1;
            if (true)
            {
                ivs += " | " + sv.ToString("0000");
            }
            //else
            //{
            //    // Not an Egg. Return TSV instead.
            //    uint TID = (uint)(buff[0x0C] + buff[0x0D] * 0x100);
            //    uint SID = (uint)(buff[0x0E] + buff[0x0F] * 0x100);
            //    uint TSV = (TID ^ SID) >> 4;

            //    ivs += " (" + TSV.ToString("0000") + ")";
            //}
            return ivs;
        }
        private string getivs3(byte[] buff)
        {
            int IV32 = buff[0x77] * 0x1000000 + buff[0x76] * 0x10000 + buff[0x75] * 0x100 + buff[0x74];
            int HP_IV = IV32 & 0x1F;
            int ATK_IV = (IV32 >> 5) & 0x1F;
            int DEF_IV = (IV32 >> 10) & 0x1F;
            int SPE_IV = (IV32 >> 15) & 0x1F;
            int SPA_IV = (IV32 >> 20) & 0x1F;
            int SPD_IV = (IV32 >> 25) & 0x1F;

            string ivs = "";
            ivs += HP_IV.ToString("00") + ",";
            ivs += ATK_IV.ToString("00") + ",";
            ivs += DEF_IV.ToString("00") + ",";
            ivs += SPA_IV.ToString("00") + ",";
            ivs += SPD_IV.ToString("00") + ",";
            ivs += SPE_IV.ToString("00");

            return ivs;
        }
        private string getevs(byte[] buff)
        {
            int HP_EV = buff[0x1E];
            int ATK_EV = buff[0x1F];
            int DEF_EV = buff[0x20];
            int SPE_EV = buff[0x21];
            int SPA_EV = buff[0x22];
            int SPD_EV = buff[0x23];

            string evs = "";
            evs += HP_EV.ToString() + ",";
            evs += ATK_EV.ToString() + ",";
            evs += DEF_EV.ToString() + ",";
            evs += SPA_EV.ToString() + ",";
            evs += SPD_EV.ToString() + ",";
            evs += SPE_EV.ToString();

            return evs;
        }
        private string getnature(byte[] buff)
        {
            int nature = buff[0x1C];
            string[] nattable = new string[] { "Hardy", "Lonely", "Brave", "Adamant", "Naughty", "Bold", "Docile", "Relaxed", "Impish", "Lax", "Timid", "Hasty", "Serious", "Jolly", "Naive", "Modest", "Mild", "Quiet", "Bashful", "Rash", "Calm", "Gentle", "Sassy", "Careful", "Quirky" };
            return nattable[nature];
        }
        private string getgender(byte[] buff)
        {
            string g = "";
            int genderflag = (buff[0x1D] >> 1) & 0x3;
            if (genderflag == 0)
            {
                // Gender = Male
                g = " (M)";
            }
            else if (genderflag == 1)
            {
                // Gender = Female
                g = " (F)";
            }
            else { g = ""; }
            return g;
        }
        private string getability(byte[] buff)
        {
            int ability = buff[0x14];
            string[] abiltable = new string[] { "None", "Stench", "Drizzle", "Speed Boost", "Battle Armor", "Sturdy", "Damp", "Limber", "Sand Veil", "Static", "Volt Absorb", "Water Absorb", "Oblivious", "Cloud Nine", "Compound Eyes", "Insomnia", "Color Change", "Immunity", "Flash Fire", "Shield Dust", "Own Tempo", "Suction Cups", "Intimidate", "Shadow Tag", "Rough Skin", "Wonder Guard", "Levitate", "Effect Spore", "Synchronize", "Clear Body", "Natural Cure", "Lightning Rod", "Serene Grace", "Swift Swim", "Chlorophyll", "Illuminate", "Trace", "Huge Power", "Poison Point", "Inner Focus", "Magma Armor", "Water Veil", "Magnet Pull", "Soundproof", "Rain Dish", "Sand Stream", "Pressure", "Thick Fat", "Early Bird", "Flame Body", "Run Away", "Keen Eye", "Hyper Cutter", "Pickup", "Truant", "Hustle", "Cute Charm", "Plus", "Minus", "Forecast", "Sticky Hold", "Shed Skin", "Guts", "Marvel Scale", "Liquid Ooze", "Overgrow", "Blaze", "Torrent", "Swarm", "Rock Head", "Drought", "Arena Trap", "Vital Spirit", "White Smoke", "Pure Power", "Shell Armor", "Air Lock", "Tangled Feet", "Motor Drive", "Rivalry", "Steadfast", "Snow Cloak", "Gluttony", "Anger Point", "Unburden", "Heatproof", "Simple", "Dry Skin", "Download", "Iron Fist", "Poison Heal", "Adaptability", "Skill Link", "Hydration", "Solar Power", "Quick Feet", "Normalize", "Sniper", "Magic Guard", "No Guard", "Stall", "Technician", "Leaf Guard", "Klutz", "Mold Breaker", "Super Luck", "Aftermath", "Anticipation", "Forewarn", "Unaware", "Tinted Lens", "Filter", "Slow Start", "Scrappy", "Storm Drain", "Ice Body", "Solid Rock", "Snow Warning", "Honey Gather", "Frisk", "Reckless", "Multitype", "Flower Gift", "Bad Dreams", "Pickpocket", "Sheer Force", "Contrary", "Unnerve", "Defiant", "Defeatist", "Cursed Body", "Healer", "Friend Guard", "Weak Armor", "Heavy Metal", "Light Metal", "Multiscale", "Toxic Boost", "Flare Boost", "Harvest", "Telepathy", "Moody", "Overcoat", "Poison Touch", "Regenerator", "Big Pecks", "Sand Rush", "Wonder Skin", "Analytic", "Illusion", "Imposter", "Infiltrator", "Mummy", "Moxie", "Justified", "Rattled", "Magic Bounce", "Sap Sipper", "Prankster", "Sand Force", "Iron Barbs", "Zen Mode", "Victory Star", "Turboblaze", "Teravolt", "Aroma Veil", "Flower Veil", "Cheek Pouch", "Protean", "Fur Coat", "Magician", "Bulletproof", "Competitive", "Strong Jaw", "Refrigerate", "Sweet Veil", "Stance Change", "Gale Wings", "Mega Launcher", "Grass Pelt", "Symbiosis", "Tough Claws", "Pixilate", "Gooey", "-184-", "-185-", "Dark Aura", "Fairy Aura", "Aura Break", "-189-", };
            return abiltable[ability];
        }
        private string bytes2text(byte[] buff, int o)
        {
            string charstring;
            charstring = ((char)(buff[o] + 0x100 * buff[o + 1])).ToString();
            for (int i = 1; i <= 12; i++)
            {
                int val = buff[o + 2 * i] + 0x100 * buff[o + 2 * i + 1];
                if (val != 0)
                {
                    charstring += ((char)(val)).ToString();
                }
            }
            return charstring;
        }
        private string getTSV(byte[] buff)
        {
            uint TID = (uint)(buff[0x0C] + buff[0x0D] * 0x100);
            uint SID = (uint)(buff[0x0E] + buff[0x0F] * 0x100);
            uint TSV = (TID ^ SID) >> 4;
            return TSV.ToString("0000");
        }
        private string getball(byte[] buff)
        {
            string[] balltable = new string[] { "Poke Ball (Default)", "Master Ball", "Ultra Ball", "Great Ball", "Poke Ball", "Safari Ball", "Net Ball", "Dive Ball", "Nest Ball", "Repeat Ball", "Timer Ball", "Luxury Ball", "Premier Ball", "Dusk Ball", "Heal Ball", "Quick Ball", "Cherish Ball", "Fast Ball", "Level Ball", "Lure Ball", "Heavy Ball", "Love Ball", "Friend Ball", "Moon Ball", "Comp Ball", "Dream Ball" };

            int ball = buff[0xDC];

            try
            {
                return balltable[ball];
            }
            catch { return "Error"; }
        }
        
        // Load Handling
        private void openfile(object sender, EventArgs e)
        {
            Button[] B_ARRAY = { B_V1, B_V2, B_Video, B_Key };
            TextBox[] T_ARRAY = { TB_V1, TB_V2, TB_Video, TB_Key };
            // Get index of triggering Button
            int index = Array.IndexOf(B_ARRAY, sender as Button);

            // Open File
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "";
            if (index == 3)
            {
                openfile.Filter = "Keystream|*.bin";
            }

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                string path = openfile.FileName;
                byte[] input = File.ReadAllBytes(path);

                T_ARRAY[index].Text = path;
            }

            // Enable Functions if Text Boxes are Filled
            bool tab1 = ((TB_V1.Text != "") && (TB_V2.Text != ""));
                B_Break.Enabled = tab1;

            bool tab2 = ((TB_Video.Text != "") && (TB_Key.Text != ""));
                B_Dump.Enabled = tab2;
                CB_Mode.Enabled = tab2;
                CHK_PK6.Enabled = tab2;
        }

        // Offset Handling
        private int getoffset(ComboBox sender)
        {
            int[] osa = { 0x4E18, 0x5438 };
            return osa[sender.SelectedIndex];
        }

        // Tab 1
        private void dobreak(object sender, EventArgs e)
        {
            // Get Videos
            byte[] video1 = File.ReadAllBytes(TB_V1.Text);
            byte[] video2 = File.ReadAllBytes(TB_V2.Text);

            // Idiot Checking
            if (video1.Length != 28256)
            {
                MessageBox.Show("Video 1 Filesize Incorrect.\r\nCurrent Size: " + video1.Length + "\r\nRequired Length: 28256", "Error");
                RTB.Text = "Fix Video 1 size -- load the correct file please :(";
            }
            else if (video2.Length != 28256)
            {
                MessageBox.Show("Video 2 Filesize Incorrect.\r\nCurrent Size: " + video2.Length + "\r\nRequired Length: 28256", "Error");
                RTB.Text = "Fix Video 2 size -- load the correct file please :(";
            }
            else // Do Trick
            {

                // Fill our Keystream
                int offset = getoffset(CB_TeamSelect); // Begin EKX Data
                Array.Copy(video1, offset, breakstream, 0, 260 * 6);

                // XOR them together at party offset
                byte[] xorstream = new Byte[260 * 6];
                for (int i = 0; i < (260 * 6); i++)
                {
                    xorstream[i] = (byte)(breakstream[i] ^ video2[i + offset]);
                }

                // Get Encrypted Zeroes
                byte[] ezeros = encryptarray(new Byte[260]);

                // Retrieve EKX_1's data
                byte[] ekx1 = new Byte[260];
                for (int i = 0; i < (260); i++)
                {
                    ekx1[i] = (byte)(xorstream[i + 260] ^ ezeros[i]);
                }

                // Rectify Keystream
                for (int i = 0; i < 260; i++)
                {
                    breakstream[i] = (byte)(breakstream[i] ^ ekx1[i]);
                }
                for (int i = 260; i < 260 * 6; i++)
                {
                    breakstream[i] = (byte)(breakstream[i] ^ ezeros[i % 260]);
                }
                // Finished, allow dumping of breakstream
                B_KS.Enabled = true;
                RTB.Text = "Success\r\nCan now dump the keystream.";
            }
        }
        private void dumpkey(object sender, EventArgs e)
        {
            SaveFileDialog savekey = new SaveFileDialog();
            savekey.Filter = "Keystream|*.bin";
            string team = "";
            if (CB_TeamSelect.SelectedIndex == 0)
            {
                team = "My Team";
            }
            else
            {
                team = "Opponent's Team";
            }
            savekey.FileName = "BV Key - " + team + ".bin";

            if (savekey.ShowDialog() == DialogResult.OK)
            {
                string path = savekey.FileName;
                File.WriteAllBytes(path, breakstream);
            }
        }

        // Tab 2
        private void dodump(object sender, EventArgs e)
        {
            // Get Video & Keystream
            byte[] video = File.ReadAllBytes(TB_Video.Text);
            byte[] keystr = File.ReadAllBytes(TB_Key.Text);
            // Idiot Checking
            if (video.Length != 28256)
            {
                MessageBox.Show("Video Filesize Incorrect.\r\nCurrent Size: " + video.Length + "\r\nRequired Length: 28256", "Error");
                RTB.Text = "Fix Video size -- load the correct file please :(";
            }
            else if (keystr.Length != (6 * 260))
            {
                MessageBox.Show("Key Filesize Incorrect.\r\nCurrent Size: " + keystr.Length + "\r\nRequired Length: " + (6 * 260), "Error");
                RTB.Text = "Fix Key size -- load the correct file please :(";
            }
            else // Do Trick
            {
                int offset = getoffset(CB_TeamSelect2); // Begin EKX Data

                // Initialize
                int valid = 0;
                int errors = 0;
                RTB.Text = "";
                modestring = "";
                result = "";

                // Get EKXes
                byte[,] ekxfiles = new Byte[6, 260];
                for (int i = 0; i < 260 * 6; i++)
                {
                    ekxfiles[i / 260, i % 260] = (byte)(video[i + offset] ^ keystr[i]);
                }

                // Get PKXes
                byte[,] pkxfiles = new Byte[6, 260];
                for (int i = 0; i < 6; i++)
                {
                    byte[] sekx = new Byte[260];
                    for (int j = 0; j < 260; j++)
                    {
                        sekx[j] = ekxfiles[i, j];
                    }
                    byte[] pkx = decryptarray(sekx);
                    for (int j = 0; j < 260; j++)
                    {
                        pkxfiles[i, j] = pkx[j];
                    }

                    // Populate Results



                    // Corruption Check
                    uint checksum = getchecksum(pkx);
                    uint actualsum = (uint)(pkx[0x06] + pkx[0x07] * 0x100);

                    if (checksum != actualsum)
                    {
                        // Get Encrypted Zeroes
                        byte[] ezeros = encryptarray(new Byte[260]);

                        for (int a = 0; a < 260; a++)
                        {
                            pkx[a] = (byte)(pkx[a] ^ ezeros[a]);
                        }

                        checksum = getchecksum(pkx);
                        if (checksum != actualsum)
                        {
                            for (int a = 0; a < 260; a++)
                            {
                                pkx[a] = (byte)(pkx[a] ^ ezeros[a]);
                            }
                            errors++;
                            result += "\r\nError @" + (i + 1);
                            if (CB_Mode.Text == "Files")
                            {
                                string format = ".pk6";
                                string path = TB_Path.Text + "\\" + (i + 1) + format;
                                File.WriteAllBytes(path, pkx);
                                format = ".ek6";
                                path = TB_Path.Text + "\\" + (i + 1) + format;
                                File.WriteAllBytes(path, sekx);
                            }
                            continue;
                        }
                        else
                        {
                            for (int a = 0; a<260; a++)
                            {
                                keystr[i * 260 + a] = (byte)(keystr[i * 260 + a] ^ ezeros[a]);
                            }
                            File.WriteAllBytes(TB_Key.Text, keystr);
                        }
                    }

                    // Get PID, ShinyValue and Species Name
                    uint PID = BitConverter.ToUInt32(pkx, 0x18);
                    uint ShinyValue = (((PID & 0xFFFF) ^ (PID >> 16)) >> 4);
                    int species = BitConverter.ToInt16(pkx, 0x08);

                    if (species != 0)
                    {
                        valid++;
                        string specname = getspecies(species);

                        if ((CB_Mode.Text == "Default") || (CB_Mode.Text == "Files"))
                        {
                            {
                                // Default
                                modestring = "Position - Name - Nature - Ability - Spread - SV";
                                string filename =
                                (i + 1)
                                + " - "
                                + specname
                                + getgender(pkx)
                                + " - "
                                + getnature(pkx)
                                + " - "
                                + getability(pkx)
                                + " - "
                                + getivs(pkx, ShinyValue);
                                result += "\r\n" + filename;
                            }
                        }
                        else if (CB_Mode.Text == "Reddit")
                        {
                            modestring = "| Position | Name | Nature | Ability | Spread | SV\r\n|:--|:--|:--|:--|:--|:--";
                            string resultline =
                                "| " + (i + 1) +
                                " | " + specname + getgender(pkx) +
                                " | " + getnature(pkx) +
                                " | " + getability(pkx) +
                                " | " + getivs2(pkx, ShinyValue) +
                                " |"
                                ;
                            result += "\r\n" + resultline;
                        }
                        else if (CB_Mode.Text == "TSV")
                        {
                            // TSV Checking Mode
                            modestring = "| Position | Species | OT | TID | TSV\r\n|:--|:--|:--|:--|:--";
                            string resultline =
                                "| " + (i + 1) +
                                " | " + specname + getgender(pkx) + // Species
                                " | " + bytes2text(pkx, 0xB0) + // OT
                                " | " + ((uint)(pkx[0x0C] + pkx[0x0D] * 0x100)).ToString("00000") + // TID
                                " | " + getTSV(pkx) +
                                " |"
                                ;
                            result += "\r\n" + resultline;
                        }

                        else if (CB_Mode.Text == ".csv")
                        {
                            modestring = "";
                            string resultline =
                                (i + 1) +
                                "," + specname + getgender(pkx) + // Species Gender
                                "," + getnature(pkx) + // Nature
                                "," + getability(pkx) + // Ability
                                "," + getball(pkx) + // Ball
                                "," + getivs3(pkx) + // IVs
                                "," + getevs(pkx) + // EVs
                                "," + getmove(pkx, 0x5A) +
                                "," + getmove(pkx, 0x5C) +
                                "," + getmove(pkx, 0x5E) +
                                "," + getmove(pkx, 0x60) +
                                "," + getmove(pkx, 0x6A) +
                                "," + getmove(pkx, 0x6C) +
                                "," + getmove(pkx, 0x6E) +
                                "," + getmove(pkx, 0x70) +
                                "," + bytes2text(pkx, 0xB0) + // OT
                                "," + ((uint)(pkx[0x0C] + pkx[0x0D] * 0x100)).ToString("00000") +
                                "," + getTSV(pkx) +
                                "," + ShinyValue.ToString("0000");
                            result += "\r\n" + resultline;
                        }

                        if (CB_Mode.Text == "Files")
                        {
                            // Do Dump
                            string format;
                            if (CHK_PK6.Checked)
                            {
                                format = ".pk6";
                                string path = TB_Path.Text + "\\" + (i + 1) + format;
                                File.WriteAllBytes(path, pkx);
                            }
                            else
                            {
                                format = ".ek6";
                                string path = TB_Path.Text + "\\" + (i + 1) + format;
                                for (int j = 0; j < 260; j++)
                                {
                                    sekx[j] = ekxfiles[i, j];
                                }
                                File.WriteAllBytes(path, sekx);
                            };
                        }
                    }
                }
                // Special Outputs
                if (CB_Mode.Text == ".csv")
                {
                    RTB.Text += "Data exported to CSV:";

                    result = "Position,Species,Nature,Ability,Ball,HP,ATK,DEF,SpA,SpD,Spe,E_HP,E_ATK,E_DEF,E_SpA,E_SpD,E_SpE,Move1,Move2,Move3,Move4,EggMove1,EggMove2,EggMove3,EggMove4,OT,TID,TSV,ESV" + result;

                    SaveFileDialog savecsv = new SaveFileDialog();
                    savecsv.Filter = "Spreadsheet|*.csv";
                    savecsv.FileName = "KeyBV.csv";
                    if (savecsv.ShowDialog() == DialogResult.OK)
                    {
                        string path = savecsv.FileName;
                        System.IO.File.WriteAllText(path, result);

                        RTB.Text += "Path: " + path + "\r\n\r\nRefer to this file to see the results.";
                    }
                    else
                    {
                        RTB.Text += "Didn't write to CSV.";
                    }
                }

                // Output To Text Box
                if (result == "")
                {
                    result = "Nothing was dumped.";
                }

                if ((errors > 0) && (valid > 0))
                {
                    MessageBox.Show("Partial Dump :(", "Alert");
                }
                else if (valid > 0)
                {
                    MessageBox.Show("Successful Dump!", "Alert");
                }
                RTB.Text = modestring + result + "\r\n";

                try { Clipboard.SetText(RTB.Text); }
                catch { };
                if (CB_Mode.Text == "Files")
                {
                    if (CHK_PK6.Checked)
                    {
                        RTB.Text += "All .pk6's dumped to:\n" + TB_Path.Text + "\r\n\r\n";
                    }
                    else { RTB.Text += "All .ek6's dumped to:\n" + TB_Path.Text + "\r\n\r\n"; }
                }
                RTB.Text += "Dumped info copied to Clipboard!\r\n";
                RTB.Text += "Total Dumped: " + valid + "\r\n";
                RTB.Text += "Empty Slots: " + (6 - valid) + "\r\n";
            }
        }
        private void changemode(object sender, EventArgs e)
        {
            // Enable File Dumping UI if Files is selected.
            bool filemode = (CB_Mode.Text == "Files");
            CHK_PK6.Visible = filemode;
            B_ChangePath.Visible = filemode;
            TB_Path.Visible = filemode;
        }
        private void changepath(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                TB_Path.Text = fbd.SelectedPath;
            }
        }

        private void changeteam(object sender, EventArgs e)
        {
            B_KS.Enabled = false;
        }
    }
}
