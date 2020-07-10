using System;
using System.Diagnostics;
using UIKit;

namespace CGImageSave
{
    public partial class ViewController : UIViewController
    {
        private readonly DemoService _service = new DemoService();

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override async void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);



            var filePath = await _service.GetVideoAsync();
            Debug.WriteLine($"Input file: {filePath}");

            var thumbnailPath = await _service.CreateThumbnailAsync(filePath);
            Debug.WriteLine($"Output image: {thumbnailPath}");
        }
    }
}