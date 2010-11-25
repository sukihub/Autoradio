namespace Autoradio.Helpers
{
    //stavy prehravaca
    public enum State
    { 
        Playing, Paused, Stopped, TurnedOff, PlaylistOn, PlaylistOff
    };

    //protoyp metody hlavnej aplikacie, ktora je volana modulom pri zmene stavu
    public delegate void StateChangedNotify(State newState);

    //interface pouzitych modulov/stranok viditelny hlavnej aplikacii
    interface IModuleInterface
    {
        void initialize(StateChangedNotify stateChanged, Playlist playlist);
        void playlistHidden();
    }

    interface ICloneable
    {
        object Clone();
    }
}