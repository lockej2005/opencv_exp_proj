using OpenCvSharp;
using System;

namespace opencv_proj
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load the cascade
            var faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");

            using (var capture = new VideoCapture("http://192.168.1.38:8080/video")) // Use VideoCapture("http://192.168.1.2:8080/video") for IP camera
            {
                Mat frame = new Mat();
                while (true)
                {
                    capture.Read(frame);

                    if (!frame.Empty())
                    {
                        // Convert the frame to grayscale
                        var gray = new Mat();
                        Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

                        // Detect faces
                        var faces = faceCascade.DetectMultiScale(
                            image: gray, 
                            scaleFactor: 1.1, 
                            minNeighbors: 5, 
                            flags: HaarDetectionTypes.ScaleImage, 
                            minSize: new OpenCvSharp.Size(30, 30)
                        );

                        // Draw a rectangle around the faces
                        foreach (var face in faces)
                        {
                            Cv2.Rectangle(frame, face, new Scalar(0, 255, 0), thickness: 2);
                        }
                    }

                    // Show the image in a window
                    Cv2.ImShow("Camera", frame);

                    // Wait for a key press and break if one is detected
                    if (Cv2.WaitKey(1) >= 0)
                        break;
                }
            }
        }
    }
}
