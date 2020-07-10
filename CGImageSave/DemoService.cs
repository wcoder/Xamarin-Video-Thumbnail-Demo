using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AVFoundation;
using CoreMedia;
using Foundation;
using ImageIO;
using MobileCoreServices;

namespace CGImageSave
{
    public class DemoService
    {
        public Task<string> CreateThumbnailAsync(string videoPath)
        {
            return Task.Run(() =>
            {
                var url = NSUrl.FromFilename(videoPath);
                var asset = AVAsset.FromUrl(url);
                var assetImageGenerator = AVAssetImageGenerator.FromAsset(asset);
                assetImageGenerator.AppliesPreferredTrackTransform = true;

                var time = CMTime.FromSeconds(1, 1);

                using (var img = assetImageGenerator.CopyCGImageAtTime(time, out CMTime _, out NSError __))
                {
                    if (img == null)
                    {
                        Debug.WriteLine($"Could not find file at url: {videoPath}");
                        return string.Empty;
                    }

                    var outputPath = Path.ChangeExtension(videoPath, ".png");
                    var fileUrl = NSUrl.FromFilename(outputPath);

                    using (var imageDestination = CGImageDestination.Create(fileUrl, UTType.PNG, 1))
                    {
                        imageDestination.AddImage(img);
                        imageDestination.Close();
                    }

                    return outputPath;
                }
            });
        }


        // Sample video, only for demo:

        private const string VideoUrl = "https://filesamples.com/samples/video/mov/sample_640x360.mov";
        public async Task<string> GetVideoAsync()
        {
            var videoFilePath = Path.Combine(Path.GetTempPath(), "test.mov");

            if (!File.Exists(videoFilePath))
            {
                var httpClient = new HttpClient();
                using (var remoteStream = await httpClient.GetStreamAsync(VideoUrl))
                {
                    using (var fileStream = File.Create(videoFilePath))
                    {
                        await remoteStream.CopyToAsync(fileStream).ConfigureAwait(false);
                    }
                }
            }

            return videoFilePath;
        }
    }
}
