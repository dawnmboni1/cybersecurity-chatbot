using System;
using System.Collections.Generic;
using System.Media;

namespace CybersecurityAwarenessBot
{
    class Program
    {
        // Keyword to response mapping
        static Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
        {
            { "password", new List<string> {
                "Use strong, unique passwords for each account.",
                "Avoid using personal details in your passwords.",
                "Consider using a password manager for secure storage."
            }},
            { "phishing", new List<string> {
                "Don't click suspicious links in emails.",
                "Be cautious of urgent emails asking for login details.",
                "Verify sender addresses before trusting any email."
            }},
            { "privacy", new List<string> {
                "Limit the personal information you share online.",
                "Adjust your social media privacy settings.",
                "Avoid using public Wi-Fi for sensitive transactions."
            }},
        };

        // Memory for personalization
        static string userName = "";
        static string userInterest = "";

        static void Main(string[] args)
        {
            DisplayImageLogo();   // show PNG logo first
            PlayGreeting();       // play greeting audio
            GreetUser();          // ask for user's name
            RunChat();            // start chatbot loop
        }

        static void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("greeting.wav");
                player.PlaySync(); // Play audio greeting
            }
            catch
            {
                Console.WriteLine("Could not play greeting audio. Make sure 'greeting.wav' exists.");
            }
        }

        static void DisplayImageLogo()
        {
            string imagePath = "Welcome.png"; // your logo image
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(imagePath) { UseShellExecute = true });
            }
            catch
            {
                Console.WriteLine("Could not open the image. Make sure 'Welcome.png' is in the output folder.");
            }
        }

        static void GreetUser()
        {
            Console.Write("Welcome! Please enter your name: ");
            userName = Console.ReadLine();
            Console.WriteLine($"Nice to meet you, {userName}!");
        }

        static void RunChat()
        {
            while (true)
            {
                Console.Write("\nYou: ");
                string input = Console.ReadLine().ToLower();

                if (input.Contains("exit") || input.Contains("quit"))
                {
                    Console.WriteLine("Chatbot: Goodbye! Stay safe online.");
                    break;
                }

                // Sentiment detection
                if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
                {
                    Console.WriteLine("Chatbot: It's okay to feel that way. Scammers can be sneaky. Let me share a helpful tip.");
                    continue;
                }

                // Store user interest
                if (input.Contains("interested in"))
                {
                    int idx = input.IndexOf("interested in") + "interested in".Length;
                    userInterest = input.Substring(idx).Trim();
                    Console.WriteLine($"Chatbot: Got it! I'll remember that you're interested in {userInterest}.");
                    continue;
                }

                // Recall interest
                if (input.Contains("remind me") && !string.IsNullOrEmpty(userInterest))
                {
                    Console.WriteLine($"Chatbot: As someone interested in {userInterest}, here’s a tip: Always double-check sender emails.");
                    continue;
                }

                // Keyword response with variation
                bool matched = false;
                string[] words = input.Split(new[] { ' ', '.', '?', ',', '!' }, StringSplitOptions.RemoveEmptyEntries);
                Random rand = new Random();

                foreach (var word in words)
                {
                    if (keywordResponses.ContainsKey(word))
                    {
                        var responses = keywordResponses[word];
                        Console.WriteLine("Chatbot: " + responses[rand.Next(responses.Count)]);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    Console.WriteLine("Chatbot: I'm not sure I understand. Try asking about phishing, passwords, or privacy.");
                }
            }
        }
    }
}
