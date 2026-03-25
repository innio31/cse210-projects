using System;
using System.Collections.Generic;

namespace YouTubeVideos
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a list to store all videos
            List<Video> videos = new List<Video>();

            // Create Video 1
            Video video1 = new Video("C# Programming Tutorial for Beginners", "TechAcademy", 1800);
            video1.AddComment(new Comment("HannahAkinwunmi", "Great tutorial! Very easy to follow."));
            video1.AddComment(new Comment("PeculiarEdekere", "This helped me understand classes better."));
            video1.AddComment(new Comment("StellaFaramade", "When is the next part coming out?"));
            video1.AddComment(new Comment("BukolaOrokoyu", "Best C# tutorial I've found!"));
            videos.Add(video1);

            // Create Video 2
            Video video2 = new Video("Understanding Object-Oriented Programming", "CodeMaster", 2400);
            video2.AddComment(new Comment("OdunayoKoleosho", "OOP finally makes sense now!"));
            video2.AddComment(new Comment("BlessingOdion", "The abstraction explanation was perfect."));
            video2.AddComment(new Comment("OsamudiameTestimony", "Could you do a video on design patterns?"));
            videos.Add(video2);

            // Create Video 3
            Video video3 = new Video("Building Your First Web API", "DevPro", 3000);
            video3.AddComment(new Comment("AminaAbdulaziz", "Followed along and got it working!"));
            video3.AddComment(new Comment("AliceJohnson", "Very clear instructions."));
            video3.AddComment(new Comment("MarcusWilliams", "This should have more views!"));
            video3.AddComment(new Comment("AbrahamOkonkwo", "Thanks for this content!"));
            videos.Add(video3);

            // Create Video 4
            Video video4 = new Video("Debugging Techniques That Save Time", "CodeNinja", 1500);
            video4.AddComment(new Comment("JerryEze", "The breakpoint tips were game-changing!"));
            video4.AddComment(new Comment("JoshuaSelman", "Wish I knew these sooner."));
            video4.AddComment(new Comment("DavidOyedepo", "Great practical advice."));
            videos.Add(video4);

            // Display all videos and their comments
            foreach (Video video in videos)
            {
                // Convert seconds to minutes:seconds format for better readability
                int minutes = video.GetLengthInSeconds() / 60;
                int seconds = video.GetLengthInSeconds() % 60;

                Console.WriteLine($"Title: {video.GetTitle()}");
                Console.WriteLine($"Author: {video.GetAuthor()}");
                Console.WriteLine($"Length: {minutes}:{seconds:D2} ({video.GetLengthInSeconds()} seconds)");
                Console.WriteLine($"Number of Comments: {video.GetNumberOfComments()}");
                Console.WriteLine("Comments:");

                foreach (Comment comment in video.GetComments())
                {
                    Console.WriteLine($"  - {comment.GetCommenterName()}: \"{comment.GetCommentText()}\"");
                }

                Console.WriteLine(); // Blank line between videos
            }
        }
    }
}