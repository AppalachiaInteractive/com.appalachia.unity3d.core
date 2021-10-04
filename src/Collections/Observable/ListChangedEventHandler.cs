namespace Appalachia.Core.Collections.Observable
{
    public delegate void ListChangedEventHandler<T>(
        ObservableList<T> sender,
        ListChangedEventArgs<T> e);
}
