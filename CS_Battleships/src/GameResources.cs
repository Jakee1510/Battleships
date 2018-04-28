using System.Collections.Generic;
using SwinGameSDK;
public sealed class GameResources
{
    
    // Imports fonts to be used in the game
    private static void LoadFonts()
    {
        NewFont("ArialLarge", "arial.ttf", 80);
        NewFont("Courier", "cour.ttf", 14);
        NewFont("CourierSmall", "cour.ttf", 8);
        NewFont("Menu", "ffaccess.ttf", 8);
    }

    // Imports images to be used in the game
    private static void LoadImages()
    {
        //Backgrounds
        NewImage("Menu", "main_page.jpg");
        NewImage("Discovery", "discover.jpg");
        NewImage("Deploy", "deploy.jpg");
        
        //Deployment
        NewImage("LeftRightButton", "deploy_dir_button_horiz.png");
        NewImage("UpDownButton", "deploy_dir_button_vert.png");
        NewImage("SelectedShip", "deploy_button_hl.png");
        NewImage("PlayButton", "deploy_play_button.png");
        NewImage("RandomButton", "deploy_randomize_button.png");
        
        //Ships, since they're predictable, loop name finder
        int i = 0;
        for (i = 1; i <= 5; i++)
        {
            NewImage("ShipLR" + System.Convert.ToString(i), "ship_deploy_horiz_" + System.Convert.ToString(i) +".png");
            NewImage("ShipUD" + System.Convert.ToString(i), "ship_deploy_vert_" + System.Convert.ToString(i) +".png");
        }
        
        //Explosions
        NewImage("Explosion", "explosion.png");
        NewImage("Splash", "splash.png");
        
    }

    // Imports sounds to be used in the game
    private static void LoadSounds()
    {
        NewSound("Error", "error.wav");
        NewSound("Hit", "hit.wav");
        NewSound("Sink", "sink.wav");
        NewSound("Siren", "siren.wav");
        NewSound("Miss", "watershot.wav");
        NewSound("Winner", "winner.wav");
        NewSound("Lose", "lose.wav");
    }

    // Background music, to be played constantly, to the point of annoyance
    private static void LoadMusic()
    {
        NewMusic("Background", "horrordrone.mp3");
    }
    
    /// <summary>
    /// Gets a Font Loaded in the Resources
    /// </summary>
    /// <param name="font">Name of Font</param>
    /// <returns>The Font Loaded with this Name</returns>
    
    public static Font GameFont(string font)
    {
        return _Fonts[font];
    }
    
    /// <summary>
    /// Gets an Image loaded in the Resources
    /// </summary>
    /// <param name="image">Name of image</param>
    /// <returns>The image loaded with this name</returns>
    
    public static Bitmap GameImage(string image)
    {
        return _Images[image];
    }
    
    /// <summary>
    /// Gets an sound loaded in the Resources
    /// </summary>
    /// <param name="sound">Name of sound</param>
    /// <returns>The sound with this name</returns>
    
    public static SoundEffect GameSound(string sound)
    {
        return _Sounds[sound];
    }
    
    /// <summary>
    /// Gets the music loaded in the Resources
    /// </summary>
    /// <param name="music">Name of music</param>
    /// <returns>The music with this name</returns>
    
    public static Music GameMusic(string music)
    {
        return _Music[music];
    }
    
    // Fill dictionaries with the files that were imported
    private static Dictionary<string, Bitmap> _Images = new Dictionary<string, Bitmap>();
    private static Dictionary<string, Font> _Fonts = new Dictionary<string, Font>();
    private static Dictionary<string, SoundEffect> _Sounds = new Dictionary<string, SoundEffect>();
    private static Dictionary<string, Music> _Music = new Dictionary<string, Music>();

    // Allocate memory for resources, prepare game
    private static Bitmap _Background;
    private static Bitmap _Animation;
    private static Bitmap _LoaderFull;
    private static Bitmap _LoaderEmpty;
    private static Font _LoadingFont;
    private static SoundEffect _StartSound;
    
    /// <summary>
    /// The Resources Class stores all of the Games Media Resources, such as Images, Fonts
    /// Sounds, Music.
    /// </summary>
    
    public static void LoadResources()
    {
        // Set up window
        int width = 0;
        int height = 0;
        
        SwinGame.ChangeScreenSize(800, 600);
        
        // Display loading screen
        ShowLoadingScreen();
        
        ShowMessage("Loading fonts...", 0);
        LoadFonts();
        SwinGame.Delay((uint) 100);
        
        ShowMessage("Loading images...", 1);
        LoadImages();
        SwinGame.Delay((uint) 100);
        
        ShowMessage("Loading sounds...", 2);
        LoadSounds();
        SwinGame.Delay((uint) 100);
        
        ShowMessage("Loading music...", 3);
        LoadMusic();
        SwinGame.Delay((uint) 100);
        
        SwinGame.Delay((uint) 100);
        ShowMessage("Game loaded...", 5);
        SwinGame.Delay((uint) 100);
        EndLoadingScreen(width, height);
    }

    // Set up ps1-era loading screen, load resources into memory allocated above
    private static void ShowLoadingScreen()
    {
        _Background = SwinGame.LoadBitmap(SwinGame.PathToResource("SplashBack.png", ResourceKind.BitmapResource));
        SwinGame.DrawBitmap(_Background, 0, 0);
        SwinGame.RefreshScreen();
        SwinGame.ProcessEvents();
        
        _Animation = SwinGame.LoadBitmap(SwinGame.PathToResource("SwinGameAni.jpg", ResourceKind.BitmapResource));
        _LoadingFont = SwinGame.LoadFont(SwinGame.PathToResource("arial.ttf", ResourceKind.FontResource), 12);
        _StartSound = Audio.LoadSoundEffect(SwinGame.PathToResource("SwinGameStart.ogg", ResourceKind.SoundResource));
        
        _LoaderFull = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_full.png", ResourceKind.BitmapResource));
        _LoaderEmpty = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_empty.png", ResourceKind.BitmapResource));
        
        PlaySwinGameIntro();
    }

    // Prepares the background
    private static void PlaySwinGameIntro()
    {
        const int ANI_CELL_COUNT = 11;
        
        Audio.PlaySoundEffect(_StartSound);
        SwinGame.Delay((uint) 200);
        
        int i = 0;
        for (i = 0; i <= ANI_CELL_COUNT - 1; i++)
        {
            SwinGame.DrawBitmap(_Background, 0, 0);
            SwinGame.Delay((uint) 20);
            SwinGame.RefreshScreen();
            SwinGame.ProcessEvents();
        }
        
        SwinGame.Delay((uint) 1500); // Geez, how long does this need to be? Consider this to be a FIXME for issue #7

    }
    
    // Notifies user of messages, takes a string as a message to display, and an int as parameters
    // Could we refactor it down to one parameter, and just check string length? Maybe later
    private static void ShowMessage(string message, int number)
    {
        const int TX = 310;
        const int TY = 493;
        const int TW = 200;
        const int TH = 25;
        const int STEPS = 5;
        const int BG_X = 279;
        const int BG_Y = 453;
        
        int fullW;
        Rectangle toDraw = new Rectangle();
        
        fullW = System.Convert.ToInt32(260 * number / STEPS);
        SwinGame.DrawBitmap(_LoaderEmpty, BG_X, BG_Y);
        SwinGame.DrawCell(_LoaderFull, 0, BG_X, BG_Y);
        // SwinGame.DrawBitmapPart(_LoaderFull, 0, 0, fullW, 66, BG_X, BG_Y)
        
        toDraw.X = TX;
        toDraw.Y = TY;
        toDraw.Width = TW;
        toDraw.Height = TH;
        SwinGame.DrawTextLines(message, Color.White, Color.Transparent, _LoadingFont, FontAlignment.AlignCenter, toDraw);
        // SwinGame.DrawTextLines(message, Color.White, Color.Transparent, _LoadingFont, FontAlignment.AlignCenter, TX, TY, TW, TH)
        
        SwinGame.RefreshScreen();
        SwinGame.ProcessEvents();
    }

    // Done loading, once again there is a half-second delay before clearing the memory. Seems big. Another FIXME for #7
    private static void EndLoadingScreen(int width, int height)
    {
        SwinGame.ProcessEvents();
        SwinGame.Delay((uint) 500);
        SwinGame.ClearScreen();
        SwinGame.RefreshScreen();
        SwinGame.FreeFont(_LoadingFont);
        SwinGame.FreeBitmap(_Background);
        SwinGame.FreeBitmap(_Animation);
        SwinGame.FreeBitmap(_LoaderEmpty);
        SwinGame.FreeBitmap(_LoaderFull);
        Audio.FreeSoundEffect(_StartSound);
        SwinGame.ChangeScreenSize(width, height);
    }

    // Set up fonts
    private static void NewFont(string fontName, string filename, int size)
    {
        _Fonts.Add(fontName, SwinGame.LoadFont(SwinGame.PathToResource(filename, ResourceKind.FontResource), size));
    }

    // Set up solid images
    private static void NewImage(string imageName, string filename)
    {
        _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(filename, ResourceKind.BitmapResource)));
    }

    // Set up transparent images
    private static void NewTransparentColorImage(string imageName, string fileName, Color transColor)
    {
        _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, ResourceKind.BitmapResource)));
    }

    // Seriously? Are we that picky about American vs. Australian/British spelling? At the end, refactor code, with intent to factor this out. TODO
    private static void NewTransparentColourImage(string imageName, string fileName, Color transColor)
    {
        NewTransparentColorImage(imageName, fileName, transColor);
    }

    // Set up game sounds
    private static void NewSound(string soundName, string filename)
    {
        _Sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
    }

    // Set up background music
    private static void NewMusic(string musicName, string filename)
    {
        _Music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
    }

    // Free memory

    // Clear fonts from memory
    private static void FreeFonts()
    {
        Font obj = default(Font);
        foreach (Font tempLoopVar_obj in _Fonts.Values)
        {
            obj = tempLoopVar_obj;
            SwinGame.FreeFont(obj);
        }
    }

    // Clear images from memory
    private static void FreeImages()
    {
        Bitmap obj = default(Bitmap);
        foreach (Bitmap tempLoopVar_obj in _Images.Values)
        {
            obj = tempLoopVar_obj;
            SwinGame.FreeBitmap(obj);
        }
    }

    // Clear game sounds from memory
    private static void FreeSounds()
    {
        SoundEffect obj = default(SoundEffect);
        foreach (SoundEffect tempLoopVar_obj in _Sounds.Values)
        {
            obj = tempLoopVar_obj;
            Audio.FreeSoundEffect(obj);
        }
    }

    // Clear music from memory
    private static void FreeMusic()
    {
        Music obj = default(Music);
        foreach (Music tempLoopVar_obj in _Music.Values)
        {
            obj = tempLoopVar_obj;
            Audio.FreeMusic(obj);
        }
    }

    // Runs above scripts, clearing memory
    public static void FreeResources()
    {
        FreeFonts();
        FreeImages();
        FreeMusic();
        FreeSounds();
        SwinGame.ProcessEvents();
    }
}
