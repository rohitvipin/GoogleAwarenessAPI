using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Awareness;
using Android.Gms.Awareness.Fence;
using Android.Gms.Common.Apis;
using Android.Gms.Extensions;
using Android.OS;

namespace GoogleAwarenessAPI
{
    [Activity(Label = "GoogleAwarenessAPI", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const double Latitude = 40, Longitude = 50, Radius = 1;
        private const string FenceReceiverAction = "FENCE_RECEIVER_ACTION";
        private const string LocationFenceKey = "locationFence";
        private GoogleApiClient _googleApiClient;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);

            _googleApiClient = await new GoogleApiClient.Builder(this)
                      .AddApi(Awareness.Api)
                      .AddConnectionCallbacks(() =>
                      {
                          //Connected Successful
                      })
                      .BuildAndConnectAsync(i => { });

            Application.Context.RegisterReceiver(new LocationFenceBroadcastReceiver(), new IntentFilter(FenceReceiverAction));

            var status = await RegisterLocationFence();

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            var status = UnRegisterLocationFence();
        }

        private async Task<Statuses> RegisterLocationFence() => await Awareness.FenceApi.UpdateFencesAsync(_googleApiClient, new FenceUpdateRequestBuilder()
                                                                        .AddFence(LocationFenceKey, LocationFence.Entering(Latitude, Longitude, Radius)
                                                                            , PendingIntent.GetBroadcast(this, 10001, new Intent(FenceReceiverAction), PendingIntentFlags.UpdateCurrent))
                                                                        .Build());

        private async Task<Statuses> UnRegisterLocationFence() => await Awareness.FenceApi.UpdateFencesAsync(_googleApiClient, new FenceUpdateRequestBuilder()
            .RemoveFence(LocationFenceKey)
            .Build());
    }
}

