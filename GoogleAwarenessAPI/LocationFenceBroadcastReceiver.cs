using Android.Content;
using Android.Widget;

namespace GoogleAwarenessAPI
{
    internal class LocationFenceBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Toast.MakeText(context, intent.Action, ToastLength.Long);
        }
    }
}